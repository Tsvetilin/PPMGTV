using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Common.Helpers
{
    public static class SuggestionsJsonResultExtensions
    {
        public static string ToSuggestionJsonResult(this IEnumerable<string> listOfNames)
        {
            var suggestions = listOfNames.
               Select(x =>
                   new Suggestion
                   {
                       Data = x,
                       Value = x
                   }).
                ToArray();

            var suggestionResponse = new SuggestionResponse
            {
                Suggestions = suggestions
            };

            var response = JsonSerializer.Serialize(suggestionResponse);

            return response.ToLower();
        }

        private class SuggestionResponse
        {
            public Suggestion[] Suggestions { get; set; }
        }

        private class Suggestion
        {
            public string Value { get; set; }
            public string Data { get; set; }
        }
    }
}
