using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        public DbSet<InstrumentType> InstrumentTypes { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentStatusHistory> InstrumentStatusHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ReferenceStandard> ReferenceStandards { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Calibration> Calibrations { get; set; }
        public DbSet<CalibrationMeasurement> CalibrationMeasurements { get; set; }
        public DbSet<NonConformity> NonConformities { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }

        public TPIContext(DbContextOptions<TPIContext> options) : base(options)
        {
            this.Database.EnsureCreated();
            SeedInitialData();
        }

        internal TPIContext()
        {
            this.Database.EnsureCreated();
            SeedInitialData();
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
                entity.HasData
                (
                    new { Id = 1, Name = "Sede Central", Address = "Cordoba 947", IsActive = true, CreatedAt = DateTime.Now },
                    new { Id = 2, Name = "Sede Sur", Address = "Av. San Martín 4042", IsActive = true, CreatedAt = DateTime.Now },
                    new { Id = 3, Name = "Sede Norte", Address = "Av. Rondeau 752", IsActive = true, CreatedAt = DateTime.Now }
                );
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
                entity.HasData
                (
                    new { Id = 1, Name = "Calibrador de presión", Description = "Instrumento para calibrar presión", MeasurementUnit = "psi", MaxAllowedError = 0.5, CalibrationFrequencyMonths = 12, IsActive = true, CreatedAt = DateTime.Now },
                    new { Id = 2, Name = "Calibrador de temperatura", Description = "Instrumento para calibrar temperatura", MeasurementUnit = "°C", MaxAllowedError = 1.0, CalibrationFrequencyMonths = 6, IsActive = true, CreatedAt = DateTime.Now },
                    new { Id = 3, Name = "Calibrador de flujo", Description = "Instrumento para calibrar flujo", MeasurementUnit = "L/min", MaxAllowedError = 0.2, CalibrationFrequencyMonths = 12, IsActive = true, CreatedAt = DateTime.Now }
                );
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
                entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.LastLoginAt);
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                var adminUser = new User(1, "Admin", "adminuser@calibrar.com", "admin123", UserRole.Administrador, true, DateTime.Now);
                var responsableUser = new User(2, "Responsable", "responsableuser@calibrar.com", "responsable123", UserRole.Responsable, true, DateTime.Now);
                var operadorUser = new User(3, "Operador", "operadoruser@calibrar.com", "operador123", UserRole.Operador, true, DateTime.Now);
                var auditorUser = new User(4, "Auditor", "auditoruser@calibrar.com", "auditor123", UserRole.Auditor, true, DateTime.Now);

                entity.HasData
                (
                    new
                    {
                        Id = adminUser.Id,
                        FullName = adminUser.FullName,
                        Email = adminUser.Email,
                        PasswordHash = adminUser.PasswordHash,
                        Salt = adminUser.Salt,
                        Role = adminUser.Role,
                        IsActive = adminUser.IsActive,
                        CreatedAt = adminUser.CreatedAt
                    },
                    new
                    {
                        Id = responsableUser.Id,
                        FullName = responsableUser.FullName,
                        Email = responsableUser.Email,
                        PasswordHash = responsableUser.PasswordHash,
                        Salt = responsableUser.Salt,
                        Role = responsableUser.Role,
                        IsActive = responsableUser.IsActive,
                        CreatedAt = responsableUser.CreatedAt
                    },
                    new
                    {
                        Id = operadorUser.Id,
                        FullName = operadorUser.FullName,
                        Email = operadorUser.Email,
                        PasswordHash = operadorUser.PasswordHash,
                        Salt = operadorUser.Salt,
                        Role = operadorUser.Role,
                        IsActive = operadorUser.IsActive,
                        CreatedAt = operadorUser.CreatedAt
                    },
                    new
                    {
                        Id = auditorUser.Id,
                        FullName = auditorUser.FullName,
                        Email = auditorUser.Email,
                        PasswordHash = auditorUser.PasswordHash,
                        Salt = auditorUser.Salt,
                        Role = auditorUser.Role,
                        IsActive = auditorUser.IsActive,
                        CreatedAt = auditorUser.CreatedAt
                    }
                );
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

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(30);
                entity.Property(e => e.IsActive).IsRequired();
                entity.HasIndex(e => new { e.Name, e.Category }).IsUnique();
            });

            modelBuilder.Entity<PermissionGroup>(entity =>
            {
                entity.ToTable("PermissionGroups");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IsActive).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
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

            modelBuilder.Entity<PermissionGroup>()
                .HasMany(g => g.Permissions)
                .WithMany(p => p.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "PermissionGroupPermission",
                    j => j.HasOne<Permission>().WithMany().HasForeignKey("PermissionsId"),
                    j => j.HasOne<PermissionGroup>().WithMany().HasForeignKey("GroupsId"),
                    j =>
                    {
                        j.HasKey("GroupsId", "PermissionsId");

                    });

            modelBuilder.Entity<Permission>().HasData(
                // Permisos para Locations
                new { Id = 1, Name = "read", Description = "Leer locations", Category = "Locations", IsActive = true },
                new { Id = 2, Name = "create", Description = "Agregar locations", Category = "Locations", IsActive = true },
                new { Id = 3, Name = "update", Description = "Actualizar locations", Category = "Locations", IsActive = true },
                new { Id = 4, Name = "delete", Description = "Eliminar locations", Category = "Locations", IsActive = true },

                // Permisos para Areas
                new { Id = 5, Name = "read", Description = "Leer areas", Category = "Areas", IsActive = true },
                new { Id = 6, Name = "create", Description = "Agregar areas", Category = "Areas", IsActive = true },
                new { Id = 7, Name = "update", Description = "Actualizar areas", Category = "Areas", IsActive = true },
                new { Id = 8, Name = "delete", Description = "Eliminar areas", Category = "Areas", IsActive = true },

                // Permisos para Instruments
                new { Id = 9, Name = "read", Description = "Leer instrumentos", Category = "Instruments", IsActive = true },
                new { Id = 10, Name = "create", Description = "Agregar instrumentos", Category = "Instruments", IsActive = true },
                new { Id = 11, Name = "update", Description = "Actualizar instrumentos", Category = "Instruments", IsActive = true },
                new { Id = 12, Name = "delete", Description = "Eliminar instrumentos", Category = "Instruments", IsActive = true },

                // Permisos para Calibrations
                new { Id = 13, Name = "read", Description = "Leer calibraciones", Category = "Calibrations", IsActive = true },
                new { Id = 14, Name = "create", Description = "Agregar calibraciones", Category = "Calibrations", IsActive = true },
                new { Id = 15, Name = "update", Description = "Actualizar calibraciones", Category = "Calibrations", IsActive = true },
                new { Id = 16, Name = "delete", Description = "Eliminar calibraciones", Category = "Calibrations", IsActive = true },
                new { Id = 17, Name = "approve", Description = "Aprobar calibraciones", Category = "Calibrations", IsActive = true },

                // Permisos para Users
                new { Id = 18, Name = "read", Description = "Leer usuarios", Category = "Users", IsActive = true },
                new { Id = 19, Name = "create", Description = "Agregar usuarios", Category = "Users", IsActive = true },
                new { Id = 20, Name = "update", Description = "Actualizar usuarios", Category = "Users", IsActive = true },
                new { Id = 21, Name = "delete", Description = "Eliminar usuarios", Category = "Users", IsActive = true },

                // Permisos para InstrumentTypes
                new { Id = 22, Name = "read", Description = "Leer tipos de instrumentos", Category = "InstrumentTypes", IsActive = true },
                new { Id = 23, Name = "create", Description = "Agregar tipos de instrumentos", Category = "InstrumentTypes", IsActive = true },
                new { Id = 24, Name = "update", Description = "Actualizar tipos de instrumentos", Category = "InstrumentTypes", IsActive = true },
                new { Id = 25, Name = "delete", Description = "Eliminar tipos de instrumentos", Category = "InstrumentTypes", IsActive = true },

                // Permisos para Procedures
                new { Id = 26, Name = "read", Description = "Leer procedimientos", Category = "Procedures", IsActive = true },
                new { Id = 27, Name = "create", Description = "Agregar procedimientos", Category = "Procedures", IsActive = true },
                new { Id = 28, Name = "update", Description = "Actualizar procedimientos", Category = "Procedures", IsActive = true },
                new { Id = 29, Name = "delete", Description = "Eliminar procedimientos", Category = "Procedures", IsActive = true },

                //Permisos para Nonconformities
                new { Id = 30, Name = "read", Description = "Leer no conformidades", Category = "NonConformities", IsActive = true },
                new { Id = 31, Name = "create", Description = "Agregar no conformidades", Category = "NonConformities", IsActive = true },
                new { Id = 32, Name = "update", Description = "Actualizar no conformidades", Category = "NonConformities", IsActive = true },
                new { Id = 33, Name = "delete", Description = "Eliminar no conformidades", Category = "NonConformities", IsActive = true },

                //Permisos para Reference standards
                new { Id = 34, Name = "read", Description = "Leer patrones de referencia", Category = "ReferenceStandards", IsActive = true },
                new { Id = 35, Name = "create", Description = "Agregar patrones de referencia", Category = "ReferenceStandards", IsActive = true },
                new { Id = 36, Name = "update", Description = "Actualizar patrones de referencia", Category = "ReferenceStandards", IsActive = true },
                new { Id = 37, Name = "delete", Description = "Eliminar patrones de referencia", Category = "ReferenceStandards", IsActive = true },

                //Permisos para Instrument status history
                new { Id = 38, Name = "read", Description = "Leer historial de estado de instrumentos", Category = "InstrumentStatusHistory", IsActive = true },
                new { Id = 39, Name = "create", Description = "Agregar historial de estado de instrumentos", Category = "InstrumentStatusHistory", IsActive = true },
                new { Id = 40, Name = "update", Description = "Actualizar historial de estado de instrumentos", Category = "InstrumentStatusHistory", IsActive = true },
                new { Id = 41, Name = "delete", Description = "Eliminar historial de estado de instrumentos", Category = "InstrumentStatusHistory", IsActive = true },

                //Permisos para Calibration Measurements
                new { Id = 42, Name = "read", Description = "Leer mediciones de calibración", Category = "CalibrationMeasurements", IsActive = true },
                new { Id = 43, Name = "create", Description = "Agregar mediciones de calibración", Category = "CalibrationMeasurements", IsActive = true },
                new { Id = 44, Name = "update", Description = "Actualizar mediciones de calibración", Category = "CalibrationMeasurements", IsActive = true },
                new { Id = 45, Name = "delete", Description = "Eliminar mediciones de calibración", Category = "CalibrationMeasurements", IsActive = true }
                );

            modelBuilder.Entity<PermissionGroup>().HasData(
                new { Id = 1, Name = "Administrador", Description = "Grupo de administradores", IsActive = true, CreatedAt = DateTime.Now },
                new { Id = 2, Name = "Responsable", Description = "Grupo de responsables", IsActive = true, CreatedAt = DateTime.Now },
                new { Id = 3, Name = "Operador", Description = "Grupo de operadores", IsActive = true, CreatedAt = DateTime.Now },
                new { Id = 4, Name = "Auditor", Description = "Grupo de auditores", IsActive = true, CreatedAt = DateTime.Now }
                );
        }

        private void SeedInitialData()
        {
            try
            {
                if (!Users.Any(u => u.PermissionGroupId != null) &&
                    Users.Any() &&
                    PermissionGroups.Any() &&
                    Permissions.Any())
                {
                    var adminUser = Users.Include(u => u.Group).FirstOrDefault(u => u.Email == "admin@calibrar.com");
                    var responsableUser = Users.Include(u => u.Group).FirstOrDefault(u => u.Email == "responsable@calibrar.com");
                    var operadorUser = Users.Include(u => u.Group).FirstOrDefault(u => u.Email == "operador@calibrar.com");
                    var auditorUser = Users.Include(u => u.Group).FirstOrDefault(u => u.Email == "auditor@calibrar.com");
                    var adminGroup = PermissionGroups.Include(g => g.Permissions).FirstOrDefault(g => g.Name == "Administrador");
                    var responsableGroup = PermissionGroups.Include(g => g.Permissions).FirstOrDefault(g => g.Name == "Responsable");
                    var operadorGroup = PermissionGroups.Include(g => g.Permissions).FirstOrDefault(g => g.Name == "Operador");
                    var auditorGroup = PermissionGroups.Include(g => g.Permissions).FirstOrDefault(g => g.Name == "Auditor");
                    var allPermissions = Permissions.ToList();

                    if (adminGroup != null && 
                        responsableGroup != null &&
                        operadorGroup != null &&
                        auditorGroup != null &&
                        allPermissions.Any())
                    {
                        foreach (var permission in allPermissions)
                        {
                            if (!adminGroup.Permissions.Contains(permission))
                            {
                                adminGroup.Permissions.Add(permission);
                            }
                        }

                        var responsablePermissions = allPermissions.Where(
                            
                            p => (p.Category == "Locations") || 
                                 (p.Category == "Areas") || 
                                 (p.Category == "InstrumentTypes") || 
                                 (p.Category == "Instruments") || 
                                 (p.Category == "Calibrations") ||
                                 (p.Category == "CalibrationMeasurements") ||
                                 (p.Category == "NonConformities") ||
                                 (p.Category == "ReferenceStandards")
                        ).ToList();

                        foreach (var permission in responsablePermissions)
                        {
                            if (!responsableGroup.Permissions.Contains(permission))
                            {
                                responsableGroup.Permissions.Add(permission);
                            }
                        }

                        var operadorPermissions = allPermissions.Where(
                            p => (p.Category == "Instruments") || 
                                 (p.Category == "Calibrations" && (p.Name == "read" || p.Name == "create" || p.Name == "update" || p.Name == "delete")) ||
                                 (p.Category == "CalibrationMeasurements") ||
                                 (p.Category == "NonConformities")
                        ).ToList();

                        foreach (var permission in operadorPermissions)
                        {
                            if (!operadorGroup.Permissions.Contains(permission))
                            {
                                operadorGroup.Permissions.Add(permission);
                            }
                        }

                        var auditorPermissions = allPermissions.Where(
                            p => (p.Category == "Instruments" && p.Name == "read")
                        ).ToList();

                        foreach(var permission in auditorPermissions)
                        {
                            if (!auditorGroup.Permissions.Contains(permission))
                            {
                                auditorGroup.Permissions.Add(permission);
                            }
                        }

                        if (adminUser != null)
                        {
                            adminUser.SetGroup(adminGroup);
                        }

                        if (responsableUser != null)
                        {
                            responsableUser.SetGroup(responsableGroup);
                        }

                        if (operadorUser != null)
                        {
                            operadorUser.SetGroup(operadorGroup);
                        }

                        if (auditorUser != null)
                        {
                            auditorUser.SetGroup(auditorGroup);
                        }

                        SaveChanges();
                    }
                }
            }
            catch {}
        }
    }
}
