using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    public partial class ExamScheduleForm : Form
    {
        public ExamScheduleForm()
        {
            InitializeComponent();
        }

        private void ExamScheduleForm_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;

            cmbBx_examType.Items.Add("TOEFL");
            cmbBx_examType.SelectedIndex = 0;

            nmUpDwn_maxNumOfExaminees.Value = 20;

            nmUpDwn_year.Value = nmUpDwn_year_regInit.Value = nmUpDwn_year_regEnd.Value = DateTime.Now.Year;
            foreach (var month in DateTimeFormatInfo.CurrentInfo.MonthNames)
            {
                if (month != "")
                {
                    cmbBx_month.Items.Add(month.ToString());
                    cmbBx_month_regInit.Items.Add(month.ToString());
                    cmbBx_month_regEnd.Items.Add(month.ToString());
                }
            }
            cmbBx_month.SelectedIndex = 0;
            cmbBx_month_regInit.SelectedIndex = 0;
            cmbBx_month_regEnd.SelectedIndex = 0;

            InitializeMainViewTable();

            RefreshMainViewTable();
            dgv_main.ClearSelection();
        }

        private bool m_isInitializingDgv = false;

        #region Component Initialization
        private void InitializeMainViewTable()
        {
            // Set columns
            if (dgv_main.ColumnCount == 0)
            {
                var columns = dgv_main.Columns;
                columns.Add("Id", "Id");
                columns.Add("ExamType", "Exam Type");
                columns.Add("ExamDate", "Exam Date & Time");
                columns.Add("RegistrationDates", "Registration Dates");
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            m_isInitializingDgv = true;

            // Set rows
            DataContainer container = DataContainer.Instance;

            var examDictionary = container.NonInstitutionalExam_InternalDictionary;

            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var exam in examDictionary)
                {
                    var row = new string[] { exam.Key.ToString(), exam.Value.Type.ToString(), exam.Value.Date.ToString("dd/MM/yyyy"), exam.Value.RegistrationInitDate.ToString("dd/MM/yyyy HH:mm") + " to " + exam.Value.RegistrationEndDate.ToString("dd/MM/yyyy") };

                    rows.Add(row);
                }
            }

            if (selectingRowIndex != -1)
                rows[selectingRowIndex].Selected = true;
            else if (rows.Count != 0)
                rows[rows.GetLastRow(DataGridViewElementStates.None)].Selected = true;

            m_isInitializingDgv = false;
        }
        #endregion

        private void btn_addExam_Click(object sender, EventArgs e)
        {
            int year_regInit = Convert.ToInt32(nmUpDwn_year_regInit.Value);
            string yearString_regInit = nmUpDwn_year_regInit.Value.ToString();
            string monthString_regInit = cmbBx_month_regInit.Text;
            int month_regInit = DateTime.ParseExact(monthString_regInit, "MMMM", CultureInfo.CurrentCulture).Month; ;
            int day_regInit = Convert.ToInt32(nmUpDwn_day_regInit.Value);
            if (day_regInit > DateTime.DaysInMonth(year_regInit, month_regInit))
            {
                MessageBox.Show("The selected month does not contain such day!");
                return;
            }

            int year_regEnd = Convert.ToInt32(nmUpDwn_year_regEnd.Value);
            string yearString_regEnd = nmUpDwn_year_regEnd.Value.ToString();
            string monthString_regEnd = cmbBx_month_regEnd.Text;
            int month_regEnd = DateTime.ParseExact(monthString_regEnd, "MMMM", CultureInfo.CurrentCulture).Month;
            int day_regEnd = Convert.ToInt32(nmUpDwn_day_regEnd.Value);
            if (day_regEnd > DateTime.DaysInMonth(year_regEnd, month_regEnd))
            {
                MessageBox.Show("The selected month does not contain such day!");
                return;
            }

            eExamType examType = cmbBx_examType.Text.ToCorrespondingEnumValue<eExamType>();
            int maxNumOfExaminees = Convert.ToInt32(nmUpDwn_maxNumOfExaminees.Value);
            int year = Convert.ToInt32(nmUpDwn_year.Value);
            string yearString = nmUpDwn_year.Value.ToString();
            string monthString = cmbBx_month.Text;
            int month = DateTime.ParseExact(monthString, "MMMM", CultureInfo.CurrentCulture).Month;
            int day = Convert.ToInt32(nmUpDwn_day.Value);
            if (day > DateTime.DaysInMonth(year, month))
            {
                MessageBox.Show("The selected month does not contain such day!");
                return;
            }

            string dayString_regInit = nmUpDwn_day_regInit.Value.ToString("00");

            string dayString_regEnd = nmUpDwn_day_regEnd.Value.ToString("00");

            string dayString = nmUpDwn_day.Value.ToString("00");
            string timeString = nmUpDwn_hour.Value.ToString("00") + ":" + nmUpDwn_minute.Value.ToString("00");

            DateTime examDateRegistrationInitDate = DateTime.ParseExact(monthString_regInit + " " + dayString_regInit + ", " + yearString_regInit + " " + "00:00", "MMMM dd, yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime examDateRegistrationEndDate = DateTime.ParseExact(monthString_regEnd + " " + dayString_regEnd + ", " + yearString_regEnd + " " + "23:59", "MMMM dd, yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime examDate = DateTime.ParseExact(monthString + " " + dayString + ", " + yearString + " " + timeString, "MMMM dd, yyyy HH:mm", CultureInfo.InvariantCulture);

            if (examDateRegistrationEndDate >= examDate)
            {
                MessageBox.Show("End Date must be prior to the Exam Date");
                return;
            }

            if (examDateRegistrationInitDate >= examDateRegistrationEndDate)
            {
                MessageBox.Show("Init Date must be prior to End Date");
                return;
            }

            DataContainer container = DataContainer.Instance;
            if (!container.NonInstitutionalExam_InternalDictionary.Any(x => x.Value.Date == examDate && x.Value.Type == examType))
            {
                DocumentLoader.AddNonInstitutionalExam_InternalToCSV(examDateRegistrationInitDate, examDateRegistrationEndDate, examDate, maxNumOfExaminees, examType);
                container.SharePointConnectionManager.UploadFile(container.FilePath_NonInstitutionalExam_Internal);
                container.ResetAllFileModificationStatus();

                RefreshMainViewTable();
            }
            else
                MessageBox.Show("An exam with the same values are already registered!");
        }

        private void btn_deleteExam_Click(object sender, EventArgs e)
        {
            if (dgv_main.SelectedRows.Count == 0)
                return;

            DataContainer container = DataContainer.Instance;

            var cells = dgv_main.SelectedRows[0].Cells;
            int selectedExamId = Convert.ToInt32((string)(cells["Id"].Value));
            NonInstitutionalExam_Internal selectedExam = container.NonInstitutionalExam_InternalDictionary[selectedExamId];

            var registrationInfos = container.RegistrationInfos_Internal;
            if (registrationInfos.Any(x => x.Exam.Date == selectedExam.Date && x.Exam.Type == selectedExam.Type))
                return;

            if (DocumentLoader.DeleteNonInstitutionalExam_InternalFromCSV(selectedExam))
            {
                container.SharePointConnectionManager.UploadFile(container.FilePath_NonInstitutionalExam_Internal);
                container.ResetAllFileModificationStatus();

                RefreshMainViewTable();
            }
            else
                MessageBox.Show("Failed to delete exam! Please try again.");
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
