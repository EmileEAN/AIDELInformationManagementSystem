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
    public partial class CourseForm: Form
    {
        public CourseForm()
        {
            InitializeComponent();
        }

        private void CourseForm_Load(object sender, EventArgs e)
        {
            InitializeMainViewTable();

            InitializeCourseBaseComboBox(cmbBx_courseBase_add);
            InitializeCourseBaseComboBox(cmbBx_courseBase_edit);
            InitializeCourseBaseComboBox(cmbBx_courseBase_delete);

            InitializeTermComboBox(cmbBx_term_add);
            InitializeTermComboBox(cmbBx_term_edit);
            InitializeTermComboBox(cmbBx_term_delete);

            numUpDwn_year_add.Value = numUpDwn_year_edit.Value = numUpDwn_year_delete.Value = DateTime.Now.Year;

            InitializeGroupNameFormatComboBox(cmbBx_groupNameFormat_add);
            InitializeGroupNameFormatComboBox(cmbBx_groupNameFormat_delete);

            InitializeQuantitativeCriteriaSetComboBox(cmbBx_quantitativeCriteria_add);
            InitializeQuantitativeCriteriaSetComboBox(cmbBx_quantitativeCriteria_edit);
            InitializeQuantitativeCriteriaSetComboBox(cmbBx_quantitativeCriteria_delete);

            InitializeQualitativeCriteriaSetComboBox(cmbBx_qualitativeCriteria_add);
            InitializeQualitativeCriteriaSetComboBox(cmbBx_qualitativeCriteria_edit);
            InitializeQualitativeCriteriaSetComboBox(cmbBx_qualitativeCriteria_delete);

            InitializeQuantitativeCriteriaTable(dgv_quantitativeCriteria_add);
            InitializeQuantitativeCriteriaTable(dgv_quantitativeCriteria_edit);
            InitializeQuantitativeCriteriaTable(dgv_quantitativeCriteria_delete);

            InitializeQualitativeCriteriaTable(dgv_qualitativeCriteria_add);
            InitializeQualitativeCriteriaTable(dgv_qualitativeCriteria_edit);
            InitializeQualitativeCriteriaTable(dgv_qualitativeCriteria_delete);

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
        private Course m_selectedCourse;
        private int m_numOfSelectedCourseGroups;
        private eGroupNamingFormat m_groupNameFormat;
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
                m_selectedCourse = null;
                m_numOfSelectedCourseGroups = 0;
                m_groupNameFormat = default;
            }
            else if (dgv_main.Rows.Count != 0)
            {
                m_selectedRow = dgv_main.SelectedRows[0];
                m_selectedCourse = DataContainer.Instance.CourseDictionary.First(x => x.Value.Base.ToString() == "(" + (string)(SelectedCells["Id"].Value) + ") " + (string)(SelectedCells["Name"].Value)
                                                                                        && x.Value.Semester.Term.ToString() == (string)(SelectedCells["Term"].Value)
                                                                                        && x.Value.Semester.Year.ToString() == (string)(SelectedCells["Year"].Value)
                                                                                        && x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name == (string)(SelectedCells["QuantitativeEvaluation"].Value)
                                                                                        && x.Value.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name == (string)(SelectedCells["QualitativeEvaluation"].Value)).Value;
                m_numOfSelectedCourseGroups = Convert.ToInt32((string)(SelectedCells["NumOfGroups"].Value));
                m_groupNameFormat = ((string)(SelectedCells["GroupNameFormat"].Value)).ToCorrespondingEnumValue<eGroupNamingFormat>();
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
        private void cmbBx_quantitativeCriteria_add_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaSetName = cmbBx_quantitativeCriteria_add.SelectedItem.ToString();

            var criteriaSet = (selectedCriteriaSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == selectedCriteriaSetName).Value :
                    null;

            RefreshQuantitativeCriteriaTable(dgv_quantitativeCriteria_add, criteriaSet);
        }

        private void cmbBx_qualitativeCriteria_add_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaSetName = cmbBx_qualitativeCriteria_add.SelectedItem.ToString();

            var criteriaSet = (selectedCriteriaSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == selectedCriteriaSetName).Value :
                    null;

            RefreshQualitativeCriteriaTable(dgv_qualitativeCriteria_add, criteriaSet);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Add())
            {
                DataContainer container = DataContainer.Instance;
                var courseBase = container.CourseBaseList.First(x => x.ToString() == cmbBx_courseBase_add.SelectedItem.ToString());
                var numOfGroups = Convert.ToInt32(numUpDwn_numberOfGroups_add.Value);
                var groupNameFormat = cmbBx_groupNameFormat_add.SelectedItem.ToString().ToCorrespondingEnumValue<eGroupNamingFormat>();
                var term = cmbBx_term_add.SelectedItem.ToString().ToCorrespondingEnumValue<eTerm>();
                var year = Convert.ToInt32(numUpDwn_year_add.Value);
                var quantitativeEvaluationCriteriaSet = container.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == cmbBx_quantitativeCriteria_add.SelectedItem.ToString()).Value;
                var qualitativeEvaluationCriteriaSet = container.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == cmbBx_qualitativeCriteria_add.SelectedItem.ToString()).Value;

                if (DocumentLoader.AddCourseToCSV(courseBase, numOfGroups, groupNameFormat, term, year, quantitativeEvaluationCriteriaSet, qualitativeEvaluationCriteriaSet) != default)
                {
                    MessageBox.Show("Added new course.");

                    ClearInput_Add();
                    RefreshMainViewTable();
                    RefreshQuantitativeCriteriaSetComboBox(cmbBx_quantitativeCriteria_add);
                    RefreshQuantitativeCriteriaSetComboBox(cmbBx_quantitativeCriteria_edit);
                    RefreshQuantitativeCriteriaSetComboBox(cmbBx_quantitativeCriteria_delete);
                    RefreshQualitativeCriteriaSetComboBox(cmbBx_qualitativeCriteria_add);
                    RefreshQualitativeCriteriaSetComboBox(cmbBx_qualitativeCriteria_edit);
                    RefreshQualitativeCriteriaSetComboBox(cmbBx_qualitativeCriteria_delete);
                }
                else
                    MessageBox.Show("Failed to add new course! Please try again.");
            }
        }
        #endregion

        #region Edit Tab
        private void cmbBx_quantitativeCriteria_edit_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaSetName = cmbBx_quantitativeCriteria_edit.SelectedItem.ToString();

            var criteriaSet = (selectedCriteriaSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == selectedCriteriaSetName).Value :
                    null;

            RefreshQuantitativeCriteriaTable(dgv_quantitativeCriteria_edit, criteriaSet);
        }

        private void cmbBx_qualitativeCriteria_edit_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaSetName = cmbBx_qualitativeCriteria_edit.SelectedItem.ToString();

            var criteriaSet = (selectedCriteriaSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == selectedCriteriaSetName).Value :
                    null;

            RefreshQualitativeCriteriaTable(dgv_qualitativeCriteria_edit, criteriaSet);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (ValidateInput_Edit())
            {
                DataContainer container = DataContainer.Instance;
                var courseBase = container.CourseBaseList.First(x => x.ToString() == cmbBx_courseBase_edit.SelectedItem.ToString());
                var term = cmbBx_term_edit.SelectedItem.ToString().ToCorrespondingEnumValue<eTerm>();
                var year = Convert.ToInt32(numUpDwn_year_edit.Value);
                var quantitativeEvaluationCriteriaSet = container.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == cmbBx_quantitativeCriteria_edit.SelectedItem.ToString()).Value;
                var qualitativeEvaluationCriteriaSet = container.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == cmbBx_qualitativeCriteria_edit.SelectedItem.ToString()).Value;

                if (DocumentLoader.EditCourseInCSV(m_selectedCourse, term, year, quantitativeEvaluationCriteriaSet, qualitativeEvaluationCriteriaSet))
                {
                    MessageBox.Show("Edited course successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to edit course! Please try again.");
            }
        }
        #endregion

        #region Delete Tab
        private void cmbBx_quantitativeCriteria_delete_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaSetName = cmbBx_quantitativeCriteria_delete.SelectedItem.ToString();

            var criteriaSet = (selectedCriteriaSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == selectedCriteriaSetName).Value :
                    null;

            RefreshQuantitativeCriteriaTable(dgv_quantitativeCriteria_delete, criteriaSet);
        }

        private void cmbBx_qualitativeCriteria_delete_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCriteriaSetName = cmbBx_qualitativeCriteria_delete.SelectedItem.ToString();

            var criteriaSet = (selectedCriteriaSetName != CoreValues.ComboBox_DefaultString) ?
                    DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value.Name == selectedCriteriaSetName).Value :
                    null;

            RefreshQualitativeCriteriaTable(dgv_qualitativeCriteria_delete, criteriaSet);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to delete the selected item?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int selectedCourseId = DataContainer.Instance.CourseDictionary.First(x => x.Value == m_selectedCourse).Key;
                if (DocumentLoader.DeleteCourseFromCSV(selectedCourseId))
                {
                    MessageBox.Show("Deleted course successfully.");

                    RefreshMainViewTable();
                }
                else
                    MessageBox.Show("Failed to delete course! Please try again.");
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
                columns.Add("NumOfGroups", "Number Of Groups");
                columns.Add("GroupNameFormat", "Group Name Format");
                columns.Add("QuantitativeEvaluation", "Quantitative Evaluation");
                columns.Add("QualitativeEvaluation", "Qualitative Evaluation");
            }
        }

        private void InitializeCourseBaseComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var courseBase in DataContainer.Instance.CourseBaseList)
            {
                items.Add(courseBase.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeTermComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var term in DataContainer.Instance.TermDictionary.Values)
            {
                items.Add(term.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeGroupNameFormatComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var groupNameFormat in Enum.GetValues(typeof(eGroupNamingFormat)))
            {
                items.Add(groupNameFormat.ToString());
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeQuantitativeCriteriaSetComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var criteriaSet in DataContainer.Instance.QuantitativeEvaluationCriteriaSetDictionary.Values)
            {
                items.Add(criteriaSet.Name);
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeQualitativeCriteriaSetComboBox(ComboBox _cmbBx)
        {
            // Set items
            var items = _cmbBx.Items;
            items.Clear();
            string defaultString = CoreValues.ComboBox_DefaultString;
            items.Add(defaultString);

            foreach (var criteriaSet in DataContainer.Instance.QualitativeEvaluationCriteriaSetDictionary.Values)
            {
                items.Add(criteriaSet.Name);
            }

            _cmbBx.SelectedIndex = _cmbBx.FindStringExact(defaultString);
        }

        private void InitializeQuantitativeCriteriaTable(DataGridView _dgv)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;

                columns.Add("Criteria", "(Weight) Criteria");
                columns.Add("Criterion", "Criterion");
                columns.Add("Weight", "Weight");
                columns.Add("ValueType", "Value Type");
                columns.Add("ValueRange", "Value Range");

                _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }

        private void InitializeQualitativeCriteriaTable(DataGridView _dgv)
        {
            // Set columns
            if (_dgv.ColumnCount == 0)
            {
                var columns = _dgv.Columns;

                columns.Add("Type", "Type");
                columns.Add("Criterion", "Criterion");
                columns.Add("EvaluationColorSet", "Evaluation Color Set");

                _dgv.SelectionChanged += (_sender, _e) => _dgv.ClearSelection();
            }
        }
        #endregion

        #region Component Fill
        private void FillEditTabComponents()
        {
            if (SelectedCells != null)
            {
                cmbBx_courseBase_edit.SelectedIndex = cmbBx_courseBase_edit.FindStringExact(m_selectedCourse.Base.ToString());

                cmbBx_term_edit.SelectedIndex = cmbBx_term_edit.FindStringExact(m_selectedCourse.Semester.Term.ToString());
                numUpDwn_year_edit.Value = m_selectedCourse.Semester.Year;

                cmbBx_quantitativeCriteria_edit.SelectedIndex = cmbBx_quantitativeCriteria_edit.FindStringExact(m_selectedCourse.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name);
                cmbBx_qualitativeCriteria_edit.SelectedIndex = cmbBx_qualitativeCriteria_edit.FindStringExact(m_selectedCourse.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name);
            }
            else
            {
                string defaultString = CoreValues.ComboBox_DefaultString;

                cmbBx_courseBase_edit.SelectedIndex = cmbBx_courseBase_edit.FindStringExact(defaultString);

                cmbBx_term_edit.SelectedIndex = cmbBx_term_edit.FindStringExact(defaultString);
                numUpDwn_year_edit.Value = DateTime.Now.Year;

                cmbBx_quantitativeCriteria_edit.SelectedIndex = cmbBx_quantitativeCriteria_edit.FindStringExact(defaultString);
                cmbBx_qualitativeCriteria_edit.SelectedIndex = cmbBx_qualitativeCriteria_edit.FindStringExact(defaultString);
            }
        }

        private void FillDeleteTabComponents()
        {
            if (SelectedCells != null)
            {
                cmbBx_courseBase_delete.SelectedIndex = cmbBx_courseBase_delete.FindStringExact(m_selectedCourse.Base.ToString());

                cmbBx_term_delete.SelectedIndex = cmbBx_term_delete.FindStringExact(m_selectedCourse.Semester.Term.ToString());
                numUpDwn_year_delete.Value = m_selectedCourse.Semester.Year;

                numUpDwn_numberOfGroups_delete.Value = m_numOfSelectedCourseGroups;
                cmbBx_groupNameFormat_delete.SelectedIndex = cmbBx_courseBase_delete.FindStringExact(m_groupNameFormat.ToString());

                cmbBx_quantitativeCriteria_delete.SelectedIndex = cmbBx_quantitativeCriteria_delete.FindStringExact(m_selectedCourse.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name);
                cmbBx_qualitativeCriteria_delete.SelectedIndex = cmbBx_qualitativeCriteria_delete.FindStringExact(m_selectedCourse.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name);
            }
            else
            {
                string defaultString = CoreValues.ComboBox_DefaultString;

                cmbBx_courseBase_delete.SelectedIndex = cmbBx_courseBase_delete.FindStringExact(defaultString);

                cmbBx_term_delete.SelectedIndex = cmbBx_term_delete.FindStringExact(defaultString);
                numUpDwn_year_delete.Value = DateTime.Now.Year;

                numUpDwn_numberOfGroups_delete.Value = numUpDwn_numberOfGroups_delete.Minimum;
                cmbBx_groupNameFormat_delete.SelectedIndex = cmbBx_courseBase_delete.FindStringExact(defaultString);

                cmbBx_quantitativeCriteria_delete.SelectedIndex = cmbBx_quantitativeCriteria_delete.FindStringExact(defaultString);
                cmbBx_qualitativeCriteria_delete.SelectedIndex = cmbBx_qualitativeCriteria_delete.FindStringExact(defaultString);
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            // Set rows
            ApplyFilter_Main();
        }

        private void RefreshQuantitativeCriteriaSetComboBox(ComboBox _cmbBx)
        {
            InitializeQuantitativeCriteriaSetComboBox(_cmbBx);
        }

        private void RefreshQualitativeCriteriaSetComboBox(ComboBox _cmbBx)
        {
            InitializeQualitativeCriteriaSetComboBox(_cmbBx);
        }

        private void RefreshQuantitativeCriteriaTable(DataGridView _dgv, CriteriaSet_QuantitativeEvaluation _criteriaSet)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_criteriaSet == null)
                return;

            // First Partial
            {
                var criteria = _criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Criteria;
                foreach (var criterionWeight in criteria.WeightPerCriterion)
                {
                    var criterion = criterionWeight.Criterion;
                    rows.Add(new string[] { "(" + _criteriaSet.WeightPercentage_FirstPartial + ") First Partial" , criterion.String, criteria.CriterionWeightPercentage(criterionWeight), criterion.EvaluationValueType.ToString(), criterion.Min.ToString() + " - " + criterion.Max.ToString() });
                }
            }

            // Midterm
            {
                var criteria = _criteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm.Criteria;
                foreach (var criterionWeight in criteria.WeightPerCriterion)
                {
                    var criterion = criterionWeight.Criterion;
                    rows.Add(new string[] { "(" + _criteriaSet.WeightPercentage_Midterm + ") Midterm" , criterion.String, criteria.CriterionWeightPercentage(criterionWeight), criterion.EvaluationValueType.ToString(), criterion.Min.ToString() + " - " + criterion.Max.ToString() });
                }
            }

            // Finals
            {
                var criteria = _criteriaSet.CriteriaWeight_QuantitativeEvaluation_Final.Criteria;
                foreach (var criterionWeight in criteria.WeightPerCriterion)
                {
                    var criterion = criterionWeight.Criterion;
                    rows.Add(new string[] { "(" + _criteriaSet.WeightPercentage_Final + ") Final" , criterion.String, criteria.CriterionWeightPercentage(criterionWeight), criterion.EvaluationValueType.ToString(), criterion.Min.ToString() + " - " + criterion.Max.ToString() });
                }
            }

            // Additional
            {
                var criteria = _criteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional.Criteria;
                foreach (var criterionWeight in criteria.WeightPerCriterion)
                {
                    var criterion = criterionWeight.Criterion;
                    rows.Add(new string[] { "(" + _criteriaSet.WeightPercentage_Additional + ") Additional" , criterion.String, criteria.CriterionWeightPercentage(criterionWeight), criterion.EvaluationValueType.ToString(), criterion.Min.ToString() + " - " + criterion.Max.ToString() });
                }
            }
        }

        private void RefreshQualitativeCriteriaTable(DataGridView _dgv, CriteriaSet_QualitativeEvaluation _criteriaSet)
        {
            // Set rows
            var rows = _dgv.Rows;
            rows.Clear();

            if (_criteriaSet == null)
                return;

            // First Partial
            {
                foreach (var criterion in _criteriaSet.Criteria_QualitativeEvaluation_FirstPartial.CriterionList)
                {
                    rows.Add(new string[] { "First Partial", criterion.String, criterion.EvaluationColorSet.Name });
                }
            }

            // Midterm
            {
                foreach (var criterion in _criteriaSet.Criteria_QualitativeEvaluation_Midterm.CriterionList)
                {
                    rows.Add(new string[] { "Midterm", criterion.String, criterion.EvaluationColorSet.Name });
                }
            }

            // Finals
            {
                foreach (var criterion in _criteriaSet.Criteria_QualitativeEvaluation_Final.CriterionList)
                {
                    rows.Add(new string[] { "Final", criterion.String, criterion.EvaluationColorSet.Name });
                }
            }
        }
        #endregion

        #region Data Clearance
        private void ClearInput_Add()
        {
            string defaultString = CoreValues.ComboBox_DefaultString;
            cmbBx_courseBase_add.SelectedIndex = cmbBx_courseBase_add.FindStringExact(defaultString);
            cmbBx_term_add.SelectedIndex = cmbBx_term_add.FindStringExact(defaultString);
            numUpDwn_year_add.Value = DateTime.Now.Year;
            numUpDwn_numberOfGroups_add.Value = numUpDwn_numberOfGroups_add.Minimum;
            cmbBx_groupNameFormat_add.SelectedIndex = cmbBx_groupNameFormat_add.FindStringExact(defaultString);
            cmbBx_quantitativeCriteria_add.SelectedIndex = cmbBx_quantitativeCriteria_add.FindStringExact(defaultString);
            cmbBx_qualitativeCriteria_add.SelectedIndex = cmbBx_qualitativeCriteria_add.FindStringExact(defaultString);
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            m_addingRows = true;

            string filterText = txtBx_filter_main.Text;

            var courseDictionary = DataContainer.Instance.CourseDictionary;
            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? courseDictionary : courseDictionary.Where(x => x.Value.Base.Id.ToString().Contains(filterText)
                                                                                                                                || x.Value.Base.Name.Contains(filterText)
                                                                                                                                || x.Value.Semester.Term.ToString().Contains(filterText)
                                                                                                                                || x.Value.Semester.Year.ToString().Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.CriteriaWeight_QuantitativeEvaluation_FirstPartial.Criteria.Name.Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.CriteriaWeight_QuantitativeEvaluation_Midterm.Criteria.Name.Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.CriteriaWeight_QuantitativeEvaluation_Final.Criteria.Name.Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.CriteriaWeight_QuantitativeEvaluation_Additional.Criteria.Name.Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Criteria_QualitativeEvaluation_FirstPartial.Name.Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Criteria_QualitativeEvaluation_Midterm.Name.Contains(filterText)
                                                                                                                                || x.Value.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Criteria_QualitativeEvaluation_Final.Name.Contains(filterText));
            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var element in targetDictionary)
                {
                    Course course = element.Value;
                    CourseBase courseBase = course.Base;
                    Semester semester = course.Semester;
                    var groups = DataContainer.Instance.CourseGroupDictionary.Values.Where(x => x.Course == course).ToList();
                    int numberOfGroups = groups.Count();
                    eGroupNamingFormat groupNamingFormat = Regex.IsMatch(groups[0].Name, "[a-z]", RegexOptions.IgnoreCase) ? eGroupNamingFormat.Alphabet : eGroupNamingFormat.Number;
                    FullEvaluationCriteria fullEvaluationCriteria = course.FullEvaluationCriteria;

                    var row = new string[] { courseBase.Id.ToString(), courseBase.Name, semester.Term.ToString(), semester.Year.ToString(), numberOfGroups.ToString(), groupNamingFormat.ToString(), fullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name, fullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name };
                    rows.Add(row);

                    string selectedCourseId = (m_selectedCourse != null) ? m_selectedCourse.Base.Id.ToString() : "";
                    if (selectedCourseId != string.Empty && row[0] == selectedCourseId)
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

            if (cmbBx_courseBase_add.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a course base!");

            if (cmbBx_term_add.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a term!");

            if (cmbBx_groupNameFormat_add.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a group name format!");

            if (cmbBx_quantitativeCriteria_add.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a quantitative criteria set!");

            if (cmbBx_qualitativeCriteria_add.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a qualitative criteria set!");

            else if (DataContainer.Instance.CourseDictionary.Any(x => x.Value.Base.ToString() == cmbBx_courseBase_add.SelectedItem.ToString()
                                                                        || x.Value.Semester.Term.ToString() == cmbBx_term_add.SelectedItem.ToString()
                                                                        || x.Value.Semester.Year == numUpDwn_year_add.Value
                                                                        || x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name == cmbBx_quantitativeCriteria_add.SelectedItem.ToString()
                                                                        || x.Value.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name == cmbBx_qualitativeCriteria_add.SelectedItem.ToString()))
                errorMessages.Add("An course with the same properties already exists!");

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

            if (cmbBx_courseBase_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a course base!");

            if (cmbBx_term_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a term!");

            if (cmbBx_quantitativeCriteria_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a quantitative criteria set!");

            if (cmbBx_qualitativeCriteria_edit.SelectedItem.ToString() == defaultString)
                errorMessages.Add("Select a qualitative criteria set!");

            if (!HaveDataBeenChanged())
                errorMessages.Add("Modify at least one field!");

            if (DataContainer.Instance.CourseDictionary.Any(x => x.Value.Base.ToString() == cmbBx_courseBase_edit.SelectedItem.ToString()
                                                            || x.Value.Semester.Term.ToString() == cmbBx_term_edit.SelectedItem.ToString()
                                                            || x.Value.Semester.Year == numUpDwn_year_edit.Value
                                                            || x.Value.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name == cmbBx_quantitativeCriteria_edit.SelectedItem.ToString()
                                                            || x.Value.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name == cmbBx_qualitativeCriteria_edit.SelectedItem.ToString()))
                errorMessages.Add("An course with the same properties already exists!");

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
            return cmbBx_courseBase_edit.SelectedItem.ToString() != m_selectedCourse.Base.ToString()
                || cmbBx_term_edit.SelectedItem.ToString() != m_selectedCourse.Semester.Term.ToString()
                || numUpDwn_year_edit.Value != m_selectedCourse.Semester.Year
                || cmbBx_term_edit.SelectedItem.ToString() != m_selectedCourse.Semester.Term.ToString()
                || cmbBx_quantitativeCriteria_edit.SelectedItem.ToString() != m_selectedCourse.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation.Name
                || cmbBx_qualitativeCriteria_edit.SelectedItem.ToString() != m_selectedCourse.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation.Name;
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
