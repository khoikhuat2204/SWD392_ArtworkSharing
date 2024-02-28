using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Context
{
    public class ASPContext : DbContext
    {
        public ASPContext() { }

        public ASPContext(DbContextOptions<ASPContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:ASPDB"];

            return strConn;
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Rating>? Ratings { get; set; }
        public DbSet<Reservation>? Reservations { get; set; }
        public DbSet<Package>? Packages { get; set; }
        public DbSet<Artwork>? Artworks { get; set; }
        public DbSet<ArtworkType>? ArtworkTypes { get; set; }
        public DbSet<ArtworkStatus>? ArtworkStatuses { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Report>? Reports { get; set; }
        public DbSet<ReportCause>? ReportCauses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Artwork>()
                .HasOne(r => r.Artist)
                .WithMany(u => u.Artworks)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Other configurations...

            base.OnModelCreating(modelBuilder);
        }
    }
}
