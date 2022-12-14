using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem
{
    public static class StringExtension
    {
        public static FullName ToFullName(this string _string, bool _firstNameFirst)
        {
            string firstName = string.Empty;
            string middleName = string.Empty;
            string paternalSurname = string.Empty;
            string maternalSurname = string.Empty;

            string[] words = _string.Remove(".").MultipleSpacesToSingleSpace().Split(' ');
            List<string> names = words.ToNameBlocks();

            if (names.Count == 1)
                firstName = names[0];
            if (names.Count == 2)
            {
                firstName = _firstNameFirst ? names[0] : names[1];
                paternalSurname = _firstNameFirst ? names[1] : names[0];
            }
            else if (names.Count == 3)
            {
                firstName = _firstNameFirst ? names[0] : names[2];
                paternalSurname = _firstNameFirst ? names[1] : names[0];
                maternalSurname = _firstNameFirst ? names[2] : names[1];
            }
            else if (names.Count > 3)
            {
                int lastIndex = names.Count - 1;
                int penultimateIndex = lastIndex - 1;

                firstName = _firstNameFirst ? names[0] : names[penultimateIndex];
                middleName = _firstNameFirst ? names[1] : names[lastIndex];
                paternalSurname = _firstNameFirst ? names[2] : names[0];
                maternalSurname = _firstNameFirst ? names.Sum(3, lastIndex, " ") : names.Sum(1, penultimateIndex - 1, " ");
            }

            return new FullName(firstName, middleName, paternalSurname, maternalSurname);
        }
    }

    public static class StringArrayExtension
    {
        public static List<string> nameExceptions = new List<string>()
        {
            "del",
            "de",
            "la",
            "y"
        };

        public static List<string> ToNameBlocks(this string[] _stringArray)
        {
            List<string> nameBlocks = new List<string>();

            string name = string.Empty;
            foreach (string word in _stringArray)
            {
                name += ((name != String.Empty) ? " " : "") + word;
                if (!nameExceptions.Any(x => x.EqualsIgnoringCase(word)))
                {
                    nameBlocks.Add(name);
                    name = string.Empty;
                }
            }

            return nameBlocks;
        }
    }
}

