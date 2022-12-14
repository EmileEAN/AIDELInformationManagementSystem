using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration
{
    public abstract class RegistrationInfo
    {
        public RegistrationInfo(int _id, eRegistrationStatus _status, DateTime _completionTime, string _email, NonInstitutionalExam_Internal _exam, string _id1Url, string _id2Url, string _registrationPdfUrl, string _voucherUrl = null, int _bookingNumber = default)
        {
            Id = _id;
            Status = _status;

            CompletionTime = _completionTime;
            Email = _email;
            Exam = _exam;
            Id1Url = _id1Url;
            Id2Url = _id2Url;
            RegistrationPdfUrl = _registrationPdfUrl;
            VoucherUrl = _voucherUrl ?? "";
            BookingNumber = _bookingNumber;
        }

        public int Id { get; }
        public eRegistrationStatus Status { get; set; }

        public DateTime StartTime => (this is RegistrationInfo_Internal @internal) ? @internal.StartTime : default;
        public DateTime CompletionTime { get; }
        public string Email { get; }
        public NonInstitutionalExam_Internal Exam { get; }
        public string Id1Url { get; }
        public string Id2Url { get; }
        public string RegistrationPdfUrl { get; set; }
        public string VoucherUrl { get; set; }

        public int BookingNumber { get; set; }
    }
    public class RegistrationInfo_Internal : RegistrationInfo
    {
        public RegistrationInfo_Internal(int _id, eRegistrationStatus _status, DateTime _startTime, DateTime _completionTime, string _organizationEmail, NonInstitutionalExam_Internal _exam, string _id1Url, string _id2Url, string _registrationPdfUrl, string _voucherUrl = null, int _bookingNumber = default)
            : base(_id, _status, _completionTime, _organizationEmail, _exam, _id1Url, _id2Url, _registrationPdfUrl, _voucherUrl, _bookingNumber)
        {
            StartTime = _startTime;
        }

        public DateTime StartTime { get; }

        public string OrganizationEmail
        {
            get
            {
                return Email;
            }
        }
    }

    public class RegistrationInfo_External : RegistrationInfo
    {
        public RegistrationInfo_External(int _id, eRegistrationStatus _status, DateTime _completionTime, int _personalId, string _email, NonInstitutionalExam_Internal _exam, string _id1Url, string _id2Url, string _registrationPdfUrl, string _voucherUrl = null, int _bookingNumber = default)
            : base(_id, _status, _completionTime, _email, _exam, _id1Url, _id2Url, _registrationPdfUrl, _voucherUrl, _bookingNumber)
        {
            PersonalId = _personalId;
        }

        public int PersonalId { get; }
    }
}
