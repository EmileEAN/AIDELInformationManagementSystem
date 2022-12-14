using EEANWorks.Microsoft;
using EEANWorks.WinForms;
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
    public partial class BaseEvaluationsFileGenerator : Form
    {
        public BaseEvaluationsFileGenerator()
        {
            InitializeComponent();
        }

        private void BaseEvaluationsFileGenerator_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;

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

            foreach (var criteriaSet in container.QuantitativeEvaluationCriteriaSetDictionary.Where(x => !x.Value.Name.Contains("270")).Reverse())
            {
                cmbBx_criteriaSet_quant.Items.Add(criteriaSet.Value.Name);
                cmbBx_criteriaSet_quant.SelectedIndex = 0;
            }

            foreach (var criteriaSet in container.QuantitativeEvaluationCriteriaSetDictionary.Where(x => x.Value.Name.Contains("270")).Reverse())
            {
                cmbBx_criteriaSet_270_quant.Items.Add(criteriaSet.Value.Name);
                cmbBx_criteriaSet_270_quant.SelectedIndex = 0;
            }

            foreach (var criteriaSet in container.QualitativeEvaluationCriteriaSetDictionary.Values.Reverse())
            {
                cmbBx_criteriaSet_qual.Items.Add(criteriaSet.Name);
                cmbBx_criteriaSet_qual.SelectedIndex = 0;
            }
        }

        private void btn_browseImportingFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnFlDlg_importingFile.ShowDialog() == DialogResult.OK)
                    lbl_importingFileName.Text = Path.GetFileName(opnFlDlg_importingFile.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown Error: " + ex.Message + "Please try it again.");
            }
        }

        private void btn_templateFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnFlDlg_templateFile.ShowDialog() == DialogResult.OK)
                    lbl_templateFileName.Text = Path.GetFileName(opnFlDlg_templateFile.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown Error: " + ex.Message + "Please try it again.");
            }
        }


        private void btn_generate_Click(object sender, EventArgs e)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var courseBaseList = container.CourseBaseList;
                var quantCriteriaSetDictionary = container.QuantitativeEvaluationCriteriaSetDictionary;
                var qualCriteriaSetDictionary = container.QualitativeEvaluationCriteriaSetDictionary;

                int criteriaSet_quant_id = quantCriteriaSetDictionary.GetFirstKeyOrDefault(x => x.Value.Name == cmbBx_criteriaSet_quant.SelectedItem.ToString());
                int criteriaSet_270_quant_id = quantCriteriaSetDictionary.GetFirstKeyOrDefault(x => x.Value.Name == cmbBx_criteriaSet_270_quant.SelectedItem.ToString());
                int criteriaSet_qual_id = qualCriteriaSetDictionary.GetFirstKeyOrDefault(x => x.Value.Name == cmbBx_criteriaSet_qual.SelectedItem.ToString());


                
                string termString = cmbBx_term.SelectedItem.ToString();
                eTerm term = termString.ToCorrespondingEnumValue<eTerm>();
                int year = Convert.ToInt32(numUpDwn_year.Value);
                
                Semester semester = null;
                // Get semester. Add semester to database if not exists.
                {
                    bool semesterAdditionSucceeded = true;
                    {
                        int yearId = container.YearDictionary.GetFirstKeyOrDefault(year);
                        if (yearId == default) // If the item does not exist
                        {
                            yearId = DocumentLoader.AddYearToCSV(year);
                            if (yearId == default)
                                semesterAdditionSucceeded = false;
                        }

                        int termId = container.TermDictionary.GetFirstKey(term);
                        int semesterId = container.SemesterDictionary.FirstOrDefault(x => x.Value.Year == year
                                                                                && x.Value.Term == term).Key;
                        if (semesterId == default) // If the item does not exist
                        {
                            semesterId = DocumentLoader.AddSemesterToCSV(yearId, termId);
                            if (semesterId == default)
                                semesterAdditionSucceeded = false;
                        }
                        semester = container.SemesterDictionary[semesterId];
                    }

                    if (!semesterAdditionSucceeded)
                    {
                        MessageBox.Show("Failed to add semester!");
                        return;
                    }
                }

                string sharePointSaveFolderPath = @"Evaluation Files/" + TermToString(term) + year.ToString();
                string localSaveFolderPath = CoreValues.LocalSaveFilesPath + @"TmpEvaluationFiles\" + TermToString(term) + year.ToString();
                Dictionary<eCourseCategory, ExcelPackage> courseCategoryFilesDictionary = new Dictionary<eCourseCategory, ExcelPackage>();
                // Generate evaluation file per course category
                {
                    if (container.SharePointConnectionManager_EvaluationFiles.FolderExists(sharePointSaveFolderPath))
                    {
                        MessageBox.Show("No se pudieron generar los archivos. Ya existen archivos para el semestre!");
                        return;
                    }

                    if (!Directory.Exists(localSaveFolderPath))
                        Directory.CreateDirectory(localSaveFolderPath);


                    // Open the document for reading only.
                    using (var dataFile = new ExcelPackage(new System.IO.FileInfo(opnFlDlg_importingFile.FileName)))
                    {
                        using (var templateFile = new ExcelPackage(new System.IO.FileInfo(opnFlDlg_templateFile.FileName)))
                        {
                            string dataFileName = Path.GetFileNameWithoutExtension(opnFlDlg_importingFile.FileName);
                            // Load data from dataFile, create new files based on templateFile, and store loaded data to correspoinding file
                            {
                                // Get the Worksheet instances of both files.
                                ExcelWorksheet worksheet_dataFile = dataFile.Workbook.Worksheets.First();

                                ExcelWorksheet worksheet_quant_templateFile = templateFile.Workbook.Worksheets.First();
                                ExcelWorksheet worksheet_270_quant_templateFile = templateFile.Workbook.Worksheets[1];
                                ExcelWorksheet worksheet_qual_templateFile = templateFile.Workbook.Worksheets[2];

                                // Loop through the Worksheet rows.
                                {
                                    var rowCount_dataFile = worksheet_dataFile.Dimension.Rows;

                                    var columnCount_quant_templateFile = worksheet_quant_templateFile.Dimension.Columns;
                                    var columnCount_quant_270_templateFile = worksheet_270_quant_templateFile.Dimension.Columns;
                                    var columnCount_qual_templateFile = worksheet_qual_templateFile.Dimension.Columns;
                                    int templateHeaderRowsCount_quant = 3;
                                    int templateHeaderRowsCount_qual = 5;
                                    int templateRowNumber_quant = templateHeaderRowsCount_quant + 1;
                                    int templateRowNumber_qual = templateHeaderRowsCount_qual + 1;

                                    ExcelRange templateRow_quant = worksheet_quant_templateFile.Cells[templateRowNumber_quant, 1, templateRowNumber_quant, columnCount_quant_templateFile];
                                    ExcelRange templateRow_270_quant = worksheet_270_quant_templateFile.Cells[templateRowNumber_quant, 1, templateRowNumber_quant, columnCount_quant_270_templateFile];
                                    ExcelRange templateRow_qual = worksheet_qual_templateFile.Cells[templateRowNumber_qual, 1, templateRowNumber_qual, columnCount_qual_templateFile];

                                    List<int> non270RowNums = new List<int>();
                                    List<int> _270RowNums = new List<int>();
                                    // Ignore the first row, which does not include evaluation data information.
                                    for (int rowNum_dataFile = 2; rowNum_dataFile <= rowCount_dataFile; rowNum_dataFile++)
                                    {
                                        string studentAccountNumber = worksheet_dataFile.Cells[rowNum_dataFile, "A".ToColumnNum()].Value?.ToString();
                                        if (string.IsNullOrEmpty(studentAccountNumber))
                                            break;

                                        int courseBaseId = Convert.ToInt32(worksheet_dataFile.Cells[rowNum_dataFile, "B".ToColumnNum()].Value?.ToString());
                                        if (courseBaseId == 270)
                                            _270RowNums.Add(rowNum_dataFile);
                                        else
                                            non270RowNums.Add(rowNum_dataFile);

                                        CourseBase courseBase = courseBaseList.First(x => x.Id == courseBaseId);
                                        eCourseCategory courseCategory = courseBase.Category;

                                        if (!courseCategoryFilesDictionary.ContainsKey(courseCategory))
                                        {
                                            // Create new file per course category
                                            var newFile = Excel.CloneFile(opnFlDlg_templateFile.FileName, localSaveFolderPath, TermToString(term) + " " + year.ToString() + " CALIFICACIONES " + CourseCategoryToString(courseCategory));
                                            courseCategoryFilesDictionary.Add(courseCategory, newFile);

                                            var newWorksheets = newFile.Workbook.Worksheets;
                                            if (courseCategory != eCourseCategory.English) // Remove the "270" sheet as the file should not contain it.
                                            {
                                                var _270Worksheet = newWorksheets[1];
                                                newWorksheets.Delete(_270Worksheet);

                                                var newWorksheet_quant = newWorksheets.First(); // Quantitative evaluation worksheet for non-270 course rows
                                                var newWorksheet_qual = newWorksheets[1]; // Qualitative evaluation worksheet

                                                // Delete all rows except header rows
                                                int nonHeaderRowsCount_quant = newWorksheet_quant.Dimension.End.Row - templateHeaderRowsCount_quant;
                                                int nonHeaderRowsCount_qual = newWorksheet_qual.Dimension.End.Row - templateHeaderRowsCount_qual;
                                                newWorksheet_quant.DeleteRow(templateHeaderRowsCount_quant + 1, nonHeaderRowsCount_quant);
                                                newWorksheet_qual.DeleteRow(templateHeaderRowsCount_qual + 1, nonHeaderRowsCount_qual);
                                            }
                                            else
                                            {
                                                var newWorksheet_quant = newWorksheets.First(); // Quantitative evaluation worksheet for non-270 course rows
                                                var newWorksheet_270_quant = newWorksheets[1]; // Quantitative evaluation worksheet for 270 course rows
                                                var newWorksheet_qual = newWorksheets[2]; // Qualitative evaluation worksheet

                                                // Delete all rows except header rows
                                                int nonHeaderRowsCount_quant = newWorksheet_quant.Dimension.End.Row - templateHeaderRowsCount_quant;
                                                int nonHeaderRowsCount_270_quant = newWorksheet_270_quant.Dimension.End.Row - templateHeaderRowsCount_quant;
                                                int nonHeaderRowsCount_qual = newWorksheet_qual.Dimension.End.Row - templateHeaderRowsCount_qual;
                                                newWorksheet_quant.DeleteRow(templateHeaderRowsCount_quant + 1, nonHeaderRowsCount_quant);
                                                newWorksheet_270_quant.DeleteRow(templateHeaderRowsCount_quant + 1, nonHeaderRowsCount_270_quant);
                                                newWorksheet_qual.DeleteRow(templateHeaderRowsCount_qual + 1, nonHeaderRowsCount_qual);
                                            }


                                        }
                                    }

                                    foreach (int rowNum_dataFile in non270RowNums)
                                    {
                                        string studentAccountNumber = worksheet_dataFile.Cells[rowNum_dataFile, "A".ToColumnNum()].Value?.ToString();
                                        if (string.IsNullOrEmpty(studentAccountNumber))
                                            break;

                                        int courseBaseId = Convert.ToInt32(worksheet_dataFile.Cells[rowNum_dataFile, "B".ToColumnNum()].Value?.ToString());
                                        string groupName = worksheet_dataFile.Cells[rowNum_dataFile, "F".ToColumnNum()].Value?.ToString();
                                        string studentName = worksheet_dataFile.Cells[rowNum_dataFile, "I".ToColumnNum()].Value?.ToString();
                                        string facultyName = worksheet_dataFile.Cells[rowNum_dataFile, "K".ToColumnNum()].Value?.ToString().Remove(".").MultipleSpacesToSingleSpace();

                                        CourseBase courseBase = courseBaseList.First(x => x.Id == courseBaseId);
                                        eCourseCategory courseCategory = courseBase.Category;

                                        var targetWorksheets = courseCategoryFilesDictionary[courseCategory].Workbook.Worksheets;

                                        var targetWorksheet_quant = targetWorksheets.First();
                                        var targetWorksheet_qual = (courseCategory != eCourseCategory.English) ? targetWorksheets[1] : targetWorksheets[2];
                                        var targetRowCount_quant = targetWorksheet_quant.Dimension.Rows;
                                        var targetRowCount_qual = targetWorksheet_qual.Dimension.Rows;
                                        var newRowNumber_quant = targetRowCount_quant + 1;
                                        var newRowNumber_qual = targetRowCount_qual + 1;

                                        // Insert new row (copy of the template row)
                                        templateRow_quant.Copy(targetWorksheet_quant.Cells[newRowNumber_quant, 1]);
                                        templateRow_qual.Copy(targetWorksheet_qual.Cells[newRowNumber_qual, 1]);

                                        // Add data to new row                                                   
                                        {
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "A".ToColumnNum()].Value = (newRowNumber_quant - templateHeaderRowsCount_quant).ToString();
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "B".ToColumnNum()].Value = facultyName;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "C".ToColumnNum()].Value = courseBaseId.ToString();
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "D".ToColumnNum()].Value = courseBase.Name;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "E".ToColumnNum()].Value = groupName;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "F".ToColumnNum()].Value = studentAccountNumber;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "G".ToColumnNum()].Value = studentName;

                                            targetWorksheet_qual.Cells[newRowNumber_qual, "A".ToColumnNum()].Value = (newRowNumber_qual - templateHeaderRowsCount_qual).ToString();
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "B".ToColumnNum()].Value = facultyName;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "C".ToColumnNum()].Value = courseBaseId.ToString();
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "D".ToColumnNum()].Value = courseBase.Name;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "E".ToColumnNum()].Value = groupName;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "F".ToColumnNum()].Value = studentAccountNumber;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "G".ToColumnNum()].Value = studentName;
                                        }

                                        // Add data to database
                                        {
                                            string columnSetName = cmbBx_columnSet_general.SelectedItem.ToString();
                                            var columnSet = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName).Value;

                                            DocumentLoader.AddEvaluationData(columnSet, criteriaSet_quant_id, criteriaSet_qual_id, semester, facultyName, courseBase, groupName, Convert.ToInt32(studentAccountNumber), studentName);
                                        }
                                    }

                                    foreach (int rowNum_dataFile in _270RowNums)
                                    {
                                        string studentAccountNumber = worksheet_dataFile.Cells[rowNum_dataFile, "A".ToColumnNum()].Value?.ToString();
                                        if (string.IsNullOrEmpty(studentAccountNumber))
                                            break;

                                        int courseBaseId = 270;
                                        string groupName = worksheet_dataFile.Cells[rowNum_dataFile, "F".ToColumnNum()].Value?.ToString();
                                        string studentName = worksheet_dataFile.Cells[rowNum_dataFile, "I".ToColumnNum()].Value?.ToString();
                                        string facultyName = worksheet_dataFile.Cells[rowNum_dataFile, "K".ToColumnNum()].Value?.ToString().Remove(".").MultipleSpacesToSingleSpace();

                                        CourseBase courseBase = courseBaseList.First(x => x.Id == courseBaseId);
                                        eCourseCategory courseCategory = courseBase.Category;

                                        var targetWorksheets = courseCategoryFilesDictionary[courseCategory].Workbook.Worksheets;

                                        var targetWorksheet_quant = targetWorksheets[1];
                                        var targetWorksheet_qual = targetWorksheets[2];
                                        var targetRowCount_quant = targetWorksheet_quant.Dimension.Rows;
                                        var targetRowCount_qual = targetWorksheet_qual.Dimension.Rows;
                                        var newRowNumber_quant = targetRowCount_quant + 1;
                                        var newRowNumber_qual = targetRowCount_qual + 1;

                                        // Insert new row (copy of the template row)
                                        templateRow_270_quant.Copy(targetWorksheet_quant.Cells[newRowNumber_quant, 1]);
                                        templateRow_qual.Copy(targetWorksheet_qual.Cells[newRowNumber_qual, 1]);

                                        // Add data to new row                                                   
                                        {
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "A".ToColumnNum()].Value = (newRowNumber_quant - templateHeaderRowsCount_quant).ToString();
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "B".ToColumnNum()].Value = facultyName;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "C".ToColumnNum()].Value = groupName;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "D".ToColumnNum()].Value = studentAccountNumber;
                                            targetWorksheet_quant.Cells[newRowNumber_quant, "E".ToColumnNum()].Value = studentName;

                                            targetWorksheet_qual.Cells[newRowNumber_qual, "A".ToColumnNum()].Value = (newRowNumber_qual - templateHeaderRowsCount_qual).ToString();
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "B".ToColumnNum()].Value = facultyName;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "C".ToColumnNum()].Value = courseBaseId.ToString();
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "D".ToColumnNum()].Value = courseBase.Name;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "E".ToColumnNum()].Value = groupName;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "F".ToColumnNum()].Value = studentAccountNumber;
                                            targetWorksheet_qual.Cells[newRowNumber_qual, "G".ToColumnNum()].Value = studentName;
                                        }

                                        // Add data to database
                                        {
                                            string columnSetName = cmbBx_columnSet_270.SelectedItem.ToString();
                                            var columnSet = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName).Value;

                                            DocumentLoader.AddEvaluationData(columnSet, criteriaSet_270_quant_id, criteriaSet_qual_id, semester, facultyName, courseBase, groupName, Convert.ToInt32(studentAccountNumber), studentName);
                                        }
                                    }
                                }
                            }

                            // Upload all data to online database
                            container.SharePointConnectionManager.UploadAllModifiedFiles();

                            // Apply all modifications to each new file and upload it to Sharepoint
                            foreach (var entry in courseCategoryFilesDictionary)
                            {
                                entry.Value.Save();

                                container.SharePointConnectionManager_EvaluationFiles.UploadFile(entry.Value.File.FullName, sharePointSaveFolderPath);
                            }
                        }
                    }
                }

                MessageBox.Show("Files generated successfully");
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
