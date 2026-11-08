using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;

namespace RaceDayAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EventOrganiser> EventOrganisers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventRoute> Routes { get; set; }
        public DbSet<EventEnrolment> EventEnrolments { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .ToTable("User")
               .HasIndex(u => u.EmailAddress).IsUnique();

            modelBuilder.Entity<EventOrganiser>()
                .ToTable("EventOrganiser")
                .HasOne(eo => eo.User)
                .WithOne(u => u.EventOrganiser)
                .HasForeignKey<EventOrganiser>(eo => eo.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Participant>()
                .ToTable("Participant")
                .HasOne(p => p.User)
                .WithOne(u => u.Participant)
                .HasForeignKey<Participant>(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .ToTable("Event");

            modelBuilder.Entity<EventCategory>()
                .ToTable("EventCategories")
                .HasKey(ec => ec.CategoryID);

            modelBuilder.Entity<EventRoute>()
                .ToTable("Route")
                .HasKey(r => r.RouteID);

            modelBuilder.Entity<EventEnrolment>()
                .ToTable("EventEnrolment")
                .HasKey(e => e.EnrolmentID);

            modelBuilder.Entity<EventEnrolment>()
                .HasIndex(e => new { e.ParticipantID, e.EventID, e.CategoryID })
                .IsUnique();

            modelBuilder.Entity<EventRoute>()
                .HasIndex(r => r.EventID).IsUnique();

            modelBuilder.Entity<Result>()
                .ToTable("Results")
                .HasKey(r => r.ResultID);

            modelBuilder.Entity<EventEnrolment>()
                .HasOne(e => e.Result)
                .WithOne(r => r.Enrolment)
                .HasForeignKey<Result>(r => r.EnrolmentID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}