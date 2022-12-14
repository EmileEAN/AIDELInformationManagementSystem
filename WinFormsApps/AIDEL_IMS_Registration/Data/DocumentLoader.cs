using AIDEL_IMS_Registration.EEANWorksScripts.WinFormsApps.AIDEL_IMS_Registration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EEANWorks.Microsoft;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data
{
    public static class DocumentLoader
    {
        #region Import Data from CSV
        public static bool ImportSystemDataDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<string, string> dictionary = container.SystemDataDictionary;

                using (var reader = new StreamReader(container.FilePath_SystemData))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        string dataName = values[0];

                        if (!dictionary.ContainsKey(dataName))
                        {
                            string value = values[1];

                            dictionary.Add(dataName, value);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportNameDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, string> dictionary = container.NameDictionary;

                string filePath = container.FilePath_Name;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            string s = values[1];

                            dictionary.Add(id, s);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportSurnameDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, string> dictionary = container.SurnameDictionary;

                string filePath = container.FilePath_Surname;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            string s = values[1];

                            dictionary.Add(id, s);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportFullNameDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, FullName> dictionary = container.FullNameDictionary;

                string filePath = container.FilePath_FullName;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            int firstNameId = Convert.ToInt32(values[1]);
                            int middleNameId = Convert.ToInt32(values[2]);
                            int paternalSurnameId = Convert.ToInt32(values[3]);
                            int maternalSurnameId = Convert.ToInt32(values[4]);

                            var nameDictionary = container.NameDictionary;
                            var surnameDictionary = container.SurnameDictionary;

                            dictionary.Add(id, new FullName(nameDictionary[firstNameId], nameDictionary[middleNameId], surnameDictionary[paternalSurnameId], surnameDictionary[maternalSurnameId]));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportStudentListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Student> list = container.StudentList;

                string filePath = container.FilePath_Student;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int accountNumber = Convert.ToInt32(values[0]);

                        if (accountNumber == 0 || !list.Any(x => x.AccountNumber == accountNumber))
                        {
                            int fullNameId = Convert.ToInt32(values[1]);
                            string organizationEmail = values[2];

                            var fullNameDictionary = container.FullNameDictionary;

                            list.Add(new Student(accountNumber, fullNameDictionary[fullNameId], organizationEmail));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportNonIberoCommunityMemberListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<NonIberoCommunityMember> list = container.NonIberoCommunityMemberList;

                string filePath = container.FilePath_NonIberoCommunityMember;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int personalId = Convert.ToInt32(values[0]);

                        if (personalId == 0 || !list.Any(x => x.PersonalId == personalId))
                        {
                            int fullNameId = Convert.ToInt32(values[1]);
                            string email = values[2];

                            var fullNameDictionary = container.FullNameDictionary;

                            list.Add(new NonIberoCommunityMember(fullNameDictionary[fullNameId], email, personalId));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportYearDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, int> dictionary = container.YearDictionary;

                string filePath = container.FilePath_Year;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            int number = Convert.ToInt32(values[1]);

                            dictionary.Add(id, number);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportTermDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, eTerm> dictionary = container.TermDictionary;

                string filePath = container.FilePath_Term;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            string name = values[1];

                            dictionary.Add(id, name.ToCorrespondingEnumValue<eTerm>());
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportSemesterDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Semester> dictionary = container.SemesterDictionary;

                string filePath = container.FilePath_Semester;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            int yearId = Convert.ToInt32(values[1]);
                            int termId = Convert.ToInt32(values[2]);

                            var yearDictionary = container.YearDictionary;
                            var termDictionary = container.TermDictionary;

                            dictionary.Add(id, new Semester(yearDictionary[yearId], termDictionary[termId]));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportRegistrationInfoListFromCSVs()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var list_internal = container.RegistrationInfos_Internal;
                var list_external = container.RegistrationInfos_External;

                string filePath_internal = container.FilePath_RegistrationInfo_Internal;
                using (var reader = new StreamReader(filePath_internal))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!list_internal.Any(x => x.Id == id))
                        {
                            eRegistrationStatus status = container.RegistrationStatusDictionary[Convert.ToInt32(values[1])];
                            DateTime startTime = DateTime.Parse(values[2]);
                            DateTime completionTime = DateTime.Parse(values[3]);
                            string organizationEmail = values[4];
                            eExamType examType = container.ExamTypeDictionary[Convert.ToInt32(values[5])];
                            DateTime examDate = (values[6] != "") ? DateTime.Parse(values[6]) : default;
                            NonInstitutionalExam_Internal exam = examDate != default ? container.NonInstitutionalExam_InternalDictionary.First(x => x.Value.Type == examType && x.Value.Date == examDate).Value : new NonInstitutionalExam_Internal(default, default, default, default, examType);
                            string id1Url = values[7];
                            string id2Url = values[8];
                            string registrationPdfUrl = values[9];
                            string voucherUrl = values[10];
                            int bookingNumber = string.IsNullOrWhiteSpace(values[11]) ? default : Convert.ToInt32(values[11]);

                            list_internal.Add(new RegistrationInfo_Internal(id, status, startTime, completionTime, organizationEmail, exam, id1Url, id2Url, registrationPdfUrl, voucherUrl, bookingNumber));
                        }
                    }
                }

                string filePath_external = container.FilePath_RegistrationInfo_External;
                using (var reader = new StreamReader(filePath_external))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!list_external.Any(x => x.Id == id))
                        {
                            eRegistrationStatus status = container.RegistrationStatusDictionary[Convert.ToInt32(values[1])];
                            DateTime completionTime = DateTime.Parse(values[2]);
                            int examineeAccountNumber = Convert.ToInt32(values[3]);
                            string email = values[4];
                            eExamType examType = container.ExamTypeDictionary[Convert.ToInt32(values[5])];
                            DateTime examDate = (values[6] != "") ? DateTime.Parse(values[6]) : default;
                            NonInstitutionalExam_Internal exam = examDate != default ? container.NonInstitutionalExam_InternalDictionary.First(x => x.Value.Type == examType && x.Value.Date == examDate).Value : new NonInstitutionalExam_Internal(default, default, default, default, examType);
                            string id1Url = values[7];
                            string id2Url = values[8];
                            string registrationPdfUrl = values[9];
                            string voucherUrl = values[10];
                            int bookingNumber = string.IsNullOrWhiteSpace(values[11]) ? default : Convert.ToInt32(values[11]);

                            list_external.Add(new RegistrationInfo_External(id, status, completionTime, examineeAccountNumber, email, exam, id1Url, id2Url, registrationPdfUrl, voucherUrl, bookingNumber));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportRegistrationStatusDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, eRegistrationStatus> dictionary = container.RegistrationStatusDictionary;

                string filePath = container.FilePath_RegistrationStatus;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            string name = values[1];

                            dictionary.Add(id, name.ToCorrespondingEnumValue<eRegistrationStatus>());
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportExamTypeDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, eExamType> dictionary = container.ExamTypeDictionary;

                string filePath = container.FilePath_ExamType;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(id))
                        {
                            string name = values[1];

                            dictionary.Add(id, name.ToCorrespondingEnumValue<eExamType>());
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool ImportNonInstitutionalExam_InternalDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var dictionary = container.NonInstitutionalExam_InternalDictionary;

                string filePath = container.FilePath_NonInstitutionalExam_Internal;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int id = Convert.ToInt32(values[0]);

                        if (!dictionary.Any(x => x.Key == id))
                        {
                            DateTime registrationInitDate = DateTime.Parse(values[1]);
                            DateTime registrationEndDate = DateTime.Parse(values[2]);
                            DateTime date = DateTime.Parse(values[3]);
                            int maxNumOfExaminees = Convert.ToInt32(values[4]);
                            int examTypeId = Convert.ToInt32(values[5]);
                            eExamType type = container.ExamTypeDictionary[examTypeId];

                            dictionary.Add(id, new NonInstitutionalExam_Internal(registrationInitDate, registrationEndDate, date, maxNumOfExaminees, type));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Add Data to CSV
        public static bool AddSystemDataToCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                using (var sw = new StreamWriter(container.FilePath_SystemData))
                {
                    sw.WriteLine("DataName;Value");

                    foreach (var systemData in DataContainer.Instance.SystemDataDictionary)
                    {
                        sw.WriteLine("{0};{1}", systemData.Key, systemData.Value);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool AddRegistrationInfo_InternalToCSV(DataContainer _container, List<RegistrationInfo> _registrationInfos, DateTime _startTime, DateTime _completionTime,  string _organizationEmail, eExamType _examType, string _fullName, string _id1Url, string _id2Url, string _registrationPdfUrl)
        {
            try
            {
                int examineeAccountNumber = 0;
                Student examinee = DataContainer.Instance.StudentList.FirstOrDefault(x => x.OrganizationEmail == _organizationEmail || (x.Name.ToString().EqualsIgnoringCase(_fullName) && x.OrganizationEmail == ""));
                if (examinee != null)
                {
                    if (examinee.OrganizationEmail == _organizationEmail)
                        examineeAccountNumber = examinee.AccountNumber;
                    else if (examinee.Name.ToString().EqualsIgnoringCase(_fullName))
                        DocumentLoader.EditStudentInCSV(examinee, _organizationEmail); // Register the organization email
                }
                else
                {
                    FullName fn = _fullName.ToFullName(true);
                    if (!AddStudentToCSV("000000", fn.FirstName, fn.MiddleName, fn.PaternalSurname, fn.MaternalSurname, _organizationEmail))
                        return false;

                    examinee = DataContainer.Instance.StudentList.First(x => x.OrganizationEmail == _organizationEmail);
                }

                int lastId = 0;
                RegistrationInfo last = _registrationInfos.LastOrDefault();
                if (last != null)
                    lastId = last.Id;
                int newId = lastId + 1;

                eRegistrationStatus status = eRegistrationStatus.IDsAndPDFSignCheckRequired;

                var examInfo = _container.NonInstitutionalExam_InternalDictionary.FirstOrDefault(x => x.Value.RegistrationInitDate <= _completionTime && _completionTime <= x.Value.RegistrationEndDate);

                if (examInfo.Value == null) // It is unlikely for this to happen, but in case there is no exam registered for the given period, do nothing
                    return true; // Just return so that nothing is registered

                int bookingNumber = 0;
                NonInstitutionalExam_Internal exam = examInfo.Value;

                var targetRegistrationInfos = _registrationInfos.Where(x => x.Exam == exam 
                                                                        && x.Status != eRegistrationStatus.Repeated
                                                                        && x.Status != eRegistrationStatus.WaitingList
                                                                        && x.Status != eRegistrationStatus.Canceled)
                                                                .OrderBy(x => x.BookingNumber)
                                                                .ToList();
                if (targetRegistrationInfos.Any(x => x.Email == examinee.OrganizationEmail)) // Examinee already registered for the exam
                    status = eRegistrationStatus.Repeated;
                else
                {
                    int registeredExamineesCount = targetRegistrationInfos.Count();
                    if (registeredExamineesCount < exam.MaxNumOfExaminees) // There are seats available for the exam
                    {
                        string bookingNumberString_examDatePortion = exam.Date.ToString("ddMMyy");

                        // Check each existing registration info for the exam; and if a booking number is missing, set that value as the booking number for the new registration info
                        for (int i = 1; i <= registeredExamineesCount; i++)
                        {
                            string bookingNumberString = (i.ToString("D2") + bookingNumberString_examDatePortion);
                            if (targetRegistrationInfos[i - 1].BookingNumber.ToString(CoreValues.BookingNumberStringFormat) != bookingNumberString)
                            {
                                bookingNumber = Convert.ToInt32(bookingNumberString);
                                break;
                            }
                        }
                        // If no booking number was missing, set the smallest booking number available
                        if (bookingNumber == 0)
                            bookingNumber = Convert.ToInt32((registeredExamineesCount + 1).ToString("D2") + bookingNumberString_examDatePortion);
                    }
                    else
                        status = eRegistrationStatus.WaitingList;
                }

                int statusId = DataContainer.Instance.RegistrationStatusDictionary.GetFirstKey(status);

                string filePath = _container.FilePath_RegistrationInfo_Internal;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",
                                    newId.ToString(),
                                    statusId.ToString(),
                                    _startTime.ToString(),
                                    _completionTime.ToString(),
                                    _organizationEmail,
                                    DataContainer.Instance.ExamTypeDictionary.GetFirstKey(_examType).ToString(),
                                    exam.Date.ToString(),
                                    _id1Url,
                                    _id2Url,
                                    _registrationPdfUrl,
                                    "",
                                    bookingNumber.ToString(CoreValues.BookingNumberStringFormat));
                }

                _registrationInfos.Add(new RegistrationInfo_Internal(newId, status, _startTime, _completionTime, _organizationEmail, exam, _id1Url, _id2Url, _registrationPdfUrl, null, bookingNumber));

                _container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }
        public static bool AddRegistrationInfo_ExternalToCSV(DataContainer _container, List<RegistrationInfo> _registrationInfos, DateTime _completionTime, int _personalId, string _email, eExamType _examType, string _fullName, string _id1Url, string _id2Url, string _registrationPdfUrl)
        {
            try
            {
                int examineePersonalId = _personalId;
                NonIberoCommunityMember examinee = DataContainer.Instance.NonIberoCommunityMemberList.FirstOrDefault(x => x.PersonalId == _personalId || (x.Email == _email && x.Name.ToString().EqualsIgnoringCase(_fullName)));
                if (examinee != null)
                {
                    if (examinee.Email == _email)
                        examineePersonalId = examinee.PersonalId;
                    else if (examinee.Name.ToString().EqualsIgnoringCase(_fullName))
                        DocumentLoader.EditNonIberoCommunityMemberInCSV(examinee, _email); // Register the email
                }
                else
                {
                    FullName fn = _fullName.ToFullName(true);
                    if (!AddNonIberoCommunityMemberToCSV("000000", fn.FirstName, fn.MiddleName, fn.PaternalSurname, fn.MaternalSurname, _email))
                        return false;

                    examinee = DataContainer.Instance.NonIberoCommunityMemberList.First(x => x.Email == _email);
                }


                int lastId = 0;
                RegistrationInfo last = _registrationInfos.LastOrDefault();
                if (last != null)
                    lastId = last.Id;
                int newId = lastId + 1;

                eRegistrationStatus status = eRegistrationStatus.IDsAndPDFSignCheckRequired;

                var examInfo = _container.NonInstitutionalExam_InternalDictionary.FirstOrDefault(x => x.Value.RegistrationInitDate <= _completionTime && _completionTime <= x.Value.RegistrationEndDate);

                if (examInfo.Value == null) // It is unlikely for this to happen, but in case there is no exam registered for the given period, do nothing
                    return true; // Just return so that nothing is registered

                int bookingNumber = 0;
                NonInstitutionalExam_Internal exam = examInfo.Value;

                var targetRegistrationInfos = _registrationInfos.Where(x => x.Exam == exam
                                                                        && x.Status != eRegistrationStatus.Repeated
                                                                        && x.Status != eRegistrationStatus.WaitingList
                                                                        && x.Status != eRegistrationStatus.Canceled)
                                                                .OrderBy(x => x.BookingNumber)
                                                                .ToList();
                if (targetRegistrationInfos.Any(x => x.Email == _email)) // Examinee already registered for the exam
                    status = eRegistrationStatus.Repeated;
                else
                {
                    int registeredExamineesCount = targetRegistrationInfos.Count();
                    if (registeredExamineesCount < exam.MaxNumOfExaminees) // There are seats available for the exam
                    {
                        string bookingNumberString_examDatePortion = exam.Date.ToString("ddMMyy");

                        // Check each existing registration info for the exam; and if a booking number is missing, set that value as the booking number for the new registration info
                        for (int i = 1; i <= registeredExamineesCount; i++)
                        {
                            string bookingNumberString = (i.ToString("D2") + bookingNumberString_examDatePortion);
                            if (targetRegistrationInfos[i - 1].BookingNumber.ToString(CoreValues.BookingNumberStringFormat) != bookingNumberString)
                            {
                                bookingNumber = Convert.ToInt32(bookingNumberString);
                                break;
                            }
                        }
                        // If no booking number was missing, set the smallest booking number available
                        if (bookingNumber == 0)
                            bookingNumber = Convert.ToInt32((registeredExamineesCount + 1).ToString("D2") + bookingNumberString_examDatePortion);
                    }
                    else
                        status = eRegistrationStatus.WaitingList;
                }

                int statusId = DataContainer.Instance.RegistrationStatusDictionary.GetFirstKey(status);

                string filePath = _container.FilePath_RegistrationInfo_External;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",
                                    newId.ToString(),
                                    statusId.ToString(),
                                    _completionTime.ToString(),
                                    examineePersonalId.ToString(),
                                    _email,
                                    DataContainer.Instance.ExamTypeDictionary.GetFirstKey(_examType).ToString(),
                                    exam.Date.ToString(),
                                    _id1Url,
                                    _id2Url,
                                    _registrationPdfUrl,
                                    "",
                                    bookingNumber.ToString(CoreValues.BookingNumberStringFormat));
                }

                _registrationInfos.Add(new RegistrationInfo_External(newId, status, _completionTime, examineePersonalId, _email, exam, _id1Url, _id2Url, _registrationPdfUrl, null, bookingNumber));

                _container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool AddStudentToCSV(string _accountNumber, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname)
        {
            return AddStudentToCSV(_accountNumber, _firstName, _middleName, _paternalSurname, _maternalSurname, null, null, null);
        }
        public static bool AddStudentToCSV(string _accountNumber, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname, string _organizationEmail)
        {
            return AddStudentToCSV(_accountNumber, _firstName, _middleName, _paternalSurname, _maternalSurname, _organizationEmail, null, null);
        }
        public static bool AddStudentToCSV(string _accountNumber, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname, string _organizationEmail, string _preferredEmail, string _phone)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var nameDictionary = container.NameDictionary;
                var surnameDictionary = container.SurnameDictionary;
                var fullNameDictionary = container.FullNameDictionary;
                var semesterDictionary = container.SemesterDictionary;

                int firstNameId = nameDictionary.GetFirstKeyOrDefault(_firstName);
                if (firstNameId == default) // If the item does not exist
                {
                    firstNameId = AddNameToCSV(_firstName);
                    if (firstNameId == default)
                        return false;
                }

                int middleNameId = nameDictionary.GetFirstKeyOrDefault(_middleName);
                if (middleNameId == default) // If the item does not exist
                {
                    middleNameId = AddNameToCSV(_middleName);
                    if (middleNameId == default)
                        return false;
                }

                int paternalSurnameId = surnameDictionary.GetFirstKeyOrDefault(_paternalSurname);
                if (paternalSurnameId == default) // If the item does not exist
                {
                    paternalSurnameId = AddSurnameToCSV(_paternalSurname);
                    if (paternalSurnameId == default)
                        return false;
                }

                int maternalSurnameId = surnameDictionary.GetFirstKeyOrDefault(_maternalSurname);
                if (maternalSurnameId == default) // If the item does not exist
                {
                    maternalSurnameId = AddSurnameToCSV(_maternalSurname);
                    if (maternalSurnameId == default)
                        return false;
                }

                int fullNameId = fullNameDictionary.FirstOrDefault(x => x.Value.FirstName == _firstName
                                                                        && x.Value.MiddleName == _middleName
                                                                        && x.Value.PaternalSurname == _paternalSurname
                                                                        && x.Value.MaternalSurname == _maternalSurname).Key;
                if (fullNameId == default) // If the item does not exist
                {
                    fullNameId = AddFullNameToCSV(firstNameId, _firstName, middleNameId, _middleName, paternalSurnameId, _paternalSurname, maternalSurnameId, _maternalSurname);
                    if (fullNameId == default)
                        return false;
                }

                string majorIdString = "";

                string semesterIdString = "";

                string filePath = container.FilePath_Student;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5};{6}",
                                _accountNumber, 
                                fullNameId.ToString(),
                                _organizationEmail,
                                _preferredEmail ?? "",
                                _phone ?? "",
                                majorIdString,
                                semesterIdString);
                }

                DataContainer.Instance.StudentList.Add(new Student(Convert.ToInt32(_accountNumber), fullNameDictionary[fullNameId], _organizationEmail)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool AddNonIberoCommunityMemberToCSV(string _personalId, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname)
        {
            return AddNonIberoCommunityMemberToCSV(_personalId, _firstName, _middleName, _paternalSurname, _maternalSurname, null, null, null);
        }
        public static bool AddNonIberoCommunityMemberToCSV(string _personalId, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname, string _email)
        {
            return AddNonIberoCommunityMemberToCSV(_personalId, _firstName, _middleName, _paternalSurname, _maternalSurname, _email, null, null);
        }
        public static bool AddNonIberoCommunityMemberToCSV(string _personalId, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname, string _email, string _preferredEmail, string _phone)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var nameDictionary = container.NameDictionary;
                var surnameDictionary = container.SurnameDictionary;
                var fullNameDictionary = container.FullNameDictionary;
                var semesterDictionary = container.SemesterDictionary;

                int firstNameId = nameDictionary.GetFirstKeyOrDefault(_firstName);
                if (firstNameId == default) // If the item does not exist
                {
                    firstNameId = AddNameToCSV(_firstName);
                    if (firstNameId == default)
                        return false;
                }

                int middleNameId = nameDictionary.GetFirstKeyOrDefault(_middleName);
                if (middleNameId == default) // If the item does not exist
                {
                    middleNameId = AddNameToCSV(_middleName);
                    if (middleNameId == default)
                        return false;
                }

                int paternalSurnameId = surnameDictionary.GetFirstKeyOrDefault(_paternalSurname);
                if (paternalSurnameId == default) // If the item does not exist
                {
                    paternalSurnameId = AddSurnameToCSV(_paternalSurname);
                    if (paternalSurnameId == default)
                        return false;
                }

                int maternalSurnameId = surnameDictionary.GetFirstKeyOrDefault(_maternalSurname);
                if (maternalSurnameId == default) // If the item does not exist
                {
                    maternalSurnameId = AddSurnameToCSV(_maternalSurname);
                    if (maternalSurnameId == default)
                        return false;
                }

                int fullNameId = fullNameDictionary.FirstOrDefault(x => x.Value.FirstName == _firstName
                                                                        && x.Value.MiddleName == _middleName
                                                                        && x.Value.PaternalSurname == _paternalSurname
                                                                        && x.Value.MaternalSurname == _maternalSurname).Key;
                if (fullNameId == default) // If the item does not exist
                {
                    fullNameId = AddFullNameToCSV(firstNameId, _firstName, middleNameId, _middleName, paternalSurnameId, _paternalSurname, maternalSurnameId, _maternalSurname);
                    if (fullNameId == default)
                        return false;
                }

                string filePath = container.FilePath_NonIberoCommunityMember;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                _personalId,
                                fullNameId.ToString(),
                                _email,
                                _phone ?? "");
                }

                DataContainer.Instance.NonIberoCommunityMemberList.Add(new NonIberoCommunityMember(fullNameDictionary[fullNameId], _email, Convert.ToInt32(_personalId))); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        private static int AddNameToCSV(string _name)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, string> nameDictionary = container.NameDictionary;

                int lastId = (nameDictionary.Count != 0) ? nameDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_Name;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _name);
                }

                nameDictionary.Add(newId, _name); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static int AddSurnameToCSV(string _surname)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, string> surnameDictionary = container.SurnameDictionary;

                int lastId = (surnameDictionary.Count != 0) ? surnameDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_Surname;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _surname);
                }

                surnameDictionary.Add(newId, _surname); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static int AddFullNameToCSV(int _firstNameId, string _firstName, int _middleNameId, string _middleName, int _paternalSurnameId, string _paternalSurname, int _maternalSurnameId, string _maternalSurname)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, FullName> fullNameDictionary = container.FullNameDictionary;

                int lastId = (fullNameDictionary.Count != 0) ? fullNameDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_FullName;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4}", 
                                    newId.ToString(),
                                    _firstNameId, 
                                    _middleNameId, 
                                    _paternalSurnameId, 
                                    _maternalSurnameId);
                }

                fullNameDictionary.Add(newId, new FullName(_firstName, _middleName, _paternalSurname, _maternalSurname)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddYearToCSV(int _year)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var yearDictionary = container.YearDictionary;

                int lastId = (yearDictionary.Count != 0) ? yearDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_Year;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _year.ToString());
                }

                yearDictionary.Add(newId, _year); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddSemesterToCSV(int _yearId, int _termId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var yearDictionary = container.YearDictionary;
                var termDictionary = container.TermDictionary;
                var semesterDictionary = container.SemesterDictionary;

                int lastId = (semesterDictionary.Count != 0) ? semesterDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_Semester;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}", newId.ToString(), _yearId.ToString(), _termId.ToString());
                }

                semesterDictionary.Add(newId, new Semester(yearDictionary[_yearId], termDictionary[_termId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddNonInstitutionalExam_InternalToCSV(DateTime _registrationInitDate, DateTime _registrationEndDate, DateTime _examDate, int _maxNumOfExaminees, eExamType _examType)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var examTypeDictionary = container.ExamTypeDictionary;

                int examTypeId = examTypeDictionary.GetFirstKey(_examType);

                var examDictionary = container.NonInstitutionalExam_InternalDictionary;

                int lastId = (examDictionary.Count != 0) ? examDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_NonInstitutionalExam_Internal;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5}",
                        newId.ToString(),
                        _registrationInitDate.ToString(),
                        _registrationEndDate.ToString(),
                        _examDate.ToString(),
                        _maxNumOfExaminees.ToString(),
                        examTypeId.ToString());
                }

                examDictionary.Add(newId, new NonInstitutionalExam_Internal(_registrationInitDate, _registrationEndDate, _examDate, _maxNumOfExaminees, _examType)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }
        #endregion

        #region Edit Data in CSV
        public static bool EditStudentInCSV(Student _student, string _organizationEmail)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_Student;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == _student.AccountNumber) // If it is the row to be edited
                            {
                                values[2] = _organizationEmail.ToString();

                                line = string.Join(";", values);
                            }

                            lines.Add(line);
                        }
                    }

                    using (var sw = new StreamWriter(filePath))
                    {
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line);
                        }
                    }

                    container.SetFileModificationStatus(filePath, true);
                }

                // Update application-side data
                {
                    _student.OrganizationEmail = _organizationEmail;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }
        public static bool EditStudentInCSV(Student _student, int _accountNumber)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_Student;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (values[2] == _student.OrganizationEmail) // If it is the row to be edited
                            {
                                values[0] = _accountNumber.ToString();

                                line = string.Join(";", values);
                            }

                            lines.Add(line);
                        }
                    }

                    using (var sw = new StreamWriter(filePath))
                    {
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line);
                        }
                    }

                    container.SetFileModificationStatus(filePath, true);
                }

                // Update application-side data
                {
                    _student.AccountNumber = _accountNumber;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditNonIberoCommunityMemberInCSV(NonIberoCommunityMember _person, string _email)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_NonIberoCommunityMember;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == _person.PersonalId) // If it is the row to be edited
                            {
                                values[2] = _email.ToString();

                                line = string.Join(";", values);
                            }

                            lines.Add(line);
                        }
                    }

                    using (var sw = new StreamWriter(filePath))
                    {
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line);
                        }
                    }

                    container.SetFileModificationStatus(filePath, true);
                }

                // Update application-side data
                {
                    _person.Email = _email;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }
        public static bool EditNonIberoCommunityMemberInCSV(NonIberoCommunityMember _person, int _personalId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_NonIberoCommunityMember;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (values[2] == _person.Email) // If it is the row to be edited
                            {
                                values[0] = _personalId.ToString();

                                line = string.Join(";", values);
                            }

                            lines.Add(line);
                        }
                    }

                    using (var sw = new StreamWriter(filePath))
                    {
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line);
                        }
                    }

                    container.SetFileModificationStatus(filePath, true);
                }

                // Update application-side data
                {
                    _person.PersonalId = _personalId;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditRegistrationInfoInCSV(RegistrationInfo_Internal _registrationInfo, eRegistrationStatus _status)
        {
            return EditRegistrationInfoInCSV(_registrationInfo, DataContainer.Instance.FilePath_RegistrationInfo_Internal, _status);
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo_Internal _registrationInfo, DateTime _examDate, int _bookingNumber)
        {
            return EditRegistrationInfoInCSV(_registrationInfo, DataContainer.Instance.FilePath_RegistrationInfo_Internal, _examDate, _bookingNumber);
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo_Internal _registrationInfo, string _paymentEvidenceUrl)
        {
            return EditRegistrationInfoInCSV(_registrationInfo, DataContainer.Instance.FilePath_RegistrationInfo_Internal, _paymentEvidenceUrl);
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo_External _registrationInfo, eRegistrationStatus _status)
        {
            return EditRegistrationInfoInCSV(_registrationInfo, DataContainer.Instance.FilePath_RegistrationInfo_External, _status);
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo_External _registrationInfo, DateTime _examDate, int _bookingNumber)
        {
            return EditRegistrationInfoInCSV(_registrationInfo, DataContainer.Instance.FilePath_RegistrationInfo_External, _examDate, _bookingNumber);
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo_External _registrationInfo, string _paymentEvidenceUrl)
        {
            return EditRegistrationInfoInCSV(_registrationInfo, DataContainer.Instance.FilePath_RegistrationInfo_External, _paymentEvidenceUrl);
        }

        public static bool EditRegistrationInfoInCSV(RegistrationInfo _registrationInfo, string _filePath, eRegistrationStatus _status)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var statusDictionary = container.RegistrationStatusDictionary;
                int statusId = statusDictionary.GetFirstKey(_status);

                int id = _registrationInfo.Id;

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

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[1] = statusId.ToString();

                                line = string.Join(";", values);
                            }

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

                    container.SetFileModificationStatus(_filePath, true);
                }

                // Update application-side data
                {
                    _registrationInfo.Status = _status;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo _registrationInfo, string _filePath, DateTime _examDate, int _bookingNumber)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var statusDictionary = container.RegistrationStatusDictionary;

                int id = _registrationInfo.Id;

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

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[6] = (_examDate != default) ? _examDate.ToString("dd/MM/yyyy HH:mm") : "";
                                values[11] = _bookingNumber.ToString(CoreValues.BookingNumberStringFormat);

                                line = string.Join(";", values);
                            }

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

                    container.SetFileModificationStatus(_filePath, true);
                }

                // Update application-side data
                {
                    _registrationInfo.Exam.Date = _examDate;
                    _registrationInfo.BookingNumber = _bookingNumber;

                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }
        public static bool EditRegistrationInfoInCSV(RegistrationInfo _registrationInfo, string _filePath, string _paymentEvidenceUrl)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var statusDictionary = container.RegistrationStatusDictionary;

                int id = _registrationInfo.Id;

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

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[10] = _paymentEvidenceUrl;

                                line = string.Join(";", values);
                            }

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

                    container.SetFileModificationStatus(_filePath, true);
                }

                // Update application-side data
                {
                    _registrationInfo.VoucherUrl = _paymentEvidenceUrl;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        //public static bool EditRegistrationInfoInCSV(RegistrationInfo_Internal _registrationInfo, int _bookingNumber)
        //{
        //    try
        //    {
        //        DataContainer container = DataContainer.Instance;
        //        var statusDictionary = container.RegistrationStatusDictionary;

        //        int id = _registrationInfo.Id;

        //        // Modify target row in CSV
        //        {
        //            string filePath = container.FilePath_RegistrationInfo_Internal;
        //            List<string> lines = new List<string>();

        //            using (var sr = new StreamReader(filePath))
        //            {
        //                string line;
        //                if ((line = sr.ReadLine()) != null) // Skip the header row
        //                    lines.Add(line);

        //                while ((line = sr.ReadLine()) != null)
        //                {
        //                    string[] values = line.Split(';');

        //                    if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
        //                    {
        //                        values[11] = _bookingNumber.ToString();

        //                        line = string.Join(";", values);
        //                    }

        //                    lines.Add(line);
        //                }
        //            }

        //            using (var sw = new StreamWriter(filePath))
        //            {
        //                foreach (var line in lines)
        //                {
        //                    sw.WriteLine(line);
        //                }
        //            }

        //            container.SetFileModificationStatus(filePath, true);
        //        }

        //        // Update application-side data
        //        {
        //            _registrationInfo.BookingNumber = _bookingNumber;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
        //        return false;
        //    }
        //}
        #endregion

        #region Delete Data from CSV
        public static bool DeleteNonInstitutionalExam_InternalFromCSV(NonInstitutionalExam_Internal nonInstitutionalExam_Internal)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                int? id = container.NonInstitutionalExam_InternalDictionary.GetFirstKeyOrDefault(nonInstitutionalExam_Internal);
                if (id == null)
                    return false;
                string idString = id.ToString();

                if (!CSV.DeleteRow(idString, container.FilePath_NonInstitutionalExam_Internal))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.NonInstitutionalExam_InternalDictionary;
                    dictionary.Remove(id.Value);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }
        #endregion

        public static bool LoadRegistrationInfoFromExcel(string _filePath_internal, string _filePath_external, eExamType _examType)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<RegistrationInfo> registrationInfos = new List<RegistrationInfo>();
                registrationInfos.AddRange(container.RegistrationInfos_Internal);
                registrationInfos.AddRange(container.RegistrationInfos_External);
                registrationInfos = registrationInfos.OrderBy(x => x.CompletionTime).ToList();

                List<Tuple<RegistrationInfo, string>> newRegistrationInfos = new List<Tuple<RegistrationInfo, string>>(); // string is the full name of the individual

                // Open the first (internal) document for reading only.
                using (var document = SpreadsheetDocument.Open(_filePath_internal, false))
                {
                    // Get the Workbook part.
                    WorkbookPart workbookPart = document.WorkbookPart;

                    List<Sheet> sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToList();

                    // Read the third Sheet from Excel file.
                    Sheet sheet = sheets[2];

                    // Get the Worksheet instance.
                    Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                    // Fetch all the rows present in the Worksheet.
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    // Loop through the Worksheet rows.
                    foreach (var row in rows)
                    {
                        // Ignore the first row, which includes the column titles.
                        if (row.RowIndex.Value != 1)
                        {
                            IEnumerable<Cell> nonNullCells = row.Elements<Cell>();
                            if (nonNullCells.Count() < 3)
                                return true;

                            DateTime startTime = Excel.GetCellValue<DateTime>(nonNullCells.GetCellForColumn("B"), workbookPart);
                            DateTime completionTime = Excel.GetCellValue<DateTime>(nonNullCells.GetCellForColumn("C"), workbookPart);

                            string organizationEmail = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("D"), workbookPart);
                            // Add registration info if it is new.
                            if (!registrationInfos.Any(x => x.StartTime == startTime
                                                        && x.CompletionTime == completionTime
                                                        && x.Email == organizationEmail))
                            {
                                string fullName = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("E"), workbookPart);

                                string id1Url = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("F"), workbookPart);
                                string id2Url = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("G"), workbookPart);
                                string registrationPdfUrl = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("H"), workbookPart);

                                newRegistrationInfos.Add(new Tuple<RegistrationInfo, string>(new RegistrationInfo_Internal(default, default, startTime, completionTime, organizationEmail, default, id1Url, id2Url, registrationPdfUrl), fullName));
                            }
                        }
                    }
                }

                // Open the second (external) document for reading only.
                using (var document = SpreadsheetDocument.Open(_filePath_external, false))
                {
                    // Get the Workbook part.
                    WorkbookPart workbookPart = document.WorkbookPart;

                    List<Sheet> sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToList();

                    // Read the second Sheet from Excel file.
                    Sheet sheet = sheets[0];

                    // Get the Worksheet instance.
                    Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                    // Fetch all the rows present in the Worksheet.
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    // Loop through the Worksheet rows.
                    foreach (var row in rows)
                    {
                        // Ignore the first row, which includes the column titles.
                        if (row.RowIndex.Value != 1)
                        {
                            IEnumerable<Cell> nonNullCells = row.Elements<Cell>();
                            if (nonNullCells.Count() < 3)
                                return true;

                            DateTime completionTime = Excel.GetCellValue<DateTime>(nonNullCells.GetCellForColumn("A"), workbookPart);

                            string email = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("E"), workbookPart);
                            // Add registration info if it is new.
                            if (!registrationInfos.Any(x => x.CompletionTime == completionTime
                                                        && x.Email == email))
                            {
                                string fullName = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("F"), workbookPart) + " " + Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("G"), workbookPart);

                                string id1Url = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("B"), workbookPart);
                                string id2Url = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("C"), workbookPart);
                                string registrationPdfUrl = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("D"), workbookPart);

                                newRegistrationInfos.Add(new Tuple<RegistrationInfo, string>(new RegistrationInfo_External(default, default, completionTime, 0, email, default, id1Url, id2Url, registrationPdfUrl), fullName));
                            }
                        }
                    }
                }

                foreach (var registrationInfo in newRegistrationInfos.OrderBy(x => x.Item1.CompletionTime))
                {
                    string fullName = registrationInfo.Item2;
                    if (registrationInfo.Item1 is RegistrationInfo_Internal)
                    {
                        var info = registrationInfo.Item1 as RegistrationInfo_Internal;
                        if (!AddRegistrationInfo_InternalToCSV(container, registrationInfos, info.StartTime, info.CompletionTime, info.OrganizationEmail, _examType, fullName, info.Id1Url, info.Id2Url, info.RegistrationPdfUrl))
                            return false;
                    }
                    else
                    {
                        var info = registrationInfo.Item1 as RegistrationInfo_External;
                        if (!AddRegistrationInfo_ExternalToCSV(container, registrationInfos, info.CompletionTime, info.PersonalId, info.Email, _examType, fullName, info.Id1Url, info.Id2Url, info.RegistrationPdfUrl))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data file: " + ex.Message);
                return false;
            }
        }

        public static bool LoadPaymentEvidencesFromExcel(string _filePath_internal, string _filePath_external)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<RegistrationInfo> registrationInfos = new List<RegistrationInfo>();
                registrationInfos.AddRange(container.RegistrationInfos_Internal);
                registrationInfos.AddRange(container.RegistrationInfos_External);
                registrationInfos = registrationInfos.OrderBy(x => x.CompletionTime).ToList();

                // Open the first (internal) document for reading only.
                using (var document = SpreadsheetDocument.Open(_filePath_internal, false))
                {
                    // Get the Workbook part.
                    WorkbookPart workbookPart = document.WorkbookPart;

                    List<Sheet> sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToList();

                    // Read the third Sheet from Excel file.
                    Sheet sheet = sheets[2];

                    // Get the Worksheet instance.
                    Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                    // Fetch all the rows present in the Worksheet.
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    // Loop through the Worksheet rows.
                    foreach (var row in rows)
                    {
                        // Ignore the first row, which includes the column titles.
                        if (row.RowIndex.Value != 1)
                        {
                            IEnumerable<Cell> nonNullCells = row.Elements<Cell>();
                            if (nonNullCells.Count() < 3)
                                return true;

                            DateTime completionTime = Excel.GetCellValue<DateTime>(nonNullCells.GetCellForColumn("C"), workbookPart);
                            string organizationEmail = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("D"), workbookPart); 
                            // Get registration info related to the payment evidence
                            var registrationInfo = registrationInfos.FirstOrDefault(x => x.Status == eRegistrationStatus.PaymentPending
                                                                            && x.Exam.RegistrationInitDate <= completionTime 
                                                                            && completionTime <= x.Exam.RegistrationEndDate
                                                                            && x.Email == organizationEmail) as RegistrationInfo_Internal;
                            if (registrationInfo != null)
                            {
                                if (registrationInfo.Status != eRegistrationStatus.StudentInfoFormPending && registrationInfo.Status != eRegistrationStatus.ETSInfoAndBookingCheckRequired && registrationInfo.Status != eRegistrationStatus.Completed)
                                {
                                    string documentUrl = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("F"), workbookPart);
                                    if (!EditRegistrationInfoInCSV(registrationInfo, documentUrl) || !EditRegistrationInfoInCSV(registrationInfo, eRegistrationStatus.VoucherCheckRequired))
                                    {
                                        MessageBox.Show("Failed to update registration info!");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                // Open the second (external) document for reading only.
                using (var document = SpreadsheetDocument.Open(_filePath_external, false))
                {
                    // Get the Workbook part.
                    WorkbookPart workbookPart = document.WorkbookPart;

                    List<Sheet> sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToList();

                    // Read the first Sheet from Excel file.
                    Sheet sheet = sheets[0];

                    // Get the Worksheet instance.
                    Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                    // Fetch all the rows present in the Worksheet.
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    // Loop through the Worksheet rows.
                    foreach (var row in rows)
                    {
                        // Ignore the first row, which includes the column titles.
                        if (row.RowIndex.Value != 1)
                        {
                            IEnumerable<Cell> nonNullCells = row.Elements<Cell>();
                            if (nonNullCells.Count() < 3)
                                return true;

                            DateTime completionTime = Excel.GetCellValue<DateTime>(nonNullCells.GetCellForColumn("A"), workbookPart);
                            string email = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("C"), workbookPart);

                            // Get registration info related to the payment evidence
                            var registrationInfo = registrationInfos.FirstOrDefault(x => x.Status == eRegistrationStatus.PaymentPending
                                                                            && x.Exam.RegistrationInitDate <= completionTime
                                                                            && completionTime <= x.Exam.RegistrationEndDate
                                                                            && x.Email == email) as RegistrationInfo_External;
                            if (registrationInfo != null)
                            {
                                if (registrationInfo.Status != eRegistrationStatus.StudentInfoFormPending && registrationInfo.Status != eRegistrationStatus.ETSInfoAndBookingCheckRequired && registrationInfo.Status != eRegistrationStatus.Completed)
                                {
                                    string documentUrl = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("B"), workbookPart);
                                    if (!EditRegistrationInfoInCSV(registrationInfo, documentUrl) || !EditRegistrationInfoInCSV(registrationInfo, eRegistrationStatus.VoucherCheckRequired))
                                    {
                                        MessageBox.Show("Failed to update registration info!");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data file: " + ex.Message);
                return false;
            }
        }

        public static bool LoadStudentInformation_InternalFromExcel(string _filePath_internal)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<RegistrationInfo_Internal> registrationInfos = container.RegistrationInfos_Internal;

                // Open the document for reading only.
                using (var document = SpreadsheetDocument.Open(_filePath_internal, false))
                {
                    // Get the Workbook part.
                    WorkbookPart workbookPart = document.WorkbookPart;

                    List<Sheet> sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToList();

                    // Read the third Sheet from Excel file.
                    Sheet sheet = sheets[2];

                    // Get the Worksheet instance.
                    Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                    // Fetch all the rows present in the Worksheet.
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    // Loop through the Worksheet rows.
                    foreach (var row in rows)
                    {
                        // Ignore the first row, which includes the column titles.
                        if (row.RowIndex.Value != 1)
                        {
                            IEnumerable<Cell> nonNullCells = row.Elements<Cell>();
                            if (nonNullCells.Count() < 3)
                                return true;

                            string organizationEmail = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("D"), workbookPart);
                            // Get registration info related to the payment evidence
                            var examinee = container.StudentList.FirstOrDefault(x => x.OrganizationEmail == organizationEmail && x.AccountNumber == default);
                            var registrationInfo = registrationInfos.FirstOrDefault(x => x.Status == eRegistrationStatus.StudentInfoFormPending
                                                                                    && x.OrganizationEmail == organizationEmail);
                            if (examinee != null && registrationInfo != null)
                            {
                                if (registrationInfo.Status != eRegistrationStatus.ETSInfoAndBookingCheckRequired && registrationInfo.Status != eRegistrationStatus.Completed)
                                {
                                    string accountNumberString = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("F"), workbookPart);
                                    int accountNumber = Convert.ToInt32(accountNumberString);
                                    if (!EditStudentInCSV(examinee, accountNumber) || !EditRegistrationInfoInCSV(registrationInfo, eRegistrationStatus.ETSInfoAndBookingCheckRequired))
                                    {
                                        MessageBox.Show("Failed to update student account number!");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data file: " + ex.Message);
                return false;
            }
        }
    }
}
