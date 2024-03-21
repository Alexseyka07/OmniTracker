using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniTracker.Data;
using OmniTracker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace OmniTracker.Controllers
{
    public class AccessController : Controller
    {
        private readonly OmniTrackerContext _context;
        public AccessController(OmniTrackerContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity.IsAuthenticated) 
                return RedirectToAction("MyRequests", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User modelUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == modelUser.Email && m.Password == modelUser.Password);

            if (user == null)
            {
                ViewData["ValidateMessage"] = "Пользователь не найден";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
           
           
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
                
               
            };
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);
           
            return RedirectToAction("MyRequests", user.Role);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
