using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem
{
    public class Major
    {
        public Major(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }

        public int Id { get; }
        public string Name { get; }

        public override string ToString()
        {
            return "(" + Id.ToString() + ") " + Name; 
        }
    }
}
