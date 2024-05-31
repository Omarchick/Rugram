namespace Auth.Services.Infrastructure.EmailSenderService;

public interface IEmailSenderService
{
	public Task SendMessageAsync(string subject, string body, string sendTo);
}