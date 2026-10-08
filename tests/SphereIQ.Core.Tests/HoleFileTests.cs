using System;
using System.Collections.Generic;
using System.IO;
using EzyPlanData;
using EzyPlanMath;
using EzyPlanStorage;
using SphereIQ.Reports;
using Xunit;

namespace SphereIQ.Core.Tests
{
    public sealed class HoleFileTests
    {
        private static SurveyList TwoSurveyList()
        {
            SurveyList list = new SurveyList();
            Survey s0 = new Survey();
            s0.SetMD(0m); s0.SetDip(5m); s0.SetAzimuth(10m);
            list.Append(s0);
            Survey s1 = new Survey();
            s1.SetMD(100m); s1.SetDip(6m); s1.SetAzimuth(12m);
            list.Append(s1);
            return list;
        }

        [Fact]
        public void BuildToTarget_ReturnsPlan()
        {
            Collar collar = new Collar(500000m, 7000000m);
            Target target = new Target(500100m, 7000100m, 1000m);
            SurveyList surveys = TwoSurveyList();
            EzyPlanMath.Math.ThreeDWellPlan plan = new EzyPlanMath.Math.ThreeDWellPlan();
            AbsoluteSurveyList abs = new AbsoluteSurveyList();
            Assert.Equal(AbsoluteSurvey_Errors.NoError, plan.AbsoluteSurvey(collar, surveys, ref abs));
            AbsoluteSurvey last = abs.RecordAt(abs.Count() - 1);
            BuildAndHoldResults results = new BuildAndHoldResults();
            BuildToTarget_Errors rc = plan.BuildToTarget(3.0, last, target, ref results, 10.0);
            Assert.Equal(BuildToTarget_Errors.NoError, rc);
        }

        [Fact]
        public void HoleFile_RoundTripsThroughJsonFormat()
        {
            string path = Path.Combine(Path.GetTempPath(), "sphereiq-test-" + Guid.NewGuid().ToString("N") + ".ezn");
            try
            {
                DrillHoleFile file = new DrillHoleFile();
                file.HoleID = "TEST-001";
                file.RigID = "RIG-7";
                file.Location = "Perth";
                file.Area = "Block A";
                file.DrillingContractor = "DrillCo";
                file.SurveyingContractor = "SurveyCo";
                file.Client = "ClientCo";
                file.SideTrackMD = -1m;
                file.Collar = new Collar(500000m, 7000000m);
                file.Target = new Target(500100m, 7000100m, 1000m);
                file.SurveyList = TwoSurveyList();
                Assert.Equal(DrillHoleFile.SaveAs_ErrorsEnum.NoError, file.SaveAs(path));
                file.Close();

                DrillHoleFile reopened = new DrillHoleFile();
                Assert.Equal(DrillHoleFile.OpenFile_ErrorsEnum.NoError, reopened.Open(path));
                Assert.Equal("TEST-001", reopened.HoleID);
                Assert.Equal("RIG-7", reopened.RigID);
                Assert.Equal(2, reopened.SurveyList.Count());
                Assert.Equal(500000m, reopened.Collar.Easting);
                Assert.Equal(1000m, reopened.Target.TVD);
                reopened.Close();
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        [Fact]
        public void HoleFile_RejectsLegacyBinaryPayload()
        {
            string path = Path.Combine(Path.GetTempPath(), "sphereiq-legacy-" + Guid.NewGuid().ToString("N") + ".ezn");
            try
            {
                File.WriteAllBytes(path, new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF });
                DrillHoleFile file = new DrillHoleFile();
                DrillHoleFile.OpenFile_ErrorsEnum rc = file.Open(path);
                Assert.Equal(DrillHoleFile.OpenFile_ErrorsEnum.DeSerialisationError, rc);
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        [Fact]
        public void SeverityColor_MapsWarningAndSevere()
        {
            DrillHoleStatus w = new DrillHoleStatus();
            w.MinimumDLStateColor = SeverityColor.FromSeverity(MinDLSeverity.Warning);
            Assert.Equal(MinDLSeverity.Warning, w.MinimumDLSeverity);
            DrillHoleStatus s = new DrillHoleStatus();
            s.MinimumDLSeverity = MinDLSeverity.Severe;
            Assert.Equal((uint)0xFFFF0000, s.MinimumDLStateColor.ToArgb());
        }

        [Fact]
        public void ReportDocument_BuildsSurveyAndStatusPages()
        {
            ReportDocument doc = new ReportDocument("Hole TEST-001", "SphereIQ");
            doc.AppendCoverPage("Cover", "Hole TEST-001", "Drilling report");
            doc.AppendSurveyPage("Surveys", new List<AbsoluteSurvey> { new AbsoluteSurvey(10m, 5m, 350m, 100m, 200m, 150m, 0.5m) });
            DrillHoleStatus status = new DrillHoleStatus();
            status.MinimumDLStateString = "OK";
            status.MinimumDLSeverity = MinDLSeverity.Ok;
            doc.AppendHoleStatusPage("Hole Status", status);
            Assert.Equal(3, doc.NumberOfPages);
        }

        [Fact]
        public void HoleChartData_CapturesSeries()
        {
            HoleChartData chart = new HoleChartData();
            chart.SetCollar(new Collar(500000m, 7000000m), true);
            chart.SetTarget(new Target(500100m, 7000100m, 1000m), true);
            Assert.True(chart.Series["Collar"].Points.Count == 1);
            Assert.True(chart.Series["Target"].Points.Count == 1);
        }
    }
}
