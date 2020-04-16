using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class HourglassContext : DbContext
    {
        private IConfiguration _config;

        public HourglassContext() { }

        public HourglassContext(DbContextOptions<HourglassContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Counties> Counties { get; set; }
        public virtual DbSet<Degrees> Degrees { get; set; }
        public virtual DbSet<DegreesAwarded> DegreesAwarded { get; set; }
        public virtual DbSet<Dummy> Dummy { get; set; }
        public virtual DbSet<Universities> Universities { get; set; }
        public virtual DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("HourglassDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Counties>(entity =>
            {
                entity.HasKey(e => e.IdCounty);

                entity.Property(e => e.IdCounty)
                    .HasColumnName("ID_County")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Degrees>(entity =>
            {
                entity.HasKey(e => new { e.Gender, e.Year, e.IdUniversity });

                entity.Property(e => e.Gender)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Year)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.IdUniversity)
                    .HasColumnName("ID_University")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Completions).HasColumnType("int(11)");

                entity.Property(e => e.University)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DegreesAwarded>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.IdCounty });

                entity.Property(e => e.Year).HasColumnType("int(11)");

                entity.Property(e => e.IdCounty)
                    .HasColumnName("ID_County")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Associates).HasColumnType("int(11)");

                entity.Property(e => e.Bachelor).HasColumnType("int(11)");

                entity.Property(e => e.HsDiploma)
                    .HasColumnName("HS_Diploma")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NumNoDiploma)
                    .HasColumnName("Num_No_Diploma")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TotalPopulation)
                    .HasColumnName("Total_Population")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Dummy>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Universities>(entity =>
            {
                entity.HasKey(e => e.IdUniversity);

                entity.Property(e => e.IdUniversity)
                    .HasColumnName("ID_University")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.IdGeography)
                    .HasColumnName("ID_Geography")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PublicPrivate)
                    .HasColumnName("Public_Private")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("person_pkey");

                entity.ToTable("person");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasMaxLength(128);

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasMaxLength(64);
            });
        }
    }
}
