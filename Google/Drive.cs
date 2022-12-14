using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using static Google.Apis.Drive.v3.FilesResource;

namespace EEANWorks.Google
{
    public class GoogleDriveConnectionManager
    {
        private static readonly string m_pathToServiceAccountKeyFile = Path.GetDirectoryName(Application.ExecutablePath) + @"\client_secret.json";

        private static readonly Dictionary<eMimeType, string> mimeTypeTranslations = new Dictionary<eMimeType, string> 
        {
            { eMimeType.Audio, "application/vnd.google-apps.audio" },
            { eMimeType.Document, "application/vnd.google-apps.document" },
            { eMimeType.ThirdPartyShortcut, "application/vnd.google-apps.drive-sdk" },
            { eMimeType.Drawing, "application/vnd.google-apps.drawing" },
            { eMimeType.DriveFile, "application/vnd.google-apps.file" },
            { eMimeType.DriveFolder, "application/vnd.google-apps.folder" },
            { eMimeType.Forms, "application/vnd.google-apps.form" },
            { eMimeType.FusionTables, "application/vnd.google-apps.fusiontable" },
            { eMimeType.Jamboard, "application/vnd.google-apps.jam" },
            { eMimeType.MyMaps, "application/vnd.google-apps.map" },
            { eMimeType.Photo, "application/vnd.google-apps.photo" },
            { eMimeType.Slides, "application/vnd.google-apps.presentation" },
            { eMimeType.AppsScripts, "application/vnd.google-apps.script" },
            { eMimeType.Shortcut, "application/vnd.google-apps.shortcut" },
            { eMimeType.Sites, "application/vnd.google-apps.site" },
            { eMimeType.Sheets, "application/vnd.google-apps.spreadsheet" },
            { eMimeType.Video, "application/vnd.google-apps.unknown" },

            { eMimeType.Unknown, "application/vnd.google-apps.video" }
        };

        private static readonly Dictionary<eResultMimeType, string> resultMimeTypeTranslations = new Dictionary<eResultMimeType, string>
        {

            { eResultMimeType.XLSX, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },

        };

        public GoogleDriveConnectionManager(string _applicationName, string _targetDirectoryId, string _localSaveFilesPath = null)
        {
            m_applicationName = _applicationName;

            m_targetDirectoryId = _targetDirectoryId;

            LocalSaveFilesPath = _localSaveFilesPath ?? (Path.GetDirectoryName(Application.ExecutablePath) + @"\");

            m_onlineCredentials = GetCredentials();
        }

        private static string m_applicationName;

        private readonly UserCredential m_onlineCredentials;

        private readonly string m_targetDirectoryId;

        public string LocalSaveFilesPath { get; }

        private UserCredential GetCredentials()
        {

            // Credential storage location
            var tokenStorage = new FileDataStore(LocalSaveFilesPath, true);

            UserCredential credentials;

            using (var stream = new FileStream(m_pathToServiceAccountKeyFile, FileMode.Open, FileAccess.Read))
            {
                // Requesting Authentication or loading previously stored authentication for userName
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.ScopeConstants.DriveReadonly },
                    "username",
                    CancellationToken.None,
                    tokenStorage
                    ).Result;
            }

            return credentials;
        }

        //public bool CheckConnection()
        //{
        //    try
        //    {
        //        using (ClientContext cContext = new ClientContext(m_siteUrl))
        //        {
        //            cContext.Credentials = m_onlineCredentials;
        //            Web web = cContext.Web;

        //            List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
        //            Folder folder = PathToFolder(DefaultFolderPath, cContext);

        //            if (folder != null)
        //                return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception occurred : " + ex.Message
        //            + "\nPlease check your internet connection and try again.");
        //        return false;
        //    }
        //}

        //public bool UploadFile(string _filePath, bool _showSuccessMessage = false)
        //{
        //    return UploadFile(_filePath, _showSuccessMessage, DefaultFolderPath);
        //}
        //private bool UploadFile(string _filePath, bool _showSuccessMessage, string _subFolderPath)
        //{
        //    try
        //    {
        //        #region Insert the data
        //        using (ClientContext cContext = new ClientContext(m_siteUrl))
        //        {
        //            cContext.Credentials = m_onlineCredentials;
        //            Web web = cContext.Web;

        //            FileCreationInformation newFile = new FileCreationInformation();
        //            byte[] fileContent = System.IO.File.ReadAllBytes(_filePath);
        //            newFile.ContentStream = new MemoryStream(fileContent);
        //            newFile.Url = Path.GetFileName(_filePath);
        //            newFile.Overwrite = true;
        //            List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
        //            Folder folder = PathToFolder(_subFolderPath, cContext);
        //            SPFile uploadFile = folder.Files.Add(newFile);
        //            cContext.Load(documentLibrary);
        //            cContext.Load(uploadFile);
        //            cContext.ExecuteQuery();

        //            if (_showSuccessMessage)
        //                MessageBox.Show("The file has been uploaded" + Environment.NewLine + "FileUrl -->" + m_siteUrl + "/" + m_docLibrary + "/" + _subFolderPath + "/" + Path.GetFileName(_filePath));
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception occurred : " + ex.Message);
        //        return false;
        //    }
        //}

        public async Task<FileInfo> DownloadFile(string _fileName, string _fileId, string _localSaveFilesPath = null)
        {
            string localSaveFilesPath = (_localSaveFilesPath != null) ? _localSaveFilesPath : LocalSaveFilesPath;

            try
            {
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = m_onlineCredentials,
                });

                string filePath = localSaveFilesPath + _fileName;

                return await DownloadFile(service, _fileName, _fileId, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }
        public async Task<FileInfo> DownloadFile(DriveService _service, string _fileName, string _fileId, string _filePath)
        {
            FileInfo result = null;

            var getRequest = _service.Files.Get(_fileId);

            using (FileStream fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
            {
                var downloadProgress = getRequest.DownloadWithStatus(fileStream);
                switch (downloadProgress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(downloadProgress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Google Drive - Download complete.");
                            result = new FileInfo(_fileName, _filePath, fileStream);
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Google Drive - Download failed.");
                            break;
                        }
                }
            }

            return result;
        }

        public async Task<FileInfo> DownloadFile(string _fileId, string _localSaveFilesPath = null)
        {
            string localSaveFilesPath = (_localSaveFilesPath != null) ? _localSaveFilesPath : LocalSaveFilesPath;

            try
            {
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = m_onlineCredentials,
                });

                var getRequest = service.Files.Get(_fileId);
                var requestResult = await getRequest.ExecuteAsync();
                string fileName = requestResult.Name;

                string filePath = localSaveFilesPath + fileName;

                return await DownloadFile(service, fileName, _fileId, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }
        public async Task<FileInfo> DownloadFile(GetRequest _getRequest, string _fileName, string _fileId, string _filePath)
        {
            FileInfo result = null;

            using (FileStream fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
            {
                var downloadProgress = _getRequest.DownloadWithStatus(fileStream);
                switch (downloadProgress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(downloadProgress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Google Drive - Download complete.");
                            result = new FileInfo(_fileName, _filePath, fileStream);
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Google Drive - Download failed.");
                            break;
                        }
                }
            }

            return result;
        }

        public async Task<FileInfo> DownloadExportFile(string _fileName, eResultMimeType _resultMimeType, string _localSaveFilesPath = null)
        {
            string localSaveFilesPath = (_localSaveFilesPath != null) ? _localSaveFilesPath : LocalSaveFilesPath;

            try
            {
                FileInfo fileInfo = null;

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = m_onlineCredentials,
                });

                var request = service.Files.List();
                var results = await request.ExecuteAsync();

                var file = results.Files.FirstOrDefault(x => x.Name == Path.GetFileNameWithoutExtension(_fileName));
                if (file != null)
                {
                    string filePath = localSaveFilesPath + _fileName;
                    fileInfo = await DownloadExportFile(service, _fileName, file.Id, _resultMimeType, filePath);
                }

                return fileInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }
        public async Task<FileInfo> DownloadExportFile(string _fileName, string _fileId, eResultMimeType _resultMimeType, string _localSaveFilesPath = null)
        {
            string localSaveFilesPath = (_localSaveFilesPath != null) ? _localSaveFilesPath : LocalSaveFilesPath;
            string filePath = localSaveFilesPath + _fileName;

            try
            {
                FileInfo fileInfo = null;

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = m_onlineCredentials,
                });

                return await DownloadExportFile(service, _fileName, _fileId, _resultMimeType, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }
        public async Task<FileInfo> DownloadExportFile(DriveService _service, string _fileName, string _fileId, eResultMimeType _resultMimeType, string _filePath)
        {
            FileInfo result = null;

            var getRequest = _service.Files.Export(_fileId, resultMimeTypeTranslations[_resultMimeType]);

            using (FileStream fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
            {
                var downloadProgress = getRequest.DownloadWithStatus(fileStream);
                switch (downloadProgress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(downloadProgress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Google Drive - Download complete.");
                            result = new FileInfo(_fileName, _filePath, fileStream);
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Google Drive - Download failed.");
                            break;
                        }
                }
            }

            return result;
        }

        //public string DownloadAndSaveFile(string _fileName)
        //{
        //    return DownloadAndSaveFile(_fileName, DefaultFolderPath);
        //}
        //public string DownloadAndSaveFile(string _fileName, string _subFolderPath)
        //{
        //    try
        //    {
        //        #region Load the data
        //        using (ClientContext cContext = new ClientContext(m_siteUrl))
        //        {
        //            cContext.Credentials = m_onlineCredentials;
        //            Web web = cContext.Web;

        //            List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
        //            Folder folder = PathToFolder(_subFolderPath, cContext);

        //            var files = folder.Files;
        //            cContext.Load(files);
        //            cContext.ExecuteQuery();

        //            SPFile downloadedFile = files.First(x => x.Name == _fileName);
        //            cContext.Load(downloadedFile);
        //            cContext.ExecuteQuery();

        //            string filePath_localSaveFile = LocalSaveFilesPath + _fileName;

        //            //Open file to copy loaded data. Create it if not exists.
        //            FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
        //            fs.SetLength(0);

        //            //Copy loaded data to local save file.
        //            ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
        //            cContext.ExecuteQuery();
        //            if (crStream.Value != null)
        //                crStream.Value.CopyTo(fs);

        //            fs.Close();

        //            return filePath_localSaveFile;
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception occurred : " + ex.Message
        //            + "\nPlease check your internet connection and try again.");
        //        return null;
        //    }
        //}

        //public List<string> DownloadAndSaveFiles(List<string> _fileNames)
        //{
        //    return DownloadAndSaveFiles(_fileNames, DefaultFolderPath);
        //}
        //public List<string> DownloadAndSaveFiles(List<string> _fileNames, string _subFolderPath)
        //{
        //    List<string> filePaths = new List<string>();

        //    try
        //    {
        //        #region Load the data
        //        using (ClientContext cContext = new ClientContext(m_siteUrl))
        //        {
        //            cContext.Credentials = m_onlineCredentials;
        //            Web web = cContext.Web;

        //            List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
        //            Folder folder = PathToFolder(_subFolderPath, cContext);

        //            var files = folder.Files;
        //            cContext.Load(files);
        //            cContext.ExecuteQuery();

        //            foreach (var fileName in _fileNames)
        //            {
        //                SPFile downloadedFile = files.First(x => x.Name == fileName);
        //                cContext.Load(downloadedFile);
        //                cContext.ExecuteQuery();

        //                string filePath_localSaveFile = LocalSaveFilesPath + fileName;

        //                //Open file to copy loaded data. Create it if not exists.
        //                FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
        //                fs.SetLength(0);

        //                //Copy loaded data to local save file.
        //                ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
        //                cContext.ExecuteQuery();
        //                if (crStream.Value != null)
        //                    crStream.Value.CopyTo(fs);

        //                fs.Close();

        //                filePaths.Add(filePath_localSaveFile);
        //            }

        //            return filePaths;
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception occurred : " + ex.Message);
        //        return null;
        //    }
        //}

        //    public async Task<List<string>> DownloadAndSaveFiles(List<string> _fileNames, Label _lbl_loadingStatus, string _subFolderPath = "")
        //    {
        //        List<string> filePaths = new List<string>();

        //        try
        //        {
        //            #region Load the data
        //            using (ClientContext cContext = new ClientContext(m_siteUrl))
        //            {
        //                cContext.Credentials = m_onlineCredentials;
        //                Web web = cContext.Web;

        //                List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
        //                Folder clientfolder = PathToFolder(_subFolderPath, cContext);

        //                var files = clientfolder.Files;
        //                cContext.Load(files);
        //                cContext.ExecuteQuery();

        //                await SaveFilesAndVisualizeLoadingStatus(cContext, files, _fileNames, filePaths, _lbl_loadingStatus);

        //                return filePaths;
        //            }
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Exception occurred : " + ex.Message);
        //            return null;
        //        }
        //    }

        //    /// <summary>
        //    /// Returns the folder corresponding to its relative path starting from the name of the direct child folder of the document library.
        //    /// A new folder is created for each sub folder's name within the path that has no corresponding folder.
        //    /// </summary>
        //    private Folder PathToFolder(string _subFolderPath, ClientContext _cContext)
        //    {
        //        try
        //        {
        //            if (_cContext == null)
        //                return null;
        //            if (_subFolderPath == null)
        //                return null;

        //            Web web = _cContext.Web;

        //            List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
        //            Folder folder = documentLibrary.RootFolder;
        //            var folders = folder.Folders;
        //            _cContext.Load(folders);
        //            _cContext.ExecuteQuery();

        //            if (_subFolderPath != "")
        //            {
        //                string[] subFolderTree = _subFolderPath.Split(new char[] { '/' });
        //                foreach (string folderName in subFolderTree)
        //                {
        //                    var tmp_folder = folders.FirstOrDefault(x => x.Name == folderName);
        //                    if (tmp_folder == null)
        //                    {
        //                        folder.Folders.Add(folderName);
        //                        folder.Update();
        //                        tmp_folder = folder.Folders.Last();
        //                    }

        //                    folder = tmp_folder;
        //                    folders = folder.Folders;
        //                    _cContext.Load(folders);
        //                    _cContext.ExecuteQuery();
        //                }
        //            }

        //            return folder;
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }

        //    private async Task SaveFilesAndVisualizeLoadingStatus(ClientContext _cContext, FileCollection _files, List<string> _fileNames, List<string> _filePaths, Label _lbl_loadingStatus)
        //    {
        //        try
        //        {
        //            string numOfFiles = _fileNames.Count.ToString();

        //            int count = 0;
        //            foreach (var fileName in _fileNames)
        //            {
        //                await Task.Delay(1);

        //                SPFile downloadedFile = _files.First(x => x.Name == fileName);

        //                _cContext.Load(downloadedFile);
        //                _cContext.ExecuteQuery();

        //                string filePath_localSaveFile = LocalSaveFilesPath + fileName;

        //                //Open file to copy loaded data. Create it if not exists.
        //                FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
        //                fs.SetLength(0);

        //                //Copy loaded data to local save file.
        //                ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
        //                _cContext.ExecuteQuery();
        //                if (crStream.Value != null)
        //                    crStream.Value.CopyTo(fs);

        //                fs.Close();

        //                _filePaths.Add(filePath_localSaveFile);

        //                _lbl_loadingStatus.Text = "Loading data from server... (" + (++count).ToString() + "/" + numOfFiles + " file(s) loaded.)";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
    }

    public class FileInfo
    {
        public FileInfo(string _name, string _path, Stream _stream)
        {
            Name = _name;
            Path = _path;
            Stream = _stream;
        }

        public string Name { get; }
        public string Path { get; }
        public Stream Stream { get; }
    }

    public enum eMimeType
    {
        Audio,
        Document,
        ThirdPartyShortcut,
        Drawing,
        DriveFile,
        DriveFolder,
        Forms,
        FusionTables,
        Jamboard,
        MyMaps,
        Photo,
        Slides,
        AppsScripts,
        Shortcut,
        Sites,
        Sheets,
        Video,

        Unknown
    }

    public enum eResultMimeType
    {
        XLSX
    }
}
