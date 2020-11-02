using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class OrderSearchResultsExtensions
    {
        public static IQueryable<string> GetSearchSuggestionsResult(this IEnumerable<string> names, string part)
        {
            names = names.Select(x => x.ToLower());

            var search = part.ToLower();
            var maxLengthScore = search.Length;

            var scoredNames = new Dictionary<string, (int Score, int Index)>();

            for (int startIndex = 0; startIndex < search.Length; startIndex++)
            {
                for (int endIndex = 0; endIndex < search.Length; endIndex++)
                {
                    var substringMatchLength = endIndex + 1 - startIndex;
                    if (substringMatchLength > 0)
                    {
                        var substringToMatchWith = search.Substring(startIndex, substringMatchLength);

                        var matches = names.Where(x => x.Contains(substringToMatchWith)).ToList();
                        foreach (var match in matches)
                        {
                            if (scoredNames.ContainsKey(match))
                            {
                                if (scoredNames[match].Score < substringMatchLength)
                                {
                                    scoredNames[match] = (substringMatchLength, match.IndexOf(substringToMatchWith));
                                }
                            }
                            else
                            {
                                scoredNames[match] = (substringMatchLength, match.IndexOf(substringToMatchWith));
                            }
                        }
                    }
                }
            }

            return scoredNames.
                OrderByDescending(x => x.Value.Score).
                ThenBy(x => x.Value.Index).
                Select(x => x.Key).
                AsQueryable();
        }
    }
}
