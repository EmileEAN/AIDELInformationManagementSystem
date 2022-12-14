using EEANWorks.Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data
{
    public class CustomSharePointConnectionManager : SharePointConnectionManager
    {
        public CustomSharePointConnectionManager(string _siteUrl, string _docLibrary, string _defaultSubFolderPath, string _username, string _password, string _localSaveFilesPath = null) : base(_siteUrl, _docLibrary, _username, _password, _defaultSubFolderPath, _localSaveFilesPath)
        {
        }

        public bool UploadAllModifiedFiles()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                string trueString = true.ToString();
                string baseString = "CSVModified_";
                int baseStringLength = baseString.Length;
                var fileNameIds = container.SystemDataDictionary.Where(x => x.Key.Contains(baseString) && x.Value == trueString).Select(x => Convert.ToInt32(x.Key.Substring(baseStringLength, x.Key.Length - baseStringLength)));
                foreach (var fileNameId in fileNameIds)
                {
                    string fileName = container.FileNames[fileNameId - 1];
                    string filePath = LocalSaveFilesPath + fileName;
                    UploadFile(filePath);
                }

                //MessageBox.Show(container.NumOfModifiedFiles().ToString() + " files have been updated in the server!");

                container.ResetAllFileModificationStatus();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
