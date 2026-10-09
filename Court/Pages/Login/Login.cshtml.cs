using CourtDataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CourtDataAccess.DbModel;
using Microsoft.EntityFrameworkCore;

namespace Court.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUser _user;
        private readonly AppDbContext _context;

        public LoginModel(IUser user, AppDbContext context)
        {
            _user = user;
            _context = context;
        }


        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }


        public string ErrorMessage { get; set; }


        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync("MyCookie");
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _user.Login(UserName, Password);

            if (user == null)
            {
                ErrorMessage = "Invalid user name or password";
                return Page();
            }

            // Get the role name from the database
            var userRoleName = await _context.Roles
                .Where(r => r.Id == user.RoleId)
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userRoleName))
            {
                ErrorMessage = "User role not found.";
                return Page();
            }

            // Create claims for the logged-in user
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, userRoleName),

        new Claim("UserType", userRoleName)
    };

            var identity = new ClaimsIdentity(claims, "MyCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookie", principal);

            TempData["LoginSuccess"] = "Login successful!";

            return RedirectToPage("/Dashboard");
        }
    }
    }