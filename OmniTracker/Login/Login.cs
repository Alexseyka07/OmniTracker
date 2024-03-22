using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniTracker.Data;
using OmniTracker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace OmniTracker.Login
{
    public static class Login
    {
        public async static Task<User> SetLogin(OmniTrackerContext _context, User modelUser,HttpContext httpContext)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == modelUser.Email && m.Password == modelUser.Password);
            if (user == null)
            {
                return null;
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


            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);
            return user;
        }
        public static bool IsInRole(ClaimsPrincipal _user, OmniTrackerContext _context, HttpContext httpContext)
        {
           return IsInRoleAsync(_user, _context, httpContext).Result;
        }

        private async static Task<bool> IsInRoleAsync(ClaimsPrincipal _user,OmniTrackerContext _context, HttpContext httpContext)
        {
            var id = _user.Claims.FirstOrDefault(c => c.Type == "id").Value;
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Id.ToString() == id);
            if (!_user.IsInRole(user.Role))
            {
                user = await SetLogin(_context, user, httpContext);
                return false;
            }
            return true;
        }
    }
}
