using Common.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class UrlGenerator
    {
        public static string GenerateUrl(
            string controller,
            string action,
            string id = null,
            string slug = null,
            string area = null,
            IDictionary<string, string> queryParams = null
            )
        {
            var queryBuilder = new StringBuilder($"{SystemNames.BaseUrl}/");
            if (area != null)
            {
                queryBuilder.Append($"{area}/".ToLower());
            }

            queryBuilder.Append($"{controller}/".ToLower());
            if (action != null)
            {
                queryBuilder.Append($"{action}/".ToLower());
            }
            if (id != null)
            {
                queryBuilder.Append($"{id}/".ToLower());
            }
            if (slug != null)
            {
                queryBuilder.Append($"{slug}/".ToLower());
            }
            if (queryParams != null)
            {
                queryBuilder.Append("?");
                foreach (var param in queryParams)
                {
                    queryBuilder.Append($"{param.Key}={param.Value}&");
                }
                queryBuilder.Remove(queryBuilder.Length - 1, 1);
            }

            return queryBuilder.ToString();
        }

        public static string GenerateVideoUrl(string id, string slug)
        {
            return GenerateUrl(DetailsConstants.VideosControllerName, DetailsConstants.VideoWatchActionName, id, slug);
        }
    }
}
