using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<GeoType>();
        }


        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<CensusData> CensusData { get; set; }
        public virtual DbSet<CensusVariables> CensusVariables { get; set; }
        public virtual DbSet<Datasetmetadata> Datasetmetadata { get; set; }
        public virtual DbSet<FederalContractsFy2019> FederalContractsFy2019 { get; set; }
        public virtual DbSet<GeoMapTables> GeoMapTables { get; set; }
        public virtual DbSet<Geomap> Geomap { get; set; }
        public virtual DbSet<Geomapblock> Geomapblock { get; set; }
        public virtual DbSet<Graph> Graph { get; set; }
        public virtual DbSet<Graphblock> Graphblock { get; set; }
        public virtual DbSet<Graphseries> Graphseries { get; set; }
        public virtual DbSet<Hourglassuser> Hourglassuser { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Point> Point { get; set; }
        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<Textblock> Textblock { get; set; }
        public virtual DbSet<Tmp> Tmp { get; set; }
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
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.HasPostgresEnum("geo_type", new[] { "city", "zip", "county" });

            modelBuilder.Entity<Story>(StoryEntityCreator.create);

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

            modelBuilder.Entity<CensusData>(entity =>
            {
                entity.HasKey(e => new { e.VariableName, e.Year, e.GeoName })
                    .HasName("census_data_pkey");

                entity.ToTable("census_data");

                entity.Property(e => e.VariableName)
                    .HasColumnName("variable_name")
                    .HasMaxLength(40);

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e.GeoName)
                    .HasColumnName("geo_name")
                    .HasMaxLength(100);

                entity.Property(e => e.GeoType)
                    .HasColumnName("geo_type");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.VariableNameNavigation)
                    .WithMany(p => p.CensusData)
                    .HasForeignKey(d => d.VariableName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("census_data_variable_name_fkey");
            });

            modelBuilder.Entity<CensusVariables>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("census_variables_pkey");

                entity.ToTable("census_variables");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(40);

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Datasetmetadata>(entity =>
            {
                entity.HasKey(e => e.Tablename)
                    .HasName("datasetmetadata_pkey");

                entity.ToTable("datasetmetadata");

                entity.Property(e => e.Tablename)
                    .HasColumnName("tablename")
                    .HasMaxLength(500);

                entity.Property(e => e.Citycolumn).HasColumnName("citycolumn");

                entity.Property(e => e.Columnnames)
                    .HasColumnName("columnnames")
                    .HasColumnType("character varying(500)[]");

                entity.Property(e => e.Countycolumn).HasColumnName("countycolumn");

                entity.Property(e => e.Datatypes)
                    .HasColumnName("datatypes")
                    .HasColumnType("character varying(500)[]");

                entity.Property(e => e.Locationvalues).HasColumnName("locationvalues");

                entity.Property(e => e.Zipcodecolumn).HasColumnName("zipcodecolumn");
            });

            modelBuilder.Entity<FederalContractsFy2019>(entity =>
            {
                entity.ToTable("federal_contracts_fy2019");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActionDate).HasColumnName("action_date");

                entity.Property(e => e.AwardDescription).HasColumnName("award_description");

                entity.Property(e => e.AwardType).HasColumnName("award_type");

                entity.Property(e => e.AwardingAgencyName).HasColumnName("awarding_agency_name");

                entity.Property(e => e.CurrentTotalValueOfAward)
                    .HasColumnName("current_total_value_of_award")
                    .HasColumnType("money");

                entity.Property(e => e.FundingAgencyName).HasColumnName("funding_agency_name");

                entity.Property(e => e.PeriodOfPerformanceCurrentEndDate).HasColumnName("period_of_performance_current_end_date");

                entity.Property(e => e.PeriodOfPerformancePotentialEndDate).HasColumnName("period_of_performance_potential_end_date");

                entity.Property(e => e.PeriodOfPerformanceStartDate).HasColumnName("period_of_performance_start_date");

                entity.Property(e => e.PotentialTotalValueOfAward)
                    .HasColumnName("potential_total_value_of_award")
                    .HasColumnType("money");

                entity.Property(e => e.PrimaryPlaceOfPerformanceCityName).HasColumnName("primary_place_of_performance_city_name");

                entity.Property(e => e.PrimaryPlaceOfPerformanceCountryCode).HasColumnName("primary_place_of_performance_country_code");

                entity.Property(e => e.PrimaryPlaceOfPerformanceCountryName).HasColumnName("primary_place_of_performance_country_name");

                entity.Property(e => e.PrimaryPlaceOfPerformanceCountyName).HasColumnName("primary_place_of_performance_county_name");

                entity.Property(e => e.PrimaryPlaceOfPerformanceStateCode).HasColumnName("primary_place_of_performance_state_code");

                entity.Property(e => e.PrimaryPlaceOfPerformanceStateName).HasColumnName("primary_place_of_performance_state_name");

                entity.Property(e => e.PrimaryPlaceOfPerformanceZip4).HasColumnName("primary_place_of_performance_zip_4");

                entity.Property(e => e.RecipientAddressLine1).HasColumnName("recipient_address_line_1");

                entity.Property(e => e.RecipientCityName).HasColumnName("recipient_city_name");

                entity.Property(e => e.RecipientCountryName).HasColumnName("recipient_country_name");

                entity.Property(e => e.RecipientName).HasColumnName("recipient_name");

                entity.Property(e => e.RecipientParentName).HasColumnName("recipient_parent_name");

                entity.Property(e => e.RecipientStateName).HasColumnName("recipient_state_name");

                entity.Property(e => e.RecipientZip4Code).HasColumnName("recipient_zip_4_code");

                entity.Property(e => e.TotalDollarsObligated)
                    .HasColumnName("total_dollars_obligated")
                    .HasColumnType("money");
            });

            modelBuilder.Entity<GeoMapTables>(entity =>
            {
                entity.HasKey(e => new { e.GeoMapId, e.TableName })
                    .HasName("geo_map_tables_pkey");

                entity.ToTable("geo_map_tables");

                entity.Property(e => e.GeoMapId)
                    .HasColumnName("geo_map_id")
                    .HasMaxLength(36);

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(100);

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

            modelBuilder.Entity<Geomap>(entity =>
            {
                entity.ToTable("geomap");

                entity.Property(e => e.Geomapid)
                    .HasColumnName("geomapid")
                    .HasMaxLength(36)
                    .IsFixedLength();
            });

            //modelBuilder.Entity<Geomapblock>(entity =>
            //{
            //    entity.HasKey(e => e.Blockid)
            //        .HasName("geomapblock_pkey");

            //    entity.ToTable("geomapblock");

            //    entity.Property(e => e.Blockid)
            //        .HasColumnName("blockid")
            //        .HasMaxLength(36)
            //        .IsFixedLength();

            //    entity.Property(e => e.Geomapid)
            //        .IsRequired()
            //        .HasColumnName("geomapid")
            //        .HasMaxLength(36)
            //        .IsFixedLength();

            //    entity.HasOne(d => d.Block)
            //        .WithOne(p => p.Geomapblock)
            //        .HasForeignKey<Geomapblock>(d => d.Blockid)
            //        .HasConstraintName("geomapblock_blockid_fkey");

            //    entity.HasOne(d => d.Geomap)
            //        .WithMany(p => p.Geomapblock)
            //        .HasForeignKey(d => d.Geomapid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("geomapblock_geomapid_fkey");
            //});

            modelBuilder.Entity<Graph>(entity =>
            {
                entity.ToTable("graph");

                entity.Property(e => e.Graphid)
                    .HasColumnName("graphid")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(500);
            });

            //modelBuilder.Entity<Graphblock>(entity =>
            //{
            //    entity.HasKey(e => e.Blockid)
            //        .HasName("graphblock_pkey");

            //    entity.ToTable("graphblock");

            //    entity.Property(e => e.Blockid)
            //        .HasColumnName("blockid")
            //        .HasMaxLength(36)
            //        .IsFixedLength();

            //    entity.Property(e => e.Graphid)
            //        .IsRequired()
            //        .HasColumnName("graphid")
            //        .HasMaxLength(36)
            //        .IsFixedLength();

            //    entity.HasOne(d => d.Block)
            //        .WithOne(p => p.Graphblock)
            //        .HasForeignKey<Graphblock>(d => d.Blockid)
            //        .HasConstraintName("graphblock_blockid_fkey");

            //    entity.HasOne(d => d.Graph)
            //        .WithMany(p => p.Graphblock)
            //        .HasForeignKey(d => d.Graphid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("graphblock_graphid_fkey");
            //});

            modelBuilder.Entity<Graphseries>(entity =>
            {
                entity.HasKey(e => new { e.Graphid, e.Tablename, e.Columnname, e.Seriestype })
                    .HasName("graphseries_pkey");

                entity.ToTable("graphseries");

                entity.Property(e => e.Graphid)
                    .HasColumnName("graphid")
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.Tablename)
                    .HasColumnName("tablename")
                    .HasMaxLength(500);

                entity.Property(e => e.Columnname)
                    .HasColumnName("columnname")
                    .HasMaxLength(500);

                entity.Property(e => e.Seriestype)
                    .HasColumnName("seriestype")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Graph)
                    .WithMany(p => p.Graphseries)
                    .HasForeignKey(d => d.Graphid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graphseries_graphid_fkey");

                entity.HasOne(d => d.TablenameNavigation)
                    .WithMany(p => p.Graphseries)
                    .HasForeignKey(d => d.Tablename)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graphseries_tablename_fkey");
            });

            modelBuilder.Entity<Hourglassuser>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("hourglassuser_pkey");

                entity.ToTable("hourglassuser");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasMaxLength(36)
                    .IsFixedLength();
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
                    .HasPrincipalKey(p => p.Id)
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
                    .HasName("Person_pkey");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Role).HasColumnName("role");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("point_pkey");

                entity.ToTable("point");

                entity.HasIndex(e => e.Id)
                    .HasName("point_id_key")
                    .IsUnique();

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
            });

            //modelBuilder.Entity<Textblock>(entity =>
            //{
            //    entity.HasKey(e => e.Blockid)
            //        .HasName("textblock_pkey");

            //    entity.ToTable("textblock");

            //    entity.Property(e => e.Blockid)
            //        .HasColumnName("blockid")
            //        .HasMaxLength(36)
            //        .IsFixedLength();

            //    entity.Property(e => e.Editorstate)
            //        .IsRequired()
            //        .HasColumnName("editorstate")
            //        .HasMaxLength(100000);

            //    entity.HasOne(d => d.Block)
            //        .WithOne(p => p.Textblock)
            //        .HasForeignKey<Textblock>(d => d.Blockid)
            //        .HasConstraintName("textblock_blockid_fkey");
            //});

            modelBuilder.Entity<Tmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp");
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
        }

    }
}
