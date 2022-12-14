﻿namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class CourseStudentsForm
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
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtBx_filter_main = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_course = new System.Windows.Forms.Label();
            this.btn_return = new System.Windows.Forms.Button();
            this.lbl_faculty = new System.Windows.Forms.Label();
            this.btn_switchColumns = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_main
            // 
            this.dgv_main.AllowUserToAddRows = false;
            this.dgv_main.AllowUserToDeleteRows = false;
            this.dgv_main.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_main.Location = new System.Drawing.Point(65, 104);
            this.dgv_main.MultiSelect = false;
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.ReadOnly = true;
            this.dgv_main.RowHeadersWidth = 10;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_main.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_main.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_main.Size = new System.Drawing.Size(1151, 555);
            this.dgv_main.TabIndex = 9;
            this.dgv_main.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_main_CellContentClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AIDELInformationManagementSystem.Properties.Resources.SearchIcon;
            this.pictureBox1.Location = new System.Drawing.Point(1183, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // txtBx_filter_main
            // 
            this.txtBx_filter_main.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBx_filter_main.Location = new System.Drawing.Point(130, 56);
            this.txtBx_filter_main.Name = "txtBx_filter_main";
            this.txtBx_filter_main.Size = new System.Drawing.Size(1053, 33);
            this.txtBx_filter_main.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(60, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 28);
            this.label11.TabIndex = 6;
            this.label11.Text = "Filter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = "Course:";
            // 
            // lbl_course
            // 
            this.lbl_course.AutoSize = true;
            this.lbl_course.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_course.Location = new System.Drawing.Point(152, 13);
            this.lbl_course.Name = "lbl_course";
            this.lbl_course.Size = new System.Drawing.Size(274, 28);
            this.lbl_course.TabIndex = 11;
            this.lbl_course.Text = "ID Name Term Year Group";
            // 
            // btn_return
            // 
            this.btn_return.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_return.Location = new System.Drawing.Point(12, 637);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(39, 32);
            this.btn_return.TabIndex = 12;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // lbl_faculty
            // 
            this.lbl_faculty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_faculty.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_faculty.Location = new System.Drawing.Point(671, 13);
            this.lbl_faculty.Name = "lbl_faculty";
            this.lbl_faculty.Size = new System.Drawing.Size(545, 28);
            this.lbl_faculty.TabIndex = 14;
            this.lbl_faculty.Text = "Faculty: ID Full Name ";
            this.lbl_faculty.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btn_switchColumns
            // 
            this.btn_switchColumns.Font = new System.Drawing.Font("Palatino Linotype", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_switchColumns.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btn_switchColumns.Location = new System.Drawing.Point(1222, 57);
            this.btn_switchColumns.Name = "btn_switchColumns";
            this.btn_switchColumns.Size = new System.Drawing.Size(30, 32);
            this.btn_switchColumns.TabIndex = 15;
            this.btn_switchColumns.Text = "⇔";
            this.btn_switchColumns.UseVisualStyleBackColor = true;
            this.btn_switchColumns.Click += new System.EventHandler(this.btn_switchColumns_Click);
            // 
            // CourseStudentsForm
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.btn_switchColumns);
            this.Controls.Add(this.lbl_faculty);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.lbl_course);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_main);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtBx_filter_main);
            this.Controls.Add(this.label11);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "CourseStudentsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Course Students";
            this.Load += new System.EventHandler(this.CourseStudentsForm_Load);
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
        private System.Windows.Forms.Label lbl_course;
        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.Label lbl_faculty;
        private System.Windows.Forms.Button btn_switchColumns;
    }
}