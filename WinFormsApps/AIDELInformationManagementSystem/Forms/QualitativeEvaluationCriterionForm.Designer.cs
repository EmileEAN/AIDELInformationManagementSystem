namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class QualitativeEvaluationCriterionForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbCtrl_main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtBx_filter_main = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmbBx_evaluationColorSet_add = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_valueColors_add = new System.Windows.Forms.DataGridView();
            this.btn_add = new System.Windows.Forms.Button();
            this.txtBx_string_add = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmbBx_evaluationColorSet_edit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgv_valueColors_edit = new System.Windows.Forms.DataGridView();
            this.txtBx_string_edit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_edit = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cmbBx_evaluationColorSet_delete = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv_valueColors_delete = new System.Windows.Forms.DataGridView();
            this.txtBx_string_delete = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_return = new System.Windows.Forms.Button();
            this.tbCtrl_main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_valueColors_add)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_valueColors_edit)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_valueColors_delete)).BeginInit();
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
            this.tabPage1.Controls.Add(this.txtBx_filter_main);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1256, 639);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "View/Ver";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.dgv_main.Location = new System.Drawing.Point(51, 62);
            this.dgv_main.MultiSelect = false;
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.ReadOnly = true;
            this.dgv_main.RowHeadersWidth = 10;
            this.dgv_main.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_main.Size = new System.Drawing.Size(1151, 538);
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
            // txtBx_filter_main
            // 
            this.txtBx_filter_main.Location = new System.Drawing.Point(116, 12);
            this.txtBx_filter_main.Name = "txtBx_filter_main";
            this.txtBx_filter_main.Size = new System.Drawing.Size(1053, 33);
            this.txtBx_filter_main.TabIndex = 3;
            this.txtBx_filter_main.TextChanged += new System.EventHandler(this.txtBx_filter_main_TextChanged);
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
            this.tabPage2.Controls.Add(this.cmbBx_evaluationColorSet_add);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dgv_valueColors_add);
            this.tabPage2.Controls.Add(this.btn_add);
            this.tabPage2.Controls.Add(this.txtBx_string_add);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1256, 639);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Add/Añadir";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmbBx_evaluationColorSet_add
            // 
            this.cmbBx_evaluationColorSet_add.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_evaluationColorSet_add.FormattingEnabled = true;
            this.cmbBx_evaluationColorSet_add.Location = new System.Drawing.Point(272, 66);
            this.cmbBx_evaluationColorSet_add.Name = "cmbBx_evaluationColorSet_add";
            this.cmbBx_evaluationColorSet_add.Size = new System.Drawing.Size(396, 34);
            this.cmbBx_evaluationColorSet_add.TabIndex = 25;
            this.cmbBx_evaluationColorSet_add.SelectedIndexChanged += new System.EventHandler(this.cmbBx_evaluationColorSet_add_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 28);
            this.label1.TabIndex = 24;
            this.label1.Text = "Evaluation Color Set:";
            // 
            // dgv_valueColors_add
            // 
            this.dgv_valueColors_add.AllowUserToAddRows = false;
            this.dgv_valueColors_add.AllowUserToDeleteRows = false;
            this.dgv_valueColors_add.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_valueColors_add.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_valueColors_add.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_valueColors_add.Location = new System.Drawing.Point(55, 118);
            this.dgv_valueColors_add.MultiSelect = false;
            this.dgv_valueColors_add.Name = "dgv_valueColors_add";
            this.dgv_valueColors_add.ReadOnly = true;
            this.dgv_valueColors_add.RowHeadersWidth = 10;
            this.dgv_valueColors_add.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_valueColors_add.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_valueColors_add.Size = new System.Drawing.Size(433, 366);
            this.dgv_valueColors_add.TabIndex = 23;
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
            // txtBx_string_add
            // 
            this.txtBx_string_add.Location = new System.Drawing.Point(159, 14);
            this.txtBx_string_add.Name = "txtBx_string_add";
            this.txtBx_string_add.Size = new System.Drawing.Size(1042, 33);
            this.txtBx_string_add.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Criterion:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmbBx_evaluationColorSet_edit);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.dgv_valueColors_edit);
            this.tabPage3.Controls.Add(this.txtBx_string_edit);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.btn_edit);
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1256, 639);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Edit/Editar";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmbBx_evaluationColorSet_edit
            // 
            this.cmbBx_evaluationColorSet_edit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_evaluationColorSet_edit.FormattingEnabled = true;
            this.cmbBx_evaluationColorSet_edit.Location = new System.Drawing.Point(272, 66);
            this.cmbBx_evaluationColorSet_edit.Name = "cmbBx_evaluationColorSet_edit";
            this.cmbBx_evaluationColorSet_edit.Size = new System.Drawing.Size(396, 34);
            this.cmbBx_evaluationColorSet_edit.TabIndex = 48;
            this.cmbBx_evaluationColorSet_edit.SelectedIndexChanged += new System.EventHandler(this.cmbBx_evaluationColorSet_edit_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(47, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 28);
            this.label3.TabIndex = 47;
            this.label3.Text = "Evaluation Color Set:";
            // 
            // dgv_valueColors_edit
            // 
            this.dgv_valueColors_edit.AllowUserToAddRows = false;
            this.dgv_valueColors_edit.AllowUserToDeleteRows = false;
            this.dgv_valueColors_edit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_valueColors_edit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_valueColors_edit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_valueColors_edit.Location = new System.Drawing.Point(55, 118);
            this.dgv_valueColors_edit.MultiSelect = false;
            this.dgv_valueColors_edit.Name = "dgv_valueColors_edit";
            this.dgv_valueColors_edit.ReadOnly = true;
            this.dgv_valueColors_edit.RowHeadersWidth = 10;
            this.dgv_valueColors_edit.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_valueColors_edit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_valueColors_edit.Size = new System.Drawing.Size(433, 366);
            this.dgv_valueColors_edit.TabIndex = 46;
            // 
            // txtBx_string_edit
            // 
            this.txtBx_string_edit.Location = new System.Drawing.Point(159, 14);
            this.txtBx_string_edit.Name = "txtBx_string_edit";
            this.txtBx_string_edit.Size = new System.Drawing.Size(1042, 33);
            this.txtBx_string_edit.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(47, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 28);
            this.label4.TabIndex = 44;
            this.label4.Text = "Criterion:";
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.cmbBx_evaluationColorSet_delete);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.dgv_valueColors_delete);
            this.tabPage4.Controls.Add(this.txtBx_string_delete);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.btn_delete);
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1256, 639);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "Delete/Eliminar";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // cmbBx_evaluationColorSet_delete
            // 
            this.cmbBx_evaluationColorSet_delete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_evaluationColorSet_delete.Enabled = false;
            this.cmbBx_evaluationColorSet_delete.FormattingEnabled = true;
            this.cmbBx_evaluationColorSet_delete.Location = new System.Drawing.Point(272, 66);
            this.cmbBx_evaluationColorSet_delete.Name = "cmbBx_evaluationColorSet_delete";
            this.cmbBx_evaluationColorSet_delete.Size = new System.Drawing.Size(396, 34);
            this.cmbBx_evaluationColorSet_delete.TabIndex = 67;
            this.cmbBx_evaluationColorSet_delete.SelectedIndexChanged += new System.EventHandler(this.cmbBx_evaluationColorSet_delete_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(219, 28);
            this.label5.TabIndex = 66;
            this.label5.Text = "Evaluation Color Set:";
            // 
            // dgv_valueColors_delete
            // 
            this.dgv_valueColors_delete.AllowUserToAddRows = false;
            this.dgv_valueColors_delete.AllowUserToDeleteRows = false;
            this.dgv_valueColors_delete.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_valueColors_delete.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_valueColors_delete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_valueColors_delete.Enabled = false;
            this.dgv_valueColors_delete.Location = new System.Drawing.Point(55, 118);
            this.dgv_valueColors_delete.MultiSelect = false;
            this.dgv_valueColors_delete.Name = "dgv_valueColors_delete";
            this.dgv_valueColors_delete.ReadOnly = true;
            this.dgv_valueColors_delete.RowHeadersWidth = 10;
            this.dgv_valueColors_delete.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_valueColors_delete.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_valueColors_delete.Size = new System.Drawing.Size(433, 366);
            this.dgv_valueColors_delete.TabIndex = 65;
            // 
            // txtBx_string_delete
            // 
            this.txtBx_string_delete.Enabled = false;
            this.txtBx_string_delete.Location = new System.Drawing.Point(159, 14);
            this.txtBx_string_delete.Name = "txtBx_string_delete";
            this.txtBx_string_delete.Size = new System.Drawing.Size(1042, 33);
            this.txtBx_string_delete.TabIndex = 64;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(47, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 28);
            this.label6.TabIndex = 63;
            this.label6.Text = "Criterion:";
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
            // btn_return
            // 
            this.btn_return.Location = new System.Drawing.Point(6, 601);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(39, 32);
            this.btn_return.TabIndex = 7;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // QualitativeEvaluationCriterionForm
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.tbCtrl_main);
            this.Name = "QualitativeEvaluationCriterionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Qualitative Evaluation Criterion";
            this.Load += new System.EventHandler(this.QualitativeEvaluationCriterionForm_Load);
            this.tbCtrl_main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_valueColors_add)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_valueColors_edit)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_valueColors_delete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbCtrl_main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtBx_string_add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.TextBox txtBx_filter_main;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.DataGridView dgv_valueColors_add;
        private System.Windows.Forms.ComboBox cmbBx_evaluationColorSet_add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBx_evaluationColorSet_edit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgv_valueColors_edit;
        private System.Windows.Forms.TextBox txtBx_string_edit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBx_evaluationColorSet_delete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv_valueColors_delete;
        private System.Windows.Forms.TextBox txtBx_string_delete;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_return;
    }
}