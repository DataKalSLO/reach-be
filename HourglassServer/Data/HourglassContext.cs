using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data
{
    public partial class HourglassContext : DbContext
    {
        public HourglassContext()
        {
        }

        private readonly IConfiguration _config;

        public HourglassContext(DbContextOptions<HourglassContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<BookmarkGeoMap> BookmarkGeoMap { get; set; }
        public virtual DbSet<BookmarkGraph> BookmarkGraph { get; set; }
        public virtual DbSet<BookmarkStory> BookmarkStory { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<DatasetMetaData> DatasetMetaData { get; set; }
        public virtual DbSet<GeoMap> GeoMap { get; set; }
        public virtual DbSet<GeoMapBlock> GeoMapBlock { get; set; }
        public virtual DbSet<GeoMapTables> GeoMapTables { get; set; }
        public virtual DbSet<Graph> Graph { get; set; }
        public virtual DbSet<GraphBlock> GraphBlock { get; set; }
        public virtual DbSet<GraphSource> GraphSource { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Point> Point { get; set; }
        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<StoryCategory> StoryCategory { get; set; }
        public virtual DbSet<TextBlock> TextBlock { get; set; }
        public virtual DbSet<ZipArea> ZipArea { get; set; }
        public virtual DbSet<ZipCode> ZipCode { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("HourglassDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum(null, "geo_type", new[] { "city", "zip", "county" })
                .HasPostgresEnum(null, "graph_category", new[] { "Industry", "Demographics", "Assets", "Education", "Housing" });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("area_pkey");

                entity.ToTable("area");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Coordinates).HasColumnName("coordinates");
            });

            modelBuilder.Entity<BookmarkGeoMap>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GeoMapId })
                    .HasName("geo_map_bookmark_pkey");

                entity.ToTable("bookmark_geo_map");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.Property(e => e.GeoMapId)
                    .HasColumnName("geo_map_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.GeoMap)
                    .WithMany(p => p.BookmarkGeoMap)
                    .HasForeignKey(d => d.GeoMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("geo_map_bookmark_geo_map_id_fkey");
            });

            modelBuilder.Entity<BookmarkGraph>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GraphId })
                    .HasName("graph_bookmark_pkey");

                entity.ToTable("bookmark_graph");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.Property(e => e.GraphId)
                    .HasColumnName("graph_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.Graph)
                    .WithMany(p => p.BookmarkGraph)
                    .HasForeignKey(d => d.GraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graph_bookmark_graph_id_fkey");
            });

            modelBuilder.Entity<BookmarkStory>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.StoryId })
                    .HasName("story_bookmark_pkey");

                entity.ToTable("bookmark_story");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.Property(e => e.StoryId)
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.BookmarkStory)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("story_bookmark_story_id_fkey");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryName)
                    .HasName("category_pkey");

                entity.ToTable("category");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("category_name")
                    .HasMaxLength(500);

                entity.Property(e => e.CategoryDescription)
                    .HasColumnName("category_description")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<DatasetMetaData>(entity =>
            {
                entity.HasKey(e => e.TableName)
                    .HasName("datasetmetadata_pkey");

                entity.ToTable("dataset_meta_data");

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(500);

                entity.Property(e => e.CityColumn).HasColumnName("city_column");

                entity.Property(e => e.ColumnNames).HasColumnName("column_names");

                entity.Property(e => e.CountyColumn).HasColumnName("county_column");

                entity.Property(e => e.DataTypes).HasColumnName("data_types");

                entity.Property(e => e.ZipCodeColumn).HasColumnName("zip_code_column");
            });

            modelBuilder.Entity<GeoMap>(entity =>
            {
                entity.ToTable("geo_map");

                entity.Property(e => e.GeoMapId)
                    .HasColumnName("geo_map_id")
                    .HasMaxLength(36)
                    .IsFixedLength();
            });

            modelBuilder.Entity<GeoMapBlock>(entity =>
            {
                entity.HasKey(e => e.BlockId)
                    .HasName("geo_map_block_pkey");

                entity.ToTable("geo_map_block");

                entity.Property(e => e.BlockId)
                    .HasColumnName("block_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.BlockPosition).HasColumnName("block_position");

                entity.Property(e => e.GeoMapId)
                    .IsRequired()
                    .HasColumnName("geo_map_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.StoryId)
                    .IsRequired()
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.GeoMap)
                    .WithMany(p => p.GeoMapBlock)
                    .HasForeignKey(d => d.GeoMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("geo_map_block_geo_map_id_fkey");

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.GeoMapBlock)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("geo_map_block_story_id_fkey");
            });

            modelBuilder.Entity<GeoMapTables>(entity =>
            {
                entity.HasKey(e => new { e.GeoMapId, e.TableName })
                    .HasName("geo_map_tables_pkey");

                entity.ToTable("geo_map_tables");

                entity.Property(e => e.GeoMapId)
                    .HasColumnName("geo_map_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.HasOne(d => d.GeoMap)
                    .WithMany(p => p.GeoMapTables)
                    .HasForeignKey(d => d.GeoMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("geo_map_tables_geo_map_id_fkey");

                entity.HasOne(d => d.TableNameNavigation)
                    .WithMany(p => p.GeoMapTables)
                    .HasForeignKey(d => d.TableName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("geo_map_tables_table_name_fkey");
            });

            modelBuilder.Entity<Graph>(entity =>
            {
                entity.ToTable("graph");

                entity.Property(e => e.GraphId)
                    .HasColumnName("graph_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.GraphOptions)
                    .HasColumnName("graph_options")
                    .HasColumnType("jsonb");

                entity.Property(e => e.GraphTitle)
                    .IsRequired()
                    .HasColumnName("graph_title")
                    .HasMaxLength(500);

                entity.Property(e => e.SnapshotUrl)
                    .HasColumnName("snapshot_url")
                    .HasMaxLength(500);

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Graph)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("graph_user_id_fkey");
            });

            modelBuilder.Entity<GraphBlock>(entity =>
            {
                entity.HasKey(e => e.BlockId)
                    .HasName("graph_block_pkey");

                entity.ToTable("graph_block");

                entity.Property(e => e.BlockId)
                    .HasColumnName("block_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.BlockPosition).HasColumnName("block_position");

                entity.Property(e => e.GraphId)
                    .IsRequired()
                    .HasColumnName("graph_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.StoryId)
                    .IsRequired()
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.Graph)
                    .WithMany(p => p.GraphBlock)
                    .HasForeignKey(d => d.GraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graph_block_graph_id_fkey");

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.GraphBlock)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graph_block_story_id_fkey");
            });

            modelBuilder.Entity<GraphSource>(entity =>
            {
                entity.HasKey(e => new { e.GraphId, e.SeriesType })
                    .HasName("graph_source_pkey");

                entity.ToTable("graph_source");

                entity.Property(e => e.GraphId)
                    .HasColumnName("graph_id")
                    .HasMaxLength(36);

                entity.Property(e => e.SeriesType)
                    .HasColumnName("series_type")
                    .HasMaxLength(10);

                entity.Property(e => e.ColumnNames).HasColumnName("column_names");

                entity.Property(e => e.DatasetName)
                    .HasColumnName("dataset_name")
                    .HasMaxLength(500);

                entity.HasOne(d => d.DatasetNameNavigation)
                    .WithMany(p => p.GraphSource)
                    .HasForeignKey(d => d.DatasetName)
                    .HasConstraintName("graph_source_datasetname_fkey");

                entity.HasOne(d => d.Graph)
                    .WithMany(p => p.GraphSource)
                    .HasForeignKey(d => d.GraphId)
                    .HasConstraintName("graph_source_graphid_fkey");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.TableName })
                    .HasName("location_pkey");

                entity.ToTable("location");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.PointId).HasColumnName("point_id");

                entity.HasOne(d => d.Point)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.PointId)
                    .HasConstraintName("location_point_id_fkey");

                entity.HasOne(d => d.TableNameNavigation)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.TableName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("location_table_name_fkey");
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

                entity.Property(e => e.NotificationsEnabled).HasColumnName("notifications_enabled");

                entity.Property(e => e.Occupation)
                    .HasColumnName("occupation")
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

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("point");

                entity.HasIndex(e => e.Id)
                    .HasName("point_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<Story>(entity =>
            {
                entity.ToTable("story");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_story_user_id_fkey");

                entity.Property(e => e.StoryId)
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.DateCreated).HasColumnName("date_created");

                entity.Property(e => e.DateLastEdited).HasColumnName("date_last_edited");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.PublicationStatus)
                    .IsRequired()
                    .HasColumnName("publication_status")
                    .HasMaxLength(10);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(250);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Story)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("story_user_id_fkey");
            });

            modelBuilder.Entity<StoryCategory>(entity =>
            {
                entity.HasKey(e => new { e.StoryId, e.CategoryName })
                    .HasName("story_category_pkey");

                entity.ToTable("story_category");

                entity.Property(e => e.StoryId)
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.CategoryName)
                    .HasColumnName("category_name")
                    .HasMaxLength(500);

                entity.HasOne(d => d.CategoryNameNavigation)
                    .WithMany(p => p.StoryCategory)
                    .HasForeignKey(d => d.CategoryName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("story_category_category_name_fkey");

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.StoryCategory)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("story_category_story_id_fkey");
            });

            modelBuilder.Entity<TextBlock>(entity =>
            {
                entity.HasKey(e => e.BlockId)
                    .HasName("text_block_pkey");

                entity.ToTable("text_block");

                entity.Property(e => e.BlockId)
                    .HasColumnName("block_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.BlockPosition).HasColumnName("block_position");

                entity.Property(e => e.EditorState)
                    .IsRequired()
                    .HasColumnName("editor_state")
                    .HasMaxLength(100000);

                entity.Property(e => e.StoryId)
                    .IsRequired()
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.TextBlock)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("text_block_story_id_fkey");
            });

            modelBuilder.Entity<ZipArea>(entity =>
            {
                entity.HasKey(e => new { e.Zip, e.Area })
                    .HasName("zip_area_pkey");

                entity.ToTable("zip_area");

                entity.Property(e => e.Zip).HasColumnName("zip");

                entity.Property(e => e.Area)
                    .HasColumnName("area")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.ZipArea)
                    .HasForeignKey(d => d.Area)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("zip_area_area_fkey");

                entity.HasOne(d => d.ZipNavigation)
                    .WithMany(p => p.ZipArea)
                    .HasForeignKey(d => d.Zip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("zip_area_zip_fkey");
            });

            modelBuilder.Entity<ZipCode>(entity =>
            {
                entity.HasKey(e => e.Zip)
                    .HasName("zip_code_pkey");

                entity.ToTable("zip_code");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .ValueGeneratedNever();

                entity.Property(e => e.Coordinates).HasColumnName("coordinates");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
