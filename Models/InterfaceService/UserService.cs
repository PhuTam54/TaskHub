using Microsoft.EntityFrameworkCore;
using TaskHub.Data;

namespace TaskHub.Models.InterfaceService
{
    public class UserService : IUserService
    {
        private readonly TaskHubContext _context;
        private readonly IEmailService _emailService;

        public UserService(TaskHubContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            var token = Guid.NewGuid().ToString(); // Generate a random token
            user.PasswordResetToken = token;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<bool> VerifyPasswordResetTokenAsync(User user, string token)
        {
            return user.PasswordResetToken == token;
        }

        public async Task ResetPasswordAsync(User user, string newPassword)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null; // Clear the token after password reset
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

       
        public Task<User> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }

}
