using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinForms
{
    public sealed class Log
    {
        public static Log Instance
        {
            get 
            {
                if (m_instance == null)
                    m_instance = new Log();
            
                return m_instance;
            }
        }

        private Log()
        {
            PreviousForms = new List<Form>();
        }

        private static Log m_instance;

        public List<Form> PreviousForms { get; set; }
    }
}
