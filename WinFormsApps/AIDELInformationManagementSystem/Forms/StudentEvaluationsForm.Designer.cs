namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class StudentEvaluationsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_student = new System.Windows.Forms.Label();
            this.lbl_course = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_return = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rdBtn_qualitative = new System.Windows.Forms.RadioButton();
            this.rdBtn_quantitative = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbBx_view_quantitative = new System.Windows.Forms.ComboBox();
            this.dgv_quantitative = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.pnl_quantitative = new System.Windows.Forms.Panel();
            this.pnl_qualitative = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv_qualitative = new System.Windows.Forms.DataGridView();
            this.cmbBx_view_qualitative = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_quantitative)).BeginInit();
            this.pnl_quantitative.SuspendLayout();
            this.pnl_qualitative.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_qualitative)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = "Student:";
            // 
            // lbl_student
            // 
            this.lbl_student.AutoSize = true;
            this.lbl_student.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_student.Location = new System.Drawing.Point(152, 13);
            this.lbl_student.Name = "lbl_student";
            this.lbl_student.Size = new System.Drawing.Size(242, 28);
            this.lbl_student.TabIndex = 11;
            this.lbl_student.Text = "AccountNumber Name";
            // 
            // lbl_course
            // 
            this.lbl_course.AutoSize = true;
            this.lbl_course.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_course.Location = new System.Drawing.Point(910, 13);
            this.lbl_course.Name = "lbl_course";
            this.lbl_course.Size = new System.Drawing.Size(274, 28);
            this.lbl_course.TabIndex = 13;
            this.lbl_course.Text = "ID Name Term Year Group";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(818, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 28);
            this.label3.TabIndex = 12;
            this.label3.Text = "Course:";
            // 
            // btn_return
            // 
            this.btn_return.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_return.Location = new System.Drawing.Point(12, 637);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(39, 32);
            this.btn_return.TabIndex = 16;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(60, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 28);
            this.label4.TabIndex = 17;
            this.label4.Text = "Evaluation Type:";
            // 
            // rdBtn_qualitative
            // 
            this.rdBtn_qualitative.AutoSize = true;
            this.rdBtn_qualitative.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtn_qualitative.Location = new System.Drawing.Point(167, 0);
            this.rdBtn_qualitative.Name = "rdBtn_qualitative";
            this.rdBtn_qualitative.Size = new System.Drawing.Size(137, 32);
            this.rdBtn_qualitative.TabIndex = 28;
            this.rdBtn_qualitative.TabStop = true;
            this.rdBtn_qualitative.Text = "Qualitative";
            this.rdBtn_qualitative.UseVisualStyleBackColor = true;
            this.rdBtn_qualitative.CheckedChanged += new System.EventHandler(this.rdBtn_qualitative_CheckedChanged);
            // 
            // rdBtn_quantitative
            // 
            this.rdBtn_quantitative.AutoSize = true;
            this.rdBtn_quantitative.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtn_quantitative.Location = new System.Drawing.Point(1, 0);
            this.rdBtn_quantitative.Name = "rdBtn_quantitative";
            this.rdBtn_quantitative.Size = new System.Drawing.Size(151, 32);
            this.rdBtn_quantitative.TabIndex = 27;
            this.rdBtn_quantitative.TabStop = true;
            this.rdBtn_quantitative.Text = "Quantitative";
            this.rdBtn_quantitative.UseVisualStyleBackColor = true;
            this.rdBtn_quantitative.CheckedChanged += new System.EventHandler(this.rdBtn_quantitative_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdBtn_qualitative);
            this.panel1.Controls.Add(this.rdBtn_quantitative);
            this.panel1.Location = new System.Drawing.Point(261, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(509, 30);
            this.panel1.TabIndex = 31;
            // 
            // cmbBx_view_quantitative
            // 
            this.cmbBx_view_quantitative.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_view_quantitative.FormattingEnabled = true;
            this.cmbBx_view_quantitative.Location = new System.Drawing.Point(77, 8);
            this.cmbBx_view_quantitative.Name = "cmbBx_view_quantitative";
            this.cmbBx_view_quantitative.Size = new System.Drawing.Size(311, 36);
            this.cmbBx_view_quantitative.TabIndex = 14;
            this.cmbBx_view_quantitative.SelectedIndexChanged += new System.EventHandler(this.cmbBx_view_quantitative_SelectedIndexChanged);
            // 
            // dgv_quantitative
            // 
            this.dgv_quantitative.AllowUserToAddRows = false;
            this.dgv_quantitative.AllowUserToDeleteRows = false;
            this.dgv_quantitative.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_quantitative.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_quantitative.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_quantitative.Location = new System.Drawing.Point(8, 52);
            this.dgv_quantitative.Name = "dgv_quantitative";
            this.dgv_quantitative.RowHeadersWidth = 10;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_quantitative.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_quantitative.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_quantitative.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_quantitative.Size = new System.Drawing.Size(1151, 509);
            this.dgv_quantitative.TabIndex = 9;
            this.dgv_quantitative.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_quantitative_CellValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 28);
            this.label2.TabIndex = 15;
            this.label2.Text = "View:";
            // 
            // pnl_quantitative
            // 
            this.pnl_quantitative.Controls.Add(this.label2);
            this.pnl_quantitative.Controls.Add(this.dgv_quantitative);
            this.pnl_quantitative.Controls.Add(this.cmbBx_view_quantitative);
            this.pnl_quantitative.Location = new System.Drawing.Point(57, 95);
            this.pnl_quantitative.Name = "pnl_quantitative";
            this.pnl_quantitative.Size = new System.Drawing.Size(1195, 586);
            this.pnl_quantitative.TabIndex = 32;
            // 
            // pnl_qualitative
            // 
            this.pnl_qualitative.Controls.Add(this.label5);
            this.pnl_qualitative.Controls.Add(this.dgv_qualitative);
            this.pnl_qualitative.Controls.Add(this.cmbBx_view_qualitative);
            this.pnl_qualitative.Location = new System.Drawing.Point(57, 95);
            this.pnl_qualitative.Name = "pnl_qualitative";
            this.pnl_qualitative.Size = new System.Drawing.Size(1195, 586);
            this.pnl_qualitative.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 28);
            this.label5.TabIndex = 15;
            this.label5.Text = "View:";
            // 
            // dgv_qualitative
            // 
            this.dgv_qualitative.AllowUserToAddRows = false;
            this.dgv_qualitative.AllowUserToDeleteRows = false;
            this.dgv_qualitative.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_qualitative.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgv_qualitative.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_qualitative.Location = new System.Drawing.Point(8, 52);
            this.dgv_qualitative.Name = "dgv_qualitative";
            this.dgv_qualitative.RowHeadersWidth = 10;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_qualitative.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgv_qualitative.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_qualitative.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_qualitative.Size = new System.Drawing.Size(1151, 509);
            this.dgv_qualitative.TabIndex = 9;
            this.dgv_qualitative.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_qualitative_CellValueChanged);
            // 
            // cmbBx_view_qualitative
            // 
            this.cmbBx_view_qualitative.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_view_qualitative.FormattingEnabled = true;
            this.cmbBx_view_qualitative.Location = new System.Drawing.Point(77, 8);
            this.cmbBx_view_qualitative.Name = "cmbBx_view_qualitative";
            this.cmbBx_view_qualitative.Size = new System.Drawing.Size(311, 36);
            this.cmbBx_view_qualitative.TabIndex = 14;
            this.cmbBx_view_qualitative.SelectedIndexChanged += new System.EventHandler(this.cmbBx_view_qualitative_SelectedIndexChanged);
            // 
            // StudentEvaluationsForm
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pnl_qualitative);
            this.Controls.Add(this.pnl_quantitative);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.lbl_course);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_student);
            this.Controls.Add(this.label1);
            this.Name = "StudentEvaluationsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Evaluations";
            this.Load += new System.EventHandler(this.StudentEvaluationsForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_quantitative)).EndInit();
            this.pnl_quantitative.ResumeLayout(false);
            this.pnl_quantitative.PerformLayout();
            this.pnl_qualitative.ResumeLayout(false);
            this.pnl_qualitative.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_qualitative)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_student;
        private System.Windows.Forms.Label lbl_course;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdBtn_qualitative;
        private System.Windows.Forms.RadioButton rdBtn_quantitative;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbBx_view_quantitative;
        private System.Windows.Forms.DataGridView dgv_quantitative;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnl_quantitative;
        private System.Windows.Forms.Panel pnl_qualitative;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv_qualitative;
        private System.Windows.Forms.ComboBox cmbBx_view_qualitative;
    }
}