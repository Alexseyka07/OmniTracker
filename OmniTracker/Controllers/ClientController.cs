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
using OmniTracker.Login;
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

        public async Task<IActionResult> MyRequests()
        {
            var users = await _context.Users.ToListAsync();
            var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            var requests = await _context.Requests.Where(
                r => r.User.Id.ToString() == id).ToListAsync();

           if(!Login.Login.IsInRole(User, _context, HttpContext))
           {
               return RedirectToAction("MyRequests", users.FirstOrDefault(c => c.Id.ToString() == id).Role);
           }
            
               
            

            return View(requests);
        }          
        public async Task<IActionResult> Create()
        {
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                var users = await _context.Users.ToListAsync();
                var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                return RedirectToAction("MyRequests", users.FirstOrDefault(c => c.Id.ToString() == id).Role);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Request request)
        {
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                var users = await _context.Users.ToListAsync();
                var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                return RedirectToAction("MyRequests", users.FirstOrDefault(c => c.Id.ToString() == id).Role);
            }
            if (request.Description != null)
            {
                request.CreateDate = DateTime.Now;
                request.Status = "Новая";
                request.TermEimination = "не известно";
                var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                request.User = await _context.Users.FirstOrDefaultAsync(
                    u => u.Id.ToString() == id);
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyRequests));
            }
            return View(request);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                var users = await _context.Users.ToListAsync();
                var idl = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                return RedirectToAction("MyRequests", users.FirstOrDefault(c => c.Id.ToString() == idl).Role);
            }
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
        public async Task<IActionResult> Edit(int id, Request requestModel)
        {
            if (!Login.Login.IsInRole(User, _context, HttpContext))
            {
                var users = await _context.Users.ToListAsync();
                var idl = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                return RedirectToAction("MyRequests", users.FirstOrDefault(c => c.Id.ToString() == idl).Role);
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
