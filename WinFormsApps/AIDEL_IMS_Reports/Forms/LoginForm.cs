using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Forms
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
            Dictionary<string, string> systemDataDictionary = container.SystemDataDictionary; 

            // Set the values that will be used by the singleton instance of DataContainer to initialize
            string username = phTxtBx_username.Text;
            string password = phTxtBx_password.Text;
            SharePointConnectionManagerPrerequisites.Username = phTxtBx_username.IsNullOrWhitespace() ? systemDataDictionary["LastLoginUser"] : username;
            SharePointConnectionManagerPrerequisites.Password = phTxtBx_password.IsNullOrWhitespace() ? systemDataDictionary["Password"] : password;

            if (!container.InitializeSharePointConnectionManager(false))
                MessageBox.Show("Wrong credentials!");
            else
            {
                systemDataDictionary["LastLoginUser"] = SharePointConnectionManagerPrerequisites.Username;
                systemDataDictionary["Password"] = SharePointConnectionManagerPrerequisites.Password;
                DocumentLoader.AddSystemDataToCSV(); // Updates the SystemData.csv file so that it includes the login info
                this.LogAndSwitchTo(new DataLoadingForm());
            }
        }
    }
}
