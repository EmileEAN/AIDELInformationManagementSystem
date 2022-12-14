using EEANWorks.WinFormsApps.AIDEL_IMS_Reports.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TitleForm());
        }
    }
}
