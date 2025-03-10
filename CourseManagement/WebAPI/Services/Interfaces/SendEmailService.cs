using Azure.Core;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using WebAPI.DTOS;
using WebAPI.Services.Interfaces;

public class SendEmailService : ISendEmail
{
    private readonly SendEmail _sendEmail;


    public SendEmailService(IOptions<SendEmail> options)
    {
        _sendEmail = options.Value;

    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.Sender = new MailboxAddress(_sendEmail.DisplayName, _sendEmail.Email);
        message.From.Add(new MailboxAddress(_sendEmail.DisplayName, _sendEmail.Email));
        message.Subject = subject;
        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder();
        builder.HtmlBody = htmlMessage;
        message.Body = builder.ToMessageBody();
        // dùng SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            smtp.Connect(_sendEmail.Host, _sendEmail.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_sendEmail.Email, _sendEmail.Password);
            await smtp.SendAsync(message);
        }

        catch (Exception ex)
        {
            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
            Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await message.WriteToAsync(emailsavefile);


        }

        smtp.Disconnect(true);
    }
}
