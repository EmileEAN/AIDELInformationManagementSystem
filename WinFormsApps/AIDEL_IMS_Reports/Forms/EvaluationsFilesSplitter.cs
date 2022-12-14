using EEANWorks;
using EEANWorks.Microsoft;
using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDEL_IMS_Reports;
using EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Forms
{
    public partial class EvaluationsFilesSplitter : Form
    {
        public EvaluationsFilesSplitter()
        {
            InitializeComponent();
        }

        private void EvaluationsFilesSplitter_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;

            lbl_status.Text = "";

            foreach (var term in container.TermDictionary.Values)
            {
                cmbBx_term.Items.Add(term.ToString());
                cmbBx_term.SelectedIndex = 0;
            }

            numUpDwn_year.Value = DateTime.Now.Year;

            foreach (var columnSet in container.EvaluationFileCriterionColumnsRangeSetDictionary.Where(x => !x.Value.Name.Contains("270")).Reverse())
            {
                cmbBx_columnSet_general.Items.Add(columnSet.Value.Name);
                cmbBx_columnSet_general.SelectedIndex = 0;
            }

            foreach (var columnSet in container.EvaluationFileCriterionColumnsRangeSetDictionary.Where(x => x.Value.Name.Contains("270")).Reverse())
            {
                cmbBx_columnSet_270.Items.Add(columnSet.Value.Name);
                cmbBx_columnSet_270.SelectedIndex = 0;
            }
        }

        private void btn_split_Click(object sender, EventArgs e)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                string termString = cmbBx_term.SelectedItem.ToString();
                eTerm term = termString.ToCorrespondingEnumValue<eTerm>();
                int year = Convert.ToInt32(numUpDwn_year.Value);


                lbl_status.DynamicUpdate("Verificando existencia de directorios...");

                string sharePointFolderPath_base = @"Evaluation Files/" + TermToString(term) + year.ToString();
                string localSaveFolderPath_base = CoreValues.LocalSaveFilesPath + @"TmpEvaluationFiles\" + TermToString(term) + year.ToString();
                if (!(container.SharePointConnectionManager_EvaluationFiles.FolderExists(sharePointFolderPath_base)))
                {
                    MessageBox.Show("Los alistados para el semestre indicado no existen!");
                    return;
                }

                // Split generated category files for each faculty
                {
                    string sharePointSaveFolderPath_faculties = sharePointFolderPath_base + @"/Faculties";

                    string localSaveFolderPath_faculties = localSaveFolderPath_base + @"\Faculties\";
                    lbl_status.DynamicUpdate("Generando directorio para archivos de profesores...");
                    if (!Directory.Exists(localSaveFolderPath_faculties))
                        Directory.CreateDirectory(localSaveFolderPath_faculties);

                    lbl_status.DynamicUpdate("Descargando archivo(s) base...");
                    List<string> filePaths_base = container.SharePointConnectionManager_EvaluationFiles.DownloadAndSaveAllFiles(sharePointFolderPath_base, localSaveFolderPath_base);
                    string englishCategoryString = CourseCategoryToString(eCourseCategory.English);
                    foreach (string filePath in filePaths_base)
                    {
                        lbl_status.DynamicUpdate("Leyendo datos del archivo: " + Path.GetFileName(filePath));

                        if (filePath.Contains(englishCategoryString))
                            SplitEnglishEvaluationFile(new ExcelPackage(new System.IO.FileInfo(filePath)), localSaveFolderPath_faculties, sharePointSaveFolderPath_faculties);
                        else
                            SplitNonEnglishEvaluationFile(new ExcelPackage(new System.IO.FileInfo(filePath)), localSaveFolderPath_faculties, sharePointSaveFolderPath_faculties);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SplitNonEnglishEvaluationFile(ExcelPackage _document, string _localSaveFolderPath, string _sharePointSaveFolderPath)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string dataFileName = Path.GetFileNameWithoutExtension(_document.File.Name);
                string dataFileFullName = _document.File.FullName;
                Dictionary<string, ExcelPackage> facultyFilesDictionary = new Dictionary<string, ExcelPackage>();
                var worksheets = _document.Workbook.Worksheets;

                // Load data from the qualitative evaluation sheet from Excel file. Initialize all new worksheets here.
                {
                    // Get the Worksheet instance.
                    ExcelWorksheet worksheet = worksheets[1];

                    // Loop through the Worksheet rows.
                    var columnCount = worksheet.Dimension.Columns;
                    var rowCount = worksheet.Dimension.Rows;
                    int headerRowsCount_qual = 5;
                    // Ignore the first 5 rows, which do not include evaluation data information.
                    for (int rowNum = headerRowsCount_qual + 1; rowNum <= rowCount; rowNum++)
                    {
                        string rowId = worksheet.Cells[rowNum, "A".ToColumnNum()].Value?.ToString();
                        if (string.IsNullOrEmpty(rowId))
                            break;

                        string facultyName = worksheet.Cells[rowNum, "B".ToColumnNum()].Value?.ToString();

                        if (!facultyFilesDictionary.ContainsKey(facultyName))
                        {
                            // Create new file for faculty
                            var newFile = Excel.CloneFile(dataFileFullName, _localSaveFolderPath, facultyName + " " + dataFileName);
                            facultyFilesDictionary.Add(facultyName, newFile);

                            var newWorksheets = newFile.Workbook.Worksheets;
                            var newWorksheet_quant = newWorksheets.First();
                            var newWorksheet_qual = newWorksheets[1];

                            // Delete non-header rows for each sheet
                            int headerRowsCount_quant = 3;
                            int nonHeaderRowsCount_quant = newWorksheet_quant.Dimension.End.Row - headerRowsCount_quant;
                            int nonHeaderRowsCount_qual = newWorksheet_qual.Dimension.End.Row - headerRowsCount_qual;
                            newWorksheet_quant.DeleteRow(headerRowsCount_quant + 1, nonHeaderRowsCount_quant);
                            newWorksheet_qual.DeleteRow(headerRowsCount_qual + 1, nonHeaderRowsCount_qual);
                        }

                        var targetWorksheet = facultyFilesDictionary[facultyName].Workbook.Worksheets[1];
                        var targetRowCount = targetWorksheet.Dimension.Rows;
                        worksheet.Cells[rowNum, 1, rowNum, columnCount].Copy(targetWorksheet.Cells[targetRowCount + 1, 1]);
                    }
                }

                // Load data from the quantitative evaluation sheet from Excel file.
                {
                    // Get the Worksheet instance.
                    ExcelWorksheet worksheet = worksheets.First();

                    // Loop through the Worksheet rows.
                    var columnCount = worksheet.Dimension.Columns;
                    var rowCount = worksheet.Dimension.Rows;
                    int headerRowsCount = 3;
                    // Ignore the first 3 rows, which do not include evaluation data information.
                    for (int rowNum = headerRowsCount + 1; rowNum <= rowCount; rowNum++)
                    {
                        string rowId = worksheet.Cells[rowNum, "A".ToColumnNum()].Value?.ToString();
                        if (string.IsNullOrEmpty(rowId))
                            break;

                        string facultyName = worksheet.Cells[rowNum, "B".ToColumnNum()].Value?.ToString();

                        var targetWorksheet = facultyFilesDictionary[facultyName].Workbook.Worksheets.First();
                        var targetRowCount = targetWorksheet.Dimension.Rows;
                        worksheet.Cells[rowNum, 1, rowNum, columnCount].Copy(targetWorksheet.Cells[targetRowCount + 1, 1]);
                    }
                }

                // Apply all modifications to each new file
                foreach (var entry in facultyFilesDictionary)
                {
                    entry.Value.Save();

                    container.SharePointConnectionManager_EvaluationFiles.UploadFile(entry.Value.File.FullName, _sharePointSaveFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SplitEnglishEvaluationFile(ExcelPackage _document, string _localSaveFolderPath, string _sharePointSaveFolderPath)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string dataFileName = Path.GetFileNameWithoutExtension(_document.File.Name);
                string dataFileFullName = _document.File.FullName;
                Dictionary<string, ExcelPackage> facultyFilesDictionary = new Dictionary<string, ExcelPackage>();
                var worksheets = _document.Workbook.Worksheets;

                // Load data from the qualitative evaluation sheet from Excel file. Initialize all new worksheets here.
                {
                    // Get the Worksheet instance.
                    ExcelWorksheet worksheet = worksheets[2];

                    // Loop through the Worksheet rows.
                    var columnCount = worksheet.Dimension.Columns;
                    var rowCount = worksheet.Dimension.Rows;
                    int headerRowsCount_qual = 5;
                    // Ignore the first 5 rows, which do not include evaluation data information.
                    for (int rowNum = headerRowsCount_qual + 1; rowNum <= rowCount; rowNum++)
                    {
                        string rowId = worksheet.Cells[rowNum, "A".ToColumnNum()].Value?.ToString();
                        if (string.IsNullOrEmpty(rowId))
                            break;

                        string facultyName = worksheet.Cells[rowNum, "B".ToColumnNum()].Value?.ToString();

                        if (!facultyFilesDictionary.ContainsKey(facultyName))
                        {
                            // Create new file for faculty
                            var newFile = Excel.CloneFile(dataFileFullName, _localSaveFolderPath, facultyName + " " + dataFileName);
                            facultyFilesDictionary.Add(facultyName, newFile);

                            var newWorksheets = newFile.Workbook.Worksheets;
                            var newWorksheet_quant = newWorksheets.First();
                            var newWorksheet_270_quant = newWorksheets[1];
                            var newWorksheet_qual = newWorksheets[2];

                            // Delete non-header rows for each sheet
                            int headerRowsCount_quant = 3;
                            int nonHeaderRowsCount_quant = newWorksheet_quant.Dimension.End.Row - headerRowsCount_quant;
                            int nonHeaderRowsCount_270_quant = newWorksheet_270_quant.Dimension.End.Row - headerRowsCount_quant;
                            int nonHeaderRowsCount_qual = newWorksheet_qual.Dimension.End.Row - headerRowsCount_qual;
                            newWorksheet_quant.DeleteRow(headerRowsCount_quant + 1, nonHeaderRowsCount_quant);
                            newWorksheet_270_quant.DeleteRow(headerRowsCount_quant + 1, nonHeaderRowsCount_270_quant);
                            newWorksheet_qual.DeleteRow(headerRowsCount_qual + 1, nonHeaderRowsCount_qual);
                        }

                        var targetWorksheet = facultyFilesDictionary[facultyName].Workbook.Worksheets[2];
                        var targetRowCount = targetWorksheet.Dimension.Rows;
                        worksheet.Cells[rowNum, 1, rowNum, columnCount].Copy(targetWorksheet.Cells[targetRowCount + 1, 1]);
                    }
                }

                // Load data from the quantitative evaluation sheet from Excel file.
                {
                    // Get the Worksheet instance.
                    ExcelWorksheet worksheet = worksheets.First();

                    // Loop through the Worksheet rows.
                    var columnCount = worksheet.Dimension.Columns;
                    var rowCount = worksheet.Dimension.Rows;
                    int headerRowsCount = 3;
                    // Ignore the first 3 rows, which do not include evaluation data information.
                    for (int rowNum = headerRowsCount + 1; rowNum <= rowCount; rowNum++)
                    {
                        string rowId = worksheet.Cells[rowNum, "A".ToColumnNum()].Value?.ToString();
                        if (string.IsNullOrEmpty(rowId))
                            break;

                        string facultyName = worksheet.Cells[rowNum, "B".ToColumnNum()].Value?.ToString();

                        var targetWorksheet = facultyFilesDictionary[facultyName].Workbook.Worksheets.First();
                        var targetRowCount = targetWorksheet.Dimension.Rows;
                        worksheet.Cells[rowNum, 1, rowNum, columnCount].Copy(targetWorksheet.Cells[targetRowCount + 1, 1]);
                    }
                }

                // Load data from the 270 course quantitative evaluation sheet from Excel file.
                {
                    // Get the Worksheet instance.
                    ExcelWorksheet worksheet = worksheets[1];

                    // Loop through the Worksheet rows.
                    var columnCount = worksheet.Dimension.Columns;
                    var rowCount = worksheet.Dimension.Rows;
                    int headerRowsCount = 3;
                    // Ignore the first 3 rows, which do not include evaluation data information.
                    for (int rowNum = headerRowsCount + 1; rowNum <= rowCount; rowNum++)
                    {
                        string rowId = worksheet.Cells[rowNum, "A".ToColumnNum()].Value?.ToString();
                        if (string.IsNullOrEmpty(rowId))
                            break;

                        string facultyName = worksheet.Cells[rowNum, "B".ToColumnNum()].Value?.ToString();

                        var targetWorksheet = facultyFilesDictionary[facultyName].Workbook.Worksheets[1];
                        var targetRowCount = targetWorksheet.Dimension.Rows;
                        worksheet.Cells[rowNum, 1, rowNum, columnCount].Copy(targetWorksheet.Cells[targetRowCount + 1, 1]);
                    }
                }

                // Apply all modifications to each new file
                foreach (var entry in facultyFilesDictionary)
                {
                    entry.Value.Save();

                    container.SharePointConnectionManager_EvaluationFiles.UploadFile(entry.Value.File.FullName, _sharePointSaveFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string TermToString(eTerm _term)
        {
            switch (_term)
            {
                case eTerm.Spring: return "PRIMAVERA";
                case eTerm.Summer: return "VERANO";
                case eTerm.Fall: return "OTOÑO";
                default: return default;
            }
        }

        private string CourseCategoryToString(eCourseCategory _category)
        {
            switch (_category)
            {
                case eCourseCategory.German: return "ALEMÁN";
                case eCourseCategory.Chinese: return "CHINO";
                case eCourseCategory.SpanishArrupe: return "ESPAÑOL ARRUPE";
                case eCourseCategory.SpanishForeigner: return "ESPAÑOL PARA EXTRANJEROS";
                case eCourseCategory.French: return "FRANCÉS";
                case eCourseCategory.English: return "INGLÉS";
                case eCourseCategory.Italian: return "ITALIANO";
                case eCourseCategory.Japanese: return "JAPONÉS";
                case eCourseCategory.Portuguese: return "PORTUGUÉS";
                default: return default;
            }
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
