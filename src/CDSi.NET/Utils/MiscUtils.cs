using CDSi.NET.Models;

namespace CDSi.NET.Utils;

public static class MiscUtils
{
	public static Gender GetGender(string gender)
	{
		return gender.ToLower() switch
		{
			"m" or "male" => Gender.Male,
			"f" or "female" => Gender.Female,
			"t" or "transgender" or "transexual" or "trans" => Gender.Transgender,
			_ => Gender.Unknown
		};
	}
}
