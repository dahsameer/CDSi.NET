using System.Data;
using System.Reflection;
using System.Text;
using CDSi.NET.Test.Models;
using ExcelDataReader;

namespace CDSi.NET.Test;

public class MainTest
{
	private static readonly Dictionary<TestType, string> testResource = new()
	{
		{TestType.Healthy, "CDSi.NET.Test.Resources.cdsi-healthy-childhood-and-adult-test-cases-v4.42.xlsx"},
		{TestType.Condition, "CDSi.NET.Test.Resources.CDSi-Underlying-Conditions-Test-Cases-v4.5.xlsx"}
	};

	[Fact]
	public void HealthyTest()
	{
		var data = GetTestData(TestType.Healthy);
		var testCases = data.Tables[0].Rows.Cast<DataRow>().AsEnumerable().Select(x => new HealthyTest(x));
		foreach (var tc in testCases)
		{
			Console.WriteLine($"{tc.TestId} : {tc.Vaccines.Count()}");
		}
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