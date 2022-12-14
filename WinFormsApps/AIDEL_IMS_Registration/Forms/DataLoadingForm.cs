using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    public partial class DataLoadingForm : Form
    {
        public DataLoadingForm()
        {
            InitializeComponent();
        }

        private bool m_downloadSucceeded;
        private bool m_importSucceeded;

        private string m_numOfFiles;

        private void DataLoadingForm_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;
            m_numOfFiles = container.FileNames.Count.ToString();

            container.ResetAllFileModificationStatus();
            LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (!m_downloadSucceeded)
                {
                    lbl_loadingStatus.Text = "Loading data from server...";

                    await DownloadFromSharePoint();

                    if (!m_downloadSucceeded)
                    {
                        lbl_loadingStatus.Text = "Failed to load data from server. Please check your internet connection and try again.";
                        return;
                    }
                }

                await UpdateImportStatusText(0);

                await ImportFromLocalCSV();

                if (!m_importSucceeded)
                {
                    lbl_loadingStatus.Text = "Failed to import local data into application. Please try again.";
                    return;
                }

                DataContainer container = DataContainer.Instance;

                container.SystemDataDictionary["LocalSaveFilesExist"] = (true).ToString();
                if (!DocumentLoader.AddSystemDataToCSV())
                {
                    MessageBox.Show("Failed to change internal flag.");
                    return;
                }

                if (!(await LoadFormsData()))
                {
                    MessageBox.Show("Failed to load Forms' data.");
                    return;
                }

                lbl_loadingStatus.Text = "Loading completed!";

                await Task.Delay(300);

                this.LogAndSwitchTo(new RegistrationInfoForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Data:" + ex.Message);
            }
        }

        private async Task DownloadFromSharePoint()
        {
            m_downloadSucceeded = false;

            string subFolderPath = DataContainer.Instance.SharePointConnectionManager.DefaultFolderPath;

            try
            {
                // Download CSV files from SharePoint and create/overwrite local save files

                List<string> fileNames = DataContainer.Instance.FileNames;

                List<string> filePaths = await DataContainer.Instance.SharePointConnectionManager.DownloadAndSaveFiles(fileNames, lbl_loadingStatus, subFolderPath);
                if (filePaths.Count != fileNames.Count) // If not all download succeeded
                    return;

                m_downloadSucceeded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Downloading Data:" + ex.Message);
            }
        }

        private async Task ImportFromLocalCSV()
        {
            m_importSucceeded = false;

            try
            {
                int count = 0;
                // Import data into DataContainer from the local save files
                if (!DocumentLoader.ImportNameDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportSurnameDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportFullNameDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportYearDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportTermDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportSemesterDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportExamTypeDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportStudentListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportNonIberoCommunityMemberListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportNonInstitutionalExam_InternalDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportRegistrationStatusDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportRegistrationInfoListFromCSVs()) return; else await UpdateImportStatusText(++count);

                m_importSucceeded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Importing Data:" + ex.Message);
            }
        }

        private async Task UpdateImportStatusText(int _count)
        {
            await Task.Delay(1);
            lbl_loadingStatus.Text = "Importing data into application... (" + _count.ToString() + "/" + m_numOfFiles + " file(s) imported.)";
        }

        private async Task<bool> LoadFormsData()
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string localSaveFilesPath_internal = CoreValues.LocalSaveFilesPath + @"Excel\";
                string localSaveFilesPath_external = CoreValues.LocalSaveFilesPath + @"Excel\External\";

                var fileInfo_internal = container.SharePointConnectionManager.DownloadFile("IdsAndPdf.xlsx", null, localSaveFilesPath_internal);
                var fileInfo_external = await container.GoogleDriveConnectionManager.DownloadExportFile("IdsAndPdf.xlsx", Google.eResultMimeType.XLSX, localSaveFilesPath_external);
                if (fileInfo_internal != null && fileInfo_external != null)
                {
                    if (DocumentLoader.LoadRegistrationInfoFromExcel(fileInfo_internal.Path, fileInfo_external.Path, default))
                    {
                        if (container.SharePointConnectionManager.UploadAllModifiedFiles())
                        {
                            // Apply Payment Evidence changes
                            {
                                var fileInfo2_internal = container.SharePointConnectionManager.DownloadFile("Voucher.xlsx", null, localSaveFilesPath_internal);
                                var fileInfo2_external = await container.GoogleDriveConnectionManager.DownloadExportFile("Voucher.xlsx", Google.eResultMimeType.XLSX, localSaveFilesPath_external);
                                if (fileInfo2_internal != null && fileInfo2_external != null)
                                {
                                    if (DocumentLoader.LoadPaymentEvidencesFromExcel(fileInfo2_internal.Path, fileInfo2_external.Path))
                                    {
                                        if (container.SharePointConnectionManager.UploadAllModifiedFiles())
                                        {
                                            // Apply ETS form changes
                                            {
                                                var fileInfo3_internal = container.SharePointConnectionManager.DownloadFile("TOEFL_ITP_ON_CAMPUS_SIF.xlsx", null, localSaveFilesPath_internal);
                                                //var fileInfo3_external = await container.GoogleDriveConnectionManager.DownloadExportFile("TOEFL_ITP_ON_CAMPUS_SIF.xlsx", Google.eMimeType.Sheets, Google.eResultMimeType.XLSX, localSaveFilesPath_external);
                                                if (fileInfo3_internal != null)
                                                {
                                                    if (DocumentLoader.LoadStudentInformation_InternalFromExcel(fileInfo3_internal.Path))
                                                    {
                                                        if (container.SharePointConnectionManager.UploadAllModifiedFiles())
                                                            return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Failed to sync data!");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to sync data! " + ex.Message);
                return false;
            }
        }
    }
}
