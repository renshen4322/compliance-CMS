using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Core.Enums
{
    public struct LegalUpdateEditStatus
    {
        public const string New = "New";
        public const string Editing = "Editing";
        public const string Published = "Published";
    }

    public struct LegalStatus
    {
        public const string New = "New";
        public const string Update = "Update";
        public const string FYI = "FYI";
        public const string Archived = "Archived";
    }

    public struct EmailMeLinkPageNames
    {
        public const string CurrentReport = "currentreport";
        public const string FullReport = "fullreport";
        public const string NextStep = "nextstep";
        public const string History = "history";
    }

    public struct ReportType
    {
        public const string NumberOfVisit = "NumberOfVisit";
        public const string DurationOfSurveyCompletion = "DurationOfSurveyCompletion";
        public const string UnsureBehavior = "UnsureBehavior";
        public const string MemberNewsSubscription = "MemberNewsSubscription";
    }

    public enum MailType
    {
        TempPassword,
        Receipt,
        LegalUpdateNotification,
        EmailMeLink_CurrentReport,
        EmailMeLink_FullReport,
        EmailMeLink_NextStep,
        EmailMeLink_History,
        ReportDownload,
        Bnz,
        BnzInquirer
    }

    public enum NotificationType
    {
        Email,
        Sms
    }

    public enum NotificationStatus
    {
        Failed,
        Success
    }

    public enum IpMatchType
    {
        Equal,
        Contains,
        Regex
    }

    public enum ProductServerMode
    {
        None,
        Master,
        Slave
    }
}
