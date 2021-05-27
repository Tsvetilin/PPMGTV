using Ganss.XSS;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class StringSanitizeExtensions
    {
        public static string SanitizeHtml(this string str)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            return sanitizer.Sanitize(str);
        }

        public static string RemoveAllHtmlTags(this string str)
        {
            var sanitizer = new HtmlSanitizer(new List<string> {"nonexistinghtmltag"});
            return sanitizer.Sanitize(str);
        }
    }
}
