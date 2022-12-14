using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using SPFile = Microsoft.SharePoint.Client.File;

namespace EEANWorks.Microsoft
{
    public class SharePointConnectionManager
    {
        public SharePointConnectionManager(string _siteUrl, string _docLibrary, string _username, string _password, string _defaultSubfolderPath = "", string _localSaveFilesPath = null)
        {
            m_siteUrl = _siteUrl;
            m_docLibrary = _docLibrary;
            DefaultFolderPath = _defaultSubfolderPath;

            SecureString securePassword = new SecureString();
            foreach (char c in _password)
            { securePassword.AppendChar(c); }

            m_onlineCredentials = new SharePointOnlineCredentials(_username, securePassword);

            LocalSaveFilesPath = _localSaveFilesPath ?? (Path.GetDirectoryName(Application.ExecutablePath) + @"\");
        }

        private readonly string m_siteUrl;
        private readonly string m_docLibrary;
        public string DefaultFolderPath { get; }

        private readonly SharePointOnlineCredentials m_onlineCredentials;

        public string LocalSaveFilesPath { get; }

        public bool CheckConnection()
        {
            try
            {
                using (ClientContext cContext = new ClientContext(m_siteUrl))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                    Folder folder = PathToFolder(DefaultFolderPath, cContext);

                    if (folder != null)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return false;
            }
        }

        public bool UploadFile(string _filePath, bool _showSuccessMessage = false)
        {
            return UploadFile(_filePath, _showSuccessMessage, DefaultFolderPath);
        }
        private bool UploadFile(string _filePath, bool _showSuccessMessage, string _subFolderPath)
        {
            try
            {
                #region Insert the data
                using (ClientContext cContext = new ClientContext(m_siteUrl))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    FileCreationInformation newFile = new FileCreationInformation();
                    byte[] fileContent = System.IO.File.ReadAllBytes(_filePath);
                    newFile.ContentStream = new MemoryStream(fileContent);
                    newFile.Url = Path.GetFileName(_filePath);
                    newFile.Overwrite = true;
                    List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                    Folder folder = PathToFolder(_subFolderPath, cContext);
                    SPFile uploadFile = folder.Files.Add(newFile);
                    cContext.Load(documentLibrary);
                    cContext.Load(uploadFile);
                    cContext.ExecuteQuery();

                    if (_showSuccessMessage)
                        MessageBox.Show("The file has been uploaded" + Environment.NewLine + "FileUrl -->" + m_siteUrl + "/" + m_docLibrary + "/" + _subFolderPath + "/" + Path.GetFileName(_filePath));
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message);
                return false;
            }
        }

        public FileInfo DownloadFile(string _fileName, string _subFolderPath = null, string _localSaveFilesPath = null)
        {
            string subFolderPath = (_subFolderPath != null) ? _subFolderPath : DefaultFolderPath;
            string localSaveFilesPath = (_localSaveFilesPath != null) ? _localSaveFilesPath : LocalSaveFilesPath;

            try
            {
                FileInfo fileInfo = null;

                #region Load the data
                using (ClientContext cContext = new ClientContext(m_siteUrl))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                    Folder clientfolder = PathToFolder(subFolderPath, cContext);

                    var files = clientfolder.Files;
                    cContext.Load(files);
                    cContext.ExecuteQuery();

                    SPFile downloadedFile = files.First(x => x.Name == _fileName);
                    cContext.Load(downloadedFile);
                    cContext.ExecuteQuery();

                    string filePath_localSaveFile = localSaveFilesPath + _fileName;

                    //Open file to copy loaded data. Create it if not exists.
                    FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
                    fs.SetLength(0);

                    //Copy loaded data to local save file.
                    ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
                    cContext.ExecuteQuery();
                    if (crStream.Value != null)
                    {
                        crStream.Value.CopyTo(fs);
                        fileInfo = new FileInfo(_fileName, filePath_localSaveFile, crStream.Value);
                    }

                    fs.Close();

                    return fileInfo;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }

        public string DownloadAndSaveFile(string _fileName)
        {
            return DownloadAndSaveFile(_fileName, DefaultFolderPath);
        }
        public string DownloadAndSaveFile(string _fileName, string _subFolderPath)
        {
            try
            {
                #region Load the data
                using (ClientContext cContext = new ClientContext(m_siteUrl))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                    Folder folder = PathToFolder(_subFolderPath, cContext);

                    var files = folder.Files;
                    cContext.Load(files);
                    cContext.ExecuteQuery();

                    SPFile downloadedFile = files.First(x => x.Name == _fileName);
                    cContext.Load(downloadedFile);
                    cContext.ExecuteQuery();

                    string filePath_localSaveFile = LocalSaveFilesPath + _fileName;

                    //Open file to copy loaded data. Create it if not exists.
                    FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
                    fs.SetLength(0);

                    //Copy loaded data to local save file.
                    ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
                    cContext.ExecuteQuery();
                    if (crStream.Value != null)
                        crStream.Value.CopyTo(fs);

                    fs.Close();

                    return filePath_localSaveFile;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }

        public List<string> DownloadAndSaveFiles(List<string> _fileNames)
        {
            return DownloadAndSaveFiles(_fileNames, DefaultFolderPath);
        }
        public List<string> DownloadAndSaveFiles(List<string> _fileNames, string _subFolderPath)
        {
            List<string> filePaths = new List<string>();

            try
            {
                #region Load the data
                using (ClientContext cContext = new ClientContext(m_siteUrl))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                    Folder folder = PathToFolder(_subFolderPath, cContext);

                    var files = folder.Files;
                    cContext.Load(files);
                    cContext.ExecuteQuery();

                    foreach (var fileName in _fileNames)
                    {
                        SPFile downloadedFile = files.First(x => x.Name == fileName);
                        cContext.Load(downloadedFile);
                        cContext.ExecuteQuery();

                        string filePath_localSaveFile = LocalSaveFilesPath + fileName;

                        //Open file to copy loaded data. Create it if not exists.
                        FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
                        fs.SetLength(0);

                        //Copy loaded data to local save file.
                        ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
                        cContext.ExecuteQuery();
                        if (crStream.Value != null)
                            crStream.Value.CopyTo(fs);

                        fs.Close();

                        filePaths.Add(filePath_localSaveFile);
                    }

                    return filePaths;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message);
                return null;
            }
        }

        public async Task<List<string>> DownloadAndSaveFiles(List<string> _fileNames, Label _lbl_loadingStatus, string _subFolderPath = "")
        {
            List<string> filePaths = new List<string>();

            try
            {
                #region Load the data
                using (ClientContext cContext = new ClientContext(m_siteUrl))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                    Folder clientfolder = PathToFolder(_subFolderPath, cContext);

                    var files = clientfolder.Files;
                    cContext.Load(files);
                    cContext.ExecuteQuery();

                    await SaveFilesAndVisualizeLoadingStatus(cContext, files, _fileNames, filePaths, _lbl_loadingStatus);

                    return filePaths;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns the folder corresponding to its relative path starting from the name of the direct child folder of the document library.
        /// A new folder is created for each sub folder's name within the path that has no corresponding folder.
        /// </summary>
        private Folder PathToFolder(string _subFolderPath, ClientContext _cContext)
        {
            try
            {
                if (_cContext == null)
                    return null;
                if (_subFolderPath == null)
                    return null;

                Web web = _cContext.Web;

                List documentLibrary = web.Lists.GetByTitle(m_docLibrary);
                Folder folder = documentLibrary.RootFolder;
                var folders = folder.Folders;
                _cContext.Load(folders);
                _cContext.ExecuteQuery();

                if (_subFolderPath != "")
                {
                    string[] subFolderTree = _subFolderPath.Split(new char[] { '/' });
                    foreach (string folderName in subFolderTree)
                    {
                        var tmp_folder = folders.FirstOrDefault(x => x.Name == folderName);
                        if (tmp_folder == null)
                        {
                            folder.Folders.Add(folderName);
                            folder.Update();
                            tmp_folder = folder.Folders.Last();
                        }

                        folder = tmp_folder;
                        folders = folder.Folders;
                        _cContext.Load(folders);
                        _cContext.ExecuteQuery();
                    }
                }

                return folder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task SaveFilesAndVisualizeLoadingStatus(ClientContext _cContext, FileCollection _files, List<string> _fileNames, List<string> _filePaths, Label _lbl_loadingStatus)
        {
            try
            {
                string numOfFiles = _fileNames.Count.ToString();

                int count = 0;
                foreach (var fileName in _fileNames)
                {
                    await Task.Delay(1);

                    SPFile downloadedFile = _files.First(x => x.Name == fileName);

                    _cContext.Load(downloadedFile);
                    _cContext.ExecuteQuery();

                    string filePath_localSaveFile = LocalSaveFilesPath + fileName;

                    //Open file to copy loaded data. Create it if not exists.
                    FileStream fs = System.IO.File.OpenWrite(filePath_localSaveFile);
                    fs.SetLength(0);

                    //Copy loaded data to local save file.
                    ClientResult<Stream> crStream = downloadedFile.OpenBinaryStream();
                    _cContext.ExecuteQuery();
                    if (crStream.Value != null)
                        crStream.Value.CopyTo(fs);

                    fs.Close();

                    _filePaths.Add(filePath_localSaveFile);

                    _lbl_loadingStatus.Text = "Loading data from server... (" + (++count).ToString() + "/" + numOfFiles + " file(s) loaded.)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
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
}
