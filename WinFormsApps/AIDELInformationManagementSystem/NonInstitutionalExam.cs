using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem
{
    public class NonInstitutionalExam_Internal
    {
        public NonInstitutionalExam_Internal(DateTime _date, eExamType _type)
        {
            Date = _date;
            Type = _type;
        }

        public DateTime Date { get; }
        public eExamType Type { get; }
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
