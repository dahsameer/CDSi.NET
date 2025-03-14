using System.Data;
using CDSi.NET.Models;

namespace CDSi.NET.Test.Models;


internal class TestCaseBase
{
	public TestCaseBase(DataRow dr)
	{
		TestId = dr.Field<string>("CDC_Test_ID")!;
		TestName = dr.Field<string>("Test_Case_Name")!;
		Patient = dr.AsPatient();
		Vaccines = dr.AsDoses();
		SeriesStatus = dr.Field<string>("Series_Status")!;
		EvaluationDate = dr.Field<DateTime>("Assessment_Date");
	}


	public string TestId { get; set; }
	public string TestName { get; set; }
	public Patient Patient { get; set; }
	public string SeriesStatus { get; set; }
	public DateTime EvaluationDate { get; set; }
	public List<ImmunizationRecord> Vaccines { get; set; }
}

internal class HealthyTest(DataRow dr) : TestCaseBase(dr)
{
}

internal class ConditionTest(DataRow dr) : TestCaseBase(dr)
{
}
