using Microsoft.EntityFrameworkCore;
using MembershipSystem.Models;
using System.Collections.Generic;

namespace MembershipSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}