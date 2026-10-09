using System.Collections.Generic;
using System.IO;
using EzyPlanData;
using SphereIQ.Reports;
using Xunit;

namespace SphereIQ.Core.Tests
{
    public sealed class ReportPdfTests
    {
        [Fact]
        public void PdfRenderer_WritesValidPdf()
        {
            ReportDocument doc = new ReportDocument("Hole TEST-001", "SphereIQ");
            doc.AppendCoverPage("Cover", "Hole TEST-001", "Drilling report");
            List<AbsoluteSurvey> surveys = new List<AbsoluteSurvey>();
            surveys.Add(new AbsoluteSurvey(0m, 5m, 10m, 0m, 7000000m, 500000m, 0m));
            surveys.Add(new AbsoluteSurvey(100m, 6m, 12m, 99m, 7000050m, 500010m, 0.5m));
            doc.AppendSurveyPage("Surveys", surveys);
            DrillHoleStatus status = new DrillHoleStatus();
            status.MinimumDLStateString = "OK";
            status.MinimumDLSeverity = MinDLSeverity.Ok;
            doc.AppendHoleStatusPage("Hole Status", status);
            using (MemoryStream ms = new MemoryStream())
            {
                ReportPdfRenderer.Save(doc, ms);
                Assert.True(ms.Length > 500);
                ms.Position = 0;
                byte[] head = new byte[5];
                ms.Read(head, 0, 5);
                Assert.Equal("%PDF-", System.Text.Encoding.ASCII.GetString(head));
            }
        }

        [Fact]
        public void FontMap_ConvertsPointsToDips()
        {
            Assert.Equal("Helvetica Neue", FontMap.MapFamily("Microsoft Sans Serif"));
            Assert.Equal(21.0, FontMap.ToDips(15.75), 6);
            Assert.True(FontMap.Roles.ContainsKey("Button"));
        }
    }
}
