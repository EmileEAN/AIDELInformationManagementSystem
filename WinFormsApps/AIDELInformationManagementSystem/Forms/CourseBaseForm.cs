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
    public partial class CourseBaseForm : Form
    {
        public CourseBaseForm()
        {
            InitializeComponent();
        }

        private void CourseBaseForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeCourseCategoryComboBox(cmbBox_category_add);
            InitializeCourseCategoryComboBox(cmbBox_category_edit);
            InitializeCourseCategoryComboBox(cmbBox_category_delete);

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
        private string m_selectedCourseBaseId = string.Empty;
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
        private void txtBx_filter_TextChanged(object sender, EventArgs e)
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
                m_selectedCourseBaseId = default;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedCourseBaseId = (string)(SelectedCells["Id"].Value);
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
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                if (DocumentLoader.AddCourseBaseToCSV(Convert.ToInt32(txtBx_id_add.Text), txtBx_name_add.Text, cmbBox_category_add.SelectedItem.ToString().ToCorrespondingEnumValue<eCourseCategory>()))
                {
                    MessageBox.Show("Added new course base.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new course base! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                int selectedCourseBaseId = Convert.ToInt32((string)(SelectedCells[0].Value));
                CourseBase selectedCourseBase = DataContainer.Instance.CourseBaseList.First(x => x.Id == selectedCourseBaseId);
                if (DocumentLoader.EditCourseBaseInCSV(selectedCourseBase, Convert.ToInt32(txtBx_id_edit.Text), txtBx_name_edit.Text, cmbBox_category_edit.SelectedItem.ToString().ToCorrespondingEnumValue<eCourseCategory>()))
                {
                    MessageBox.Show("Edited course base successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit course base! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCourseBaseId = Convert.ToInt32((string)(SelectedCells[0].Value));
                CourseBase selectedCourseBase = DataContainer.Instance.CourseBaseList.First(x => x.Id == selectedCourseBaseId);
                if (DocumentLoader.DeleteCourseBaseFromCSV(selectedCourseBase))
                {
                    MessageBox.Show("Deleted course base successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete course base! Please try again.");
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
                columns.Add("Id", "Id");
                columns.Add("Name", "Name");
                columns.Add("Category", "Category");
            }
        }

        private void InitializeCourseCategoryComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var category in DataContainer.Instance.CourseCategoryDictionary)
            {
                items.Add(category.Value.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_id_edit.Text = Convert.ToInt32(SelectedCells["Id"].Value).ToString();
                txtBx_name_edit.Text = (string)(SelectedCells["Name"].Value);
                cmbBox_category_edit.SelectedIndex = cmbBox_category_edit.FindStringExact((string)(SelectedCells["Category"].Value));
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_id_delete.Text = Convert.ToInt32(SelectedCells["Id"].Value).ToString();
                txtBx_name_delete.Text = (string)(SelectedCells["Name"].Value);
                cmbBox_category_delete.SelectedIndex = cmbBox_category_delete.FindStringExact((string)(SelectedCells["Category"].Value));
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
            string defaultString = CoreValues.ComboBox_DefaultString;
            txtBx_id_add.Clear();
            txtBx_name_add.Clear();
            cmbBox_category_add.SelectedIndex = cmbBox_category_add.FindStringExact(defaultString);
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter.Text;

            var courseBaseList = DataContainer.Instance.CourseBaseList;
            var targetList = string.IsNullOrWhiteSpace(filterText) ? courseBaseList : courseBaseList.Where(x => x.Id.ToString().Contains(filterText)
                                                                                                || x.Name.ToString().Contains(filterText)
                                                                                                || x.Category.ToString().Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var courseBase in targetList)
                {
                    var row = new string[] { courseBase.Id.ToString(), courseBase.Name, courseBase.Category.ToString() };
                    rows.Add(row);

                    if (m_selectedCourseBaseId != string.Empty && row[0] == m_selectedCourseBaseId)
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

            string defaultString = CoreValues.ComboBox_DefaultString;

            string id = txtBx_id_add.Text;
            if (string.IsNullOrWhiteSpace(id))
                errorMessages.Add("Enter an Id!");
            else if (id.Length < CoreValues.MinCourseIdLength || id.Length > CoreValues.MaxCourseIdLength)
                errorMessages.Add("Id must be " + CoreValues.MinCourseIdLength.ToString() + " digits!");
            else if (DataContainer.Instance.CourseBaseList.Any(x => x.Id == Convert.ToInt32(id)))
                errorMessages.Add("A course base with the same Id already exists!");

            string name = txtBx_name_add.Text;
            if (string.IsNullOrWhiteSpace(name))
                errorMessages.Add("Enter a name!");

            if (cmbBox_category_add.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a category!");

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

            string defaultString = CoreValues.ComboBox_DefaultString;

            string id = txtBx_id_edit.Text;
            if (id != (string)(SelectedCells[0].Value)) // If Id is to be changed
            {
                if (string.IsNullOrWhiteSpace(id))
                    errorMessages.Add("Enter an Id!");
                else if (id.Length < CoreValues.MinCourseIdLength || id.Length > CoreValues.MaxCourseIdLength)
                    errorMessages.Add("Id must be " + CoreValues.MinCourseIdLength.ToString() + " digits!");
                else if (DataContainer.Instance.CourseBaseList.Any(x => x.Id == Convert.ToInt32(id)))
                    errorMessages.Add("A course base with the same Id already exists!");
            }

            string name = txtBx_name_edit.Text;
            if (string.IsNullOrWhiteSpace(name))
                errorMessages.Add("Enter a name!");

            if (cmbBox_category_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a category!");

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
            return txtBx_id_edit.Text != (string)(SelectedCells["Id"].Value)
                || txtBx_name_edit.Text != (string)(SelectedCells["Name"].Value)
                || cmbBox_category_edit.SelectedItem.ToString() != (string)(SelectedCells["Category"].Value);
        }
        #endregion

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
