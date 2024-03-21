using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OmniTracker.Data;
using OmniTracker.Models;

namespace OmniTracker.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly OmniTrackerContext _context;

        public ClientController(OmniTrackerContext context)
        {
            _context = context;
        }

        // GET: Requests

        public async Task<IActionResult> MyRequests()
        {

            var users = await _context.Users.ToListAsync();
            var requests = await _context.Requests.Where(r => r.User.Id.ToString() == User.Claims.ToArray()[1].Value).ToListAsync();

            return _context.Users != null ?
                        View(requests) :
                        Problem("Entity set 'OmniTrackerContext.User'  is null.");
        }
       
        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request)
        {
            if (request.Description != null)
            {
                request.CreateDate = DateTime.Now;
                request.Status = "Новая";
                request.TermEimination = "не известно";
                request.User =  _context.Users.FirstOrDefault(u => u.Id.ToString() == User.Claims.ToList()[1].Value);
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyRequests));
            }
            return View(request);
        }

        // GET: Requests/Edit/5
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

        // POST: Requests/Edit/5
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
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyRequests));
            }
            return View(request);
        }


    }
}
