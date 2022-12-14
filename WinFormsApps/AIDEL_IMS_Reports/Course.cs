using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports
{
    public class CourseBase
    {
        public CourseBase(int _id, string _name, eCourseCategory _category)
        {
            Id = _id;
            Name = _name;
            Category = _category;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public eCourseCategory Category { get; set; }

        public override string ToString()
        {
            return "(" + Id.ToString() + ") " + Name;
        }
    }

    public class Course
    {
        public Course(CourseBase _base, Semester _semester, FullEvaluationCriteria _fullEvaluationCriteria)
        {
            Base = _base;

            Semester = _semester;

            FullEvaluationCriteria = _fullEvaluationCriteria;
        }

        public CourseBase Base { get; }

        public Semester Semester { get; set; }

        public FullEvaluationCriteria FullEvaluationCriteria { get; set; }
    }

    public class CourseGroup
    {
        public CourseGroup(Course _course, string _name, Faculty _assignedFaculty, List<StudentInfo> _studentInfos)
        {
            Course = _course;
            Name = _name;

            AssignedFaculty = _assignedFaculty;

            StudentInfos = _studentInfos ?? new List<StudentInfo>();
        }
        
        public Course Course { get; }
        public string Name { get; } // e.g. A, B, and C.

        public Faculty AssignedFaculty { get; set; }

        public List<StudentInfo> StudentInfos { get; set; }
    }

    public class StudentInfo
    {
        public StudentInfo(Student _student, FullEvaluation _fullEvaluation)
        {
            Student = _student;
            FullEvaluation = _fullEvaluation;
        }

        public Student Student { get; }
        public FullEvaluation FullEvaluation { get; }
    }

    public class FullEvaluation
    {
        public FullEvaluation(EvaluationSetCollection_Quantitative _evaluationSetCollection_quantitative, EvaluationSetCollection_Qualitative _evaluationSetCollection_qualitative)
        {
            EvaluationSetCollection_Quantitative = _evaluationSetCollection_quantitative;
            EvaluationSetCollection_Qualitative = _evaluationSetCollection_qualitative;
        }

        public EvaluationSetCollection_Quantitative EvaluationSetCollection_Quantitative { get; }
        public EvaluationSetCollection_Qualitative EvaluationSetCollection_Qualitative { get; }
    }

    public class EvaluationSetCollection_Quantitative
    {
        public EvaluationSetCollection_Quantitative(List<QuantitativeEvaluation> _quantitativeEvaluationSet_firstPartial, List<QuantitativeEvaluation> _quantitativeEvaluationSet_midterm, List<QuantitativeEvaluation> _quantitativeEvaluationSet_final, List<QuantitativeEvaluation> _quantitativeEvaluationSet_additional)
        {
            QuantitativeEvaluationSet_FirstPartial = _quantitativeEvaluationSet_firstPartial ?? new List<QuantitativeEvaluation>();
            QuantitativeEvaluationSet_Midterm = _quantitativeEvaluationSet_midterm ?? new List<QuantitativeEvaluation>();
            QuantitativeEvaluationSet_Final = _quantitativeEvaluationSet_final ?? new List<QuantitativeEvaluation>();
            QuantitativeEvaluationSet_Additional = _quantitativeEvaluationSet_additional ?? new List<QuantitativeEvaluation>();
        }

        public List<QuantitativeEvaluation> QuantitativeEvaluationSet_FirstPartial { get; }
        public List<QuantitativeEvaluation> QuantitativeEvaluationSet_Midterm { get; }
        public List<QuantitativeEvaluation> QuantitativeEvaluationSet_Final { get; }
        public List<QuantitativeEvaluation> QuantitativeEvaluationSet_Additional { get; }
    }

    public class EvaluationSetCollection_Qualitative
    {
        public EvaluationSetCollection_Qualitative(List<QualitativeEvaluation> _qualitativeEvaluations_firstPartial, List<QualitativeEvaluation> _qualitativeEvaluations_midterm, List<QualitativeEvaluation> _qualitativeEvaluations_final)
        {
            QualitativeEvaluations_FirstPartial = _qualitativeEvaluations_firstPartial ?? new List<QualitativeEvaluation>();
            QualitativeEvaluations_Midterm = _qualitativeEvaluations_midterm ?? new List<QualitativeEvaluation>();
            QualitativeEvaluations_Final = _qualitativeEvaluations_final ?? new List<QualitativeEvaluation>();
        }

        public List<QualitativeEvaluation> QualitativeEvaluations_FirstPartial { get; }
        public List<QualitativeEvaluation> QualitativeEvaluations_Midterm { get; }
        public List<QualitativeEvaluation> QualitativeEvaluations_Final { get; }
    }

    public class QuantitativeEvaluation
    {
        public QuantitativeEvaluation(Criterion_QuantitativeEvaluation _criterion, decimal _value)
        {
            Criterion = _criterion;
            Value = _value;
        }

        public Criterion_QuantitativeEvaluation Criterion { get; }
        public decimal Value { get; set; }
    }

    public class QualitativeEvaluation
    {
        public QualitativeEvaluation(Criterion_QualitativeEvaluation _criterion, int _value)
        {
            Criterion = _criterion;
            Value = _value;
        }

        public Criterion_QualitativeEvaluation Criterion { get; }
        public int Value { get; set; }
    }

    public enum eValueType
    {
        Integer,
        Decimal
    }

    public abstract class Criterion
    {
        public Criterion(string _string)
        {
            String = _string;
        }

        public string String { get; set; }
    }

    public class Criterion_QuantitativeEvaluation : Criterion
    {
        public Criterion_QuantitativeEvaluation(string _string,
	        eValueType _evaluationValueType, decimal _min, decimal _max) : base(_string)
        {
             EvaluationValueType = _evaluationValueType;
             Min = _min;
             Max = (_min <= _max) ? _max : _min;
        }

        public eValueType EvaluationValueType { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

    public class Criterion_QualitativeEvaluation : Criterion
    {
        public Criterion_QualitativeEvaluation(string _string,
	        EvaluationColorSet _evaluationColorSet) : base(_string)
        {
            EvaluationColorSet = _evaluationColorSet;
        }

        public EvaluationColorSet EvaluationColorSet { get; set; }
    }

    public class EvaluationColorSet
    {
        public EvaluationColorSet(string _name, List<ValueColor> _valueColors)
        {
            Name = _name;
            ValueColors = _valueColors ?? new List<ValueColor>();
        }

        public string Name { get; set; }
        public List<ValueColor> ValueColors { get; }
    }

    public struct ValueColor : IComparable<ValueColor>
    {
        public ValueColor(int _numericValue, string _textValue, Color _color)
        {
            NumericValue = _numericValue;
            TextValue = _textValue;
            Color = _color;
        }

        public int NumericValue { get; }
        public string TextValue { get; }
        public Color Color { get; }

        public int CompareTo(ValueColor _other) { return NumericValue.CompareTo(_other.NumericValue); }
    }

    public class FullEvaluationCriteria
    {
        public FullEvaluationCriteria(CriteriaSet_QuantitativeEvaluation _criteriaSet_quantitativeEvaluation, CriteriaSet_QualitativeEvaluation _criteriaSet_qualitativeEvaluation)
        {
            CriteriaSet_QuantitativeEvaluation = _criteriaSet_quantitativeEvaluation;
            CriteriaSet_QualitativeEvaluation = _criteriaSet_qualitativeEvaluation;
        }

        public CriteriaSet_QuantitativeEvaluation CriteriaSet_QuantitativeEvaluation  { get; }
        public CriteriaSet_QualitativeEvaluation CriteriaSet_QualitativeEvaluation  { get; }
    }

    public class CriterionWeight_QuantitativeEvaluation
    {
        public CriterionWeight_QuantitativeEvaluation(Criterion_QuantitativeEvaluation _criterion, decimal _weight)
        {
            Criterion = _criterion;
            Weight = (_weight <= 0) ? 0 : ((_weight > 100) ? 100 : _weight);
        }
        
        public Criterion_QuantitativeEvaluation Criterion { get; }
        public decimal Weight { get; set; }
    }

    public class Criteria_QuantitativeEvaluation
    {
        public Criteria_QuantitativeEvaluation(string _name, List<CriterionWeight_QuantitativeEvaluation> _weightPerCriterion)
        {
            Name = _name;
            WeightPerCriterion = new List<CriterionWeight_QuantitativeEvaluation>(_weightPerCriterion);
            WeightValidator.Validate(WeightPerCriterion);
        }

        public string Name { get; set; }
        public List<CriterionWeight_QuantitativeEvaluation> WeightPerCriterion { get; }

        private decimal WeightTotal { get { return WeightPerCriterion.Sum(x => x.Weight); } }
        public decimal CriterionWeightRatio(CriterionWeight_QuantitativeEvaluation _criterionWeight) { return _criterionWeight.Weight / WeightTotal; }
        public string CriterionWeightPercentage(CriterionWeight_QuantitativeEvaluation _criterionWeight) { return CriterionWeightRatio(_criterionWeight).ToString(CoreValues.PercentageStringFormat); }
    }

    public class CriteriaWeight_QuantitativeEvaluation
    {
        public CriteriaWeight_QuantitativeEvaluation(Criteria_QuantitativeEvaluation _criteria, decimal _weight)
        {
            Criteria = _criteria;
            Weight = (_weight <= 0) ? 0 : ((_weight > 100) ? 100 : _weight);
        }
        
        public Criteria_QuantitativeEvaluation Criteria { get; }
        public decimal Weight { get; set; } // Percentage
    }

    public class CriteriaSet_QuantitativeEvaluation
    {
        public CriteriaSet_QuantitativeEvaluation(string _name, CriteriaWeight_QuantitativeEvaluation _criteriaWeight_quantitativeEvaluation_firstPartial, CriteriaWeight_QuantitativeEvaluation _criteriaWeight_quantitativeEvaluation_midterm, CriteriaWeight_QuantitativeEvaluation _criteriaWeight_quantitativeEvaluation_final, CriteriaWeight_QuantitativeEvaluation _criteriaWeight_quantitativeEvaluation_additional)
        {
            Name = _name;

            CriteriaWeight_QuantitativeEvaluation_FirstPartial = _criteriaWeight_quantitativeEvaluation_firstPartial;
            CriteriaWeight_QuantitativeEvaluation_Midterm = _criteriaWeight_quantitativeEvaluation_midterm;
            CriteriaWeight_QuantitativeEvaluation_Final = _criteriaWeight_quantitativeEvaluation_final;
            CriteriaWeight_QuantitativeEvaluation_Additional = _criteriaWeight_quantitativeEvaluation_additional;

            List<CriteriaWeight_QuantitativeEvaluation> weightPerCriteria = null;
            weightPerCriteria = (CriteriaWeight_QuantitativeEvaluation_FirstPartial != null) ?
                new List<CriteriaWeight_QuantitativeEvaluation>
                {
                    CriteriaWeight_QuantitativeEvaluation_FirstPartial,
                    CriteriaWeight_QuantitativeEvaluation_Midterm,
                    CriteriaWeight_QuantitativeEvaluation_Final,
                    CriteriaWeight_QuantitativeEvaluation_Additional
                }
                :
                new List<CriteriaWeight_QuantitativeEvaluation>
                {
                    CriteriaWeight_QuantitativeEvaluation_Midterm,
                    CriteriaWeight_QuantitativeEvaluation_Final,
                    CriteriaWeight_QuantitativeEvaluation_Additional
                };
            WeightValidator.Validate(weightPerCriteria);
        }

        public string Name { get; set; }
        public CriteriaWeight_QuantitativeEvaluation CriteriaWeight_QuantitativeEvaluation_FirstPartial { get; set; }
        public CriteriaWeight_QuantitativeEvaluation CriteriaWeight_QuantitativeEvaluation_Midterm { get; set; }
        public CriteriaWeight_QuantitativeEvaluation CriteriaWeight_QuantitativeEvaluation_Final { get; set; }
        public CriteriaWeight_QuantitativeEvaluation CriteriaWeight_QuantitativeEvaluation_Additional { get; set; }
    
        private decimal WeightTotal { get { return CriteriaWeight_QuantitativeEvaluation_FirstPartial.Weight + CriteriaWeight_QuantitativeEvaluation_Midterm.Weight + CriteriaWeight_QuantitativeEvaluation_Final.Weight + CriteriaWeight_QuantitativeEvaluation_Additional.Weight; } }

        public decimal WeightRatio_FirstPartial { get { return CriteriaWeight_QuantitativeEvaluation_FirstPartial.Weight / WeightTotal; } }
        public decimal WeightRatio_Midterm { get { return CriteriaWeight_QuantitativeEvaluation_Midterm.Weight / WeightTotal; } }
        public decimal WeightRatio_Final { get { return CriteriaWeight_QuantitativeEvaluation_Final.Weight / WeightTotal; } }
        public decimal WeightRatio_Additional { get { return CriteriaWeight_QuantitativeEvaluation_Additional.Weight / WeightTotal; } }

        public string WeightPercentage_FirstPartial { get { return WeightRatio_FirstPartial.ToString(CoreValues.PercentageStringFormat); } }
        public string WeightPercentage_Midterm { get { return WeightRatio_Midterm.ToString(CoreValues.PercentageStringFormat); } }
        public string WeightPercentage_Final { get { return WeightRatio_Final.ToString(CoreValues.PercentageStringFormat); } }
        public string WeightPercentage_Additional { get { return WeightRatio_Additional.ToString(CoreValues.PercentageStringFormat); } }
    }

    public static class WeightValidator
    {
        public static void Validate(List<CriterionWeight_QuantitativeEvaluation> _weightPerItem)
        {
            if (_weightPerItem == null || _weightPerItem.Count() < 1)
                return;

            decimal totalWeight = 0;

            foreach (var itemWeight in _weightPerItem)
            {
                if (totalWeight + itemWeight.Weight > 100) // Do not let totalWeight exceed 100 (%)
                    itemWeight.Weight = 100 - totalWeight;

                totalWeight += itemWeight.Weight;
            }
        }
        public static void Validate(List<CriteriaWeight_QuantitativeEvaluation> _weightPerItem)
        {
            if (_weightPerItem == null || _weightPerItem.Count() < 1)
                return;

            decimal totalWeight = 0;

            foreach (var itemWeight in _weightPerItem)
            {
                if (totalWeight + itemWeight.Weight > 100) // Do not let totalWeight exceed 100 (%)
                    itemWeight.Weight = 100 - totalWeight;

                totalWeight += itemWeight.Weight;
            }
        }
    }

    public class Criteria_QualitativeEvaluation
    {
        public Criteria_QualitativeEvaluation(string _name, List<Criterion_QualitativeEvaluation> _criterionList)
        {
            Name = _name;
        
            CriterionList = _criterionList;
        }

        public string Name { get; set; }
        public List<Criterion_QualitativeEvaluation> CriterionList { get; }
    }

    public class CriteriaSet_QualitativeEvaluation
    {
        public CriteriaSet_QualitativeEvaluation(string _name, Criteria_QualitativeEvaluation _criteria_qualitativeEvaluation_firstPartial, Criteria_QualitativeEvaluation _criteria_qualitativeEvaluation_midterm, Criteria_QualitativeEvaluation _criteria_qualitativeEvaluation_final)
        {
            Name = _name;

            Criteria_QualitativeEvaluation_FirstPartial = _criteria_qualitativeEvaluation_firstPartial;
            Criteria_QualitativeEvaluation_Midterm = _criteria_qualitativeEvaluation_midterm;
            Criteria_QualitativeEvaluation_Final = _criteria_qualitativeEvaluation_final;
        }

        public string Name { get; set; }
        public Criteria_QualitativeEvaluation Criteria_QualitativeEvaluation_FirstPartial { get; set; }
        public Criteria_QualitativeEvaluation Criteria_QualitativeEvaluation_Midterm { get; set; }
        public Criteria_QualitativeEvaluation Criteria_QualitativeEvaluation_Final { get; set; }
    }

    public static class EvaluationCalculator
    {
        public static decimal QuantitativeEvaluationPointAverage(CourseGroup _courseGroup, Student _student)
        {
            var eventualMaxValue = CoreValues.MaxQuantitativeEvaluationValue;

            var criteriaSet = _courseGroup.Course.FullEvaluationCriteria.CriteriaSet_QuantitativeEvaluation;
            var evaluationSetCollection = _courseGroup.StudentInfos.First(x => x.Student == _student).FullEvaluation.EvaluationSetCollection_Quantitative;

            decimal totalAverageEvaluationPoints = 0;

            // First Partial
            {
                var criteriaWeight = criteriaSet.CriteriaWeight_QuantitativeEvaluation_FirstPartial;
                var evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_FirstPartial;
                decimal criteriaAverageEvaluationPoints = QuantitativePartialEvaluationPointAverage(criteriaWeight, evaluationSet);

                var criteriaWeightRatio = criteriaSet.WeightRatio_FirstPartial;
                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaWeightRatio;
            }

            // Midterm
            {
                var criteriaWeight = criteriaSet.CriteriaWeight_QuantitativeEvaluation_Midterm;
                var evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Midterm;
                decimal criteriaAverageEvaluationPoints = QuantitativePartialEvaluationPointAverage(criteriaWeight, evaluationSet);

                var criteriaWeightRatio = criteriaSet.WeightRatio_Midterm;
                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaWeightRatio;
            }

            // Finals
            {
                var criteriaWeight = criteriaSet.CriteriaWeight_QuantitativeEvaluation_Final;
                var evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Final;
                decimal criteriaAverageEvaluationPoints = QuantitativePartialEvaluationPointAverage(criteriaWeight, evaluationSet);

                var criteriaWeightRatio = criteriaSet.WeightRatio_Final;
                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaWeightRatio;
            }

            // Additional
            {
                var criteriaWeight = criteriaSet.CriteriaWeight_QuantitativeEvaluation_Additional;
                var evaluationSet = evaluationSetCollection.QuantitativeEvaluationSet_Additional;
                decimal criteriaAverageEvaluationPoints = QuantitativePartialEvaluationPointAverage(criteriaWeight, evaluationSet);

                var criteriaWeightRatio = criteriaSet.WeightRatio_Additional;
                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaWeightRatio;
            }

            return Math.Round(totalAverageEvaluationPoints, 1, MidpointRounding.AwayFromZero);
        }

        public static decimal QuantitativePartialEvaluationPointAverage(CriteriaWeight_QuantitativeEvaluation _criteriaWeight, List<QuantitativeEvaluation> _evaluationSet, int _decimals = 28)
        {
            var eventualMaxValue = CoreValues.MaxQuantitativeEvaluationValue;

            decimal criteriaAverageEvaluationPoints = 0;

            var criteria = _criteriaWeight.Criteria;
            var weightPerCriterion = criteria.WeightPerCriterion;
            foreach (var evaluation in _evaluationSet)
            {
                var criterionWeightRatio = criteria.CriterionWeightRatio(weightPerCriterion.First(x => x.Criterion == evaluation.Criterion));
                var relativeMaxValue = eventualMaxValue * criterionWeightRatio;
                var criterionValueRatio = evaluation.Value / evaluation.Criterion.Max;
                criteriaAverageEvaluationPoints += relativeMaxValue * criterionValueRatio;
            }

            return Math.Round(criteriaAverageEvaluationPoints, _decimals, MidpointRounding.AwayFromZero);
        }

        public static decimal QualitativeEvaluationPointAverage(CourseGroup _courseGroup, Student _student)
        {

            var criteriaSet = _courseGroup.Course.FullEvaluationCriteria.CriteriaSet_QualitativeEvaluation;
            var evaluationSetCollection = _courseGroup.StudentInfos.First(x => x.Student == _student).FullEvaluation.EvaluationSetCollection_Qualitative;

            var referenceValueColors = criteriaSet.Criteria_QualitativeEvaluation_FirstPartial.CriterionList[0].EvaluationColorSet.ValueColors;

            int numOfPartials = 3;
            decimal criteriaValueRatio = 1m / numOfPartials;

            decimal totalAverageEvaluationPoints = 0;

            // First Partial
            {
                var criteria = criteriaSet.Criteria_QualitativeEvaluation_FirstPartial;
                var evaluationSet = evaluationSetCollection.QualitativeEvaluations_FirstPartial;
                decimal criteriaAverageEvaluationPoints = QualitativePartialEvaluationPointAverage(criteria, evaluationSet);

                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaValueRatio;
            }

            // Midterm
            {
                var criteria = criteriaSet.Criteria_QualitativeEvaluation_Midterm;
                var evaluationSet = evaluationSetCollection.QualitativeEvaluations_Midterm;
                decimal criteriaAverageEvaluationPoints = QualitativePartialEvaluationPointAverage(criteria, evaluationSet);

                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaValueRatio;
            }

            // Finals
            {
                var criteria = criteriaSet.Criteria_QualitativeEvaluation_Final;
                var evaluationSet = evaluationSetCollection.QualitativeEvaluations_Final;
                decimal criteriaAverageEvaluationPoints = QualitativePartialEvaluationPointAverage(criteria, evaluationSet);

                totalAverageEvaluationPoints += criteriaAverageEvaluationPoints * criteriaValueRatio;
            }

            return Math.Round(totalAverageEvaluationPoints, 1, MidpointRounding.AwayFromZero);
        }

        public static decimal QualitativePartialEvaluationPointAverage(Criteria_QualitativeEvaluation _criteria, List<QualitativeEvaluation> _evaluationSet, int _decimals = 28)
        {
            var referenceValueColors = _criteria.CriterionList[0].EvaluationColorSet.ValueColors;

            decimal criteriaAverageEvaluationPoints = 0;

            foreach (var evaluation in _evaluationSet)
            {
                decimal criterionValueRatio = 1m / _criteria.CriterionList.Count;
                criteriaAverageEvaluationPoints += evaluation.Value * criterionValueRatio;
            }

            return Math.Round(criteriaAverageEvaluationPoints, _decimals, MidpointRounding.AwayFromZero);
        }
    }
}