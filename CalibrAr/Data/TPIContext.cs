using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class TPIContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Area> Areas { get; set; }

        public TPIContext(DbContextOptions<TPIContext> options) : base(options)
        {
            this.Database.EnsureCreated();
            //SeedInitialData();
        }

        internal TPIContext()
        {
            this.Database.EnsureCreated();
            //SeedInitialData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Responsible).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LocationId).IsRequired().HasField("_locationId");
                entity.Navigation(e => e.Location).HasField("_location");
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(e => e.Location)
                      .WithMany()
                      .HasForeignKey(e => e.LocationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InstrumentType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.MeasurementUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MaxAllowedError).IsRequired();
                entity.Property(e => e.CalibrationFrequencyMonths).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.MaxAllowedError);
                entity.Property(e => e.CalibrationFrequencyMonths);
                entity.Property(e => e.LastCalibrationDate);
                entity.Property(e => e.NextCalibrationDate);
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);
                entity.Property(e => e.InstrumentTypeId).IsRequired().HasField("_instrumentTypeId");
                entity.Navigation(e => e.InstrumentType).HasField("_instrumentType");
                entity.HasOne(e => e.InstrumentType)
                      .WithMany()
                      .HasForeignKey(e => e.InstrumentTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AreaId).IsRequired().HasField("_areaId");
                entity.Navigation(e => e.Area).HasField("_area");
                entity.HasOne(e => e.Area)
                      .WithMany()
                      .HasForeignKey(e => e.AreaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InstrumentStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.InstrumentId).IsRequired().HasField("_instrumentId");
                entity.Navigation(e => e.Instrument).HasField("_instrument");
                entity.Property(e => e.ChangedByUserId).IsRequired().HasField("_changedByUserId");
                entity.Navigation(e => e.ChangedByUser).HasField("_changedByUser");
                entity.Property(e => e.Reason).HasMaxLength(200);
                entity.Property(e => e.ChangedAt).IsRequired();
                entity.Property(e => e.PreviousStatus).IsRequired();
                entity.Property(e => e.NewStatus).IsRequired();
                entity.HasOne(e => e.ChangedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.ChangedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Instrument)
                      .WithMany()
                      .HasForeignKey(e => e.InstrumentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.LastLoginAt);
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<ReferenceStandard>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CertifyingBody).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CertificateNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CertificateIssuedAt).IsRequired();
                entity.Property(e => e.CertificateExpiresAt).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Procedure>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.VersionNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ApprovedAt).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.InstrumentTypeId).IsRequired().HasField("_instrumentTypeId");
                entity.Navigation(e => e.InstrumentType).HasField("_instrumentType");
                entity.HasOne(e => e.InstrumentType)
                      .WithMany()
                      .HasForeignKey(e => e.InstrumentTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Calibration>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CertificateNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Result).IsRequired();
                entity.Property(e => e.RestrictionDetail).HasMaxLength(200);
                entity.Property(e => e.NextCalibrationDate).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.InstrumentId).IsRequired().HasField("_instrumentId");
                entity.Navigation(e => e.Instrument).HasField("_instrument");
                entity.Property(e => e.ProcedureId).HasField("_procedureId");
                entity.Navigation(e => e.Procedure).HasField("_procedure");
                entity.Property(e => e.PerformedByUserId).HasField("_performedByUserId");
                entity.Navigation(e => e.PerformedByUser).HasField("_performedByUser");
                entity.Property(e => e.ApprovedByUserId).HasField("_approvedByUserId");
                entity.Navigation(e => e.ApprovedByUser).HasField("_approvedByUser");
                entity.HasOne(e => e.Instrument)
                      .WithMany()
                      .HasForeignKey(e => e.InstrumentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Procedure)
                      .WithMany()
                      .HasForeignKey(e => e.ProcedureId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.PerformedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.PerformedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ApprovedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.ApprovedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CalibrationMeasurement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CalibrationId).IsRequired().HasField("_calibrationId");
                entity.Navigation(e => e.Calibration).HasField("_calibration");
                entity.Property(e => e.NominalValue).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.MeasuredValue).IsRequired();
                entity.Property(e => e.Error).IsRequired();
                entity.Property(e => e.IsWithinTolerance).IsRequired();
                entity.HasOne(e => e.Calibration)
                      .WithMany()
                      .HasForeignKey(e => e.CalibrationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<NonConformity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.InstrumentId).IsRequired().HasField("_instrumentId");
                entity.Navigation(e => e.Instrument).HasField("_instrument");
                entity.Property(e => e.CalibrationId).HasField("_calibrationId");
                entity.Navigation(e => e.Calibration).HasField("_calibration");
                entity.Property(e => e.DetectedByUserId).IsRequired().HasField("_detectedByUserId");
                entity.Navigation(e => e.DetectedByUser).HasField("_detectedByUser");
                entity.Property(e => e.ClosedByUserId).HasField("_closedByUserId");
                entity.Navigation(e => e.ClosedByUser).HasField("_closedByUser");
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CorrectiveAction).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(e => e.Instrument)
                      .WithMany()
                      .HasForeignKey(e => e.InstrumentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Calibration)
                      .WithMany()
                      .HasForeignKey(e => e.CalibrationId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.DetectedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.DetectedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ClosedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.ClosedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Calibration>()
                .HasMany(c => c.ReferenceStandards)
                .WithMany(rs => rs.Calibrations)
                .UsingEntity<Dictionary<string, object>>(
                    "CalibrationReferenceStandard",
                    j => j.HasOne<ReferenceStandard>().WithMany().HasForeignKey("ReferenceStandardsId"),
                    j => j.HasOne<Calibration>().WithMany().HasForeignKey("CalibrationsId"),
                    j =>
                    {
                        j.HasKey("ReferenceStandardsId", "CalibrationsId");
                    });


        }
    }
}
