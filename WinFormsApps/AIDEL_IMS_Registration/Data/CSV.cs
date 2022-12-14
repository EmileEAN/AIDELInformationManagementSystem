using EEANWorks;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data
{
    public static class CSV
    {
        public static bool DeleteRow(string _primaryKey, string _filePath)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!EEANWorks.CSV.DeleteRow(_primaryKey, _filePath))
                    return false;

                container.SetFileModificationStatus(_filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }
    }
}
