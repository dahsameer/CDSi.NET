using System.Data;
using CDSi.NET.Models;
using CDSi.NET.Utils;

namespace CDSi.NET.Test;


internal static class TestUtilities
{
	public static Patient AsPatient(this DataRow row)
	{
		return new Patient()
		{
			DOB = row.Field<DateTime>("DOB"),
			Gender = MiscUtils.GetGender(row.Field<string>("Gender")!)
		};
	}

	public static List<VaccineDoseAdministered> AsDoses(this DataRow row)
	{
		var doses = new List<VaccineDoseAdministered>();
		for (var i = 1; i <= 7; i++)
		{
			if (string.IsNullOrWhiteSpace(row[$"CVX_{i}"]?.ToString())) break;
			doses.Add(row.AsDose(i));
		}
		return doses;
	}

	public static VaccineDoseAdministered AsDose(this DataRow row, int num)
	{
		return new VaccineDoseAdministered()
		{
			CVX = row[$"CVX_{num}"]?.ToString()!,
			MVX = row[$"MVX_{num}"]?.ToString()!,
			AdministeredDate = row.Field<DateTime>($"Date_Administered_{num}")!,
		};
	}
}