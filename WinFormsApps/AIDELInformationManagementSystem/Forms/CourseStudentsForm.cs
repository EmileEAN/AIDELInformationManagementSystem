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
    public partial class CourseStudentsForm: Form
    {
        public CourseStudentsForm()
        {
            InitializeComponent();
        }

        private void CourseStudentsForm_Load(object sender, EventArgs e)
        {
            m_group = DataContainer.Instance.SelectedGroup;
            var course = m_group.Course;
            var semester = course.Semester;
            var courseBase = course.Base;
            var faculty = m_group.AssignedFaculty;
            lbl_course.Text = courseBase.ToString() + " " + semester.ToString() + " Group " + m_group.Name;
            lbl_faculty.Text = "Faculty: " + ((faculty != null) ? ("(" + faculty.AccountNumber.ToString() + ") " + faculty.Name.ToString()) : "Not Assigned");

            m_viewMode = eViewMode.OverallAveragePoints;

            InitializeMainViewTable();

            RefreshMainViewTable();
            dgv_main.ClearSelection();
            dgv_main.SelectionChanged += (_sender, _e) => dgv_main.ClearSelection();
        }

        private CourseGroup m_group;
        private eViewMode m_viewMode;

        #region Component Events
        private void txtBx_filter_main_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Main();
        }

        private void dgv_main_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            var column = dgv.Columns[columnIndex];
            if (column is DataGridViewButtonColumn && rowIndex >= 0) // If it is a button column and it is not the header cell
            {
                if (column.DataPropertyName == "QuantitativeEvaluation")
                {
                    var cells = dgv.Rows[rowIndex].Cells;

                    int accountNumber = Convert.ToInt32((string)(cells["AccountNumber"].Value));
                    DataContainer container = DataContainer.Instance;
                    container.SelectedStudent = container.StudentList.First(x => x.AccountNumber == accountNumber);

                    this.LogAndSwitchTo(new QuantitativeEvaluationForm());
                }

                if (column.DataPropertyName == "QualitativeEvaluation")
                {
                    var cells = dgv.Rows[rowIndex].Cells;

                    int accountNumber = Convert.ToInt32((string)(cells["AccountNumber"].Value));
                    DataContainer container = DataContainer.Instance;
                    container.SelectedStudent = container.StudentList.First(x => x.AccountNumber == accountNumber);

                    this.LogAndSwitchTo(new QualitativeEvaluationForm());
                }
            }
        }

        private void btn_switchColumns_Click(object sender, EventArgs e)
        {
            switch (m_viewMode)
            {
                default: // case eViewMode.OverallAveragePoints
                    m_viewMode = eViewMode.QuantitativeAveragePoints;
                    break;

                case eViewMode.QuantitativeAveragePoints:
                    m_viewMode = eViewMode.QualitativeAveragePoints;
                    break;

                case eViewMode.QualitativeAveragePoints:
                    m_viewMode = eViewMode.OverallAveragePoints;
                    break;
            }

            InitializeMainViewTable();
            RefreshMainViewTable();
        }
        #endregion 

        #region Component Initialization
        private void InitializeMainViewTable()
        {
            // Clear columns
            dgv_main.Columns.Clear();

            // Set columns
            switch (m_viewMode)
            {
                default: // case eViewMode.OverallAveragePoints
                    {
                        var columns = dgv_main.Columns;
                        columns.Add("AccountNumber", "Account Number");
                        columns.Add("FirstName", "First Name");
                        columns.Add("MiddleName", "Middle Name");
                        columns.Add("PaternalSurname", "Paternal Surname");
                        columns.Add("MaternalSurname", "Maternal Surname");

                        Font font = new Font("Palatino Linotype", 12F, GraphicsUnit.Pixel);

                        var quantitativeEvaluationColumn = new DataGridViewButtonColumn();
                        quantitativeEvaluationColumn.DataPropertyName = "QuantitativeEvaluation";
                        quantitativeEvaluationColumn.Name = "Quantitative Evaluation";
                        quantitativeEvaluationColumn.CellTemplate.Style.Font = font;
                        columns.Add(quantitativeEvaluationColumn);

                        var qualitativeEvaluationColumn = new DataGridViewButtonColumn();
                        qualitativeEvaluationColumn.DataPropertyName = "QualitativeEvaluation";
                        qualitativeEvaluationColumn.Name = "Qualitative Evaluation";
                        qualitativeEvaluationColumn.CellTemplate.Style.Font = font;
                        columns.Add(qualitativeEvaluationColumn);
                    }
                    break;

                case eViewMode.QuantitativeAveragePoints:
                    {
                        var columns = dgv_main.Columns;
                        columns.Add("AccountNumber", "Account Number");
                        columns.Add("Name", "Name");

                        List<string> criterionStrings = new List<string>();
                        var criteriaSet = m_group.Course.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation;
                        List<CriterionWeight_QuantitativeEvaluation> weightPerCriterion = new List<CriterionWeight_QuantitativeEvaluation>();
                        if (criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial != null)
                            weightPerCriterion.AddRange(criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Criteria.WeightPerCriterion);
                        weightPerCriterion.AddRange(criteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm.Criteria.WeightPerCriterion);
                        weightPerCriterion.AddRange(criteriaSet.CriteriaWeight_QuantitativeEvaluation_Final.Criteria.WeightPerCriterion);
                        foreach (var criterionWeight in weightPerCriterion)
                        {
                            string criterionString = criterionWeight.Criterion.String;
                            if (criterionStrings.Any(x => x == criterionString))
                                criterionStrings.Add(criterionString);
                        }
                        for (int i = 0; i < criterionStrings.Count; i++)
                        {
                            columns.Add("Criterion" + i.ToString(), criterionStrings[i]);
                        }

                        var additionalCriterionReferences = m_group.Course.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.CriteriaWeight_QuantitativeEvaluation_Additional.Criteria.WeightPerCriterion;
                        for (int i = 0; i < additionalCriterionReferences.Count; i++)
                        {
                            columns.Add("AdditionalCriterion" + i.ToString(), additionalCriterionReferences[i].Criterion.String);
                        }
                    }
                    break;

                case eViewMode.QualitativeAveragePoints:
                    {
                        var columns = dgv_main.Columns;
                        columns.Add("AccountNumber", "Account Number");
                        columns.Add("Name", "Name");

                        List<string> criterionStrings = new List<string>();
                        var criteriaSet = m_group.Course.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation;
                        List<Criterion_QualitativeEvaluation> criteria = new List<Criterion_QualitativeEvaluation>();
                        if (criteriaSet.Criteria_QualitativeEvaluation_FirstPartial != null)
                            criteria.AddRange(criteriaSet.Criteria_QualitativeEvaluation_FirstPartial.CriterionList);
                        criteria.AddRange(criteriaSet.Criteria_QualitativeEvaluation_Midterm.CriterionList);
                        criteria.AddRange(criteriaSet.Criteria_QualitativeEvaluation_Final.CriterionList);
                        foreach (var criterion in criteria)
                        {
                            string criterionString = criterion.String;
                            if (criterionStrings.Any(x => x == criterionString))
                                criterionStrings.Add(criterionString);
                        }
                        for (int i = 0; i < criterionStrings.Count; i++)
                        {
                            columns.Add("Criterion" + i.ToString(), criterionStrings[i]);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            string filterText = txtBx_filter_main.Text;

            DataContainer container = DataContainer.Instance;

            var studentInfos = m_group.StudentInfos;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? studentInfos : studentInfos.Where(x => x.Student.AccountNumber.ToString().Contains(filterText)
                                                                                                                || x.Student.Name.ToString().Contains(filterText));
            int selectingRowIndex = -1;
            var columns = dgv_main.Columns;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    var group = container.SelectedGroup;
                    var student = element.Student;
                    var fullEvaluation = element.FullEvaluation;
                    var studentName = student.Name;
                    string[] row = null;

                    List<Color> evaluationColors = new List<Color>();
                    switch (m_viewMode)
                    {
                        default: // case eViewMode.OverallAveragePoints
                            var quantitativeAverage = EvaluationCalculator.QuantitativeEvaluationPointAverage(group, student);
                            var qualitativeAverage = EvaluationCalculator.QualitativeEvaluationPointAverage(group, student);
                            row = new string[] { student.AccountNumber.ToString(), studentName.FirstName, studentName.MiddleName, studentName.PaternalSurname, studentName.MaternalSurname, quantitativeAverage.ToString(), qualitativeAverage.ToString() };
                            break;

                        case eViewMode.QuantitativeAveragePoints:
                            {
                                int columnCount = dgv_main.ColumnCount;
                                row = new string[columnCount];
                                row[0] = student.AccountNumber.ToString();
                                row[1] = studentName.ToString();

                                var evaluationSetCollection = fullEvaluation.EvaluationSetCollection_Quantitative;
                                var evaluations_firstPartial = evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial;
                                var evaluations_midterm = evaluationSetCollection.QuantitativeEvaluationSet_Midterm;
                                var evaluations_finals = evaluationSetCollection.QuantitativeEvaluationSet_Final;
                                var evaluations_additional = evaluationSetCollection.QuantitativeEvaluationSet_Additional;
                                for (int i = 2; i < columnCount; i++)
                                {
                                    string criterionString = columns[i].HeaderText;

                                    decimal averagePoints = 0;
                                    int criterionCount = 0;

                                    // First Partial
                                    if (evaluations_firstPartial != null)
                                    {
                                        var criterion = evaluations_firstPartial.FirstOrDefault(x => x.Criterion.String == criterionString);
                                        if (criterion != null)
                                        {
                                            averagePoints += criterion.Value;
                                            criterionCount++;
                                        }
                                    }

                                    // Midterm
                                    {
                                        var criterion = evaluations_midterm.FirstOrDefault(x => x.Criterion.String == criterionString);
                                        if (criterion != null)
                                        {
                                            averagePoints += criterion.Value;
                                            criterionCount++;
                                        }
                                    }

                                    // Finals
                                    {
                                        var criterion = evaluations_finals.FirstOrDefault(x => x.Criterion.String == criterionString);
                                        if (criterion != null)
                                        {
                                            averagePoints += criterion.Value;
                                            criterionCount++;
                                        }
                                    }

                                    // Addititonal
                                    {
                                        var criterion = evaluations_additional.FirstOrDefault(x => x.Criterion.String == criterionString);
                                        if (criterion != null)
                                        {
                                            averagePoints += criterion.Value;
                                            criterionCount++;
                                        }
                                    }

                                    row[i] = (averagePoints / criterionCount).ToString();
                                }
                            }
                            break;

                        case eViewMode.QualitativeAveragePoints:
                            {
                                int columnCount = dgv_main.ColumnCount;
                                row = new string[columnCount];
                                row[0] = student.AccountNumber.ToString();
                                row[1] = studentName.ToString();

                                var evaluationSetCollection = fullEvaluation.EvaluationSetCollection_Qualitative;
                                var evaluations_firstPartial = evaluationSetCollection.QualitativeEvaluations_FirstPartial;
                                var evaluations_midterm = evaluationSetCollection.QualitativeEvaluations_Midterm;
                                var evaluations_finals = evaluationSetCollection.QualitativeEvaluations_Final;
                                var baseCriterionReferences = m_group.Course.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Criteria_QualitativeEvaluation_FirstPartial.CriterionList;
                                for (int i = 2; i < columnCount; i++)
                                {
                                    int totalPoints = ((evaluations_firstPartial != null) ? evaluations_firstPartial[i - 2].Value : 0) + evaluations_midterm[i - 2].Value + evaluations_finals[i - 2].Value;
                                    decimal averagePoints = totalPoints / ((evaluations_firstPartial != null) ? 3.0m : 2.0m);
                                    int roundedAveragePoints = Convert.ToInt32(averagePoints); // Remove decimal points
                                    Color evaluationColor = baseCriterionReferences[i - 2].EvaluationColorSet.ColorFromNumericValue(roundedAveragePoints);
                                    evaluationColors.Add(evaluationColor);

                                    row[i] = averagePoints.ToString();
                                }
                            }
                            break;
                    }

                    rows.Add(row);

                    if (m_viewMode == eViewMode.QualitativeAveragePoints)
                    {
                        var lastRowIndex = rows.GetLastRow(DataGridViewElementStates.None);
                        var lastRow = dgv_main.Rows[lastRowIndex];

                        for (int i = 2; i < dgv_main.ColumnCount; i++)
                        {
                            lastRow.Cells[i].Style.BackColor = evaluationColors[i - 2];
                        }
                    }

                }
            }

            if (selectingRowIndex != -1)
                rows[selectingRowIndex].Selected = true;
            else if (rows.Count != 0)
                rows[rows.GetLastRow(DataGridViewElementStates.None)].Selected = true;
        }
        #endregion

        #region Shared
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

        private enum eViewMode
        {
            OverallAveragePoints,
            QuantitativeAveragePoints,
            QualitativeAveragePoints
        }
    }
}
