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
    public partial class EvaluationFileColumnsForm : Form
    {
        public EvaluationFileColumnsForm()
        {
            InitializeComponent();
        }

        private void EvaluationFileColumnsForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

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
        private string m_selectedColumnSetId = string.Empty;
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
        private void dgv_main_SelectionChanged(object sender, EventArgs e)
        {
            if (m_addingRows)
                return;

            m_selectionChanged = true;

            if (dgv_main.SelectedRows.Count == 0)
            {
                m_selectedRow = null;
                m_selectedColumnSetId = default;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedColumnSetId = (string)(SelectedCells["Id"].Value);
            }

            ClearInput_Edit();
            ClearInput_Delete();

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
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                List<string> qntAdd_columnLetters = new List<string>();
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_1_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_1_add.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_2_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_2_add.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_3_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_3_add.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_4_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_4_add.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_5_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_5_add.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_6_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_6_add.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_7_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_7_add.Text);

                if (DocumentLoader.AddEvaluationFileCriterionColumnsRangeSetToCSV(txtBx_name_add.Text,
                    txtBx_qntFP_start_add.Text, txtBx_qntFP_end_add.Text,
                    txtBx_qntMid_start_add.Text, txtBx_qntMid_end_add.Text,
                    txtBx_qntFin_start_add.Text, txtBx_qntFin_end_add.Text,
                    qntAdd_columnLetters,
                    txtBx_qlFP_start_add.Text, txtBx_qlFP_end_add.Text,
                    txtBx_qlMid_start_add.Text, txtBx_qlMid_end_add.Text,
                    txtBx_qlFin_start_add.Text, txtBx_qlFin_end_add.Text))
                {
                    MessageBox.Show("Added new column set.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new column set! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                DataContainer container = DataContainer.Instance;

                int selectedColumnSetId = Convert.ToInt32((string)(SelectedCells[0].Value));
                EvaluationFileCriterionColumnsRangeSet selectedRangeSet = container.EvaluationFileCriterionColumnsRangeSetDictionary[selectedColumnSetId];

                List<string> qntAdd_columnLetters = new List<string>();
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_1_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_1_edit.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_2_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_2_edit.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_3_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_3_edit.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_4_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_4_edit.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_5_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_5_edit.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_6_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_6_edit.Text);
                if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_7_edit.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_7_edit.Text);

                if (DocumentLoader.EditEvaluationFileCriterionColumnsRangeSetInCSV(selectedRangeSet, txtBx_name_edit.Text, 
                    txtBx_qntFP_start_edit.Text, txtBx_qntFP_end_edit.Text,
                    txtBx_qntMid_start_edit.Text, txtBx_qntMid_end_edit.Text,
                    txtBx_qntFin_start_edit.Text, txtBx_qntFin_end_edit.Text,
                    qntAdd_columnLetters,
                    txtBx_qlFP_start_edit.Text, txtBx_qlFP_end_edit.Text,
                    txtBx_qlMid_start_edit.Text, txtBx_qlMid_end_edit.Text,
                    txtBx_qlFin_start_edit.Text, txtBx_qlFin_end_edit.Text))
                {
                    MessageBox.Show("Edited column set successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit column set! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            //DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    //int selectedStudentAccountNumber = Convert.ToInt32((string)(SelectedCells[0].Value));
            //    //Student selectedStudent = DataContainer.Instance.StudentList.First(x => x.AccountNumber == selectedStudentAccountNumber);
            //    //if (DocumentLoader.DeleteStudentFromCSV(selectedStudent))
            //    //{
            //    //    MessageBox.Show("Deleted column set successfully.");

            //    //    RefreshMainViewTable();
            //    //}
            //    //else
            //    //    MessageBox.Show("Failed to delete column set! Please try again.");
            //}
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
                columns.Add("Id", "Id");
                columns.Add("Name", "Name");
                columns.Add("Rng1", "(Quantitative) First Partial");
                columns.Add("Rng2", "(Quantitative) Midterm");
                columns.Add("Rng3", "(Quantitative) Finals");
                columns.Add("Rng4", "(Quantitative) Additional");
                columns.Add("Rng5", "(Qualitative) First Partial");
                columns.Add("Rng6", "(Qualitative) Midterm");
                columns.Add("Rng7", "(Qualitative) Finals");
            }
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_name_edit.Text = (string)(SelectedCells["Name"].Value);

                string[] qntFP = ((string)(SelectedCells["Rng1"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qntMid = ((string)(SelectedCells["Rng2"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qntFin = ((string)(SelectedCells["Rng3"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qntAdd = ((string)(SelectedCells["Rng4"].Value)).Split(new string[] { "," }, StringSplitOptions.None);
                string[] qlFP = ((string)(SelectedCells["Rng5"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qlMid = ((string)(SelectedCells["Rng6"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qlFin = ((string)(SelectedCells["Rng7"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);

                if (qntFP.Length > 1)
                {
                    txtBx_qntFP_start_edit.Text = qntFP[0];
                    txtBx_qntFP_end_edit.Text = qntFP[1];
                }
                txtBx_qntMid_start_edit.Text = qntMid[0];
                txtBx_qntMid_end_edit.Text = qntMid[1];
                txtBx_qntFin_start_edit.Text = qntFin[0];
                txtBx_qntFin_end_edit.Text = qntFin[1];
                for (int i = 0; i < qntAdd.Length; i++)
                {
                    if (i == 0) txtBx_qntAdd_1_edit.Text = qntAdd[i];
                    if (i == 1) txtBx_qntAdd_2_edit.Text = qntAdd[i];
                    if (i == 2) txtBx_qntAdd_3_edit.Text = qntAdd[i];
                    if (i == 3) txtBx_qntAdd_4_edit.Text = qntAdd[i];
                    if (i == 4) txtBx_qntAdd_5_edit.Text = qntAdd[i];
                    if (i == 5) txtBx_qntAdd_6_edit.Text = qntAdd[i];
                    if (i == 6) txtBx_qntAdd_7_edit.Text = qntAdd[i];
                }
                if (qlFP.Length > 1)
                {
                    txtBx_qlFP_start_edit.Text = qlFP[0];
                    txtBx_qlFP_end_edit.Text = qlFP[1];
                }
                txtBx_qlMid_start_edit.Text = qlMid[0];
                txtBx_qlMid_end_edit.Text = qlMid[1];
                txtBx_qlFin_start_edit.Text = qlFin[0];
                txtBx_qlFin_end_edit.Text = qlFin[1];
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_name_delete.Text = (string)(SelectedCells["Name"].Value);

                string[] qntFP = ((string)(SelectedCells["Rng1"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qntMid = ((string)(SelectedCells["Rng2"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qntFin = ((string)(SelectedCells["Rng3"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qntAdd = ((string)(SelectedCells["Rng4"].Value)).Split(new string[] { "," }, StringSplitOptions.None);
                string[] qlFP = ((string)(SelectedCells["Rng5"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qlMid = ((string)(SelectedCells["Rng6"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);
                string[] qlFin = ((string)(SelectedCells["Rng7"].Value)).Split(new string[] { " -> " }, StringSplitOptions.None);

                if (qntFP.Length > 1)
                {
                    txtBx_qntFP_start_delete.Text = qntFP[0];
                    txtBx_qntFP_end_delete.Text = qntFP[1];
                }
                txtBx_qntMid_start_delete.Text = qntMid[0];
                txtBx_qntMid_end_delete.Text = qntMid[1];
                txtBx_qntFin_start_delete.Text = qntFin[0];
                txtBx_qntFin_end_delete.Text = qntFin[1];
                for (int i = 0; i < qntAdd.Length; i++)
                {
                    if (i == 0) txtBx_qntAdd_1_delete.Text = qntAdd[i];
                    if (i == 1) txtBx_qntAdd_2_delete.Text = qntAdd[i];
                    if (i == 2) txtBx_qntAdd_3_delete.Text = qntAdd[i];
                    if (i == 3) txtBx_qntAdd_4_delete.Text = qntAdd[i];
                    if (i == 4) txtBx_qntAdd_5_delete.Text = qntAdd[i];
                    if (i == 5) txtBx_qntAdd_6_delete.Text = qntAdd[i];
                    if (i == 6) txtBx_qntAdd_7_delete.Text = qntAdd[i];
                }
                if (qlFP.Length > 1)
                {
                    txtBx_qlFP_start_delete.Text = qlFP[0];
                    txtBx_qlFP_end_delete.Text = qlFP[1];
                }
                txtBx_qlMid_start_delete.Text = qlMid[0];
                txtBx_qlMid_end_delete.Text = qlMid[1];
                txtBx_qlFin_start_delete.Text = qlFin[0];
                txtBx_qlFin_end_delete.Text = qlFin[1];
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ResetTable();
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            txtBx_name_add.Clear();
            txtBx_qntFP_start_add.Clear();
            txtBx_qntMid_start_add.Clear();
            txtBx_qntFin_start_add.Clear();
            txtBx_qlFP_start_add.Clear();
            txtBx_qlMid_start_add.Clear();
            txtBx_qlFin_start_add.Clear();
            txtBx_qntFP_end_add.Clear();
            txtBx_qntMid_end_add.Clear();
            txtBx_qntFin_end_add.Clear();
            txtBx_qlFP_end_add.Clear();
            txtBx_qlMid_end_add.Clear();
            txtBx_qlFin_end_add.Clear();

            {
                txtBx_qntAdd_1_add.Clear();
                txtBx_qntAdd_2_add.Clear();
                txtBx_qntAdd_3_add.Clear();
                txtBx_qntAdd_4_add.Clear();
                txtBx_qntAdd_5_add.Clear();
                txtBx_qntAdd_6_add.Clear();
                txtBx_qntAdd_7_add.Clear();
            }
        }

        private void ClearInput_Edit()
        {
            txtBx_name_edit.Clear();
            txtBx_qntFP_start_edit.Clear();
            txtBx_qntMid_start_edit.Clear();
            txtBx_qntFin_start_edit.Clear();
            txtBx_qlFP_start_edit.Clear();
            txtBx_qlMid_start_edit.Clear();
            txtBx_qlFin_start_edit.Clear();
            txtBx_qntFP_end_edit.Clear();
            txtBx_qntMid_end_edit.Clear();
            txtBx_qntFin_end_edit.Clear();
            txtBx_qlFP_end_edit.Clear();
            txtBx_qlMid_end_edit.Clear();
            txtBx_qlFin_end_edit.Clear();

            {
                txtBx_qntAdd_1_edit.Clear();
                txtBx_qntAdd_2_edit.Clear();
                txtBx_qntAdd_3_edit.Clear();
                txtBx_qntAdd_4_edit.Clear();
                txtBx_qntAdd_5_edit.Clear();
                txtBx_qntAdd_6_edit.Clear();
                txtBx_qntAdd_7_edit.Clear();
            }
        }

        private void ClearInput_Delete()
        {
            txtBx_name_delete.Clear();
            txtBx_qntFP_start_delete.Clear();
            txtBx_qntMid_start_delete.Clear();
            txtBx_qntFin_start_delete.Clear();
            txtBx_qlFP_start_delete.Clear();
            txtBx_qlMid_start_delete.Clear();
            txtBx_qlFin_start_delete.Clear();
            txtBx_qntFP_end_delete.Clear();
            txtBx_qntMid_end_delete.Clear();
            txtBx_qntFin_end_delete.Clear();
            txtBx_qlFP_end_delete.Clear();
            txtBx_qlMid_end_delete.Clear();
            txtBx_qlFin_end_delete.Clear();

            {
                txtBx_qntAdd_1_delete.Clear();
                txtBx_qntAdd_2_delete.Clear();
                txtBx_qntAdd_3_delete.Clear();
                txtBx_qntAdd_4_delete.Clear();
                txtBx_qntAdd_5_delete.Clear();
                txtBx_qntAdd_6_delete.Clear();
                txtBx_qntAdd_7_delete.Clear();
            }
        }
        #endregion

        #region Data Filtration
        private void ResetTable()
        {
            m_addingRows = true;

            DataContainer container = DataContainer.Instance;

            var columnSetDictionary = container.EvaluationFileCriterionColumnsRangeSetDictionary;

            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var columnSet in columnSetDictionary)
                {
                    string qntAdd_string = "";
                    var qntAdditionalColumns = columnSet.Value.QntAdditionalColumns;
                    if (qntAdditionalColumns != null)
                    {
                        for (int i = 0; i < qntAdditionalColumns.Count; i++)
                        {
                            qntAdd_string += ((i > 0) ? "," : "") + qntAdditionalColumns[i];
                        }
                    }

                    var row = new string[] { columnSet.Key.ToString(), columnSet.Value.Name, columnSet.Value.QntFirstPartialColumnRange.ToString(), columnSet.Value.QntMidtermColumnRange.ToString(), columnSet.Value.QntFinalsColumnRange.ToString(), qntAdd_string, columnSet.Value.QlFirstPartialColumnRange.ToString(), columnSet.Value.QlMidtermColumnRange.ToString(), columnSet.Value.QlFinalsColumnRange.ToString() };
                    rows.Add(row);

                    if (m_selectedColumnSetId != string.Empty && row[0] == m_selectedColumnSetId)
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
            else if (DataContainer.Instance.EvaluationFileCriterionColumnsRangeSetDictionary.Any(x => x.Value.Name == name))
                errorMessages.Add("A column set with the same name already exists!");

            string qntFP_start = txtBx_qntFP_start_add.Text;
            string qntFP_end = txtBx_qntFP_end_add.Text;
            string qntMid_start = txtBx_qntMid_start_add.Text;
            string qntMid_end = txtBx_qntMid_end_add.Text;
            string qntFin_start = txtBx_qntFin_start_add.Text;
            string qntFin_end = txtBx_qntFin_end_add.Text;
            string qlFP_start = txtBx_qlFP_start_add.Text;
            string qlFP_end = txtBx_qlFP_end_add.Text;
            string qlMid_start = txtBx_qlMid_start_add.Text;
            string qlMid_end = txtBx_qlMid_end_add.Text;
            string qlFin_start = txtBx_qlFin_start_add.Text;
            string qlFin_end = txtBx_qlFin_end_add.Text;

            if (string.IsNullOrWhiteSpace(qntMid_start)
                || string.IsNullOrWhiteSpace(qntMid_end)
                || string.IsNullOrWhiteSpace(qntFin_start)
                || string.IsNullOrWhiteSpace(qntFin_end)
                || string.IsNullOrWhiteSpace(qlMid_start)
                || string.IsNullOrWhiteSpace(qlMid_end)
                || string.IsNullOrWhiteSpace(qlFin_start)
                || string.IsNullOrWhiteSpace(qlFin_end))
            {
                errorMessages.Add("Enter all range values!");
            }
            else
            {
                Regex regex = new Regex("^[a-zA-Z]*$");

                if (!regex.IsMatch(qntMid_start)
                    || !regex.IsMatch(qntMid_end)
                    || !regex.IsMatch(qntFin_start)
                    || !regex.IsMatch(qntFin_end)
                    || !regex.IsMatch(qlMid_start)
                    || !regex.IsMatch(qlMid_end)
                    || !regex.IsMatch(qlFin_start)
                    || !regex.IsMatch(qlFin_end))
                {
                    errorMessages.Add("Only alphabet must be entered as range values!");
                }
            }

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

            string qntFP_start = txtBx_qntFP_start_edit.Text;
            string qntFP_end = txtBx_qntFP_end_edit.Text;
            string qntMid_start = txtBx_qntMid_start_edit.Text;
            string qntMid_end = txtBx_qntMid_end_edit.Text;
            string qntFin_start = txtBx_qntFin_start_edit.Text;
            string qntFin_end = txtBx_qntFin_end_edit.Text;
            string qlFP_start = txtBx_qlFP_start_edit.Text;
            string qlFP_end = txtBx_qlFP_end_edit.Text;
            string qlMid_start = txtBx_qlMid_start_edit.Text;
            string qlMid_end = txtBx_qlMid_end_edit.Text;
            string qlFin_start = txtBx_qlFin_start_edit.Text;
            string qlFin_end = txtBx_qlFin_end_edit.Text;

            if (string.IsNullOrWhiteSpace(qntMid_start)
                || string.IsNullOrWhiteSpace(qntMid_end)
                || string.IsNullOrWhiteSpace(qntFin_start)
                || string.IsNullOrWhiteSpace(qntFin_end)
                || string.IsNullOrWhiteSpace(qlMid_start)
                || string.IsNullOrWhiteSpace(qlMid_end)
                || string.IsNullOrWhiteSpace(qlFin_start)
                || string.IsNullOrWhiteSpace(qlFin_end))
            {
                errorMessages.Add("Enter all range values!");
            }
            else
            {
                Regex regex = new Regex("^[a-zA-Z]*$");

                if (!regex.IsMatch(qntMid_start)
                    || !regex.IsMatch(qntMid_end)
                    || !regex.IsMatch(qntFin_start)
                    || !regex.IsMatch(qntFin_end)
                    || !regex.IsMatch(qlMid_start)
                    || !regex.IsMatch(qlMid_end)
                    || !regex.IsMatch(qlFin_start)
                    || !regex.IsMatch(qlFin_end))
                {
                    errorMessages.Add("Only alphabet must be entered as range values!");
                }
            }

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
            string name = txtBx_name_edit.Text;
            string qntFP_start = txtBx_qntFP_start_edit.Text;
            string qntFP_end = txtBx_qntFP_end_edit.Text;
            string qntMid_start = txtBx_qntMid_start_edit.Text;
            string qntMid_end = txtBx_qntMid_end_edit.Text;
            string qntFin_start = txtBx_qntFin_start_edit.Text;
            string qntFin_end = txtBx_qntFin_end_edit.Text;
            List<string> qntAdd_columnLetters = new List<string>();
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_1_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_1_add.Text);
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_2_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_2_add.Text);
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_3_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_3_add.Text);
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_4_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_4_add.Text);
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_5_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_5_add.Text);
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_6_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_6_add.Text);
            if (!string.IsNullOrWhiteSpace(txtBx_qntAdd_7_add.Text)) qntAdd_columnLetters.Add(txtBx_qntAdd_7_add.Text);
            string qntAdd_string = "";
            for (int i = 0; i < qntAdd_columnLetters.Count; i++)
            {
                qntAdd_string += ((i > 0) ? "," : "") + qntAdd_columnLetters[i];
            }
            string qlFP_start = txtBx_qlFP_start_edit.Text;
            string qlFP_end = txtBx_qlFP_end_edit.Text;
            string qlMid_start = txtBx_qlMid_start_edit.Text;
            string qlMid_end = txtBx_qlMid_end_edit.Text;
            string qlFin_start = txtBx_qlFin_start_edit.Text;
            string qlFin_end = txtBx_qlFin_end_edit.Text;

            return name != (string)(SelectedCells["Name"].Value)
                || qntFP_start + " -> " + qntFP_end != (string)(SelectedCells["Rng1"].Value)
                || qntMid_start + " -> " + qntMid_end != (string)(SelectedCells["Rng2"].Value)
                || qntFin_start + " -> " + qntFin_end != (string)(SelectedCells["Rng3"].Value)
                || qntAdd_string != (string)(SelectedCells["Rng4"].Value)
                || qlFP_start + " -> " + qlFP_end != (string)(SelectedCells["Rng5"].Value)
                || qlMid_start + " -> " + qlMid_end != (string)(SelectedCells["Rng6"].Value)
                || qlFin_start + " -> " + qlFin_end != (string)(SelectedCells["Rng7"].Value);
        }
        #endregion

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchTo(new CourseAndStudentInformationMainForm());
        }
    }
}
