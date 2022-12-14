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
    public partial class QuantitativeEvaluationForm : Form
    {
        public QuantitativeEvaluationForm()
        {
            InitializeComponent();
        }

        private void QuantitativeEvaluationForm_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var course = group.Course;
            var semester = course.Semester;
            var courseBase = course.Base;
            lbl_student.Text = "(" + student.AccountNumber.ToString() + ") " + student.Name.ToString();
            lbl_course.Text = courseBase.ToString() + " " + semester.ToString() + " Group " + group.Name;

            InitializeViewTypeComboBox();

            RefreshMainViewTable();
            dgv_main.ClearSelection();
            dgv_main.SelectionChanged += (_sender, _e) => dgv_main.ClearSelection();
        }

        private bool m_isInitializingDgv = false;


        #region Component Events
        private void cmbBx_view_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshMainViewTable();
        }

        private void dgv_main_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_isInitializingDgv)
                return;

            try
            {
                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;
                var column = dgv_main.Columns[columnIndex];
                if (rowIndex >= 0) // If it is not the header cell
                {
                    if (column.Name == "Evaluation")
                    {
                        var evaluation = CellsToEvaluation(dgv_main.Rows[rowIndex].Cells);
                        var criterion = evaluation.Criterion;
                        decimal min = criterion.Min;
                        decimal max = criterion.Max;

                        var cell = dgv_main[columnIndex, rowIndex];

                        decimal value = Convert.ToDecimal((string)(cell.Value));
                        decimal newValue = (value < min) ? min : ((value > max) ? max : value);
                        cell.Value = newValue.ToString();

                        DocumentLoader.EditQuantitativeEvaluationInCSV(evaluation, newValue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change evaluation value! " + ex.Message);
            }


        }
        #endregion

        #region Component Initialization
        private void InitializeViewTypeComboBox()
        {
            // Set items
            var items = cmbBx_view.Items;
            items.Clear();

            items.Add("First Partial");
            items.Add("Midterm");
            items.Add("Finals");
            items.Add("Additional");
            items.Add("All");

            cmbBx_view.SelectedIndex = 0;
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            m_isInitializingDgv = true;

            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var studentInfo = group.StudentInfos.First(x => x.Student == student);
            var evaluationSetCollection = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

            var viewType = cmbBx_view.SelectedItem.ToString();

            var columns = dgv_main.Columns;
            columns.Clear();
            if (viewType == "All")
            {
                columns.Add("Type", "Type");
                var typeColumn = columns["Type"];
                typeColumn.ReadOnly = true;
                typeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }    

            columns.Add("Criterion", "Criterion");
            var criterionColumn = columns["Criterion"];
            criterionColumn.ReadOnly = true;
            criterionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            columns.Add("Evaluation", "Evaluation");
            var evaluationColumn = columns["Evaluation"];
            evaluationColumn.ReadOnly = false;
            evaluationColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            var rows = dgv_main.Rows;
            switch (viewType)
            {
                case "First Partial":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial)
                    {
                        AddEvaluationRow(evaluation);
                    }
                    break;

                case "Midterm":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Midterm)
                    {
                        AddEvaluationRow(evaluation);
                    }
                    break;

                case "Finals":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Final)
                    {
                        AddEvaluationRow(evaluation);
                    }
                    break;

                case "Additional":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Additional)
                    {
                        AddEvaluationRow(evaluation);
                    }
                    break;

                case "All":
                    {
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial)
                        {
                            AddEvaluationRow(evaluation, "First Partial");
                        }
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Midterm)
                        {
                            AddEvaluationRow(evaluation, "Midterm");
                        }
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Final)
                        {
                            AddEvaluationRow(evaluation, "Finals");
                        }
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Additional)
                        {
                            AddEvaluationRow(evaluation, "Additional");
                        }
                    }
                    break;
            }

            m_isInitializingDgv = false;
        }

        private void AddEvaluationRow(QuantitativeEvaluation _evaluation, string _type = "")
        {
            if (_type == "")
                dgv_main.Rows.Add(new string[] { _evaluation.Criterion.String, _evaluation.Value.ToString() });
            else
                dgv_main.Rows.Add(new string[] { _type, _evaluation.Criterion.String, _evaluation.Value.ToString() });
        }
        #endregion

        #region Data Validation
        private QuantitativeEvaluation CellsToEvaluation(DataGridViewCellCollection _cells)
        {
            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var studentInfo = group.StudentInfos.First(x => x.Student == student);
            var evaluationSetCollection = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

            string criterionString = (string)(_cells["Criterion"].Value);

            List<QuantitativeEvaluation> evaluationSet = null;
            switch (cmbBx_view.SelectedItem.ToString())
            {
                case "First Partial":
                    evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial;
                    break;

                case "Midterm":
                    evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Midterm;
                    break;

                case "Finals":
                    evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Final;
                    break;

                case "Additional":
                    evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Additional;
                    break;

                case "All":
                    switch ((string)(_cells["Type"].Value))
                    {
                        case "First Partial":
                            evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial;
                            break;

                        case "Midterm":
                            evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Midterm;
                            break;

                        case "Finals":
                            evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Final;
                            break;

                        case "Additional":
                            evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Additional;
                            break;
                    }
                    break;
            }

            var evaluation = evaluationSet.First(x => x.Criterion.String == criterionString);

            return evaluation;
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
    }
}
