using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace ByLearningMimeKit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //config the message info
            //from, to and the subject, message body
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("By Learning", "no-reply-bypro@hotmail.com"));
            // send email to xxx@xxx.com
            message.To.Add(new MailboxAddress("yubin", "312519932@qq.com"));
            // subject in the email
            message.Subject = "How you doin'?";
            // subjetc body
            message.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,
                 I just wanted to let you know that Monica and I were going to go play some paintball, you in?            
                 -- Joey"
            };
            // client connection
            using (var client = new SmtpClient())
            {
                //smtp server and port, usessl or not?
                await client.ConnectAsync("smtp.office365.com", 587, false);
                // Note: only needed if the SMTP server requires authentication, email password
                await client.AuthenticateAsync("no-reply-bypro@hotmail.com", "XXXXXX");
                // send the message
                client.Send(message);
                // disconnect
                client.Disconnect(true);
            }
        }
    }
}