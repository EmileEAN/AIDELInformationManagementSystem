namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class DataSelectionForm
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
            this.btn_continueWorkingLocally = new System.Windows.Forms.Button();
            this.btn_downloadNewestData = new System.Windows.Forms.Button();
            this.lbl_modificationCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_loadingStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_return = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(364, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 55);
            this.label1.TabIndex = 6;
            this.label1.Text = "Course and Student Info";
            // 
            // btn_continueWorkingLocally
            // 
            this.btn_continueWorkingLocally.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_continueWorkingLocally.Location = new System.Drawing.Point(812, 298);
            this.btn_continueWorkingLocally.Name = "btn_continueWorkingLocally";
            this.btn_continueWorkingLocally.Size = new System.Drawing.Size(240, 85);
            this.btn_continueWorkingLocally.TabIndex = 8;
            this.btn_continueWorkingLocally.Text = "Continue Working Locally";
            this.btn_continueWorkingLocally.UseVisualStyleBackColor = true;
            this.btn_continueWorkingLocally.Click += new System.EventHandler(this.btn_continueWorkingLocally_Click);
            // 
            // btn_downloadNewestData
            // 
            this.btn_downloadNewestData.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_downloadNewestData.Location = new System.Drawing.Point(212, 298);
            this.btn_downloadNewestData.Name = "btn_downloadNewestData";
            this.btn_downloadNewestData.Size = new System.Drawing.Size(240, 85);
            this.btn_downloadNewestData.TabIndex = 7;
            this.btn_downloadNewestData.Text = "Download Newest Data";
            this.btn_downloadNewestData.UseVisualStyleBackColor = true;
            this.btn_downloadNewestData.Click += new System.EventHandler(this.btn_downloadNewestData_Click);
            // 
            // lbl_modificationCount
            // 
            this.lbl_modificationCount.AutoSize = true;
            this.lbl_modificationCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_modificationCount.Location = new System.Drawing.Point(774, 425);
            this.lbl_modificationCount.Name = "lbl_modificationCount";
            this.lbl_modificationCount.Size = new System.Drawing.Size(24, 25);
            this.lbl_modificationCount.TabIndex = 9;
            this.lbl_modificationCount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(807, 425);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(311, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Non-applied local modifications";
            // 
            // lbl_loadingStatus
            // 
            this.lbl_loadingStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_loadingStatus.AutoSize = true;
            this.lbl_loadingStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_loadingStatus.Location = new System.Drawing.Point(369, 565);
            this.lbl_loadingStatus.Name = "lbl_loadingStatus";
            this.lbl_loadingStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_loadingStatus.Size = new System.Drawing.Size(0, 25);
            this.lbl_loadingStatus.TabIndex = 11;
            this.lbl_loadingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(207, 565);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(162, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Loading Status:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AIDELInformationManagementSystem.Properties.Resources.AIDELLogo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(354, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btn_return
            // 
            this.btn_return.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_return.Location = new System.Drawing.Point(13, 598);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(70, 70);
            this.btn_return.TabIndex = 13;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // DataSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_loadingStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_modificationCount);
            this.Controls.Add(this.btn_continueWorkingLocally);
            this.Controls.Add(this.btn_downloadNewestData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "DataSelectionForm";
            this.Text = "Data Selection";
            this.Load += new System.EventHandler(this.DataSelectionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_continueWorkingLocally;
        private System.Windows.Forms.Button btn_downloadNewestData;
        private System.Windows.Forms.Label lbl_modificationCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_loadingStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_return;
    }
}