using System;

namespace Common.Helpers
{
    public static class IdGenerator
    {
        public static string GenerateShortId(this Guid guid)
        {
            var base64Guid = Convert.ToBase64String(guid.ToByteArray());

            // Replace URL unfriendly characters
            base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');

            // Remove the trailing == and shorten the length to 16
            return base64Guid[0..^2][0..^6];
        }
    }
}
