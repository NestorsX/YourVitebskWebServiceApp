using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace YourVitebskWebServiceApp.APIServices
{
    public static class SMTPService
    {
        public static async Task SendPasswordByEmail(string email, string firstName, string newPassword)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("yourvitebsk.by", "noreply.yourvitebsk@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Ваш новый пароль";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<p style='font-size: 14pt;'>Здравствуйте, {firstName}!<br>" +
                $"Вы забыли свой пароль и мы сделали вам новый. " +
                $"Ваш новый пароль:<br><br>" +
                $"<b style='font-size: 16pt;'>{newPassword}</b><br><br>" +
                $"Используйте его для входа в свой аккаунт. Не забудьте поменять пароль в профиле после входа.<p>"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 587);
                await client.AuthenticateAsync("noreply.yourvitebsk@mail.ru", "afgRfMVHnZ019aF1w1c6");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
