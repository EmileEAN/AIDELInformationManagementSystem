using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EEANWorks.Microsoft;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Data
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

        public static bool ImportMajorListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Major> list = container.MajorList;

                string filePath = container.FilePath_Major;
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

                        if (!list.Any(x => x.Id == id))
                        {
                            string name = values[1];

                            list.Add(new Major(id, name));
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
                        string organizationEmail = values[2];

                        if ((accountNumber == 0 && !list.Any(x => x.OrganizationEmail == organizationEmail)) || !list.Any(x => x.AccountNumber == accountNumber))
                        {
                            int fullNameId = Convert.ToInt32(values[1]);
                            string preferredEmail = values[3];
                            string phone = values[4];
                            int majorId = string.IsNullOrWhiteSpace(values[5]) ? default : Convert.ToInt32(values[5]);
                            int semesterId = string.IsNullOrWhiteSpace(values[6]) ? default : Convert.ToInt32(values[6]);

                            var fullNameDictionary = container.FullNameDictionary;
                            var majorList = container.MajorList;
                            var semesterDictionary = container.SemesterDictionary;

                            var major = (majorId == default) ? new Major(default, default) : majorList.First(x => x.Id == majorId);
                            var semester = (semesterId == default) ? new Semester(default, default) : semesterDictionary[semesterId];

                            list.Add(new Student(accountNumber, fullNameDictionary[fullNameId], organizationEmail, preferredEmail, phone, major, semester));
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

        public static bool ImportPermissionsTypeDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, ePermissionsType> dictionary = container.PermissionsTypeDictionary;

                string filePath = container.FilePath_PermissionsType;
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

                            dictionary.Add(id, name.ToCorrespondingEnumValue<ePermissionsType>());
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

        public static bool ImportFacultyListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Faculty> list = container.FacultyList;

                string filePath = container.FilePath_Faculty;
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

                        if (!list.Any(x => x.AccountNumber == accountNumber))
                        {
                            int fullNameId = Convert.ToInt32(values[1]);
                            string username = values[2];
                            string password = values[3];
                            int permissionsTypeId = Convert.ToInt32(values[4]);

                            var fullNameDictionary = container.FullNameDictionary;
                            var permissionsTypeDictionary = container.PermissionsTypeDictionary;

                            list.Add(new Faculty(accountNumber, fullNameDictionary[fullNameId], username, password, permissionsTypeDictionary[permissionsTypeId]));
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

        public static bool ImportBaseCriterionDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, string> dictionary = container.BaseCriterionDictionary;

                string filePath = container.FilePath_BaseCriterion;
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

        #region Evaluation Criteria
        #region Quantitative Evaluation
        public static bool ImportValueTypeDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, eValueType> dictionary = container.ValueTypeDictionary;

                string filePath = container.FilePath_ValueType;
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

                            dictionary.Add(id, name.ToCorrespondingEnumValue<eValueType>());
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

        public static bool ImportQuantitativeEvaluationCriterionDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Criterion_QuantitativeEvaluation> dictionary = container.QuantitativeEvaluationCriterionDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationCriterion;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int baseId = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(baseId))
                        {
                            int evaluationValueTypeId = Convert.ToInt32(values[1]);
                            decimal min = Convert.ToDecimal(values[2]);
                            decimal max = Convert.ToDecimal(values[3]);

                            var baseCriterionDictionary = container.BaseCriterionDictionary;
                            var valueTypeDictionary = container.ValueTypeDictionary;

                            dictionary.Add(baseId, new Criterion_QuantitativeEvaluation(baseCriterionDictionary[baseId], valueTypeDictionary[evaluationValueTypeId], min, max));
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

        public static bool ImportQuantitativeEvaluationCriterionWeightDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, CriterionWeight_QuantitativeEvaluation> dictionary = container.QuantitativeEvaluationCriterionWeightDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationCriterionWeight;
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
                            int criterionId = Convert.ToInt32(values[1]);
                            decimal weight = Convert.ToDecimal(values[2]);

                            var criterionDictionary = container.QuantitativeEvaluationCriterionDictionary;

                            dictionary.Add(id, new CriterionWeight_QuantitativeEvaluation(criterionDictionary[criterionId], weight));
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

        public static bool ImportQuantitativeEvaluationCriteria_CriterionWeightMappingListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Tuple<int, int>> list = container.QuantitativeEvaluationCriteria_CriterionWeightMappingList;

                string filePath = container.FilePath_QuantitativeEvaluationCriteria_CriterionWeight;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int criteriaId = Convert.ToInt32(values[0]);
                        int criterionWeightId = Convert.ToInt32(values[1]);

                        if (!list.Any(x => x.Item1 == criteriaId && x.Item2 == criterionWeightId))
                            list.Add(new Tuple<int, int>(criteriaId, criterionWeightId));
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

        public static bool ImportQuantitativeEvaluationCriteriaDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Criteria_QuantitativeEvaluation> dictionary = container.QuantitativeEvaluationCriteriaDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationCriteria;
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

                            var criterionWeightDictionary = container.QuantitativeEvaluationCriterionWeightDictionary;

                            List<CriterionWeight_QuantitativeEvaluation> criterionWeights = new List<CriterionWeight_QuantitativeEvaluation>();
                            foreach (var mapping in DataContainer.Instance.QuantitativeEvaluationCriteria_CriterionWeightMappingList.Where(x => x.Item1 == id))
                            {
                                criterionWeights.Add(criterionWeightDictionary[mapping.Item2]);
                            }

                            dictionary.Add(id, new Criteria_QuantitativeEvaluation(name, criterionWeights));
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

        public static bool ImportQuantitativeEvaluationCriteriaWeightDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, CriteriaWeight_QuantitativeEvaluation> dictionary = container.QuantitativeEvaluationCriteriaWeightDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationCriteriaWeight;
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
                            int criteriaId = Convert.ToInt32(values[1]);
                            decimal weight = Convert.ToDecimal(values[2]);

                            var criteriaDictionary = container.QuantitativeEvaluationCriteriaDictionary;

                            dictionary.Add(id, new CriteriaWeight_QuantitativeEvaluation(criteriaDictionary[criteriaId], weight));
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

        public static bool ImportQuantitativeEvaluationCriteriaSetDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, CriteriaSet_QuantitativeEvaluation> dictionary = container.QuantitativeEvaluationCriteriaSetDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationCriteriaSet;
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
                            int firstPartialCriteriaWeightId = Convert.ToInt32(values[2]);
                            int midtermCriteriaWeightId = Convert.ToInt32(values[3]);
                            int finalCriteriaWeightId = Convert.ToInt32(values[4]);
                            int additionalCriteriaWeightId = Convert.ToInt32(values[5]);

                            var criteriaWeightDictionary = container.QuantitativeEvaluationCriteriaWeightDictionary;

                            dictionary.Add(id, new CriteriaSet_QuantitativeEvaluation(name, (firstPartialCriteriaWeightId > 0) ? criteriaWeightDictionary[firstPartialCriteriaWeightId] : null, criteriaWeightDictionary[midtermCriteriaWeightId], criteriaWeightDictionary[finalCriteriaWeightId], criteriaWeightDictionary[additionalCriteriaWeightId]));
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

        #region Qualitative Evaluation
        public static bool ImportColorDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, System.Drawing.Color> dictionary = container.ColorDictionary;

                string filePath = container.FilePath_Color;
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
                            int r = Convert.ToInt32(values[1]);
                            int g = Convert.ToInt32(values[2]);
                            int b = Convert.ToInt32(values[3]);

                            dictionary.Add(id, System.Drawing.Color.FromArgb(r, g, b));
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

        public static bool ImportValueColorDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, ValueColor> dictionary = container.ValueColorDictionary;

                string filePath = container.FilePath_ValueColor;
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
                            int numericValue = Convert.ToInt32(values[1]);
                            string textValue = values[2];
                            int colorId = Convert.ToInt32(values[3]);

                            var colorDictionary = container.ColorDictionary;

                            dictionary.Add(id, new ValueColor(numericValue, textValue, colorDictionary[colorId]));
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

        public static bool ImportEvaluationColorSet_ValueColorMappingListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Tuple<int, int>> list = container.EvaluationColorSet_ValueColorMappingList;

                string filePath = container.FilePath_EvaluationColorSet_ValueColor;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int colorSetId = Convert.ToInt32(values[0]);
                        int valueColorId = Convert.ToInt32(values[1]);

                        if (!list.Any(x => x.Item1 == colorSetId && x.Item2 == valueColorId))
                            list.Add(new Tuple<int, int>(colorSetId, valueColorId));
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

        public static bool ImportEvaluationColorSetDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, EvaluationColorSet> dictionary = container.EvaluationColorSetDictionary;

                string filePath = container.FilePath_EvaluationColorSet;
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

                            var valueColorDictionary = container.ValueColorDictionary;

                            List<ValueColor> valueColors = new List<ValueColor>();
                            foreach (var mapping in DataContainer.Instance.EvaluationColorSet_ValueColorMappingList.Where(x => x.Item1 == id))
                            {
                                valueColors.Add(valueColorDictionary[mapping.Item2]);
                            }

                            dictionary.Add(id, new EvaluationColorSet(name, valueColors));
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

        public static bool ImportQualitativeEvaluationCriterionDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Criterion_QualitativeEvaluation> dictionary = container.QualitativeEvaluationCriterionDictionary;

                string filePath = container.FilePath_QualitativeEvaluationCriterion;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int baseId = Convert.ToInt32(values[0]);

                        if (!dictionary.ContainsKey(baseId))
                        {
                            int evaluationColorSetId = Convert.ToInt32(values[1]);

                            var baseCriterionDictionary = container.BaseCriterionDictionary;
                            var evaluationColorSetDictionary = container.EvaluationColorSetDictionary;

                            dictionary.Add(baseId, new Criterion_QualitativeEvaluation(baseCriterionDictionary[baseId], evaluationColorSetDictionary[evaluationColorSetId]));
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

        public static bool ImportQualitativeEvaluationCriteria_CriterionMappingListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Tuple<int, int>> list = container.QualitativeEvaluationCriteria_CriterionMappingList;

                string filePath = container.FilePath_QualitativeEvaluationCriteria_Criterion;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int criteriaId = Convert.ToInt32(values[0]);
                        int criterionId = Convert.ToInt32(values[1]);

                        if (!list.Any(x => x.Item1 == criteriaId && x.Item2 == criterionId))
                            list.Add(new Tuple<int, int>(criteriaId, criterionId));
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

        public static bool ImportQualitativeEvaluationCriteriaDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Criteria_QualitativeEvaluation> dictionary = container.QualitativeEvaluationCriteriaDictionary;

                string filePath = container.FilePath_QualitativeEvaluationCriteria;
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

                            var criterionDictionary = container.QualitativeEvaluationCriterionDictionary;

                            List<Criterion_QualitativeEvaluation> criteria = new List<Criterion_QualitativeEvaluation>();
                            foreach (var mapping in DataContainer.Instance.QualitativeEvaluationCriteria_CriterionMappingList.Where(x => x.Item1 == id))
                            {
                                criteria.Add(criterionDictionary[mapping.Item2]);
                            }

                            dictionary.Add(id, new Criteria_QualitativeEvaluation(name, criteria));
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

        public static bool ImportQualitativeEvaluationCriteriaSetDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, CriteriaSet_QualitativeEvaluation> dictionary = container.QualitativeEvaluationCriteriaSetDictionary;

                string filePath = container.FilePath_QualitativeEvaluationCriteriaSet;
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
                            int firstPartialCriteriaId = Convert.ToInt32(values[2]);
                            int midtermCriteriaId = Convert.ToInt32(values[3]);
                            int finalCriteriaId = Convert.ToInt32(values[4]);

                            var criteriaDictionary = container.QualitativeEvaluationCriteriaDictionary;

                            dictionary.Add(id, new CriteriaSet_QualitativeEvaluation(name, (firstPartialCriteriaId > 0) ? criteriaDictionary[firstPartialCriteriaId] : null, criteriaDictionary[midtermCriteriaId], criteriaDictionary[finalCriteriaId]));
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

        public static bool ImportFullEvaluationCriteriaDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, FullEvaluationCriteria> dictionary = container.FullEvaluationCriteriaDictionary;

                string filePath = container.FilePath_FullEvaluationCriteria;
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
                            int quantitativeEvaluationCriteriaSetId = Convert.ToInt32(values[1]);
                            int qualitativeEvaluationCriteriaSetId = Convert.ToInt32(values[2]);

                            var quantitativeEvaluationCriteriaSetDictionary = container.QuantitativeEvaluationCriteriaSetDictionary;
                            var qualitativeEvaluationCriteriaSetDictionary = container.QualitativeEvaluationCriteriaSetDictionary;

                            dictionary.Add(id, new FullEvaluationCriteria(quantitativeEvaluationCriteriaSetDictionary[quantitativeEvaluationCriteriaSetId], qualitativeEvaluationCriteriaSetDictionary[qualitativeEvaluationCriteriaSetId]));
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

        #region Evaluation
        #region Quantitative Evaluation
        public static bool ImportQuantitativeEvaluationDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, QuantitativeEvaluation> dictionary = container.QuantitativeEvaluationDictionary;

                string filePath = container.FilePath_QuantitativeEvaluation;
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
                            int criterionId = Convert.ToInt32(values[1]);
                            decimal value = Convert.ToDecimal(values[2]);

                            var criterionDictionary = container.QuantitativeEvaluationCriterionDictionary;

                            dictionary.Add(id, new QuantitativeEvaluation(criterionDictionary[criterionId], value));
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

        public static bool ImportQuantitativeEvaluationSet_EvaluationMappingListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Tuple<int, int>> list = container.QuantitativeEvaluationSet_EvaluationMappingList;

                string filePath = container.FilePath_QuantitativeEvaluationSet_Evaluation;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int evaluationSetId = Convert.ToInt32(values[0]);
                        int evaluationId = Convert.ToInt32(values[1]);

                        if (!list.Any(x => x.Item1 == evaluationSetId && x.Item2 == evaluationId))
                            list.Add(new Tuple<int, int>(evaluationSetId, evaluationId));
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

        public static bool ImportQuantitativeEvaluationSetDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, List<QuantitativeEvaluation>> dictionary = container.QuantitativeEvaluationSetDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationSet;
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
                            var evaluationDictionary = container.QuantitativeEvaluationDictionary;

                            List<QuantitativeEvaluation> evaluationSet = new List<QuantitativeEvaluation>();
                            foreach (var mapping in DataContainer.Instance.QuantitativeEvaluationSet_EvaluationMappingList.Where(x => x.Item1 == id))
                            {
                                evaluationSet.Add(evaluationDictionary[mapping.Item2]);
                            }

                            dictionary.Add(id, evaluationSet);
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

        public static bool ImportQuantitativeEvaluationSetCollectionDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, EvaluationSetCollection_Quantitative> dictionary = container.QuantitativeEvaluationSetCollectionDictionary;

                string filePath = container.FilePath_QuantitativeEvaluationSetCollection;
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
                            int firstPartialEvaluationSetId = Convert.ToInt32(values[1]);
                            int midtermEvaluationSetId = Convert.ToInt32(values[2]);
                            int finalEvaluationSetId = Convert.ToInt32(values[3]);
                            int additionalEvaluationSetId = Convert.ToInt32(values[4]);

                            var evaluationSetDictionary = container.QuantitativeEvaluationSetDictionary;

                            dictionary.Add(id, new EvaluationSetCollection_Quantitative((firstPartialEvaluationSetId > 0) ? evaluationSetDictionary[firstPartialEvaluationSetId] : null, evaluationSetDictionary[midtermEvaluationSetId], evaluationSetDictionary[finalEvaluationSetId], evaluationSetDictionary[additionalEvaluationSetId]));
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

        #region Qualitative Evaluation
        public static bool ImportQualitativeEvaluationDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, QualitativeEvaluation> dictionary = container.QualitativeEvaluationDictionary;

                string filePath = container.FilePath_QualitativeEvaluation;
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
                            int criterionId = Convert.ToInt32(values[1]);
                            int value = Convert.ToInt32(values[2]);

                            var criterionDictionary = container.QualitativeEvaluationCriterionDictionary;

                            dictionary.Add(id, new QualitativeEvaluation(criterionDictionary[criterionId], value));
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

        public static bool ImportQualitativeEvaluationSet_EvaluationMappingListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Tuple<int, int>> list = container.QualitativeEvaluationSet_EvaluationMappingList;

                string filePath = container.FilePath_QualitativeEvaluationSet_Evaluation;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int evaluationSetId = Convert.ToInt32(values[0]);
                        int evaluationId = Convert.ToInt32(values[1]);

                        if (!list.Any(x => x.Item1 == evaluationSetId && x.Item2 == evaluationId))
                            list.Add(new Tuple<int, int>(evaluationSetId, evaluationId));
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

        public static bool ImportQualitativeEvaluationSetDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, List<QualitativeEvaluation>> dictionary = container.QualitativeEvaluationSetDictionary;

                string filePath = container.FilePath_QualitativeEvaluationSet;
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
                            var evaluationDictionary = container.QualitativeEvaluationDictionary;

                            List<QualitativeEvaluation> evaluationSet = new List<QualitativeEvaluation>();
                            foreach (var mapping in DataContainer.Instance.QualitativeEvaluationSet_EvaluationMappingList.Where(x => x.Item1 == id))
                            {
                                evaluationSet.Add(evaluationDictionary[mapping.Item2]);
                            }

                            dictionary.Add(id, evaluationSet);
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

        public static bool ImportQualitativeEvaluationSetCollectionDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, EvaluationSetCollection_Qualitative> dictionary = container.QualitativeEvaluationSetCollectionDictionary;

                string filePath = container.FilePath_QualitativeEvaluationSetCollection;
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
                            int firstPartialEvaluationSetId = Convert.ToInt32(values[1]);
                            int midtermEvaluationSetId = Convert.ToInt32(values[2]);
                            int finalEvaluationSetId = Convert.ToInt32(values[3]);

                            var evaluationSetDictionary = container.QualitativeEvaluationSetDictionary;

                            dictionary.Add(id, new EvaluationSetCollection_Qualitative((firstPartialEvaluationSetId > 0) ? evaluationSetDictionary[firstPartialEvaluationSetId] : null, evaluationSetDictionary[midtermEvaluationSetId], evaluationSetDictionary[finalEvaluationSetId]));
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

        public static bool ImportFullEvaluationDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, FullEvaluation> dictionary = container.FullEvaluationDictionary;

                string filePath = container.FilePath_FullEvaluation;
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
                            int quantitativeEvaluationSetId = Convert.ToInt32(values[1]);
                            int qualitativeEvaluationSetId = Convert.ToInt32(values[2]);

                            var quantitativeEvaluationSetCollectionDictionary = container.QuantitativeEvaluationSetCollectionDictionary;
                            var qualitativeEvaluationSetCollectionDictionary = container.QualitativeEvaluationSetCollectionDictionary;

                            dictionary.Add(id, new FullEvaluation(quantitativeEvaluationSetCollectionDictionary[quantitativeEvaluationSetId], qualitativeEvaluationSetCollectionDictionary[qualitativeEvaluationSetId]));
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

        public static bool ImportStudentInfoDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, StudentInfo> dictionary = container.StudentInfoDictionary;

                string filePath = container.FilePath_StudentInfo;
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
                            int studentAccountNumber = Convert.ToInt32(values[1]);
                            int fullEvaluationId = Convert.ToInt32(values[2]);

                            var studentList = container.StudentList;
                            var fullEvaluationDictionary = container.FullEvaluationDictionary;

                            dictionary.Add(id, new StudentInfo(studentList.First(x => x.AccountNumber == studentAccountNumber), fullEvaluationDictionary[fullEvaluationId]));
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

        public static bool ImportCourseCategoryDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, eCourseCategory> dictionary = container.CourseCategoryDictionary;

                string filePath = container.FilePath_CourseCategory;
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

                            dictionary.Add(id, name.ToCorrespondingEnumValue<eCourseCategory>());
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

        public static bool ImportCourseBaseListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<CourseBase> list = container.CourseBaseList;

                string filePath = container.FilePath_CourseBase;
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

                        if (!list.Any(x => x.Id == id))
                        {
                            string name = values[1];
                            int categoryId = Convert.ToInt32(values[2]);

                            eCourseCategory category = container.CourseCategoryDictionary[categoryId];

                            list.Add(new CourseBase(id, name, category));
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

        public static bool ImportCourseDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Course> dictionary = container.CourseDictionary;

                string filePath = container.FilePath_Course;
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
                            int baseId = Convert.ToInt32(values[1]);
                            int semesterId = Convert.ToInt32(values[2]);
                            int fullEvaluationCriteriaId = Convert.ToInt32(values[3]);

                            var courseBaseList = container.CourseBaseList;
                            var semesterDictionary = container.SemesterDictionary;
                            var fullEvaluationCriteriaDictionary = container.FullEvaluationCriteriaDictionary;

                            dictionary.Add(id, new Course(courseBaseList.First(x => x.Id == baseId), semesterDictionary[semesterId], fullEvaluationCriteriaDictionary[fullEvaluationCriteriaId]));
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

        public static bool ImportCourseGroup_StudentInfoMappingListFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                List<Tuple<int, int>> list = container.CourseGroup_StudentInfoMappingList;

                string filePath = container.FilePath_CourseGroup_StudentInfo;
                using (var reader = new StreamReader(filePath))
                {
                    // If there is at least a row, ignore the first row, which includes the column titles.
                    if (!reader.EndOfStream)
                        reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        int groupId = Convert.ToInt32(values[0]);
                        int studentInfoId = Convert.ToInt32(values[1]);

                        if (!list.Any(x => x.Item1 == groupId && x.Item2 == studentInfoId))
                            list.Add(new Tuple<int, int>(groupId, studentInfoId));
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

        public static bool ImportCourseGroupDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, CourseGroup> dictionary = container.CourseGroupDictionary;

                string filePath = container.FilePath_CourseGroup;
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
                            int courseId = Convert.ToInt32(values[1]);
                            string name = values[2];
                            int assignedFacultyId = Convert.ToInt32(values[3]);

                            var courseDictionary = container.CourseDictionary;
                            var facultyList = container.FacultyList;

                            var studentInfoDictionary = container.StudentInfoDictionary;

                            List<StudentInfo> studentInfos = new List<StudentInfo>();
                            foreach (var mapping in DataContainer.Instance.CourseGroup_StudentInfoMappingList.Where(x => x.Item1 == id))
                            {
                                var studentInfo = studentInfoDictionary[mapping.Item2];
                                studentInfos.Add(studentInfo);
                            }

                            dictionary.Add(id, new CourseGroup(courseDictionary[courseId], name, facultyList.FirstOrDefault(x => x.AccountNumber == assignedFacultyId), studentInfos));
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
                            DateTime date = DateTime.Parse(values[1]);
                            eExamType type = values[2].ToCorrespondingEnumValue<eExamType>();

                            dictionary.Add(id, new NonInstitutionalExam_Internal(date, type));
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

        //public static bool ImportNonInstitutionalExamDictionaryFromCSV()
        //{
        //    try
        //    {
        //        DataContainer container = DataContainer.Instance;
        //        var dictionary = container.NonInstitutionalExamDictionary;

        //        string filePath = container.FilePath_NonInstitutionalExam;
        //        using (var reader = new StreamReader(filePath))
        //        {
        //            // If there is at least a row, ignore the first row, which includes the column titles.
        //            if (!reader.EndOfStream)
        //                reader.ReadLine();

        //            while (!reader.EndOfStream)
        //            {
        //                string line = reader.ReadLine();
        //                string[] values = line.Split(';');

        //                int id = Convert.ToInt32(values[0]);

        //                if (!dictionary.Any(x => x.Key == id))
        //                {
        //                    DateTime date = DateTime.Parse(values[1]);
        //                    eExamType type = values[2].ToCorrespondingEnumValue<eExamType>();
        //                    Student examinee = container.StudentList.First(x => x.AccountNumber == Convert.ToInt32(values[3]));
        //                    int score = Convert.ToInt32(values[4]);

        //                    dictionary.Add(id, new NonInstitutionalExam(date, type, examinee, score));
        //                }
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to import data from CSV file: " + ex.Message);
        //        return false;
        //    }
        //}

        public static bool ImportColumnRangeDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var dictionary = container.ColumnRangeDictionary;

                string filePath = container.FilePath_ColumnRange;
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
                            string startColumnLetter = values[1];
                            string endColumnLetter = values[2];

                            dictionary.Add(id, new ColumnRange(startColumnLetter, endColumnLetter));
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

        public static bool ImportEvaluationFileCriterionColumnsRangeSetDictionaryFromCSV()
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var dictionary = container.EvaluationFileCriterionColumnsRangeSetDictionary;
                var columnRangeDictionary = container.ColumnRangeDictionary;

                string filePath = container.FilePath_EvaluationFileCriterionColumnsRangeSet;
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
                            string name = values[1];
                            int qntFPColumnRangeId = Convert.ToInt32(values[2]);
                            int qntMidColumnRangeId = Convert.ToInt32(values[3]);
                            int qntFinColumnRangeId = Convert.ToInt32(values[4]);
                            string qntAddColumnsString = values[5];
                            int qlFPColumnRangeId = Convert.ToInt32(values[6]);
                            int qlMidColumnRangeId = Convert.ToInt32(values[7]);
                            int qlFinColumnRangeId = Convert.ToInt32(values[8]);

                            ColumnRange? columnRange_qntFP = null;
                            if (qntFPColumnRangeId > 0)
                                columnRange_qntFP = columnRangeDictionary[qntFPColumnRangeId];
                            ColumnRange? columnRange_qlFP = null;
                            if (qlFPColumnRangeId > 0)
                                columnRange_qlFP = columnRangeDictionary[qlFPColumnRangeId];

                            List<string> qntAddColumns = qntAddColumnsString.Split(',').ToList();

                            dictionary.Add(id, new EvaluationFileCriterionColumnsRangeSet(name, columnRange_qntFP, columnRangeDictionary[qntMidColumnRangeId], columnRangeDictionary[qntFinColumnRangeId], qntAddColumns, columnRange_qlFP, columnRangeDictionary[qlMidColumnRangeId], columnRangeDictionary[qlFinColumnRangeId]));
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

        public static bool AddFacultyToCSV(string _accountNumber, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, string> nameDictionary = container.NameDictionary;
                Dictionary<int, string> surnameDictionary = container.SurnameDictionary;
                Dictionary<int, FullName> fullNameDictionary = container.FullNameDictionary;
                Dictionary<int, ePermissionsType> permissionTypeDictionary = container.PermissionsTypeDictionary;

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

                string defaultUesername = "username";
                string defaultPassword = "password";
                ePermissionsType defaultPermissionType = ePermissionsType.AssignedCoursesOnly;

                int permissionTypeId = permissionTypeDictionary.GetFirstKey(defaultPermissionType);

                string filePath = container.FilePath_Faculty;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4}",
                                _accountNumber,
                                fullNameId,
                                defaultUesername,
                                defaultPassword,
                                permissionTypeId.ToString());
                }

                DataContainer.Instance.FacultyList.Add(new Faculty(Convert.ToInt32(_accountNumber), fullNameDictionary[fullNameId], defaultUesername, defaultPassword, defaultPermissionType)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
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
            return AddStudentToCSV(_accountNumber, _firstName, _middleName, _paternalSurname, _maternalSurname, null, null, null, null, null);
        }
        public static bool AddStudentToCSV(string _accountNumber, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname, string _organizationEmail, string _preferredEmail, string _phone, Major _major, Semester _semester)
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

                string majorIdString = (_major != null) ? _major.Id.ToString() : "";

                string semesterIdString = (_semester != null) ? semesterDictionary.GetFirstKey(_semester).ToString() : "";

                string filePath = container.FilePath_Student;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5};{6}",
                                _accountNumber,
                                fullNameId.ToString(),
                                _organizationEmail ?? "",
                                _preferredEmail ?? "",
                                _phone ?? "",
                                majorIdString,
                                semesterIdString);
                }

                DataContainer.Instance.StudentList.Add(new Student(Convert.ToInt32(_accountNumber), fullNameDictionary[fullNameId])); // Update application-side data

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

        public static int AddEvaluationColorSetToCSV(string _name, List<Tuple<int, string, System.Drawing.Color>> _valueColors)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, ValueColor> valueColorDictionary = container.ValueColorDictionary;

                List<ValueColor> valueColors = new List<ValueColor>();
                foreach (var valueColor in _valueColors)
                {
                    int valueColorId = valueColorDictionary.FirstOrDefault(x => x.Value.NumericValue == valueColor.Item1
                                                                                && x.Value.TextValue == valueColor.Item2
                                                                                && x.Value.Color == valueColor.Item3).Key;
                    if (valueColorId == default) // If the item does not exist
                    {
                        valueColorId = AddValueColorToCSV(valueColor.Item1, valueColor.Item2, valueColor.Item3);
                        if (valueColorId == default)
                            return default;
                    }

                    valueColors.Add(container.ValueColorDictionary[valueColorId]);
                }

                Dictionary<int, EvaluationColorSet> evaluationColorSetDictionary = container.EvaluationColorSetDictionary;

                int lastId = (evaluationColorSetDictionary.Count != 0) ? evaluationColorSetDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_EvaluationColorSet;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _name);
                }

                if (!AddEvaluationColorSet_ValueColorMappingsToCSV(newId, valueColors))
                    return default;

                evaluationColorSetDictionary.Add(newId, new EvaluationColorSet(_name, valueColors)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static int AddValueColorToCSV(int _numericValue, string _textValue, System.Drawing.Color _color)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, System.Drawing.Color> colorDictionary = container.ColorDictionary;

                int colorId = colorDictionary.GetFirstKeyOrDefault(_color);
                if (colorId == default) // If the item does not exist
                {
                    colorId = AddColorToCSV(_color);
                    if (colorId == default)
                        return default;
                }

                Dictionary<int, ValueColor> valueColorDictionary = container.ValueColorDictionary;

                int lastId = (valueColorDictionary.Count != 0) ? valueColorDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_ValueColor;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                    newId.ToString(),
                                    _numericValue.ToString(),
                                    _textValue,
                                    colorId.ToString());
                }

                valueColorDictionary.Add(newId, new ValueColor(_numericValue, _textValue, _color)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }
        private static int AddColorToCSV(System.Drawing.Color _color)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, System.Drawing.Color> colorDictionary = container.ColorDictionary;

                int lastId = (colorDictionary.Count != 0) ? colorDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_Color;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                    newId.ToString(),
                                    _color.R.ToString(),
                                    _color.G.ToString(),
                                    _color.B.ToString());
                }

                colorDictionary.Add(newId, _color); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static bool AddEvaluationColorSet_ValueColorMappingsToCSV(int _evaluationColorSetId, List<ValueColor> _valueColors)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var valueColorDictionary = container.ValueColorDictionary;
                var mappingList = container.EvaluationColorSet_ValueColorMappingList;

                List<int> valueColorIds = new List<int>();
                foreach (var valueColor in _valueColors)
                {
                    valueColorIds.Add(valueColorDictionary.GetFirstKey(valueColor));
                }

                string filePath = container.FilePath_EvaluationColorSet_ValueColor;
                using (var sw = new StreamWriter(filePath, true))
                {
                    foreach (var valueColorId in valueColorIds)
                    {
                        sw.WriteLine("{0};{1}", _evaluationColorSetId.ToString(), valueColorId.ToString());
                    }
                }

                // Update application-side data
                {
                    foreach (var valueColorId in valueColorIds)
                    {
                        mappingList.Add(new Tuple<int, int>(_evaluationColorSetId, valueColorId));
                    }
                }

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static int AddQuantitativeEvaluationCriterionToCSV(string _string, eValueType _evaluationValueType, decimal _min, decimal _max)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, eValueType> valueTypeDictionary = container.ValueTypeDictionary;

                int evaluationValueTypeId = valueTypeDictionary.GetFirstKey(_evaluationValueType);

                Dictionary<int, string> baseCriterionDictionary = container.BaseCriterionDictionary;
                Dictionary<int, Criterion_QuantitativeEvaluation> criterionDictionary = container.QuantitativeEvaluationCriterionDictionary;

                int lastId = (baseCriterionDictionary.Count != 0) ? baseCriterionDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath_base = container.FilePath_BaseCriterion;
                using (var sw = new StreamWriter(filePath_base, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _string);
                }
                string filePath_criterion = container.FilePath_QuantitativeEvaluationCriterion;
                using (var sw = new StreamWriter(filePath_criterion, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                        newId.ToString(),
                                        evaluationValueTypeId.ToString(),
                                        _min.ToString(),
                                        _max.ToString());
                }

                baseCriterionDictionary.Add(newId, _string);
                criterionDictionary.Add(newId, new Criterion_QuantitativeEvaluation(_string, _evaluationValueType, _min, _max)); // Update application-side data

                container.SetFileModificationStatus(filePath_base, true);
                container.SetFileModificationStatus(filePath_criterion, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQuantitativeEvaluationCriteriaToCSV(string _name, List<Tuple<Criterion_QuantitativeEvaluation, decimal>> _criterionWeights)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, CriterionWeight_QuantitativeEvaluation> criterionWeightDictionary = container.QuantitativeEvaluationCriterionWeightDictionary;

                List<CriterionWeight_QuantitativeEvaluation> criterionWeights = new List<CriterionWeight_QuantitativeEvaluation>();
                foreach (var criterionWeight in _criterionWeights)
                {
                    int criterionWeightId = criterionWeightDictionary.FirstOrDefault(x => x.Value.Criterion == criterionWeight.Item1
                                                                                        && x.Value.Weight == criterionWeight.Item2).Key;
                    if (criterionWeightId == default) // If the item does not exist
                    {
                        criterionWeightId = AddQuantitativeEvaluationCriterionWeightToCSV(criterionWeight.Item1, criterionWeight.Item2);
                        if (criterionWeightId == default)
                            return default;
                    }

                    criterionWeights.Add(container.QuantitativeEvaluationCriterionWeightDictionary[criterionWeightId]);
                }

                Dictionary<int, Criteria_QuantitativeEvaluation> criteriaDictionary = container.QuantitativeEvaluationCriteriaDictionary;

                int lastId = (criteriaDictionary.Count != 0) ? criteriaDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluationCriteria;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _name);
                }

                if (!AddQuantitativeEvaluationCriteria_CriterionWeightMappingsToCSV(newId, criterionWeights))
                    return default;

                criteriaDictionary.Add(newId, new Criteria_QuantitativeEvaluation(_name, criterionWeights)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static int AddQuantitativeEvaluationCriterionWeightToCSV(Criterion_QuantitativeEvaluation _criterion, decimal _weight)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionDictionary = container.QuantitativeEvaluationCriterionDictionary;

                int criterionId = criterionDictionary.GetFirstKey(_criterion);

                var criterionWeightDictionary = container.QuantitativeEvaluationCriterionWeightDictionary;

                int lastId = (criterionWeightDictionary.Count != 0) ? criterionWeightDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluationCriterionWeight;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    criterionId.ToString(),
                                    _weight.ToString());
                }

                criterionWeightDictionary.Add(newId, new CriterionWeight_QuantitativeEvaluation(_criterion, _weight)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static bool AddQuantitativeEvaluationCriteria_CriterionWeightMappingsToCSV(int _criteriaId, List<CriterionWeight_QuantitativeEvaluation> _criterionWeights)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionWeightDictionary = container.QuantitativeEvaluationCriterionWeightDictionary;
                var mappingList = container.EvaluationColorSet_ValueColorMappingList;

                List<int> criterionWeightIds = new List<int>();
                foreach (var criterionWeight in _criterionWeights)
                {
                    criterionWeightIds.Add(criterionWeightDictionary.GetFirstKey(criterionWeight));
                }

                string filePath = container.FilePath_QuantitativeEvaluationCriteria_CriterionWeight;
                using (var sw = new StreamWriter(filePath, true))
                {
                    foreach (var criterionWeightId in criterionWeightIds)
                    {
                        sw.WriteLine("{0};{1}", _criteriaId.ToString(), criterionWeightId.ToString());
                    }
                }

                // Update application-side data
                {
                    foreach (var criterionWeightId in criterionWeightIds)
                    {
                        mappingList.Add(new Tuple<int, int>(_criteriaId, criterionWeightId));
                    }
                }

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static int AddQuantitativeEvaluationCriteriaSetToCSV(string _name, Tuple<Criteria_QuantitativeEvaluation, decimal> _firstPartialCriteriaWeight, Tuple<Criteria_QuantitativeEvaluation, decimal> _midtermCriteriaWeight, Tuple<Criteria_QuantitativeEvaluation, decimal> _finalCriteriaWeight, Tuple<Criteria_QuantitativeEvaluation, decimal> _additionalCriteriaWeight)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criteriaDictionary = container.QuantitativeEvaluationCriteriaDictionary;
                var criteriaWeightDictionary = container.QuantitativeEvaluationCriteriaWeightDictionary;

                int firstPartialCriteriaWeightId = default;
                if (_firstPartialCriteriaWeight.Item1 != null)
                {
                    firstPartialCriteriaWeightId = criteriaWeightDictionary.GetFirstKeyOrDefault(x => x.Value.Criteria == _firstPartialCriteriaWeight.Item1
                                                                    && x.Value.Weight == _firstPartialCriteriaWeight.Item2);
                    if (firstPartialCriteriaWeightId == default) // If the item does not exist
                    {
                        firstPartialCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_firstPartialCriteriaWeight.Item1, _firstPartialCriteriaWeight.Item2);
                        if (firstPartialCriteriaWeightId == default)
                            return default;
                    }
                }

                int midtermCriteriaWeightId = criteriaWeightDictionary.GetFirstKeyOrDefault(x => x.Value.Criteria == _midtermCriteriaWeight.Item1
                                                                    && x.Value.Weight == _midtermCriteriaWeight.Item2);
                if (midtermCriteriaWeightId == default) // If the item does not exist
                {
                    midtermCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_midtermCriteriaWeight.Item1, _midtermCriteriaWeight.Item2);
                    if (midtermCriteriaWeightId == default)
                        return default;
                }

                int finalCriteriaWeightId = criteriaWeightDictionary.GetFirstKeyOrDefault(x => x.Value.Criteria == _finalCriteriaWeight.Item1
                                                                    && x.Value.Weight == _finalCriteriaWeight.Item2);
                if (finalCriteriaWeightId == default) // If the item does not exist
                {
                    finalCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_finalCriteriaWeight.Item1, _finalCriteriaWeight.Item2);
                    if (finalCriteriaWeightId == default)
                        return default;
                }

                int additionalCriteriaWeightId = criteriaWeightDictionary.GetFirstKeyOrDefault(x => x.Value.Criteria == _additionalCriteriaWeight.Item1
                                                                    && x.Value.Weight == _additionalCriteriaWeight.Item2);
                if (additionalCriteriaWeightId == default) // If the item does not exist
                {
                    additionalCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_additionalCriteriaWeight.Item1, _additionalCriteriaWeight.Item2);
                    if (additionalCriteriaWeightId == default)
                        return default;
                }

                Dictionary<int, CriteriaSet_QuantitativeEvaluation> criteriaSetDictionary = container.QuantitativeEvaluationCriteriaSetDictionary;

                int lastId = (criteriaSetDictionary.Count != 0) ? criteriaSetDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluationCriteriaSet;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5}",
                                    newId.ToString(),
                                    _name,
                                    firstPartialCriteriaWeightId.ToString(),
                                    midtermCriteriaWeightId.ToString(),
                                    finalCriteriaWeightId.ToString(),
                                    additionalCriteriaWeightId.ToString());
                }

                criteriaSetDictionary.Add(newId, new CriteriaSet_QuantitativeEvaluation(_name, (firstPartialCriteriaWeightId > 0) ? criteriaWeightDictionary[firstPartialCriteriaWeightId] : null, criteriaWeightDictionary[midtermCriteriaWeightId], criteriaWeightDictionary[finalCriteriaWeightId], criteriaWeightDictionary[additionalCriteriaWeightId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static int AddQuantitativeEvaluationCriteriaWeightToCSV(Criteria_QuantitativeEvaluation _criteria, decimal _weight)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criteriaDictionary = container.QuantitativeEvaluationCriteriaDictionary;

                int criteriaId = criteriaDictionary.GetFirstKey(_criteria);

                var criteriaWeightDictionary = container.QuantitativeEvaluationCriteriaWeightDictionary;

                int lastId = (criteriaWeightDictionary.Count != 0) ? criteriaWeightDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluationCriteriaWeight;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    criteriaId.ToString(),
                                    _weight.ToString());
                }

                criteriaWeightDictionary.Add(newId, new CriteriaWeight_QuantitativeEvaluation(_criteria, _weight)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQualitativeEvaluationCriterionToCSV(string _string, EvaluationColorSet _evaluationColorSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, EvaluationColorSet> evaluationColorSetDictionary = container.EvaluationColorSetDictionary;

                int evaluationColorSetId = evaluationColorSetDictionary.GetFirstKey(_evaluationColorSet);

                Dictionary<int, string> baseCriterionDictionary = container.BaseCriterionDictionary;
                Dictionary<int, Criterion_QualitativeEvaluation> criterionDictionary = container.QualitativeEvaluationCriterionDictionary;

                int lastId = (baseCriterionDictionary.Count != 0) ? baseCriterionDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath_base = container.FilePath_BaseCriterion;
                using (var sw = new StreamWriter(filePath_base, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _string);
                }
                string filePath_criterion = container.FilePath_QualitativeEvaluationCriterion;
                using (var sw = new StreamWriter(filePath_criterion, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), evaluationColorSetId.ToString());
                }

                baseCriterionDictionary.Add(newId, _string);
                criterionDictionary.Add(newId, new Criterion_QualitativeEvaluation(_string, _evaluationColorSet)); // Update application-side data

                container.SetFileModificationStatus(filePath_base, true);
                container.SetFileModificationStatus(filePath_criterion, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQualitativeEvaluationCriteriaToCSV(string _name, List<Criterion_QualitativeEvaluation> _criterionList)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                Dictionary<int, Criteria_QualitativeEvaluation> criteriaDictionary = container.QualitativeEvaluationCriteriaDictionary;

                int lastId = (criteriaDictionary.Count != 0) ? criteriaDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QualitativeEvaluationCriteria;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1}", newId.ToString(), _name);
                }

                if (!AddQualitativeEvaluationCriteria_CriterionMappingsToCSV(newId, _criterionList))
                    return default;

                criteriaDictionary.Add(newId, new Criteria_QualitativeEvaluation(_name, _criterionList)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static bool AddQualitativeEvaluationCriteria_CriterionMappingsToCSV(int _criteriaId, List<Criterion_QualitativeEvaluation> _criterionList)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionDictionary = container.QualitativeEvaluationCriterionDictionary;
                var mappingList = container.QualitativeEvaluationCriteria_CriterionMappingList;

                List<int> criterionIds = new List<int>();
                foreach (var criterion in _criterionList)
                {
                    criterionIds.Add(criterionDictionary.GetFirstKey(criterion));
                }

                string filePath = container.FilePath_QualitativeEvaluationCriteria_Criterion;
                using (var sw = new StreamWriter(filePath, true))
                {
                    foreach (var criterionId in criterionIds)
                    {
                        sw.WriteLine("{0};{1}", _criteriaId.ToString(), criterionId.ToString());
                    }
                }

                // Update application-side data
                {
                    foreach (var valueColorId in criterionIds)
                    {
                        mappingList.Add(new Tuple<int, int>(_criteriaId, valueColorId));
                    }
                }

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static int AddQualitativeEvaluationCriteriaSetToCSV(string _name, Criteria_QualitativeEvaluation _firstPartialCriteria, Criteria_QualitativeEvaluation _midtermCriteria, Criteria_QualitativeEvaluation _finalCriteria)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criteriaDictionary = container.QualitativeEvaluationCriteriaDictionary;
                var criteriaSetDictionary = container.QualitativeEvaluationCriteriaSetDictionary;

                int firstPartialCriteriaId = criteriaDictionary.GetFirstKeyOrDefault(_firstPartialCriteria);
                int midtermCriteriaId = criteriaDictionary.GetFirstKey(_midtermCriteria);
                int finalCriteriaId = criteriaDictionary.GetFirstKey(_finalCriteria);

                int lastId = (criteriaSetDictionary.Count != 0) ? criteriaSetDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QualitativeEvaluationCriteriaSet;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4}",
                                    newId.ToString(),
                                    _name,
                                    firstPartialCriteriaId.ToString(),
                                    midtermCriteriaId.ToString(),
                                    finalCriteriaId.ToString());
                }

                criteriaSetDictionary.Add(newId, new CriteriaSet_QualitativeEvaluation(_name, _firstPartialCriteria, _midtermCriteria, _finalCriteria)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddFullEvaluationCriteriaToCSV(int _quantitativeEvaluationCriteriaSetId, int _qualitativeEvaluationCriteriaSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var quantitativeEvaluationCriteriaSetDictionary = container.QuantitativeEvaluationCriteriaSetDictionary;
                var qualitativeEvaluationCriteriaSetDictionary = container.QualitativeEvaluationCriteriaSetDictionary;
                var fullEvaluationCriteriaDictionary = container.FullEvaluationCriteriaDictionary;

                int lastId = (fullEvaluationCriteriaDictionary.Count != 0) ? fullEvaluationCriteriaDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_FullEvaluationCriteria;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    _quantitativeEvaluationCriteriaSetId.ToString(),
                                    _qualitativeEvaluationCriteriaSetId.ToString());
                }

                fullEvaluationCriteriaDictionary.Add(newId, new FullEvaluationCriteria(quantitativeEvaluationCriteriaSetDictionary[_quantitativeEvaluationCriteriaSetId], qualitativeEvaluationCriteriaSetDictionary[_qualitativeEvaluationCriteriaSetId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static bool AddCourseBaseToCSV(int _id, string _name, eCourseCategory _category)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var courseBaseList = container.CourseBaseList;
                var courseCategoryDictionary = container.CourseCategoryDictionary;


                string filePath = container.FilePath_CourseBase;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}", _id.ToString(), _name, courseCategoryDictionary.GetFirstKey(_category).ToString());
                }

                courseBaseList.Add(new CourseBase(_id, _name, _category)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static int AddCourseToCSV(CourseBase _courseBase, int _numOfGroups, eGroupNamingFormat _groupNamingFormat, eTerm _term, int _year, CriteriaSet_QuantitativeEvaluation _quantitativeEvaluationCriteriaSet, CriteriaSet_QualitativeEvaluation _qualitativeEvaluationCriteriaSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var courseBaseList = container.CourseBaseList;
                var courseGroupDictionary = container.CourseGroupDictionary;
                var yearDictionary = container.YearDictionary;
                var termDictionary = container.TermDictionary;
                var semesterDictionary = container.SemesterDictionary;
                var fullEvaluationCriteriaDictionary = container.FullEvaluationCriteriaDictionary;

                int yearId = yearDictionary.GetFirstKeyOrDefault(_year);
                if (yearId == default) // If the item does not exist
                {
                    yearId = AddYearToCSV(_year);
                    if (yearId == default)
                        return default;
                }

                int termId = termDictionary.GetFirstKey(_term);

                int semesterId = semesterDictionary.FirstOrDefault(x => x.Value.Year == _year
                                                                        && x.Value.Term == _term).Key;
                if (semesterId == default) // If the item does not exist
                {
                    semesterId = AddSemesterToCSV(yearId, termId);
                    if (semesterId == default)
                        return default;
                }

                int fullEvaluationCriteriaId = fullEvaluationCriteriaDictionary.FirstOrDefault(x => x.Value.CriteriaSet_QuantitativeEvaluation == _quantitativeEvaluationCriteriaSet
                                                                                                    && x.Value.CriteriaSet_QualitativeEvaluation == _qualitativeEvaluationCriteriaSet).Key;
                if (fullEvaluationCriteriaId == default) // If the item does not exist
                {
                    int quantitativeEvaluationCriteriaSetId = container.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value == _quantitativeEvaluationCriteriaSet).Key;
                    int qualitativeEvaluationCriteriaSetId = container.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value == _qualitativeEvaluationCriteriaSet).Key;
                    fullEvaluationCriteriaId = AddFullEvaluationCriteriaToCSV(quantitativeEvaluationCriteriaSetId, qualitativeEvaluationCriteriaSetId);
                    if (fullEvaluationCriteriaId == default)
                        return default;
                }

                var courseDictionary = container.CourseDictionary;

                int lastId = (courseDictionary.Count != 0) ? courseDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_Course;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                    newId.ToString(),
                                    _courseBase.Id.ToString(),
                                    semesterId.ToString(),
                                    fullEvaluationCriteriaId.ToString());
                }

                courseDictionary.Add(newId, new Course(_courseBase, semesterDictionary[semesterId], fullEvaluationCriteriaDictionary[fullEvaluationCriteriaId])); // Update application-side data

                for (int i = 0; i < _numOfGroups; i++)
                {
                    string groupName = string.Empty;
                    switch (_groupNamingFormat)
                    {
                        case eGroupNamingFormat.Alphabet:
                            groupName = ((char)(i + 65)).ToString();
                            break;

                        case eGroupNamingFormat.Number:
                            groupName = (i + 1).ToString();
                            break;
                    }

                    int groupId = AddCourseGroupToCSV(newId, groupName);
                    if (groupId == default)
                        return default;
                }

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddCourseGroupToCSV(int _courseId, string _name, List<Student> _studentList = null, Faculty _faculty = null)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var courseDictionary = container.CourseDictionary;
                var groupStudentInfoMappingList = container.CourseGroup_StudentInfoMappingList;
                var studentInfoDictionary = container.StudentInfoDictionary;

                var course = courseDictionary[_courseId];

                var courseGroupDictionary = container.CourseGroupDictionary;

                int lastId = (courseGroupDictionary.Count != 0) ? courseGroupDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_CourseGroup;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                    newId.ToString(),
                                    _courseId.ToString(),
                                    _name,
                                    (_faculty != null ? _faculty.AccountNumber : default).ToString());
                }

                List<StudentInfo> studentInfos = new List<StudentInfo>();
                if (_studentList != null)
                {
                    if (!AddCourseGroup_StudentInfoMappingsToCSV(newId, _studentList, course.FullEvaluationCriteria))
                        return default;

                    var mappingList = groupStudentInfoMappingList.Where(x => x.Item1 == newId).ToList();

                    foreach (var element in mappingList)
                    {
                        studentInfos.Add(studentInfoDictionary[element.Item2]);
                    }
                }

                courseGroupDictionary.Add(newId, new CourseGroup(courseDictionary[_courseId], _name, _faculty, studentInfos)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static bool AddCourseGroup_StudentInfoMappingsToCSV(int _groupId, List<Student> _studentList, FullEvaluationCriteria _fullEvaluationCriteria)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var groupDictionary = container.CourseGroupDictionary;
                var mappingList = container.CourseGroup_StudentInfoMappingList;
                var studentInfoDictionary = container.StudentInfoDictionary;

                var group = groupDictionary[_groupId];

                List<int> studentInfoIds = new List<int>();
                foreach (var student in _studentList)
                {
                    int studentInfoId = default;
                    if (!(group.StudentInfos.Any(x => x.Student == student)))
                    {
                        studentInfoId = AddStudentInfoToCSV(student, _fullEvaluationCriteria);
                        if (studentInfoId == default)
                            return false;
                    }
                    else
                        studentInfoId = studentInfoDictionary.GetFirstKey(group.StudentInfos.First(x => x.Student == student));

                    studentInfoIds.Add(studentInfoId);
                }

                string filePath = container.FilePath_CourseGroup_StudentInfo;
                using (var sw = new StreamWriter(filePath, true))
                {
                    foreach (var studentInfoId in studentInfoIds)
                    {
                        sw.WriteLine("{0};{1}", _groupId.ToString(), studentInfoId.ToString());
                    }
                }

                // Update application-side data
                {
                    foreach (var studentInfoId in studentInfoIds)
                    {
                        mappingList.Add(new Tuple<int, int>(_groupId, studentInfoId));
                    }
                }

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static int AddStudentInfoToCSV(Student _student, FullEvaluationCriteria _fullEvaluationCriteria)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var fullEvaluationDictionary = container.FullEvaluationDictionary;

                int fullEvaluationId = AddFullEvaluationToCSV(_fullEvaluationCriteria);
                if (fullEvaluationId == default)
                    return default;

                var studentInfoDictionary = container.StudentInfoDictionary;

                int lastId = (studentInfoDictionary.Count != 0) ? studentInfoDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_StudentInfo;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    _student.AccountNumber.ToString(),
                                    fullEvaluationId.ToString());
                }

                studentInfoDictionary.Add(newId, new StudentInfo(_student, fullEvaluationDictionary[fullEvaluationId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }


        public static int AddFullEvaluationToCSV(FullEvaluationCriteria _fullEvaluationCriteria)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var quantitativeEvaluationSetCollectionDictionary = container.QuantitativeEvaluationSetCollectionDictionary;
                var qualitativeEvaluationSetCollectionDictionary = container.QualitativeEvaluationSetCollectionDictionary;

                int quantitativeEvaluationSetCollectionId = AddQuantitativeEvaluationSetCollectionToCSV(_fullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation);
                if (quantitativeEvaluationSetCollectionId == default)
                    return default;

                int qualitativeEvaluationSetCollectionId = AddQualitativeEvaluationSetCollectionToCSV(_fullEvaluationCriteria.CriteriaSet_QualitativeEvaluation);
                if (qualitativeEvaluationSetCollectionId == default)
                    return default;

                var fullEvaluationDictionary = container.FullEvaluationDictionary;

                int lastId = (fullEvaluationDictionary.Count != 0) ? fullEvaluationDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_FullEvaluation;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}", newId.ToString(), quantitativeEvaluationSetCollectionId.ToString(), qualitativeEvaluationSetCollectionId.ToString());
                }

                fullEvaluationDictionary.Add(newId, new FullEvaluation(quantitativeEvaluationSetCollectionDictionary[quantitativeEvaluationSetCollectionId], qualitativeEvaluationSetCollectionDictionary[qualitativeEvaluationSetCollectionId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQuantitativeEvaluationSetCollectionToCSV(CriteriaSet_QuantitativeEvaluation _criteriaSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationSetDictionary = container.QuantitativeEvaluationSetDictionary;

                int firstPartialEvaluationSetId = AddQuantitativeEvaluationSetToCSV(_criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial);

                int midtermEvaluationSetId = AddQuantitativeEvaluationSetToCSV(_criteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm);
                if (midtermEvaluationSetId == default)
                    return default;

                int finalEvaluationSetId = AddQuantitativeEvaluationSetToCSV(_criteriaSet.CriteriaWeight_QuantitativeEvaluation_Final);
                if (finalEvaluationSetId == default)
                    return default;

                int additionalEvaluationSetId = AddQuantitativeEvaluationSetToCSV(_criteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional);
                if (additionalEvaluationSetId == default)
                    return default;

                var evaluationSetCollectionDictionary = container.QuantitativeEvaluationSetCollectionDictionary;

                int lastId = (evaluationSetCollectionDictionary.Count != 0) ? evaluationSetCollectionDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluationSetCollection;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4}",
                                    newId.ToString(),
                                    firstPartialEvaluationSetId,
                                    midtermEvaluationSetId,
                                    finalEvaluationSetId,
                                    additionalEvaluationSetId);
                }

                evaluationSetCollectionDictionary.Add(newId, new EvaluationSetCollection_Quantitative((firstPartialEvaluationSetId > 0) ? evaluationSetDictionary[firstPartialEvaluationSetId] : null, evaluationSetDictionary[midtermEvaluationSetId], evaluationSetDictionary[finalEvaluationSetId], evaluationSetDictionary[additionalEvaluationSetId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQuantitativeEvaluationSetToCSV(CriteriaWeight_QuantitativeEvaluation _criteriaWeight)
        {
            if (_criteriaWeight == null)
                return default;

            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationDictionary = container.QuantitativeEvaluationDictionary;

                List<int> evaluationIds = new List<int>();
                List<QuantitativeEvaluation> evaluationList = new List<QuantitativeEvaluation>();
                foreach (var criterionWeight in _criteriaWeight.Criteria.WeightPerCriterion)
                {
                    int evaluationId = AddQuantitativeEvaluationToCSV(criterionWeight.Criterion);
                    if (evaluationId == default)
                        return default;

                    evaluationIds.Add(evaluationId);
                    evaluationList.Add(evaluationDictionary[evaluationId]);
                }

                var evaluationSetDictionary = container.QuantitativeEvaluationSetDictionary;

                int lastId = (evaluationSetDictionary.Count != 0) ? evaluationSetDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluationSet;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0}", newId.ToString());
                }

                if (!AddQuantitativeEvaluationSet_EvaluationMappingsToCSV(newId, evaluationIds))
                    return default;

                evaluationSetDictionary.Add(newId, evaluationList); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQuantitativeEvaluationToCSV(Criterion_QuantitativeEvaluation _criterion)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionDictionary = container.QuantitativeEvaluationCriterionDictionary;

                int criterionId = criterionDictionary.GetFirstKey(_criterion);
                decimal criterionMinValue = _criterion.Min;

                var evaluationDictionary = container.QuantitativeEvaluationDictionary;

                int lastId = (evaluationDictionary.Count != 0) ? evaluationDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QuantitativeEvaluation;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    criterionId.ToString(),
                                    criterionMinValue.ToString());
                }

                evaluationDictionary.Add(newId, new QuantitativeEvaluation(_criterion, criterionMinValue)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static bool AddQuantitativeEvaluationSet_EvaluationMappingsToCSV(int _evaluationSetId, List<int> _evaluationIds)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var mappingList = container.QuantitativeEvaluationSet_EvaluationMappingList;

                string filePath = container.FilePath_QuantitativeEvaluationSet_Evaluation;
                using (var sw = new StreamWriter(filePath, true))
                {
                    foreach (var evaluationId in _evaluationIds)
                    {
                        sw.WriteLine("{0};{1}", _evaluationSetId.ToString(), evaluationId.ToString());
                    }
                }

                // Update application-side data
                {
                    foreach (var evaluationId in _evaluationIds)
                    {
                        mappingList.Add(new Tuple<int, int>(_evaluationSetId, evaluationId));
                    }
                }

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        public static int AddQualitativeEvaluationSetCollectionToCSV(CriteriaSet_QualitativeEvaluation _criteriaSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationSetDictionary = container.QualitativeEvaluationSetDictionary;

                int firstPartialEvaluationSetId = AddQualitativeEvaluationSetToCSV(_criteriaSet.Criteria_QualitativeEvaluation_FirstPartial);

                int midtermEvaluationSetId = AddQualitativeEvaluationSetToCSV(_criteriaSet.Criteria_QualitativeEvaluation_Midterm);
                if (midtermEvaluationSetId == default)
                    return default;

                int finalEvaluationSetId = AddQualitativeEvaluationSetToCSV(_criteriaSet.Criteria_QualitativeEvaluation_Final);
                if (finalEvaluationSetId == default)
                    return default;

                var evaluationSetCollectionDictionary = container.QualitativeEvaluationSetCollectionDictionary;

                int lastId = (evaluationSetCollectionDictionary.Count != 0) ? evaluationSetCollectionDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QualitativeEvaluationSetCollection;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3}",
                                    newId.ToString(),
                                    firstPartialEvaluationSetId,
                                    midtermEvaluationSetId,
                                    finalEvaluationSetId);
                }

                evaluationSetCollectionDictionary.Add(newId, new EvaluationSetCollection_Qualitative((firstPartialEvaluationSetId > 0) ? evaluationSetDictionary[firstPartialEvaluationSetId] : null, evaluationSetDictionary[midtermEvaluationSetId], evaluationSetDictionary[finalEvaluationSetId])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQualitativeEvaluationSetToCSV(Criteria_QualitativeEvaluation _criteria)
        {
            if (_criteria == null)
                return default;

            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationDictionary = container.QualitativeEvaluationDictionary;

                List<int> evaluationIds = new List<int>();
                List<QualitativeEvaluation> evaluationList = new List<QualitativeEvaluation>();
                foreach (var criterion in _criteria.CriterionList)
                {
                    int evaluationId = AddQualitativeEvaluationToCSV(criterion);
                    if (evaluationId == default)
                        return default;

                    evaluationIds.Add(evaluationId);
                    evaluationList.Add(evaluationDictionary[evaluationId]);
                }

                var evaluationSetDictionary = container.QualitativeEvaluationSetDictionary;

                int lastId = (evaluationSetDictionary.Count != 0) ? evaluationSetDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QualitativeEvaluationSet;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0}", newId.ToString());
                }

                if (!AddQualitativeEvaluationSet_EvaluationMappingsToCSV(newId, evaluationIds))
                    return default;

                evaluationSetDictionary.Add(newId, evaluationList); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static int AddQualitativeEvaluationToCSV(Criterion_QualitativeEvaluation _criterion)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionDictionary = container.QualitativeEvaluationCriterionDictionary;

                int criterionId = criterionDictionary.GetFirstKey(_criterion);
                int criterionMinValue = _criterion.EvaluationColorSet.ValueColors.Min().NumericValue;

                var evaluationDictionary = container.QualitativeEvaluationDictionary;

                int lastId = (evaluationDictionary.Count != 0) ? evaluationDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_QualitativeEvaluation;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    criterionId.ToString(),
                                    criterionMinValue.ToString());
                }

                evaluationDictionary.Add(newId, new QualitativeEvaluation(_criterion, criterionMinValue)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        private static bool AddQualitativeEvaluationSet_EvaluationMappingsToCSV(int _evaluationSetId, List<int> _evaluationIds)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var mappingList = container.QualitativeEvaluationSet_EvaluationMappingList;

                string filePath = container.FilePath_QualitativeEvaluationSet_Evaluation;
                using (var sw = new StreamWriter(filePath, true))
                {
                    foreach (var evaluationId in _evaluationIds)
                    {
                        sw.WriteLine("{0};{1}", _evaluationSetId.ToString(), evaluationId.ToString());
                    }
                }

                // Update application-side data
                {
                    foreach (var evaluationId in _evaluationIds)
                    {
                        mappingList.Add(new Tuple<int, int>(_evaluationSetId, evaluationId));
                    }
                }

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
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

        public static int AddNonInstitutionalExam_InternalToCSV(DateTime _examDate, eExamType _examType)
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
                    sw.WriteLine("{0};{1};{2}",
                        newId.ToString(),
                        _examDate.ToString(),
                        examTypeId.ToString());
                }

                examDictionary.Add(newId, new NonInstitutionalExam_Internal(_examDate, _examType)); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return default;
            }
        }

        public static bool AddEvaluationFileCriterionColumnsRangeSetToCSV(string _name, string _qntFirstPartialColumnRange_start, string _qntFirstPartialColumnRange_end, string _qntMidtermColumnRange_start, string _qntMidtermColumnRange_end, string _qntFinalsColumnRange_start, string _qntFinalsColumnRange_end, List<string> _qntAdditionalColumns, string _qlFirstPartialColumnRange_start, string _qlFirstPartialColumnRange_end, string _qlMidtermColumnRange_start, string _qlMidtermColumnRange_end, string _qlFinalsColumnRange_start, string _qlFinalsColumnRange_end)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var columnRangeDictionary = container.ColumnRangeDictionary;

                int qntFP_id = default;
                if (!string.IsNullOrWhiteSpace(_qntFirstPartialColumnRange_start) && !string.IsNullOrWhiteSpace(_qntFirstPartialColumnRange_end))
                {
                    qntFP_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qntFirstPartialColumnRange_start && x.Value.EndColumnLetter == _qntFirstPartialColumnRange_end);
                    if (qntFP_id == default) // If the item does not exist
                    {
                        qntFP_id = AddColumnRangeToCSV(_qntFirstPartialColumnRange_start, _qntFirstPartialColumnRange_end);
                        if (qntFP_id == default)
                            return false;
                    }
                }

                int qntMid_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qntMidtermColumnRange_start && x.Value.EndColumnLetter == _qntMidtermColumnRange_end);
                if (qntMid_id == default) // If the item does not exist
                {
                    qntMid_id = AddColumnRangeToCSV(_qntMidtermColumnRange_start, _qntMidtermColumnRange_end);
                    if (qntMid_id == default)
                        return false;
                }

                int qntFin_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qntFinalsColumnRange_start && x.Value.EndColumnLetter == _qntFinalsColumnRange_end);
                if (qntFin_id == default) // If the item does not exist
                {
                    qntFin_id = AddColumnRangeToCSV(_qntFinalsColumnRange_start, _qntFinalsColumnRange_end);
                    if (qntFin_id == default)
                        return false;
                }

                string qntAdd_string = "";
                if (_qntAdditionalColumns != null)
                {
                    for (int i = 0; i < _qntAdditionalColumns.Count; i++)
                    {
                        qntAdd_string += ((i > 0) ? "," : "") + _qntAdditionalColumns[i];
                    }
                }

                int qlFP_id = default;
                if (!string.IsNullOrWhiteSpace(_qlFirstPartialColumnRange_start) && !string.IsNullOrWhiteSpace(_qlFirstPartialColumnRange_end))
                {
                    qlFP_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qlFirstPartialColumnRange_start && x.Value.EndColumnLetter == _qlFirstPartialColumnRange_end);
                    if (qlFP_id == default) // If the item does not exist
                    {
                        qlFP_id = AddColumnRangeToCSV(_qlFirstPartialColumnRange_start, _qlFirstPartialColumnRange_end);
                        if (qlFP_id == default)
                            return false;
                    }
                }

                int qlMid_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qlMidtermColumnRange_start && x.Value.EndColumnLetter == _qlMidtermColumnRange_end);
                if (qlMid_id == default) // If the item does not exist
                {
                    qlMid_id = AddColumnRangeToCSV(_qlMidtermColumnRange_start, _qlMidtermColumnRange_end);
                    if (qlMid_id == default)
                        return false;
                }

                int qlFin_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qlFinalsColumnRange_start && x.Value.EndColumnLetter == _qlFinalsColumnRange_end);
                if (qlFin_id == default) // If the item does not exist
                {
                    qlFin_id = AddColumnRangeToCSV(_qlFinalsColumnRange_start, _qlFinalsColumnRange_end);
                    if (qlFin_id == default)
                        return false;
                }

                var rangeSetDictionary = container.EvaluationFileCriterionColumnsRangeSetDictionary;
                int lastId = (rangeSetDictionary.Count != 0) ? rangeSetDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_EvaluationFileCriterionColumnsRangeSet;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                                newId.ToString(),
                                _name,
                                qntFP_id.ToString(),
                                qntMid_id.ToString(),
                                qntFin_id.ToString(),
                                qntAdd_string,
                                qlFP_id.ToString(),
                                qlMid_id.ToString(),
                                qlFin_id.ToString());
                }


                ColumnRange? columnRange_qntFP = null;
                if (qntFP_id > 0)
                    columnRange_qntFP = columnRangeDictionary[qntFP_id];
                ColumnRange? columnRange_qlFP = null;
                if (qlFP_id > 0)
                    columnRange_qlFP = columnRangeDictionary[qlFP_id];

                rangeSetDictionary.Add(newId, new EvaluationFileCriterionColumnsRangeSet(_name, columnRange_qntFP, columnRangeDictionary[qntMid_id], columnRangeDictionary[qntFin_id], _qntAdditionalColumns, columnRange_qlFP, columnRangeDictionary[qlMid_id], columnRangeDictionary[qlFin_id])); // Update application-side data

                container.SetFileModificationStatus(filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add data to CSV file: " + ex.Message);
                return false;
            }
        }

        private static int AddColumnRangeToCSV(string _startColumnLetter, string _endColumnLetter)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var columnRangeDictionary = container.ColumnRangeDictionary;

                int lastId = (columnRangeDictionary.Count != 0) ? columnRangeDictionary.Keys.Max() : 0;
                int newId = lastId + 1;

                string filePath = container.FilePath_ColumnRange;
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("{0};{1};{2}",
                                    newId.ToString(),
                                    _startColumnLetter,
                                    _endColumnLetter);
                }

                columnRangeDictionary.Add(newId, new ColumnRange(_startColumnLetter, _endColumnLetter)); // Update application-side data

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
        public static bool EditStudentInCSV(Student _student, string _accountNumber, string _firstName, string _middleName, string _paternalSurname, string _maternalSurname, string _organizationEmail, string _preferredEmail, string _phone, Major _major, Semester _semester)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var nameDictionary = container.NameDictionary;
                var surnameDictionary = container.SurnameDictionary;
                var fullNameDictionary = container.FullNameDictionary;
                var semesterDictionary = container.SemesterDictionary;

                int accountNumber = Convert.ToInt32(_accountNumber);

                if (_student.AccountNumber != accountNumber)
                {
                    ChangeStudentAccountNumberForAllReferencingTables(_student.AccountNumber, accountNumber);
                }

                bool nameChanged = false;

                int firstNameId = nameDictionary.GetFirstKeyOrDefault(_firstName);
                if (_student.Name.FirstName != _firstName)
                {
                    if (firstNameId == default) // If the item does not exist
                    {
                        firstNameId = AddNameToCSV(_firstName);
                        if (firstNameId == default)
                            return false;
                    }

                    nameChanged = true;
                }

                int middleNameId = nameDictionary.GetFirstKeyOrDefault(_middleName);
                if (_student.Name.MiddleName != _middleName)
                {
                    if (middleNameId == default) // If the item does not exist
                    {
                        middleNameId = AddNameToCSV(_middleName);
                        if (middleNameId == default)
                            return false;
                    }

                    nameChanged = true;
                }

                int paternalSurnameId = surnameDictionary.GetFirstKeyOrDefault(_paternalSurname);
                if (_student.Name.PaternalSurname != _paternalSurname)
                {
                    if (paternalSurnameId == default) // If the item does not exist
                    {
                        paternalSurnameId = AddSurnameToCSV(_paternalSurname);
                        if (paternalSurnameId == default)
                            return false;
                    }

                    nameChanged = true;
                }

                int maternalSurnameId = surnameDictionary.GetFirstKeyOrDefault(_maternalSurname);
                if (_student.Name.MaternalSurname != _maternalSurname)
                {
                    if (maternalSurnameId == default) // If the item does not exist
                    {
                        maternalSurnameId = AddSurnameToCSV(_maternalSurname);
                        if (maternalSurnameId == default)
                            return false;
                    }

                    nameChanged = true;
                }

                int fullNameId = fullNameDictionary.FirstOrDefault(x => x.Value.FirstName == _firstName
                                                                        && x.Value.MiddleName == _middleName
                                                                        && x.Value.PaternalSurname == _paternalSurname
                                                                        && x.Value.MaternalSurname == _maternalSurname).Key;
                if (nameChanged)
                {
                    if (fullNameId == default) // If the item does not exist
                    {
                        fullNameId = AddFullNameToCSV(firstNameId, _firstName, middleNameId, _middleName, paternalSurnameId, _paternalSurname, maternalSurnameId, _maternalSurname);
                        if (fullNameId == default)
                            return false;
                    }
                }

                int semesterId = semesterDictionary.GetFirstKey(_semester);

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
                                values[0] = accountNumber.ToString(CoreValues.AccountNumberStringFormat);
                                values[1] = fullNameId.ToString();
                                values[2] = _organizationEmail.ToString();
                                values[3] = _preferredEmail.ToString();
                                values[4] = _phone.ToString();
                                values[5] = _major.Id.ToString();
                                values[6] = semesterId.ToString();

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
                    _student.AccountNumber = accountNumber;
                    _student.Name = fullNameDictionary[fullNameId];
                    _student.OrganizationEmail = _organizationEmail;
                    _student.PreferredEmail = _preferredEmail;
                    _student.Phone = _phone;
                    _student.Major = _major;
                    _student.SemesterAdmitted = _semester;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool ChangeStudentAccountNumberForAllReferencingTables(int _currentAccountNumber, int _newAccountNumber)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string format = CoreValues.AccountNumberStringFormat;
                string curretAccountNumber_formatted = _currentAccountNumber.ToString(format);
                string newAccountNumber_formatted = _newAccountNumber.ToString(format);

                // Modify target row in CSV
                {
                    // StudentInfo.csv
                    {
                        string filePath = container.FilePath_StudentInfo;
                        List<string> lines = new List<string>();

                        using (var sr = new StreamReader(filePath))
                        {
                            string line;
                            if ((line = sr.ReadLine()) != null) // Skip the header row
                                lines.Add(line);

                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] values = line.Split(';');

                                if (values[1] == curretAccountNumber_formatted) // If it is the row to be edited
                                {
                                    values[1] = newAccountNumber_formatted;

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
                }

                // Update application-side data
                {
                    // StudentInfo is a Tuple<Student, FullEvaluation> and the changes to foreign key references are made automatically as Student is updated.
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change foreign key references in CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool EditEvaluationColorSetInCSV(EvaluationColorSet _evaluationColorSet, string _name, List<Tuple<int, string, System.Drawing.Color>> _valueColors)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var colorDictionary = container.ColorDictionary;
                var valueColorDictionary = container.ValueColorDictionary;
                var evaluationColorSet_valueColorMappingList = container.EvaluationColorSet_ValueColorMappingList;
                var evaluationColorSetDictionary = container.EvaluationColorSetDictionary;

                int id = evaluationColorSetDictionary.GetFirstKey(_evaluationColorSet);

                bool valueColorsChanged = false;

                List<ValueColor> valueColors = new List<ValueColor>();
                foreach (var valueColor in _valueColors)
                {
                    int numericValue = valueColor.Item1;
                    string textValue = valueColor.Item2;
                    System.Drawing.Color color = valueColor.Item3;

                    int valueColorId = valueColorDictionary.FirstOrDefault(x => x.Value.NumericValue == numericValue
                                                                                && x.Value.TextValue == textValue
                                                                                && x.Value.Color == color).Key;
                    if (valueColorId == default) // If the item does not exist
                    {
                        valueColorId = AddValueColorToCSV(numericValue, textValue, color);
                        if (valueColorId == default)
                            return false;

                        valueColorsChanged = true;
                    }

                    valueColors.Add(valueColorDictionary[valueColorId]);
                }

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_EvaluationColorSet;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _name;

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

                // Modify target rows in mapping CSV
                {
                    if (valueColorsChanged)
                    {
                        if (!DeleteEvaluationColorSet_ValueColorMappingFromCSV(id))
                            return false;
                        if (!AddEvaluationColorSet_ValueColorMappingsToCSV(id, valueColors))
                            return false;
                    }
                }

                // Update application-side data
                {
                    _evaluationColorSet.Name = _name;
                    _evaluationColorSet.ValueColors.Clear();
                    _evaluationColorSet.ValueColors.AddRange(valueColors);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQuantitativeEvaluationCriterionInCSV(Criterion_QuantitativeEvaluation _criterion, string _string, eValueType _evaluationValueType, decimal _min, decimal _max)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var valueTypeDictionary = container.ValueTypeDictionary;
                var baseCriterionDictionary = container.BaseCriterionDictionary;
                var criterionDictionary = container.QuantitativeEvaluationCriterionDictionary;

                int id = criterionDictionary.GetFirstKey(_criterion);

                bool baseCriterionPropertiesChanged = _criterion.String != _string;

                bool quantitativeEvaluationCriterionPropertiesChanged = _criterion.EvaluationValueType != _evaluationValueType
                                                                        || _criterion.Min != _min
                                                                        || _criterion.Max != _max;

                int evaluationValueTypeId = valueTypeDictionary.GetFirstKey(_evaluationValueType);

                // Modify target row in CSV
                if (baseCriterionPropertiesChanged)
                {
                    string filePath = container.FilePath_BaseCriterion;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row 
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _string;

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
                if (quantitativeEvaluationCriterionPropertiesChanged)
                {
                    string filePath = container.FilePath_QuantitativeEvaluationCriterion;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = evaluationValueTypeId.ToString();
                                values[2] = _min.ToString();
                                values[3] = _max.ToString();

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
                    _criterion.String = _string;
                    _criterion.EvaluationValueType = _evaluationValueType;
                    _criterion.Min = _min;
                    _criterion.Max = _max;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQualitativeEvaluationCriterionInCSV(Criterion_QualitativeEvaluation _criterion, string _string, EvaluationColorSet _evaluationColorSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationColorSetDictionary = container.EvaluationColorSetDictionary;
                var baseCriterionDictionary = container.BaseCriterionDictionary;
                var criterionDictionary = container.QualitativeEvaluationCriterionDictionary;

                int id = criterionDictionary.GetFirstKey(_criterion);

                bool baseCriterionPropertiesChanged = _criterion.String != _string;

                bool qualitativeEvaluationCriterionPropertiesChanged = _criterion.EvaluationColorSet != _evaluationColorSet;

                int evaluationColorSetId = evaluationColorSetDictionary.GetFirstKey(_evaluationColorSet);

                // Modify target row in CSV
                if (baseCriterionPropertiesChanged)
                {
                    string filePath = container.FilePath_BaseCriterion;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _string;

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
                if (qualitativeEvaluationCriterionPropertiesChanged)
                {
                    string filePath = container.FilePath_QualitativeEvaluationCriterion;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = evaluationColorSetId.ToString();

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
                    _criterion.String = _string;
                    _criterion.EvaluationColorSet = _evaluationColorSet;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQuantitativeEvaluationCriteriaInCSV(Criteria_QuantitativeEvaluation _criteria, string _name, List<Tuple<Criterion_QuantitativeEvaluation, decimal>> _criterionWeights)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionWeightDictionary = container.QuantitativeEvaluationCriterionWeightDictionary;
                var criteriaDictionary = container.QuantitativeEvaluationCriteriaDictionary;

                int id = criteriaDictionary.GetFirstKey(_criteria);

                bool criteriaPropertiesChanged = _criteria.Name != _name;

                bool criterionWeightsChanged = false;

                List<CriterionWeight_QuantitativeEvaluation> criterionWeights = new List<CriterionWeight_QuantitativeEvaluation>();
                foreach (var criterionWeight in _criterionWeights)
                {
                    Criterion_QuantitativeEvaluation criterion = criterionWeight.Item1;
                    decimal weight = criterionWeight.Item2;

                    int criterionWeightId = criterionWeightDictionary.FirstOrDefault(x => x.Value.Criterion == criterion
                                                                                        && x.Value.Weight == weight).Key;
                    if (criterionWeightId == default) // If the item does not exist
                    {
                        criterionWeightId = AddQuantitativeEvaluationCriterionWeightToCSV(criterion, weight);
                        if (criterionWeightId == default)
                            return false;

                        criterionWeightsChanged = true;
                    }

                    criterionWeights.Add(criterionWeightDictionary[criterionWeightId]);
                }

                // Modify target row in CSV
                if (criteriaPropertiesChanged)
                {
                    string filePath = container.FilePath_QuantitativeEvaluationCriteria;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _name;

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

                // Modify target rows in mapping CSV
                {
                    if (criterionWeightsChanged)
                    {
                        if (!DeleteQuantitativeEvaluationCriteria_CriterionWeightMappingFromCSV(id))
                            return false;
                        if (!AddQuantitativeEvaluationCriteria_CriterionWeightMappingsToCSV(id, criterionWeights))
                            return false;
                    }
                }

                // Update application-side data
                {
                    _criteria.Name = _name;
                    _criteria.WeightPerCriterion.Clear();
                    _criteria.WeightPerCriterion.AddRange(criterionWeights);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQualitativeEvaluationCriteriaInCSV(Criteria_QualitativeEvaluation _criteria, string _name, List<Criterion_QualitativeEvaluation> _criterionList)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criterionDictionary = container.QualitativeEvaluationCriterionDictionary;
                var criteriaDictionary = container.QualitativeEvaluationCriteriaDictionary;

                int id = criteriaDictionary.GetFirstKey(_criteria);

                bool criteriaPropertiesChanged = _criteria.Name != _name;

                bool criterionListChanged = _criteria.CriterionList.SequenceEqual(_criterionList);

                // Modify target row in CSV
                if (criteriaPropertiesChanged)
                {
                    string filePath = container.FilePath_QualitativeEvaluationCriteria;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _name;

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

                // Modify target rows in mapping CSV
                {
                    if (criterionListChanged)
                    {
                        if (!DeleteQualitativeEvaluationCritera_CriterionMappingFromCSV(id))
                            return false;
                        if (!AddQualitativeEvaluationCriteria_CriterionMappingsToCSV(id, _criterionList))
                            return false;
                    }
                }

                // Update application-side data
                {
                    _criteria.Name = _name;
                    _criteria.CriterionList.Clear();
                    _criteria.CriterionList.AddRange(_criterionList);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQuantitativeEvaluationCriteriaSetInCSV(CriteriaSet_QuantitativeEvaluation _criteriaSet, string _name, Tuple<Criteria_QuantitativeEvaluation, decimal> _firstPartialCriteriaWeight, Tuple<Criteria_QuantitativeEvaluation, decimal> _midtermCriteriaWeight, Tuple<Criteria_QuantitativeEvaluation, decimal> _finalCriteriaWeight, Tuple<Criteria_QuantitativeEvaluation, decimal> _additionalCriteriaWeight)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criteriaWeightDictionary = container.QuantitativeEvaluationCriteriaWeightDictionary;
                var criteriaSetDictionary = container.QuantitativeEvaluationCriteriaSetDictionary;

                int id = criteriaSetDictionary.GetFirstKey(_criteriaSet);

                int firstPartialCriteriaWeightId = default;
                if (_firstPartialCriteriaWeight.Item1 != null)
                {
                    firstPartialCriteriaWeightId = criteriaWeightDictionary.FirstOrDefault(x => x.Value.Criteria == _firstPartialCriteriaWeight.Item1
                                                                            && x.Value.Weight == _firstPartialCriteriaWeight.Item2).Key;
                    if (firstPartialCriteriaWeightId == default) // If the item does not exist
                    {
                        firstPartialCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_firstPartialCriteriaWeight.Item1, _firstPartialCriteriaWeight.Item2);
                        if (firstPartialCriteriaWeightId == default)
                            return false;
                    }
                }

                int midtermCriteriaWeightId = criteriaWeightDictionary.FirstOrDefault(x => x.Value.Criteria == _midtermCriteriaWeight.Item1
                                                                                && x.Value.Weight == _midtermCriteriaWeight.Item2).Key;
                if (midtermCriteriaWeightId == default) // If the item does not exist
                {
                    midtermCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_midtermCriteriaWeight.Item1, _midtermCriteriaWeight.Item2);
                    if (midtermCriteriaWeightId == default)
                        return false;
                }

                int finalCriteriaWeightId = criteriaWeightDictionary.FirstOrDefault(x => x.Value.Criteria == _finalCriteriaWeight.Item1
                                                                                && x.Value.Weight == _finalCriteriaWeight.Item2).Key;
                if (finalCriteriaWeightId == default) // If the item does not exist
                {
                    finalCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_finalCriteriaWeight.Item1, _finalCriteriaWeight.Item2);
                    if (finalCriteriaWeightId == default)
                        return false;
                }

                int additionalCriteriaWeightId = criteriaWeightDictionary.FirstOrDefault(x => x.Value.Criteria == _additionalCriteriaWeight.Item1
                                                                                && x.Value.Weight == _additionalCriteriaWeight.Item2).Key;
                if (additionalCriteriaWeightId == default) // If the item does not exist
                {
                    additionalCriteriaWeightId = AddQuantitativeEvaluationCriteriaWeightToCSV(_additionalCriteriaWeight.Item1, _additionalCriteriaWeight.Item2);
                    if (additionalCriteriaWeightId == default)
                        return false;
                }

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_QuantitativeEvaluationCriteriaSet;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _name;
                                values[2] = firstPartialCriteriaWeightId.ToString();
                                values[3] = midtermCriteriaWeightId.ToString();
                                values[4] = finalCriteriaWeightId.ToString();
                                values[5] = additionalCriteriaWeightId.ToString();

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
                    _criteriaSet.Name = _name;
                    _criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial = (firstPartialCriteriaWeightId > 0) ? criteriaWeightDictionary[firstPartialCriteriaWeightId] : null;
                    _criteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm = criteriaWeightDictionary[midtermCriteriaWeightId];
                    _criteriaSet.CriteriaWeight_QuantitativeEvaluation_Final = criteriaWeightDictionary[finalCriteriaWeightId];
                    _criteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional = criteriaWeightDictionary[additionalCriteriaWeightId];
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQualitativeEvaluationCriteriaSetInCSV(CriteriaSet_QualitativeEvaluation _criteriaSet, string _name, Criteria_QualitativeEvaluation _firstPartialCriteria, Criteria_QualitativeEvaluation _midtermCriteria, Criteria_QualitativeEvaluation _finalCriteria)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var criteriaDictionary = container.QualitativeEvaluationCriteriaDictionary;
                var criteriaSetDictionary = container.QualitativeEvaluationCriteriaSetDictionary;

                int id = criteriaSetDictionary.GetFirstKey(_criteriaSet);

                int firstPartialCriteriaId = criteriaDictionary.GetFirstKeyOrDefault(_firstPartialCriteria);
                int midtermCriteriaId = criteriaDictionary.GetFirstKey(_midtermCriteria);
                int finalCriteriaId = criteriaDictionary.GetFirstKey(_finalCriteria);

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_QualitativeEvaluationCriteriaSet;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _name;
                                values[2] = firstPartialCriteriaId.ToString();
                                values[3] = midtermCriteriaId.ToString();
                                values[4] = finalCriteriaId.ToString();

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
                    _criteriaSet.Name = _name;
                    _criteriaSet.Criteria_QualitativeEvaluation_FirstPartial = _firstPartialCriteria;
                    _criteriaSet.Criteria_QualitativeEvaluation_Midterm = _midtermCriteria;
                    _criteriaSet.Criteria_QualitativeEvaluation_Final = _finalCriteria;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditCourseBaseInCSV(CourseBase _courseBase, int _id, string _name, eCourseCategory _category)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (_courseBase.Id != _id)
                {
                    ChangeCourseBaseIdForAllReferencingTables(_courseBase.Id, _id);
                }

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_CourseBase;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == _courseBase.Id) // If it is the row to be edited
                            {
                                int _categoryId = container.CourseCategoryDictionary.GetFirstKey(_category);

                                values[0] = _id.ToString();
                                values[1] = _name;
                                values[2] = _categoryId.ToString();

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
                    _courseBase.Id = _id;
                    _courseBase.Name = _name;
                    _courseBase.Category = _category;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool ChangeCourseBaseIdForAllReferencingTables(int _currentId, int _newId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string currentId_string = _currentId.ToString();
                string newId_string = _newId.ToString();

                // Modify target row in CSV
                {
                    // StudentInfo.csv
                    {
                        string filePath = container.FilePath_Course;
                        List<string> lines = new List<string>();

                        using (var sr = new StreamReader(filePath))
                        {
                            string line;
                            if ((line = sr.ReadLine()) != null) // Skip the header row
                                lines.Add(line);

                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] values = line.Split(';');

                                if (values[1] == currentId_string) // If it is the row to be edited
                                {
                                    values[1] = newId_string;

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
                }

                // Update application-side data
                {
                    // Course contains CourseBase and the changes to foreign key references are made automatically as CourseBase is updated.
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change foreign key references in CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool EditCourseInCSV(Course _course, eTerm _term, int _year, CriteriaSet_QuantitativeEvaluation _quantitativeEvaluationCriteriaSet, CriteriaSet_QualitativeEvaluation _qualitativeEvaluationCriteriaSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var courseBaseList = container.CourseBaseList;
                var courseGroupDictionary = container.CourseGroupDictionary;
                var yearDictionary = container.YearDictionary;
                var termDictionary = container.TermDictionary;
                var semesterDictionary = container.SemesterDictionary;
                var fullEvaluationCriteriaDictionary = container.FullEvaluationCriteriaDictionary;
                var courseDictionary = container.CourseDictionary;

                int id = courseDictionary.GetFirstKey(_course);

                int yearId = yearDictionary.GetFirstKeyOrDefault(_year);
                if (yearId == default) // If the item does not exist
                {
                    yearId = AddYearToCSV(_year);
                    if (yearId == default)
                        return default;
                }

                int termId = termDictionary.GetFirstKey(_term);

                int semesterId = semesterDictionary.FirstOrDefault(x => x.Value.Year == _year
                                                                        && x.Value.Term == _term).Key;
                if (semesterId == default) // If the item does not exist
                {
                    semesterId = AddSemesterToCSV(yearId, termId);
                    if (semesterId == default)
                        return default;
                }

                int fullEvaluationCriteriaId = fullEvaluationCriteriaDictionary.FirstOrDefault(x => x.Value.CriteriaSet_QuantitativeEvaluation == _quantitativeEvaluationCriteriaSet
                                                                                    && x.Value.CriteriaSet_QualitativeEvaluation == _qualitativeEvaluationCriteriaSet).Key;
                if (fullEvaluationCriteriaId == default) // If the item does not exist
                {
                    int quantitativeEvaluationCriteriaSetId = container.QuantitativeEvaluationCriteriaSetDictionary.First(x => x.Value == _quantitativeEvaluationCriteriaSet).Key;
                    int qualitativeEvaluationCriteriaSetId = container.QualitativeEvaluationCriteriaSetDictionary.First(x => x.Value == _qualitativeEvaluationCriteriaSet).Key;
                    fullEvaluationCriteriaId = AddFullEvaluationCriteriaToCSV(quantitativeEvaluationCriteriaSetId, qualitativeEvaluationCriteriaSetId);
                    if (fullEvaluationCriteriaId == default)
                        return default;
                }

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_Course;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[0] = id.ToString();
                                values[1] = _course.Base.Id.ToString();
                                values[2] = semesterId.ToString();
                                values[3] = fullEvaluationCriteriaId.ToString();

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
                    _course.Semester = semesterDictionary[semesterId];
                    _course.FullEvaluationCriteria = fullEvaluationCriteriaDictionary[fullEvaluationCriteriaId];
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditCourseGroupInCSV(CourseGroup _group, List<Student> _studentList = null, Faculty _faculty = null)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var groupDictionary = container.CourseGroupDictionary;
                var courseDictionary = container.CourseDictionary;
                var groupStudentInfoMappingList = container.CourseGroup_StudentInfoMappingList;
                var studentInfoDictionary = container.StudentInfoDictionary;

                int id = groupDictionary.GetFirstKey(_group);

                bool groupPropertiesChanged = _group.AssignedFaculty != _faculty;

                bool studentListChanged = !(_group.StudentInfos.Select(x => x.Student).SequenceEqual(_studentList));

                // Modify target row in CSV
                if (groupPropertiesChanged)
                {
                    string filePath = container.FilePath_CourseGroup;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[3] = _faculty.AccountNumber.ToString();

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

                // Modify target rows in mapping CSV
                if (studentListChanged)
                {
                    if (!DeleteCourseGroup_StudentInfoMappingFromCSV(id))
                        return false;
                    if (!AddCourseGroup_StudentInfoMappingsToCSV(id, _studentList, _group.Course.FullEvaluationCriteria))
                        return false;
                }

                // Update application-side data
                {
                    List<StudentInfo> studentInfos = new List<StudentInfo>();
                    var mappingList = groupStudentInfoMappingList.Where(x => x.Item1 == id);
                    foreach (var element in mappingList)
                    {
                        studentInfos.Add(studentInfoDictionary[element.Item2]);
                    }
                    _group.StudentInfos = studentInfos;
                    _group.AssignedFaculty = _faculty;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQuantitativeEvaluationInCSV(QuantitativeEvaluation _evaluation, decimal _value)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationDictionary = container.QuantitativeEvaluationDictionary;
                int id = evaluationDictionary.GetFirstKey(_evaluation);

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_QuantitativeEvaluation;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[2] = _value.ToString();

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
                    _evaluation.Value = _value;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditQualitativeEvaluationInCSV(QualitativeEvaluation _evaluation, int _value)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var evaluationDictionary = container.QualitativeEvaluationDictionary;
                int id = evaluationDictionary.GetFirstKey(_evaluation);

                // Modify target row in CSV
                {
                    string filePath = container.FilePath_QualitativeEvaluation;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == id) // If it is the row to be edited
                            {
                                values[2] = _value.ToString();

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
                    _evaluation.Value = _value;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool EditEvaluationFileCriterionColumnsRangeSetInCSV(EvaluationFileCriterionColumnsRangeSet _rangeSet, string _name, string _qntFirstPartialColumnRange_start, string _qntFirstPartialColumnRange_end, string _qntMidtermColumnRange_start, string _qntMidtermColumnRange_end, string _qntFinalsColumnRange_start, string _qntFinalsColumnRange_end, List<string> _qntAdditionalColumns, string _qlFirstPartialColumnRange_start, string _qlFirstPartialColumnRange_end, string _qlMidtermColumnRange_start, string _qlMidtermColumnRange_end, string _qlFinalsColumnRange_start, string _qlFinalsColumnRange_end)
        {
            try
            {
                DataContainer container = DataContainer.Instance;
                var columnRangeDictionary = container.ColumnRangeDictionary;

                bool nameChanged = false;

                int qntFP_id = default;
                if (!string.IsNullOrWhiteSpace(_qntFirstPartialColumnRange_start) && !string.IsNullOrWhiteSpace(_qntFirstPartialColumnRange_end))
                {
                    qntFP_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qntFirstPartialColumnRange_start && x.Value.EndColumnLetter == _qntFirstPartialColumnRange_end);
                    if (qntFP_id == default) // If the item does not exist
                    {
                        qntFP_id = AddColumnRangeToCSV(_qntFirstPartialColumnRange_start, _qntFirstPartialColumnRange_end);
                        if (qntFP_id == default)
                            return false;
                    }
                }

                int qntMid_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qntMidtermColumnRange_start && x.Value.EndColumnLetter == _qntMidtermColumnRange_end);
                if (qntMid_id == default) // If the item does not exist
                {
                    qntMid_id = AddColumnRangeToCSV(_qntMidtermColumnRange_start, _qntMidtermColumnRange_end);
                    if (qntMid_id == default)
                        return false;
                }

                int qntFin_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qntFinalsColumnRange_start && x.Value.EndColumnLetter == _qntFinalsColumnRange_end);
                if (qntFin_id == default) // If the item does not exist
                {
                    qntFin_id = AddColumnRangeToCSV(_qntFinalsColumnRange_start, _qntFinalsColumnRange_end);
                    if (qntFin_id == default)
                        return false;
                }

                string qntAdd_string = "";
                if (_qntAdditionalColumns != null)
                {
                    for (int i = 0; i < _qntAdditionalColumns.Count; i++)
                    {
                        qntAdd_string += ((i > 0) ? "," : "") + _qntAdditionalColumns[i];
                    }
                }

                int qlFP_id = default;
                if (!string.IsNullOrWhiteSpace(_qlFirstPartialColumnRange_start) && !string.IsNullOrWhiteSpace(_qlFirstPartialColumnRange_end))
                {
                    qlFP_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qlFirstPartialColumnRange_start && x.Value.EndColumnLetter == _qlFirstPartialColumnRange_end);
                    if (qlFP_id == default) // If the item does not exist
                    {
                        qlFP_id = AddColumnRangeToCSV(_qlFirstPartialColumnRange_start, _qlFirstPartialColumnRange_end);
                        if (qlFP_id == default)
                            return false;
                    }
                }

                int qlMid_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qlMidtermColumnRange_start && x.Value.EndColumnLetter == _qlMidtermColumnRange_end);
                if (qlMid_id == default) // If the item does not exist
                {
                    qlMid_id = AddColumnRangeToCSV(_qlMidtermColumnRange_start, _qlMidtermColumnRange_end);
                    if (qlMid_id == default)
                        return false;
                }

                int qlFin_id = columnRangeDictionary.GetFirstKeyOrDefault(x => x.Value.StartColumnLetter == _qlFinalsColumnRange_start && x.Value.EndColumnLetter == _qlFinalsColumnRange_end);
                if (qlFin_id == default) // If the item does not exist
                {
                    qlFin_id = AddColumnRangeToCSV(_qlFinalsColumnRange_start, _qlFinalsColumnRange_end);
                    if (qlFin_id == default)
                        return false;
                }

                // Modify target row in CSV
                {
                    var rangeSetDictionary = container.EvaluationFileCriterionColumnsRangeSetDictionary;

                    string filePath = container.FilePath_EvaluationFileCriterionColumnsRangeSet;
                    List<string> lines = new List<string>();

                    using (var sr = new StreamReader(filePath))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null) // Skip the header row
                            lines.Add(line);

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (Convert.ToInt32(values[0]) == rangeSetDictionary.GetFirstKey(_rangeSet)) // If it is the row to be edited
                            {
                                values[1] = _name;
                                values[2] = qntFP_id.ToString();
                                values[3] = qntMid_id.ToString();
                                values[4] = qntFin_id.ToString();
                                values[5] = qntAdd_string;
                                values[6] = qlFP_id.ToString();
                                values[7] = qlMid_id.ToString();
                                values[8] = qlFin_id.ToString();

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
                    ColumnRange? columnRange_qntFP = null;
                    if (qntFP_id > 0)
                        columnRange_qntFP = columnRangeDictionary[qntFP_id];
                    ColumnRange? columnRange_qlFP = null;
                    if (qlFP_id > 0)
                        columnRange_qlFP = columnRangeDictionary[qlFP_id];

                    _rangeSet.Name = _name;
                    _rangeSet.QntFirstPartialColumnRange = columnRange_qntFP;
                    _rangeSet.QntMidtermColumnRange = columnRangeDictionary[qntMid_id];
                    _rangeSet.QntFinalsColumnRange = columnRangeDictionary[qntFin_id];
                    _rangeSet.QntAdditionalColumns = _qntAdditionalColumns;
                    _rangeSet.QlFirstPartialColumnRange = columnRange_qlFP;
                    _rangeSet.QlMidtermColumnRange = columnRangeDictionary[qlMid_id];
                    _rangeSet.QlFinalsColumnRange = columnRangeDictionary[qlFin_id];
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to edit data in CSV file: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Delete Data from CSV
        public static bool DeleteStudentFromCSV(Student _student)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteStudentRelatedRows(_student))
                    return false;

                // Modify target row in CSV
                string formattedAccountNumber = _student.AccountNumber.ToString(CoreValues.AccountNumberStringFormat);
                if (!CSV.DeleteRow(formattedAccountNumber, container.FilePath_Student))
                    return false;

                // Update application-side data
                {
                    var list = container.StudentList;
                    list.Remove(_student);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteStudentRelatedRows(Student _student)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                string accountNumberFormatted = _student.AccountNumber.ToString(CoreValues.AccountNumberStringFormat);

                // Student Info
                {
                    List<int> deletingItemIds = container.StudentInfoDictionary.Where(x => x.Value.Student == _student).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteStudentInfoFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteStudentInfoFromCSV(int _studentInfoId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteStudentInfoRelatedRows(_studentInfoId))
                    return false;



                // Modify target row in CSV
                if (!CSV.DeleteRow(_studentInfoId.ToString(), container.FilePath_StudentInfo))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.StudentInfoDictionary;
                    dictionary.Remove(_studentInfoId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteStudentInfoRelatedRows(int _id)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var studentInfo = container.StudentInfoDictionary[_id];

                // Course Group
                {
                    List<int> deletingItemIds = container.CourseGroupDictionary.Where(x => x.Value.StudentInfos.Any(y => y.Student == studentInfo.Student && y.FullEvaluation == studentInfo.FullEvaluation)).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteCourseGroupFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteCourseGroup_StudentInfoMappingFromCSV(int _courseGroupId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_courseGroupId.ToString(), container.FilePath_CourseGroup_StudentInfo))
                    return false;

                // Update application-side data
                {
                    var list = container.CourseGroup_StudentInfoMappingList;
                    list.RemoveAll(x => x.Item1 == _courseGroupId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteCourseGroupFromCSV(int _courseGroupId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Cascade delete function goes here if required

                // Delete mapping
                if (!DeleteCourseGroup_StudentInfoMappingFromCSV(_courseGroupId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_courseGroupId.ToString(), container.FilePath_CourseGroup))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.CourseGroupDictionary;
                    dictionary.Remove(_courseGroupId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteEvaluationColorSetFromCSV(int _evaluationColorSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteEvaluationColorSetRelatedRows(_evaluationColorSetId))
                    return false;

                // Delete mapping
                if (!DeleteEvaluationColorSet_ValueColorMappingFromCSV(_evaluationColorSetId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationColorSetId.ToString(), container.FilePath_EvaluationColorSet))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.EvaluationColorSetDictionary;
                    dictionary.Remove(_evaluationColorSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteEvaluationColorSetRelatedRows(int _evaluationColorSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var evaluationColorSet = container.EvaluationColorSetDictionary[_evaluationColorSetId];

                // Qualitative Evaluation Criterion
                {
                    List<int> deletingItemIds = container.QualitativeEvaluationCriterionDictionary.Where(x => x.Value.EvaluationColorSet == evaluationColorSet).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQualitativeEvaluationCriterionFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationCriterionFromCSV(int _criterionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQualitativeEvaluationCriterionRelatedRows(_criterionId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criterionId.ToString(), container.FilePath_QualitativeEvaluationCriterion))
                    return false;
                if (!CSV.DeleteRow(_criterionId.ToString(), container.FilePath_BaseCriterion))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QualitativeEvaluationCriterionDictionary;
                    dictionary.Remove(_criterionId);

                    var baseDictionary = container.BaseCriterionDictionary;
                    baseDictionary.Remove(_criterionId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationCriterionFromCSV(int _criterionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationCriterionRelatedRows(_criterionId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criterionId.ToString(), container.FilePath_QuantitativeEvaluationCriterion))
                    return false;
                if (!CSV.DeleteRow(_criterionId.ToString(), container.FilePath_BaseCriterion))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationCriterionDictionary;
                    dictionary.Remove(_criterionId);

                    var baseDictionary = container.BaseCriterionDictionary;
                    baseDictionary.Remove(_criterionId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationCriterionRelatedRows(int _criterionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationCriterion = container.QuantitativeEvaluationCriterionDictionary[_criterionId];

                // Quantitative Evaluation Criterion Weight
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationCriterionWeightDictionary.Where(x => x.Value.Criterion == quantitativeEvaluationCriterion).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationCriterionWeightFromCSV(id))
                            return false;
                    }
                }

                // Quantitative Evaluation
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationDictionary.Where(x => x.Value.Criterion == quantitativeEvaluationCriterion).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationCriterionWeightFromCSV(int _criterionWeightId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationCriterionWeightRelatedRows(_criterionWeightId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criterionWeightId.ToString(), container.FilePath_QuantitativeEvaluationCriterionWeight))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationCriterionWeightDictionary;
                    dictionary.Remove(_criterionWeightId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationCriterionWeightRelatedRows(int _criterionWeightId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationCriterionWeight = container.QuantitativeEvaluationCriterionWeightDictionary[_criterionWeightId];

                // Quantitative Evaluation Criteria & Mapping
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationCriteriaDictionary.Where(x => x.Value.WeightPerCriterion.Contains(quantitativeEvaluationCriterionWeight)).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationCriteriaFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationCriteria_CriterionWeightMappingFromCSV(int _criteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaId.ToString(), container.FilePath_QuantitativeEvaluationCriteria_CriterionWeight))
                    return false;

                // Update application-side data
                {
                    var list = container.QuantitativeEvaluationCriteria_CriterionWeightMappingList;
                    list.RemoveAll(x => x.Item1 == _criteriaId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationCriteriaFromCSV(int _criteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationCriteriaRelatedRows(_criteriaId))
                    return false;

                // Delete mapping
                if (!DeleteQuantitativeEvaluationCriteria_CriterionWeightMappingFromCSV(_criteriaId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaId.ToString(), container.FilePath_QuantitativeEvaluationCriteria))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationCriteriaDictionary;
                    dictionary.Remove(_criteriaId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationCriteriaRelatedRows(int _criteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationCriteria = container.QuantitativeEvaluationCriteriaDictionary[_criteriaId];

                // Quantitative Evaluation Criteria Weight
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationCriteriaWeightDictionary.Where(x => x.Value.Criteria == quantitativeEvaluationCriteria).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationCriteriaWeightFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationCriteriaWeightFromCSV(int _criteriaWeightId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationCriteriaWeightRelatedRows(_criteriaWeightId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaWeightId.ToString(), container.FilePath_QuantitativeEvaluationCriteriaWeight))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationCriteriaWeightDictionary;
                    dictionary.Remove(_criteriaWeightId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationCriteriaWeightRelatedRows(int _criteriaWeightId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationCriteriaWeight = container.QuantitativeEvaluationCriteriaWeightDictionary[_criteriaWeightId];

                // Quantitative Evaluation Criteria Set
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationCriteriaSetDictionary.Where(x => x.Value.CriteriaWeight_QuantitativeEvaluation_FirstPartial == quantitativeEvaluationCriteriaWeight
                                                                                                                || x.Value.CriteriaWeight_QuantitativeEvaluation_Midterm == quantitativeEvaluationCriteriaWeight
                                                                                                                || x.Value.CriteriaWeight_QuantitativeEvaluation_Final == quantitativeEvaluationCriteriaWeight
                                                                                                                || x.Value.CriteriaWeight_QuantitativeEvaluation_Additional == quantitativeEvaluationCriteriaWeight)
                                                                                                        .ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationCriteriaSetFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationCriteriaSetFromCSV(int _criteriaSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationCriteriaSetRelatedRows(_criteriaSetId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaSetId.ToString(), container.FilePath_QuantitativeEvaluationCriteriaSet))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationCriteriaSetDictionary;
                    dictionary.Remove(_criteriaSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationCriteriaSetRelatedRows(int _criteriaSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationCriteriaSet = container.QuantitativeEvaluationCriteriaSetDictionary[_criteriaSetId];

                // Full Evaluation Criteria
                {
                    List<int> deletingItemIds = container.FullEvaluationCriteriaDictionary.Where(x => x.Value.CriteriaSet_QuantitativeEvaluation == quantitativeEvaluationCriteriaSet).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteFullEvaluationCriteriaFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationFromCSV(int _evaluationId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationRelatedRows(_evaluationId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationId.ToString(), container.FilePath_QuantitativeEvaluation))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationDictionary;
                    dictionary.Remove(_evaluationId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationRelatedRows(int _evaluationId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluation = container.QuantitativeEvaluationDictionary[_evaluationId];

                // Quantitative Evaluation Set & Mapping
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationSetDictionary.Where(x => x.Value.Any(y => y == quantitativeEvaluation)).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationSetFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationSet_EvaluationMappingFromCSV(int _evaluationSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationSetId.ToString(), container.FilePath_QuantitativeEvaluationSet_Evaluation))
                    return false;

                // Update application-side data
                {
                    var list = container.QuantitativeEvaluationSet_EvaluationMappingList;
                    list.RemoveAll(x => x.Item1 == _evaluationSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationSetFromCSV(int _evaluationSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationSetRelatedRows(_evaluationSetId))
                    return false;

                // Delete mapping
                if (!DeleteQuantitativeEvaluationSet_EvaluationMappingFromCSV(_evaluationSetId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationSetId.ToString(), container.FilePath_QuantitativeEvaluationSet))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationSetDictionary;
                    dictionary.Remove(_evaluationSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationSetRelatedRows(int _evaluationSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationSet = container.QuantitativeEvaluationSetDictionary[_evaluationSetId];

                // Quantitative Evaluation Set Collection
                {
                    List<int> deletingItemIds = container.QuantitativeEvaluationSetCollectionDictionary.Where(x => x.Value.QuantitativeEvaluationSet_FirstPartial == quantitativeEvaluationSet
                                                                                                                || x.Value.QuantitativeEvaluationSet_Midterm == quantitativeEvaluationSet
                                                                                                                || x.Value.QuantitativeEvaluationSet_Final == quantitativeEvaluationSet
                                                                                                                || x.Value.QuantitativeEvaluationSet_Additional == quantitativeEvaluationSet)
                                                                                                        .ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQuantitativeEvaluationSetCollectionFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQuantitativeEvaluationSetCollectionFromCSV(int _evaluationSetCollectionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQuantitativeEvaluationSetCollectionRelatedRows(_evaluationSetCollectionId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationSetCollectionId.ToString(), container.FilePath_QuantitativeEvaluationSetCollection))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QuantitativeEvaluationSetCollectionDictionary;
                    dictionary.Remove(_evaluationSetCollectionId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQuantitativeEvaluationSetCollectionRelatedRows(int _evaluationSetCollectionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var quantitativeEvaluationSetCollection = container.QuantitativeEvaluationSetCollectionDictionary[_evaluationSetCollectionId];

                // Full Evaluation
                {
                    List<int> deletingItemIds = container.FullEvaluationDictionary.Where(x => x.Value.EvaluationSetCollection_Quantitative == quantitativeEvaluationSetCollection).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteFullEvaluationFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQualitativeEvaluationCriterionRelatedRows(int _criterionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var qualitativeEvaluationCriterion = container.QualitativeEvaluationCriterionDictionary[_criterionId];

                // Qualitative Evaluation Criteria
                {
                    List<int> deletingItemIds = container.QualitativeEvaluationCriteriaDictionary.Where(x => x.Value.CriterionList.Any(y => y == qualitativeEvaluationCriterion)).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQualitativeEvaluationCriteriaFromCSV(id))
                            return false;
                    }
                }

                // Qualitative Evaluation
                {
                    List<int> deletingItemIds = container.QualitativeEvaluationDictionary.Where(x => x.Value.Criterion == qualitativeEvaluationCriterion).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQualitativeEvaluationFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationFromCSV(int _evaluationId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQualitativeEvaluationRelatedRows(_evaluationId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationId.ToString(), container.FilePath_QualitativeEvaluation))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QualitativeEvaluationDictionary;
                    dictionary.Remove(_evaluationId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQualitativeEvaluationRelatedRows(int _evaluationId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var qualitativeEvaluation = container.QualitativeEvaluationDictionary[_evaluationId];

                // Qualitative Evaluation Set & Mapping
                {
                    List<int> deletingItemIds = container.QualitativeEvaluationSetDictionary.Where(x => x.Value.Any(y => y == qualitativeEvaluation)).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQualitativeEvaluationSetFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationSet_EvaluationMappingFromCSV(int _evaluationSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationSetId.ToString(), container.FilePath_QualitativeEvaluationSet_Evaluation))
                    return false;

                // Update application-side data
                {
                    var list = container.QualitativeEvaluationSet_EvaluationMappingList;
                    list.RemoveAll(x => x.Item1 == _evaluationSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationSetFromCSV(int _evaluationSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQualitativeEvaluationSetRelatedRows(_evaluationSetId))
                    return false;

                // Delete mapping
                if (!DeleteQualitativeEvaluationSet_EvaluationMappingFromCSV(_evaluationSetId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationSetId.ToString(), container.FilePath_QualitativeEvaluationSet))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QualitativeEvaluationSetDictionary;
                    dictionary.Remove(_evaluationSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQualitativeEvaluationSetRelatedRows(int _evaluationSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var qualitativeEvaluationSet = container.QualitativeEvaluationSetDictionary[_evaluationSetId];

                // Qualitative Evaluation Set Collection
                {
                    List<int> deletingItemIds = container.QualitativeEvaluationSetCollectionDictionary.Where(x => x.Value.QualitativeEvaluations_FirstPartial == qualitativeEvaluationSet
                                                                                                                || x.Value.QualitativeEvaluations_Midterm == qualitativeEvaluationSet
                                                                                                                || x.Value.QualitativeEvaluations_Final == qualitativeEvaluationSet)
                                                                                                        .ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQualitativeEvaluationSetCollectionFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationSetCollectionFromCSV(int _evaluationSetCollectionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQualitativeEvaluationSetCollectionRelatedRows(_evaluationSetCollectionId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationSetCollectionId.ToString(), container.FilePath_QualitativeEvaluationSetCollection))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QualitativeEvaluationSetCollectionDictionary;
                    dictionary.Remove(_evaluationSetCollectionId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQualitativeEvaluationSetCollectionRelatedRows(int _evaluationSetCollectionId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var qualitativeEvaluationSetCollection = container.QualitativeEvaluationSetCollectionDictionary[_evaluationSetCollectionId];

                // Full Evaluation
                {
                    List<int> deletingItemIds = container.FullEvaluationDictionary.Where(x => x.Value.EvaluationSetCollection_Qualitative == qualitativeEvaluationSetCollection).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteFullEvaluationFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteFullEvaluationFromCSV(int _fullEvaluationId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteFullEvaluationRelatedRows(_fullEvaluationId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_fullEvaluationId.ToString(), container.FilePath_FullEvaluation))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.FullEvaluationDictionary;
                    dictionary.Remove(_fullEvaluationId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteFullEvaluationRelatedRows(int _fullEvaluationId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var fullEvaluation = container.FullEvaluationDictionary[_fullEvaluationId];

                // Student Info
                {
                    List<int> deletingItemIds = container.StudentInfoDictionary.Where(x => x.Value.FullEvaluation == fullEvaluation).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteStudentInfoFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteEvaluationColorSet_ValueColorMappingFromCSV(int _evaluationColorSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_evaluationColorSetId.ToString(), container.FilePath_EvaluationColorSet_ValueColor))
                    return false;

                // Update application-side data
                {
                    var list = container.EvaluationColorSet_ValueColorMappingList;
                    list.RemoveAll(x => x.Item1 == _evaluationColorSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationCritera_CriterionMappingFromCSV(int _criteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaId.ToString(), container.FilePath_QualitativeEvaluationCriteria_Criterion))
                    return false;

                // Update application-side data
                {
                    var list = container.QualitativeEvaluationCriteria_CriterionMappingList;
                    list.RemoveAll(x => x.Item1 == _criteriaId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationCriteriaFromCSV(int _criteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQualitativeEvaluationCriteriaRelatedRows(_criteriaId))
                    return false;

                // Delete mapping
                if (!DeleteQualitativeEvaluationCritera_CriterionMappingFromCSV(_criteriaId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaId.ToString(), container.FilePath_QualitativeEvaluationCriteria))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QualitativeEvaluationCriteriaDictionary;
                    dictionary.Remove(_criteriaId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQualitativeEvaluationCriteriaRelatedRows(int _criteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var qualitativeEvaluationCriteria = container.QualitativeEvaluationCriteriaDictionary[_criteriaId];

                // Qualitative Evaluation Criteria Set
                {
                    List<int> deletingItemIds = container.QualitativeEvaluationCriteriaSetDictionary.Where(x => x.Value.Criteria_QualitativeEvaluation_FirstPartial == qualitativeEvaluationCriteria
                                                                                                                || x.Value.Criteria_QualitativeEvaluation_Midterm == qualitativeEvaluationCriteria
                                                                                                                || x.Value.Criteria_QualitativeEvaluation_Final == qualitativeEvaluationCriteria)
                                                                                                    .ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteQualitativeEvaluationCriteriaSetFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteQualitativeEvaluationCriteriaSetFromCSV(int _criteriaSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteQualitativeEvaluationCriteriaSetRelatedRows(_criteriaSetId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_criteriaSetId.ToString(), container.FilePath_QualitativeEvaluationCriteriaSet))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.QualitativeEvaluationCriteriaSetDictionary;
                    dictionary.Remove(_criteriaSetId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteQualitativeEvaluationCriteriaSetRelatedRows(int _criteriaSetId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var qualitativeEvaluationCriteriaSet = container.QualitativeEvaluationCriteriaSetDictionary[_criteriaSetId];

                // Full Evaluation Criteria
                {
                    List<int> deletingItemIds = container.FullEvaluationCriteriaDictionary.Where(x => x.Value.CriteriaSet_QualitativeEvaluation == qualitativeEvaluationCriteriaSet).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteFullEvaluationCriteriaFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteFullEvaluationCriteriaFromCSV(int _fullEvaluationCriteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteFullEvaluationCriteriaRelatedRows(_fullEvaluationCriteriaId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_fullEvaluationCriteriaId.ToString(), container.FilePath_FullEvaluationCriteria))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.FullEvaluationCriteriaDictionary;
                    dictionary.Remove(_fullEvaluationCriteriaId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteFullEvaluationCriteriaRelatedRows(int _fullEvaluationCriteriaId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var fullEvaluationCriteria = container.FullEvaluationCriteriaDictionary[_fullEvaluationCriteriaId];

                // Course
                {
                    List<int> deletingItemIds = container.CourseDictionary.Where(x => x.Value.FullEvaluationCriteria == fullEvaluationCriteria).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteCourseFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteCourseFromCSV(int _courseId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteCourseRelatedRows(_courseId))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_courseId.ToString(), container.FilePath_Course))
                    return false;

                // Update application-side data
                {
                    var dictionary = container.CourseDictionary;
                    dictionary.Remove(_courseId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteCourseRelatedRows(int _courseId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var course = container.CourseDictionary[_courseId];

                // Course Group
                {
                    List<int> deletingItemIds = container.CourseGroupDictionary.Where(x => x.Value.Course == course).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteCourseGroupFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }

        public static bool DeleteCourseBaseFromCSV(CourseBase _courseBase)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                if (!CascadeDeleteCourseBaseRelatedRows(_courseBase.Id))
                    return false;

                // Modify target row in CSV
                if (!CSV.DeleteRow(_courseBase.Id.ToString(), container.FilePath_CourseBase))
                    return false;

                // Update application-side data
                {
                    var list = container.CourseBaseList;
                    list.Remove(_courseBase);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete data from CSV file: " + ex.Message);
                return false;
            }
        }

        private static bool CascadeDeleteCourseBaseRelatedRows(int _courseBaseId)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                var courseBase = container.CourseBaseList.First(x => x.Id == _courseBaseId);

                // Course Group
                {
                    List<int> deletingItemIds = container.CourseDictionary.Where(x => x.Value.Base == courseBase).ToDictionary(x => x.Key, x => x.Value).Keys.ToList<int>();
                    foreach (var id in deletingItemIds)
                    {
                        if (!DeleteCourseFromCSV(id))
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cascade delete rows from CSV file(s): " + ex.Message);
                return false;
            }
        }
        #endregion

        public static void LoadEvaluationDataFromExcel(string _filePath, Semester _semester, EvaluationFileCriterionColumnsRangeSet _rangeSet)
        {
            try
            {
                DataContainer container = DataContainer.Instance;

                // Open the document for reading only.
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(_filePath, false))
                {
                    // Get the Workbook part.
                    WorkbookPart workbookPart = document.WorkbookPart;

                    List<Sheet> sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToList();
                    if (sheets.Count() < 2) // Evaluation Excel files must have two sheets: One for quantitative evaluation and another for qualitative evaluation
                        return;

                    // Load data from the first Sheet from Excel file.
                    {
                        // Read the first Sheet from Excel file.
                        Sheet sheet = sheets[0];

                        // Get the Worksheet instance.
                        Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                        // Fetch all the rows present in the Worksheet.
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                        // Loop through the Worksheet rows.
                        foreach (var row in rows)
                        {
                            // Ignore the first 8 rows, which do not include evaluation data information.
                            if (row.RowIndex.Value > 8)
                            {
                                IEnumerable<Cell> cells = row.Elements<Cell>();

                                string rowId = Excel.GetCellValue<string>(cells.GetCellForColumn("A"), workbookPart, "");
                                if (rowId == "")
                                    break;

                                string facultyName = Excel.GetCellValue<string>(cells.GetCellForColumn("B"), workbookPart);
                                int courseBaseId = Excel.GetCellValue<int>(cells.GetCellForColumn("C"), workbookPart);
                                string courseName = Excel.GetCellValue<string>(cells.GetCellForColumn("D"), workbookPart);
                                string groupName = Excel.GetCellValue<string>(cells.GetCellForColumn("E"), workbookPart);
                                int studentAccountNumber = Excel.GetCellValue<int>(cells.GetCellForColumn("F"), workbookPart);

                                FullName facutlyFn = facultyName.ToFullName(false);
                                var faculty = container.FacultyList.FirstOrDefault(x => x.Name.FirstName == facutlyFn.FirstName
                                                        && x.Name.MiddleName == facutlyFn.MiddleName
                                                        && x.Name.PaternalSurname == facutlyFn.PaternalSurname
                                                        && x.Name.MaternalSurname == facutlyFn.MaternalSurname);
                                if (faculty == null)
                                {
                                    int i = 1;
                                    while (container.FacultyList.Any(x => x.AccountNumber == i))
                                    {
                                        i++;
                                    }
                                    AddFacultyToCSV(i.ToString(CoreValues.AccountNumberStringFormat), facutlyFn.FirstName, facutlyFn.MiddleName, facutlyFn.PaternalSurname, facutlyFn.MaternalSurname);
                                    faculty = container.FacultyList.Last(x => x.AccountNumber == 0);
                                }

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);

                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId;
                                Course course = null;
                                if (courseEntry.Value == null)
                                {
                                    var quantitativeEvaluationCriteriaSet = container.QuantitativeEvaluationCriteriaSetDictionary.First().Value;
                                    var qualitativeEvaluationCriteriaSet = container.QualitativeEvaluationCriteriaSetDictionary.First().Value;
                                    courseId = AddCourseToCSV(courseBase, 0, default, _semester.Term, _semester.Year, quantitativeEvaluationCriteriaSet, qualitativeEvaluationCriteriaSet);
                                }
                                else
                                {
                                    courseId = courseEntry.Key;
                                    course = courseEntry.Value;
                                }

                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                if (group == null)
                                {
                                    int groupId = AddCourseGroupToCSV(courseId, groupName, null, faculty);
                                    group = container.CourseGroupDictionary[groupId];
                                }

                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == studentAccountNumber);
                                if (student == null)
                                {
                                    string studentName = Excel.GetCellValue<string>(cells.ElementAt(6), workbookPart);
                                    FullName studentFn = studentName.ToFullName(false);
                                    AddStudentToCSV(studentAccountNumber.ToString(), studentFn.FirstName, studentFn.MiddleName, studentFn.PaternalSurname, studentFn.MaternalSurname);
                                    student = container.StudentList.First(x => x.AccountNumber == studentAccountNumber);
                                }

                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                if (!studentList.Contains(student))
                                {
                                    studentList.Add(student);
                                    EditCourseGroupInCSV(group, studentList, faculty);
                                }

                                var studentInfo = group.StudentInfos.First(x => x.Student == student);
                                {
                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Quantitative;

                                        // First Partial
                                        //{
                                        //    var evaluationSet = evaluationSets.QuantitativeEvaluationSet_FirstPartial;

                                        //    int startIndex = _rangeSet.QntFirstPartialColumnRange.StartColumnLetter.ToColumnIndex();
                                        //    int endIndex = _rangeSet.QntFirstPartialColumnRange.EndColumnLetter.ToColumnIndex();
                                        //    for (int i = startIndex; i <= endIndex; i++)
                                        //    {
                                        //        var evaluationValue = Excel.GetCellValue<decimal>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.Min);
                                        //        DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                        //    }
                                        //}

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Midterm;

                                            int startIndex = _rangeSet.QntMidtermColumnRange.StartColumnLetter.ToColumnIndex();
                                            int endIndex = _rangeSet.QntMidtermColumnRange.EndColumnLetter.ToColumnIndex();
                                            for (int i = startIndex; i <= endIndex; i++)
                                            {
                                                var evaluationValue = Excel.GetCellValue<decimal>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.Min);
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                            }
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Final;

                                            int startIndex = _rangeSet.QntFinalsColumnRange.StartColumnLetter.ToColumnIndex();
                                            int endIndex = _rangeSet.QntFinalsColumnRange.EndColumnLetter.ToColumnIndex();
                                            for (int i = startIndex; i <= endIndex; i++)
                                            {
                                                var evaluationValue = Excel.GetCellValue<decimal>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.Min);
                                                DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                            }
                                        }

                                        // Additional
                                        {
                                            var evaluationSet = evaluationSets.QuantitativeEvaluationSet_Additional;

                                            //int startIndex = _rangeSet.QntAdditionalColumnRange.StartColumnLetter.ToColumnIndex();
                                            //int endIndex = _rangeSet.QntAdditionalColumnRange.EndColumnLetter.ToColumnIndex();
                                            //for (int i = startIndex; i <= endIndex; i++)
                                            //{
                                            //    var evaluationValue = Excel.GetCellValue<decimal>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.Min);
                                            //    DocumentLoader.EditQuantitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Load data from the second Sheet from Excel file.
                    {
                        // Read the second Sheet from Excel file.
                        Sheet sheet = sheets[1];

                        // Get the Worksheet instance.
                        Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

                        // Fetch all the rows present in the Worksheet.
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                        // Loop through the Worksheet rows.
                        foreach (var row in rows)
                        {
                            // Ignore the first 8 rows, which do not include evaluation data information.
                            if (row.RowIndex.Value > 8)
                            {
                                IEnumerable<Cell> cells = row.Elements<Cell>();

                                string rowId = Excel.GetCellValue<string>(cells.GetCellForColumn("A"), workbookPart, "");
                                if (rowId == "")
                                    break;

                                string facultyName = Excel.GetCellValue<string>(cells.GetCellForColumn("B"), workbookPart);
                                int courseBaseId = Excel.GetCellValue<int>(cells.GetCellForColumn("C"), workbookPart);
                                string courseName = Excel.GetCellValue<string>(cells.GetCellForColumn("D"), workbookPart);
                                string groupName = Excel.GetCellValue<string>(cells.GetCellForColumn("E"), workbookPart);
                                int studentAccountNumber = Excel.GetCellValue<int>(cells.GetCellForColumn("F"), workbookPart);

                                FullName fn = facultyName.ToFullName(false);
                                var faculty = container.FacultyList.FirstOrDefault(x => x.Name.FirstName == fn.FirstName
                                                        && x.Name.MiddleName == fn.MiddleName
                                                        && x.Name.PaternalSurname == fn.PaternalSurname
                                                        && x.Name.MaternalSurname == fn.MaternalSurname);
                                if (faculty == null)
                                {
                                    AddFacultyToCSV("000000", fn.FirstName, fn.MiddleName, fn.PaternalSurname, fn.MaternalSurname);
                                    faculty = container.FacultyList.Last();
                                }

                                var courseBase = container.CourseBaseList.FirstOrDefault(x => x.Id == courseBaseId);

                                var courseEntry = container.CourseDictionary.FirstOrDefault(x => x.Value.Base == courseBase && x.Value.Semester == _semester);
                                int courseId;
                                Course course = null;
                                if (courseEntry.Value == null)
                                {
                                    var quantitativeEvaluationCriteriaSet = container.QuantitativeEvaluationCriteriaSetDictionary.First().Value;
                                    var qualitativeEvaluationCriteriaSet = container.QualitativeEvaluationCriteriaSetDictionary.First().Value;
                                    courseId = AddCourseToCSV(courseBase, 0, default, _semester.Term, _semester.Year, quantitativeEvaluationCriteriaSet, qualitativeEvaluationCriteriaSet);
                                }
                                else
                                {
                                    courseId = courseEntry.Key;
                                    course = courseEntry.Value;
                                }

                                var group = container.CourseGroupDictionary.FirstOrDefault(x => x.Value.Course == course && x.Value.Name == groupName).Value;
                                if (group == null)
                                {
                                    int groupId = AddCourseGroupToCSV(courseId, groupName, null, faculty);
                                    group = container.CourseGroupDictionary[groupId];
                                }

                                var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == studentAccountNumber);
                                if (student == null)
                                {
                                    string studentName = Excel.GetCellValue<string>(cells.GetCellForColumn("G"), workbookPart);
                                    FullName studentFn = studentName.ToFullName(false);
                                    AddStudentToCSV(studentAccountNumber.ToString(), studentFn.FirstName, studentFn.MiddleName, studentFn.PaternalSurname, studentFn.MaternalSurname);
                                    student = container.StudentList.First(x => x.AccountNumber == studentAccountNumber);
                                }

                                var studentList = group.StudentInfos.Select(x => x.Student).ToList();
                                if (!studentList.Contains(student))
                                {
                                    studentList.Add(student);
                                    EditCourseGroupInCSV(group, studentList, faculty);
                                }

                                var studentInfo = group.StudentInfos.First(x => x.Student == student);
                                {
                                    // Evaluations
                                    {
                                        var evaluationSets = studentInfo.FullEvaluation.EvaluationSetCollection_Qualitative;

                                        // First Partial
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_FirstPartial;

                                            //int startIndex = _rangeSet.QlFirstPartialColumnRange.StartColumnLetter.ToColumnIndex();
                                            //int endIndex = _rangeSet.QlFirstPartialColumnRange.EndColumnLetter.ToColumnIndex();
                                            //for (int i = startIndex; i <= endIndex; i++)
                                            //{
                                            //    var evaluationValue = Excel.GetCellValue<int>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue);
                                            //    DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                            //}
                                        }

                                        // Midterm
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_Midterm;

                                            int startIndex = _rangeSet.QlMidtermColumnRange.StartColumnLetter.ToColumnIndex();
                                            int endIndex = _rangeSet.QlMidtermColumnRange.EndColumnLetter.ToColumnIndex();
                                            for (int i = startIndex; i <= endIndex; i++)
                                            {
                                                var evaluationValue = Excel.GetCellValue<int>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue);
                                                DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                            }
                                        }

                                        // Finals
                                        {
                                            var evaluationSet = evaluationSets.QualitativeEvaluations_Final;

                                            int startIndex = _rangeSet.QlFinalsColumnRange.StartColumnLetter.ToColumnIndex();
                                            int endIndex = _rangeSet.QlFinalsColumnRange.EndColumnLetter.ToColumnIndex();
                                            for (int i = startIndex; i <= endIndex; i++)
                                            {
                                                var evaluationValue = Excel.GetCellValue<int>(cells.ElementAt(i), workbookPart, evaluationSet[i - startIndex].Criterion.EvaluationColorSet.ValueColors.Min().NumericValue);
                                                DocumentLoader.EditQualitativeEvaluationInCSV(evaluationSet[i - startIndex], evaluationValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Imported evaluations file data successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Excel file: " + ex.Message);
            }
        }

        //public static void LoadExamDataFromExcel(string _filePath)
        //{
        //    try
        //    {
        //        DataContainer container = DataContainer.Instance;

        //        // Open the document for reading only.
        //        using (SpreadsheetDocument document = SpreadsheetDocument.Open(_filePath, false))
        //        {
        //            // Get the Workbook part.
        //            WorkbookPart workbookPart = document.WorkbookPart;

        //            // Read the first Sheet from Excel file.
        //            Sheet sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

        //            // Get the Worksheet instance.
        //            Worksheet worksheet = ((document.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;

        //            // Fetch all the rows present in the Worksheet.
        //            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

        //            // Loop through the Worksheet rows.
        //            foreach (var row in rows)
        //            {
        //                // Ignore the first 8 rows, which do not include evaluation data information.
        //                if (row.RowIndex.Value > 8)
        //                {
        //                    IEnumerable<Cell> nonNullCells = row.Elements<Cell>();

        //                    int examineeAccountNumber = Excel.GetCellValue<int>(nonNullCells.GetCellForColumn("A"), workbookPart);
        //                    string firstName = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("B"), workbookPart);
        //                    string middleName = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("C"), workbookPart);
        //                    string paternalSurname = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("D"), workbookPart);
        //                    string maternalSurname = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("E"), workbookPart);
        //                    eExamType examType = Excel.GetCellValue<eExamType>(nonNullCells.GetCellForColumn("F"), workbookPart);
        //                    DateTime examDate = Excel.GetCellValue<DateTime>(nonNullCells.GetCellForColumn("G"), workbookPart);
        //                    int score = Excel.GetCellValue<int>(nonNullCells.GetCellForColumn("I"), workbookPart);

        //                    var student = container.StudentList.FirstOrDefault(x => x.AccountNumber == examineeAccountNumber);
        //                    if (student == null)
        //                    {
        //                        string studentName = Excel.GetCellValue<string>(nonNullCells.GetCellForColumn("H"), workbookPart);
        //                        string[] studentNames = studentName.Split(' ');
        //                        if (studentNames.Length == 3)
        //                        {
        //                            Array.Resize(ref studentNames, 4);
        //                            studentNames[3] = "";
        //                        }
        //                        AddStudentToCSV(examineeAccountNumber.ToString(), studentNames[2], studentNames[3], studentNames[0], studentNames[1]);
        //                        student = container.StudentList.First(x => x.AccountNumber == examineeAccountNumber);
        //                    }

        //                    int examId = container.NonInstitutionalExamDictionary.FirstOrDefault(x => x.Value.Examinee == student
        //                                                                                            || x.Value.Type == examType
        //                                                                                            || x.Value.Date == examDate).Key;
        //                    if (examId == default)
        //                    {
        //                        examId = AddNonInstitutionalExamToCSV(examDate, examType, student, score);
        //                        if (examId == default)
        //                        {
        //                            MessageBox.Show("Failed to add exam data!");
        //                            return;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        MessageBox.Show("Imported exam file data successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to load Excel file: " + ex.Message);
        //    }
        //}
    }
}
