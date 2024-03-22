using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OmniTracker.Data;
using OmniTracker.Models;

namespace OmniTracker.Controllers
{
    public class UsersController : Controller
    {
        private readonly OmniTrackerContext _context;

        public UsersController(OmniTrackerContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> My()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id.ToString() == id);
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                return RedirectToAction("MyRequests", user.Role);
            }
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }      
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {

            if (id == null)
            {
                return RedirectToAction(nameof(My));
            }
            var user = await _context.Users.FindAsync(id);
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                var idl = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                return RedirectToAction("MyRequests", user.Role);
            }
            if (user == null)
            {
                return RedirectToAction(nameof(My));
            }
            return View(user);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, User userModel)
        {
            if (id != userModel.Id)
            {
                return RedirectToAction(nameof(My));
            }
            var user = await _context.Users.FindAsync(id);
            if (userModel != null)
            {
                user.Role = userModel.Role;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyRequests",User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllUsers(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var usersContarins = _context.Users.Where(r => r.Name!.Contains(search) || r.Email!.Contains(search));
                return View(await usersContarins.ToListAsync());
            }
            var users = _context.Users;
            return View(await users.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                var users = await _context.Users.ToListAsync();
                var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                return RedirectToAction("MyRequests", users.FirstOrDefault(c => c.Id.ToString() == id).Role);
            }
            if (user.Email != null)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(My));
            }
            return View(user);
        }

    }
}
