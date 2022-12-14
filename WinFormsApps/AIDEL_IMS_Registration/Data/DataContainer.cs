using AIDEL_IMS_Registration.Properties;
using EEANWorks.Google;
using EEANWorks.Microsoft;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data
{
    public class SharePointConnectionManagerPrerequisites
    {
        public static string Username = "";
        public static string Password = "";
    }

    public sealed class DataContainer
    {
        public static DataContainer Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new DataContainer();

                return m_instance;
            }
        }

        private DataContainer()
        {
            SharePointConnectionManager = null;
            GoogleDriveConnectionManager = null;

            FileNames = new List<string>()
            {
                "ExamType.csv",
                "FullName.csv",
                "Name.csv",
                "NonIberoCommunityMember.csv",
                "NonInstitutionalExam_Internal.csv",
                "RegistrationInfo_Internal.csv",
                "RegistrationInfo_External.csv",
                "RegistrationStatus.csv",
                "Semester.csv",
                "Student.csv",
                "Surname.csv",
                "Term.csv",
                "Year.csv"
            };

            FilePath_SystemData = CoreValues.LocalSaveFilesPath + @"Data/SystemData.csv";

            SetFilePaths();

            SystemDataDictionary = new Dictionary<string, string>();

            RegistrationInfos_Internal = new List<RegistrationInfo_Internal>();
            RegistrationInfos_External = new List<RegistrationInfo_External>();
            RegistrationStatusDictionary = new Dictionary<int, eRegistrationStatus>();

            NameDictionary = new Dictionary<int, string>();
            SurnameDictionary = new Dictionary<int, string>();
            FullNameDictionary = new Dictionary<int, FullName>();

            YearDictionary = new Dictionary<int, int>();
            TermDictionary = new Dictionary<int, eTerm>();
            SemesterDictionary = new Dictionary<int, Semester>();

            StudentList = new List<Student>();
            NonIberoCommunityMemberList = new List<NonIberoCommunityMember>();

            ExamTypeDictionary = new Dictionary<int, eExamType>();
            NonInstitutionalExam_InternalDictionary = new Dictionary<int, NonInstitutionalExam_Internal>();

            InlineImage1_2 = Resources.inlineImage1_2.ToByteArray(ImageFormat.Bmp);
            InlineImage1_3 = Resources.inlineImage1_3.ToByteArray(ImageFormat.Bmp);
            InlineImage1_4 = Resources.inlineImage1_4.ToByteArray(ImageFormat.Bmp);
        }

        private static DataContainer m_instance = null;

        public CustomSharePointConnectionManager SharePointConnectionManager { get; private set; }
        public GoogleDriveConnectionManager GoogleDriveConnectionManager { get; private set; }

        #region CSV File Name
        public List<string> FileNames { get; }
        #endregion

        #region CSV File Path
        public string FilePath_SystemData { get; }

        public string FilePath_RegistrationInfo_Internal { get; set; }
        public string FilePath_RegistrationInfo_External { get; set; }
        public string FilePath_RegistrationStatus { get; set; }

        public string FilePath_Name { get; set; }
        public string FilePath_Surname { get; set; }
        public string FilePath_FullName { get; set; }
        public string FilePath_Student { get; set; }
        public string FilePath_NonIberoCommunityMember { get; set; }

        public string FilePath_Year { get; set; }
        public string FilePath_Term { get; set; }
        public string FilePath_Semester { get; set; }

        public string FilePath_ExamType { get; set; }
        public string FilePath_NonInstitutionalExam_Internal { get; set; }
        #endregion

        public bool InitializeSharePointConnectionManager(bool _useSavedCredentials)
        {
            SharePointConnectionManager = null;
            GoogleDriveConnectionManager = null;

            string username = (_useSavedCredentials) ? SystemDataDictionary["LastLoginUser"] : SharePointConnectionManagerPrerequisites.Username;
            string password = (_useSavedCredentials) ? SystemDataDictionary["Password"] : SharePointConnectionManagerPrerequisites.Password;

            CustomSharePointConnectionManager tmp = new CustomSharePointConnectionManager("https://iberopuebla.sharepoint.com/sites/CentroInterculturaldeLenguas",
                                                "Documentos",
                                                "App Data",
                                                username + CoreValues.EmailExtension,
                                                password,
                                                CoreValues.LocalSaveFilesPath + @"Data\");

            GoogleDriveConnectionManager = new GoogleDriveConnectionManager("Exam Reg", "1nXRZoIgq8irvl2939VYmUqp-6NZplxU1", CoreValues.LocalSaveFilesPath + @"Data\External\");

            if (tmp.CheckConnection())
            {
                SharePointConnectionManager = tmp;
                return true;
            }

            return false;
        }

        #region System Data
        public Dictionary<string, string> SystemDataDictionary { get; }
        public void SetFileModificationStatus(string _filePath, bool _modified)
        {
            string fileName = Path.GetFileName(_filePath);
            int fileIndex = FileNames.IndexOf(fileName);
            SetFileModificationStatus(fileIndex, _modified);
        }
        public void SetFileModificationStatus(int _fileIndex, bool _modified)
        {
            try
            {
                SystemDataDictionary["CSVModified_" + (_fileIndex + 1).ToString()] = _modified.ToString();
                DocumentLoader.AddSystemDataToCSV();
            }
            catch (Exception)
            {
                MessageBox.Show("No CSV file matches the file name!");
            }
        }
        public void ResetAllFileModificationStatus()
        {
            for (int i = 0; i < FileNames.Count; i++)
            {
                SetFileModificationStatus(i, false);
            }
        }

        public int NumOfModifiedFiles()
        {
            int count = 0;

            string trueString = true.ToString();
            for (int i = 1; i <= FileNames.Count; i++)
            {
                if (SystemDataDictionary["CSVModified_" + i.ToString()] == trueString)
                    count++;
            }

            return count;
        }
        #endregion

        #region Data
        public List<RegistrationInfo_Internal> RegistrationInfos_Internal { get; }
        public List<RegistrationInfo_External> RegistrationInfos_External { get; }
        public Dictionary<int, eRegistrationStatus> RegistrationStatusDictionary { get; }

        public Dictionary<int, string> NameDictionary { get; }
        public Dictionary<int, string> SurnameDictionary { get; }
        public Dictionary<int, FullName> FullNameDictionary { get; }

        public Dictionary<int, int> YearDictionary { get; }
        public Dictionary<int, eTerm> TermDictionary { get; }
        public Dictionary<int, Semester> SemesterDictionary { get; }

        public List<Student> StudentList { get; }
        public List<NonIberoCommunityMember> NonIberoCommunityMemberList { get; }

        public Dictionary<int, eExamType> ExamTypeDictionary { get; }
        public Dictionary<int, NonInstitutionalExam_Internal> NonInstitutionalExam_InternalDictionary { get; }
        #endregion

        #region ImageBytes
        public readonly byte[] InlineImage1_2;
        public readonly byte[] InlineImage1_3;
        public readonly byte[] InlineImage1_4;
        #endregion

        private void SetFilePaths()
        {
            List<string> filePaths = new List<string>();
            string localSaveFilesPath = CoreValues.LocalSaveFilesPath + @"Data\";
            foreach (var fileName in FileNames)
            {
                filePaths.Add(localSaveFilesPath + fileName);
            }

            FilePath_ExamType = filePaths[0];
            FilePath_FullName = filePaths[1];
            FilePath_Name = filePaths[2];
            FilePath_NonIberoCommunityMember = filePaths[3];
            FilePath_NonInstitutionalExam_Internal = filePaths[4];
            FilePath_RegistrationInfo_Internal = filePaths[5];
            FilePath_RegistrationInfo_External = filePaths[6];
            FilePath_RegistrationStatus = filePaths[7];
            FilePath_Semester = filePaths[8];
            FilePath_Student = filePaths[9];
            FilePath_Surname = filePaths[10];
            FilePath_Term = filePaths[11];
            FilePath_Year = filePaths[12];
        }
    }
}
