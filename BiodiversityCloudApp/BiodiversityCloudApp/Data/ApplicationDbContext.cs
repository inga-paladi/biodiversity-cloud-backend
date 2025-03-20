using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }

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

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Title).IsRequired().HasMaxLength(100);
            entity.Property(o => o.Species).IsRequired();
            entity.Property(o => o.Date).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Url).IsRequired();
            entity.HasOne(p => p.Observation)
                .WithMany(p => p.Photos)
                .HasForeignKey(p => p.ObservationId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);
            entity.HasOne(c => c.Observation)
                .WithMany()
                .HasForeignKey(c => c.ObservationId);
        });
    }
}