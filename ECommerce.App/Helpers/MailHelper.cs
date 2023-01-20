using ECommerce.Common.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ECommerce.App.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GenericResponse<object> SendMail(string to, string subject, string body)
        {
            try
            {
                string from = _configuration["Mail:From"];
                string smtp = _configuration["Mail:Smtp"];
                string port = _configuration["Mail:Port"];
                string sSLPort = _configuration["Mail:SSLPort"];
                string password = _configuration["Mail:Password"];



                MimeMessage message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(from));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), SecureSocketOptions.StartTls);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new GenericResponse<object> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<object>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Result = ex
                };
            }
        }
        public GenericResponse<object> SendMailAttachments(string to, string subject, string body, string FilePath)
        {
            try
            {
                string from = _configuration["Mail:From"];
                string smtp = _configuration["Mail:Smtp"];
                string port = _configuration["Mail:Port"];
                string sSLPort = _configuration["Mail:SSLPort"];
                string password = _configuration["Mail:Password"];
                string MailOrders = _configuration["Mail:MailOrders"];



                MimeMessage message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(from));
                message.To.Add(new MailboxAddress(MailOrders, to));
                message.Cc.Add(new MailboxAddress("marcos.nava@comtecom.com.mx", "marcosnavaramirez@gmail.com"));
                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                bodyBuilder.Attachments.Add(@FilePath, new ContentType("application", "xlsx"));

                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), SecureSocketOptions.None);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return new GenericResponse<object> { IsSuccess = true };
            }
            catch (Exception exMail)
            {

                return new GenericResponse<object>
                {
                    IsSuccess = false,
                    ErrorMessage = exMail.Message,
                    Result = exMail
                };
            }
        }
    }
}
