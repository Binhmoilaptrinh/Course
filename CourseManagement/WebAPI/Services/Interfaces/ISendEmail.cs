namespace WebAPI.Services.Interfaces
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, string subject, string Htmlmessage);

    }
}
