namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Forms
{
    partial class MainForm
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
            this.btn_unifier = new System.Windows.Forms.Button();
            this.btn_generator = new System.Windows.Forms.Button();
            this.btn_splitter = new System.Windows.Forms.Button();
            this.btn_addStudents = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_unifier
            // 
            this.btn_unifier.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_unifier.Location = new System.Drawing.Point(31, 149);
            this.btn_unifier.Name = "btn_unifier";
            this.btn_unifier.Size = new System.Drawing.Size(327, 42);
            this.btn_unifier.TabIndex = 0;
            this.btn_unifier.Text = "Actualizar Evaluaciónes";
            this.btn_unifier.UseVisualStyleBackColor = true;
            this.btn_unifier.Click += new System.EventHandler(this.btn_unifier_Click);
            // 
            // btn_generator
            // 
            this.btn_generator.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_generator.Location = new System.Drawing.Point(31, 12);
            this.btn_generator.Name = "btn_generator";
            this.btn_generator.Size = new System.Drawing.Size(327, 42);
            this.btn_generator.TabIndex = 1;
            this.btn_generator.Text = "Generar Listado";
            this.btn_generator.UseVisualStyleBackColor = true;
            this.btn_generator.Click += new System.EventHandler(this.btn_generator_Click);
            // 
            // btn_splitter
            // 
            this.btn_splitter.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_splitter.Location = new System.Drawing.Point(31, 81);
            this.btn_splitter.Name = "btn_splitter";
            this.btn_splitter.Size = new System.Drawing.Size(327, 42);
            this.btn_splitter.TabIndex = 2;
            this.btn_splitter.Text = "Generar Archivos por Porfesor";
            this.btn_splitter.UseVisualStyleBackColor = true;
            this.btn_splitter.Click += new System.EventHandler(this.btn_splitter_Click);
            // 
            // btn_addStudents
            // 
            this.btn_addStudents.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_addStudents.Location = new System.Drawing.Point(438, 81);
            this.btn_addStudents.Name = "btn_addStudents";
            this.btn_addStudents.Size = new System.Drawing.Size(327, 42);
            this.btn_addStudents.TabIndex = 3;
            this.btn_addStudents.Text = "Añadir Alumnos";
            this.btn_addStudents.UseVisualStyleBackColor = true;
            this.btn_addStudents.Click += new System.EventHandler(this.btn_addStudents_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_addStudents);
            this.Controls.Add(this.btn_splitter);
            this.Controls.Add(this.btn_generator);
            this.Controls.Add(this.btn_unifier);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_unifier;
        private System.Windows.Forms.Button btn_generator;
        private System.Windows.Forms.Button btn_splitter;
        private System.Windows.Forms.Button btn_addStudents;
    }
}