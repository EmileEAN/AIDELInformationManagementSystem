using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    public partial class StudentsComparisonForm : Form
    {
        public StudentsComparisonForm()
        {
            InitializeComponent();
        }

        private void StudentsComparisonForm_Load(object sender, EventArgs e)
        {
            InitializeStudentsTable(dgv_availableStudents);
            InitializeStudentsTable(dgv_comparingStudents);

            RefreshStudentsTable(dgv_availableStudents, DataContainer.Instance.StudentList);
        }

        #region Component Events
        private void txtBx_filter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_AvailableStudents();
        }

        private void btn_addStudent_Click(object sender, EventArgs e)
        {
            if (dgv_availableStudents.SelectedRows.Count == 0)
                return;

            dgv_availableStudents.SelectedRows[0].MoveTo(dgv_comparingStudents);
        }

        private void btn_removeStudent_Click(object sender, EventArgs e)
        {
            if (dgv_comparingStudents.SelectedRows.Count == 0)
                return;

            dgv_comparingStudents.SelectedRows[0].MoveTo(dgv_availableStudents);
        }

        private void btn_importExamData_Click(object sender, EventArgs e)
        {
            //if (opnFlDlg_importingFile.ShowDialog() == DialogResult.OK)
            //{
            //    DialogResult dialogResult = MessageBox.Show("Would you really like to import exam data?", "Caution!", MessageBoxButtons.YesNo);
            //    if (dialogResult == DialogResult.Yes)
            //        DocumentLoader.LoadExamDataFromExcel(opnFlDlg_importingFile.FileName);
            //}
        }
        #endregion

        #region Component Initialization
        private void InitializeStudentsTable(DataGridView _dgv)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;
                columns.Add("AccountNumber", "Account Number");
                columns.Add("Name", "Name");
                columns.Add("QuantitiaveEvaluation", "Quantitative Evaluation");
                columns.Add("QualitiaveEvaluation", "Qualitative Evaluation");
                foreach (var examType in DataContainer.Instance.ExamTypeDictionary.Values)
                {
                    columns.Add(examType.ToString(), examType.ToString());
                }
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshStudentsTable(DataGridView _dgv, List<Student> _studentList = null)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_studentList == null)
                return;

            DataContainer container = DataContainer.Instance;
            var examTypes = container.ExamTypeDictionary.Values.ToList();
            int numOfExamTypes = examTypes.Count;
            var exams = container.NonInstitutionalExam_InternalDictionary.Values;

            foreach (var student in _studentList)
            {
                var name = student.Name;

                var courseGroupDictionary = container.CourseGroupDictionary;
                var relatedGroups = courseGroupDictionary.Where(x => x.Value.StudentInfos.Any(y => y.Student == student)).Select(x => x.Value).ToList();
                decimal quantitativeEvaluationPointAverage = 0;
                decimal qualitativeEvaluationPointAverage = 0;
                int numOfRelatedGroups = relatedGroups.Count;
                foreach (var group in relatedGroups)
                {
                    quantitativeEvaluationPointAverage += EvaluationCalculator.QuantitativeEvaluationPointAverage(group, student);
                    qualitativeEvaluationPointAverage += EvaluationCalculator.QualitativeEvaluationPointAverage(group, student);
                }
                quantitativeEvaluationPointAverage /= numOfRelatedGroups;
                qualitativeEvaluationPointAverage /= numOfRelatedGroups;

                int[] examHighestScores = new int[numOfExamTypes];
                //foreach (var exam in exams.Where(x => x.Examinee == student))
                //{
                //    int examTypeIndex = examTypes.IndexOf(exam.Type);
                //    int currentScore = examHighestScores[examTypeIndex];
                //    examHighestScores[examTypeIndex] = (currentScore < exam.Score) ? exam.Score : currentScore;
                //}

                var row = new string[4 + numOfExamTypes];
                {
                    row[0] = student.AccountNumber.ToString();
                    row[1] = name.FirstName + " " + name.PaternalSurname;
                    try { row[2] = Math.Round(quantitativeEvaluationPointAverage, 3, MidpointRounding.AwayFromZero).ToString("0.###"); } catch { row[2] = ""; }
                    try { row[3] = Math.Round(qualitativeEvaluationPointAverage, 3, MidpointRounding.AwayFromZero).ToString("0.###"); } catch { row[3] = ""; }
                    for (int i = 0; i < examHighestScores.Length; i++)
                    {
                        int score = examHighestScores[i];
                        row[4 + i] = (score != default) ? score.ToString() : "";
                    }
                }

                rows.Add(row);
            }
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_AvailableStudents()
        {
            string filterText = txtBx_filter.Text;

            var studentListForComparison = TableToList(dgv_comparingStudents);

            var fullStudentList = DataContainer.Instance.StudentList;
            var studentListNotForComparison = fullStudentList.Except(studentListForComparison).ToList();
            var targetList = string.IsNullOrWhiteSpace(filterText) ? studentListNotForComparison : studentListNotForComparison.Where(x => x.AccountNumber.ToString().Contains(filterText) || x.Name.ToString().Contains(filterText)).ToList();
            RefreshStudentsTable(dgv_availableStudents, targetList);
        }
        #endregion

        #region Shared
        private List<Student> TableToList(DataGridView _dgv)
        {
            List<Student> result = new List<Student>();

            try
            {
                var studentList = DataContainer.Instance.StudentList;
                foreach (DataGridViewRow row in _dgv.Rows)
                {
                    var cells = row.Cells;
                    int accountNumber = Convert.ToInt32((string)(cells["AccountNumber"].Value));

                    Student student = studentList.First(x => x.AccountNumber == accountNumber);

                    result.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to convert table data to list! " + ex.Message);
            }

            return result;
        }

        private class RowComparer : System.Collections.IComparer
        {
            private static int m_sortOrderModifier = 1;

            public RowComparer(SortOrder _sortOrder)
            {
                if (_sortOrder == SortOrder.Descending)
                    m_sortOrderModifier = -1;
                else if (_sortOrder == SortOrder.Ascending)
                    m_sortOrderModifier = 1;
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow dataGridViewRow1 = (DataGridViewRow)x;
                DataGridViewRow dataGridViewRow2 = (DataGridViewRow)y;

                // Try to sort based on the Value column.
                int compareResult = Convert.ToInt32(dataGridViewRow1.Cells[0].Value.ToString())
                                            .CompareTo(Convert.ToInt32(dataGridViewRow2.Cells[0].Value.ToString()));

                return compareResult * m_sortOrderModifier;
            }
        }
        #endregion

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
