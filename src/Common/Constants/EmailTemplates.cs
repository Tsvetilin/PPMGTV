namespace Common.Constants
{
    public static class EmailTemplates
    {
        //About,Description, SenderName, SenderEmail, OtherContactInfo
        public const string ContactLetterEmail = @"<h5>Ново писмо беше получено!</h5>
<p>Относно: {0}</p>
<p>Описание: {1}</p>
<p>От: {2}</p>
<p>Имейл: {3}</p>
<p>Друга информация: {4}</p>";

        //Title
        public const string SubscriptionLetterEmail = @"<p>Ново видео беше току-що качено - {0}! Може да го гледате на <a href=""https://ppmgtv.com"">PPMGTV.com</a></p>";
    }
}
