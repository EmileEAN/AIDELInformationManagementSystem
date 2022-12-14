using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports
{
    public static class CoreValues
    {
        public static decimal MaxQuantitativeEvaluationValue { get; } = 10;

        public static int AccountNumberLength { get; } = 6;
        public static string AccountNumberStringFormat { get; } = "D" + AccountNumberLength.ToString();
        public static int MinCourseIdLength { get; } = 3;
        public static int MaxCourseIdLength { get; } = 5;
        public static int PhoneNumberLength { get; } = 10;

        public static int PercentageDecimalPlaces { get; } = 2;
        public static string PercentageStringFormat { get; } = "P" + PercentageDecimalPlaces.ToString();

        public static string ComboBox_DefaultString = "--Select an item--";

        public static string LocalSaveFilesPath { get; } = (Path.GetDirectoryName(Application.ExecutablePath) + @"\");

        public static string EmailExtension { get; } = "@iberopuebla.mx";
    }
}
