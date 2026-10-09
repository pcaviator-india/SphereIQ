using System.IO;
using EzyPlanData;
using EzyPlanStorage;
using SphereIQ.Reports;
using Xunit;

namespace SphereIQ.Core.Tests
{
    public sealed class HolePlanningEngineTests
    {
        private static DrillHoleFile MakeHole()
        {
            DrillHoleFile f = new DrillHoleFile();
            f.HoleID = "ENG-1";
            f.Collar = new Collar(500000m, 7000000m);
            f.Target = new Target(500050m, 7000050m, 300m);
            SurveyList sl = new SurveyList();
            Survey s0 = new Survey();
            s0.SetMD(0m); s0.SetDip(0m); s0.SetAzimuth(0m);
            sl.Append(s0);
            Survey s1 = new Survey();
            s1.SetMD(200m); s1.SetDip(2m); s1.SetAzimuth(45m);
            sl.Append(s1);
            f.SurveyList = sl;
            return f;
        }

        [Fact]
        public void Recalculate_ProducesStatusAndChartSeries()
        {
            DrillHoleFile hole = MakeHole();
            HolePlanningEngine engine = new HolePlanningEngine();
            DrillHoleStatus status = engine.Recalculate(hole);
            Assert.NotNull(status);
            Assert.False(string.IsNullOrEmpty(status.MinimumDLStateString));
            Assert.NotNull(status.AbsoluteSurveyList);
            Assert.NotNull(status.CurrentDistanceFromTarget);
            Assert.True(status.AbsoluteSurveyList.Count() >= 2);
        }

        [Fact]
        public void Recalculate_RoundTripsThroughFile()
        {
            DrillHoleFile hole = MakeHole();
            DrillHoleStatus status = new HolePlanningEngine().Recalculate(hole);
            string path = Path.Combine(Path.GetTempPath(), "sphereiq-eng-" + System.Guid.NewGuid().ToString("N") + ".ezn");
            try
            {
                Assert.Equal(DrillHoleFile.SaveAs_ErrorsEnum.NoError, hole.SaveAs(path));
                hole.Close();
                DrillHoleFile re = new DrillHoleFile();
                Assert.Equal(DrillHoleFile.OpenFile_ErrorsEnum.NoError, re.Open(path));
                DrillHoleStatus status2 = new HolePlanningEngine().Recalculate(re);
                Assert.Equal(status.MinimumDLStateString, status2.MinimumDLStateString);
                Assert.Equal(status.CurrentDistanceFromTarget.HorizontalDeparture,
                    status2.CurrentDistanceFromTarget.HorizontalDeparture);
                re.Close();
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        [Fact]
        public void Recalculate_DaughterHole_SidetracksAtMD()
        {
            DrillHoleFile hole = MakeHole();
            hole.SideTrackMD = 100m;
            Assert.Equal(DrillHoleFile.DrillHoleTypeEnum.daughterHole, hole.DrillHoleType);
            DrillHoleStatus status = new HolePlanningEngine().Recalculate(hole);
            Assert.NotNull(status.SidetrackSurvey);
        }
    }
}
