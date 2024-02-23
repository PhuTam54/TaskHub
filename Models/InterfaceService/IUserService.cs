namespace TaskHub.Models.InterfaceService
{
        public interface IUserService
        {
            Task<User> GetUserByEmailAsync(string email);
            Task<string> GeneratePasswordResetTokenAsync(User user);
            Task<bool> VerifyPasswordResetTokenAsync(User user, string token);
            Task ResetPasswordAsync(User user, string newPassword);
        Task<User> GetUserByIdAsync(string userId);
    }

        public interface IEmailService
    {
            Task SendEmailAsync(string email, string subject, string message);
        }
}
