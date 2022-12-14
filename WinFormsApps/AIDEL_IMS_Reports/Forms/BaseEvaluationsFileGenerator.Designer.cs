namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Forms
{
    partial class BaseEvaluationsFileGenerator
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
            this.btn_generate = new System.Windows.Forms.Button();
            this.btn_browseImportingFile = new System.Windows.Forms.Button();
            this.lbl_importingFileName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.opnFlDlg_importingFile = new System.Windows.Forms.OpenFileDialog();
            this.btn_templateFile = new System.Windows.Forms.Button();
            this.lbl_templateFileName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.opnFlDlg_templateFile = new System.Windows.Forms.OpenFileDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbBx_columnSet_general = new System.Windows.Forms.ComboBox();
            this.numUpDwn_year = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbBx_term = new System.Windows.Forms.ComboBox();
            this.cmbBx_columnSet_270 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBx_criteriaSet_270_quant = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbBx_criteriaSet_quant = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbBx_criteriaSet_qual = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_return = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_year)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_generate
            // 
            this.btn_generate.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_generate.Location = new System.Drawing.Point(439, 580);
            this.btn_generate.Name = "btn_generate";
            this.btn_generate.Size = new System.Drawing.Size(388, 41);
            this.btn_generate.TabIndex = 13;
            this.btn_generate.Text = "Generar Documentos de Evaluación";
            this.btn_generate.UseVisualStyleBackColor = true;
            this.btn_generate.Click += new System.EventHandler(this.btn_generate_Click);
            // 
            // btn_browseImportingFile
            // 
            this.btn_browseImportingFile.Location = new System.Drawing.Point(17, 508);
            this.btn_browseImportingFile.Name = "btn_browseImportingFile";
            this.btn_browseImportingFile.Size = new System.Drawing.Size(75, 23);
            this.btn_browseImportingFile.TabIndex = 12;
            this.btn_browseImportingFile.Text = "Seleccionar";
            this.btn_browseImportingFile.UseVisualStyleBackColor = true;
            this.btn_browseImportingFile.Click += new System.EventHandler(this.btn_browseImportingFile_Click);
            // 
            // lbl_importingFileName
            // 
            this.lbl_importingFileName.AutoSize = true;
            this.lbl_importingFileName.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_importingFileName.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.lbl_importingFileName.Location = new System.Drawing.Point(105, 505);
            this.lbl_importingFileName.Name = "lbl_importingFileName";
            this.lbl_importingFileName.Size = new System.Drawing.Size(198, 28);
            this.lbl_importingFileName.TabIndex = 11;
            this.lbl_importingFileName.Text = "Nombre de archivo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 477);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 28);
            this.label3.TabIndex = 10;
            this.label3.Text = "Lista de Asistencia:";
            // 
            // opnFlDlg_importingFile
            // 
            this.opnFlDlg_importingFile.FileName = "opnFlDlg_importingFile";
            // 
            // btn_templateFile
            // 
            this.btn_templateFile.Location = new System.Drawing.Point(17, 174);
            this.btn_templateFile.Name = "btn_templateFile";
            this.btn_templateFile.Size = new System.Drawing.Size(75, 23);
            this.btn_templateFile.TabIndex = 16;
            this.btn_templateFile.Text = "Seleccionar";
            this.btn_templateFile.UseVisualStyleBackColor = true;
            this.btn_templateFile.Click += new System.EventHandler(this.btn_templateFile_Click);
            // 
            // lbl_templateFileName
            // 
            this.lbl_templateFileName.AutoSize = true;
            this.lbl_templateFileName.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_templateFileName.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.lbl_templateFileName.Location = new System.Drawing.Point(105, 171);
            this.lbl_templateFileName.Name = "lbl_templateFileName";
            this.lbl_templateFileName.Size = new System.Drawing.Size(198, 28);
            this.lbl_templateFileName.TabIndex = 15;
            this.lbl_templateFileName.Text = "Nombre de archivo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 28);
            this.label4.TabIndex = 14;
            this.label4.Text = "Plantilla:";
            // 
            // opnFlDlg_templateFile
            // 
            this.opnFlDlg_templateFile.FileName = "opnFlDlg_templateFile";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(220, 28);
            this.label9.TabIndex = 50;
            this.label9.Text = "Formatos de plantilla:";
            // 
            // cmbBx_columnSet_general
            // 
            this.cmbBx_columnSet_general.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_columnSet_general.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_columnSet_general.FormattingEnabled = true;
            this.cmbBx_columnSet_general.Location = new System.Drawing.Point(242, 99);
            this.cmbBx_columnSet_general.Name = "cmbBx_columnSet_general";
            this.cmbBx_columnSet_general.Size = new System.Drawing.Size(466, 36);
            this.cmbBx_columnSet_general.TabIndex = 49;
            // 
            // numUpDwn_year
            // 
            this.numUpDwn_year.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUpDwn_year.Location = new System.Drawing.Point(295, 9);
            this.numUpDwn_year.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numUpDwn_year.Minimum = new decimal(new int[] {
            2018,
            0,
            0,
            0});
            this.numUpDwn_year.Name = "numUpDwn_year";
            this.numUpDwn_year.Size = new System.Drawing.Size(81, 36);
            this.numUpDwn_year.TabIndex = 48;
            this.numUpDwn_year.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numUpDwn_year.Value = new decimal(new int[] {
            2018,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(230, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 28);
            this.label8.TabIndex = 47;
            this.label8.Text = "Año:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 28);
            this.label7.TabIndex = 46;
            this.label7.Text = "Periodo:";
            // 
            // cmbBx_term
            // 
            this.cmbBx_term.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_term.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_term.FormattingEnabled = true;
            this.cmbBx_term.Location = new System.Drawing.Point(105, 9);
            this.cmbBx_term.Name = "cmbBx_term";
            this.cmbBx_term.Size = new System.Drawing.Size(107, 36);
            this.cmbBx_term.TabIndex = 45;
            // 
            // cmbBx_columnSet_270
            // 
            this.cmbBx_columnSet_270.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_columnSet_270.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_columnSet_270.FormattingEnabled = true;
            this.cmbBx_columnSet_270.Location = new System.Drawing.Point(787, 99);
            this.cmbBx_columnSet_270.Name = "cmbBx_columnSet_270";
            this.cmbBx_columnSet_270.Size = new System.Drawing.Size(466, 36);
            this.cmbBx_columnSet_270.TabIndex = 51;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(736, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 28);
            this.label2.TabIndex = 52;
            this.label2.Text = "270";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(736, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 28);
            this.label5.TabIndex = 56;
            this.label5.Text = "270";
            // 
            // cmbBx_criteriaSet_270_quant
            // 
            this.cmbBx_criteriaSet_270_quant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_criteriaSet_270_quant.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_criteriaSet_270_quant.FormattingEnabled = true;
            this.cmbBx_criteriaSet_270_quant.Location = new System.Drawing.Point(787, 296);
            this.cmbBx_criteriaSet_270_quant.Name = "cmbBx_criteriaSet_270_quant";
            this.cmbBx_criteriaSet_270_quant.Size = new System.Drawing.Size(466, 36);
            this.cmbBx_criteriaSet_270_quant.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(230, 28);
            this.label6.TabIndex = 54;
            this.label6.Text = "Criterios quantitativos:";
            // 
            // cmbBx_criteriaSet_quant
            // 
            this.cmbBx_criteriaSet_quant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_criteriaSet_quant.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_criteriaSet_quant.FormattingEnabled = true;
            this.cmbBx_criteriaSet_quant.Location = new System.Drawing.Point(242, 296);
            this.cmbBx_criteriaSet_quant.Name = "cmbBx_criteriaSet_quant";
            this.cmbBx_criteriaSet_quant.Size = new System.Drawing.Size(466, 36);
            this.cmbBx_criteriaSet_quant.TabIndex = 53;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 343);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(216, 28);
            this.label10.TabIndex = 57;
            this.label10.Text = "Criterios qualitativos:";
            // 
            // cmbBx_criteriaSet_qual
            // 
            this.cmbBx_criteriaSet_qual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBx_criteriaSet_qual.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBx_criteriaSet_qual.FormattingEnabled = true;
            this.cmbBx_criteriaSet_qual.Location = new System.Drawing.Point(242, 340);
            this.cmbBx_criteriaSet_qual.Name = "cmbBx_criteriaSet_qual";
            this.cmbBx_criteriaSet_qual.Size = new System.Drawing.Size(466, 36);
            this.cmbBx_criteriaSet_qual.TabIndex = 58;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(1, 247);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1270, 2);
            this.label11.TabIndex = 59;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.Location = new System.Drawing.Point(0, 425);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(1270, 2);
            this.label12.TabIndex = 60;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label13.Location = new System.Drawing.Point(0, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1270, 2);
            this.label13.TabIndex = 61;
            // 
            // btn_return
            // 
            this.btn_return.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_return.Location = new System.Drawing.Point(12, 575);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(47, 46);
            this.btn_return.TabIndex = 62;
            this.btn_return.Text = "⤶";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // BaseEvaluationsFileGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbBx_criteriaSet_qual);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbBx_criteriaSet_270_quant);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbBx_criteriaSet_quant);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBx_columnSet_270);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbBx_columnSet_general);
            this.Controls.Add(this.numUpDwn_year);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbBx_term);
            this.Controls.Add(this.btn_templateFile);
            this.Controls.Add(this.lbl_templateFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_generate);
            this.Controls.Add(this.btn_browseImportingFile);
            this.Controls.Add(this.lbl_importingFileName);
            this.Controls.Add(this.label3);
            this.Name = "BaseEvaluationsFileGenerator";
            this.Text = "BaseEvaluationsFileGenerator";
            this.Load += new System.EventHandler(this.BaseEvaluationsFileGenerator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_year)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_generate;
        private System.Windows.Forms.Button btn_browseImportingFile;
        private System.Windows.Forms.Label lbl_importingFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog opnFlDlg_importingFile;
        private System.Windows.Forms.Button btn_templateFile;
        private System.Windows.Forms.Label lbl_templateFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog opnFlDlg_templateFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbBx_columnSet_general;
        private System.Windows.Forms.NumericUpDown numUpDwn_year;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbBx_term;
        private System.Windows.Forms.ComboBox cmbBx_columnSet_270;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBx_criteriaSet_270_quant;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbBx_criteriaSet_quant;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbBx_criteriaSet_qual;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_return;
    }
}