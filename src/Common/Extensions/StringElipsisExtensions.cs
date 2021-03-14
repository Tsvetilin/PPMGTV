using System.Linq;

namespace Common.Extensions
{
    public static class StringElipsisExtensions
    {
        public static string AddElipsisAtLength(this string text, int length)
        {
            var words = text.Split().Select(x => (x, x.Length)).ToList();
            var currentLength = 0;

            for (int i = 0; i < words.Count; i++)
            {

                if (currentLength + words[i].Length > length)
                {
                    return $"{string.Join(" ", words.Select(x => x.x).Take(i))}...";
                }
                currentLength += words[i].Length;
            }

            return text;
        }
    }
}
