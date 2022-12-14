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
    public partial class StudentEvaluationsForm : Form
    {
        public StudentEvaluationsForm()
        {
            InitializeComponent();
        }

        private void StudentEvaluationsForm_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var course = group.Course;
            var semester = course.Semester;
            var courseBase = course.Base;
            lbl_student.Text = "(" + student.AccountNumber.ToString() + ") " + student.Name.ToString();
            lbl_course.Text = courseBase.ToString() + " " + semester.ToString() + " Group " + group.Name;

            InitializeViewTypeComboBox_Quantitative();
            InitializeViewTypeComboBox_Qualitative();

            RefreshViewTable_Quantitative();
            dgv_quantitative.ClearSelection();
            dgv_quantitative.SelectionChanged += (_sender, _e) => dgv_quantitative.ClearSelection();

            RefreshViewTable_Qualitative();
            dgv_qualitative.ClearSelection();
            dgv_qualitative.SelectionChanged += (_sender, _e) => dgv_qualitative.ClearSelection();

            rdBtn_quantitative.Checked = true;

            pnl_quantitative.Enabled = pnl_quantitative.Visible = true;
            pnl_qualitative.Enabled = pnl_qualitative.Visible = false;
        }

        private bool m_isInitializingDgv_quantitative = false;
        private bool m_isInitializingDgv_qualitative = false;

        #region Component Events
        private void cmbBx_view_quantitative_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshViewTable_Quantitative();
        }

        private void dgv_quantitative_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_isInitializingDgv_quantitative)
                return;

            try
            {
                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;
                var column = dgv_quantitative.Columns[columnIndex];
                if (rowIndex >= 0) // If it is not the header cell
                {
                    if (column.Name == "Evaluation")
                    {
                        var evaluation = CellsToEvaluation_Quantitative(dgv_quantitative.Rows[rowIndex].Cells);
                        var criterion = evaluation.Criterion;
                        decimal min = criterion.Min;
                        decimal max = criterion.Max;

                        var cell = dgv_quantitative[columnIndex, rowIndex];

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

        private void cmbBx_view_qualitative_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshViewTable_Qualitative();
        }

        private void dgv_qualitative_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_isInitializingDgv_qualitative)
                return;

            try
            {
                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;
                var column = dgv_qualitative.Columns[columnIndex];
                if (rowIndex >= 0) // If it is not the header cell
                {
                    if (column.Name == "Evaluation")
                    {
                        var evaluation = CellsToEvaluation_Qualitative(dgv_qualitative.Rows[rowIndex].Cells);
                        var valueColors = evaluation.Criterion.EvaluationColorSet.ValueColors;
                        ValueColor min = valueColors.Min();
                        ValueColor max = valueColors.Max();

                        var cell = dgv_qualitative[columnIndex, rowIndex];

                        int value = Convert.ToInt32((string)(cell.Value));
                        int newValue = (value < min.NumericValue) ? min.NumericValue : ((value > max.NumericValue) ? max.NumericValue : value);
                        cell.Value = newValue.ToString();
                        cell.Style.BackColor = evaluation.Criterion.EvaluationColorSet.ValueColors.First(x => x.NumericValue == newValue).Color;

                        DocumentLoader.EditQualitativeEvaluationInCSV(evaluation, newValue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change evaluation value! " + ex.Message);
            }
        }

        private void rdBtn_quantitative_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdBtn = sender as RadioButton;
            if (rdBtn != null)
            {
                pnl_quantitative.Enabled = pnl_quantitative.Visible = rdBtn.Checked;
            }
        }

        private void rdBtn_qualitative_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdBtn = sender as RadioButton;
            if (rdBtn != null)
            {
                pnl_qualitative.Enabled = pnl_qualitative.Visible = rdBtn.Checked;
            }
        }
        #endregion

        #region Component Initialization
        private void InitializeViewTypeComboBox_Quantitative()
        {
            // Set items
            var items = cmbBx_view_quantitative.Items;
            items.Clear();

            items.Add("First Partial");
            items.Add("Midterm");
            items.Add("Finals");
            items.Add("Additional");
            items.Add("All");

            cmbBx_view_quantitative.SelectedIndex = 0;
        }

        private void InitializeViewTypeComboBox_Qualitative()
        {
            // Set items
            var items = cmbBx_view_qualitative.Items;
            items.Clear();

            items.Add("First Partial");
            items.Add("Midterm");
            items.Add("Finals");
            items.Add("All");

            cmbBx_view_qualitative.SelectedIndex = 0;
        }
        #endregion

        #region Component Refreshment
        private void RefreshViewTable_Quantitative()
        {
            m_isInitializingDgv_quantitative = true;

            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var studentInfo = group.StudentInfos.First(x => x.Student == student);
            var evaluationSetCollection = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

            var viewType = cmbBx_view_quantitative.SelectedItem.ToString();

            var columns = dgv_quantitative.Columns;
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

            var rows = dgv_quantitative.Rows;
            switch (viewType)
            {
                case "First Partial":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial)
                    {
                        AddEvaluationRow_Quantitative(evaluation);
                    }
                    break;

                case "Midterm":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Midterm)
                    {
                        AddEvaluationRow_Quantitative(evaluation);
                    }
                    break;

                case "Finals":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Final)
                    {
                        AddEvaluationRow_Quantitative(evaluation);
                    }
                    break;

                case "Additional":
                    foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Additional)
                    {
                        AddEvaluationRow_Quantitative(evaluation);
                    }
                    break;

                case "All":
                    {
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial)
                        {
                            AddEvaluationRow_Quantitative(evaluation, "First Partial");
                        }
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Midterm)
                        {
                            AddEvaluationRow_Quantitative(evaluation, "Midterm");
                        }
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Final)
                        {
                            AddEvaluationRow_Quantitative(evaluation, "Finals");
                        }
                        foreach (var evaluation in evaluationSetCollection.QuantitativeEvaluationSet_Additional)
                        {
                            AddEvaluationRow_Quantitative(evaluation, "Additional");
                        }
                    }
                    break;
            }

            m_isInitializingDgv_quantitative = false;
        }

        private void RefreshViewTable_Qualitative()
        {
            m_isInitializingDgv_qualitative = true;

            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var studentInfo = group.StudentInfos.First(x => x.Student == student);
            var evaluationSetCollection = studentInfo.FullEvaluation.EvaluationSetCollection_Qualitative;

            var viewType = cmbBx_view_qualitative.SelectedItem.ToString();

            var columns = dgv_qualitative.Columns;
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

            var evaluationColumn = new DataGridViewComboBoxColumn();
            columns.Add(evaluationColumn);
            evaluationColumn.HeaderText = "Evaluation";
            evaluationColumn.Name = "Evaluation";
            evaluationColumn.FlatStyle = FlatStyle.Flat;
            evaluationColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            evaluationColumn.ReadOnly = false;
            evaluationColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            var rows = dgv_qualitative.Rows;
            switch (viewType)
            {
                case "First Partial":
                    foreach (var evaluation in evaluationSetCollection.QualitativeEvaluations_FirstPartial)
                    {
                        AddEvaluationRow_Qualitative(evaluation);
                    }
                    break;

                case "Midterm":
                    foreach (var evaluation in evaluationSetCollection.QualitativeEvaluations_Midterm)
                    {
                        AddEvaluationRow_Qualitative(evaluation);
                    }
                    break;

                case "Finals":
                    foreach (var evaluation in evaluationSetCollection.QualitativeEvaluations_Final)
                    {
                        AddEvaluationRow_Qualitative(evaluation);
                    }
                    break;

                case "All":
                    {
                        foreach (var evaluation in evaluationSetCollection.QualitativeEvaluations_FirstPartial)
                        {
                            AddEvaluationRow_Qualitative(evaluation, "First Partial");
                        }
                        foreach (var evaluation in evaluationSetCollection.QualitativeEvaluations_Midterm)
                        {
                            AddEvaluationRow_Qualitative(evaluation, "Midterm");
                        }
                        foreach (var evaluation in evaluationSetCollection.QualitativeEvaluations_Final)
                        {
                            AddEvaluationRow_Qualitative(evaluation, "Finals");
                        }
                    }
                    break;
            }

            m_isInitializingDgv_qualitative = false;
        }

        private void AddEvaluationRow_Quantitative(QuantitativeEvaluation _evaluation, string _type = "")
        {
            if (_type == "")
                dgv_quantitative.Rows.Add(new string[] { _evaluation.Criterion.String, _evaluation.Value.ToString() });
            else
                dgv_quantitative.Rows.Add(new string[] { _type, _evaluation.Criterion.String, _evaluation.Value.ToString() });
        }

        private void AddEvaluationRow_Qualitative(QualitativeEvaluation _evaluation, string _type = "")
        {
            int rowIndex;
            if (_type == "")
                rowIndex = dgv_qualitative.Rows.Add(new string[] { _evaluation.Criterion.String });
            else
                rowIndex = dgv_qualitative.Rows.Add(new string[] { _type, _evaluation.Criterion.String });

            var cell = ((DataGridViewComboBoxCell)(dgv_qualitative.Rows[rowIndex].Cells["Evaluation"]));
            var items = cell.Items;
            foreach (var valueColor in _evaluation.Criterion.EvaluationColorSet.ValueColors)
            {
                var itemIndex = items.Add(valueColor.NumericValue.ToString());
            }

            int initialValue = _evaluation.Value;
            cell.Value = initialValue.ToString();
            cell.Style.BackColor = _evaluation.Criterion.EvaluationColorSet.ValueColors.First(x => x.NumericValue == initialValue).Color;

        }
        #endregion

        #region Data Validation
        private QuantitativeEvaluation CellsToEvaluation_Quantitative(DataGridViewCellCollection _cells)
        {
            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var studentInfo = group.StudentInfos.First(x => x.Student == student);
            var evaluationSetCollection = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

            string criterionString = (string)(_cells["Criterion"].Value);

            List<QuantitativeEvaluation> evaluationSet = null;
            switch (cmbBx_view_quantitative.SelectedItem.ToString())
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

        private QualitativeEvaluation CellsToEvaluation_Qualitative(DataGridViewCellCollection _cells)
        {
            DataContainer container = DataContainer.Instance;
            var student = container.SelectedStudent;
            var group = container.SelectedGroup;
            var studentInfo = group.StudentInfos.First(x => x.Student == student);
            var evaluationSetCollection = studentInfo.FullEvaluation.EvaluationSetCollection_Qualitative;

            string criterionString = (string)(_cells["Criterion"].Value);

            List<QualitativeEvaluation> evaluationSet = null;
            switch (cmbBx_view_qualitative.SelectedItem.ToString())
            {
                case "First Partial":
                    evaluationSet = evaluationSetCollection.QualitativeEvaluations_FirstPartial;
                    break;

                case "Midterm":
                    evaluationSet = evaluationSetCollection.QualitativeEvaluations_Midterm;
                    break;

                case "Finals":
                    evaluationSet = evaluationSetCollection.QualitativeEvaluations_Final;
                    break;

                case "All":
                    switch ((string)(_cells["Type"].Value))
                    {
                        case "First Partial":
                            evaluationSet = evaluationSetCollection.QualitativeEvaluations_FirstPartial;
                            break;

                        case "Midterm":
                            evaluationSet = evaluationSetCollection.QualitativeEvaluations_Midterm;
                            break;

                        case "Finals":
                            evaluationSet = evaluationSetCollection.QualitativeEvaluations_Final;
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
