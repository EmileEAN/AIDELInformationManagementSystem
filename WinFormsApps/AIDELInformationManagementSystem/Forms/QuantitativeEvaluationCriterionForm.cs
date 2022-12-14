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
    public partial class QuantitativeEvaluationCriterionForm : Form
    {
        public QuantitativeEvaluationCriterionForm()
        {
            InitializeComponent();
        }

        private void QuantitativeEvaluationCriterionForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            RefreshMainViewTable();
            dgv_main.ClearSelection();

            m_permissionType = DataContainer.Instance.CurrentUser.PermissionsType;
            if (m_permissionType == ePermissionsType.ViewOnly || m_permissionType == ePermissionsType.AssignedCoursesOnly)
            {
                tbCtrl_main.SelectedIndexChanged += (_sender, _e) =>
                {
                    if (tbCtrl_main.SelectedIndex != 0)
                        tbCtrl_main.SelectedIndex = 0;
                };
            }
        }

        private ePermissionsType m_permissionType;
        private bool m_selectionChanged = false;
        private bool m_addingRows = false;
        private string m_selectedCriterionString = string.Empty;
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
                    color = (m_permissionType == ePermissionsType.ViewOnly || m_permissionType == ePermissionsType.AssignedCoursesOnly) ? Color.LightGray : Color.FromArgb(102, 255, 102);
                    break;

                case 2: // case 'Edit' tab
                    color = (m_permissionType == ePermissionsType.ViewOnly || m_permissionType == ePermissionsType.AssignedCoursesOnly) ? Color.LightGray : Color.FromArgb(255, 255, 102);
                    break;

                case 3: // case 'Delete' tab
                    color = (m_permissionType == ePermissionsType.ViewOnly || m_permissionType == ePermissionsType.AssignedCoursesOnly) ? Color.LightGray : Color.FromArgb(255, 102, 102);
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
                m_selectedCriterionString = String.Empty;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedCriterionString = (string)(SelectedCells["Criterion"].Value);
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
        private void rdBtn_integer_add_CheckedChanged(object sender, EventArgs e)
        {
            numUpDwn_minValue_add.DecimalPlaces = 0;
            numUpDwn_maxValue_add.DecimalPlaces = 0;

            numUpDwn_minValue_add.Increment = 1;
            numUpDwn_maxValue_add.Increment = 1;
        }

        private void rdBtn_decimal_add_CheckedChanged(object sender, EventArgs e)
        {
            numUpDwn_minValue_add.DecimalPlaces = 2;
            numUpDwn_maxValue_add.DecimalPlaces = 2;

            numUpDwn_minValue_add.Increment = 0.01m;
            numUpDwn_maxValue_add.Increment = 0.01m;
        }

        private void numUpDwn_minValue_add_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_minValue_add.Value > numUpDwn_maxValue_add.Value)
            {
                numUpDwn_minValue_add.Value--;
                return;
            }
        }

        private void numUpDwn_maxValue_add_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_maxValue_add.Value < numUpDwn_minValue_add.Value)
            {
                numUpDwn_maxValue_add.Value++;
                return;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                eValueType valueType = default;
                if (rdBtn_integer_add.Checked)
                    valueType = eValueType.Integer;
                if (rdBtn_decimal_add.Checked)
                    valueType = eValueType.Decimal;

                if (DocumentLoader.AddQuantitativeEvaluationCriterionToCSV(txtBx_string_add.Text, valueType, numUpDwn_minValue_add.Value, numUpDwn_maxValue_add.Value) != default)
                {
                    MessageBox.Show("Added new criterion.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new criterion! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void rdBtn_integer_edit_CheckedChanged(object sender, EventArgs e)
        {
            numUpDwn_minValue_edit.DecimalPlaces = 0;
            numUpDwn_maxValue_edit.DecimalPlaces = 0;

            numUpDwn_minValue_edit.Increment = 1;
            numUpDwn_maxValue_edit.Increment = 1;
        }

        private void rdBtn_decimal_edit_CheckedChanged(object sender, EventArgs e)
        {
            numUpDwn_minValue_edit.DecimalPlaces = 2;
            numUpDwn_maxValue_edit.DecimalPlaces = 2;

            numUpDwn_minValue_edit.Increment = 0.01m;
            numUpDwn_maxValue_edit.Increment = 0.01m;
        }

        private void numUpDwn_minValue_edit_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_minValue_edit.Value > numUpDwn_maxValue_edit.Value)
            {
                numUpDwn_minValue_edit.Value--;
                return;
            }
        }

        private void numUpDwn_maxValue_edit_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_maxValue_edit.Value < numUpDwn_minValue_edit.Value)
            {
                numUpDwn_maxValue_edit.Value++;
                return;
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                Criterion_QuantitativeEvaluation selectedCriterion = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.First(x => x.Value.String == m_selectedCriterionString).Value;

                eValueType valueType = default;
                if (rdBtn_integer_edit.Checked)
                    valueType = eValueType.Integer;
                if (rdBtn_decimal_edit.Checked)
                    valueType = eValueType.Decimal;

                if (DocumentLoader.EditQuantitativeEvaluationCriterionInCSV(selectedCriterion, txtBx_string_edit.Text, valueType, numUpDwn_minValue_edit.Value, numUpDwn_maxValue_edit.Value))
                {
                    MessageBox.Show("Edited criterion successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit criterion! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCriterionId = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.First(x => x.Value.String == m_selectedCriterionString).Key;
                if (DocumentLoader.DeleteQuantitativeEvaluationCriterionFromCSV(selectedCriterionId))
                {
                    MessageBox.Show("Deleted criterion successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete criterion! Please try again.");
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
                columns.Add("Criterion", "Criterion");
                columns.Add("ValueType", "Value Type");
                columns.Add("MinValue", "Min Value");
                columns.Add("MaxValue", "Max Value");
            }
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_string_edit.Text = (string)(SelectedCells["Criterion"].Value);
                eValueType valueType = ((string)(SelectedCells["ValueType"].Value)).ToCorrespondingEnumValue<eValueType>();
                if (valueType == eValueType.Integer)
                    rdBtn_integer_edit.Checked = true;
                else
                    rdBtn_decimal_edit.Checked = true;

                numUpDwn_maxValue_edit.Value = Convert.ToDecimal((string)(SelectedCells["MaxValue"].Value));
                numUpDwn_minValue_edit.Value = Convert.ToDecimal((string)(SelectedCells["MinValue"].Value));
            }
            else
            {
                txtBx_string_edit.Clear();
                rdBtn_integer_delete.Checked = true;
                numUpDwn_minValue_edit.Value = numUpDwn_minValue_edit.Minimum;
                numUpDwn_maxValue_edit.Value = numUpDwn_minValue_edit.Minimum;
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_string_delete.Text = (string)(SelectedCells["Criterion"].Value);
                eValueType valueType = ((string)(SelectedCells["ValueType"].Value)).ToCorrespondingEnumValue<eValueType>();
                if (valueType == eValueType.Integer)
                    rdBtn_integer_delete.Checked = true;
                else
                    rdBtn_decimal_delete.Checked = true;

                numUpDwn_maxValue_delete.Value = Convert.ToDecimal((string)(SelectedCells["MaxValue"].Value));
                numUpDwn_minValue_delete.Value = Convert.ToDecimal((string)(SelectedCells["MinValue"].Value));
            }
            else
            {
                txtBx_string_delete.Clear();
                rdBtn_integer_delete.Checked = true;
                numUpDwn_minValue_delete.Value = numUpDwn_minValue_delete.Minimum;
                numUpDwn_maxValue_delete.Value = numUpDwn_minValue_delete.Minimum;
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

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_string_add.Clear();
            rdBtn_integer_add.Checked = true;
            numUpDwn_minValue_add.Value = numUpDwn_minValue_delete.Minimum;
            numUpDwn_maxValue_add.Value = numUpDwn_minValue_delete.Minimum;
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var criterionDictionary = DataContainer.Instance.QuantitativeEvaluationCriterionDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? criterionDictionary : criterionDictionary.Where(x => x.Value.String.Contains(filterText)
                                                                                                                                || x.Value.EvaluationValueType.ToString().Contains(filterText)
                                                                                                                                || x.Value.Min.ToString().Contains(filterText)
                                                                                                                                || x.Value.Max.ToString().Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    Criterion_QuantitativeEvaluation criterion = element.Value;
                    var row = new string[] { criterion.String, criterion.EvaluationValueType.ToString(), criterion.Min.ToString(), criterion.Max.ToString() };
                    rows.Add(row);

                    if (m_selectedCriterionString != string.Empty && row[0] == m_selectedCriterionString)
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

            string @string = txtBx_string_add.Text;
            if (string.IsNullOrWhiteSpace(@string))
                errorMessages.Add("Enter a string!");
            else if (DataContainer.Instance.QuantitativeEvaluationCriterionDictionary.Any(x => x.Value.String == @string))
                errorMessages.Add("An criterion with the same string already exists!");

            if (numUpDwn_minValue_add.Value == numUpDwn_maxValue_add.Value)
                errorMessages.Add("Min value and max value must not be equal!");

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

            string @string = txtBx_string_edit.Text;
            if (string.IsNullOrWhiteSpace(@string))
                errorMessages.Add("Enter a string!");

            if (numUpDwn_minValue_edit.Value == numUpDwn_maxValue_edit.Value)
                errorMessages.Add("Min value and max value must not be equal!");

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
            string valueType = (rdBtn_integer_edit.Checked) ? eValueType.Integer.ToString() : eValueType.Decimal.ToString();

            return txtBx_string_edit.Text != (string)(SelectedCells["Criterion"].Value)
                || valueType != (string)(SelectedCells["ValueType"].Value)
                || numUpDwn_minValue_edit.Value != Convert.ToDecimal((string)(SelectedCells["MinValue"].Value))
                || numUpDwn_maxValue_edit.Value != Convert.ToDecimal((string)(SelectedCells["MaxValue"].Value));
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
