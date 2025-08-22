using ClientMSystem.Data;
using ClientMSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClientMSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignUp model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input. Please fill all fields correctly.";
                return View(model);
            }

            var user = await _context.signUps
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Username not found.";
                return View(model);
            }

            if (user.Password != model.Password)
            {
                TempData["ErrorMessage"] = "Invalid password.";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.ID.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            HttpContext.Session.SetInt32("UserId", user.ID);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> UserNameIsExits(string uname)
        {
            var exists = await _context.signUps.AnyAsync(u => u.Username == uname);
            return Json(exists ? $"Username '{uname}' is already taken." : (object)true);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return View(model);
            }

            var userExists = await _context.signUps.AnyAsync(u => u.Username == model.Username);
            if (userExists)
            {
                TempData["ErrorMessage"] = "Username already exists.";
                return View(model);
            }

            var newUser = new SignUp
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Mobile = model.Mobile,
                Password = model.Password, // Consider hashing in real-world apps
                ConformPassword = model.ConformPassword
            };

            await _context.signUps.AddAsync(newUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }
    }
}
