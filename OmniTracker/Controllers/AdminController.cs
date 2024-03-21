using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OmniTracker.Data;
using OmniTracker.Models;

namespace OmniTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly OmniTrackerContext _context;

        public AdminController(OmniTrackerContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> MyRequests(string search)
        {
            var users = await _context.Users.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                var requestsContarins = _context.Requests.Where(r => r.User.Name!.Contains(search) || r.User.Email!.Contains(search) || r.Description!.Contains(search));
                return View(await requestsContarins.ToListAsync());
            }
            var requests = _context.Requests;
            return View(await requests.ToListAsync());

        }


        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Request requestModel)
        {
            if (id != requestModel.Id)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            if (requestModel.Description != null)
            {
                request.Description = requestModel.Description;
                request.Status = requestModel.Status;
                request.TermEimination = requestModel.TermEimination;
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyRequests));
            }
            return View(request);
        }
    }
}
