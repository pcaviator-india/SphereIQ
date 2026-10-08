using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EzyPlanData;

namespace EzyPlanStorage
{
    /// <summary>
    /// Portable .ezn hole-file format (Phase 1 storage migration).
    /// JSON with private-field inclusion; the file graph (SurveyList, Collar,
    /// Target, ReportList, DrillHoleStatus) is POCO + events (ignored by STJ)
    /// so it round-trips losslessly with field inclusion on.
    /// Magic header "EZN1" distinguishes new files from legacy
    /// BinaryFormatter .ezn files (handled by LegacyHoleFileReader).
    /// </summary>
    public static class HoleFileFormat
    {
        public const string Magic = "EZN1";

        public const string Extension = ".ezn";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = false,
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            Converters = { new JsonStringEnumConverter() }
        };

        public sealed class HoleFileDto
        {
            public string HoleID { get; set; }
            public string RigID { get; set; }
            public string Location { get; set; }
            public string Area { get; set; }
            public string DrillingContractor { get; set; }
            public string SurveyingContractor { get; set; }
            public string Client { get; set; }
            public DateTime CreatedDate { get; set; }
            public decimal SideTrackMD { get; set; }
            public List<SurveyRow> Surveys { get; set; }
            public CollarDto Collar { get; set; }
            public TargetDto Target { get; set; }
        }

        public sealed class SurveyRow
        {
            public decimal MD { get; set; }
            public decimal Dip { get; set; }
            public decimal Azimuth { get; set; }
        }

        public sealed class CollarDto
        {
            public decimal Easting { get; set; }
            public decimal Northing { get; set; }
        }

        public sealed class TargetDto
        {
            public decimal Easting { get; set; }
            public decimal Northing { get; set; }
            public decimal TVD { get; set; }
        }

        public static HoleFileDto FromFile(DrillHoleFile file)
        {
            HoleFileDto dto = new HoleFileDto
            {
                HoleID = file.HoleID,
                RigID = file.RigID,
                Location = file.Location,
                Area = file.Area,
                DrillingContractor = file.DrillingContractor,
                SurveyingContractor = file.SurveyingContractor,
                Client = file.Client,
                CreatedDate = file.CreatedDate,
                SideTrackMD = file.SideTrackMD,
                Surveys = new List<SurveyRow>(),
                Collar = file.Collar != null ? new CollarDto { Easting = file.Collar.Easting, Northing = file.Collar.Northing } : null,
                Target = file.Target != null ? new TargetDto { Easting = file.Target.Easting, Northing = file.Target.Northing, TVD = file.Target.TVD } : null
            };
            if (file.SurveyList != null)
            {
                foreach (Survey s in file.SurveyList)
                {
                    dto.Surveys.Add(new SurveyRow { MD = s.MD, Dip = s.Dip, Azimuth = s.Azimuth });
                }
            }
            return dto;
        }

        public static void ApplyTo(HoleFileDto dto, DrillHoleFile file)
        {
            file.HoleID = dto.HoleID;
            file.RigID = dto.RigID;
            file.Location = dto.Location;
            file.Area = dto.Area;
            file.DrillingContractor = dto.DrillingContractor;
            file.SurveyingContractor = dto.SurveyingContractor;
            file.Client = dto.Client;
            file.SetCreatedDate(dto.CreatedDate);
            file.SideTrackMD = dto.SideTrackMD;
            SurveyList list = new SurveyList();
            if (dto.Surveys != null)
            {
                foreach (SurveyRow row in dto.Surveys)
                {
                    Survey s = new Survey();
                    if (s.SetMD(row.MD) == Survey.SetMDEnum.ValueSet
                        && s.SetDip(row.Dip) == Survey.SetDipEnum.ValueSet
                        && s.SetAzimuth(row.Azimuth) == Survey.SetAzimuthEnum.ValueSet)
                    {
                        list.Append(s);
                    }
                }
            }
            file.SurveyList = list;
            file.Target = dto.Target != null ? new Target(dto.Target.Easting, dto.Target.Northing, dto.Target.TVD) : null;
            file.Collar = dto.Collar != null ? new Collar(dto.Collar.Easting, dto.Collar.Northing) : null;
            file.ReportList = new ReportList();
        }

        public static void Write(Stream stream, DrillHoleFile file)
        {
            byte[] magic = Encoding.ASCII.GetBytes(Magic + "\n");
            stream.Write(magic, 0, magic.Length);
            JsonSerializer.Serialize(stream, FromFile(file), Options);
        }

        public static HoleFileDto Read(Stream stream)
        {
            byte[] header = new byte[Magic.Length + 1];
            int read = 0;
            while (read < header.Length)
            {
                int n = stream.Read(header, read, header.Length - read);
                if (n == 0)
                {
                    throw new InvalidDataException("Not a SphereIQ hole file (truncated header).");
                }
                read += n;
            }
            string magic = Encoding.ASCII.GetString(header, 0, Magic.Length);
            if (!string.Equals(magic, Magic, StringComparison.Ordinal))
            {
                throw new LegacyHoleFileException();
            }
            HoleFileDto dto = JsonSerializer.Deserialize<HoleFileDto>(stream, Options);
            if (dto == null)
            {
                throw new InvalidDataException("Hole file payload is empty or corrupt.");
            }
            return dto;
        }
    }

    /// <summary>
    /// Thrown when a legacy BinaryFormatter .ezn file is detected.
    /// Legacy files must be converted once with the Windows net462 helper
    /// (tools/LegacyHoleFileConverter, Phase 1) which re-saves them in EZN1.
    /// </summary>
    public sealed class LegacyHoleFileException : IOException
    {
        public LegacyHoleFileException()
            : base("Legacy BinaryFormatter hole file: convert once with tools/LegacyHoleFileConverter on Windows.")
        {
        }
    }
}
