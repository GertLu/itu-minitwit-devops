using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiniTwitInfra.Data;
using MiniTwitInfra.Models.DataModels;

namespace MiniTwitInfra.Controllers
{
    [IgnoreAntiforgeryToken]
    public class RegisterController : PageModel
    {
        private readonly MiniTwitContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterController(MiniTwitContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;

            Username = string.Empty;
            Password = string.Empty;
            password2 = string.Empty;
            Email = string.Empty;
        }

        [BindProperty]
        [StringLength(16)]
        [Required(ErrorMessage = "You have to enter a username")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "You have to enter a valid email address")]
        [StringLength(32)]
        [EmailAddress(ErrorMessage = "You have to enter a valid email address")]
        public string Email { get; set; }

        [BindProperty]
        [StringLength(32)]
        [Required(ErrorMessage = "You have to enter a password")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please confirm your password")]
        [StringLength(32)]
        [Compare(nameof(Password), ErrorMessage = "The two passwords do not match")]
        public string password2 { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "The username is already taken");
                return Page();
            }

            var newUser = new User
            {
                UserName = Username, // Set the user properties here
                Email = Email,
                PasswordHash = Password
            };

            string hashedPassword = _passwordHasher.HashPassword(newUser, newUser.PasswordHash);

            newUser.PasswordHash = hashedPassword;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            TempData["Message"] = "You were successfully registered and can login now";
            return RedirectToPage("/login");
        }
    }
}

