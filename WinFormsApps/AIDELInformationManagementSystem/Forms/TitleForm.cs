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

            this.SwitchTo(new DataSelectionForm());
        }
    }
}
