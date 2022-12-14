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
    public partial class CourseAndStudentInformationMainForm : Form
    {
        public CourseAndStudentInformationMainForm()
        {
            InitializeComponent();
        }

        private void CourseAndStudentInformationMainForm_Load(object sender, EventArgs e)
        {
             var user = DataContainer.Instance.CurrentUser;

            lbl_fullName.Text = "Logged in as: " + user.Name.ToString();

            lbl_modificationCount.Text = DataContainer.Instance.NumOfModifiedFiles().ToString();

            if (user.PermissionsType == ePermissionsType.Full)
            {
                btn_advancedSettings.Enabled = true;
                btn_advancedSettings.Visible = true;
            }
            else
            {
                foreach (Control ctrl in pnl_import.Controls)
                {
                    ctrl.Enabled = false;
                }
            }

            foreach (var term in DataContainer.Instance.TermDictionary.Values)
            {
                cmbBx_term.Items.Add(term.ToString());
                cmbBx_term.SelectedIndex = 0;
            }

            foreach (var columnSet in DataContainer.Instance.EvaluationFileCriterionColumnsRangeSetDictionary)
            {
                cmbBx_columnSet.Items.Add(columnSet.Value.Name);
                cmbBx_columnSet.SelectedIndex = 0;
            }
        }

        private void btn_student_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new StudentForm());
        }

        private void btn_evaluationColorSet_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new EvaluationColorSetForm());
        }

        private void btn_qualitativeCriterion_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new QualitativeEvaluationCriterionForm());
        }

        private void btn_qualitativeEvaluationCriteria_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new QualitativeEvaluationCriteriaForm());
        }

        private void btn_qualitativeEvaluationCriteriaSet_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new QualitativeEvaluationCriteriaSetForm());
        }

        private void btn_quantitativeEvaluationCriterion_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new QuantitativeEvaluationCriterionForm());
        }

        private void btn_quantitativeEvaluationCriteria_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new QuantitativeEvaluationCriteriaForm());
        }

        private void btn_quantitativeEvaluationCriteriaSet_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new QuantitativeEvaluationCriteriaSetForm());
        }

        private void btn_courseBase_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new CourseBaseForm());
        }

        private void btn_course_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new CourseForm());
        }

        private void btn_courseGroup_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new CourseGroupForm());
        }

        private void btn_faculty_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new FacultyForm());
        }

        private void btn_advancedSettings_Click(object sender, EventArgs e)
        {
            // Toggle accessibility
            pnl_advancedSettings.Enabled = !pnl_advancedSettings.Enabled;
            pnl_advancedSettings.Visible = !pnl_advancedSettings.Visible;
        }

        private void btn_applyModifications_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Would you really like to apply modifications?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DataContainer.Instance.SharePointConnectionManager.UploadAllModifiedFiles();
                lbl_modificationCount.Text = DataContainer.Instance.NumOfModifiedFiles().ToString();
            }
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnFlDlg_importingFile.ShowDialog() == DialogResult.OK)
                    lbl_fileName.Text = string.Join(" ", opnFlDlg_importingFile.FileNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown Error: " + ex.Message + "Please try it again.");
            }
        }

        private void btn_importEvaluation_Click(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;

            bool semesterAdditionSucceeded = true;

            string termString = cmbBx_term.SelectedItem.ToString();
            eTerm term = termString.ToCorrespondingEnumValue<eTerm>();
            int year = Convert.ToInt32(numUpDwn_year.Value);
            int semesterId = default;
            {
                int yearId = container.YearDictionary.GetFirstKeyOrDefault(year);
                if (yearId == default) // If the item does not exist
                {
                    yearId = DocumentLoader.AddYearToCSV(year);
                    if (yearId == default)
                        semesterAdditionSucceeded = false;
                }

                int termId = container.TermDictionary.GetFirstKey(term);

                semesterId = container.SemesterDictionary.FirstOrDefault(x => x.Value.Year == year
                                                                        && x.Value.Term == term).Key;
                if (semesterId == default) // If the item does not exist
                {
                    semesterId = DocumentLoader.AddSemesterToCSV(yearId, termId);
                    if (semesterId == default)
                        semesterAdditionSucceeded = false;
                }
            }

            if (semesterAdditionSucceeded)
            {
                Semester semester = container.SemesterDictionary[semesterId];

                string columnSetName = cmbBx_columnSet.SelectedItem.ToString();
                var columnSet = container.EvaluationFileCriterionColumnsRangeSetDictionary.First(x => x.Value.Name == columnSetName).Value;

                DialogResult dialogResult = MessageBox.Show("Would you really like to import evaluation data?"
                                                        + "\n Semester: " + termString + " " + year.ToString()
                                                        + "\n File: " + lbl_fileName.Text, "Caution!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    DocumentLoader.LoadEvaluationDataFromExcel(lbl_fileName.Text, semester, columnSet);
            }
            else
                MessageBox.Show("Failed to add semester!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SwitchTo(new StudentCoursesForm());
        }

        private void btn_columnSet_Click(object sender, EventArgs e)
        {
            this.SwitchTo(new EvaluationFileColumnsForm());
        }
    }
}
