using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    public partial class DataSelectionForm : Form
    {
        public DataSelectionForm()
        {
            InitializeComponent();
        }

        private bool m_downloadSucceeded;
        private bool m_importSucceeded;

        private string m_numOfFiles;

        private void DataSelectionForm_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;
            Dictionary<string, string> systemDataDictionary = container.SystemDataDictionary;
            if (!(Convert.ToBoolean(systemDataDictionary["LocalSaveFilesExist"]) && systemDataDictionary["LastLoginUser"] != string.Empty))
                btn_continueWorkingLocally.Enabled = false;

            m_numOfFiles = container.FileNames.Count.ToString();

            lbl_modificationCount.Text = container.NumOfModifiedFiles().ToString();
        }

        private void btn_downloadNewestData_Click(object sender, EventArgs e)
        {
            if (lbl_modificationCount.Text != "0")
            {
                DialogResult dialogResult = MessageBox.Show("Would you really like to download newest data?"
                                                              + "\n By doing so, local modifications would be lost!", "Caution!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    LoadData(true);
            }
            else
                LoadData(true);
        }

        private async Task LoadData(bool _loadOnlineData)
        {
            try
            {
                if (_loadOnlineData && !m_downloadSucceeded)
                {
                    lbl_loadingStatus.Text = "Loading data from server...";

                    await DownloadFromSharePoint();

                    if (!m_downloadSucceeded)
                    {
                        lbl_loadingStatus.Text = "Failed to load data from server. Please check your internet connection and try again.";
                        return;
                    }

                    DataContainer.Instance.ResetAllFileModificationStatus();
                }

                await UpdateImportStatusText(0);

                await ImportFromLocalCSV();

                if (!m_importSucceeded)
                {
                    lbl_loadingStatus.Text = "Failed to import local data into application. Please try again.";
                    return;
                }

                lbl_loadingStatus.Text = "Loading completed!";

                DataContainer container = DataContainer.Instance;

                container.SystemDataDictionary["LocalSaveFilesExist"] = (true).ToString();
                if (!DocumentLoader.AddSystemDataToCSV())
                    MessageBox.Show("Failed to change internal flag.");

                await Task.Delay(1000);

                if (_loadOnlineData)
                    this.LogAndSwitchTo(new LoginForm());
                else
                {
                    container.CurrentUser = container.FacultyList.FirstOrDefault(x => x.Username == container.SystemDataDictionary["LastLoginUser"]);
                    if (container.CurrentUser == null)
                    {
                        MessageBox.Show("Unidentified user logged in last time. Please try loading newest data.");
                        return;
                    }

                    this.LogAndSwitchTo(new CourseAndStudentInformationMainForm());
                }
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
                if (!DocumentLoader.ImportMajorListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportYearDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportTermDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportSemesterDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportExamTypeDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportStudentListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportPermissionsTypeDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportFacultyListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportValueTypeDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportBaseCriterionDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationCriterionDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationCriterionWeightDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationCriteria_CriterionWeightMappingListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationCriteriaDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationCriteriaWeightDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationCriteriaSetDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportColorDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportValueColorDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportEvaluationColorSet_ValueColorMappingListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportEvaluationColorSetDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationCriterionDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationCriteria_CriterionMappingListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationCriteriaDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationCriteriaSetDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportFullEvaluationCriteriaDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationSet_EvaluationMappingListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationSetDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQuantitativeEvaluationSetCollectionDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationSet_EvaluationMappingListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationSetDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportQualitativeEvaluationSetCollectionDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportFullEvaluationDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportStudentInfoDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportCourseCategoryDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportCourseBaseListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportCourseDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportCourseGroup_StudentInfoMappingListFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportCourseGroupDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportNonInstitutionalExam_InternalDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportColumnRangeDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);
                if (!DocumentLoader.ImportEvaluationFileCriterionColumnsRangeSetDictionaryFromCSV()) return; else await UpdateImportStatusText(++count);

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

        private void btn_continueWorkingLocally_Click(object sender, EventArgs e)
        {
            LoadData(false);
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
