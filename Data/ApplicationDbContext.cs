using JobTrackerAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerAPI.Data;

/// <summary>
/// Entity Framework Core database context for the Job Tracker application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<InterviewRound> InterviewRounds => Set<InterviewRound>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).HasMaxLength(255);
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Role).HasMaxLength(50).HasDefaultValue("User");
        });

        // ── Company ───────────────────────────────────────────────────────
        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(200);
            entity.Property(c => c.Website).HasMaxLength(500);
            entity.Property(c => c.Industry).HasMaxLength(100);
            entity.Property(c => c.Location).HasMaxLength(200);
        });

        // ── JobApplication ────────────────────────────────────────────────
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.Property(j => j.JobTitle).HasMaxLength(200);
            entity.Property(j => j.CompanyName).HasMaxLength(200);
            entity.Property(j => j.JobLocation).HasMaxLength(200);
            entity.Property(j => j.JobUrl).HasMaxLength(1000);

            // Convert enum to string for readability in the DB
            entity.Property(j => j.Status)
                  .HasConversion<string>()
                  .HasMaxLength(50);

            // One User → Many Applications
            entity.HasOne(j => j.User)
                  .WithMany(u => u.JobApplications)
                  .HasForeignKey(j => j.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // One Company → Many Applications (nullable FK)
            entity.HasOne(j => j.Company)
                  .WithMany(c => c.JobApplications)
                  .HasForeignKey(j => j.CompanyId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ── InterviewRound ────────────────────────────────────────────────
        modelBuilder.Entity<InterviewRound>(entity =>
        {
            entity.Property(i => i.InterviewType)
                  .HasConversion<string>()
                  .HasMaxLength(50);

            entity.Property(i => i.Result)
                  .HasConversion<string>()
                  .HasMaxLength(50);

            entity.Property(i => i.Interviewer).HasMaxLength(200);

            // One Application → Many Interview Rounds
            entity.HasOne(i => i.JobApplication)
                  .WithMany(j => j.InterviewRounds)
                  .HasForeignKey(i => i.JobApplicationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Seed Data ─────────────────────────────────────────────────────
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed an admin user (password: Admin@12345)
       modelBuilder.Entity<User>().HasData(new User
{
    Id = 1,
    FirstName = "Admin",
    LastName = "User",
    Email = "admin@jobtracker.com",
    PasswordHash = "$2a$11$zMOGMQMpAeHtRuoTq2GZBO7AEMfJjQRuhkGJA3VZnfLEBBBtaUTNu",
    Role = "Admin",
    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
});

        // Seed sample companies
        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 1,
                Name = "Acme Corp",
                Website = "https://acme.com",
                Industry = "Technology",
                Location = "San Francisco, CA",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 2,
                Name = "Tech Solutions Inc.",
                Website = "https://techsolutions.com",
                Industry = "Software",
                Location = "New York, NY",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
