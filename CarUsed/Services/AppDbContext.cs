using CarUsed.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarUsed.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImages> CarImages { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
    }
}
