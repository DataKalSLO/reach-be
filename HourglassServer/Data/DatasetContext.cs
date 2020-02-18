using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HourglassServer.Data
{
    public partial class DatabaseContext : DbContext
    {
        private IConfiguration _config;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("HourglassDatabase"));
            }
        }
    }
}
