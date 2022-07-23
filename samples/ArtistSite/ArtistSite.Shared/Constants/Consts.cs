namespace ArtistSite.Shared.Constants
{
    public static class Consts
    {
        #region Authorization

        public const string CLAIM_USERID = "userId";
        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_ADMIN_OR_USER = "ADMIN, USER";
        public const string ROLE_USER = "USER";

        #endregion Authorization

        #region CGH

        public const string API_FILTER_DELIMITER = "~";
        public const string CONNECTIONIDENTIFIER = "cid";
        public const string OPERATOR_CONTAINS = "Contains";
        public const string OPERATOR_DOESNOTCONTAIN = "DoesNotContain";
        public const string OPERATOR_ENDSWITH = "EndsWith";
        public const string OPERATOR_ISEMPTY = "IsEmpty";
        public const string OPERATOR_ISEQUALTO = "IsEqualTo";
        public const string OPERATOR_ISGREATERTHAN = "IsGreaterThan";
        public const string OPERATOR_ISGREATERTHANOREQUAL = "IsGreaterThanOrEqual";
        public const string OPERATOR_ISLESSTHAN = "IsLessThan";
        public const string OPERATOR_ISLESSTHANOREQUAL = "IsLessThanOrEqual";
        public const string OPERATOR_ISNOTEMPTY = "IsNotEmpty";
        public const string OPERATOR_ISNOTEQUALTO = "IsNotEqualTo";
        public const string OPERATOR_ISNOTNULL = "IsNotNull";
        public const string OPERATOR_ISNULL = "IsNull";
        public const string OPERATOR_STARTSWITH = "StartsWith";

        #endregion CGH
    }
}