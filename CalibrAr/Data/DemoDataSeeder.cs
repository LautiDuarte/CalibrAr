using Domain.Model;

namespace Data
{
    // Datos de ejemplo para poder probar la aplicación y ver los reportes con contenido.
    // Las fechas son relativas a hoy. Se cargan una sola vez: si ya hay instrumentos no hace nada.
    // Se escribe directo en el contexto porque el servicio de calibraciones fuerza la fecha a "ahora".
    public static class DemoDataSeeder
    {
        public static void Seed(CalibrArContext context)
        {
            if (context.Instruments.Any())
                return;

            var admin = context.Users.FirstOrDefault(u => u.Email == "admin@calibrar.com");
            var responsible = context.Users.FirstOrDefault(u => u.Email == "responsible@calibrar.com");
            var operatorUser = context.Users.FirstOrDefault(u => u.Email == "operator@calibrar.com");
            var central = context.Locations.FirstOrDefault(l => l.Name == "Sede Central");
            var south = context.Locations.FirstOrDefault(l => l.Name == "Sede Sur");
            var north = context.Locations.FirstOrDefault(l => l.Name == "Sede Norte");
            var pressure = context.InstrumentTypes.FirstOrDefault(t => t.Name == "Calibrador de presión");
            var temperature = context.InstrumentTypes.FirstOrDefault(t => t.Name == "Calibrador de temperatura");
            var flow = context.InstrumentTypes.FirstOrDefault(t => t.Name == "Calibrador de flujo");

            // Si falta algo de la carga inicial no se inventa nada.
            if (admin == null || responsible == null || operatorUser == null || central == null || south == null
                || north == null || pressure == null || temperature == null || flow == null)
                return;

            var today = DateTime.Now.Date;
            var now = DateTime.Now;

            var lab = new Area(0, "Laboratorio de metrología", "Marta Giménez", true, now, central.Id);
            var production = new Area(0, "Producción", "Carlos Ruiz", true, now, south.Id);
            var quality = new Area(0, "Control de calidad", "Laura Peña", true, now, north.Id);
            var maintenance = new Area(0, "Mantenimiento", "Diego Soto", true, now, central.Id);
            context.Areas.AddRange(lab, production, quality, maintenance);

            // PC-001 tiene dos versiones: la vieja quedó inactiva y solo la usan calibraciones anteriores.
            var pressureProcedure = new Procedure(0, "PC-001", "Calibración de manómetros", "3", today.AddMonths(-10), true, now, pressure.Id, "https://example.com/procedimientos/PC-001-v3.pdf");
            var oldPressureProcedure = new Procedure(0, "PC-001", "Calibración de manómetros", "2", today.AddMonths(-26), false, now, pressure.Id);
            var temperatureProcedure = new Procedure(0, "PC-002", "Calibración de termómetros", "2", today.AddMonths(-14), true, now, temperature.Id, "https://example.com/procedimientos/PC-002-v2.pdf");
            var flowProcedure = new Procedure(0, "PC-003", "Calibración de medidores de flujo", "1", today.AddMonths(-8), true, now, flow.Id, "https://example.com/procedimientos/PC-003-v1.pdf");
            context.Procedures.AddRange(pressureProcedure, oldPressureProcedure, temperatureProcedure, flowProcedure);

            var pressureStandard = new ReferenceStandard(0, "Calibrador de presión patrón, 0 a 100 psi", "INTI", "C-2025-118",
                today.AddMonths(-20), today.AddMonths(4), true, now);
            var temperatureStandard = new ReferenceStandard(0, "Termómetro patrón de referencia, -20 a 150 °C", "INTI", "C-2025-203",
                today.AddMonths(-18), today.AddMonths(6), true, now);
            var flowStandard = new ReferenceStandard(0, "Medidor de flujo patrón, 0 a 50 L/min", "Laboratorio Metrológico Cuyo", "LMC-0457",
                today.AddMonths(-23), today.AddDays(20), true, now);
            var expiredStandard = new ReferenceStandard(0, "Juego de pesas patrón, 1 mg a 1 kg", "INTI", "C-2023-077",
                today.AddMonths(-26), today.AddMonths(-2), true, now);
            context.ReferenceStandards.AddRange(pressureStandard, temperatureStandard, flowStandard, expiredStandard);

            // Se guarda ahora para que áreas, procedimientos y patrones ya tengan Id cuando se los use abajo.
            context.SaveChanges();

            // Fecha de la última calibración de cada instrumento: de ahí sale la próxima (12 meses en presión
            // y flujo, 6 en temperatura). Hay vencidos, próximos a vencer y con plazo de sobra.
            var man1 = today.AddMonths(-12).AddDays(10);
            var man2 = today.AddMonths(-6).AddDays(-25); // este instrumento tiene frecuencia propia de 6 meses
            var man3 = today.AddMonths(-12).AddDays(45);
            var man4 = today.AddMonths(-5);
            var man5 = today.AddDays(-40);
            var ter1 = today.AddMonths(-6).AddDays(20);
            var ter2 = today.AddMonths(-6).AddDays(75);
            var ter3 = today.AddMonths(-6).AddDays(-8);
            var ter4 = today.AddMonths(-20);
            var flu1 = today.AddMonths(-12).AddDays(150);
            var flu2 = today.AddDays(-12);

            var specs = new (string Code, string Name, string Brand, string Model, string Serial, InstrumentType Type, Area Area, InstrumentStatus Status, DateTime? Last)[]
            {
                ("MAN-001", "Manómetro digital", "Wika", "CPG1500", "WK-10234", pressure, lab, InstrumentStatus.Active, man1),
                ("MAN-002", "Manómetro de columna", "Ashcroft", "1009", "AS-5521", pressure, production, InstrumentStatus.CalibrationExpired, man2),
                ("MAN-003", "Transmisor de presión", "Endress+Hauser", "Cerabar PMC21", "EH-33017", pressure, quality, InstrumentStatus.Active, man3),
                ("MAN-004", "Manómetro de banco", "Wika", "232.50", "WK-88120", pressure, maintenance, InstrumentStatus.UnderRepair, man4),
                ("MAN-005", "Manómetro portátil", "Fluke", "700G", "FL-70412", pressure, lab, InstrumentStatus.OnLoan, man5),
                ("TER-001", "Termómetro infrarrojo", "Fluke", "62 MAX", "FL-62210", temperature, lab, InstrumentStatus.Active, ter1),
                ("TER-002", "Termopar tipo K", "Omega", "KMQSS-125", "OM-40318", temperature, production, InstrumentStatus.Active, ter2),
                ("TER-003", "Termómetro de vidrio", "Brannan", "Serie 3", "BR-1207", temperature, quality, InstrumentStatus.CalibrationExpired, ter3),
                ("TER-004", "Termómetro digital antiguo", "Testo", "110", "TE-0098", temperature, maintenance, InstrumentStatus.Decommissioned, ter4),
                ("FLU-001", "Caudalímetro másico", "Siemens", "Sitrans FC330", "SI-77410", flow, production, InstrumentStatus.Active, flu1),
                ("FLU-002", "Rotámetro", "Dwyer", "RMA-26", "DW-9034", flow, lab, InstrumentStatus.Active, flu2),
                ("FLU-003", "Medidor de flujo portátil", "Testo", "6446", "TE-6446", flow, quality, InstrumentStatus.Active, null)
            };

            var instruments = new Dictionary<string, Instrument>();
            foreach (var s in specs)
            {
                int? ownFrequency = s.Code == "MAN-002" ? 6 : null;
                var next = Instrument.CalculateNextCalibrationDate(s.Last, ownFrequency ?? s.Type.CalibrationFrequencyMonths);
                var instrument = new Instrument(0, s.Code, s.Name, s.Serial, s.Brand, s.Model, s.Status, null, ownFrequency, s.Last, next,
                    s.Status != InstrumentStatus.Decommissioned, today.AddYears(-2), null, s.Type.Id, s.Area.Id);
                instruments[s.Code] = instrument;
                context.Instruments.Add(instrument);
            }

            context.SaveChanges();

            // Historial: los cambios de estado de los instrumentos que no están activos (null = lo puso el sistema).
            context.InstrumentStatusHistories.AddRange(
                new InstrumentStatusHistory(0, instruments["MAN-002"].Id, InstrumentStatus.Active, InstrumentStatus.CalibrationExpired, null, man2.AddMonths(6)),
                new InstrumentStatusHistory(0, instruments["TER-003"].Id, InstrumentStatus.Active, InstrumentStatus.CalibrationExpired, null, ter3.AddMonths(6)),
                new InstrumentStatusHistory(0, instruments["MAN-004"].Id, InstrumentStatus.Active, InstrumentStatus.UnderRepair, responsible.Id, today.AddDays(-20)),
                new InstrumentStatusHistory(0, instruments["MAN-005"].Id, InstrumentStatus.Active, InstrumentStatus.OnLoan, operatorUser.Id, today.AddDays(-30)),
                new InstrumentStatusHistory(0, instruments["TER-004"].Id, InstrumentStatus.Active, InstrumentStatus.Decommissioned, admin.Id, ter4.AddDays(20)));

            Calibration NewCalibration(string code, DateTime date, Result result, User performer, User? approver,
                bool external = false, string? externalLab = null, string? certificate = null, string? restriction = null)
            {
                var instrument = instruments[code];
                var type = specs.First(s => s.Code == code).Type;
                var at = date.Date.AddHours(10);

                // Las externas no llevan procedimiento propio ni mediciones ni patrones.
                Procedure? procedure = null;
                if (!external)
                    procedure = type == pressure ? (at < today.AddMonths(-12) ? oldPressureProcedure : pressureProcedure)
                        : type == temperature ? temperatureProcedure : flowProcedure;

                var calibration = new Calibration(0, at, InterventionType.Calibration, external, externalLab, certificate, result,
                    restriction, null, at, instrument.Id, procedure?.Id, performer.Id, approver?.Id);

                if (!external)
                {
                    calibration.SetMeasurements(BuildMeasurements(type, result));

                    // Se usa el patrón de ese tipo solo si su certificado estaba vigente ese día.
                    var standard = type == pressure ? pressureStandard : type == temperature ? temperatureStandard : flowStandard;
                    if (at >= standard.CertificateIssuedAt && at <= standard.CertificateExpiresAt)
                        calibration.SetReferenceStandards(new[] { standard });
                }

                context.Calibrations.Add(calibration);
                return calibration;
            }

            NewCalibration("MAN-001", man1.AddMonths(-12), Result.Conformant, operatorUser, responsible);
            NewCalibration("MAN-001", man1, Result.Conformant, operatorUser, admin);

            NewCalibration("MAN-002", man2.AddMonths(-6), Result.Conformant, operatorUser, responsible);
            var man2Calibration = NewCalibration("MAN-002", man2, Result.NonConformant, operatorUser, admin);

            NewCalibration("MAN-003", man3.AddMonths(-12), Result.WithRestrictions, responsible, admin, restriction: "Usar solo entre 0 y 50 psi");
            NewCalibration("MAN-003", man3, Result.Conformant, operatorUser, responsible);

            NewCalibration("MAN-004", man4, Result.WithRestrictions, operatorUser, responsible, restriction: "Usar solo hasta 50 psi");
            NewCalibration("MAN-005", man5, Result.Conformant, operatorUser, admin);

            NewCalibration("TER-001", ter1.AddMonths(-12), Result.NonConformant, operatorUser, responsible);
            NewCalibration("TER-001", ter1.AddMonths(-6), Result.Conformant, operatorUser, admin);
            NewCalibration("TER-001", ter1, Result.Conformant, responsible, admin);

            NewCalibration("TER-002", ter2.AddMonths(-6), Result.NonConformant, operatorUser, responsible);
            NewCalibration("TER-002", ter2, Result.Conformant, operatorUser, admin);

            NewCalibration("TER-003", ter3.AddMonths(-6), Result.Conformant, operatorUser, responsible);
            NewCalibration("TER-003", ter3, Result.WithRestrictions, operatorUser, admin, restriction: "Usar solo hasta 100 °C");

            var ter4Calibration = NewCalibration("TER-004", ter4, Result.NonConformant, operatorUser, admin);

            NewCalibration("FLU-001", flu1.AddMonths(-12), Result.Conformant, operatorUser, responsible);
            NewCalibration("FLU-001", flu1, Result.Conformant, responsible, admin, external: true,
                externalLab: "Laboratorio Metrológico Cuyo", certificate: "LMC-2025-0457");

            // La más reciente queda pendiente de aprobación.
            NewCalibration("FLU-002", flu2, Result.Conformant, operatorUser, null);

            context.SaveChanges();

            context.NonConformities.AddRange(
                new NonConformity(0, "NC-0001", "El manómetro quedó fuera de tolerancia en la última calibración. Se retira de uso hasta analizar la causa.",
                    Origin.NonConformantResult, NonConformityStatus.UnderAnalysis, null, man2.AddDays(1), null, man2.AddDays(1),
                    instruments["MAN-002"].Id, man2Calibration.Id, operatorUser.Id, null),
                new NonConformity(0, "NC-0002", "Se encontró la mirilla del manómetro rota durante el uso. Está en reparación.",
                    Origin.PhysicalDamage, NonConformityStatus.Open, null, today.AddDays(-20), null, today.AddDays(-20),
                    instruments["MAN-004"].Id, null, operatorUser.Id, null),
                new NonConformity(0, "NC-0003", "El termómetro dio fuera de tolerancia y no se pudo ajustar.",
                    Origin.NonConformantResult, NonConformityStatus.Closed, "Se dio de baja el instrumento y se reemplazó por el TER-002.",
                    ter4.AddDays(1), ter4.AddDays(15), ter4.AddDays(1),
                    instruments["TER-004"].Id, ter4Calibration.Id, operatorUser.Id, admin.Id),
                new NonConformity(0, "NC-0004", "La calibración del termómetro venció y todavía no se renovó.",
                    Origin.CalibrationExpired, NonConformityStatus.Open, null, today.AddDays(-7), null, today.AddDays(-7),
                    instruments["TER-003"].Id, null, operatorUser.Id, null));

            context.SaveChanges();
        }

        // Mediciones de ejemplo según el resultado: conforme queda bien dentro de la tolerancia, con restricciones
        // llega cerca del límite y no conforme se pasa en un punto.
        private static List<CalibrationMeasurement> BuildMeasurements(InstrumentType type, Result result)
        {
            decimal[] nominals = type.MeasurementUnit switch
            {
                "psi" => new[] { 10m, 25m, 50m, 75m },
                "°C" => new[] { 25m, 50m, 100m, 150m },
                _ => new[] { 1m, 5m, 10m, 20m }
            };

            decimal[] factors = result switch
            {
                Result.Conformant => new[] { 0.2m, -0.3m, 0.1m, -0.2m },
                Result.WithRestrictions => new[] { 0.3m, 0.9m, -0.4m, 0.2m },
                _ => new[] { 0.2m, -0.3m, 1.6m, 0.4m }
            };

            var measurements = new List<CalibrationMeasurement>();
            for (int i = 0; i < nominals.Length; i++)
            {
                var measured = decimal.Round(nominals[i] + factors[i] * type.MaxAllowedError, 4);
                var error = measured - nominals[i];
                measurements.Add(new CalibrationMeasurement(0, nominals[i], measured, error, Math.Abs(error) <= type.MaxAllowedError, null, 0));
            }
            return measurements;
        }
    }
}
