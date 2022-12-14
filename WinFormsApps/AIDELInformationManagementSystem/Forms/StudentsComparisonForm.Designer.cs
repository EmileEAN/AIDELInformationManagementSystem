namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class StudentsComparisonForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_return = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.txtBx_filter = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_removeStudent = new System.Windows.Forms.Button();
            this.btn_addStudent = new System.Windows.Forms.Button();
            this.dgv_comparingStudents = new System.Windows.Forms.DataGridView();
            this.dgv_availableStudents = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_importExamData = new System.Windows.Forms.Button();
            this.opnFlDlg_importingFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_comparingStudents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_availableStudents)).BeginInit();
            this.SuspendLayout();
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
            // pictureBox3
            // 
            this.pictureBox3.Image = global::AIDELInformationManagementSystem.Properties.Resources.SearchIcon;
            this.pictureBox3.Location = new System.Drawing.Point(1054, 13);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(36, 36);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 65;
            this.pictureBox3.TabStop = false;
            // 
            // txtBx_filter
            // 
            this.txtBx_filter.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBx_filter.Location = new System.Drawing.Point(122, 13);
            this.txtBx_filter.Name = "txtBx_filter";
            this.txtBx_filter.Size = new System.Drawing.Size(932, 36);
            this.txtBx_filter.TabIndex = 64;
            this.txtBx_filter.TextChanged += new System.EventHandler(this.txtBx_filter_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(46, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 28);
            this.label14.TabIndex = 63;
            this.label14.Text = "Filter";
            // 
            // btn_removeStudent
            // 
            this.btn_removeStudent.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_removeStudent.Location = new System.Drawing.Point(756, 385);
            this.btn_removeStudent.Name = "btn_removeStudent";
            this.btn_removeStudent.Size = new System.Drawing.Size(95, 39);
            this.btn_removeStudent.TabIndex = 61;
            this.btn_removeStudent.Text = "Remove";
            this.btn_removeStudent.UseVisualStyleBackColor = true;
            this.btn_removeStudent.Click += new System.EventHandler(this.btn_removeStudent_Click);
            // 
            // btn_addStudent
            // 
            this.btn_addStudent.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_addStudent.Location = new System.Drawing.Point(421, 385);
            this.btn_addStudent.Name = "btn_addStudent";
            this.btn_addStudent.Size = new System.Drawing.Size(95, 39);
            this.btn_addStudent.TabIndex = 60;
            this.btn_addStudent.Text = "Add";
            this.btn_addStudent.UseVisualStyleBackColor = true;
            this.btn_addStudent.Click += new System.EventHandler(this.btn_addStudent_Click);
            // 
            // dgv_comparingStudents
            // 
            this.dgv_comparingStudents.AllowUserToAddRows = false;
            this.dgv_comparingStudents.AllowUserToDeleteRows = false;
            this.dgv_comparingStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_comparingStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_comparingStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_comparingStudents.Location = new System.Drawing.Point(51, 441);
            this.dgv_comparingStudents.MultiSelect = false;
            this.dgv_comparingStudents.Name = "dgv_comparingStudents";
            this.dgv_comparingStudents.ReadOnly = true;
            this.dgv_comparingStudents.RowHeadersWidth = 10;
            this.dgv_comparingStudents.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_comparingStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_comparingStudents.Size = new System.Drawing.Size(1165, 196);
            this.dgv_comparingStudents.TabIndex = 58;
            // 
            // dgv_availableStudents
            // 
            this.dgv_availableStudents.AllowUserToAddRows = false;
            this.dgv_availableStudents.AllowUserToDeleteRows = false;
            this.dgv_availableStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_availableStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_availableStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_availableStudents.Location = new System.Drawing.Point(51, 63);
            this.dgv_availableStudents.MultiSelect = false;
            this.dgv_availableStudents.Name = "dgv_availableStudents";
            this.dgv_availableStudents.ReadOnly = true;
            this.dgv_availableStudents.RowHeadersWidth = 10;
            this.dgv_availableStudents.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_availableStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_availableStudents.Size = new System.Drawing.Size(1165, 304);
            this.dgv_availableStudents.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(355, 367);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 73);
            this.label1.TabIndex = 66;
            this.label1.Text = "⇓";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(853, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 73);
            this.label2.TabIndex = 67;
            this.label2.Text = "⇑";
            // 
            // btn_importExamData
            // 
            this.btn_importExamData.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importExamData.Location = new System.Drawing.Point(1096, 11);
            this.btn_importExamData.Name = "btn_importExamData";
            this.btn_importExamData.Size = new System.Drawing.Size(120, 39);
            this.btn_importExamData.TabIndex = 68;
            this.btn_importExamData.Text = "Import";
            this.btn_importExamData.UseVisualStyleBackColor = true;
            this.btn_importExamData.Click += new System.EventHandler(this.btn_importExamData_Click);
            // 
            // opnFlDlg_importingFile
            // 
            this.opnFlDlg_importingFile.Filter = "Excel files|*.xls;*.xlsx;";
            // 
            // StudentsComparisonForm
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.btn_importExamData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.txtBx_filter);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btn_removeStudent);
            this.Controls.Add(this.btn_addStudent);
            this.Controls.Add(this.dgv_comparingStudents);
            this.Controls.Add(this.dgv_availableStudents);
            this.Controls.Add(this.btn_return);
            this.Name = "StudentsComparisonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Comparison";
            this.Load += new System.EventHandler(this.StudentsComparisonForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_comparingStudents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_availableStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox txtBx_filter;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_removeStudent;
        private System.Windows.Forms.Button btn_addStudent;
        private System.Windows.Forms.DataGridView dgv_comparingStudents;
        private System.Windows.Forms.DataGridView dgv_availableStudents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_importExamData;
        private System.Windows.Forms.OpenFileDialog opnFlDlg_importingFile;
    }
}