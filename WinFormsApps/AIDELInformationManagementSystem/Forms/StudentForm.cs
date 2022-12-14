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
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeMajorComboBox(cmbBx_major_add);
            InitializeMajorComboBox(cmbBx_major_edit);
            InitializeMajorComboBox(cmbBx_major_delete);

            InitializeSemesterAdmittedComboBox(cmbBx_semesterAdmitted_add);
            InitializeSemesterAdmittedComboBox(cmbBx_semesterAdmitted_edit);
            InitializeSemesterAdmittedComboBox(cmbBx_semesterAdmitted_delete);

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
        private string m_selectedStudentAccountNumber = string.Empty;
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
        private void btn_comparison_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new StudentsComparisonForm());
        }
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
                m_selectedStudentAccountNumber = default;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedStudentAccountNumber = (string)(SelectedCells["AccountNumber"].Value);
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

        private void dgv_main_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            var column = dgv.Columns[columnIndex];
            if (column is DataGridViewButtonColumn && rowIndex >= 0) // If it is a button column and it is not the header cell
            {
                if (column.DataPropertyName == "Courses")
                {
                    var cells = dgv.Rows[rowIndex].Cells;

                    int accountNumber = Convert.ToInt32((string)(cells["AccountNumber"].Value));

                    DataContainer container = DataContainer.Instance;
                    container.SelectedStudent = container.StudentList.First(x => x.AccountNumber == accountNumber);
                    this.LogAndSwitchTo(new StudentCoursesForm());
                }
            }
        }
        #endregion

        #region Add Tab
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                if (DocumentLoader.AddStudentToCSV(txtBx_accountNumber_add.Text, txtBx_firstName_add.Text, txtBx_middleName_add.Text, txtBx_paternalSurname_add.Text, txtBx_maternalSurname_add.Text))
                {
                    MessageBox.Show("Added new student.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new student! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                DataContainer container = DataContainer.Instance;

                int selectedStudentAccountNumber = Convert.ToInt32((string)(SelectedCells[0].Value));
                Student selectedStudent = container.StudentList.First(x => x.AccountNumber == selectedStudentAccountNumber);
                
                Major major = container.MajorList.First(x => x.ToString() == cmbBx_major_edit.SelectedItem.ToString());
                Semester semesterAdmitted = container.SemesterDictionary.First(x => x.Value.ToString() == cmbBx_semesterAdmitted_edit.SelectedItem.ToString()).Value;

                if (DocumentLoader.EditStudentInCSV(selectedStudent, txtBx_accountNumber_edit.Text, txtBx_firstName_edit.Text, txtBx_middleName_edit.Text, txtBx_paternalSurname_edit.Text, txtBx_maternalSurname_edit.Text,
                                                    txtBx_organizationEmail_edit.Text, txtBx_preferredEmail_edit.Text, txtBx_phone_edit.Text, major, semesterAdmitted))
                {
                    MessageBox.Show("Edited student successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit student! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedStudentAccountNumber = Convert.ToInt32((string)(SelectedCells[0].Value));
                Student selectedStudent = DataContainer.Instance.StudentList.First(x => x.AccountNumber == selectedStudentAccountNumber);
                if (DocumentLoader.DeleteStudentFromCSV(selectedStudent))
                {
                    MessageBox.Show("Deleted student successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete student! Please try again.");
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
                columns.Add("AccountNumber", "Account Number");
                columns.Add("FirstName", "First Name");
                columns.Add("MiddleName", "Middle Name");
                columns.Add("PaternalSurname", "Paternal Surname");
                columns.Add("MaternalSurname", "Maternal Surname");

                var coursesColumn = new DataGridViewButtonColumn();
                coursesColumn.DataPropertyName = coursesColumn.Name = "Courses";
                coursesColumn.CellTemplate.Style.Font = new Font("Palatino Linotype", 12F, GraphicsUnit.Pixel);
                columns.Add(coursesColumn);

                columns.Add("OrganizationEmail", "Organization Email");
                columns.Add("PreferredEmail", "Preferred Email");
                columns.Add("Phone", "Phone");
                columns.Add("Major", "Major");
                columns.Add("SemesterAdmitted", "Semester Admitted");
            }
        }

        private void InitializeMajorComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var major in DataContainer.Instance.MajorList)
            {
                items.Add(major.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeSemesterAdmittedComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var semester in DataContainer.Instance.SemesterDictionary.Values)
            {
                items.Add(semester.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_accountNumber_edit.Text = Convert.ToInt32(SelectedCells["AccountNumber"].Value).ToString(CoreValues.AccountNumberStringFormat);
                txtBx_firstName_edit.Text = (string)(SelectedCells["FirstName"].Value);
                txtBx_middleName_edit.Text = (string)(SelectedCells["MiddleName"].Value);
                txtBx_paternalSurname_edit.Text = (string)(SelectedCells["PaternalSurname"].Value);
                txtBx_maternalSurname_edit.Text = (string)(SelectedCells["MaternalSurname"].Value);
                txtBx_organizationEmail_edit.Text = (string)(SelectedCells["OrganizationEmail"].Value);
                txtBx_preferredEmail_edit.Text = (string)(SelectedCells["PreferredEmail"].Value);
                txtBx_phone_edit.Text = (string)(SelectedCells["Phone"].Value);
                cmbBx_major_edit.SelectedIndex = cmbBx_major_edit.FindStringExact((string)(SelectedCells["Major"].Value));
                cmbBx_semesterAdmitted_edit.SelectedIndex = cmbBx_semesterAdmitted_edit.FindStringExact((string)(SelectedCells["SemesterAdmitted"].Value));
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                txtBx_accountNumber_delete.Text = Convert.ToInt32(SelectedCells["AccountNumber"].Value).ToString(CoreValues.AccountNumberStringFormat);
                txtBx_firstName_delete.Text = (string)(SelectedCells["FirstName"].Value);
                txtBx_middleName_delete.Text = (string)(SelectedCells["MiddleName"].Value);
                txtBx_paternalSurname_delete.Text = (string)(SelectedCells["PaternalSurname"].Value);
                txtBx_maternalSurname_delete.Text = (string)(SelectedCells["MaternalSurname"].Value);
                txtBx_organizationEmail_delete.Text = (string)(SelectedCells["OrganizationEmail"].Value);
                txtBx_preferredEmail_delete.Text = (string)(SelectedCells["PreferredEmail"].Value);
                txtBx_phone_delete.Text = (string)(SelectedCells["Phone"].Value);
                cmbBx_major_delete.SelectedIndex = cmbBx_major_delete.FindStringExact((string)(SelectedCells["Major"].Value));
                cmbBx_semesterAdmitted_delete.SelectedIndex = cmbBx_semesterAdmitted_delete.FindStringExact((string)(SelectedCells["SemesterAdmitted"].Value));
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
            txtBx_accountNumber_add.Clear();
            txtBx_firstName_add.Clear();
            txtBx_middleName_add.Clear();
            txtBx_paternalSurname_add.Clear();
            txtBx_maternalSurname_add.Clear();
            txtBx_organizationEmail_add.Clear();
            txtBx_preferredEmail_add.Clear();
            txtBx_phone_add.Clear();
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter.Text;

            DataContainer container = DataContainer.Instance;

            var studentList = container.StudentList;
            Faculty currentUser = container.CurrentUser;
            if (currentUser.PermissionsType == ePermissionsType.AssignedCoursesOnly)
            {
                var courseGroupList = container.CourseGroupDictionary;
                var facultyCourses = courseGroupList.Values.Where(x => x.AssignedFaculty == currentUser);
                studentList.Clear();
                foreach (var group in facultyCourses)
                {
                    foreach (var student in group.StudentInfos.Select(x => x.Student))
                    {
                        if (!studentList.Contains(student))
                            studentList.Add(student);
                    }    
                }
            }
            var targetList = string.IsNullOrWhiteSpace(filterText) ? studentList : studentList.Where(x => x.AccountNumber.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                                                                                || x.Name.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                                                                                || x.OrganizationEmail.Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                                                                                || x.PreferredEmail.Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                                                                                || x.Phone.Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                                                                                || x.Major.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                                                                                || x.SemesterAdmitted.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var student in targetList)
                {
                    var row = new string[] { student.AccountNumber.ToString(CoreValues.AccountNumberStringFormat), student.Name.FirstName, student.Name.MiddleName, student.Name.PaternalSurname, student.Name.MaternalSurname,
                                                "Courses", student.OrganizationEmail ?? "", student.PreferredEmail ?? "", student.Phone ?? "", (student.Major != null) ? student.Major.ToString() : "", (student.SemesterAdmitted != null) ? student.SemesterAdmitted.ToString() : "" };
                    rows.Add(row);

                    if (m_selectedStudentAccountNumber != string.Empty && row[0] == m_selectedStudentAccountNumber)
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

            int accountNumberLength = CoreValues.AccountNumberLength;
            string accountNumber = txtBx_accountNumber_add.Text;
            if (string.IsNullOrWhiteSpace(accountNumber))
                errorMessages.Add("Enter an account number!");
            else if (accountNumber.Length > accountNumberLength)
                errorMessages.Add("Account number must be up to " + accountNumberLength.ToString() + " digits!");
            else if (DataContainer.Instance.StudentList.Any(x => x.AccountNumber == Convert.ToInt32(accountNumber)))
                errorMessages.Add("A student with the same account number already exists!");

            string firstName = txtBx_firstName_add.Text;
            if (string.IsNullOrWhiteSpace(firstName))
                errorMessages.Add("Enter a first name!");

            string paternalSurname = txtBx_paternalSurname_add.Text;
            if (string.IsNullOrWhiteSpace(paternalSurname))
                errorMessages.Add("Enter a paternal surname!");

            //string organizationEmail = txtBx_organizationEmail_add.Text;
            //if (string.IsNullOrWhiteSpace(organizationEmail))
            //    errorMessages.Add("Enter an organization email!");

            int phoneNumberLength = CoreValues.PhoneNumberLength;
            string phone = txtBx_phone_add.Text;
            if (!string.IsNullOrWhiteSpace(phone) && phone.Length != phoneNumberLength)
                errorMessages.Add("Phone number must be " + phoneNumberLength.ToString() + " digits!");

            string defaultString = CoreValues.ComboBox_DefaultString;

            //if (cmbBx_major_add.SelectedItem.ToString() == defaultString)
            //    errorMessages.Add("Select a major!");

            //if (cmbBx_semesterAdmitted_add.SelectedItem.ToString() == defaultString)
            //    errorMessages.Add("Select a semester!");

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

            string accountNumber = txtBx_accountNumber_edit.Text;
            if (accountNumber != (string)(SelectedCells[0].Value)) // If account number is to be changed
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                    errorMessages.Add("Enter an account number!");
                else if (accountNumber.Length > CoreValues.AccountNumberLength)
                    errorMessages.Add("Account number must be up to " + CoreValues.AccountNumberLength.ToString() + " digits!");
                else if (DataContainer.Instance.StudentList.Any(x => x.AccountNumber == Convert.ToInt32(accountNumber)))
                    errorMessages.Add("A student with the same account number already exists!");
            }

            string firstName = txtBx_firstName_edit.Text;
            if (string.IsNullOrWhiteSpace(firstName))
                errorMessages.Add("Enter a first name!");

            string paternalSurname = txtBx_paternalSurname_edit.Text;
            if (string.IsNullOrWhiteSpace(paternalSurname))
                errorMessages.Add("Enter a paternal surname!");

            string organizationEmail = txtBx_organizationEmail_edit.Text;
            if (string.IsNullOrWhiteSpace(organizationEmail))
                errorMessages.Add("Enter an organization email!");

            int phoneNumberLength = CoreValues.PhoneNumberLength;
            string phone = txtBx_phone_edit.Text;
            if (!string.IsNullOrWhiteSpace(phone) && phone.Length != phoneNumberLength)
                errorMessages.Add("Phone number must be " + phoneNumberLength.ToString() + " digits!");

            string defaultString = CoreValues.ComboBox_DefaultString;

            if (cmbBx_major_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a major!");

            if (cmbBx_semesterAdmitted_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a semester!");

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
            return txtBx_accountNumber_edit.Text != (string)(SelectedCells["AccountNumber"].Value)
                || txtBx_firstName_edit.Text != (string)(SelectedCells["FirstName"].Value)
                || txtBx_middleName_edit.Text != (string)(SelectedCells["MiddleName"].Value)
                || txtBx_paternalSurname_edit.Text != (string)(SelectedCells["PaternalSurname"].Value)
                || txtBx_maternalSurname_edit.Text != (string)(SelectedCells["MaternalSurname"].Value)
                || txtBx_organizationEmail_edit.Text != (string)(SelectedCells["OrganizationEmail"].Value)
                || txtBx_preferredEmail_edit.Text != (string)(SelectedCells["PreferredEmail"].Value)
                || txtBx_phone_edit.Text != (string)(SelectedCells["Phone"].Value)
                || cmbBx_major_edit.SelectedItem.ToString() != (string)(SelectedCells["Major"].Value)
                || cmbBx_semesterAdmitted_edit.SelectedItem.ToString() != (string)(SelectedCells["SemesterAdmitted"].Value);
        }
        #endregion

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
