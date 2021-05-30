using Ganss.XSS;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class StringSanitizeExtensions
    {
        public static string SanitizeHtml(this string str, bool avoidIframe = true)
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedAttributes.Add("class");
            if (!avoidIframe)
            {
                sanitizer.AllowedAttributes.Add("frameborder");
                sanitizer.AllowedAttributes.Add("allowfullscreen");
                sanitizer.AllowedTags.Add("iframe");
            }

            return sanitizer.Sanitize(str);
        }

        public static string RemoveAllHtmlTags(this string str)
        {
            var sanitizer = new HtmlSanitizer(new List<string> { "nonexistinghtmltag" });
            sanitizer.KeepChildNodes = true;
            return sanitizer.Sanitize(str);
        }
    }
}
