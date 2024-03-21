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

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> My()
        {
    

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id.ToString() == User.Claims.ToList()[1].Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Requests/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Requests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, User userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
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

    }
}
