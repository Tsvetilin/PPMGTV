namespace Common.Constants
{
    public static class DetailsConstants
    {
        public const int YearsToAddToInfinity = 1000;
        public const string VideosControllerName = "Videos";
        public const string VideoWatchActionName = "Watch";
        public const string AuthenticationCookieHeaderName = ".AspNetCore.Identity.Application";
        public const string CSRFCookieHeaderName = "X-CSRF-TOKEN";
        public const string AspNetEnviramentVariableName = "ASPNETCORE_ENVIRONMENT";
        public const string ProductionEnvironmentName = "Production";
        public const string VaultUri = "VaultUri";

        public static string CookieConsentRedirect => $"{SystemNames.BaseUrl}/home/setconsentcookie";
    }
}
