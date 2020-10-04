using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DominosGeolocation.Data.Models
{
    public partial class GeolocationContext : DbContext
    {
        public GeolocationContext()
        {
        }

        public GeolocationContext(DbContextOptions<GeolocationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DestinationSource> DestinationSource { get; set; }
        public virtual DbSet<WorkOrder> WorkOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=Geolocation;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DestinationSource>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DestinationLatitude)
                    .HasColumnName("Destination_latitude")
                    .HasMaxLength(50);

                entity.Property(e => e.DestinationLongitude)
                    .HasColumnName("Destination_longitude")
                    .HasMaxLength(50);

                entity.Property(e => e.SourceLatitude)
                    .HasColumnName("Source_latitude")
                    .HasMaxLength(50);

                entity.Property(e => e.SourceLongitude)
                    .HasColumnName("Source_longitude")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DbEndDate).HasColumnType("datetime");

                entity.Property(e => e.DbStartDate).HasColumnType("datetime");

                entity.Property(e => e.FilePath).HasMaxLength(250);

                entity.Property(e => e.MqEndDate).HasColumnType("datetime");

                entity.Property(e => e.MqStartDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
