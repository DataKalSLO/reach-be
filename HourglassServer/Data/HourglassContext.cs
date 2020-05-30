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
        public virtual DbSet<DefaultGraph> DefaultGraph { get; set; }
        public virtual DbSet<GeoMap> GeoMap { get; set; }
        public virtual DbSet<GeoMapBlock> GeoMapBlock { get; set; }
        public virtual DbSet<GeoMapTables> GeoMapTables { get; set; }
        public virtual DbSet<Graph> Graph { get; set; }
        public virtual DbSet<GraphBlock> GraphBlock { get; set; }
        public virtual DbSet<GraphSource> GraphSource { get; set; }
        public virtual DbSet<ImageBlock> ImageBlock { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Point> Point { get; set; }
        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<StoryCategory> StoryCategory { get; set; }
        public virtual DbSet<StoryFeedback> StoryFeedback { get; set; }
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

            modelBuilder.Entity<Airports>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("airports_pkey");

                entity.ToTable("airports", "datasets");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

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

            modelBuilder.Entity<CommuteTimes>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.City })
                    .HasName("commute_times_pkey");

                entity.ToTable("commute_times", "datasets");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("date");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.AvgMinutes)
                    .HasColumnName("avg_minutes")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<CovidUnemployment>(entity =>
            {
                entity.HasKey(e => e.WeekEnd)
                    .HasName("covid_unemployment_pkey");

                entity.ToTable("covid_unemployment", "datasets");

                entity.Property(e => e.WeekEnd)
                    .HasColumnName("week_end")
                    .HasColumnType("date");

                entity.Property(e => e.UnemploymentClaims).HasColumnName("unemployment_claims");
            });

            modelBuilder.Entity<DatasetMetaData>(entity =>
            {
                entity.HasKey(e => e.TableName)
                    .HasName("datasetmetadata_pkey");

                entity.ToTable("dataset_meta_data");

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(500);

                entity.Property(e => e.ColumnNames).HasColumnName("column_names");

                entity.Property(e => e.GeoType).HasColumnName("geo_type");

                entity.Property(e => e.DataTypes).HasColumnName("data_types");
            });

            modelBuilder.Entity<DefaultGraph>(entity =>
            {
                entity.HasKey(e => e.GraphId)
                    .HasName("default_graph_pkey");

                entity.ToTable("default_graph");

                entity.Property(e => e.GraphId)
                    .HasColumnName("graph_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Graph)
                    .WithOne(p => p.DefaultGraph)
                    .HasForeignKey<DefaultGraph>(d => d.GraphId)
                    .HasConstraintName("default_graph_graph_id_fkey");
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

            modelBuilder.Entity<ImageBlock>(entity =>
            {
                entity.HasKey(e => e.BlockId)
                    .HasName("image_block_pkey");

                entity.ToTable("image_block");

                entity.Property(e => e.BlockId)
                    .HasColumnName("block_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.BlockPosition).HasColumnName("block_position");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("image_url")
                    .HasMaxLength(1000);

                entity.Property(e => e.StoryId)
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.ImageBlock)
                    .HasForeignKey(d => d.StoryId)
                    .HasConstraintName("image_block_story_id_fkey");
            });

            modelBuilder.Entity<IncomeInequalitySlo>(entity =>
            {
                entity.HasKey(e => e.Year)
                    .HasName("income_inequality_slo_pkey");

                entity.ToTable("income_inequality_slo", "datasets");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("date");

                entity.Property(e => e.IncomeInequality)
                    .HasColumnName("income_inequality")
                    .HasColumnType("numeric");
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

            modelBuilder.Entity<MedianHouseIncomeSlo>(entity =>
            {
                entity.HasKey(e => e.Year)
                    .HasName("median_house_income_slo_pkey");

                entity.ToTable("median_house_income_slo", "datasets");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("date");

                entity.Property(e => e.MedianIncome).HasColumnName("median_income");
            });
            
            modelBuilder.Entity<NetMigrationSlo>(entity =>
            {
                entity.HasKey(e => e.Year)
                    .HasName("net_migration_slo_pkey");

                entity.ToTable("net_migration_slo", "datasets");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("date");

                entity.Property(e => e.NetMigration).HasColumnName("net_migration");
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

                entity.Property(e => e.IsThirdParty).HasColumnName("is_third_party");

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

            modelBuilder.Entity<SloAirport>(entity =>
            {
                entity.HasKey(e => e.Month)
                    .HasName("slo_airport_pkey");

                entity.ToTable("slo_airport", "datasets");

                entity.Property(e => e.Month)
                    .HasColumnName("month")
                    .HasColumnType("date");

                entity.Property(e => e.Alaska).HasColumnName("alaska");

                entity.Property(e => e.American).HasColumnName("american");

                entity.Property(e => e.Contour).HasColumnName("contour");

                entity.Property(e => e.GrandTotal2018).HasColumnName("grand_total_2018");

                entity.Property(e => e.GrandTotal2019).HasColumnName("grand_total_2019");

                entity.Property(e => e.PctChange)
                    .HasColumnName("pct_change")
                    .HasColumnType("numeric");

                entity.Property(e => e.United).HasColumnName("united");
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

            modelBuilder.Entity<StoryFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("story_feedback_pkey");

                entity.ToTable("story_feedback");

                entity.Property(e => e.FeedbackId)
                    .HasColumnName("feedback_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.Feedback)
                    .HasColumnName("feedback")
                    .HasMaxLength(10000);

                entity.Property(e => e.ReviewerId)
                    .HasColumnName("reviewer_id")
                    .HasMaxLength(100);

                entity.Property(e => e.StoryId)
                    .HasColumnName("story_id")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.StoryFeedback)
                    .HasForeignKey(d => d.StoryId)
                    .HasConstraintName("story_feedback_story_id_fkey");
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

            modelBuilder.Entity<UniversityInfo>(entity =>
            {
                entity.HasKey(e => new { e.IdGender, e.IdUniversity, e.Year })
                    .HasName("university_info_pkey");

                entity.ToTable("university_info", "datasets");

                entity.Property(e => e.IdGender).HasColumnName("id_gender");

                entity.Property(e => e.IdUniversity).HasColumnName("id_university");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e.Completions).HasColumnName("completions");

                entity.Property(e => e.County)
                    .IsRequired()
                    .HasColumnName("county");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender");

                entity.Property(e => e.IdGeography)
                    .IsRequired()
                    .HasColumnName("id_geography");

                entity.Property(e => e.University)
                    .IsRequired()
                    .HasColumnName("university");
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
