using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration
{
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
        Repeated,
        WaitingList,
        IDsAndPDFSignCheckRequired,
        PaymentPending,
        VoucherCheckRequired,
        StudentInfoFormPending,
        ETSInfoAndBookingCheckRequired,
        Completed,
        Canceled
    }
}
