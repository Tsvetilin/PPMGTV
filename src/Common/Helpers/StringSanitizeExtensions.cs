using Ganss.XSS;

namespace Common.Helpers
{
    public static class StringSanitizeExtensions
    {
        public static string SanitizeHtml(this string str)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            return sanitizer.Sanitize(str);
        }
    }
}
