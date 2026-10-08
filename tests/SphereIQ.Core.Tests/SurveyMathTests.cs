using System;
using System.IO;
using System.Text;
using EzyPlanData;
using EzyPlanMath;
using Xunit;

namespace SphereIQ.Core.Tests
{
    public sealed class SurveyMathTests
    {
        private static SurveyList TwoSurveyList()
        {
            SurveyList list = new SurveyList();
            Survey s0 = new Survey();
            Assert.Equal(Survey.SetMDEnum.ValueSet, s0.SetMD(0m));
            Assert.Equal(Survey.SetDipEnum.ValueSet, s0.SetDip(5m));
            Assert.Equal(Survey.SetAzimuthEnum.ValueSet, s0.SetAzimuth(10m));
            Assert.Equal(SurveyList.AppendErrorsEnum.NoError, list.Append(s0));
            Survey s1 = new Survey();
            Assert.Equal(Survey.SetMDEnum.ValueSet, s1.SetMD(100m));
            Assert.Equal(Survey.SetDipEnum.ValueSet, s1.SetDip(6m));
            Assert.Equal(Survey.SetAzimuthEnum.ValueSet, s1.SetAzimuth(12m));
            Assert.Equal(SurveyList.AppendErrorsEnum.NoError, list.Append(s1));
            return list;
        }

        [Fact]
        public void CsvImport_MapsColumnsAndValidatesZeroMD()
        {
            SurveyList list = new SurveyList();
            string csv = "MD,Dip,Azi\n0,5,10\n100,6,12\n";
            using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(csv))))
            {
                ICsvReader csvReader = new SimpleCsvReader(reader, hasHeaders: true);
                ImportSurveyMapping map = new ImportSurveyMapping("t", 2, 1, 0, setHeader: false);
                SurveyList.ImportCSV_ErrorsEnum rc = list.ImportCSV(csvReader, map);
                Assert.Equal(SurveyList.ImportCSV_ErrorsEnum.NoError, rc);
                Assert.Equal(2, list.Count());
            }
        }

        [Fact]
        public void CsvImport_RejectsNonZeroFirstMD()
        {
            SurveyList list = new SurveyList();
            string csv = "50,5,10\n100,6,12\n";
            using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(csv))))
            {
                ICsvReader csvReader = new SimpleCsvReader(reader, hasHeaders: false);
                ImportSurveyMapping map = new ImportSurveyMapping("t", 2, 1, 0, setHeader: false);
                SurveyList.ImportCSV_ErrorsEnum rc = list.ImportCSV(csvReader, map);
                Assert.Equal(SurveyList.ImportCSV_ErrorsEnum.FirstSurveyMustHaveZeroMD, rc);
            }
        }

        [Fact]
        public void AbsoluteSurvey_ComputationSucceeds()
        {
            Collar collar = new Collar(500000m, 7000000m);
            SurveyList surveys = TwoSurveyList();
            EzyPlanMath.Math math = new EzyPlanMath.Math();
            EzyPlanMath.Math.ThreeDWellPlan plan = new EzyPlanMath.Math.ThreeDWellPlan();
            AbsoluteSurveyList result = new AbsoluteSurveyList();
            AbsoluteSurvey_Errors rc = plan.AbsoluteSurvey(collar, surveys, ref result);
            Assert.Equal(AbsoluteSurvey_Errors.NoError, rc);
            Assert.True(result.Count() >= 2);
            Assert.Equal(0m, result.RecordAt(0).MD);
        }
    }
}
