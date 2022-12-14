namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    partial class RegistrationInfoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.txtBx_filter_main = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.opnFlDlg_importingFile = new System.Windows.Forms.OpenFileDialog();
            this.btn_signOut = new System.Windows.Forms.Button();
            this.btn_addExam = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_openExcelFileFolder = new System.Windows.Forms.Button();
            this.chckBx_hideRepeated = new System.Windows.Forms.CheckBox();
            this.chckBx_hideCanceled = new System.Windows.Forms.CheckBox();
            this.chckBx_hideEndedExams = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_main
            // 
            this.dgv_main.AllowUserToAddRows = false;
            this.dgv_main.AllowUserToDeleteRows = false;
            this.dgv_main.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_main.Location = new System.Drawing.Point(65, 131);
            this.dgv_main.MultiSelect = false;
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.RowHeadersWidth = 10;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_main.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_main.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_main.Size = new System.Drawing.Size(1151, 538);
            this.dgv_main.TabIndex = 9;
            this.dgv_main.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_main_CellContentClick);
            this.dgv_main.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_main_CellValueChanged);
            // 
            // txtBx_filter_main
            // 
            this.txtBx_filter_main.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBx_filter_main.Location = new System.Drawing.Point(130, 61);
            this.txtBx_filter_main.Name = "txtBx_filter_main";
            this.txtBx_filter_main.Size = new System.Drawing.Size(1053, 33);
            this.txtBx_filter_main.TabIndex = 7;
            this.txtBx_filter_main.TextChanged += new System.EventHandler(this.txtBx_filter_main_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(60, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 28);
            this.label11.TabIndex = 6;
            this.label11.Text = "Filter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(418, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = "Non-Institutional Exam Registration Info";
            // 
            // opnFlDlg_importingFile
            // 
            this.opnFlDlg_importingFile.Filter = "Excel files|*.xls;*.xlsx;";
            // 
            // btn_signOut
            // 
            this.btn_signOut.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_signOut.Location = new System.Drawing.Point(1108, 19);
            this.btn_signOut.Name = "btn_signOut";
            this.btn_signOut.Size = new System.Drawing.Size(108, 30);
            this.btn_signOut.TabIndex = 12;
            this.btn_signOut.Text = "Sign out";
            this.btn_signOut.UseVisualStyleBackColor = true;
            this.btn_signOut.Click += new System.EventHandler(this.btn_signOut_Click);
            // 
            // btn_addExam
            // 
            this.btn_addExam.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_addExam.Location = new System.Drawing.Point(748, 19);
            this.btn_addExam.Name = "btn_addExam";
            this.btn_addExam.Size = new System.Drawing.Size(180, 30);
            this.btn_addExam.TabIndex = 13;
            this.btn_addExam.Text = "Add Exam";
            this.btn_addExam.UseVisualStyleBackColor = true;
            this.btn_addExam.Click += new System.EventHandler(this.btn_addExam_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AIDEL_IMS_Registration.Properties.Resources.SearchIcon;
            this.pictureBox1.Location = new System.Drawing.Point(1183, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btn_openExcelFileFolder
            // 
            this.btn_openExcelFileFolder.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_openExcelFileFolder.Location = new System.Drawing.Point(531, 19);
            this.btn_openExcelFileFolder.Name = "btn_openExcelFileFolder";
            this.btn_openExcelFileFolder.Size = new System.Drawing.Size(180, 30);
            this.btn_openExcelFileFolder.TabIndex = 14;
            this.btn_openExcelFileFolder.Text = "Open Excel File Folder";
            this.btn_openExcelFileFolder.UseVisualStyleBackColor = true;
            this.btn_openExcelFileFolder.Click += new System.EventHandler(this.btn_openExcelFileFolder_Click);
            // 
            // chckBx_hideRepeated
            // 
            this.chckBx_hideRepeated.AutoSize = true;
            this.chckBx_hideRepeated.Location = new System.Drawing.Point(234, 103);
            this.chckBx_hideRepeated.Name = "chckBx_hideRepeated";
            this.chckBx_hideRepeated.Size = new System.Drawing.Size(98, 17);
            this.chckBx_hideRepeated.TabIndex = 15;
            this.chckBx_hideRepeated.Text = "Hide Repeated";
            this.chckBx_hideRepeated.UseVisualStyleBackColor = true;
            this.chckBx_hideRepeated.CheckedChanged += new System.EventHandler(this.chckBx_hideRepeated_CheckedChanged);
            // 
            // chckBx_hideCanceled
            // 
            this.chckBx_hideCanceled.AutoSize = true;
            this.chckBx_hideCanceled.Location = new System.Drawing.Point(348, 103);
            this.chckBx_hideCanceled.Name = "chckBx_hideCanceled";
            this.chckBx_hideCanceled.Size = new System.Drawing.Size(96, 17);
            this.chckBx_hideCanceled.TabIndex = 16;
            this.chckBx_hideCanceled.Text = "Hide Canceled";
            this.chckBx_hideCanceled.UseVisualStyleBackColor = true;
            this.chckBx_hideCanceled.CheckedChanged += new System.EventHandler(this.chckBx_hideCanceled_CheckedChanged);
            // 
            // chckBx_hideEndedExams
            // 
            this.chckBx_hideEndedExams.AutoSize = true;
            this.chckBx_hideEndedExams.Location = new System.Drawing.Point(65, 103);
            this.chckBx_hideEndedExams.Name = "chckBx_hideEndedExams";
            this.chckBx_hideEndedExams.Size = new System.Drawing.Size(116, 17);
            this.chckBx_hideEndedExams.TabIndex = 17;
            this.chckBx_hideEndedExams.Text = "Hide Ended Exams";
            this.chckBx_hideEndedExams.UseVisualStyleBackColor = true;
            this.chckBx_hideEndedExams.CheckedChanged += new System.EventHandler(this.chckBx_hideEndedExams_CheckedChanged);
            // 
            // RegistrationInfoForm
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.chckBx_hideEndedExams);
            this.Controls.Add(this.chckBx_hideCanceled);
            this.Controls.Add(this.chckBx_hideRepeated);
            this.Controls.Add(this.btn_openExcelFileFolder);
            this.Controls.Add(this.btn_addExam);
            this.Controls.Add(this.btn_signOut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_main);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtBx_filter_main);
            this.Controls.Add(this.label11);
            this.Name = "RegistrationInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registration Info";
            this.Load += new System.EventHandler(this.RegistrationInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtBx_filter_main;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog opnFlDlg_importingFile;
        private System.Windows.Forms.Button btn_signOut;
        private System.Windows.Forms.Button btn_addExam;
        private System.Windows.Forms.Button btn_openExcelFileFolder;
        private System.Windows.Forms.CheckBox chckBx_hideRepeated;
        private System.Windows.Forms.CheckBox chckBx_hideCanceled;
        private System.Windows.Forms.CheckBox chckBx_hideEndedExams;
    }
}