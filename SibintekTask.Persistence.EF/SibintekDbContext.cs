using Microsoft.EntityFrameworkCore;
using SibintekTask.Core.Models;

namespace SibintekTask.Persistence.EF
{
    public class SibintekDbContext : DbContext
    {
        public SibintekDbContext(DbContextOptions<SibintekDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairType> RepairTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(e => e.Id);

                e.Property(e => e.Id);
                e.Property(e => e.ITN).HasMaxLength(12).HasColumnType("character varying").IsRequired(true);
                e.Property(e => e.Name).HasMaxLength(30).HasColumnType("character varying").IsRequired(true);
                e.Property(e => e.Surname).HasMaxLength(50).HasColumnType("character varying");
                e.Property(e => e.PasswordHash).HasColumnType("character varying");

                e.HasMany(e => e.ExecutorRepairs).WithOne(r => r.Executor).HasForeignKey(r => r.ExecutorId);
                e.HasMany(e => e.CustomerRepairs).WithOne(r => r.Customer).HasForeignKey(r => r.CustomerId);

                e.HasMany(e => e.Roles).WithMany(r => r.Users);
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(e => e.Id);

                e.Property(e => e.Id);
                e.Property(e => e.Name);

                e.HasMany(e => e.Users).WithMany(u => u.Roles);
            });

            modelBuilder.Entity<Vehicle>(e =>
            {
                e.HasKey(v => v.Id);

                e.Property(v => v.Id);
                e.Property(v => v.NumberPlate).HasMaxLength(10).HasColumnType("character varying").IsRequired(true);
                e.Property(v => v.MarkId);

                e.HasOne(v => v.Mark).WithMany(m => m.Vehicles).HasForeignKey(v => v.MarkId);

                e.HasMany(v => v.Repairs).WithOne(r => r.Vehicle).HasForeignKey(r => r.VehicleId);
            });

            modelBuilder.Entity<Mark>(e =>
            {
                e.HasKey(m => m.Id);

                e.Property(m => m.Id);
                e.Property(m => m.Name).HasMaxLength(30).HasColumnType("character varying").IsRequired(true);

                e.HasMany(m => m.Vehicles).WithOne(v => v.Mark).HasForeignKey(v => v.MarkId);
            });

            modelBuilder.Entity<Repair>(e =>
            {
                e.HasKey(r => r.Id);

                e.Property(r => r.Id);
                e.Property(r => r.AcceptedAt).HasDefaultValueSql("now()").HasColumnType("timestamp without time zone");
                e.Property(r => r.FinishedAt).HasColumnType("timestamp without time zone");
                e.Property(r => r.Cost);
                e.Property(r => r.Mileage);
                e.Property(r => r.RepairTypeId);
                e.Property(r => r.VehicleId);
                e.Property(r => r.CustomerId);
                e.Property(r => r.ExecutorId);

                e.HasOne(r => r.Customer).WithMany(u => u.CustomerRepairs).HasForeignKey(r => r.CustomerId);

                e.HasOne(r => r.Executor).WithMany(u => u.ExecutorRepairs).HasForeignKey(r => r.ExecutorId);

                e.HasOne(r => r.Vehicle).WithMany(v => v.Repairs).HasForeignKey(r => r.VehicleId);

                e.HasOne(r => r.RepairType).WithMany(t => t.Repairs).HasForeignKey(r => r.RepairTypeId);
            });

            modelBuilder.Entity<RepairType>(e =>
            {
                e.HasKey(t => t.Id);

                e.Property(t => t.Id);
                e.Property(t => t.Name);

                e.HasMany(t => t.Repairs).WithOne(r => r.RepairType).HasForeignKey(r => r.RepairTypeId);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
