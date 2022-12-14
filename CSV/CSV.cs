using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks
{
    public static class CSV
    {
        public static bool DeleteRow(string _primaryKey, string _filePath)
        {
            try
            {
                // Modify target row in CSV
                {
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(_filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (values[0] != _primaryKey) // If it is not the row to be deleted
                                lines.Add(line);
                        }
                    }

                    using (var sw = new StreamWriter(_filePath))
                    {
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

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
