using System.Data;
using CDSi.NET.Models;

namespace CDSi.NET.Test;


internal static class TestUtilities
{
	public static Patient AsPatient(this DataRow row)
	{
		return new Patient()
		{
			DOB = row.Field<DateTime>("DOB"),
			Gender = GetGender(row.Field<string>("Gender")!)
		};
	}

	public static List<ImmunizationRecord> AsDoses(this DataRow row)
	{
		var doses = new List<ImmunizationRecord>();
		for (var i = 1; i <= 7; i++)
		{
			if (string.IsNullOrWhiteSpace(row[$"CVX_{i}"]?.ToString())) break;
			doses.Add(row.AsDose(i));
		}
		return doses;
	}

	public static ImmunizationRecord AsDose(this DataRow row, int num)
	{
		return new ImmunizationRecord()
		{
			CVX = row[$"CVX_{num}"]?.ToString()!,
			MVX = row[$"MVX_{num}"]?.ToString()!,
			AdministeredDate = row.Field<DateTime>($"Date_Administered_{num}")!,
		};
	}

	public static Gender GetGender(string gender)
	{
		return gender.ToLower() switch
		{
			"m" or "male" => Gender.Male,
			"f" or "female" => Gender.Female,
			_ => Gender.Unknown
		};
	}
}