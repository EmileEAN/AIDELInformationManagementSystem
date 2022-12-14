namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    partial class DataLoadingForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_loadingStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AIDEL_IMS_Registration.Properties.Resources.AIDELLogo;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(354, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_loadingStatus
            // 
            this.lbl_loadingStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbl_loadingStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_loadingStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_loadingStatus.Font = new System.Drawing.Font("Palatino Linotype", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_loadingStatus.Location = new System.Drawing.Point(0, 0);
            this.lbl_loadingStatus.Name = "lbl_loadingStatus";
            this.lbl_loadingStatus.Size = new System.Drawing.Size(1264, 681);
            this.lbl_loadingStatus.TabIndex = 2;
            this.lbl_loadingStatus.Text = "Loading status";
            this.lbl_loadingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataLoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbl_loadingStatus);
            this.Name = "DataLoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataLoadingForm";
            this.Load += new System.EventHandler(this.DataLoadingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_loadingStatus;
    }
}