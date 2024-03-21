using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OmniTracker.Models;

namespace OmniTracker.Data
{
    public class OmniTrackerContext : DbContext
    {
        public OmniTrackerContext (DbContextOptions<OmniTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Request> Requests { get; set; }
    }
}
