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
    public partial class QuantitativeEvaluationCriteriaForm: Form
    {
        public QuantitativeEvaluationCriteriaForm()
        {
            InitializeComponent();
        }

        private void QuantitativeEvaluationCriteriaForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeCriterionTable(dgv_criterion_add, true);
            InitializeCriterionTable(dgv_criterion_edit, true);
            InitializeCriterionTable(dgv_criterion_delete, false);

            InitializeCriteriaTable(dgv_criteria_view, false);
            InitializeCriteriaTable(dgv_criteria_add, true);
            InitializeCriteriaTable(dgv_criteria_edit, true);
            InitializeCriteriaTable(dgv_criteria_delete, false);

            RefreshMainViewTable();
            dgv_main.ClearSelection();

            List<Criterion_QuantitativeEvaluation> initialCriterionList = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.Values.ToList();
            RefreshCriterionTable(dgv_criterion_add, initialCriterionList);
            RefreshCriterionTable(dgv_criterion_edit, initialCriterionList);
            RefreshCriterionTable(dgv_criterion_delete, initialCriterionList);

            var permissiontType = DataContainer.Instance.CurrentUser.PermissionsType;
            if (permissiontType == ePermissionsType.ViewOnly || permissiontType == ePermissionsType.AssignedCoursesOnly)
            {
                tbCtrl_main.TabPages.Remove(tabPage2);
                tbCtrl_main.TabPages.Remove(tabPage3);
                tbCtrl_main.TabPages.Remove(tabPage4);
            }
        }

        private bool m_selectionChanged = false;
        private bool m_addingRows = false;
        private string m_selectedCriteriaName = string.Empty;
        private DataGridViewRow m_selectedRow;
        private DataGridViewCellCollection SelectedCells { get { return m_selectedRow?.Cells; } }

        private void tbCtrl_main_DrawItem(object _sender, DrawItemEventArgs _e)
        {
            TabPage page = tbCtrl_main.TabPages[_e.Index];
            Color color = Color.Black;
            switch (_e.Index)
            {
                case 0: // case 'View' tab
                    color = Color.FromArgb(102, 204, 255);
                    break;

                case 1: // case 'Add' tab
                    color = Color.FromArgb(102, 255, 102);
                    break;

                case 2: // case 'Edit' tab
                    color = Color.FromArgb(255, 255, 102);
                    break;

                case 3: // case 'Delete' tab
                    color = Color.FromArgb(255, 102, 102);
                    break;
            }
            _e.Graphics.FillRectangle(new SolidBrush(color), _e.Bounds);

            Rectangle paddedBounds = _e.Bounds;
            int yOffset = (_e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(_e.Graphics, page.Text, _e.Font, paddedBounds, page.ForeColor);
        }


        #region Component Events
        #region View Tab
        private void txtBx_filter_main_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Main();
        }

        private void dgv_main_SelectionChanged(object sender, EventArgs e)
        {
            if (m_addingRows)
                return;

            m_selectionChanged = true;

            if (dgv_main.SelectedRows.Count == 0)
            {
                m_selectedRow = null;
                m_selectedCriteriaName = String.Empty;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedCriteriaName = (string)(SelectedCells["Name"].Value);
            }

            FillViewTabComponents();
            FillEditTabComponents();
            FillDeleteTabComponents();
        }

        private void dgv_main_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!m_selectionChanged)
            {
                dgv_main.ClearSelection();
                m_selectionChanged = true;
            }
            else
                m_selectionChanged = false;
        }
        #endregion

        #region Add Tab
        private void txtBx_filter_add_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Add();
        }

        private void btn_addCriterion_add_Click(object sender, EventArgs e)
        {
            if (dgv_criterion_add.SelectedRows.Count == 0)
                return;

            int newRowIndex = dgv_criterion_add.SelectedRows[0].MoveTo(dgv_criteria_add);
            dgv_criteria_add.Rows[newRowIndex].Cells["Weight"].Value = numUpDwn_weight_add.Value.ToString();
        }

        private void btn_removeCriterion_add_Click(object sender, EventArgs e)
        {
            if (dgv_criteria_add.SelectedRows.Count == 0)
                return;

            dgv_criteria_add.SelectedRows[0].MoveTo(dgv_criterion_add, 2);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                if (DocumentLoader.AddQuantitativeEvaluationCriteriaToCSV(txtBx_name_add.Text, TableToList(dgv_criteria_add)) != default)
                {
                    MessageBox.Show("Added new criteria.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new criteria! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void txtBx_filter_edit_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Edit();
        }

        private void btn_addCriterion_edit_Click(object sender, EventArgs e)
        {
            if (dgv_criterion_edit.SelectedRows.Count == 0)
                return;

            int newRowIndex = dgv_criterion_edit.SelectedRows[0].MoveTo(dgv_criteria_edit);
            dgv_criteria_edit.Rows[newRowIndex].Cells["Weight"].Value = numUpDwn_weight_edit.Value.ToString();
        }

        private void btn_removeCriterion_edit_Click(object sender, EventArgs e)
        {
            if (dgv_criteria_edit.SelectedRows.Count == 0)
                return;

            dgv_criteria_edit.SelectedRows[0].MoveTo(dgv_criterion_edit, 2);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                Criteria_QuantitativeEvaluation selectedCriteria = DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == m_selectedCriteriaName).Value;

                if (DocumentLoader.EditQuantitativeEvaluationCriteriaInCSV(selectedCriteria, txtBx_name_edit.Text, TableToList(dgv_criteria_edit)))
                {
                    MessageBox.Show("Edited criteria successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit criteria! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCriteriaId = DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == m_selectedCriteriaName).Key;
                if (DocumentLoader.DeleteQuantitativeEvaluationCriteriaFromCSV(selectedCriteriaId))
                {
                    MessageBox.Show("Deleted criteria successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete criteria! Please try again.");
            }
        }
        #endregion
        #endregion

        #region Component Initialization
        private void InitializeMainViewTable()
        {
            // Set columns
            if (dgv_main.ColumnCount == 0)
            {
                var columns = dgv_main.Columns;
                columns.Add("Name", "Name");
            }
        }

        private void InitializeCriterionTable(DataGridView _dgv, bool _selectable)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;

                columns.Add("Criterion", "Criterion");
                columns.Add("ValueRange", "Value Range");

                if (!_selectable)
                    _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }

        private void InitializeCriteriaTable(DataGridView _dgv, bool _selectable)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;

                columns.Add("Criterion", "Criterion");
                columns.Add("ValueRange", "Value Range");
                columns.Add("Weight", "Weight");

                if (!_selectable)
                    _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }
        #endregion

        #region Component Fill
        private void FillViewTabComponents()
        {
            DataContainer container = DataContainer.Instance;
            Criteria_QuantitativeEvaluation selectedCriteria = container.QuantitativeEvaluationCriteriaDictionary.Values.FirstOrDefault(x => x.Name == m_selectedCriteriaName);

            if (SelectedCells != null)
                RefreshCriteriaTable(dgv_criteria_view, selectedCriteria.WeightPerCriterion);
            else
                RefreshCriteriaTable(dgv_criteria_view);
        }

        private void FillEditTabComponents()
        {
            DataContainer container = DataContainer.Instance;
            Criteria_QuantitativeEvaluation selectedCriteria = container.QuantitativeEvaluationCriteriaDictionary.Values.FirstOrDefault(x => x.Name == m_selectedCriteriaName);
            List<Criterion_QuantitativeEvaluation> fullCriterionList = container.QuantitativeEvaluationCriterionDictionary.Values.ToList();

            txtBx_filter_edit.Clear();

            if (SelectedCells != null)
            {
                txtBx_name_edit.Text = (string)(SelectedCells["Name"].Value);

                List<CriterionWeight_QuantitativeEvaluation> criterionWeightListInCriteria = selectedCriteria.WeightPerCriterion;
                List<Criterion_QuantitativeEvaluation> criterionListNotInCriteria = fullCriterionList.Except(criterionWeightListInCriteria.Select(x => x.Criterion)).ToList();

                RefreshCriterionTable(dgv_criterion_edit, criterionListNotInCriteria);
                RefreshCriteriaTable(dgv_criteria_edit, criterionWeightListInCriteria);
            }
            else
            {
                txtBx_name_edit.Clear();
                RefreshCriterionTable(dgv_criterion_edit, fullCriterionList);
                RefreshCriteriaTable(dgv_criteria_edit);
            }
        }

        private void FillDeleteTabComponents()
        {
            DataContainer container = DataContainer.Instance;
            Criteria_QuantitativeEvaluation selectedCriteria = container.QuantitativeEvaluationCriteriaDictionary.Values.FirstOrDefault(x => x.Name == m_selectedCriteriaName);
            List<Criterion_QuantitativeEvaluation> fullCriterionList = container.QuantitativeEvaluationCriterionDictionary.Values.ToList();

            if (SelectedCells != null)
            {
                txtBx_name_delete.Text = (string)(SelectedCells["Name"].Value);

                List<CriterionWeight_QuantitativeEvaluation> criterionWeightListInCriteria = selectedCriteria.WeightPerCriterion;
                List<Criterion_QuantitativeEvaluation> criterionListNotInCriteria = fullCriterionList.Except(criterionWeightListInCriteria.Select(x => x.Criterion)).ToList();

                RefreshCriterionTable(dgv_criterion_delete, criterionListNotInCriteria);
                RefreshCriteriaTable(dgv_criteria_delete, criterionWeightListInCriteria);
            }
            else
            {
                txtBx_name_delete.Clear();
                RefreshCriterionTable(dgv_criterion_delete, fullCriterionList);
                RefreshCriteriaTable(dgv_criteria_delete);
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }

        private void RefreshCriterionTable(DataGridView _dgv, List<Criterion_QuantitativeEvaluation> _criterionList = null)
        {           
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_criterionList == null)
                return;

            foreach (var criterion in _criterionList)
            {
                rows.Add(new string[] { criterion.String, criterion.Min.ToString() + " - " + criterion.Max.ToString() });
            }
        }

        private void RefreshCriteriaTable(DataGridView _dgv, List<CriterionWeight_QuantitativeEvaluation> _criterionWeightList = null)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_criterionWeightList == null)
                return;

            foreach (var criterionWeight in _criterionWeightList)
            {
                Criterion_QuantitativeEvaluation criterion = criterionWeight.Criterion;
                rows.Add(new string[] { criterion.String, criterion.Min.ToString() + " - " + criterion.Max.ToString(), criterionWeight.Weight.ToString() });
            }
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_name_add.Clear();
            dgv_criteria_add.Rows.Clear();
            RefreshCriterionTable(dgv_criterion_add, DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.Values.ToList());
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var criteriaDictionary = DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? criteriaDictionary : criteriaDictionary.Where(x => x.Value.Name.Contains(filterText)
                                                                                                                                || x.Value.WeightPerCriterion.Any(y => y.Criterion.String.Contains(filterText))
                                                                                                                                || x.Value.WeightPerCriterion.Any(y => y.Weight.ToString().Contains(filterText)));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    Criteria_QuantitativeEvaluation criteria = element.Value;
                    var row = new string[] { criteria.Name };
                    rows.Add(row);

                    if (m_selectedCriteriaName != string.Empty && row[0] == m_selectedCriteriaName)
                        selectingRowIndex = rows.GetLastRow(DataGridViewElementStates.None);
                }
            }

            m_addingRows = false;

            if (selectingRowIndex != -1)
                rows[selectingRowIndex].Selected = true;
            else if (rows.Count != 0)
                rows[rows.GetLastRow(DataGridViewElementStates.None)].Selected = true;
        }

        private void ApplyFilter_Add()
        {
            string filterText = txtBx_filter_add.Text;

            var criterionListInCriteria = TableToList(dgv_criteria_add).Select(x => x.Item1).ToList();

            var fullCriterionList = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.Values;
            var criterionListNotInCriteria = fullCriterionList.Except(criterionListInCriteria).ToList();
            var targetList = string.IsNullOrWhiteSpace(filterText) ? criterionListNotInCriteria : criterionListNotInCriteria.Where(x => x.String.Contains(filterText));
            var rows = dgv_criterion_add.Rows;
            {
                rows.Clear();
                foreach (var criterion in targetList)
                {
                    var row = new string[] { criterion.String, criterion.Min.ToString() + " - " + criterion.Max.ToString() };
                    rows.Add(row);
                }
            }
        }

        private void ApplyFilter_Edit()
        {
            string filterText = txtBx_filter_edit.Text;

            var criterionListInCriteria = TableToList(dgv_criteria_edit).Select(x => x.Item1).ToList();

            var fullCriterionList = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.Values;
            var criterionListNotInCriteria = fullCriterionList.Except(criterionListInCriteria).ToList();
            var targetList = string.IsNullOrWhiteSpace(filterText) ? criterionListNotInCriteria : criterionListNotInCriteria.Where(x => x.String.Contains(filterText));
            var rows = dgv_criterion_add.Rows;
            {
                rows.Clear();
                foreach (var criterion in targetList)
                {
                    var row = new string[] { criterion.String, criterion.Min.ToString() + " - " + criterion.Max.ToString() };
                    rows.Add(row);
                }
            }
        }
        #endregion

        #region Data Validation
        private bool ValidateInput_Add()
        {
            List<string> errorMessages = new List<string>();

            string name = txtBx_name_add.Text;
            if (string.IsNullOrWhiteSpace(name))
                errorMessages.Add("Enter a name!");
            else if (DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("A criterion with the same name already exists!");

            var criterionWeightList = TableToList(dgv_criteria_add);
            if (criterionWeightList.Count < 1)
                errorMessages.Add("Select at least one criterion!");

            int numOfErrors = errorMessages.Count;

            string fullMessage = string.Empty;
            for (int i = 0; i < numOfErrors; i++)
            {
                fullMessage += errorMessages[i];

                if (i != numOfErrors - 1) // If it is not the last item
                    fullMessage += "\n";
            }
            if (fullMessage != string.Empty)
                MessageBox.Show(fullMessage, "Error!");

            return numOfErrors == 0;
        }

        private bool ValidateInput_Edit()
        {
            List<string> errorMessages = new List<string>();

            string name = txtBx_name_edit.Text;
            if (string.IsNullOrWhiteSpace(name))
                errorMessages.Add("Enter a name!");

            var criterionWeightList = TableToList(dgv_criteria_edit);
            if (criterionWeightList.Count < 1)
                errorMessages.Add("Select at least one criterion!");

            if (!HaveDataBeenChanged())
                errorMessages.Add("Modify at least one field!");

            int numOfErrors = errorMessages.Count;

            string fullMessage = string.Empty;
            for (int i = 0; i < numOfErrors; i++)
            {
                fullMessage += errorMessages[i];

                if (i != numOfErrors - 1) // If it is not the last item
                    fullMessage += "\n";
            }
            if (fullMessage != string.Empty)
                MessageBox.Show(fullMessage, "Error!");

            return numOfErrors == 0;
        }

        private bool HaveDataBeenChanged()
        {
            return txtBx_name_edit.Text != (string)(SelectedCells["Name"].Value)
                || !dgv_criteria_edit.AllRowsEqual(dgv_criteria_view);
        }
        #endregion

        #region Shared
        private List<Tuple<Criterion_QuantitativeEvaluation, decimal>> TableToList(DataGridView _dgv)
        {
            List<Tuple<Criterion_QuantitativeEvaluation, decimal>> result = new List<Tuple<Criterion_QuantitativeEvaluation, decimal>>();

            try
            {
                var criterionDictionary = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary;
                foreach (DataGridViewRow row in _dgv.Rows)
                {
                    var cells = row.Cells;
                    string @string = (string)(cells["Criterion"].Value);
                    decimal weight = Convert.ToDecimal((string)(cells["Weight"].Value));

                    Criterion_QuantitativeEvaluation criterion = criterionDictionary.First(x => x.Value.String == @string).Value;

                    result.Add(new Tuple<Criterion_QuantitativeEvaluation, decimal>(criterion, weight));
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
