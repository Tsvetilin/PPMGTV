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
        public const string SubscriptionLetterEmail = @"<p>Здравейте,<br>
<br>
Ново видео беше току-що качено - {0}!<br>
Може да го гледате на <a href=""https://ppmgtv.com"">PPMGTV.com</a><br>
или в YouTube канала на телевизията - <a href=""https://www.youtube.com/channel/UCMb5RWFF9pdl5VzSpuc7bqw"">PPMGTV</a></p>
<br>
<p>Благодарим Ви за доверието!</p>
<br>
<p>Ако искате да се свържете с нас, моля попълнете контактната форма на сайта - ppmgtv.com/contact, или ни пишете на contacts@ppmgtv.com</p>
<br>
<br>
<p>Това е автоматично генерирано съобщение, Вашите отговори няма да бъдат прочетени!</p>
";
    }
}
