using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    public partial class LoginForm_NonInstitutionalExam : Form
    {
        public LoginForm_NonInstitutionalExam()
        {
            InitializeComponent();
        }

        private void LoginForm_NonInstitutionalExam_Load(object sender, EventArgs e)
        {
            phTxtBx_username.ActivatePlaceHolderEvents();
            phTxtBx_password.ActivatePlaceHolderEvents();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            // Set the values that will be used by the singleton instance of DataContainer to initialize
            string username = phTxtBx_username.Text;
            string password = phTxtBx_password.Text;

            SharePointConnectionManagerPrerequisites.Username = username;
            SharePointConnectionManagerPrerequisites.Password = password;

            DataContainer container = DataContainer.Instance;
            if (!container.InitializeSharePointConnectionManager(false))
                MessageBox.Show("Wrong credentials!");
            else
            {
                container.SystemDataDictionary["LastLoginUser"] = username;
                container.SystemDataDictionary["Password"] = password;
                DocumentLoader.AddSystemDataToCSV(); // Updates the SystemData.csv file so that it includes the login info
                this.LogAndSwitchTo(new DataLoadingForm());
            }
        }
    }
}
