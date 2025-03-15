using System.Data;
using System.Reflection;
using System.Text;
using CDSi.NET.Test.Models;
using ExcelDataReader;
using Xunit.Sdk;

namespace CDSi.NET.Test;

internal static class TestDataLoader
{
	private static readonly Dictionary<TestType, string> testResource = new()
	{
		{TestType.Healthy, "CDSi.NET.Test.Resources.cdsi-healthy-childhood-and-adult-test-cases-v4.42.xlsx"},
		{TestType.Condition, "CDSi.NET.Test.Resources.CDSi-Underlying-Conditions-Test-Cases-v4.5.xlsx"}
	};

	public static TheoryData<HealthyTest> LoadHealthyData()
	{
		var data = GetTestData(TestType.Healthy);
		var testCases = data.Tables[0].Rows.Cast<DataRow>().AsEnumerable().Select(x => new HealthyTest(x));
		var theoryData = new TheoryData<HealthyTest>();
		foreach (var testCase in testCases)
		{
			theoryData.Add(testCase);
		}
		return theoryData;
	}

	public static TheoryData<ConditionTest> LoadConditionData()
	{
		var data = GetTestData(TestType.Condition);
		var testCases = data.Tables[0].Rows.Cast<DataRow>().AsEnumerable().Select(x => new ConditionTest(x));
		var theoryData = new TheoryData<ConditionTest>();
		foreach (var testCase in testCases)
		{
			theoryData.Add(testCase);
		}
		return theoryData;
	}

	private static DataSet GetTestData(TestType type)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		var assembly = Assembly.GetAssembly(typeof(MainTest))!;
		var resourceName = testResource[type];
		using var stream = assembly.GetManifestResourceStream(resourceName);
		using var reader = ExcelReaderFactory.CreateReader(stream);
		return reader.AsDataSet(new ExcelDataSetConfiguration()
		{
			UseColumnDataType = true,
			FilterSheet = (tableReader, sheetIndex) => sheetIndex == 2,
			ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
			{
				UseHeaderRow = true
			}
		});
	}
}