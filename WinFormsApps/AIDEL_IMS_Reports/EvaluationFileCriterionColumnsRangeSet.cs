using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports
{
    public class EvaluationFileCriterionColumnsRangeSet
    {
        public EvaluationFileCriterionColumnsRangeSet(string _name, ColumnRange? _qntFirstPartialColumnRange, ColumnRange _qntMidtermColumnRange, ColumnRange _qntFinalsColumnRange, List<string> _qntAdditionalColumns, ColumnRange? _qlFirstPartialColumnRange, ColumnRange _qlMidtermColumnRange, ColumnRange _qlFinalsColumnRange)
        {
            Name = _name;
            QntFirstPartialColumnRange = _qntFirstPartialColumnRange;
            QntMidtermColumnRange = _qntMidtermColumnRange;
            QntFinalsColumnRange = _qntFinalsColumnRange;
            QntAdditionalColumns = _qntAdditionalColumns.CoalesceNullAndReturnCopyOptionally();
            QlFirstPartialColumnRange = _qlFirstPartialColumnRange;
            QlMidtermColumnRange = _qlMidtermColumnRange;
            QlFinalsColumnRange = _qlFinalsColumnRange;
        }

        public string Name { get; set; }

        public ColumnRange? QntFirstPartialColumnRange { get; set; }
        public ColumnRange QntMidtermColumnRange { get; set; }
        public ColumnRange QntFinalsColumnRange { get; set; }
        public List<string> QntAdditionalColumns { get; set; }
        public ColumnRange? QlFirstPartialColumnRange { get; set; }
        public ColumnRange QlMidtermColumnRange { get; set; }
        public ColumnRange QlFinalsColumnRange { get; set; }
    }

    public struct ColumnRange
    {
        public ColumnRange(string _startColumnLetter, string _endColumnLetter)
        {
            StartColumnLetter = _startColumnLetter;
            EndColumnLetter = _endColumnLetter;
        }

        public string StartColumnLetter { get; }
        public string EndColumnLetter { get; }

        public override string ToString() { return StartColumnLetter + " -> " + EndColumnLetter; }
    }

    public enum eEvaluationType
    {
        Quantitative,
        Qualitative
    }
}
