using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace HourglassServer.Data
{
    public partial class CentralToastContext : DbContext
    {
        private IConfiguration _config;

        public CentralToastContext(DbContextOptions<CentralToastContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Counties> Counties { get; set; }
        public virtual DbSet<Degrees> Degrees { get; set; }
        public virtual DbSet<DegreesAwarded> DegreesAwarded { get; set; }
        public virtual DbSet<Dummy> Dummy { get; set; }
        public virtual DbSet<Universities> Universities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_config.GetConnectionString("CentralToastDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Counties>(entity =>
            {
                entity.HasKey(e => e.IdCounty);

                entity.ToTable("Counties", "central_toast");

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

                entity.ToTable("Degrees", "central_toast");

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

                entity.ToTable("DegreesAwarded", "central_toast");

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
                entity.ToTable("dummy", "central_toast");

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

                entity.ToTable("Universities", "central_toast");

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
        }
    }
}
