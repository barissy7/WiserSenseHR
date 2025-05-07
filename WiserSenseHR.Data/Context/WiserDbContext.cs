using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;

    public class WiserDbContext : DbContext
    {
        public WiserDbContext(DbContextOptions<WiserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Announcement> Announcements { get; set; }  
        public DbSet<Department> Departments { get; set; }  
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<AssignedItem> AssignedItems { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<FoodList> FoodLists { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User Entity Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            entity.Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.Active)
                .HasDefaultValue(true);

            entity.Property(u => u.Role)
                .IsRequired();

            entity.Property(u => u.Department)
                .IsRequired();

            entity.HasOne(u => u.UserInfo)
               .WithOne(ui => ui.User)
               .HasForeignKey<UserInfo>(ui => ui.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        // UserInfo Entity Configuration
        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(ui => ui.Id);

            entity.Property(ui => ui.Id)
                .ValueGeneratedOnAdd();

            entity.Property(ui => ui.Birthday)
                .HasColumnType("date");

            entity.Property(ui => ui.JobStartDate)
                .HasColumnType("date");

            entity.Property(ui => ui.PhoneNumber)
                .HasColumnType("bigint")
                .IsRequired(false); 

            entity.HasIndex(ui => ui.PhoneNumber)
                .IsUnique()
                .HasFilter("\"PhoneNumber\" IS NOT NULL");

            entity.Property(ui => ui.JobTitle)
                .HasMaxLength(100)
                .IsRequired(false);
        });

        // Announcement Entity Configuration
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(an => an.Id);

            entity.Property(an => an.Id)
                .ValueGeneratedOnAdd();

            entity.Property(an => an.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(an => an.AnnouncementDate)
                .HasColumnType("timestamp")
                .IsRequired();

            entity.Property(an => an.Description)
                .HasColumnType("text");
        });

        // Department Entity Configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            entity.Property(d => d.Name)
                .IsRequired();
        });

        // Meeting Entity Configuration
        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .ValueGeneratedOnAdd();

            entity.Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired(false);

            entity.Property(m => m.Description)
                .HasColumnType("text")
                .IsRequired(false);

            entity.Property(m => m.MeetingDate)
                .HasColumnType("timestamp")
                .IsRequired();

            entity.Property(m => m.EndDate)
                .HasColumnType("timestamp")
                .IsRequired();
        });

        modelBuilder.Entity<AssignedItem>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.ItemNames)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.Description)
                .HasMaxLength(500);
                
            entity.HasOne(a => a.User)
                .WithMany(u => u.AssignedItems)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FoodList Entity Configuration
        modelBuilder.Entity<FoodList>(entity =>
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.Id)
                .ValueGeneratedOnAdd();

            entity.Property(f => f.ImageUrl)
                .HasMaxLength(500)  
                .IsRequired();  

            entity.Property(f => f.Description)
                .HasMaxLength(200); 
        });

        // Rule Entity Configuration
        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            entity.Property(r => r.Description)
                .HasMaxLength(2000)
                .IsRequired();
        });


        base.OnModelCreating(modelBuilder);
    }
}
    
