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
               .HasIndex(u => u.EmailAddress).IsUnique();

            modelBuilder.Entity<EventOrganiser>()
                .HasOne(eo => eo.User)
                .WithOne(u => u.EventOrganiser)
                .HasForeignKey<EventOrganiser>(eo => eo.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.User)
                .WithOne(u => u.Participant)
                .HasForeignKey<Participant>(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventEnrolment>()
                .HasIndex(e => new { e.ParticipantID, e.EventID, e.CategoryID })
                .IsUnique();

            modelBuilder.Entity<EventRoute>()
                .HasIndex(r => r.EventID).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}