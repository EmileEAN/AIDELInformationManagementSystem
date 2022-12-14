namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class CourseBaseForm
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
            this.tbCtrl_main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_return = new System.Windows.Forms.Button();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtBx_filter = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_add = new System.Windows.Forms.Button();
            this.txtBx_name_add = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBx_id_add = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_edit = new System.Windows.Forms.Button();
            this.txtBx_name_edit = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtBx_id_edit = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btn_delete = new System.Windows.Forms.Button();
            this.txtBx_name_delete = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.txtBx_id_delete = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.cmbBox_category_add = new System.Windows.Forms.ComboBox();
            this.cmbBox_category_edit = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBox_category_delete = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCtrl_main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbCtrl_main
            // 
            this.tbCtrl_main.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tbCtrl_main.Controls.Add(this.tabPage1);
            this.tbCtrl_main.Controls.Add(this.tabPage2);
            this.tbCtrl_main.Controls.Add(this.tabPage3);
            this.tbCtrl_main.Controls.Add(this.tabPage4);
            this.tbCtrl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtrl_main.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tbCtrl_main.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCtrl_main.Location = new System.Drawing.Point(0, 0);
            this.tbCtrl_main.Name = "tbCtrl_main";
            this.tbCtrl_main.SelectedIndex = 0;
            this.tbCtrl_main.Size = new System.Drawing.Size(1264, 681);
            this.tbCtrl_main.TabIndex = 0;
            this.tbCtrl_main.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tbCtrl_main_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_return);
            this.tabPage1.Controls.Add(this.dgv_main);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.txtBx_filter);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1256, 639);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "View/Ver";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_return
            // 
            this.btn_return.Location = new System.Drawing.Point(6, 601);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(39, 32);
            this.btn_return.TabIndex = 6;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
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
            this.dgv_main.Location = new System.Drawing.Point(51, 62);
            this.dgv_main.MultiSelect = false;
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.ReadOnly = true;
            this.dgv_main.RowHeadersWidth = 10;
            this.dgv_main.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_main.Size = new System.Drawing.Size(1150, 538);
            this.dgv_main.TabIndex = 5;
            this.dgv_main.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_main_CellClick);
            this.dgv_main.SelectionChanged += new System.EventHandler(this.dgv_main_SelectionChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AIDELInformationManagementSystem.Properties.Resources.SearchIcon;
            this.pictureBox1.Location = new System.Drawing.Point(1169, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // txtBx_filter
            // 
            this.txtBx_filter.Location = new System.Drawing.Point(116, 12);
            this.txtBx_filter.Name = "txtBx_filter";
            this.txtBx_filter.Size = new System.Drawing.Size(1053, 33);
            this.txtBx_filter.TabIndex = 3;
            this.txtBx_filter.TextChanged += new System.EventHandler(this.txtBx_filter_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(46, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 28);
            this.label11.TabIndex = 2;
            this.label11.Text = "Filter";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cmbBox_category_add);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.btn_add);
            this.tabPage2.Controls.Add(this.txtBx_name_add);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtBx_id_add);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1256, 639);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Add/Añadir";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 28);
            this.label3.TabIndex = 19;
            this.label3.Text = "Category:";
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(565, 539);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(151, 39);
            this.btn_add.TabIndex = 18;
            this.btn_add.Text = "Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // txtBx_name_add
            // 
            this.txtBx_name_add.Location = new System.Drawing.Point(222, 56);
            this.txtBx_name_add.Name = "txtBx_name_add";
            this.txtBx_name_add.Size = new System.Drawing.Size(205, 33);
            this.txtBx_name_add.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Course Name:";
            // 
            // txtBx_id_add
            // 
            this.txtBx_id_add.Location = new System.Drawing.Point(222, 12);
            this.txtBx_id_add.Name = "txtBx_id_add";
            this.txtBx_id_add.Size = new System.Drawing.Size(125, 33);
            this.txtBx_id_add.TabIndex = 1;
            this.txtBx_id_add.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmbBox_category_edit);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.btn_edit);
            this.tabPage3.Controls.Add(this.txtBx_name_edit);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.txtBx_id_edit);
            this.tabPage3.Controls.Add(this.label21);
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1256, 639);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Edit/Editar";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_edit
            // 
            this.btn_edit.Location = new System.Drawing.Point(565, 539);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(151, 39);
            this.btn_edit.TabIndex = 39;
            this.btn_edit.Text = "Edit";
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // txtBx_name_edit
            // 
            this.txtBx_name_edit.Location = new System.Drawing.Point(222, 56);
            this.txtBx_name_edit.Name = "txtBx_name_edit";
            this.txtBx_name_edit.Size = new System.Drawing.Size(205, 33);
            this.txtBx_name_edit.TabIndex = 24;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(8, 57);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(150, 28);
            this.label20.TabIndex = 23;
            this.label20.Text = "Course Name:";
            // 
            // txtBx_id_edit
            // 
            this.txtBx_id_edit.Location = new System.Drawing.Point(222, 12);
            this.txtBx_id_edit.Name = "txtBx_id_edit";
            this.txtBx_id_edit.Size = new System.Drawing.Size(125, 33);
            this.txtBx_id_edit.TabIndex = 22;
            this.txtBx_id_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(8, 13);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(42, 28);
            this.label21.TabIndex = 21;
            this.label21.Text = "ID:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.cmbBox_category_delete);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.btn_delete);
            this.tabPage4.Controls.Add(this.txtBx_name_delete);
            this.tabPage4.Controls.Add(this.label30);
            this.tabPage4.Controls.Add(this.txtBx_id_delete);
            this.tabPage4.Controls.Add(this.label31);
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1256, 639);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "Delete/Eliminar";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(565, 539);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(151, 39);
            this.btn_delete.TabIndex = 60;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // txtBx_name_delete
            // 
            this.txtBx_name_delete.Enabled = false;
            this.txtBx_name_delete.Location = new System.Drawing.Point(222, 56);
            this.txtBx_name_delete.Name = "txtBx_name_delete";
            this.txtBx_name_delete.Size = new System.Drawing.Size(205, 33);
            this.txtBx_name_delete.TabIndex = 45;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(8, 57);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(150, 28);
            this.label30.TabIndex = 44;
            this.label30.Text = "Course Name:";
            // 
            // txtBx_id_delete
            // 
            this.txtBx_id_delete.Enabled = false;
            this.txtBx_id_delete.Location = new System.Drawing.Point(222, 12);
            this.txtBx_id_delete.Name = "txtBx_id_delete";
            this.txtBx_id_delete.Size = new System.Drawing.Size(125, 33);
            this.txtBx_id_delete.TabIndex = 43;
            this.txtBx_id_delete.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(8, 13);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(42, 28);
            this.label31.TabIndex = 42;
            this.label31.Text = "ID:";
            // 
            // cmbBox_category_add
            // 
            this.cmbBox_category_add.FormattingEnabled = true;
            this.cmbBox_category_add.Location = new System.Drawing.Point(222, 104);
            this.cmbBox_category_add.Name = "cmbBox_category_add";
            this.cmbBox_category_add.Size = new System.Drawing.Size(205, 34);
            this.cmbBox_category_add.TabIndex = 20;
            // 
            // cmbBox_category_edit
            // 
            this.cmbBox_category_edit.FormattingEnabled = true;
            this.cmbBox_category_edit.Location = new System.Drawing.Point(222, 104);
            this.cmbBox_category_edit.Name = "cmbBox_category_edit";
            this.cmbBox_category_edit.Size = new System.Drawing.Size(205, 34);
            this.cmbBox_category_edit.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 28);
            this.label4.TabIndex = 40;
            this.label4.Text = "Category:";
            // 
            // cmbBox_category_delete
            // 
            this.cmbBox_category_delete.FormattingEnabled = true;
            this.cmbBox_category_delete.Location = new System.Drawing.Point(222, 104);
            this.cmbBox_category_delete.Name = "cmbBox_category_delete";
            this.cmbBox_category_delete.Size = new System.Drawing.Size(205, 34);
            this.cmbBox_category_delete.TabIndex = 62;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 28);
            this.label5.TabIndex = 61;
            this.label5.Text = "Category:";
            // 
            // CourseBaseForm
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.tbCtrl_main);
            this.Name = "CourseBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Course Base";
            this.Load += new System.EventHandler(this.CourseBaseForm_Load);
            this.tbCtrl_main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbCtrl_main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBx_name_add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBx_id_add;
        private System.Windows.Forms.TextBox txtBx_filter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.TextBox txtBx_name_edit;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtBx_id_edit;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.TextBox txtBx_name_delete;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtBx_id_delete;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBox_category_add;
        private System.Windows.Forms.ComboBox cmbBox_category_edit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBox_category_delete;
        private System.Windows.Forms.Label label5;
    }
}