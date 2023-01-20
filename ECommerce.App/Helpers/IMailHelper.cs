using ECommerce.Common.Responses;

namespace ECommerce.App.Helpers
{
    public interface IMailHelper
    {
        GenericResponse<object> SendMail(string to, string subject, string body);
        GenericResponse<object> SendMailAttachments(string to, string subject, string body, string FilePath);
    }
}
