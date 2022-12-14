using EEANWorks.WinForms;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms;
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
    public partial class TitleForm : Form
    {
        public TitleForm()
        {
            InitializeComponent();
        }

        private void TitleForm_Shown(object sender, EventArgs e)
        {
            if (!DocumentLoader.ImportSystemDataDictionaryFromCSV())
                MessageBox.Show("Error initiating application!");

            DataContainer container = DataContainer.Instance;
            Dictionary<string, string> systemDataDictionary = container.SystemDataDictionary;
            if (!string.IsNullOrWhiteSpace(systemDataDictionary["LastLoginUser"]) && !string.IsNullOrWhiteSpace(systemDataDictionary["Password"]))
            {
                if (container.InitializeSharePointConnectionManager(true))
                {
                    this.LogAndSwitchTo(new DataLoadingForm());
                    return;
                }
            }

            this.LogAndSwitchTo(new LoginForm_NonInstitutionalExam());
        }
    }
}
