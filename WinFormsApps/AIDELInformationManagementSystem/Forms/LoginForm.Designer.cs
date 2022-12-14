namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    partial class LoginForm
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
            this.components = new System.ComponentModel.Container();
            this.btn_login = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.phTxtBx_password = new EEANWorks.WinForms.UI.Components.PlaceHolderTextBox(this.components);
            this.btn_return = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.phTxtBx_username = new EEANWorks.WinForms.UI.Components.PlaceHolderTextBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.Location = new System.Drawing.Point(565, 457);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(164, 40);
            this.btn_login.TabIndex = 3;
            this.btn_login.Text = "Log In";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(364, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 55);
            this.label1.TabIndex = 4;
            this.label1.Text = "Course and Student Info";
            // 
            // phTxtBx_password
            // 
            this.phTxtBx_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phTxtBx_password.ForeColor = System.Drawing.Color.LightGray;
            this.phTxtBx_password.Location = new System.Drawing.Point(391, 361);
            this.phTxtBx_password.Name = "phTxtBx_password";
            this.phTxtBx_password.PlaceHolder = "Enter your password...";
            this.phTxtBx_password.Size = new System.Drawing.Size(500, 31);
            this.phTxtBx_password.TabIndex = 2;
            this.phTxtBx_password.UseSystemPasswordChar = true;
            // 
            // btn_return
            // 
            this.btn_return.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_return.Location = new System.Drawing.Point(13, 599);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(70, 70);
            this.btn_return.TabIndex = 14;
            this.btn_return.Text = "↩";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AIDELInformationManagementSystem.Properties.Resources.AIDELLogo;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(354, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // phTxtBx_username
            // 
            this.phTxtBx_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phTxtBx_username.ForeColor = System.Drawing.Color.LightGray;
            this.phTxtBx_username.Location = new System.Drawing.Point(391, 304);
            this.phTxtBx_username.Name = "phTxtBx_username";
            this.phTxtBx_username.PlaceHolder = "Enter your username...";
            this.phTxtBx_username.Size = new System.Drawing.Size(500, 31);
            this.phTxtBx_username.TabIndex = 1;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.phTxtBx_password);
            this.Controls.Add(this.phTxtBx_username);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.pictureBox1);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_login;
        private EEANWorks.WinForms.UI.Components.PlaceHolderTextBox phTxtBx_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_return;
        private EEANWorks.WinForms.UI.Components.PlaceHolderTextBox phTxtBx_username;
    }
}