using System.Net;
using System.Net.Mail;

namespace Auth.Services.Infrastructure.EmailSenderService;

public class EmailSenderService(IConfiguration configuration) : IEmailSenderService
{
	public async Task SendMessageAsync(string subject, string body, string sendTo)
	{
		var from = new MailAddress(configuration["EmailConfig:Sender"]!,
			configuration["EmailConfig:SenderName"]!);
		var to = new MailAddress(sendTo);
		var message = new MailMessage(from, to)
		{
			Subject = subject,
			Body = body,
			IsBodyHtml = true
		};

		var smtp = new SmtpClient(configuration["SmtpSettings:SmtpAddress"],
			int.Parse(configuration["SmtpSettings:Port"]!))
		{
			Credentials = new NetworkCredential(configuration["EmailConfig:Sender"]!,
				configuration["EmailConfig:SenderPassword"]!),
			EnableSsl = true,
			UseDefaultCredentials = false,
		};

		await smtp.SendMailAsync(message);
	}
}