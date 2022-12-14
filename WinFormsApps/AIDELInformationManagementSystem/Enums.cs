using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDELInformationManagementSystem
{
    public enum eCourseCategory
    {
        German,
        Chinese,
        SpanishArrupe,
        SpanishForeigner,
        French,
        English,
        Italian,
        Japanese,
        Portuguese
    }

    public enum eGroupNamingFormat
    {
        Alphabet,
        Number
    }

    public enum eTerm
    {
        Spring,
        Fall,
        Summer
    }

    public enum ePermissionsType
    {
        Full,
        AssignedCoursesOnly,
        ViewOnly
    }

    public enum eRegistrationStatus
    {
        PaymentPending,
        BookingPending,
        Completed
    }
}
