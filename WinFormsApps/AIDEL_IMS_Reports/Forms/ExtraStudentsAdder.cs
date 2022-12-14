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
    public partial class ExtraStudentsAdder : Form
    {
        public ExtraStudentsAdder()
        {
            InitializeComponent();
        }

        private void ExtraStudentsAdder_Load(object sender, EventArgs e)
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


        private void btn_add_Click(object sender, EventArgs e)
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


                string sharePointFolderPath_languages = @"Evaluation Files/" + TermToString(term) + year.ToString();
                string sharePointFolderPath_faculties = sharePointFolderPath_languages + @"/Faculties";

                if (!(container.SharePointConnectionManager_EvaluationFiles.FolderExists(sharePointFolderPath_languages)))
                {
                    MessageBox.Show("Los alistados para el semestre indicado no existen!");
                    return;
                }

                if (!(container.SharePointConnectionManager_EvaluationFiles.FolderExists(sharePointFolderPath_faculties)))
                {
                    MessageBox.Show("Los archivos para los maestros no han sido generados!");
                    return;
                }

                Semester semester = container.SemesterDictionary.First(x => x.Value.Year == year
                                                                            && x.Value.Term == term).Value;

                string localSaveFolderPath_languages = CoreValues.LocalSaveFilesPath + @"TmpEvaluationFiles\" + TermToString(term) + year.ToString();
                if (!Directory.Exists(localSaveFolderPath_languages))
                    Directory.CreateDirectory(localSaveFolderPath_languages);

                string localSaveFolderPath_faculties = localSaveFolderPath_languages + @"\Faculties";
                if (!Directory.Exists(localSaveFolderPath_faculties))
                    Directory.CreateDirectory(localSaveFolderPath_faculties);
                localSaveFolderPath_faculties += @"\";

                List<string> filePaths_faculty = container.SharePointConnectionManager_EvaluationFiles.DownloadAndSaveAllFiles(sharePointFolderPath_faculties, localSaveFolderPath_faculties);
                container.SharePointConnectionManager_EvaluationFiles.DownloadAndSaveAllFiles(sharePointFolderPath_languages, localSaveFolderPath_languages + @"\");
                string[] fileNames_language = Directory.GetFiles(localSaveFolderPath_languages, "*.*", SearchOption.TopDirectoryOnly);


                Dictionary<string, ExcelPackage> languageFiles = new Dictionary<string, ExcelPackage>();
                foreach (string fileName in fileNames_language)
                {
                    languageFiles.Add(fileName, new ExcelPackage(new System.IO.FileInfo(localSaveFolderPath_languages + @"\" + fileName)));
                }

                Dictionary<string, ExcelPackage> facultyFiles = new Dictionary<string, ExcelPackage>();
                foreach (string filePath in filePaths_faculty)
                {
                    facultyFiles.Add(filePath, new ExcelPackage(new System.IO.FileInfo(filePath)));
                }
                //foreach (string filePath in filePaths_faculty)
                //{
                //    if (filePath.Contains(englishCategoryString))
                //    {
                //        string fileName_target = Path.GetFileName(fileNames_language.First(x => x.Contains(englishCategoryString)));
                //        UpdateEnglishEvaluationFile(new ExcelPackage(new System.IO.FileInfo(filePath)), semester, localSaveFolderPath_target, sharePointFolderPath_target, fileName_target);
                //    }
                //    else
                //    {
                //        eCourseCategory category = CourseCategoryFromString(filePath);
                //        string fileName_target = Path.GetFileName(fileNames_language.First(x => x.Contains(CourseCategoryToString(category))));
                //        UpdateNonEnglishEvaluationFile(new ExcelPackage(new System.IO.FileInfo(filePath)), semester, localSaveFolderPath_target, sharePointFolderPath_target, fileName_target);
                //    }
                //}

                {
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

                                        // Add rows to files
                                        {
                                            var languageWorksheets = languageFiles.First(x => x.Key.Contains(CourseCategoryToString(courseCategory))).Value.Workbook.Worksheets;

                                            var languageWorksheet_quant = languageWorksheets.First();
                                            var languageWorksheet_qual = (courseCategory != eCourseCategory.English) ? languageWorksheets[1] : languageWorksheets[2];
                                            var languageRowCount_quant = languageWorksheet_quant.Dimension.Rows;
                                            var languageRowCount_qual = languageWorksheet_qual.Dimension.Rows;
                                            var newRowNumber_quant_language = languageRowCount_quant + 1;
                                            var newRowNumber_qual_language = languageRowCount_qual + 1;

                                            // Insert new row (copy of the template row)
                                            templateRow_quant.Copy(languageWorksheet_quant.Cells[newRowNumber_quant_language, 1]);
                                            templateRow_qual.Copy(languageWorksheet_qual.Cells[newRowNumber_qual_language, 1]);

                                            // Add data to new language file row                                                   
                                            {
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "A".ToColumnNum()].Value = (newRowNumber_quant_language - templateHeaderRowsCount_quant).ToString();
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "B".ToColumnNum()].Value = facultyName;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "C".ToColumnNum()].Value = courseBaseId.ToString();
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "D".ToColumnNum()].Value = courseBase.Name;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "E".ToColumnNum()].Value = groupName;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "F".ToColumnNum()].Value = studentAccountNumber;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "G".ToColumnNum()].Value = studentName;

                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "A".ToColumnNum()].Value = (newRowNumber_qual_language - templateHeaderRowsCount_qual).ToString();
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "B".ToColumnNum()].Value = facultyName;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "C".ToColumnNum()].Value = courseBaseId.ToString();
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "D".ToColumnNum()].Value = courseBase.Name;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "E".ToColumnNum()].Value = groupName;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "F".ToColumnNum()].Value = studentAccountNumber;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "G".ToColumnNum()].Value = studentName;
                                            }

                                            // Add row to faculty file
                                            {
                                                var facultyWorksheets = facultyFiles.First(x => x.Key.Contains(CourseCategoryToString(courseCategory))).Value.Workbook.Worksheets;

                                                var facultyWorksheet_quant = facultyWorksheets.First();
                                                var facultyWorksheet_qual = (courseCategory != eCourseCategory.English) ? facultyWorksheets[1] : facultyWorksheets[2];
                                                var facultyRowCount_quant = facultyWorksheet_quant.Dimension.Rows;
                                                var facultyRowCount_qual = facultyWorksheet_qual.Dimension.Rows;
                                                var newRowNumber_quant_faculty = facultyRowCount_quant + 1;
                                                var newRowNumber_qual_faculty = facultyRowCount_qual + 1;

                                                ExcelRange languageRowQuant = languageWorksheet_quant.Cells[newRowNumber_quant_language, 1, newRowNumber_quant_language, columnCount_quant_templateFile];
                                                ExcelRange languageRowQual = languageWorksheet_qual.Cells[newRowNumber_qual_language, 1, newRowNumber_qual_language, columnCount_qual_templateFile];

                                                // Copy new language file row and insert
                                                languageRowQuant.Copy(facultyWorksheet_quant.Cells[newRowNumber_quant_faculty, 1]);
                                                languageRowQual.Copy(facultyWorksheet_qual.Cells[newRowNumber_qual_faculty, 1]);
                                            }
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

                                        // Add rows to files
                                        {
                                            var languageWorksheets = languageFiles.First(x => x.Key.Contains(CourseCategoryToString(courseCategory))).Value.Workbook.Worksheets;

                                            var languageWorksheet_quant = languageWorksheets[1];
                                            var languageWorksheet_qual = languageWorksheets[2];
                                            var languageRowCount_quant = languageWorksheet_quant.Dimension.Rows;
                                            var languageRowCount_qual = languageWorksheet_qual.Dimension.Rows;
                                            var newRowNumber_quant_language = languageRowCount_quant + 1;
                                            var newRowNumber_qual_language = languageRowCount_qual + 1;

                                            // Insert new row (copy of the template row)
                                            templateRow_270_quant.Copy(languageWorksheet_quant.Cells[newRowNumber_quant_language, 1]);
                                            templateRow_qual.Copy(languageWorksheet_qual.Cells[newRowNumber_qual_language, 1]);

                                            // Add data to new language file row                                                   
                                            {
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "A".ToColumnNum()].Value = (newRowNumber_quant_language - templateHeaderRowsCount_quant).ToString();
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "B".ToColumnNum()].Value = facultyName;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "C".ToColumnNum()].Value = groupName;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "D".ToColumnNum()].Value = studentAccountNumber;
                                                languageWorksheet_quant.Cells[newRowNumber_quant_language, "E".ToColumnNum()].Value = studentName;

                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "A".ToColumnNum()].Value = (newRowNumber_qual_language - templateHeaderRowsCount_qual).ToString();
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "B".ToColumnNum()].Value = facultyName;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "C".ToColumnNum()].Value = courseBaseId.ToString();
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "D".ToColumnNum()].Value = courseBase.Name;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "E".ToColumnNum()].Value = groupName;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "F".ToColumnNum()].Value = studentAccountNumber;
                                                languageWorksheet_qual.Cells[newRowNumber_qual_language, "G".ToColumnNum()].Value = studentName;
                                            }

                                            // Add row to faculty file
                                            {
                                                var facultyWorksheets = facultyFiles.First(x => x.Key.Contains(CourseCategoryToString(courseCategory))).Value.Workbook.Worksheets;

                                                var facultyWorksheet_quant = facultyWorksheets.First();
                                                var facultyWorksheet_qual = (courseCategory != eCourseCategory.English) ? facultyWorksheets[1] : facultyWorksheets[2];
                                                var facultyRowCount_quant = facultyWorksheet_quant.Dimension.Rows;
                                                var facultyRowCount_qual = facultyWorksheet_qual.Dimension.Rows;
                                                var newRowNumber_quant_faculty = facultyRowCount_quant + 1;
                                                var newRowNumber_qual_faculty = facultyRowCount_qual + 1;

                                                ExcelRange languageRowQuant = languageWorksheet_quant.Cells[newRowNumber_quant_language, 1, newRowNumber_quant_language, columnCount_quant_templateFile];
                                                ExcelRange languageRowQual = languageWorksheet_qual.Cells[newRowNumber_qual_language, 1, newRowNumber_qual_language, columnCount_qual_templateFile];

                                                // Copy new language file row and insert
                                                languageRowQuant.Copy(facultyWorksheet_quant.Cells[newRowNumber_quant_faculty, 1]);
                                                languageRowQual.Copy(facultyWorksheet_qual.Cells[newRowNumber_qual_faculty, 1]);
                                            }
                                        }

                                        // Add data to database
                                        {
                                            string columnSetName = cmbBx_columnSet_general.SelectedItem.ToString();
                                            var columnSet = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName).Value;

                                            DocumentLoader.AddEvaluationData(columnSet, criteriaSet_270_quant_id, criteriaSet_qual_id, semester, facultyName, courseBase, groupName, Convert.ToInt32(studentAccountNumber), studentName);
                                        }
                                    }
                                }
                            }

                            // Upload all data to online database
                            container.SharePointConnectionManager.UploadAllModifiedFiles();

                            // Apply all modifications to each new file and upload it to Sharepoint
                            foreach (var entry in languageFiles)
                            {
                                entry.Value.Save();

                                container.SharePointConnectionManager_EvaluationFiles.UploadFile(entry.Value.File.FullName, sharePointFolderPath_languages);
                            }
                            foreach (var entry in facultyFiles)
                            {
                                entry.Value.Save();

                                container.SharePointConnectionManager_EvaluationFiles.UploadFile(entry.Value.File.FullName, sharePointFolderPath_faculties);
                            }
                        }
                    }
                }

                // Upload all data to online database
                container.SharePointConnectionManager.UploadAllModifiedFiles();

                MessageBox.Show("Studens added successfully");
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
