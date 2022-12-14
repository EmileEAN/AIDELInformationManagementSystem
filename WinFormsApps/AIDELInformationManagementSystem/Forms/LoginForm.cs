using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            phTxtBx_username.ActivatePlaceHolderEvents();
            phTxtBx_password.ActivatePlaceHolderEvents();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;
            List<Faculty> facultyList = container.FacultyList;
            Faculty facultyWithUsername = facultyList.FirstOrDefault(x => x.Username == phTxtBx_username.Text);
            if (facultyWithUsername == null)
                MessageBox.Show("There is no account associated to the username!");
            else if (facultyWithUsername.Password != phTxtBx_password.Text)
                MessageBox.Show("Wrong combination of username and password!");
            else
            {
                container.SystemDataDictionary["LastLoginUser"] = phTxtBx_username.Text;
                if (!DocumentLoader.AddSystemDataToCSV())
                    MessageBox.Show("Failed to update internal data.");

                container.CurrentUser = facultyWithUsername;

                this.LogAndSwitchTo(new CourseAndStudentInformationMainForm());
            }
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            this.SwitchToPrevious();
        }
    }
}
