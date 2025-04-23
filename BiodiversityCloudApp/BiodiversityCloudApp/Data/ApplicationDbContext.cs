using BiodiversityCloudApp.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<ObservationRecord> ObservationRecords { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Animal> Animals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired();
        });
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "VasileAdmin",
                Email = "vasile@biodiversity.com",
                PasswordHash = "hashedpassword", // Use a proper hashing method
                Role = "Admin"
            }
        );

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Title).IsRequired().HasMaxLength(100);
            entity.HasMany(o => o.Records)
                    .WithOne(r => r.Observation)
                    .HasForeignKey(r => r.ObservationId)
                    .OnDelete(DeleteBehavior.Cascade);
            entity.OwnsOne(o => o.EnvironmentalConditions, ec =>
            {
                ec.Property(e => e.Temperature).IsRequired();
                ec.Property(e => e.Humidity).IsRequired();
                ec.Property(e => e.WindSpeed).IsRequired();
                ec.Property(e => e.AdditionalDetails).IsRequired();
            });
            entity.OwnsOne(o => o.StartLocation, l =>
            {
                l.Property(e => e.Latitude).IsRequired();
                l.Property(e => e.Longitude).IsRequired();
            });
            entity.OwnsOne(o => o.EndLocation, l =>
            {
                l.Property(e => e.Latitude).IsRequired();
                l.Property(e => e.Longitude).IsRequired();
            });
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Url).IsRequired();
            // entity.HasOne(p => p.Observation)
            //     .WithMany(p => p.Photos)
            //     .HasForeignKey(p => p.ObservationId);
        });

        modelBuilder.Entity<ObservationRecord>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.OwnsOne(o => o.Location, l =>
            {
                l.Property(e => e.Latitude).IsRequired();
                l.Property(e => e.Longitude).IsRequired();
            });
            entity.HasOne(m => m.Observation)
                  .WithMany(o => o.Records)
                  .HasForeignKey(m => m.ObservationId);

            // entity.HasOne(m => m.Animal)
            //       .WithMany(m => m.ObservationRecords)
            //       .HasForeignKey(m => m.AnimalId)
            //       .OnDelete(DeleteBehavior.Cascade);

        });


        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name).IsRequired();
            entity.Property(a => a.Description).IsRequired();
            entity.Property(a => a.ScientificName).IsRequired();
        });
        modelBuilder.Entity<Animal>().HasData(
            new Animal
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "White Stork",
                ScientificName = "Ciconia ciconia",
                Description = "Large migratory bird with long legs and a long beak.",
                ImageUrl = "https://example.com/stork.jpg",
                Category = "Bird"
            },
            new Animal
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Red Fox",
                ScientificName = "Vulpes vulpes",
                Description = "Common fox species known for its reddish fur.",
                ImageUrl = "https://example.com/redfox.jpg",
                Category = "Mammal"
            },
            new Animal
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Common Frog",
                ScientificName = "Rana temporaria",
                Description = "A widespread amphibian found in wetlands.",
                ImageUrl = "https://example.com/frog.jpg",
                Category = "Amphibian"
            }
        );

    }
}