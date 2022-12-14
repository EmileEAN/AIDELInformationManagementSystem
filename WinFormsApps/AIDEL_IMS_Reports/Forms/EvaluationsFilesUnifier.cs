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
    public partial class EvaluationsFilesUnifier : Form
    {
        public EvaluationsFilesUnifier()
        {
            InitializeComponent();
        }

        private void EvaluationsFilesUnifier_Load(object sender, EventArgs e)
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

        private void btn_unify_Click(object sender, EventArgs e)
        {
            lbl_status.DynamicUpdate("Comenzando el proceso...");

            try
            {
                DataContainer container = DataContainer.Instance;

                string termString = cmbBx_term.SelectedItem.ToString();
                eTerm term = termString.ToCorrespondingEnumValue<eTerm>();
                int year = Convert.ToInt32(numUpDwn_year.Value);


                lbl_status.DynamicUpdate("Verificando existencia de directorios...");

                string sharePointFolderPath_target = @"Evaluation Files/" + TermToString(term) + year.ToString();
                string sharePointFolderPath_data = sharePointFolderPath_target + @"/Faculties";

                if (!(container.SharePointConnectionManager_EvaluationFiles.FolderExists(sharePointFolderPath_target)))
                {
                    MessageBox.Show("Los alistados para el semestre indicado no existen!");
                    return;
                }

                if (!(container.SharePointConnectionManager_EvaluationFiles.FolderExists(sharePointFolderPath_data)))
                {
                    MessageBox.Show("Los archivos para los maestros no han sido generados!");
                    return;
                }

                Semester semester = container.SemesterDictionary.First(x => x.Value.Year == year
                                                                            && x.Value.Term == term).Value;

                string localSaveFolderPath_target = CoreValues.LocalSaveFilesPath + @"TmpEvaluationFiles\" + TermToString(term) + year.ToString();
                if (!Directory.Exists(localSaveFolderPath_target))
                    Directory.CreateDirectory(localSaveFolderPath_target);

                string localSaveFolderPath_data = localSaveFolderPath_target + @"\Faculties";
                if (!Directory.Exists(localSaveFolderPath_data))
                    Directory.CreateDirectory(localSaveFolderPath_data);
                localSaveFolderPath_data += @"\";

                lbl_status.DynamicUpdate("Descargando copias de archivos para modificación...");
                List<string> filePaths_data = container.SharePointConnectionManager_EvaluationFiles.DownloadAndSaveAllFiles(sharePointFolderPath_data, localSaveFolderPath_data);
                container.SharePointConnectionManager_EvaluationFiles.DownloadAndSaveAllFiles(sharePointFolderPath_target, localSaveFolderPath_target + @"\");
                string[] fileNames_target = Directory.GetFiles(localSaveFolderPath_target, "*.*", SearchOption.TopDirectoryOnly);
                string englishCategoryString = CourseCategoryToString(eCourseCategory.English);
                int numOfDataFiles = filePaths_data.Count;
                foreach (string filePath in filePaths_data)
                {
                    lbl_status.DynamicUpdate("Procesando datos del archivo: " + Path.GetFileName(filePath));

                    if (filePath.Contains(englishCategoryString))
                    {
                        string fileName_target = Path.GetFileName(fileNames_target.First(x => x.Contains(englishCategoryString)));
                        UpdateEnglishEvaluationFile(new ExcelPackage(new System.IO.FileInfo(filePath)), semester, localSaveFolderPath_target, sharePointFolderPath_target, fileName_target);
                    }
                    else
                    {
                        eCourseCategory category = CourseCategoryFromString(filePath);
                        string fileName_target = Path.GetFileName(fileNames_target.First(x => x.Contains(CourseCategoryToString(category))));
                        UpdateNonEnglishEvaluationFile(new ExcelPackage(new System.IO.FileInfo(filePath)), semester, localSaveFolderPath_target, sharePointFolderPath_target, fileName_target);
                    }
                }

                // Upload all data to online database
                lbl_status.DynamicUpdate("Actualizando los archivos de SharePoint...");
                container.SharePointConnectionManager.UploadAllModifiedFiles();


                lbl_status.DynamicUpdate("Los datos han sido actualizados!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateEnglishEvaluationFile(ExcelPackage _dataDocument, Semester _semester, string _localSaveFolderPath, string _sharePointSaveFolderPath, string _fileName)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string targetFilePath = _localSaveFolderPath + @"\" + _fileName;

                string columnSetName_general = cmbBx_columnSet_general.SelectedItem.ToString();
                string columnSetName_270 = cmbBx_columnSet_270.SelectedItem.ToString();
                var columnSet_general = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName_general).Value;
                var columnSet_270 = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName_270).Value;

                using (var targetDocument = new ExcelPackage(new System.IO.FileInfo(targetFilePath)))
                {
                    var worksheets_data = _dataDocument.Workbook.Worksheets;
                    var worksheets_target = targetDocument.Workbook.Worksheets;

                    // Load data from the non 270 course quantitative evaluation sheets
                    {
                        // Get the Worksheet instances.
                        ExcelWorksheet worksheet_data = worksheets_data.First();
                        ExcelWorksheet worksheet_target = worksheets_target.First();

                        int numOfColumns = GetNumberOfColumns(worksheet_target);

                        int headerRowsCount = 3;

                        // Loop through the data worksheet rows.
                        var rowCount_data = worksheet_data.Dimension.Rows;
                        List<RowInfo> rowInfos_data = new List<RowInfo>();
                        // Ignore the first 3 rows, which do not include evaluation data information.
                        for (int rowNum_data = headerRowsCount + 1; rowNum_data <= rowCount_data; rowNum_data++)
                        {
                            try
                            {
                                string rowId = worksheet_data.Cells[rowNum_data, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_data.Cells[rowNum_data, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_data.Cells[rowNum_data, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_data.Cells[rowNum_data, "F".ToColumnNum()].Value?.ToString();

                                rowInfos_data.Add(new RowInfo(courseBaseId, groupName, studentAccountNumber, rowNum_data));
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner1>: " + ex1.Message);
                            }
                        }

                        // Loop through the target worksheet rows.
                        var rowCount_target = worksheet_target.Dimension.Rows;
                        for (int rowNum_target = headerRowsCount + 1; rowNum_target <= rowCount_target; rowNum_target++)
                        {
                            try
                            {
                                string rowId = worksheet_target.Cells[rowNum_target, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_target.Cells[rowNum_target, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_target.Cells[rowNum_target, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_target.Cells[rowNum_target, "F".ToColumnNum()].Value?.ToString();

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);
                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId = courseEntry.Key;
                                Course course = courseEntry.Value;
                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == Convert.ToInt32(studentAccountNumber));
                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                var studentInfo = group.StudentInfos.First(x => x.Student == student);

                                int rowNum_data = GetRowNumber(rowInfos_data, courseBaseId, groupName, studentAccountNumber);
                                if (rowNum_data > 0) // If data for target row exists
                                {
                                    int colNum_attendance = "H".ToColumnNum();
                                    var attendance = worksheet_data.Cells[rowNum_data, colNum_attendance].Value;
                                    if (attendance != null)
                                        worksheet_target.Cells[rowNum_target, colNum_attendance].Value = attendance;

                                    int colNum_comments = "I".ToColumnNum();
                                    var comments = worksheet_data.Cells[rowNum_data, colNum_comments].Value;
                                    if (comments != null)
                                        worksheet_target.Cells[rowNum_target, colNum_comments].Value = comments;

                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

                                        // First Partial
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_FirstPartial;
                                            if (evaluationSet.Count > 0) // This evaluation set is not used during certain terms.
                                            {
                                                int startNum = ((ColumnRange)(columnSet_general.QntFirstPartialColumnRange)).StartColumnLetter.ToColumnNum();
                                                int endNum = ((ColumnRange)(columnSet_general.QntFirstPartialColumnRange)).EndColumnLetter.ToColumnNum();
                                                for (int colNum = startNum; colNum <= endNum; colNum++)
                                                {
                                                    var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                    bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                    decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                    if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                        worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                    DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                                }

                                                int colNum_absence = endNum + 2;
                                                var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                                if (absence != null)
                                                    worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                                else
                                                    worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                            }
                                        }

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Midterm;

                                            int startNum = columnSet_general.QntMidtermColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet_general.QntMidtermColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                            }

                                            int colNum_absence = endNum + 2;
                                            var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                            if (absence != null)
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                            else
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Final;

                                            int startNum = columnSet_general.QntFinalsColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet_general.QntFinalsColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                            }

                                            int colNum_absence = endNum + 2;
                                            var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                            if (absence != null)
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                            else
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                        }

                                        // Additional
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Additional;

                                            List<string> columnNames = columnSet_general.QntAdditionalColumns;
                                            int evaluationIndex = 0;
                                            foreach (var columnName in columnNames)
                                            {
                                                int colNum = columnName.ToColumnNum();
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[evaluationIndex].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[evaluationIndex], decimalEvaluationValue);

                                                evaluationIndex++;
                                            }
                                        }
                                    }

                                    // Extra columns
                                    {
                                        int colNum_passFailed = numOfColumns - 2;
                                        var passFailed = worksheet_data.Cells[rowNum_data, colNum_passFailed].Value;
                                        if (passFailed != null)
                                            worksheet_target.Cells[rowNum_target, colNum_passFailed].Value = passFailed;

                                        int colNum_recommendation = numOfColumns - 1;
                                        var recommendation = worksheet_data.Cells[rowNum_data, colNum_recommendation].Value;
                                        if (recommendation != null)
                                            worksheet_target.Cells[rowNum_target, colNum_recommendation].Value = recommendation;

                                        int colNum_finalComments = numOfColumns;
                                        var finalComments = worksheet_data.Cells[rowNum_data, colNum_finalComments].Value;
                                        if (finalComments != null)
                                            worksheet_target.Cells[rowNum_target, colNum_finalComments].Value = finalComments;
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner2>: " + ex1.Message);
                            }
                        }
                    }


                    // Load data from the 270 course quantitative evaluation sheets
                    {
                        // Get the Worksheet instances.
                        ExcelWorksheet worksheet_data = worksheets_data[1];
                        ExcelWorksheet worksheet_target = worksheets_target[1];

                        int courseBaseId = 270;

                        int numOfColumns = GetNumberOfColumns(worksheet_target);

                        int headerRowsCount = 3;

                        // Loop through the data worksheet rows.
                        var rowCount_data = worksheet_data.Dimension.Rows;
                        List<RowInfo> rowInfos_data = new List<RowInfo>();
                        // Ignore the first 3 rows, which do not include evaluation data information.
                        for (int rowNum_data = headerRowsCount + 1; rowNum_data <= rowCount_data; rowNum_data++)
                        {
                            try
                            {
                                string rowId = worksheet_data.Cells[rowNum_data, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                string groupName = worksheet_data.Cells[rowNum_data, "C".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_data.Cells[rowNum_data, "D".ToColumnNum()].Value?.ToString();

                                rowInfos_data.Add(new RowInfo(courseBaseId, groupName, studentAccountNumber, rowNum_data));
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner3>: " + ex1.Message);
                            }
                        }

                        // Loop through the target worksheet rows.
                        var rowCount_target = worksheet_target.Dimension.Rows;
                        for (int rowNum_target = headerRowsCount + 1; rowNum_target <= rowCount_target; rowNum_target++)
                        {
                            try
                            {
                                string rowId = worksheet_target.Cells[rowNum_target, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                string groupName = worksheet_target.Cells[rowNum_target, "C".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_target.Cells[rowNum_target, "D".ToColumnNum()].Value?.ToString();

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);
                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId = courseEntry.Key;
                                Course course = courseEntry.Value;
                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == Convert.ToInt32(studentAccountNumber));
                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                var studentInfo = group.StudentInfos.First(x => x.Student == student);

                                int rowNum_data = GetRowNumber(rowInfos_data, courseBaseId, groupName, studentAccountNumber);
                                if (rowNum_data > 0) // If data for target row exists
                                {
                                    int colNum_attendance = "F".ToColumnNum();
                                    var attendance = worksheet_data.Cells[rowNum_data, colNum_attendance].Value;
                                    if (attendance != null)
                                        worksheet_target.Cells[rowNum_target, colNum_attendance].Value = attendance;

                                    int colNum_comments = "G".ToColumnNum();
                                    var comments = worksheet_data.Cells[rowNum_data, colNum_comments].Value;
                                    if (comments != null)
                                        worksheet_target.Cells[rowNum_target, colNum_comments].Value = comments;

                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

                                        // First Partial
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_FirstPartial;
                                            if (evaluationSet.Count > 0) // This evaluation set is not used during certain terms.
                                            {
                                                int startNum = ((ColumnRange)(columnSet_270.QntFirstPartialColumnRange)).StartColumnLetter.ToColumnNum();
                                                int endNum = ((ColumnRange)(columnSet_270.QntFirstPartialColumnRange)).EndColumnLetter.ToColumnNum();
                                                for (int colNum = startNum; colNum <= endNum; colNum++)
                                                {
                                                    var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                    bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                    decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                    if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                        worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                    DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                                }

                                                int colNum_absence = endNum + 3;
                                                var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                                if (absence != null)
                                                    worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                                else
                                                    worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                            }
                                        }

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Midterm;

                                            int startNum = columnSet_270.QntMidtermColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet_270.QntMidtermColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                            }

                                            int colNum_absence = endNum + 3;
                                            var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                            if (absence != null)
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                            else
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Final;

                                            int startNum = columnSet_270.QntFinalsColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet_270.QntFinalsColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                            }

                                            int colNum_absence = endNum + 3;
                                            var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                            if (absence != null)
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                            else
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                        }

                                        // Additional
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Additional;

                                            List<string> columnNames = columnSet_270.QntAdditionalColumns;
                                            int evaluationIndex = 0;
                                            foreach (var columnName in columnNames)
                                            {
                                                int colNum = columnName.ToColumnNum();
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[evaluationIndex].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[evaluationIndex], decimalEvaluationValue);

                                                evaluationIndex++;
                                            }
                                        }
                                    }

                                    // Extra columns
                                    {
                                        int colNum_passFailed = numOfColumns - 2;
                                        var passFailed = worksheet_data.Cells[rowNum_data, colNum_passFailed].Value;
                                        if (passFailed != null)
                                            worksheet_target.Cells[rowNum_target, colNum_passFailed].Value = passFailed;

                                        int colNum_recommendation = numOfColumns - 1;
                                        var recommendation = worksheet_data.Cells[rowNum_data, colNum_recommendation].Value;
                                        if (recommendation != null)
                                            worksheet_target.Cells[rowNum_target, colNum_recommendation].Value = recommendation;

                                        int colNum_finalComments = numOfColumns;
                                        var finalComments = worksheet_data.Cells[rowNum_data, colNum_finalComments].Value;
                                        if (finalComments != null)
                                            worksheet_target.Cells[rowNum_target, colNum_finalComments].Value = finalComments;
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner4>: " + ex1.Message);
                            }
                        }
                    }


                    // Load data from the qualitative evaluation sheets
                    {
                        // Get the Worksheet instances.
                        ExcelWorksheet worksheet_data = worksheets_data[2];
                        ExcelWorksheet worksheet_target = worksheets_target[2];

                        int headerRowsCount = 5;

                        // Loop through the data worksheet rows.
                        var rowCount_data = worksheet_data.Dimension.Rows;
                        List<RowInfo> rowInfos_data = new List<RowInfo>();
                        // Ignore the first 5 rows, which do not include evaluation data information.
                        for (int rowNum_data = headerRowsCount + 1; rowNum_data <= rowCount_data; rowNum_data++)
                        {
                            try
                            {
                                string rowId = worksheet_data.Cells[rowNum_data, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_data.Cells[rowNum_data, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_data.Cells[rowNum_data, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_data.Cells[rowNum_data, "F".ToColumnNum()].Value?.ToString();

                                rowInfos_data.Add(new RowInfo(courseBaseId, groupName, studentAccountNumber, rowNum_data));
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner5>: " + ex1.Message);
                            }
                        }

                        // Loop through the target worksheet rows.
                        var rowCount_target = worksheet_target.Dimension.Rows;
                        for (int rowNum_target = headerRowsCount + 1; rowNum_target <= rowCount_target; rowNum_target++)
                        {
                            try
                            {
                                string rowId = worksheet_target.Cells[rowNum_target, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_target.Cells[rowNum_target, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_target.Cells[rowNum_target, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_target.Cells[rowNum_target, "F".ToColumnNum()].Value?.ToString();

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);
                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId = courseEntry.Key;
                                Course course = courseEntry.Value;
                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == Convert.ToInt32(studentAccountNumber));
                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                var studentInfo = group.StudentInfos.First(x => x.Student == student);

                                var columnSet = (courseBaseId == 270) ? columnSet_270 : columnSet_general;

                                int rowNum_data = GetRowNumber(rowInfos_data, courseBaseId, groupName, studentAccountNumber);
                                if (rowNum_data > 0) // If data for target row exists
                                {
                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Qualitative;

                                        // First Partial
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_FirstPartial;
                                            if (evaluationSet.Count > 0) // This evaluation set is not used during certain terms.
                                            {
                                                int startNum = ((ColumnRange)(columnSet.QlFirstPartialColumnRange)).StartColumnLetter.ToColumnNum();
                                                int endNum = ((ColumnRange)(columnSet.QlFirstPartialColumnRange)).EndColumnLetter.ToColumnNum();
                                                for (int colNum = startNum; colNum <= endNum; colNum++)
                                                {
                                                    var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                    bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                    int intEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue : EvaluationValueToInt(evaluationValue);
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = intEvaluationValue;
                                                    DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[colNum - startNum], intEvaluationValue);
                                                }
                                            }
                                        }

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_Midterm;

                                            int startNum = columnSet.QlMidtermColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet.QlMidtermColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                int intEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue : EvaluationValueToInt(evaluationValue);
                                                worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? intEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[colNum - startNum], intEvaluationValue);
                                            }
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_Final;

                                            int startNum = columnSet.QlFinalsColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet.QlFinalsColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                int intEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue : EvaluationValueToInt(evaluationValue);
                                                worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? intEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[colNum - startNum], intEvaluationValue);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner6>: " + ex1.Message);
                            }
                        }
                    }

                    // Apply all modifications to file and upload it to Sharepoint
                    targetDocument.Save();
                    container.SharePointConnectionManager_EvaluationFiles.UploadFile(targetFilePath, _sharePointSaveFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("At " + _dataDocument.File.Name + ": " + ex.Message);
            }
        }

        private void UpdateNonEnglishEvaluationFile(ExcelPackage _dataDocument, Semester _semester, string _localSaveFolderPath, string _sharePointSaveFolderPath, string _fileName)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string targetFilePath = _localSaveFolderPath + @"\" + _fileName;

                string columnSetName = cmbBx_columnSet_general.SelectedItem.ToString();
                var columnSet = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName).Value;

                using (var targetDocument = new ExcelPackage(new System.IO.FileInfo(targetFilePath)))
                {
                    var worksheets_data = _dataDocument.Workbook.Worksheets;
                    var worksheets_target = targetDocument.Workbook.Worksheets;

                    // Load data from the quantitative evaluation sheets
                    {
                        // Get the Worksheet instances.
                        ExcelWorksheet worksheet_data = worksheets_data.First();
                        ExcelWorksheet worksheet_target = worksheets_target.First();

                        int numOfColumns = GetNumberOfColumns(worksheet_target);

                        int headerRowsCount = 3;

                        // Loop through the data worksheet rows.
                        var rowCount_data = worksheet_data.Dimension.Rows;
                        List<RowInfo> rowInfos_data = new List<RowInfo>();
                        // Ignore the first 3 rows, which do not include evaluation data information.
                        for (int rowNum_data = headerRowsCount + 1; rowNum_data <= rowCount_data; rowNum_data++)
                        {
                            try
                            {
                                string rowId = worksheet_data.Cells[rowNum_data, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_data.Cells[rowNum_data, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_data.Cells[rowNum_data, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_data.Cells[rowNum_data, "F".ToColumnNum()].Value?.ToString();

                                rowInfos_data.Add(new RowInfo(courseBaseId, groupName, studentAccountNumber, rowNum_data));
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner1>: " + ex1.Message);
                            }
                        }

                        // Loop through the target worksheet rows.
                        var rowCount_target = worksheet_target.Dimension.Rows;
                        for (int rowNum_target = headerRowsCount + 1; rowNum_target <= rowCount_target; rowNum_target++)
                        {
                            try
                            {
                                string rowId = worksheet_target.Cells[rowNum_target, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_target.Cells[rowNum_target, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_target.Cells[rowNum_target, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_target.Cells[rowNum_target, "F".ToColumnNum()].Value?.ToString();

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);
                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId = courseEntry.Key;
                                Course course = courseEntry.Value;
                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == Convert.ToInt32(studentAccountNumber));
                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                var studentInfo = group.StudentInfos.First(x => x.Student == student);

                                int rowNum_data = GetRowNumber(rowInfos_data, courseBaseId, groupName, studentAccountNumber);
                                if (rowNum_data > 0) // If data for target row exists
                                {
                                    int colNum_attendance = "H".ToColumnNum();
                                    var attendance = worksheet_data.Cells[rowNum_data, colNum_attendance].Value;
                                    if (attendance != null)
                                        worksheet_target.Cells[rowNum_target, colNum_attendance].Value = attendance;

                                    int colNum_comments = "I".ToColumnNum();
                                    var comments = worksheet_data.Cells[rowNum_data, colNum_comments].Value;
                                    if (comments != null)
                                        worksheet_target.Cells[rowNum_target, colNum_comments].Value = comments;

                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

                                        // First Partial
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_FirstPartial;
                                            if (evaluationSet.Count > 0) // This evaluation set is not used during certain terms.
                                            {
                                                int startNum = ((ColumnRange)(columnSet.QntFirstPartialColumnRange)).StartColumnLetter.ToColumnNum();
                                                int endNum = ((ColumnRange)(columnSet.QntFirstPartialColumnRange)).EndColumnLetter.ToColumnNum();
                                                for (int colNum = startNum; colNum <= endNum; colNum++)
                                                {
                                                    var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                    bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                    decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                    if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                        worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                    DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                                }

                                                int colNum_absence = endNum + 2;
                                                var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                                if (absence != null)
                                                    worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                                else
                                                    worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                            }
                                        }

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Midterm;

                                            int startNum = columnSet.QntMidtermColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet.QntMidtermColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                            }

                                            int colNum_absence = endNum + 2;
                                            var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                            if (absence != null)
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                            else
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Final;

                                            int startNum = columnSet.QntFinalsColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet.QntFinalsColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[colNum - startNum], decimalEvaluationValue);
                                            }

                                            int colNum_absence = endNum + 2;
                                            var absence = worksheet_data.Cells[rowNum_data, colNum_absence].Value;
                                            if (absence != null)
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = absence;
                                            else
                                                worksheet_target.Cells[rowNum_target, colNum_absence].Value = 0;
                                        }

                                        // Additional
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Additional;

                                            List<string> columnNames = columnSet.QntAdditionalColumns;
                                            int evaluationIndex = 0;
                                            foreach (var columnName in columnNames)
                                            {
                                                int colNum = columnName.ToColumnNum();
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                decimal decimalEvaluationValue = invalidEvaluationValue ? evaluationSet[evaluationIndex].Criterion.Min : EvaluationValueToDecimal(evaluationValue);
                                                if (!IsEquationValue(worksheet_target.Cells[rowNum_target, colNum].Value))
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? decimalEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[evaluationIndex], decimalEvaluationValue);

                                                evaluationIndex++;
                                            }
                                        }
                                    }

                                    // Extra columns
                                    {
                                        int colNum_passFailed = numOfColumns - 2;
                                        var passFailed = worksheet_data.Cells[rowNum_data, colNum_passFailed].Value;
                                        if (passFailed != null)
                                            worksheet_target.Cells[rowNum_target, colNum_passFailed].Value = passFailed;

                                        int colNum_recommendation = numOfColumns - 1;
                                        var recommendation = worksheet_data.Cells[rowNum_data, colNum_recommendation].Value;
                                        if (recommendation != null)
                                            worksheet_target.Cells[rowNum_target, colNum_recommendation].Value = recommendation;

                                        int colNum_finalComments = numOfColumns;
                                        var finalComments = worksheet_data.Cells[rowNum_data, colNum_finalComments].Value;
                                        if (finalComments != null)
                                            worksheet_target.Cells[rowNum_target, colNum_finalComments].Value = finalComments;
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner2>: " + ex1.Message);
                            }
                        }
                    }


                    // Load data from the qualitative evaluation sheets
                    {
                        // Get the Worksheet instances.
                        ExcelWorksheet worksheet_data = worksheets_data[1];
                        ExcelWorksheet worksheet_target = worksheets_target[1];

                        int headerRowsCount = 5;

                        // Loop through the data worksheet rows.
                        var rowCount_data = worksheet_data.Dimension.Rows;
                        List<RowInfo> rowInfos_data = new List<RowInfo>();
                        // Ignore the first 5 rows, which do not include evaluation data information.
                        for (int rowNum_data = headerRowsCount + 1; rowNum_data <= rowCount_data; rowNum_data++)
                        {
                            try
                            {
                                string rowId = worksheet_data.Cells[rowNum_data, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_data.Cells[rowNum_data, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_data.Cells[rowNum_data, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_data.Cells[rowNum_data, "F".ToColumnNum()].Value?.ToString();

                                rowInfos_data.Add(new RowInfo(courseBaseId, groupName, studentAccountNumber, rowNum_data));
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner3>: " + ex1.Message);
                            }
                        }

                        // Loop through the target worksheet rows.
                        var rowCount_target = worksheet_target.Dimension.Rows;
                        for (int rowNum_target = headerRowsCount + 1; rowNum_target <= rowCount_target; rowNum_target++)
                        {
                            try
                            {
                                string rowId = worksheet_target.Cells[rowNum_target, "A".ToColumnNum()].Value?.ToString();
                                if (string.IsNullOrEmpty(rowId))
                                    break;

                                int courseBaseId = Convert.ToInt32(worksheet_target.Cells[rowNum_target, "C".ToColumnNum()].Value?.ToString());
                                string groupName = worksheet_target.Cells[rowNum_target, "E".ToColumnNum()].Value?.ToString();
                                string studentAccountNumber = worksheet_target.Cells[rowNum_target, "F".ToColumnNum()].Value?.ToString();

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);
                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId = courseEntry.Key;
                                Course course = courseEntry.Value;
                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == Convert.ToInt32(studentAccountNumber));
                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                var studentInfo = group.StudentInfos.First(x => x.Student == student);

                                int rowNum_data = GetRowNumber(rowInfos_data, courseBaseId, groupName, studentAccountNumber);
                                if (rowNum_data > 0) // If data for target row exists
                                {
                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Qualitative;

                                        // First Partial
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_FirstPartial;
                                            if (evaluationSet.Count > 0) // This evaluation set is not used during certain terms.
                                            {
                                                int startNum = ((ColumnRange)(columnSet.QlFirstPartialColumnRange)).StartColumnLetter.ToColumnNum();
                                                int endNum = ((ColumnRange)(columnSet.QlFirstPartialColumnRange)).EndColumnLetter.ToColumnNum();
                                                for (int colNum = startNum; colNum <= endNum; colNum++)
                                                {
                                                    var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                    bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                    int intEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue : EvaluationValueToInt(evaluationValue);
                                                    worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? intEvaluationValue : evaluationValue;
                                                    DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[colNum - startNum], intEvaluationValue);
                                                }
                                            }
                                        }

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_Midterm;

                                            int startNum = columnSet.QlMidtermColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet.QlMidtermColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                int intEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue : EvaluationValueToInt(evaluationValue);
                                                worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? intEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[colNum - startNum], intEvaluationValue);
                                            }
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_Final;

                                            int startNum = columnSet.QlFinalsColumnRange.StartColumnLetter.ToColumnNum();
                                            int endNum = columnSet.QlFinalsColumnRange.EndColumnLetter.ToColumnNum();
                                            for (int colNum = startNum; colNum <= endNum; colNum++)
                                            {
                                                var evaluationValue = worksheet_data.Cells[rowNum_data, colNum].Value;
                                                bool invalidEvaluationValue = IsInvalidEvaluationValue(evaluationValue);
                                                int intEvaluationValue = invalidEvaluationValue ? evaluationSet[colNum - startNum].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue : EvaluationValueToInt(evaluationValue);
                                                worksheet_target.Cells[rowNum_target, colNum].Value = invalidEvaluationValue ? intEvaluationValue : evaluationValue;
                                                DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[colNum - startNum], intEvaluationValue);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show("At " + _dataDocument.File.Name + " <Inner4>: " + ex1.Message);
                            }
                        }
                    }

                    // Apply all modifications to file and upload it to Sharepoint
                    targetDocument.Save();
                    container.SharePointConnectionManager_EvaluationFiles.UploadFile(targetFilePath, _sharePointSaveFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("At " + _dataDocument.File.Name + ": " + ex.Message);
            }
        }

        private bool IsInvalidEvaluationValue(object _evaluationValue) { return (string.IsNullOrEmpty(_evaluationValue?.ToString()) || !_evaluationValue.IsNumeric()); }
        private bool IsEquationValue(object _evaluationValue) { return !(string.IsNullOrEmpty(_evaluationValue?.ToString())) && _evaluationValue.ToString().Contains("="); }

        private decimal EvaluationValueToDecimal(object _evaluationValue) { return Convert.ToDecimal(_evaluationValue.ToString().Replace(',', '.')); }
        private int EvaluationValueToInt(object _evaluationValue) { return Convert.ToInt32(_evaluationValue.ToString().Replace(',', '.')); }

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

        private eCourseCategory CourseCategoryFromString(string _string)
        {
            foreach (eCourseCategory category in Enum.GetValues(typeof(eCourseCategory)))
            {
                if (_string.Contains(CourseCategoryToString(category)))
                    return category;
            }

            return default;
        }

        private int GetNumberOfColumns(ExcelWorksheet worksheet)
        {
            int columnIndex = 0;
            foreach (var column in worksheet.Columns)
            {
                if (worksheet.Cells[1, columnIndex + 1].Value == null)
                    return columnIndex;

                columnIndex++;
            }

            return columnIndex;
        }

        private int GetRowNumber(List<RowInfo> _infos, int _courseBaseId, string _groupName, string _studentAccountNumber)
        {
            RowInfo rowInfo = _infos.FirstOrDefault(x => x.courseBaseId == _courseBaseId
                                                    && x.GroupName == _groupName
                                                    && x.StudentAccountNumber == _studentAccountNumber);
            if (rowInfo == null)
                return -1;
            else
                return rowInfo.RowNum;
        }

        private class RowInfo
        {
            public RowInfo(int _courseBaseId, string _groupName, string _studentAccountNumber, int _rowNum)
            {
                courseBaseId = _courseBaseId;
                GroupName = _groupName;
                StudentAccountNumber = _studentAccountNumber;
                RowNum = _rowNum;
            }

            public int courseBaseId { get; private set; }
            public string GroupName { get; private set; }
            public string StudentAccountNumber { get; private set; }
            public int RowNum { get; private set; }
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
