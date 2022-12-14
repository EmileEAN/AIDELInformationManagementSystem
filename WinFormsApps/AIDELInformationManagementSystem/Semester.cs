using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem
{
    public class Semester
    {
        public Semester(int _year, eTerm _term)
        {
            Year = _year;
            Term = _term;
        }

        public int Year { get; }
        public eTerm Term { get; }

        public override string ToString() { return Term.ToString() + " " + Year.ToString(); }
    }
}
