namespace TaskHub.Models.InterfaceService
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Implement email sending logic here
            // Example using SMTP client or any other email service
        }
    }

}
