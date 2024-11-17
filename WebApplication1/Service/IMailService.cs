using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Service
{
    public interface ISendMailService
    {
        Task<Response> SendMail(MailContent mailContent);

        Task<Response> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
