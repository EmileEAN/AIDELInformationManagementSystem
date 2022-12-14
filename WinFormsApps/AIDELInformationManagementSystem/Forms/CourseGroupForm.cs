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
    public partial class CourseGroupForm : Form
    {
        public CourseGroupForm()
        {
            InitializeComponent();
        }

        private void CourseGroupForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeCourseComboBox(cmbBx_course_add);
            InitializeCourseComboBox(cmbBx_course_edit);
            InitializeCourseComboBox(cmbBx_course_delete);

            InitializeFacultyComboBox(cmbBx_faculty_add);
            InitializeFacultyComboBox(cmbBx_faculty_edit);
            InitializeFacultyComboBox(cmbBx_faculty_delete);

            InitializeStudentsTable(dgv_availableStudents_add);
            InitializeStudentsTable(dgv_availableStudents_edit);

            InitializeStudentsTable(dgv_groupStudents_add);
            InitializeStudentsTable(dgv_groupStudents_edit);
            InitializeStudentsTable(dgv_groupStudents_delete);

            RefreshMainViewTable();
            dgv_main.ClearSelection();

            List<Student> studentList = DataContainer.Instance.StudentList;
            RefreshAvailableStudentsTable(dgv_availableStudents_add, studentList);
            RefreshAvailableStudentsTable(dgv_availableStudents_edit, studentList);

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
        private CourseGroup m_selectedGroup;
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
                m_selectedGroup = null;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];

                var id = (string)(SelectedCells["Id"].Value);
                var name = (string)(SelectedCells["Name"].Value);
                var term = (string)(SelectedCells["Term"].Value);
                var year = (string)(SelectedCells["Year"].Value);
                var groupName = (string)(SelectedCells["Group"].Value);

                m_selectedGroup = DataContainer.Instance.CourseGroupDictionary.First(x => x.Value.Course.Base.Id.ToString() == id
                                                                                        && x.Value.Course.Base.Name == name
                                                                                        && x.Value.Course.Semester.Term.ToString() == term
                                                                                        && x.Value.Course.Semester.Year.ToString() == year
                                                                                        && x.Value.Name == groupName).Value;
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
                if (column.DataPropertyName == "Students")
                {
                    var cells = dgv.Rows[rowIndex].Cells;

                    int id = Convert.ToInt32((string)(cells["Id"].Value));
                    eTerm term = ((string)(cells["Term"].Value)).ToCorrespondingEnumValue<eTerm>();
                    int year = Convert.ToInt32((string)(cells["Year"].Value));
                    string groupName = (string)(cells["Group"].Value);

                    DataContainer container = DataContainer.Instance;
                    container.SelectedGroup = container.CourseGroupDictionary.First(x => x.Value.Course.Base.Id == id
                                                                                        && x.Value.Course.Semester.Term == term
                                                                                        && x.Value.Course.Semester.Year == year
                                                                                        && x.Value.Name == groupName).Value;
                    this.LogAndSwitchTo(new CourseStudentsForm());
                }
            }
        }
        #endregion

        #region Add Tab
        private void txtBx_filterStudents_add_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Add();
        }

        private void btn_addStudent_add_Click(object sender, EventArgs e)
        {
            if (dgv_availableStudents_add.SelectedRows.Count == 0)
                return;

            dgv_availableStudents_add.SelectedRows[0].MoveTo(dgv_groupStudents_add);
        }

        private void btn_removeStudent_add_Click(object sender, EventArgs e)
        {
            if (dgv_groupStudents_add.SelectedRows.Count == 0)
                return;

            dgv_groupStudents_add.SelectedRows[0].MoveTo(dgv_availableStudents_add);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                DataContainer container = DataContainer.Instance;
                var courseKeyValuePair = container.CourseDictionary.First(x => x.Value.Base.ToString() + " " + x.Value.Semester.ToString() == cmbBx_course_add.SelectedItem.ToString());
                int courseId = courseKeyValuePair.Key;
                Course course = courseKeyValuePair.Value;
                var groups = container.CourseGroupDictionary.Where(x => x.Value.Course == course);
                int numOfGroups = groups.Count();
                eGroupNamingFormat groupNamingFormat = (numOfGroups > 0 && Regex.IsMatch(groups.First().Value.Name, "[a-z]", RegexOptions.IgnoreCase)) ? eGroupNamingFormat.Alphabet : eGroupNamingFormat.Number;
                string groupName = string.Empty;
                switch (groupNamingFormat)
                {
                    case eGroupNamingFormat.Alphabet:
                        groupName = ((char)((numOfGroups + 1) + (65 - 1))).ToString();
                        break;

                    case eGroupNamingFormat.Number:
                        groupName = (numOfGroups + 1).ToString();
                        break;
                }
                var facultyName = cmbBx_faculty_add.SelectedItem.ToString();
                Faculty faculty = (facultyName != CoreValues.ComboBox_DefaultString) ? container.FacultyList.First(x => x.Name.ToString() == facultyName) : null;

                if (DocumentLoader.AddCourseGroupToCSV(courseId, groupName, TableToList(dgv_groupStudents_add), faculty) != default)
                {
                    MessageBox.Show("Added new group.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to add new group! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void txtBx_filterStudents_edit_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Edit();
        }

        private void btn_addStudent_edit_Click(object sender, EventArgs e)
        {
            if (dgv_availableStudents_edit.SelectedRows.Count == 0)
                return;

            dgv_availableStudents_edit.SelectedRows[0].MoveTo(dgv_groupStudents_edit);
        }

        private void btn_removeStudent_edit_Click(object sender, EventArgs e)
        {
            if (dgv_groupStudents_edit.SelectedRows.Count == 0)
                return;

            dgv_groupStudents_edit.SelectedRows[0].MoveTo(dgv_availableStudents_edit);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                DataContainer container = DataContainer.Instance;
                var courseKeyValuePair = container.CourseDictionary.First(x => x.Value.Base.ToString() + " " + x.Value.Semester.ToString() == cmbBx_course_edit.SelectedItem.ToString());
                int courseId = courseKeyValuePair.Key;
                Course course = courseKeyValuePair.Value;
                var groups = container.CourseGroupDictionary.Where(x => x.Value.Course == course);
                var facultyName = cmbBx_faculty_edit.SelectedItem.ToString();
                Faculty faculty = (facultyName != CoreValues.ComboBox_DefaultString) ? container.FacultyList.First(x => x.Name.ToString() == facultyName) : null;

                if (DocumentLoader.EditCourseGroupInCSV(m_selectedGroup, TableToList(dgv_groupStudents_edit), faculty))
                {
                    MessageBox.Show("Edited group successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit group! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCourseGroupId = DataContainer.Instance.CourseGroupDictionary.GetFirstKey(m_selectedGroup);
                if (DocumentLoader.DeleteCourseGroupFromCSV(selectedCourseGroupId))
                {
                    MessageBox.Show("Deleted group successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete group! Please try again.");
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
                columns.Add("Term", "Term");
                columns.Add("Year", "Year");
                columns.Add("Group", "Group");

                var studentsColumn = new DataGridViewButtonColumn();
                studentsColumn.DataPropertyName = studentsColumn.Name = "Students";
                studentsColumn.CellTemplate.Style.Font = new Font("Palatino Linotype", 12F, GraphicsUnit.Pixel);
                columns.Add(studentsColumn);

                columns.Add("Faculty", "Faculty");
            }
        }

        private void InitializeCourseComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var course in DataContainer.Instance.CourseDictionary.Values)
            {
                items.Add(course.Base.ToString() + " " + course.Semester.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeFacultyComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var faculty in DataContainer.Instance.FacultyList)
            {
                items.Add(faculty.Name.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeStudentsTable(DataGridView _dgv)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;
                columns.Add("AccountNumber", "AccountNumber");
                columns.Add("FirstName", "First Name");
                columns.Add("MiddleName", "Middle Name");
                columns.Add("Paternal Surname", "Paternal Surname");
                columns.Add("Maternal Surname", "Maternal Surname");
            }
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            DataContainer container = DataContainer.Instance;
            List<Student> fullStudentList = container.StudentList;

            txtBx_filterStudents_edit.Clear();

            if (SelectedCells != null)
            {
                List<Student> studentListInGroup = m_selectedGroup.StudentInfos.Select(x => x.Student).ToList();
                List<Student> studentListNotInGroup = fullStudentList.Except(studentListInGroup).ToList();

                var course = m_selectedGroup.Course;
                cmbBx_course_edit.SelectedIndex = cmbBx_course_edit.FindStringExact(course.Base.ToString() + " " + course.Semester.ToString());
                cmbBx_faculty_edit.SelectedIndex = cmbBx_faculty_edit.FindStringExact((string)(SelectedCells["Faculty"].Value));
                lbl_groupName_edit.Text = m_selectedGroup.Name;
                RefreshAvailableStudentsTable(dgv_availableStudents_edit, studentListNotInGroup);
                RefreshGroupStudentsTable(dgv_groupStudents_edit, studentListInGroup);
            }
            else
            {
                var defaultString = CoreValues.ComboBox_DefaultString;
                cmbBx_course_edit.SelectedIndex = cmbBx_course_edit.FindStringExact(defaultString);
                cmbBx_faculty_edit.SelectedIndex = cmbBx_faculty_edit.FindStringExact(defaultString);
                lbl_groupName_edit.Text = "";
                RefreshAvailableStudentsTable(dgv_availableStudents_edit, fullStudentList);
                RefreshGroupStudentsTable(dgv_groupStudents_edit);
            }
        }

        private void FillDeleteTabComponents()
        {
            DataContainer container = DataContainer.Instance;
            List<Student> fullStudentList = container.StudentList;

            if (SelectedCells != null)
            {
                List<Student> studentListInGroup = m_selectedGroup.StudentInfos.Select(x => x.Student).ToList();

                var course = m_selectedGroup.Course;
                cmbBx_course_delete.SelectedIndex = cmbBx_course_delete.FindStringExact(course.Base.ToString() + " " + course.Semester.ToString());
                cmbBx_faculty_delete.SelectedIndex = cmbBx_faculty_delete.FindStringExact((string)(SelectedCells["Faculty"].Value));
                lbl_groupName_delete.Text = m_selectedGroup.Name;
                RefreshGroupStudentsTable(dgv_groupStudents_delete, studentListInGroup);
            }
            else
            {
                var defaultString = CoreValues.ComboBox_DefaultString;
                cmbBx_course_delete.SelectedIndex = cmbBx_course_delete.FindStringExact(defaultString);
                cmbBx_faculty_delete.SelectedIndex = cmbBx_faculty_delete.FindStringExact(defaultString);
                lbl_groupName_delete.Text = "";
                RefreshGroupStudentsTable(dgv_groupStudents_delete);
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }

        private void RefreshAvailableStudentsTable(DataGridView _dgv, List<Student> _studentList = null)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_studentList == null)
                return;

            foreach (var student in _studentList)
            {
                var name = student.Name;
                rows.Add(new string[] { student.AccountNumber.ToString(), name.FirstName, name.MiddleName, name.PaternalSurname, name.MaternalSurname });
            }
        }

        private void RefreshGroupStudentsTable(DataGridView _dgv, List<Student> _studentList = null)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_studentList == null)
                return;

            foreach (var student in _studentList)
            {
                var name = student.Name;
                rows.Add(new string[] { student.AccountNumber.ToString(), name.FirstName, name.MiddleName, name.PaternalSurname, name.MaternalSurname });
            }
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            var defaultString = CoreValues.ComboBox_DefaultString;
            cmbBx_course_add.SelectedIndex = cmbBx_course_add.FindStringExact(defaultString);
            cmbBx_faculty_add.SelectedIndex = cmbBx_faculty_add.FindStringExact(defaultString);
            dgv_groupStudents_add.Rows.Clear();
            RefreshAvailableStudentsTable(dgv_availableStudents_add, DataContainer.Instance.StudentList.ToList());
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            DataContainer container = DataContainer.Instance;

            var courseGroupDictionary = container.CourseGroupDictionary;
            Faculty currentUser = container.CurrentUser;
            if (currentUser.PermissionsType == ePermissionsType.AssignedCoursesOnly)
                courseGroupDictionary = courseGroupDictionary.Where(x => x.Value.AssignedFaculty == currentUser).ToDictionary(x => x.Key, x => x.Value);
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? courseGroupDictionary : courseGroupDictionary.Where(x => x.Value.Course.Base.ToString().Contains(filterText)
                                                                                                                                || x.Value.Course.Semester.ToString().Contains(filterText)
                                                                                                                                || x.Value.Name.Contains(filterText)
                                                                                                                                || x.Value.StudentInfos.Any(y => y.Student.Name.ToString().Contains(filterText))
                                                                                                                                || x.Value.AssignedFaculty.Name.ToString().Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    var group = element.Value;
                    var course = group.Course;
                    var semester = course.Semester;
                    var courseBase = course.Base;
                    var faculty = group.AssignedFaculty;
                    var row = new string[] { courseBase.Id.ToString(), courseBase.Name, semester.Term.ToString(), semester.Year.ToString(), group.Name, "Students", (faculty != null) ? faculty.Name.ToString() : "" };
                    rows.Add(row);

                    var groupId = courseGroupDictionary.GetFirstKey(group);
                    var selectedGroupId = courseGroupDictionary.GetFirstKeyOrDefault(m_selectedGroup);

                    if (m_selectedGroup != null && groupId == selectedGroupId)
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
            string filterText = txtBx_filterStudents_add.Text;

            var studentListInGroup = TableToList(dgv_groupStudents_add);

            var fullStudentList = DataContainer.Instance.StudentList;
            var studentListNotInGroup = fullStudentList.Except(studentListInGroup);
            var targetList = string.IsNullOrWhiteSpace(filterText) ? studentListNotInGroup : studentListNotInGroup.Where(x => x.AccountNumber.ToString().Contains(filterText) || x.Name.ToString().Contains(filterText));
            var rows = dgv_availableStudents_add.Rows;
            {
                rows.Clear();
                foreach (var student in targetList)
                {
                    var name = student.Name;
                    var row = new string[] { student.AccountNumber.ToString(), name.FirstName, name.MiddleName, name.PaternalSurname, name.MaternalSurname };
                    rows.Add(row);
                }
            }
        }

        private void ApplyFilter_Edit()
        {
            string filterText = txtBx_filterStudents_edit.Text;

            var studentListInGroup = TableToList(dgv_groupStudents_edit);

            var fullStudentList = DataContainer.Instance.StudentList;
            var studentListNotInGroup = fullStudentList.Except(studentListInGroup);
            var targetList = string.IsNullOrWhiteSpace(filterText) ? studentListNotInGroup : studentListNotInGroup.Where(x => x.AccountNumber.ToString().Contains(filterText) || x.Name.ToString().Contains(filterText));
            var rows = dgv_availableStudents_edit.Rows;
            {
                rows.Clear();
                foreach (var student in targetList)
                {
                    var name = student.Name;
                    var row = new string[] { student.AccountNumber.ToString(), name.FirstName, name.MiddleName, name.PaternalSurname, name.MaternalSurname };
                    rows.Add(row);
                }
            }
        }
        #endregion

        #region Data Validation
        private bool ValidateInput_Add()
        {
            List<string> errorMessages = new List<string>();

            if (cmbBx_course_add.SelectedItem.ToString() == CoreValues.ComboBox_DefaultString)
                errorMessages.Add("Select a course!");

            if (cmbBx_faculty_add.SelectedItem.ToString() == CoreValues.ComboBox_DefaultString)
                errorMessages.Add("Select a faculty!");

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

            if (cmbBx_course_edit.SelectedItem.ToString() == CoreValues.ComboBox_DefaultString)
                errorMessages.Add("Select a course!");

            if (cmbBx_faculty_edit.SelectedItem.ToString() == CoreValues.ComboBox_DefaultString)
                errorMessages.Add("Select a faculty!");

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
            var originalStudentsList = m_selectedGroup.StudentInfos.Select(x => x.Student);
            var groupStudents = TableToList(dgv_groupStudents_edit);

            return cmbBx_faculty_edit.SelectedItem.ToString() != (string)(SelectedCells["Faculty"].Value)
                                                                || originalStudentsList.Count() != groupStudents.Count
                                                                || originalStudentsList.All(groupStudents.Contains);
        }
        #endregion

        #region Shared
        private List<Student> TableToList(DataGridView _dgv)
        {
            List<Student> result = new List<Student>();

            try
            {
                var studentList = DataContainer.Instance.StudentList;
                foreach (DataGridViewRow row in _dgv.Rows)
                {
                    var cells = row.Cells;
                    int accountNumber = Convert.ToInt32((string)(cells["AccountNumber"].Value));

                    Student student = studentList.First(x => x.AccountNumber == accountNumber);

                    result.Add(student);
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
