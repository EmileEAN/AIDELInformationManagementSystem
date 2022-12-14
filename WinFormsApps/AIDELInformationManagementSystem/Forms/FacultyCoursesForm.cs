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
    public partial class FacultyCoursesForm : Form
    {
        public FacultyCoursesForm()
        {
            InitializeComponent();
        }

        private void FacultyCoursesForm_Load(object sender, EventArgs e)
        {
            m_faculty = DataContainer.Instance.SelectedFaculty;
            lbl_faculty.Text = "(" + m_faculty.AccountNumber.ToString() + ") " + m_faculty.Name.ToString();

            InitializeMainViewTable();

            RefreshMainViewTable();
            dgv_main.ClearSelection();
            dgv_main.SelectionChanged += (_sender, _e) => dgv_main.ClearSelection();
        }

        private Faculty m_faculty;

        #region Component Events
        private void txtBx_filter_main_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Main();
        }

        private void dgv_main_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            var column = dgv.Columns[columnIndex];
            if (column is DataGridViewButtonColumn && rowIndex >= 0) // If it is a button column and it is not the header cell
            {
                if (column.DataPropertyName == "Group")
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

                Font font = new Font("Palatino Linotype", 12F, GraphicsUnit.Pixel);

                var groupColumn = new DataGridViewButtonColumn();
                groupColumn.DataPropertyName = groupColumn.Name = "Group";
                groupColumn.CellTemplate.Style.Font = new Font("Palatino Linotype", 12F, GraphicsUnit.Pixel);
                columns.Add(groupColumn);
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

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            string filterText = txtBx_filter_main.Text;

            DataContainer container = DataContainer.Instance;

            var courseGroupDictionary = container.CourseGroupDictionary.Where(x => x.Value.AssignedFaculty == m_faculty);
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
                    var row = new string[] { courseBase.Id.ToString(), courseBase.Name, semester.Term.ToString(), semester.Year.ToString(), group.Name };
                    rows.Add(row);
                }
            }

            if (selectingRowIndex != -1)
                rows[selectingRowIndex].Selected = true;
            else if (rows.Count != 0)
                rows[rows.GetLastRow(DataGridViewElementStates.None)].Selected = true;
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
