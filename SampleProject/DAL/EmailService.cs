using System.Net;
using System.Net.Mail;

namespace SampleProject.DAL
{
    public class EmailService
    {

        public EmailService()
        {
                
        }


        public static void SendPublicKeyViaEmail(string OriginEmail, string password, string sourceEmail, string publicKey)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(OriginEmail, password);

            smtp.EnableSsl = false;  

            MailMessage mail= new MailMessage( new MailAddress(OriginEmail) , new MailAddress(sourceEmail));
            mail.Subject = "Public key";
            mail.Body = publicKey; 

            smtp.Send(mail);

        }
    }
}
