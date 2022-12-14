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
    public partial class EvaluationColorSetForm : Form
    {
        public EvaluationColorSetForm()
        {
            InitializeComponent();
        }

        private void EvaluationColorSetForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeValueColorTable(dgv_valueColor_view, false);
            InitializeValueColorTable(dgv_valueColor_add, true);
            InitializeValueColorTable(dgv_valueColor_edit, true);
            InitializeValueColorTable(dgv_valueColor_delete, false);

            RefreshMainViewTable();
            dgv_main.ClearSelection();

            RefreshValueColorTable_Add();

            clrPkrCtrl_add.SelectFirstCustomColor();
            ActivateColorPickerEvents();

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
        private string m_selectedEvaluationColorSetName = string.Empty;
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
                m_selectedEvaluationColorSetName = String.Empty;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedEvaluationColorSetName = (string)(SelectedCells["Name"].Value);
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
        private int m_previousMinValue_add = default;
        private void numUpDwn_minValue_add_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_minValue_add.Value > numUpDwn_maxValue_add.Value)
            {
                numUpDwn_minValue_add.Value--;
                return;
            }

            int value = Convert.ToInt32(numUpDwn_minValue_add.Value);

            if (value > m_previousMinValue_add)
            {
                foreach (DataGridViewRow row in dgv_valueColor_add.Rows)
                {
                    if (Convert.ToInt32((string)(row.Cells["Value"].Value)) < value)
                        dgv_valueColor_add.Rows.Remove(row);
                }
            }
            else if (value < m_previousMinValue_add)
            {
                for (int i = m_previousMinValue_add - 1; i >= value; i--)
                {
                    dgv_valueColor_add.Rows.Add(new string[] { i.ToString(), "" });
                }

                dgv_valueColor_add.Sort(new RowComparer(dgv_valueColor_add.SortOrder));
            }

            m_previousMinValue_add = value;
        }

        private int m_previousMaxValue_add = default;
        private void numUpDwn_maxValue_add_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_maxValue_add.Value < numUpDwn_minValue_add.Value)
            {
                numUpDwn_maxValue_add.Value++;
                return;
            }

            int value = Convert.ToInt32(numUpDwn_maxValue_add.Value);

            if (value < m_previousMaxValue_add)
            {
                foreach (DataGridViewRow row in dgv_valueColor_add.Rows)
                {
                    if (Convert.ToInt32((string)(row.Cells["Value"].Value)) > value)
                        dgv_valueColor_add.Rows.Remove(row);
                }
            }
            else if (value > m_previousMaxValue_add)
            {
                for (int i = m_previousMaxValue_add + 1; i <= value; i++)
                {
                    dgv_valueColor_add.Rows.Add(new string[] { i.ToString(), "" });
                }

                dgv_valueColor_add.Sort(new RowComparer(dgv_valueColor_add.SortOrder));
            }

            m_previousMaxValue_add = value;
        }

        private void dgv_valueColor_add_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_valueColor_add.SelectedRows.Count == 0)
                return;

            var row = dgv_valueColor_add.SelectedRows[0];

            txtBx_selectedRowText_add.Text = (string)(row.Cells["Text"].Value);
            Color color = row.Cells["Color"].Style.BackColor;
            clrPkrCtrl_add.SelectedColor = (color != Color.Empty) ? color : Color.White;
        }

        private void txtBx_selectedRowText_add_TextChanged(object sender, EventArgs e)
        {
            var row = dgv_valueColor_add.SelectedRows[0];

            row.Cells["Text"].Value = txtBx_selectedRowText_add.Text;
        }

        private void OnSelectedColorChanged_Add(object sender, EventArgs e)
        {
            dgv_valueColor_add.SelectedRows[0].Cells["Color"].Style.BackColor = clrPkrCtrl_add.SelectedColor;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                if (DocumentLoader.AddEvaluationColorSetToCSV(txtBx_name_add.Text, TableToList(dgv_valueColor_add)) != default)
                {
                    MessageBox.Show("Added new evaluation color set.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new evaluation color set! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private int m_previousMinValue_edit = default;
        private void numUpDwn_minValue_edit_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_minValue_edit.Value > numUpDwn_maxValue_edit.Value)
            {
                numUpDwn_minValue_edit.Value--;
                return;
            }

            int value = Convert.ToInt32(numUpDwn_minValue_edit.Value);

            if (value > m_previousMinValue_edit)
            {
                foreach (DataGridViewRow row in dgv_valueColor_edit.Rows)
                {
                    if (Convert.ToInt32((string)(row.Cells["Value"].Value)) < value)
                        dgv_valueColor_edit.Rows.Remove(row);
                }
            }
            else if (value < m_previousMinValue_edit)
            {
                for (int i = m_previousMinValue_edit - 1; i >= value; i--)
                {
                    dgv_valueColor_edit.Rows.Add(new string[] { i.ToString(), "" });
                }

                dgv_valueColor_edit.Sort(new RowComparer(dgv_valueColor_edit.SortOrder));
            }

            m_previousMinValue_edit = value;
        }

        private int m_previousMaxValue_edit = default;
        private void numUpDwn_maxValue_edit_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDwn_maxValue_edit.Value < numUpDwn_minValue_edit.Value)
            {
                numUpDwn_maxValue_edit.Value++;
                return;
            }

            int value = Convert.ToInt32(numUpDwn_maxValue_edit.Value);

            if (value < m_previousMaxValue_edit)
            {
                foreach (DataGridViewRow row in dgv_valueColor_edit.Rows)
                {
                    if (Convert.ToInt32((string)(row.Cells["Value"].Value)) > value)
                        dgv_valueColor_edit.Rows.Remove(row);
                }
            }
            else if (value > m_previousMaxValue_edit)
            {
                for (int i = m_previousMaxValue_edit + 1; i <= value; i++)
                {
                    dgv_valueColor_edit.Rows.Add(new string[] { i.ToString(), "" });
                }

                dgv_valueColor_edit.Sort(new RowComparer(dgv_valueColor_edit.SortOrder));
            }

            m_previousMaxValue_edit = value;
        }

        private void dgv_valueColor_edit_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_valueColor_edit.SelectedRows.Count == 0)
                return;

            var row = dgv_valueColor_edit.SelectedRows[0];

            txtBx_selectedRowText_edit.Text = (string)(row.Cells["Text"].Value);
            Color color = row.Cells["Color"].Style.BackColor;
            clrPkrCtrl_edit.SelectedColor = (color != Color.Empty) ? color : Color.White;
        }

        private void txtBx_selectedRowText_edit_TextChanged(object sender, EventArgs e)
        {
            var row = dgv_valueColor_edit.SelectedRows[0];

            row.Cells["Text"].Value = txtBx_selectedRowText_edit.Text;
        }
        private void OnSelectedColorChanged_Edit(object sender, EventArgs e)
        {
            dgv_valueColor_edit.SelectedRows[0].Cells["Color"].Style.BackColor = clrPkrCtrl_edit.SelectedColor;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                EvaluationColorSet selectedEvaluationColorSet = DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == m_selectedEvaluationColorSetName).Value;
                if (DocumentLoader.EditEvaluationColorSetInCSV(selectedEvaluationColorSet, txtBx_name_edit.Text, TableToList(dgv_valueColor_edit)))
                {
                    MessageBox.Show("Edited evaluation color set successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit evaluation color set! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string selectedEvaluationColorSetName = (string)(SelectedCells[0].Value);
                int selectedEvaluationColorSetId = DataContainer.Instance.EvaluationColorSetDictionary.First(x => x.Value.Name == selectedEvaluationColorSetName).Key;
                if (DocumentLoader.DeleteEvaluationColorSetFromCSV(selectedEvaluationColorSetId))
                {
                    MessageBox.Show("Deleted evaluation color set successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete evaluation color set! Please try again.");
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
                columns.Add("Levels", "Levels");
                columns.Add("MinValue", "Min Value");
                columns.Add("MaxValue", "Max Value");
            }
        }

        private void InitializeValueColorTable(DataGridView _dgv, bool _selectable)
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

                if (!_selectable)
                    _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }
        #endregion

        #region Component Fill
        private void FillViewTabComponents()
        {
            RefreshValueColorTable_View();
        }

        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_name_edit.Text = (string)(SelectedCells["Name"].Value);
                numUpDwn_maxValue_edit.Value = Convert.ToInt32(SelectedCells["MaxValue"].Value);
                numUpDwn_minValue_edit.Value = Convert.ToInt32(SelectedCells["MinValue"].Value);
            }
            else
            {
                txtBx_name_edit.Clear();
                numUpDwn_minValue_edit.Value = numUpDwn_minValue_edit.Minimum;
                numUpDwn_maxValue_edit.Value = numUpDwn_minValue_edit.Minimum;
            }

            RefreshValueColorTable_Edit();
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_name_delete.Text = (string)(SelectedCells["Name"].Value);
                numUpDwn_maxValue_delete.Value = Convert.ToInt32(SelectedCells["MaxValue"].Value);
                numUpDwn_minValue_delete.Value = Convert.ToInt32(SelectedCells["MinValue"].Value);
            }
            else
            {
                txtBx_name_delete.Clear();
                numUpDwn_minValue_delete.Value = numUpDwn_minValue_delete.Minimum;
                numUpDwn_maxValue_delete.Value = numUpDwn_minValue_delete.Minimum;
            }

            RefreshValueColorTable_Delete();
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }

        private void RefreshValueColorTable_View()
        {
            // Set rows
            var rows = dgv_valueColor_view.Rows;
            rows.Clear();

            if (SelectedCells != null)
            {
                string evaluationColorSetName = (string)(SelectedCells["Name"].Value);
                EvaluationColorSet evaluationColorSet = DataContainer.Instance.EvaluationColorSetDictionary.Values.First(x => x.Name == evaluationColorSetName);

                foreach (var valueColor in evaluationColorSet.ValueColors)
                {
                    int rowIndex = rows.Add(new string[] { valueColor.NumericValue.ToString(), valueColor.TextValue, "" });
                    rows[rowIndex].Cells["Color"].Style.BackColor = valueColor.Color;
                }
            }
        }

        private void RefreshValueColorTable_Add()
        {
            // Set rows
            var rows = dgv_valueColor_add.Rows;
            rows.Clear();
            rows.Add(new string[] { numUpDwn_minValue_add.Value.ToString(), "" });

            // Set initial sort direction
            dgv_valueColor_add.Sort(dgv_valueColor_add.Columns["Value"], ListSortDirection.Descending);
        }
        private void RefreshValueColorTable_Edit()
        {
            // Set rows
            var rows = dgv_valueColor_edit.Rows;
            rows.Clear();
            rows.Load(dgv_valueColor_view.Rows);

            // Set initial sort direction
            dgv_valueColor_edit.Sort(dgv_valueColor_edit.Columns["Value"], ListSortDirection.Descending);
        }

        private void RefreshValueColorTable_Delete()
        {
            // Set rows
            var rows = dgv_valueColor_delete.Rows;
            rows.Clear();
            rows.Load(dgv_valueColor_view.Rows);

            // Set initial sort direction
            dgv_valueColor_delete.Sort(dgv_valueColor_delete.Columns["Value"], ListSortDirection.Descending);
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_name_add.Clear();
            RefreshValueColorTable_Add();
            numUpDwn_minValue_add.Value = numUpDwn_maxValue_add.Value = numUpDwn_minValue_add.Minimum;
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var evaluationColorSetDictionary = DataContainer.Instance.EvaluationColorSetDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? evaluationColorSetDictionary : evaluationColorSetDictionary.Where(x => x.Value.Name.Contains(filterText)
                                                                                                                                              || (x.Value.ValueColors.Max().NumericValue - x.Value.ValueColors.Min().NumericValue).ToString().Contains(filterText)
                                                                                                                                              || x.Value.ValueColors.Min().NumericValue.ToString().Contains(filterText)
                                                                                                                                              || x.Value.ValueColors.Max().NumericValue.ToString().Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    EvaluationColorSet evaluationColorSet = element.Value;
                    int minValue = evaluationColorSet.ValueColors.Min().NumericValue;
                    int maxValue = evaluationColorSet.ValueColors.Max().NumericValue;
                    int levels = (maxValue - minValue) + 1;
                    var row = new string[] { evaluationColorSet.Name, levels.ToString(), minValue.ToString(), maxValue.ToString() };
                    rows.Add(row);

                    if (m_selectedEvaluationColorSetName != string.Empty && row[0] == m_selectedEvaluationColorSetName)
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
            else if (DataContainer.Instance.EvaluationColorSetDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("An evaluation color set with the same name already exists!");

            int minValue = Convert.ToInt32(numUpDwn_minValue_add.Value);
            int maxValue = Convert.ToInt32(numUpDwn_maxValue_add.Value);
            if (maxValue - minValue == 0)
                errorMessages.Add("Please define a larger range of values!");

            List<Tuple<int, string, Color>> valueColors = TableToList(dgv_valueColor_add);
            if (valueColors.Any(x => string.IsNullOrWhiteSpace(x.Item2)))
                errorMessages.Add("Please specify text for all value colors!");
            if (valueColors.Any(x => x.Item3.A == 0))
                errorMessages.Add("Opacity should not be 0 for any color!");
            if (valueColors.Count != valueColors.Select(x => x.Item3).Distinct().Count())
                errorMessages.Add("Same color must not be repeated!");

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
            if (name != (string)(SelectedCells[0].Value)) // If name is to be changed
            {
                if (string.IsNullOrWhiteSpace(name))
                    errorMessages.Add("Enter a name!");
                else if (DataContainer.Instance.EvaluationColorSetDictionary.Any(x => x.Value.Name == name))
                    errorMessages.Add("An evaluation color set with the same name already exists!");
            }

            int minValue = Convert.ToInt32(numUpDwn_minValue_edit.Value);
            int maxValue = Convert.ToInt32(numUpDwn_maxValue_edit.Value);
            if (maxValue - minValue == 0)
                errorMessages.Add("Please define a larger range of values!");

            List<Tuple<int, string, Color>> valueColors = TableToList(dgv_valueColor_edit);
            if (valueColors.Any(x => string.IsNullOrWhiteSpace(x.Item2)))
                errorMessages.Add("Please specify text for all value colors!");
            if (valueColors.Any(x => x.Item3.A == 0))
                errorMessages.Add("Opacity should not be 0 for any color!");
            if (valueColors.Count != valueColors.Select(x => x.Item3).Distinct().Count())
                errorMessages.Add("Same color must not be repeated!");

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
                || numUpDwn_minValue_edit.Value.ToString() != (string)(SelectedCells["MinValue"].Value)
                || numUpDwn_maxValue_edit.Value.ToString() != (string)(SelectedCells["MaxValue"].Value)
                || !dgv_valueColor_edit.AllRowsEqual(dgv_valueColor_view);
        }
        #endregion

        #region Shared
        private void ActivateColorPickerEvents()
        {
            clrPkrCtrl_add.SelectedColorChanged += new EventHandler(OnSelectedColorChanged_Add);
            clrPkrCtrl_edit.SelectedColorChanged += new EventHandler(OnSelectedColorChanged_Edit);
        }

        private List<Tuple<int, string, Color>> TableToList(DataGridView _dgv)
        {
            List<Tuple<int, string, Color>> result = new List<Tuple<int, string, Color>>();

            try
            {
                foreach (DataGridViewRow row in _dgv.Rows)
                {
                    int numericValue = Convert.ToInt32((string)(row.Cells["Value"].Value));
                    string textValue = (string)(row.Cells["Text"].Value);
                    Color color = row.Cells["Color"].Style.BackColor;

                    result.Add(new Tuple<int, string, Color>(numericValue, textValue, color));
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
