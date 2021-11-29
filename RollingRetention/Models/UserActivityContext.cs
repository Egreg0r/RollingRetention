using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;




namespace RollingRetention.Models
{
    public class UserActivityContext : DbContext
    {
        public DbSet<UserActivity> userActivitys { get; set; }

        public UserActivityContext(DbContextOptions<UserActivityContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
            
            

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
