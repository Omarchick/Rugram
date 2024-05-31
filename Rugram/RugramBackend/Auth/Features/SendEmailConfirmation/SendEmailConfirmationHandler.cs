using Auth.Data;
using Auth.Data.Models;
using Auth.Services.Infrastructure.EmailSenderService;
using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using static Auth.Features.UserAuthHelper;

namespace Auth.Features.SendEmailConfirmation;

public class SendEmailConfirmationHandler(
		AppDbContext dbContext,
		IConfiguration configuration,
		IEmailSenderService emailSenderService)
	: IGrpcRequestHandler<SendEmailConfirmationRequest>
{
	public async Task<GrpcResult> Handle(
		SendEmailConfirmationRequest request,
		CancellationToken cancellationToken)
	{
		var alreadyExistUserWithThisEmail = await dbContext.Users.AsNoTracking()
			.AnyAsync(user => user.Email == request.Email, cancellationToken);

		if (alreadyExistUserWithThisEmail) return StatusCodes.Status409Conflict;


		var token = GenerateSecureToken();
		var mailConfirmationToken = new MailConfirmationToken
		{
			Email = request.Email,
			Value = token.HashSha256(),
			ValidTo = DateTime.UtcNow + TimeSpan.FromHours(int.Parse(
				configuration["MailConfirmationToken:LifetimeInHours"]!))
		};

		dbContext.MailConfirmationTokens.Add(mailConfirmationToken);
		await dbContext.SaveChangesAsync(cancellationToken);

		const string messageSubject = "Потверждение почты";
		var messageBody = GetEmailConfirmationHtmlMessageBody(request.Email, token);

		await emailSenderService.SendMessageAsync(messageSubject, messageBody, request.Email);

		return StatusCodes.Status204NoContent;
	}

	private string GetEmailConfirmationHtmlMessageBody(string email, string emailConfirmationToken)
	{
		return @"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Подтверждение почты</title>
            <style>
                body {
                    background-color: #F0F0F0;
                    font-family: Arial, sans-serif;
                }
        
                .container {
                    background-color: #FFFFFF;
                    border-radius: 5px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                    margin: 20px auto;
                    padding: 20px;
                    max-width: 400px;
                }
        
                h1 {
                    font-size: 20px;
                    margin-bottom: 20px;
                    text-align: center;
                }
        
                p {
                    margin-bottom: 20px;
                }
        
                .button {
                    background-color: #5BC0DE;
                    border-radius: 5px;
                    color: #FFFFFF;
                    display: inline-block;
                    font-size: 16px;
                    padding: 10px 20px;
                    text-decoration: none;
                }
        
                .button:hover {
                    background-color: #449DCA;
                }
            </style>
        </head>
        <body>
            <div class=""container"">
                <h1>Подтверждение почты</h1>
                <p>Спасибо вам за выбор Rugram! Пожалуйста, нажмите на кнопку ниже, чтобы продолжить регистрацию</p>
                <a class=""button"" href=""" +
		       configuration["Domain:AppUrl"] +
		       $"/auth/confirm-email?token={emailConfirmationToken}&email={email}" +
		       @""">Продолжить регистрацию</a>
            </div>
        </body>
        </html>";
	}
}