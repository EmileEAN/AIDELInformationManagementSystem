using EEANWorks.WinForms;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_generator_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new BaseEvaluationsFileGenerator());
        }

        private void btn_splitter_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new EvaluationsFilesSplitter());
        }

        private void btn_addStudents_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new ExtraStudentsAdder());
        }

        private void btn_unifier_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new EvaluationsFilesUnifier());
        }
    }
}
