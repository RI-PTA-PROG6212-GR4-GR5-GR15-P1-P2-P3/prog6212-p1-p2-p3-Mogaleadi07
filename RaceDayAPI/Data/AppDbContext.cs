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
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.EmailAddress).IsRequired().HasMaxLength(255);
                entity.Property(u => u.ContactNumber).IsRequired().HasMaxLength(20);
                entity.Property(u => u.PasswordHash).IsRequired();

                entity.HasIndex(u => u.EmailAddress).IsUnique();
            });

            // Event Organiser
            modelBuilder.Entity<EventOrganiser>(entity =>
            {
                entity.ToTable("EventOrganiser");
                entity.HasKey(e => e.OrganiserID);

                entity.HasOne(e => e.User)
                      .WithOne(u => u.EventOrganiser)
                      .HasForeignKey<EventOrganiser>(e => e.UserID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Events)
                      .WithOne(ev => ev.Organiser)
                      .HasForeignKey(ev => ev.OrganiserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Participant
            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("Participant");
                entity.HasKey(p => p.ParticipantID);

                entity.HasOne(p => p.User)
                      .WithOne(u => u.Participant)
                      .HasForeignKey<Participant>(p => p.UserID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Enrolments)
                      .WithOne(e => e.Participant)
                      .HasForeignKey(e => e.ParticipantID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event", t =>
                {
                    t.HasCheckConstraint("CK_Event_EventType",
                        "EventType IN ('Walk', 'Run', 'Cycle')");
                    t.HasCheckConstraint("CK_Event_Distance",
                        "Distance > 0");
                });

                entity.HasKey(e => e.EventID);

                entity.Property(e => e.EventName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(255);
                entity.Property(e => e.EventType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Distance).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Route)
                      .WithOne(r => r.Event)
                      .HasForeignKey<EventRoute>(r => r.EventID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Categories)
                      .WithOne(c => c.Event)
                      .HasForeignKey(c => c.EventID)
                      .OnDelete(DeleteBehavior.Cascade);

                // ONE cascade path to EventEnrolment
                entity.HasMany(e => e.Enrolments)
                      .WithOne(en => en.Event)
                      .HasForeignKey(en => en.EventID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //  Event Category
            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.ToTable("EventCategories", t =>
                    t.HasCheckConstraint("CK_EventCategory_AgeOrDistance",
                        "AgeGroup IS NOT NULL OR DistanceType IS NOT NULL"));

                entity.HasKey(c => c.CategoryID);
                entity.Property(c => c.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.AgeGroup).HasMaxLength(50);
                entity.Property(c => c.DistanceType).HasMaxLength(50);

                //  Restrict (was Cascade) — avoids multiple cascade paths
                entity.HasMany(c => c.Enrolments)
                      .WithOne(e => e.Category)
                      .HasForeignKey(e => e.CategoryID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Route
            modelBuilder.Entity<EventRoute>(entity =>
            {
                entity.ToTable("Route");
                entity.HasKey(r => r.RouteID);
                entity.Property(r => r.Distance).HasColumnType("decimal(10,2)");
                entity.Property(r => r.Difficulty).HasMaxLength(50);
            });

            // Event Enrolment 
            modelBuilder.Entity<EventEnrolment>(entity =>
            {
                entity.ToTable("EventEnrolment", t =>
                    t.HasCheckConstraint("CK_EventEnrolment_Status",
                        "Status IN ('Confirmed', 'Withdrawn')"));

                entity.HasKey(e => e.EnrolmentID);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);

                entity.HasIndex(e => new { e.ParticipantID, e.EventID, e.CategoryID })
                      .IsUnique();

                entity.HasOne(e => e.Result)
                      .WithOne(r => r.Enrolment)
                      .HasForeignKey<Result>(r => r.EnrolmentID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Result
            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Results", t =>
                    t.HasCheckConstraint("CK_Result_Position", "Position > 0"));

                entity.HasKey(r => r.ResultID);
                entity.Property(r => r.FinishTime).HasColumnType("time");
                entity.HasIndex(r => r.EnrolmentID).IsUnique();
            });
        }
    }
}