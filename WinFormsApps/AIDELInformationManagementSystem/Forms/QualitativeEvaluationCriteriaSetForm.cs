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
    public partial class QualitativeEvaluationCriteriaSetForm : Form
    {
        public QualitativeEvaluationCriteriaSetForm()
        {
            InitializeComponent();
        }

        private void QualitativeEvaluationCriterionForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

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

        private Criteria_QualitativeEvaluation m_firstPartialCriteria_add;
        private Criteria_QualitativeEvaluation m_midtermCriteria_add;
        private Criteria_QualitativeEvaluation m_finalsCriteria_add;

        private Criteria_QualitativeEvaluation m_firstPartialCriteria_edit;
        private Criteria_QualitativeEvaluation m_midtermCriteria_edit;
        private Criteria_QualitativeEvaluation m_finalsCriteria_edit;

        private Criteria_QualitativeEvaluation m_firstPartialCriteria_delete;
        private Criteria_QualitativeEvaluation m_midtermCriteria_delete;
        private Criteria_QualitativeEvaluation m_finalsCriteria_delete;

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
                m_selectedCriteriaSetName = String.Empty;

                m_firstPartialCriteria_edit = null;
                m_midtermCriteria_edit = null;
                m_finalsCriteria_edit = null;

                m_firstPartialCriteria_delete = null;
                m_midtermCriteria_delete = null;
                m_finalsCriteria_delete = null;

                rdBtn_firstPartial_edit.Checked = true;
                rdBtn_firstPartial_delete.Checked = true;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedCriteriaSetName = (string)(SelectedCells["Name"].Value);

                CriteriaSet_QualitativeEvaluation selectedCriteriaSet = DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Value;

                m_firstPartialCriteria_edit = selectedCriteriaSet.Criteria_QualitativeEvaluation_FirstPartial;
                m_midtermCriteria_edit = selectedCriteriaSet.Criteria_QualitativeEvaluation_Midterm;
                m_finalsCriteria_edit = selectedCriteriaSet.Criteria_QualitativeEvaluation_Final;

                m_firstPartialCriteria_delete = selectedCriteriaSet.Criteria_QualitativeEvaluation_FirstPartial;
                m_midtermCriteria_delete = selectedCriteriaSet.Criteria_QualitativeEvaluation_Midterm;
                m_finalsCriteria_delete = selectedCriteriaSet.Criteria_QualitativeEvaluation_Final;

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
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_firstPartialCriteria_add != null ? m_firstPartialCriteria_add.Name : CoreValues.ComboBox_DefaultString);
        }
        private void rdBtn_midterm_add_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_midtermCriteria_add != null ? m_midtermCriteria_add.Name : CoreValues.ComboBox_DefaultString);
        }
        private void rdBtn_finals_add_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(m_finalsCriteria_add != null ? m_finalsCriteria_add.Name : CoreValues.ComboBox_DefaultString);
        }

        private void cmbBx_criteria_add_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaName = cmbBx_criteria_add.SelectedItem.ToString();

            var criteria = (selectedCriteriaName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QualitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == selectedCriteriaName).Value :
                    null;

            if (rdBtn_firstPartial_add.Checked)
                m_firstPartialCriteria_add = criteria;
            else if (rdBtn_midterm_add.Checked)
                m_midtermCriteria_add = criteria;
            else if (rdBtn_finals_add.Checked)
                m_finalsCriteria_add = criteria;
            
            RefreshCriteriaTable(dgv_criteria_add, criteria);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                if (DocumentLoader.AddQualitativeEvaluationCriteriaSetToCSV(txtBx_name_add.Text, m_firstPartialCriteria_add, m_midtermCriteria_add, m_finalsCriteria_add) != default)
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
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_firstPartialCriteria_edit != null ? m_firstPartialCriteria_edit.Name : CoreValues.ComboBox_DefaultString);
        }
        private void rdBtn_midterm_edit_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_midtermCriteria_edit != null ? m_midtermCriteria_edit.Name : CoreValues.ComboBox_DefaultString);
        }
        private void rdBtn_finals_edit_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_edit.SelectedIndex = cmbBx_criteria_edit.FindStringExact(m_finalsCriteria_edit != null ? m_finalsCriteria_edit.Name : CoreValues.ComboBox_DefaultString);
        }

        private void cmbBx_criteria_edit_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaName = cmbBx_criteria_edit.SelectedItem.ToString();

            var criteria = (selectedCriteriaName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QualitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == selectedCriteriaName).Value :
                    null;

            if (rdBtn_firstPartial_edit.Checked)
                m_firstPartialCriteria_edit = criteria;
            else if (rdBtn_midterm_edit.Checked)
                m_midtermCriteria_edit = criteria;
            else if (rdBtn_finals_edit.Checked)
                m_finalsCriteria_edit = criteria;

            RefreshCriteriaTable(dgv_criteria_edit, criteria);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                CriteriaSet_QualitativeEvaluation selectedCriteriaSet = DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Value;

                if (DocumentLoader.EditQualitativeEvaluationCriteriaSetInCSV(selectedCriteriaSet, txtBx_name_edit.Text, m_firstPartialCriteria_edit, m_midtermCriteria_edit, m_finalsCriteria_edit))
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
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_firstPartialCriteria_delete != null ? m_firstPartialCriteria_delete.Name : CoreValues.ComboBox_DefaultString);
        }
        private void rdBtn_midterm_delete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_midtermCriteria_delete != null ? m_midtermCriteria_delete.Name : CoreValues.ComboBox_DefaultString);
        }
        private void rdBtn_finals_delete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBx_criteria_delete.SelectedIndex = cmbBx_criteria_delete.FindStringExact(m_finalsCriteria_delete != null ? m_finalsCriteria_delete.Name : CoreValues.ComboBox_DefaultString);
        }

        private void cmbBx_criteria_delete_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaName = cmbBx_criteria_delete.SelectedItem.ToString();

            var criteria = (selectedCriteriaName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QualitativeEvaluationCriteriaDictionary.First(x => x.Value.Name == selectedCriteriaName).Value :
                    null;

            if (rdBtn_firstPartial_delete.Checked)
                m_firstPartialCriteria_delete = criteria;
            else if (rdBtn_midterm_delete.Checked)
                m_midtermCriteria_delete = criteria;
            else if (rdBtn_finals_delete.Checked)
                m_finalsCriteria_delete = criteria;
            
            RefreshCriteriaTable(dgv_criteria_delete, criteria);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCriteriaSetId = DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == m_selectedCriteriaSetName).Key;
                if (DocumentLoader.DeleteQualitativeEvaluationCriteriaSetFromCSV(selectedCriteriaSetId))
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
                columns.Add("FirstPartialCriteria", "First Partial Criteria");
                columns.Add("MidtermCriteria", "Midterm Criteria");
                columns.Add("FinalsCriteria", "Finals Criteria");
            }
        }

        private void InitializeCriteriaComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var criteria in DataContainer.Instance.QualitativeEvaluationCriteriaDictionary.Values)
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
                columns.Add("EvaluationColorSet", "Evaluation Color Set");
                columns.Add("Levels", "Levels");
                columns.Add("MinValue", "Min Value");
                columns.Add("MaxValue", "Max Value");

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

        private void RefreshCriteriaTable(DataGridView _dgv, Criteria_QualitativeEvaluation _criteria)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_criteria == null)
                return;

            foreach (var criterion in _criteria.CriterionList)
            {
                EvaluationColorSet evaluationColorSet = criterion.EvaluationColorSet;
                int minValue = evaluationColorSet.ValueColors.Min().NumericValue;
                int maxValue = evaluationColorSet.ValueColors.Max().NumericValue;
                int levels = maxValue - minValue + 1;

                rows.Add(new string[] { criterion.String, criterion.EvaluationColorSet.Name, levels.ToString(), minValue.ToString(), maxValue.ToString() });
            }
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_name_add.Clear();
            rdBtn_firstPartial_add.Checked = true;
            cmbBx_criteria_add.SelectedIndex = cmbBx_criteria_add.FindStringExact(CoreValues.ComboBox_DefaultString);
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var criteriaSetDictionary = DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? criteriaSetDictionary : criteriaSetDictionary.Where(x => x.Value.Name.Contains(filterText)
                                                                                                                                || ((x.Value.Criteria_QualitativeEvaluation_FirstPartial != null) ? x.Value.Criteria_QualitativeEvaluation_FirstPartial.Name.Contains(filterText) : false)
                                                                                                                                || x.Value.Criteria_QualitativeEvaluation_Midterm.Name.Contains(filterText)
                                                                                                                                || x.Value.Criteria_QualitativeEvaluation_Final.Name.Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    CriteriaSet_QualitativeEvaluation criteriaSet = element.Value;
                    var row = new string[] { criteriaSet.Name, (criteriaSet.Criteria_QualitativeEvaluation_FirstPartial != null) ? criteriaSet.Criteria_QualitativeEvaluation_FirstPartial.Name : "", criteriaSet.Criteria_QualitativeEvaluation_Midterm.Name, criteriaSet.Criteria_QualitativeEvaluation_Final.Name };
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
            else if (DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("An criteria set with the same name already exists!");

            if (m_midtermCriteria_add == null)
                errorMessages.Add("Select a criteria for midterm!");

            if (m_finalsCriteria_add == null)
                errorMessages.Add("Select a criteria for finals!");

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
            else if (DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("An criteria set with the same name already exists!");

            if (m_midtermCriteria_edit == null)
                errorMessages.Add("Select a criteria for midterm!");

            if (m_finalsCriteria_edit == null)
                errorMessages.Add("Select a criteria for finals!");

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
                || (m_firstPartialCriteria_edit == null && ((string)(SelectedCells["FirstPartialCriteria"].Value) != ""))
                || (m_firstPartialCriteria_edit != null && (m_firstPartialCriteria_edit.Name != (string)(SelectedCells["FirstPartialCriteria"].Value)))
                || m_midtermCriteria_edit.Name != (string)(SelectedCells["MidtermCriteria"].Value)
                || m_finalsCriteria_edit.Name != (string)(SelectedCells["FinalsCriteria"].Value);
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
