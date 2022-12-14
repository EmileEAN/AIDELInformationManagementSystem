using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration
{
    public class NonInstitutionalExam_Internal
    {
        public NonInstitutionalExam_Internal(DateTime _registrationInitDate, DateTime _registrationEndDate, DateTime _date, int _maxNumOfExaminees, eExamType _type)
        {
            RegistrationInitDate = _registrationInitDate;
            RegistrationEndDate = _registrationEndDate;
            Date = _date;
            MaxNumOfExaminees = _maxNumOfExaminees;
            Type = _type;
        }
        
        public DateTime RegistrationInitDate { get; }
        public DateTime RegistrationEndDate { get; }
        public DateTime Date { get; set; }
        public int MaxNumOfExaminees { get; }
        public eExamType Type { get; }

        public bool AlreadyEnded() { return Date < DateTime.Now; }
        public bool IsRegistrationOpen() { return RegistrationInitDate <= DateTime.Now && DateTime.Now <= RegistrationEndDate; }

        public override bool Equals(object obj) => this.Equals(obj as NonInstitutionalExam_Internal);

        public bool Equals(NonInstitutionalExam_Internal target)
        {
            if (target is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, target))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != target.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (RegistrationInitDate == target.RegistrationInitDate) 
                && (RegistrationEndDate == target.RegistrationEndDate)
                && (Date == target.Date)
                && (MaxNumOfExaminees == target.MaxNumOfExaminees)
                && (Type == target.Type);
        }

        public override int GetHashCode() => (RegistrationInitDate, RegistrationEndDate, Date, MaxNumOfExaminees, Type).GetHashCode();

        public static bool operator ==(NonInstitutionalExam_Internal lhs, NonInstitutionalExam_Internal rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(NonInstitutionalExam_Internal lhs, NonInstitutionalExam_Internal rhs) => !(lhs == rhs);
    }

    public class NonInstitutionalExamResults_Internal
    {
        public NonInstitutionalExamResults_Internal(NonInstitutionalExam_Internal _exam, Dictionary<Student, int> _scores)
        {
            Exam = _exam;
            Scores = _scores.CoalesceNullAndReturnCopyOptionally(eCopyType.None);
        }

        public NonInstitutionalExam_Internal Exam { get; }
        public Dictionary<Student, int> Scores { get; }
    }
    
    public enum eExamType
    {
        TOEFL_Diag,
        TOEFL_Cert
    }
}
