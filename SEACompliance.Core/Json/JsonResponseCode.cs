using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Core.Json
{
    public class JsonResponseCode
    {
        public const string Success = "success";
        public const string InvalidUsernameOrPassword = "InvalidUsernameOrPassword";
        public const string IDLockedOut = "IDLockedOut";
        public const string InvalidParameter = "InvalidParameter";
        public const string GenerateGuidFailed = "GenerateGuidFailed";
        public const string InternalError = "InternalError";
        public const string SubscriptionExpired = "SubscriptionExpired";

        // Topic Management
        public const string CreateTopicFailed = "CreateTopicFailed";
        public const string UpdateTopicFailed = "UpdateTopicFailed";
        public const string TopicIsExists = "Topic: '{0}' IsExists";
        public const string SubTopicIsExists = "SubTopic: '{0}' IsExists";

        // Obligation Management
        public const string GetObligationByIdFailed = "GetObligationByIdFailed";
        public const string GetAllObligationsFailed = "GetAllObligationsFailed";
        public const string SaveObligationFailed = "SaveObligationFailed";
        public const string GetPreQuestionsWithPageFailed = "GetPreQuestionsWithPageFailed";
        public const string GetPreQuestionsByKeyWordWithPageFailed = "GetPreQuestionsByKeyWordWithPageFailed";
        public const string ObligationExists = "ObligationExists";
        public const string GetObligationByQuestionFailed = "GetObligationByQuestionFailed";
        public const string GetAllObligationsByKeyWordWithPageFailed = "GetAllObligationsByKeyWordWithPageFailed";
        public const string GetArchivedPreQuestionsByKeyWordWithPageFailed = "GetArchivedPreQuestionsByKeyWordWithPageFailed";
        public const string GetArchivedObligationsByKeyWordWithPageFailed = "GetArchivedObligationsByKeyWordWithPageFailed";
        public const string RestoreObligationFailed = "RestoreObligationFailed";

        // Obligation Option Management
        public const string CreateObligationOptionFailed = "CreateObligationOptionFailed";
        public const string DeleteObligationOptionByQuestionIdFailed = "DeleteObligationOptionByQuestionIdFailed";

        //Record 
        public const string CreateRecordFailed = "CreateRecordFailed";
        public const string UpdateRecordFailed = "UpdateRecordFailed";
        public const string RecordIsExists = "Record: '{0}' IsExists";
        public const string GetRecordByKeyWordWithPageFailed = "GetRecordByKeyWordWithPageFailed";

        //CheckItem 
        public const string CreateCheckItemFailed = "CreateCheckItemFailed";
        public const string UpdateCheckItemFailed = "UpdateCheckItemFailed";
        public const string CheckItemIsExists = "CheckItem: '{0}' IsExists";
    }
}
