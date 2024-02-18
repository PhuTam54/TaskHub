using System.Net;
using System.Net.Mail;

namespace TaskHub.MailUtils
{
    public class MailUtils
    {
        public static async Task<string> SendMail(string _from, string _to, string _subject, string _body)
        {
            MailMessage mailMessage = new MailMessage(_from, _to, _subject, _body);
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            mailMessage.ReplyToList.Add(new MailAddress(_from));
            mailMessage.Sender = new MailAddress(_from, "FPT Aptech");

            using var smtpClient = new SmtpClient("localhost");

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return "Send mail successfully";
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Send mail failded " + ex.Message;
            }
        }

        public static async Task<string> SendGmail(string _from, string _to, string _subject, string _body,
            string _gmail, string _password)
        {
            MailMessage mailMessage = new MailMessage(_from, _to, _subject, _body);
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            mailMessage.ReplyToList.Add(new MailAddress(_from));
            mailMessage.Sender = new MailAddress(_from, "FPT Aptech");

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_gmail, _password);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return "Send mail successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Send mail failded " + ex.Message;
            }
        }
    }
}
