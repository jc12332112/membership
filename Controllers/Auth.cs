using Microsoft.AspNetCore.Mvc;
using MembershipSystem.Models;
using MembershipSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using membership.Service;
using membership.Service.internalinterface;

namespace MembershipSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

      
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                return BadRequest("User exist");
            }
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest("User exist");
            }

            var user = new User
            {
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Birthday = model.BirthDate,
                PasswordHash = HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var subject = "Welcome to Membership System";
            var message = $"Hello {user.Username}, thank you for registering!"; 
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return Ok("success");
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                return Unauthorized("username or password error");
            }

            return Ok("success");
        }
        [HttpPost("changepassword")]
        public async Task<IActionResult> changepassword(changePassword model)
        {
            User user;
            
            user = await Find_user_by_username(model.Username);
            if (user == null) { user = await Find_user_by_email(model.Email); }
            if (user == null) { return Unauthorized("username or email error"); }
            updatepassword(user, model.NewPassword);
            return Ok("success");
        }

        //use email to reset password

        private async Task<bool> updatepassword(User user,string password) {
            if (user == null)
            {
                return false;
            }
            user.PasswordHash = HashPassword(password);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<User> Find_user_by_email(string email) {
           return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);        
        }

        private async Task<User> Find_user_by_username(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
        }


        //Hash
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

      
        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}