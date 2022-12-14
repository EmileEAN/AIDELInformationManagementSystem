namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class CourseAndStudentInformationMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btn_advancedSettings = new System.Windows.Forms.Button();
            this.btn_courseGroup = new System.Windows.Forms.Button();
            this.btn_student = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_faculty = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnl_advancedSettings = new System.Windows.Forms.Panel();
            this.pnl_import = new System.Windows.Forms.Panel();
            this.numUpDwn_year = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbBx_term = new System.Windows.Forms.ComboBox();
            this.lbl_fileName = new System.Windows.Forms.Label();
            this.btn_browse = new System.Windows.Forms.Button();
            this.btn_importEvaluation = new System.Windows.Forms.Button();
            this.btn_course = new System.Windows.Forms.Button();
            this.btn_courseBase = new System.Windows.Forms.Button();
            this.btn_evaluationColorSet = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_qualitativeEvaluationCriteria = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_qualitativeCriterion = new System.Windows.Forms.Button();
            this.btn_quantitativeEvaluationCriteriaSet = new System.Windows.Forms.Button();
            this.btn_qualitativeEvaluationCriteriaSet = new System.Windows.Forms.Button();
            this.btn_quantitativeEvaluationCriterion = new System.Windows.Forms.Button();
            this.btn_quantitativeEvaluationCriteria = new System.Windows.Forms.Button();
            this.lbl_fullName = new System.Windows.Forms.Label();
            this.btn_applyModifications = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_modificationCount = new System.Windows.Forms.Label();
            this.opnFlDlg_importingFile = new System.Windows.Forms.OpenFileDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbBx_columnSet = new System.Windows.Forms.ComboBox();
            this.btn_columnSet = new System.Windows.Forms.Button();
            this.pnl_advancedSettings.SuspendLayout();
            this.pnl_import.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_year)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 37);
            this.label1.TabIndex = 5;
            this.label1.Text = "Course";
            // 
            // btn_advancedSettings
            // 
            this.btn_advancedSettings.Enabled = false;
            this.btn_advancedSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_advancedSettings.Location = new System.Drawing.Point(1047, 621);
            this.btn_advancedSettings.Name = "btn_advancedSettings";
            this.btn_advancedSettings.Size = new System.Drawing.Size(205, 48);
            this.btn_advancedSettings.TabIndex = 6;
            this.btn_advancedSettings.Text = "Advanced Settings";
            this.btn_advancedSettings.UseVisualStyleBackColor = true;
            this.btn_advancedSettings.Visible = false;
            this.btn_advancedSettings.Click += new System.EventHandler(this.btn_advancedSettings_Click);
            // 
            // btn_courseGroup
            // 
            this.btn_courseGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_courseGroup.Location = new System.Drawing.Point(15, 113);
            this.btn_courseGroup.Name = "btn_courseGroup";
            this.btn_courseGroup.Size = new System.Drawing.Size(205, 48);
            this.btn_courseGroup.TabIndex = 7;
            this.btn_courseGroup.Text = "Course Group";
            this.btn_courseGroup.UseVisualStyleBackColor = true;
            this.btn_courseGroup.Click += new System.EventHandler(this.btn_courseGroup_Click);
            // 
            // btn_student
            // 
            this.btn_student.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_student.Location = new System.Drawing.Point(259, 113);
            this.btn_student.Name = "btn_student";
            this.btn_student.Size = new System.Drawing.Size(205, 48);
            this.btn_student.TabIndex = 11;
            this.btn_student.Text = "Student";
            this.btn_student.UseVisualStyleBackColor = true;
            this.btn_student.Click += new System.EventHandler(this.btn_student_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(297, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 37);
            this.label2.TabIndex = 10;
            this.label2.Text = "Student";
            // 
            // btn_faculty
            // 
            this.btn_faculty.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_faculty.Location = new System.Drawing.Point(505, 113);
            this.btn_faculty.Name = "btn_faculty";
            this.btn_faculty.Size = new System.Drawing.Size(205, 48);
            this.btn_faculty.TabIndex = 15;
            this.btn_faculty.Text = "Faculty";
            this.btn_faculty.UseVisualStyleBackColor = true;
            this.btn_faculty.Click += new System.EventHandler(this.btn_faculty_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(549, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 37);
            this.label3.TabIndex = 14;
            this.label3.Text = "Faculty";
            // 
            // pnl_advancedSettings
            // 
            this.pnl_advancedSettings.Controls.Add(this.pnl_import);
            this.pnl_advancedSettings.Controls.Add(this.btn_course);
            this.pnl_advancedSettings.Controls.Add(this.btn_courseBase);
            this.pnl_advancedSettings.Controls.Add(this.btn_evaluationColorSet);
            this.pnl_advancedSettings.Controls.Add(this.label5);
            this.pnl_advancedSettings.Controls.Add(this.btn_qualitativeEvaluationCriteria);
            this.pnl_advancedSettings.Controls.Add(this.label4);
            this.pnl_advancedSettings.Controls.Add(this.btn_qualitativeCriterion);
            this.pnl_advancedSettings.Controls.Add(this.btn_quantitativeEvaluationCriteriaSet);
            this.pnl_advancedSettings.Controls.Add(this.btn_qualitativeEvaluationCriteriaSet);
            this.pnl_advancedSettings.Controls.Add(this.btn_quantitativeEvaluationCriterion);
            this.pnl_advancedSettings.Controls.Add(this.btn_quantitativeEvaluationCriteria);
            this.pnl_advancedSettings.Enabled = false;
            this.pnl_advancedSettings.Location = new System.Drawing.Point(12, 167);
            this.pnl_advancedSettings.Name = "pnl_advancedSettings";
            this.pnl_advancedSettings.Size = new System.Drawing.Size(1240, 448);
            this.pnl_advancedSettings.TabIndex = 27;
            this.pnl_advancedSettings.Visible = false;
            // 
            // pnl_import
            // 
            this.pnl_import.Controls.Add(this.btn_columnSet);
            this.pnl_import.Controls.Add(this.label9);
            this.pnl_import.Controls.Add(this.cmbBx_columnSet);
            this.pnl_import.Controls.Add(this.numUpDwn_year);
            this.pnl_import.Controls.Add(this.label8);
            this.pnl_import.Controls.Add(this.label7);
            this.pnl_import.Controls.Add(this.cmbBx_term);
            this.pnl_import.Controls.Add(this.lbl_fileName);
            this.pnl_import.Controls.Add(this.btn_browse);
            this.pnl_import.Controls.Add(this.btn_importEvaluation);
            this.pnl_import.Location = new System.Drawing.Point(819, 227);
            this.pnl_import.Name = "pnl_import";
            this.pnl_import.Size = new System.Drawing.Size(420, 217);
            this.pnl_import.TabIndex = 43;
            // 
            // numUpDwn_year
            // 
            this.numUpDwn_year.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUpDwn_year.Location = new System.Drawing.Point(292, 9);
            this.numUpDwn_year.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numUpDwn_year.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numUpDwn_year.Name = "numUpDwn_year";
            this.numUpDwn_year.Size = new System.Drawing.Size(81, 36);
            this.numUpDwn_year.TabIndex = 42;
            this.numUpDwn_year.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numUpDwn_year.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(227, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 28);
            this.label8.TabIndex = 41;
            this.label8.Text = "Year:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(29, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 28);
            this.label7.TabIndex = 40;
            this.label7.Text = "Term:";
            // 
            // cmbBx_term
            // 
            this.cmbBx_term.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_term.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_term.FormattingEnabled = true;
            this.cmbBx_term.Location = new System.Drawing.Point(102, 9);
            this.cmbBx_term.Name = "cmbBx_term";
            this.cmbBx_term.Size = new System.Drawing.Size(107, 36);
            this.cmbBx_term.TabIndex = 39;
            // 
            // lbl_fileName
            // 
            this.lbl_fileName.AutoSize = true;
            this.lbl_fileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_fileName.Location = new System.Drawing.Point(124, 115);
            this.lbl_fileName.Name = "lbl_fileName";
            this.lbl_fileName.Size = new System.Drawing.Size(66, 16);
            this.lbl_fileName.TabIndex = 38;
            this.lbl_fileName.Text = "File name";
            // 
            // btn_browse
            // 
            this.btn_browse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_browse.Location = new System.Drawing.Point(31, 112);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(75, 23);
            this.btn_browse.TabIndex = 37;
            this.btn_browse.Text = "Browse";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // btn_importEvaluation
            // 
            this.btn_importEvaluation.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importEvaluation.Location = new System.Drawing.Point(31, 153);
            this.btn_importEvaluation.Name = "btn_importEvaluation";
            this.btn_importEvaluation.Size = new System.Drawing.Size(347, 48);
            this.btn_importEvaluation.TabIndex = 33;
            this.btn_importEvaluation.Text = "Import Evaluations From File";
            this.btn_importEvaluation.UseVisualStyleBackColor = true;
            this.btn_importEvaluation.Click += new System.EventHandler(this.btn_importEvaluation_Click);
            // 
            // btn_course
            // 
            this.btn_course.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_course.Location = new System.Drawing.Point(3, 3);
            this.btn_course.Name = "btn_course";
            this.btn_course.Size = new System.Drawing.Size(205, 48);
            this.btn_course.TabIndex = 29;
            this.btn_course.Text = "Course";
            this.btn_course.UseVisualStyleBackColor = true;
            this.btn_course.Click += new System.EventHandler(this.btn_course_Click);
            // 
            // btn_courseBase
            // 
            this.btn_courseBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_courseBase.Location = new System.Drawing.Point(3, 57);
            this.btn_courseBase.Name = "btn_courseBase";
            this.btn_courseBase.Size = new System.Drawing.Size(205, 48);
            this.btn_courseBase.TabIndex = 28;
            this.btn_courseBase.Text = "Course Base";
            this.btn_courseBase.UseVisualStyleBackColor = true;
            this.btn_courseBase.Click += new System.EventHandler(this.btn_courseBase_Click);
            // 
            // btn_evaluationColorSet
            // 
            this.btn_evaluationColorSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_evaluationColorSet.Location = new System.Drawing.Point(433, 397);
            this.btn_evaluationColorSet.Name = "btn_evaluationColorSet";
            this.btn_evaluationColorSet.Size = new System.Drawing.Size(224, 48);
            this.btn_evaluationColorSet.TabIndex = 36;
            this.btn_evaluationColorSet.Text = "Evaluation Color Set";
            this.btn_evaluationColorSet.UseVisualStyleBackColor = true;
            this.btn_evaluationColorSet.Click += new System.EventHandler(this.btn_evaluationColorSet_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(379, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(325, 37);
            this.label5.TabIndex = 32;
            this.label5.Text = "Qualitative Evaluation";
            // 
            // btn_qualitativeEvaluationCriteria
            // 
            this.btn_qualitativeEvaluationCriteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_qualitativeEvaluationCriteria.Location = new System.Drawing.Point(433, 289);
            this.btn_qualitativeEvaluationCriteria.Name = "btn_qualitativeEvaluationCriteria";
            this.btn_qualitativeEvaluationCriteria.Size = new System.Drawing.Size(224, 48);
            this.btn_qualitativeEvaluationCriteria.TabIndex = 35;
            this.btn_qualitativeEvaluationCriteria.Text = "Criteria";
            this.btn_qualitativeEvaluationCriteria.UseVisualStyleBackColor = true;
            this.btn_qualitativeEvaluationCriteria.Click += new System.EventHandler(this.btn_qualitativeEvaluationCriteria_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(345, 37);
            this.label4.TabIndex = 28;
            this.label4.Text = "Quantitative Evaluation";
            // 
            // btn_qualitativeCriterion
            // 
            this.btn_qualitativeCriterion.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_qualitativeCriterion.Location = new System.Drawing.Point(433, 343);
            this.btn_qualitativeCriterion.Name = "btn_qualitativeCriterion";
            this.btn_qualitativeCriterion.Size = new System.Drawing.Size(224, 48);
            this.btn_qualitativeCriterion.TabIndex = 34;
            this.btn_qualitativeCriterion.Text = "Criterion";
            this.btn_qualitativeCriterion.UseVisualStyleBackColor = true;
            this.btn_qualitativeCriterion.Click += new System.EventHandler(this.btn_qualitativeCriterion_Click);
            // 
            // btn_quantitativeEvaluationCriteriaSet
            // 
            this.btn_quantitativeEvaluationCriteriaSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_quantitativeEvaluationCriteriaSet.Location = new System.Drawing.Point(57, 235);
            this.btn_quantitativeEvaluationCriteriaSet.Name = "btn_quantitativeEvaluationCriteriaSet";
            this.btn_quantitativeEvaluationCriteriaSet.Size = new System.Drawing.Size(224, 48);
            this.btn_quantitativeEvaluationCriteriaSet.TabIndex = 29;
            this.btn_quantitativeEvaluationCriteriaSet.Text = "Set of Criteria";
            this.btn_quantitativeEvaluationCriteriaSet.UseVisualStyleBackColor = true;
            this.btn_quantitativeEvaluationCriteriaSet.Click += new System.EventHandler(this.btn_quantitativeEvaluationCriteriaSet_Click);
            // 
            // btn_qualitativeEvaluationCriteriaSet
            // 
            this.btn_qualitativeEvaluationCriteriaSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_qualitativeEvaluationCriteriaSet.Location = new System.Drawing.Point(433, 235);
            this.btn_qualitativeEvaluationCriteriaSet.Name = "btn_qualitativeEvaluationCriteriaSet";
            this.btn_qualitativeEvaluationCriteriaSet.Size = new System.Drawing.Size(224, 48);
            this.btn_qualitativeEvaluationCriteriaSet.TabIndex = 33;
            this.btn_qualitativeEvaluationCriteriaSet.Text = "Set of Criteria";
            this.btn_qualitativeEvaluationCriteriaSet.UseVisualStyleBackColor = true;
            this.btn_qualitativeEvaluationCriteriaSet.Click += new System.EventHandler(this.btn_qualitativeEvaluationCriteriaSet_Click);
            // 
            // btn_quantitativeEvaluationCriterion
            // 
            this.btn_quantitativeEvaluationCriterion.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_quantitativeEvaluationCriterion.Location = new System.Drawing.Point(57, 343);
            this.btn_quantitativeEvaluationCriterion.Name = "btn_quantitativeEvaluationCriterion";
            this.btn_quantitativeEvaluationCriterion.Size = new System.Drawing.Size(224, 48);
            this.btn_quantitativeEvaluationCriterion.TabIndex = 30;
            this.btn_quantitativeEvaluationCriterion.Text = "Criterion";
            this.btn_quantitativeEvaluationCriterion.UseVisualStyleBackColor = true;
            this.btn_quantitativeEvaluationCriterion.Click += new System.EventHandler(this.btn_quantitativeEvaluationCriterion_Click);
            // 
            // btn_quantitativeEvaluationCriteria
            // 
            this.btn_quantitativeEvaluationCriteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.btn_quantitativeEvaluationCriteria.Location = new System.Drawing.Point(57, 289);
            this.btn_quantitativeEvaluationCriteria.Name = "btn_quantitativeEvaluationCriteria";
            this.btn_quantitativeEvaluationCriteria.Size = new System.Drawing.Size(224, 48);
            this.btn_quantitativeEvaluationCriteria.TabIndex = 31;
            this.btn_quantitativeEvaluationCriteria.Text = "Criteria";
            this.btn_quantitativeEvaluationCriteria.UseVisualStyleBackColor = true;
            this.btn_quantitativeEvaluationCriteria.Click += new System.EventHandler(this.btn_quantitativeEvaluationCriteria_Click);
            // 
            // lbl_fullName
            // 
            this.lbl_fullName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_fullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_fullName.Location = new System.Drawing.Point(752, 9);
            this.lbl_fullName.Name = "lbl_fullName";
            this.lbl_fullName.Size = new System.Drawing.Size(500, 16);
            this.lbl_fullName.TabIndex = 29;
            this.lbl_fullName.Text = "Logged in as: Full Name";
            this.lbl_fullName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_applyModifications
            // 
            this.btn_applyModifications.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_applyModifications.Location = new System.Drawing.Point(862, 113);
            this.btn_applyModifications.Name = "btn_applyModifications";
            this.btn_applyModifications.Size = new System.Drawing.Size(347, 48);
            this.btn_applyModifications.TabIndex = 30;
            this.btn_applyModifications.Text = "Apply Modifications";
            this.btn_applyModifications.UseVisualStyleBackColor = true;
            this.btn_applyModifications.Click += new System.EventHandler(this.btn_applyModifications_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(898, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(311, 25);
            this.label6.TabIndex = 32;
            this.label6.Text = "Non-applied local modifications";
            // 
            // lbl_modificationCount
            // 
            this.lbl_modificationCount.AutoSize = true;
            this.lbl_modificationCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_modificationCount.Location = new System.Drawing.Point(865, 85);
            this.lbl_modificationCount.Name = "lbl_modificationCount";
            this.lbl_modificationCount.Size = new System.Drawing.Size(24, 25);
            this.lbl_modificationCount.TabIndex = 31;
            this.lbl_modificationCount.Text = "0";
            // 
            // opnFlDlg_importingFile
            // 
            this.opnFlDlg_importingFile.Filter = "Data files|*.csv;*xls;*xlsx;";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(29, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 28);
            this.label9.TabIndex = 44;
            this.label9.Text = "File Format:";
            // 
            // cmbBx_columnSet
            // 
            this.cmbBx_columnSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_columnSet.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_columnSet.FormattingEnabled = true;
            this.cmbBx_columnSet.Location = new System.Drawing.Point(159, 62);
            this.cmbBx_columnSet.Name = "cmbBx_columnSet";
            this.cmbBx_columnSet.Size = new System.Drawing.Size(214, 36);
            this.cmbBx_columnSet.TabIndex = 43;
            // 
            // btn_columnSet
            // 
            this.btn_columnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_columnSet.Location = new System.Drawing.Point(381, 69);
            this.btn_columnSet.Name = "btn_columnSet";
            this.btn_columnSet.Size = new System.Drawing.Size(22, 23);
            this.btn_columnSet.TabIndex = 45;
            this.btn_columnSet.Text = "+";
            this.btn_columnSet.UseVisualStyleBackColor = true;
            this.btn_columnSet.Click += new System.EventHandler(this.btn_columnSet_Click);
            // 
            // CourseAndStudentInformationMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbl_modificationCount);
            this.Controls.Add(this.btn_applyModifications);
            this.Controls.Add(this.lbl_fullName);
            this.Controls.Add(this.btn_faculty);
            this.Controls.Add(this.pnl_advancedSettings);
            this.Controls.Add(this.btn_student);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_courseGroup);
            this.Controls.Add(this.btn_advancedSettings);
            this.Controls.Add(this.label1);
            this.Name = "CourseAndStudentInformationMainForm";
            this.Text = "Course And Student Information";
            this.Load += new System.EventHandler(this.CourseAndStudentInformationMainForm_Load);
            this.pnl_advancedSettings.ResumeLayout(false);
            this.pnl_advancedSettings.PerformLayout();
            this.pnl_import.ResumeLayout(false);
            this.pnl_import.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_year)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_advancedSettings;
        private System.Windows.Forms.Button btn_courseGroup;
        private System.Windows.Forms.Button btn_student;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_faculty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnl_advancedSettings;
        private System.Windows.Forms.Button btn_evaluationColorSet;
        private System.Windows.Forms.Button btn_qualitativeEvaluationCriteria;
        private System.Windows.Forms.Button btn_qualitativeCriterion;
        private System.Windows.Forms.Button btn_qualitativeEvaluationCriteriaSet;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_quantitativeEvaluationCriteria;
        private System.Windows.Forms.Button btn_quantitativeEvaluationCriterion;
        private System.Windows.Forms.Button btn_quantitativeEvaluationCriteriaSet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_course;
        private System.Windows.Forms.Button btn_courseBase;
        private System.Windows.Forms.Label lbl_fullName;
        private System.Windows.Forms.Button btn_applyModifications;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_modificationCount;
        private System.Windows.Forms.Button btn_importEvaluation;
        private System.Windows.Forms.OpenFileDialog opnFlDlg_importingFile;
        private System.Windows.Forms.NumericUpDown numUpDwn_year;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbBx_term;
        private System.Windows.Forms.Label lbl_fileName;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Panel pnl_import;
        private System.Windows.Forms.Button btn_columnSet;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbBx_columnSet;
    }
}