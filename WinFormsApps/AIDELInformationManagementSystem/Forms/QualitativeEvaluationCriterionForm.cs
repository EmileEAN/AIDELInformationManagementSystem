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
    public partial class QualitativeEvaluationCriterionForm : Form
    {
        public QualitativeEvaluationCriterionForm()
        {
            InitializeComponent();
        }

        private void QualitativeEvaluationCriterionForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeEvaluationColorSetComboBox(cmbBx_evaluationColorSet_add);
            InitializeEvaluationColorSetComboBox(cmbBx_evaluationColorSet_edit);
            InitializeEvaluationColorSetComboBox(cmbBx_evaluationColorSet_delete);

            InitializeValueColorTable(dgv_valueColors_add);
            InitializeValueColorTable(dgv_valueColors_edit);
            InitializeValueColorTable(dgv_valueColors_delete);

            RefreshMainViewTable();
            dgv_main.ClearSelection();

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
        private void cmbBx_evaluationColorSet_add_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshValueColorsTable(cmbBx_evaluationColorSet_add, dgv_valueColors_add);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                EvaluationColorSet evaluationColorSet = DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == cmbBx_evaluationColorSet_add.SelectedItem.ToString()).Value;

                if (DocumentLoader.AddQualitativeEvaluationCriterionToCSV(txtBx_string_add.Text, evaluationColorSet) != default)
                {
                    MessageBox.Show("Added new criterion.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                    RefreshEvaluationColorSetComboBox(cmbBx_evaluationColorSet_add);
                    RefreshEvaluationColorSetComboBox(cmbBx_evaluationColorSet_edit);
                    RefreshEvaluationColorSetComboBox(cmbBx_evaluationColorSet_delete);
                }
                else
                    MessageBox.Show("Failed to add new criterion! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void cmbBx_evaluationColorSet_edit_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshValueColorsTable(cmbBx_evaluationColorSet_edit, dgv_valueColors_edit);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                Criterion_QualitativeEvaluation selectedCriterion = DataContainer.Instance.QualitativeEvaluationCriterionDictionary.First(x => x.Value.String == m_selectedCriterionString).Value;

                EvaluationColorSet evaluationColorSet = DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == cmbBx_evaluationColorSet_edit.SelectedItem.ToString()).Value;

                if (DocumentLoader.EditQualitativeEvaluationCriterionInCSV(selectedCriterion, txtBx_string_edit.Text, evaluationColorSet))
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
        private void cmbBx_evaluationColorSet_delete_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshValueColorsTable(cmbBx_evaluationColorSet_delete, dgv_valueColors_delete);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCriterionId = DataContainer.Instance.QualitativeEvaluationCriterionDictionary.First(x => x.Value.String == m_selectedCriterionString).Key;
                if (DocumentLoader.DeleteQualitativeEvaluationCriterionFromCSV(selectedCriterionId))
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
                columns.Add("EvaluationColorSet", "Evaluation Color Set");
                columns.Add("Levles", "Levels");
                columns.Add("MinValue", "Min Value");
                columns.Add("MaxValue", "Max Value");
            }
        }

        private void InitializeEvaluationColorSetComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var evaluationColorSet in DataContainer.Instance.EvaluationColorSetDictionary.Values)
            {
                items.Add(evaluationColorSet.Name);
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeValueColorTable(DataGridView _dgv)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;

                columns.Add("Value", "Value");
                var valueColumn = columns["Value"];
                valueColumn.Width = 135;
                valueColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                valueColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                columns.Add("Text", "Text");
                var textColumn = columns["Text"];
                textColumn.Width = 150;
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                columns.Add("Color", "Color");
                var colorColumn = columns["Color"];
                colorColumn.Width = 135;
                colorColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colorColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_string_edit.Text = (string)(SelectedCells["Criterion"].Value);
                cmbBx_evaluationColorSet_edit.SelectedIndex = cmbBx_evaluationColorSet_edit.FindStringExact((string)(SelectedCells["EvaluationColorSet"].Value));
            }
            else
            {
                txtBx_string_edit.Clear();
                cmbBx_evaluationColorSet_edit.SelectedIndex = cmbBx_evaluationColorSet_edit.FindStringExact(CoreValues.ComboBox_DefaultString);
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_string_delete.Text = (string)(SelectedCells["Criterion"].Value);
                cmbBx_evaluationColorSet_delete.SelectedIndex = cmbBx_evaluationColorSet_delete.FindStringExact((string)(SelectedCells["EvaluationColorSet"].Value));
            }
            else
            {
                txtBx_string_delete.Clear();
                cmbBx_evaluationColorSet_delete.SelectedIndex = cmbBx_evaluationColorSet_delete.FindStringExact(CoreValues.ComboBox_DefaultString);
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }

        private void RefreshEvaluationColorSetComboBox(ComboBox _cmbBx)
        {
            InitializeEvaluationColorSetComboBox(_cmbBx);
        }

        private void RefreshValueColorsTable(ComboBox _cmbBx, DataGridView _dgv)
        {
            string selectedEvaluationColorSetName = _cmbBx.SelectedItem.ToString();

            EvaluationColorSet evaluationColorSet = (selectedEvaluationColorSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == selectedEvaluationColorSetName).Value :
                    null;
                    
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (evaluationColorSet == null)
                return;

            int rowIndex = 0;
            foreach (var valueColor in evaluationColorSet.ValueColors)
            {
                rows.Add(new string[] { valueColor.NumericValue.ToString(), valueColor.TextValue, "" });
                rows[rowIndex].Cells["Color"].Style.BackColor = valueColor.Color;

                rowIndex++;
            }
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_string_add.Clear();
            cmbBx_evaluationColorSet_add.SelectedIndex = cmbBx_evaluationColorSet_add.FindStringExact(CoreValues.ComboBox_DefaultString);
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var criterionDictionary = DataContainer.Instance.QualitativeEvaluationCriterionDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? criterionDictionary : criterionDictionary.Where(x => x.Value.String.Contains(filterText)
                                                                                                                                || x.Value.EvaluationColorSet.Name.Contains(filterText)
                                                                                                                                || (x.Value.EvaluationColorSet.ValueColors.Max().NumericValue - x.Value.EvaluationColorSet.ValueColors.Min().NumericValue).ToString().Contains(filterText)
                                                                                                                                || x.Value.EvaluationColorSet.ValueColors.Min().NumericValue.ToString().Contains(filterText)
                                                                                                                                || x.Value.EvaluationColorSet.ValueColors.Max().NumericValue.ToString().Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    Criterion_QualitativeEvaluation criterion = element.Value;
                    EvaluationColorSet evaluationColorSet = criterion.EvaluationColorSet;
                    List<ValueColor> valueColors = evaluationColorSet.ValueColors;
                    int minValue = valueColors.Min().NumericValue;
                    int maxValue = valueColors.Max().NumericValue;
                    int levels = (maxValue - minValue) + 1;
                    var row = new string[] { criterion.String, evaluationColorSet.Name, levels.ToString(), minValue.ToString(), maxValue.ToString() };
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
            else if (DataContainer.Instance.QualitativeEvaluationCriterionDictionary.Any(x => x.Value.String == @string))
                errorMessages.Add("An criterion with the same string already exists!");

            EvaluationColorSet evaluationColorSet = null;
            string evaluationColorSetName = cmbBx_evaluationColorSet_add.SelectedItem.ToString();
            if (evaluationColorSetName == CoreValues.ComboBox_DefaultString)
                errorMessages.Add("Select an evaluation color set!");
            else
                evaluationColorSet = DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == evaluationColorSetName).Value;

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
            else if (DataContainer.Instance.EvaluationColorSetDictionary.Any(x => x.Value.Name == @string))
                errorMessages.Add("An criterion with the same string already exists!");

            EvaluationColorSet evaluationColorSet = null;
            string evaluationColorSetName = cmbBx_evaluationColorSet_edit.SelectedItem.ToString();
            if (evaluationColorSetName == CoreValues.ComboBox_DefaultString)
                errorMessages.Add("Select an evaluation color set!");
            else
                evaluationColorSet = DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == evaluationColorSetName).Value;

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
            return txtBx_string_edit.Text != (string)(SelectedCells["Criterion"].Value)
                || cmbBx_evaluationColorSet_edit.SelectedItem.ToString() != (string)(SelectedCells["EvaluationColorSet"].Value);
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
