using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
namespace WebApplication1.Service.InheritanceService;

public class SendMailService : ISendMailService
{
    private readonly MailSettings mailSettings;

    private readonly ILogger<SendMailService> logger;


    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
    {
        mailSettings = _mailSettings.Value;
        mailSettings.Mail = (Environment.GetEnvironmentVariable("MAIL_SMTP_EMAIL") ?? "").Length > 0 ? Environment.GetEnvironmentVariable("MAIL_SMTP_EMAIL") : mailSettings.Mail;
        mailSettings.Password = (Environment.GetEnvironmentVariable("MAIL_SMTP_PASSWORD") ?? "").Length > 0 ? Environment.GetEnvironmentVariable("MAIL_SMTP_PASSWORD") : mailSettings.Password;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }

    // Gửi email, theo nội dung trong mailContent
    public async Task<Response> SendMail(MailContent mailContent)
    {
        var email = new MimeMessage();
        email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
        email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
        email.To.Add(MailboxAddress.Parse(mailContent.To));
        email.Subject = mailContent.Subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = mailContent.Body;
        email.Body = builder.ToMessageBody();

        // dùng SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(email);


        }
        catch (Exception ex)
        {
            logger.LogInformation("Lỗi gửi mail, " + ex.Message);
            logger.LogError(ex.Message);
            return new Response
            {
                resultCd = 1,
                Error = ex.Message
            };
        }

        smtp.Disconnect(true);
        logger.LogInformation("send mail to " + mailContent.To);
        return new Response
        {
            resultCd = 0
        };
    }
    public async Task<Response> SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return await SendMail(new MailContent()
        {
            To = email,
            Subject = subject,
            Body = htmlMessage
        });
    }
}