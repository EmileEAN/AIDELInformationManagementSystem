namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    partial class ExamScheduleForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nmUpDwn_year = new System.Windows.Forms.NumericUpDown();
            this.nmUpDwn_day = new System.Windows.Forms.NumericUpDown();
            this.nmUpDwn_hour = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nmUpDwn_minute = new System.Windows.Forms.NumericUpDown();
            this.btn_addExam = new System.Windows.Forms.Button();
            this.btn_deleteExam = new System.Windows.Forms.Button();
            this.cmbBx_examType = new System.Windows.Forms.ComboBox();
            this.cmbBx_month = new System.Windows.Forms.ComboBox();
            this.btn_return = new System.Windows.Forms.Button();
            this.cmbBx_month_regInit = new System.Windows.Forms.ComboBox();
            this.nmUpDwn_day_regInit = new System.Windows.Forms.NumericUpDown();
            this.nmUpDwn_year_regInit = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbBx_month_regEnd = new System.Windows.Forms.ComboBox();
            this.nmUpDwn_day_regEnd = new System.Windows.Forms.NumericUpDown();
            this.nmUpDwn_year_regEnd = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.nmUpDwn_maxNumOfExaminees = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_year)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_day)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_hour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_minute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_day_regInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_year_regInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_day_regEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_year_regEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_maxNumOfExaminees)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_main
            // 
            this.dgv_main.AllowUserToAddRows = false;
            this.dgv_main.AllowUserToDeleteRows = false;
            this.dgv_main.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_main.Location = new System.Drawing.Point(665, 58);
            this.dgv_main.MultiSelect = false;
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.ReadOnly = true;
            this.dgv_main.RowHeadersWidth = 10;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_main.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_main.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_main.Size = new System.Drawing.Size(543, 564);
            this.dgv_main.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(660, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 28);
            this.label11.TabIndex = 11;
            this.label11.Text = "Schedule";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 28);
            this.label1.TabIndex = 12;
            this.label1.Text = "Exam Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(53, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 28);
            this.label2.TabIndex = 13;
            this.label2.Text = "Year";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(53, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 28);
            this.label3.TabIndex = 14;
            this.label3.Text = "Month";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(53, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 28);
            this.label4.TabIndex = 15;
            this.label4.Text = "Day";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(55, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 28);
            this.label5.TabIndex = 16;
            this.label5.Text = "Time";
            // 
            // nmUpDwn_year
            // 
            this.nmUpDwn_year.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_year.Location = new System.Drawing.Point(160, 108);
            this.nmUpDwn_year.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nmUpDwn_year.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nmUpDwn_year.Name = "nmUpDwn_year";
            this.nmUpDwn_year.Size = new System.Drawing.Size(120, 33);
            this.nmUpDwn_year.TabIndex = 17;
            this.nmUpDwn_year.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_year.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // nmUpDwn_day
            // 
            this.nmUpDwn_day.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_day.Location = new System.Drawing.Point(160, 197);
            this.nmUpDwn_day.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nmUpDwn_day.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmUpDwn_day.Name = "nmUpDwn_day";
            this.nmUpDwn_day.Size = new System.Drawing.Size(120, 33);
            this.nmUpDwn_day.TabIndex = 18;
            this.nmUpDwn_day.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_day.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmUpDwn_hour
            // 
            this.nmUpDwn_hour.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_hour.Location = new System.Drawing.Point(160, 245);
            this.nmUpDwn_hour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nmUpDwn_hour.Name = "nmUpDwn_hour";
            this.nmUpDwn_hour.Size = new System.Drawing.Size(54, 33);
            this.nmUpDwn_hour.TabIndex = 19;
            this.nmUpDwn_hour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(217, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 28);
            this.label6.TabIndex = 20;
            this.label6.Text = ":";
            // 
            // nmUpDwn_minute
            // 
            this.nmUpDwn_minute.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_minute.Location = new System.Drawing.Point(238, 245);
            this.nmUpDwn_minute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nmUpDwn_minute.Name = "nmUpDwn_minute";
            this.nmUpDwn_minute.Size = new System.Drawing.Size(54, 33);
            this.nmUpDwn_minute.TabIndex = 21;
            this.nmUpDwn_minute.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btn_addExam
            // 
            this.btn_addExam.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_addExam.Location = new System.Drawing.Point(404, 198);
            this.btn_addExam.Name = "btn_addExam";
            this.btn_addExam.Size = new System.Drawing.Size(180, 30);
            this.btn_addExam.TabIndex = 22;
            this.btn_addExam.Text = "Add ->";
            this.btn_addExam.UseVisualStyleBackColor = true;
            this.btn_addExam.Click += new System.EventHandler(this.btn_addExam_Click);
            // 
            // btn_deleteExam
            // 
            this.btn_deleteExam.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_deleteExam.Location = new System.Drawing.Point(404, 409);
            this.btn_deleteExam.Name = "btn_deleteExam";
            this.btn_deleteExam.Size = new System.Drawing.Size(180, 30);
            this.btn_deleteExam.TabIndex = 23;
            this.btn_deleteExam.Text = "Delete <-";
            this.btn_deleteExam.UseVisualStyleBackColor = true;
            this.btn_deleteExam.Click += new System.EventHandler(this.btn_deleteExam_Click);
            // 
            // cmbBx_examType
            // 
            this.cmbBx_examType.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_examType.FormattingEnabled = true;
            this.cmbBx_examType.Location = new System.Drawing.Point(180, 18);
            this.cmbBx_examType.Name = "cmbBx_examType";
            this.cmbBx_examType.Size = new System.Drawing.Size(121, 34);
            this.cmbBx_examType.TabIndex = 24;
            // 
            // cmbBx_month
            // 
            this.cmbBx_month.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_month.FormattingEnabled = true;
            this.cmbBx_month.Location = new System.Drawing.Point(159, 152);
            this.cmbBx_month.Name = "cmbBx_month";
            this.cmbBx_month.Size = new System.Drawing.Size(121, 34);
            this.cmbBx_month.TabIndex = 25;
            // 
            // btn_return
            // 
            this.btn_return.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_return.Location = new System.Drawing.Point(12, 637);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(39, 32);
            this.btn_return.TabIndex = 26;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // cmbBx_month_regInit
            // 
            this.cmbBx_month_regInit.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_month_regInit.FormattingEnabled = true;
            this.cmbBx_month_regInit.Location = new System.Drawing.Point(159, 370);
            this.cmbBx_month_regInit.Name = "cmbBx_month_regInit";
            this.cmbBx_month_regInit.Size = new System.Drawing.Size(121, 34);
            this.cmbBx_month_regInit.TabIndex = 32;
            // 
            // nmUpDwn_day_regInit
            // 
            this.nmUpDwn_day_regInit.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_day_regInit.Location = new System.Drawing.Point(160, 415);
            this.nmUpDwn_day_regInit.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nmUpDwn_day_regInit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmUpDwn_day_regInit.Name = "nmUpDwn_day_regInit";
            this.nmUpDwn_day_regInit.Size = new System.Drawing.Size(120, 33);
            this.nmUpDwn_day_regInit.TabIndex = 31;
            this.nmUpDwn_day_regInit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_day_regInit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmUpDwn_year_regInit
            // 
            this.nmUpDwn_year_regInit.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_year_regInit.Location = new System.Drawing.Point(160, 326);
            this.nmUpDwn_year_regInit.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nmUpDwn_year_regInit.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nmUpDwn_year_regInit.Name = "nmUpDwn_year_regInit";
            this.nmUpDwn_year_regInit.Size = new System.Drawing.Size(120, 33);
            this.nmUpDwn_year_regInit.TabIndex = 30;
            this.nmUpDwn_year_regInit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_year_regInit.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label7.Location = new System.Drawing.Point(53, 417);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 28);
            this.label7.TabIndex = 29;
            this.label7.Text = "Day";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label8.Location = new System.Drawing.Point(53, 373);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 28);
            this.label8.TabIndex = 28;
            this.label8.Text = "Month";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label9.Location = new System.Drawing.Point(53, 328);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 28);
            this.label9.TabIndex = 27;
            this.label9.Text = "Year";
            // 
            // cmbBx_month_regEnd
            // 
            this.cmbBx_month_regEnd.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_month_regEnd.FormattingEnabled = true;
            this.cmbBx_month_regEnd.Location = new System.Drawing.Point(159, 548);
            this.cmbBx_month_regEnd.Name = "cmbBx_month_regEnd";
            this.cmbBx_month_regEnd.Size = new System.Drawing.Size(121, 34);
            this.cmbBx_month_regEnd.TabIndex = 38;
            // 
            // nmUpDwn_day_regEnd
            // 
            this.nmUpDwn_day_regEnd.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_day_regEnd.Location = new System.Drawing.Point(160, 593);
            this.nmUpDwn_day_regEnd.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nmUpDwn_day_regEnd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmUpDwn_day_regEnd.Name = "nmUpDwn_day_regEnd";
            this.nmUpDwn_day_regEnd.Size = new System.Drawing.Size(120, 33);
            this.nmUpDwn_day_regEnd.TabIndex = 37;
            this.nmUpDwn_day_regEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_day_regEnd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmUpDwn_year_regEnd
            // 
            this.nmUpDwn_year_regEnd.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_year_regEnd.Location = new System.Drawing.Point(160, 504);
            this.nmUpDwn_year_regEnd.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nmUpDwn_year_regEnd.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nmUpDwn_year_regEnd.Name = "nmUpDwn_year_regEnd";
            this.nmUpDwn_year_regEnd.Size = new System.Drawing.Size(120, 33);
            this.nmUpDwn_year_regEnd.TabIndex = 36;
            this.nmUpDwn_year_regEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_year_regEnd.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label10.Location = new System.Drawing.Point(53, 595);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 28);
            this.label10.TabIndex = 35;
            this.label10.Text = "Day";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label12.Location = new System.Drawing.Point(53, 551);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 28);
            this.label12.TabIndex = 34;
            this.label12.Text = "Month";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label13.Location = new System.Drawing.Point(53, 506);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 28);
            this.label13.TabIndex = 33;
            this.label13.Text = "Year";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label14.Location = new System.Drawing.Point(55, 295);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(224, 28);
            this.label14.TabIndex = 39;
            this.label14.Text = "Registration Init Date";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label15.Location = new System.Drawing.Point(56, 473);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(228, 28);
            this.label15.TabIndex = 40;
            this.label15.Text = "Registration End Date";
            // 
            // nmUpDwn_maxNumOfExaminees
            // 
            this.nmUpDwn_maxNumOfExaminees.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmUpDwn_maxNumOfExaminees.Location = new System.Drawing.Point(222, 61);
            this.nmUpDwn_maxNumOfExaminees.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nmUpDwn_maxNumOfExaminees.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmUpDwn_maxNumOfExaminees.Name = "nmUpDwn_maxNumOfExaminees";
            this.nmUpDwn_maxNumOfExaminees.Size = new System.Drawing.Size(58, 33);
            this.nmUpDwn_maxNumOfExaminees.TabIndex = 42;
            this.nmUpDwn_maxNumOfExaminees.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmUpDwn_maxNumOfExaminees.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(53, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(166, 28);
            this.label16.TabIndex = 41;
            this.label16.Text = "Max Examinees";
            // 
            // ExamScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.nmUpDwn_maxNumOfExaminees);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cmbBx_month_regEnd);
            this.Controls.Add(this.nmUpDwn_day_regEnd);
            this.Controls.Add(this.nmUpDwn_year_regEnd);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbBx_month_regInit);
            this.Controls.Add(this.nmUpDwn_day_regInit);
            this.Controls.Add(this.nmUpDwn_year_regInit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.cmbBx_month);
            this.Controls.Add(this.cmbBx_examType);
            this.Controls.Add(this.btn_deleteExam);
            this.Controls.Add(this.btn_addExam);
            this.Controls.Add(this.nmUpDwn_minute);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nmUpDwn_hour);
            this.Controls.Add(this.nmUpDwn_day);
            this.Controls.Add(this.nmUpDwn_year);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dgv_main);
            this.Name = "ExamScheduleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExamScheduleForm";
            this.Load += new System.EventHandler(this.ExamScheduleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_year)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_day)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_hour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_minute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_day_regInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_year_regInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_day_regEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_year_regEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmUpDwn_maxNumOfExaminees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nmUpDwn_year;
        private System.Windows.Forms.NumericUpDown nmUpDwn_day;
        private System.Windows.Forms.NumericUpDown nmUpDwn_hour;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nmUpDwn_minute;
        private System.Windows.Forms.Button btn_addExam;
        private System.Windows.Forms.Button btn_deleteExam;
        private System.Windows.Forms.ComboBox cmbBx_examType;
        private System.Windows.Forms.ComboBox cmbBx_month;
        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.ComboBox cmbBx_month_regInit;
        private System.Windows.Forms.NumericUpDown nmUpDwn_day_regInit;
        private System.Windows.Forms.NumericUpDown nmUpDwn_year_regInit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbBx_month_regEnd;
        private System.Windows.Forms.NumericUpDown nmUpDwn_day_regEnd;
        private System.Windows.Forms.NumericUpDown nmUpDwn_year_regEnd;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nmUpDwn_maxNumOfExaminees;
        private System.Windows.Forms.Label label16;
    }
}