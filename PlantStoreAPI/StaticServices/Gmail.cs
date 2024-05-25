using System.Net;
using System.Net.Mail;

namespace PlantStoreAPI.StaticServices
{
    public class Gmail
    {
        private static string _email = "pkplantstore@gmail.com";
        private static string _password = "cmccrjvgehntjhvv";

        public static bool SendEmail(string subject, string content, List<string> toMail)
        {
            try
            {
                var message = new MailMessage();
                var smtp = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = _email,
                        Password = _password
                    },
                };
                var fromAddress = new MailAddress(_email, "PK Plant Store");
                message.From = fromAddress;

                foreach (var mail in toMail)
                {
                    message.To.Add(mail);
                }

                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
