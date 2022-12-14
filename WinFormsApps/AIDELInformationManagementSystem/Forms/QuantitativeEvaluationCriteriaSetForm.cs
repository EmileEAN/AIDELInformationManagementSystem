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
    public partial class QuantitativeEvaluationCriteriaSetForm : Form
    {
        public QuantitativeEvaluationCriteriaSetForm()
        {
            InitializeComponent();
        }

        private void QuantitativeEvaluationCriterionForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeCriterionWeightTuples();

            InitializeCriteriaComboBox(cmbBx_criteria_add);
            InitializeCriteriaComboBox(cmbBx_criteria_edit);
            InitializeCriteriaComboBox(cmbBx_criteria_delete);

            InitializeCriteriaTable(dgv_criteria_add);
            InitializeCriteriaTable(dgv_criteria_edit);
            InitializeCriteriaTable(dgv_criteria_delete);

            RefreshMainViewTable();
            dgv_main.ClearSelection();

            rdBtn_firstPartial_add.Checked = true;

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
        private string m_selectedCriteriaSetName = string.Empty;
        private DataGridViewRow m_selectedRow;
        private DataGridViewCellCollection SelectedCells { get { return m_selectedRow?.Cells; } }

        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_firstPartialCriteriaWeight_add;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_midtermCriteriaWeight_add;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_finalsCriteriaWeight_add;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_additionalCriteriaWeight_add;

        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_firstPartialCriteriaWeight_edit;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_midtermCriteriaWeight_edit;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_finalsCriteriaWeight_edit;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_additionalCriteriaWeight_edit;

        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_firstPartialCriteriaWeight_delete;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_midtermCriteriaWeight_delete;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_finalsCriteriaWeight_delete;
        private Tuple<Criteria_QuantitativeEvaluation, decimal> m_additionalCriteriaWeight_delete;

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

            var defaultCriteriaWeightTuple = new Tuple<Criteria_QuantitativeEvaluation, decimal>(null, numUpDwn_weight_add.Minimum); // All numeric up down components' minimum is same

            if (dgv_main.SelectedRows.Count == 0)
            {
                m_selectedRow = null;
                m_selectedCriteriaSetName = String.Empty;

                m_firstPartialCriteriaWeight_edit = defaultCriteriaWeightTuple;
                m_midtermCriteriaWeight_edit = defaultCriteriaWeightTuple;
                m_finalsCriteriaWeight_edit = defaultCriteriaWeightTuple;
                m_additionalCriteriaWeight_edit = defaultCriteriaWeightTuple;

                m_firstPartialCriteriaWeight_delete = defaultCriteriaWeightTuple;
                m_midtermCriteriaWeight_delete = defaultCriteriaWeightTuple;
                m_finalsCriteriaWeight_delete = defaultCriteriaWeightTuple;
                m_additionalCriteriaWeight_delete = defaultCriteriaWeightTuple;

                rdBtn_firstPartial_edit.Checked = true;
                rdBtn_firstPartial_delete.Checked = true;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedCriteriaSetName = (string)(SelectedCells["Name"].Value);

                CriteriaSet_QuantitativeEvaluation selectedCriteriaSet = DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Value;

                var criteriaWeight_firstPartial = selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial;
                var criteriaWeight_midterm = selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm;
                var criteriaWeight_finals = selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Final;
                var criteriaWeight_additional = selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional;

                var criteriaWeightTuple_firstPartial = (criteriaWeight_firstPartial != null) ? new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteriaWeight_firstPartial.Criteria, criteriaWeight_firstPartial.Weight) : defaultCriteriaWeightTuple;
                var criteriaWeightTuple_midterm = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteriaWeight_midterm.Criteria, criteriaWeight_midterm.Weight);
                var criteriaWeightTuple_finals = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteriaWeight_finals.Criteria, criteriaWeight_finals.Weight);
                var criteriaWeightTuple_additional = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteriaWeight_additional.Criteria, criteriaWeight_additional.Weight);

                m_firstPartialCriteriaWeight_edit = criteriaWeightTuple_firstPartial;
                m_midtermCriteriaWeight_edit = criteriaWeightTuple_midterm;
                m_finalsCriteriaWeight_edit = criteriaWeightTuple_finals;
                m_additionalCriteriaWeight_edit = criteriaWeightTuple_additional;

                m_firstPartialCriteriaWeight_delete = criteriaWeightTuple_firstPartial;
                m_midtermCriteriaWeight_delete = criteriaWeightTuple_midterm;
                m_finalsCriteriaWeight_delete = criteriaWeightTuple_finals;
                m_additionalCriteriaWeight_delete = criteriaWeightTuple_additional;

                rdBtn_firstPartial_edit.Checked = true;
                rdBtn_firstPartial_delete.Checked = true;
            }

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
        private void rdBtn_firstPartial_add_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_firstPartialCriteriaWeight_add.Item1 != null ? m_firstPartialCriteriaWeight_add.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_add.Value = (m_firstPartialCriteriaWeight_add.Item1 != null) ? m_firstPartialCriteriaWeight_add.Item2 : numUpDwn_weight_add.Minimum;
        }
        private void rdBtn_midterm_add_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_midtermCriteriaWeight_add.Item1 != null ? m_midtermCriteriaWeight_add.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_add.Value = (m_midtermCriteriaWeight_add.Item1 != null) ? m_midtermCriteriaWeight_add.Item2 : numUpDwn_weight_add.Minimum;
        }
        private void rdBtn_finals_add_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_finalsCriteriaWeight_add.Item1 != null ? m_finalsCriteriaWeight_add.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_add.Value = (m_finalsCriteriaWeight_add.Item1 != null) ? m_finalsCriteriaWeight_add.Item2 : numUpDwn_weight_add.Minimum;
        }
        private void rdBtn_additional_add_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_additionalCriteriaWeight_add.Item1 != null ? m_additionalCriteriaWeight_add.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_add.Value = (m_additionalCriteriaWeight_add.Item1 != null) ? m_additionalCriteriaWeight_add.Item2 : numUpDwn_weight_add.Minimum;
        }

        private void numUpDwn_weight_add_ValueChanged(object sender, EventArgs e)
        {
            if (cmbBx_criteria_add.SelectedItem.ToString() == CoreValues.ComboBox_DefaultString)
                return;

            decimal weight = numUpDwn_weight_add.Value;

            if (rdBtn_firstPartial_add.Checked)
                m_firstPartialCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_firstPartialCriteriaWeight_add.Item1, weight);
            else if (rdBtn_midterm_add.Checked)
                m_midtermCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_midtermCriteriaWeight_add.Item1, weight);
            else if (rdBtn_finals_add.Checked)
                m_finalsCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_finalsCriteriaWeight_add.Item1, weight);
            else if (rdBtn_additional_add.Checked)
                m_additionalCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_additionalCriteriaWeight_add.Item1, weight);
        }

        private void cmbBx_criteria_add_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaName = cmbBx_criteria_add.SelectedItem.ToString();

            var criteria = (selectedCriteriaName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == selectedCriteriaName).Value :
                    null;

            if (rdBtn_firstPartial_add.Checked)
                m_firstPartialCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_finalsCriteriaWeight_add.Item2);
            else if (rdBtn_midterm_add.Checked)
                m_midtermCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_midtermCriteriaWeight_add.Item2);
            else if (rdBtn_finals_add.Checked)
                m_finalsCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_finalsCriteriaWeight_add.Item2);
            else if (rdBtn_additional_add.Checked)
                m_additionalCriteriaWeight_add = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_additionalCriteriaWeight_add.Item2);
            
            RefreshCriteriaTable(dgv_criteria_add, criteria);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                if (DocumentLoader.AddQuantitativeEvaluationCriteriaSetToCSV(txtBx_name_add.Text, m_firstPartialCriteriaWeight_add, m_midtermCriteriaWeight_add, m_finalsCriteriaWeight_add, m_additionalCriteriaWeight_add) != default)
                {
                    MessageBox.Show("Added new criteria set.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                    RefreshCriteriaComboBox(cmbBx_criteria_add);
                    RefreshCriteriaComboBox(cmbBx_criteria_edit);
                    RefreshCriteriaComboBox(cmbBx_criteria_delete);
                }
                else
                    MessageBox.Show("Failed to add new criteria set! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void rdBtn_firstPartial_edit_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_firstPartialCriteriaWeight_edit.Item1 != null ? m_firstPartialCriteriaWeight_edit.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_edit.Value = (m_firstPartialCriteriaWeight_edit.Item1 != null) ? m_firstPartialCriteriaWeight_edit.Item2 : numUpDwn_weight_edit.Minimum;
        }
        private void rdBtn_midterm_edit_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_midtermCriteriaWeight_edit.Item1 != null ? m_midtermCriteriaWeight_edit.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_edit.Value = (m_midtermCriteriaWeight_edit.Item1 != null) ? m_midtermCriteriaWeight_edit.Item2 : numUpDwn_weight_edit.Minimum;
        }
        private void rdBtn_finals_edit_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_finalsCriteriaWeight_edit.Item1 != null ? m_finalsCriteriaWeight_edit.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_edit.Value = (m_finalsCriteriaWeight_edit.Item1 != null) ? m_finalsCriteriaWeight_edit.Item2 : numUpDwn_weight_edit.Minimum;
        }
        private void rdBtn_additional_edit_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_additionalCriteriaWeight_edit.Item1 != null ? m_additionalCriteriaWeight_edit.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_edit.Value = (m_additionalCriteriaWeight_edit.Item1 != null) ? m_additionalCriteriaWeight_edit.Item2 : numUpDwn_weight_edit.Minimum;
        }

        private void cmbBx_criteria_edit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBx_criteria_edit.SelectedIndex == -1)
                return;

            string selectedCriteriaName = cmbBx_criteria_edit.SelectedItem.ToString();

            var criteria = (selectedCriteriaName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == selectedCriteriaName).Value :
                    null;

            if (rdBtn_firstPartial_edit.Checked)
                m_firstPartialCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_finalsCriteriaWeight_edit.Item2);
            else if (rdBtn_midterm_edit.Checked)
                m_midtermCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_midtermCriteriaWeight_edit.Item2);
            else if (rdBtn_finals_edit.Checked)
                m_finalsCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_finalsCriteriaWeight_edit.Item2);
            else if (rdBtn_additional_edit.Checked)
                m_additionalCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, m_additionalCriteriaWeight_edit.Item2);

            RefreshCriteriaTable(dgv_criteria_edit, criteria);
        }

        private void numUpDwn_weight_edit_ValueChanged(object sender, EventArgs e)
        {
            if (cmbBx_criteria_edit.SelectedItem.ToString() == CoreValues.ComboBox_DefaultString)
                return;

            decimal weight = numUpDwn_weight_edit.Value;

            if (rdBtn_firstPartial_edit.Checked)
                m_firstPartialCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_firstPartialCriteriaWeight_edit.Item1, weight);
            else if (rdBtn_midterm_edit.Checked)
                m_midtermCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_midtermCriteriaWeight_edit.Item1, weight);
            else if (rdBtn_finals_edit.Checked)
                m_finalsCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_finalsCriteriaWeight_edit.Item1, weight);
            else if (rdBtn_additional_edit.Checked)
                m_additionalCriteriaWeight_edit = new Tuple<Criteria_QuantitativeEvaluation, decimal>(m_additionalCriteriaWeight_edit.Item1, weight);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                CriteriaSet_QuantitativeEvaluation selectedCriteriaSet = DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Value;

                if (DocumentLoader.EditQuantitativeEvaluationCriteriaSetInCSV(selectedCriteriaSet, txtBx_name_edit.Text, m_firstPartialCriteriaWeight_edit, m_midtermCriteriaWeight_edit, m_finalsCriteriaWeight_edit, m_additionalCriteriaWeight_edit))
                {
                    MessageBox.Show("Edited criteria set successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit criteria set! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void rdBtn_firstPartial_delete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_firstPartialCriteriaWeight_add.Item1 != null ? m_firstPartialCriteriaWeight_delete.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_delete.Value = (m_firstPartialCriteriaWeight_add.Item1 != null) ? m_firstPartialCriteriaWeight_delete.Item2 : numUpDwn_weight_delete.Minimum;
        }
        private void rdBtn_midterm_delete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_midtermCriteriaWeight_add.Item1 != null ? m_midtermCriteriaWeight_delete.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_delete.Value = (m_midtermCriteriaWeight_add.Item1 != null) ? m_midtermCriteriaWeight_delete.Item2 : numUpDwn_weight_delete.Minimum;
        }
        private void rdBtn_finals_delete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_finalsCriteriaWeight_add.Item1 != null ? m_finalsCriteriaWeight_delete.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_delete.Value = (m_finalsCriteriaWeight_add.Item1 != null) ? m_finalsCriteriaWeight_delete.Item2 : numUpDwn_weight_delete.Minimum;
        }
        private void rdBtn_additional_delete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_additionalCriteriaWeight_add.Item1 != null ? m_additionalCriteriaWeight_delete.Item1.Name : CoreValues.ComboBox_DefaultString);

            numUpDwn_weight_delete.Value = (m_additionalCriteriaWeight_add.Item1 != null) ? m_additionalCriteriaWeight_delete.Item2 : numUpDwn_weight_delete.Minimum;
        }

        private void cmbBx_criteria_delete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBx_criteria_delete.SelectedIndex == -1)
                return;

            string selectedCriteriaName = cmbBx_criteria_delete.SelectedItem.ToString();

            var criteria = (selectedCriteriaName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == selectedCriteriaName).Value :
                    null;

            decimal weight = numUpDwn_weight_add.Value;

            var criteriaWeightTuple = new Tuple<Criteria_QuantitativeEvaluation, decimal>(criteria, weight);

            if (rdBtn_firstPartial_delete.Checked)
                m_firstPartialCriteriaWeight_delete = criteriaWeightTuple;
            else if (rdBtn_midterm_delete.Checked)
                m_midtermCriteriaWeight_delete = criteriaWeightTuple;
            else if (rdBtn_finals_delete.Checked)
                m_finalsCriteriaWeight_delete = criteriaWeightTuple;
            else if (rdBtn_additional_delete.Checked)
                m_additionalCriteriaWeight_delete = criteriaWeightTuple;
            
            RefreshCriteriaTable(dgv_criteria_delete, criteria);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCriteriaSetId = DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Key;
                if (DocumentLoader.DeleteQuantitativeEvaluationCriteriaSetFromCSV(selectedCriteriaSetId))
                {
                    MessageBox.Show("Deleted criteria set successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete criteria set! Please try again.");
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
                columns.Add("FirstPartialCriteria", "First Partial Criteria (Weight)");
                columns.Add("MidtermCriteria", "Midterm Criteria (Weight)");
                columns.Add("FinalsCriteria", "Finals Criteria (Weight)");
                columns.Add("AdditionalCriteria", "Additional Criteria (Weight)");
            }
        }

        private void InitializeCriterionWeightTuples()
        {
            var defaultCriteriaWeightTuple = new Tuple<Criteria_QuantitativeEvaluation, decimal>(null, numUpDwn_weight_add.Minimum); // All numeric up down components' minimum is same

            m_firstPartialCriteriaWeight_add = defaultCriteriaWeightTuple;
            m_midtermCriteriaWeight_add = defaultCriteriaWeightTuple;
            m_finalsCriteriaWeight_add = defaultCriteriaWeightTuple;
            m_additionalCriteriaWeight_add = defaultCriteriaWeightTuple;

            m_firstPartialCriteriaWeight_edit = defaultCriteriaWeightTuple;
            m_midtermCriteriaWeight_edit = defaultCriteriaWeightTuple;
            m_finalsCriteriaWeight_edit = defaultCriteriaWeightTuple;
            m_additionalCriteriaWeight_edit = defaultCriteriaWeightTuple;

            m_firstPartialCriteriaWeight_delete = defaultCriteriaWeightTuple;
            m_midtermCriteriaWeight_delete = defaultCriteriaWeightTuple;
            m_finalsCriteriaWeight_delete = defaultCriteriaWeightTuple;
            m_additionalCriteriaWeight_delete = defaultCriteriaWeightTuple;
        }

        private void InitializeCriteriaComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var criteria in DataContainer.Instance.QuantitativeEvaluationCriteriaDictionary.Values)
            {
                items.Add(criteria.Name);
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeCriteriaTable(DataGridView _dgv)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;

                columns.Add("Criterion", "Criterion");
                columns.Add("ValueType", "Value Type");
                columns.Add("ValueRange", "Value Range");
                columns.Add("Weight", "Weight");

                _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_name_edit.Text = m_selectedCriteriaSetName;

                string criteriaName = string.Empty;
                if (rdBtn_firstPartial_edit.Checked)
                    criteriaName = (string)(SelectedCells["FirstPartialCriteria"].Value);
                else if (rdBtn_midterm_edit.Checked)
                    criteriaName = (string)(SelectedCells["MidtermCriteria"].Value);
                else if (rdBtn_finals_edit.Checked)
                    criteriaName = (string)(SelectedCells["FinalsCriteria"].Value);

                if (string.IsNullOrWhiteSpace(criteriaName))
                    criteriaName = CoreValues.ComboBox_DefaultString;

                cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(criteriaName);
            }
            else
            {
                txtBx_name_edit.Clear();
                cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(CoreValues.ComboBox_DefaultString);
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_name_delete.Text = m_selectedCriteriaSetName;

                string criteriaName = string.Empty;
                if (rdBtn_firstPartial_delete.Checked)
                    criteriaName = (string)(SelectedCells["FirstPartialCriteria"].Value);
                else if (rdBtn_midterm_delete.Checked)
                    criteriaName = (string)(SelectedCells["MidtermCriteria"].Value);
                else if (rdBtn_finals_delete.Checked)
                    criteriaName = (string)(SelectedCells["FinalsCriteria"].Value);

                if (string.IsNullOrWhiteSpace(criteriaName))
                    criteriaName = CoreValues.ComboBox_DefaultString;

                cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(criteriaName);
            }
            else
            {
                txtBx_name_delete.Clear();
                cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(CoreValues.ComboBox_DefaultString);
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }

        private void RefreshCriteriaComboBox(ComboBox _cmbBx)
        {
            InitializeCriteriaComboBox(_cmbBx);
        }

        private void RefreshCriteriaTable(DataGridView _dgv, Criteria_QuantitativeEvaluation _criteria)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_criteria == null)
                return;

            foreach (var criterionWeight in _criteria.WeightPerCriterion)
            {
                var criterion = criterionWeight.Criterion;
                rows.Add(new string[] { criterion.String, criterion.EvaluationValueType.ToString(), criterion.Min.ToString() + " - " + criterion.Max.ToString(), criterionWeight.Weight.ToString() });
            }
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_name_add.Clear();
            rdBtn_firstPartial_add.Checked = true;
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(CoreValues.ComboBox_DefaultString);
            numUpDwn_weight_add.Value = numUpDwn_weight_add.Minimum;
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var criteriaSetDictionary = DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? criteriaSetDictionary : criteriaSetDictionary.Where(x => x.Value.Name.Contains(filterText)
                                                                                                                                || ((x.Value.CriteriaWeight_QuantitativeEvaluation_FirstPartial != null) ? x.Value.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Criteria.Name.Contains(filterText) : false)
                                                                                                                                || x.Value.CriteriaWeight_QuantitativeEvaluation_Midterm.Criteria.Name.Contains(filterText)
                                                                                                                                || x.Value.CriteriaWeight_QuantitativeEvaluation_Final.Criteria.Name.Contains(filterText)
                                                                                                                                || x.Value.CriteriaWeight_QuantitativeEvaluation_Additional.Criteria.Name.Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    CriteriaSet_QuantitativeEvaluation criteriaSet = element.Value;
                    var criteriaWeight_firstPartial = criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial;
                    var criteriaWeight_midterm = criteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm;
                    var criteriaWeight_finals = criteriaSet.CriteriaWeight_QuantitativeEvaluation_Final;
                    var criteriaWeight_additional = criteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional;

                    decimal weight_firstPartial = (criteriaWeight_firstPartial != null) ? criteriaWeight_firstPartial.Weight : 0;
                    decimal weight_midterm = criteriaWeight_midterm.Weight;
                    decimal weight_finals = criteriaWeight_finals.Weight;
                    decimal weight_additional = criteriaWeight_additional.Weight;

                    decimal totalRelativeValue = weight_firstPartial + weight_midterm + weight_finals + weight_additional;

                    string percentageStringFormat = CoreValues.PercentageStringFormat;
                    string formattedWeightPercentage_firstPartial = (weight_firstPartial / totalRelativeValue).ToString(percentageStringFormat);
                    string formattedWeightPercentage_midterm = (weight_midterm / totalRelativeValue).ToString(percentageStringFormat);
                    string formattedWeightPercentage_finals = (weight_finals / totalRelativeValue).ToString(percentageStringFormat);
                    string formattedWeightPercentage_additional = (weight_additional / totalRelativeValue).ToString(percentageStringFormat);

                    var row = new string[] { criteriaSet.Name,
                                            (criteriaWeight_firstPartial != null) ? (criteriaWeight_firstPartial.Criteria.Name + " (" + formattedWeightPercentage_firstPartial + ")") : "", 
                                            criteriaWeight_midterm.Criteria.Name + " (" + formattedWeightPercentage_midterm + ")", 
                                            criteriaWeight_finals.Criteria.Name + " (" + formattedWeightPercentage_finals + ")",
                                            criteriaWeight_additional.Criteria.Name + " (" + formattedWeightPercentage_additional + ")" };
                    rows.Add(row);

                    if (m_selectedCriteriaSetName != string.Empty && row[0] == m_selectedCriteriaSetName)
                        selectingRowIndex = rows.GetLastRow(DataGridViewElementStates.None);
                }
            }

            m_addingRows = false;

            if (selectingRowIndex != -1)
                rows[selectingRowIndex].Selected = true;
            else if (rows.Count != 0)
                rows[rows.GetLastRow(DataGridViewElementStates.None)].Selected = true;
        }
        #endregion

        #region Data Validation
        private bool ValidateInput_Add()
        {
            List<string> errorMessages = new List<string>();

            string name = txtBx_name_add.Text;
            if (string.IsNullOrWhiteSpace(name))
                errorMessages.Add("Enter a name!");
            else if (DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("An criteria set with the same name already exists!");

            if (m_midtermCriteriaWeight_add.Item1 == null)
                errorMessages.Add("Select a criteria for midterm!");

            if (m_finalsCriteriaWeight_add.Item1 == null)
                errorMessages.Add("Select a criteria for finals!");

            if (m_additionalCriteriaWeight_add.Item1 == null)
                errorMessages.Add("Select a criteria for additional!");

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
            else if (DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("An criteria set with the same name already exists!");

            if (m_midtermCriteriaWeight_edit.Item1 == null)
                errorMessages.Add("Select a criteria for midterm!");

            if (m_finalsCriteriaWeight_edit.Item1 == null)
                errorMessages.Add("Select a criteria for finals!");

            if (m_additionalCriteriaWeight_edit.Item1 == null)
                errorMessages.Add("Select a criteria for additional!");

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
            CriteriaSet_QuantitativeEvaluation selectedCriteriaSet = DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Value;
            
            return txtBx_name_edit.Text != selectedCriteriaSet.Name
                || m_firstPartialCriteriaWeight_edit.Item1 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Criteria
                || m_firstPartialCriteriaWeight_edit.Item1 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Criteria
                || m_firstPartialCriteriaWeight_edit.Item2 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Weight
                || m_midtermCriteriaWeight_edit.Item1 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm.Criteria
                || m_midtermCriteriaWeight_edit.Item2 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm.Weight
                || m_finalsCriteriaWeight_edit.Item1 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Final.Criteria
                || m_finalsCriteriaWeight_edit.Item2 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Final.Weight
                || m_additionalCriteriaWeight_edit.Item1 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional.Criteria
                || m_additionalCriteriaWeight_edit.Item2 != selectedCriteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional.Weight;
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
