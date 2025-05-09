using System.Reflection;
using System.Xml.Serialization;
using CDSi.NET.Models.Generated;

namespace CDSi.NET.Utils;

public static class SupportingDataHelper
{
	private const string SCHEDULE_SUPPORTING_DATA_RESOURCE_NAME = "CDSi.NET.Resources.ScheduleSupportingData.xml";
	private const string ANTIGEN_SUPPORTING_DATA_RESOURCE_PATTERN = "CDSi.NET.Resources.AntigenSupportingData- ";

	/// <summary>
	/// Reads the antigen supporting data from the embedded resources.
	/// </summary>
	/// <returns>Dictionary of key: antigen name | value: antigen supportind data for that antigen</returns>
	public static Dictionary<string, antigenSupportingData> ReadAntigenData()
	{
		var deserializer = new XmlSerializer(typeof(antigenSupportingData));
		var assembly = Assembly.GetAssembly(typeof(SupportingDataHelper))!;

		return assembly.GetManifestResourceNames()
			  .Where(x => x.StartsWith(ANTIGEN_SUPPORTING_DATA_RESOURCE_PATTERN))
			  .Select(x => assembly.GetManifestResourceStream(x)!)
			  .Select(x => (antigenSupportingData)deserializer.Deserialize(x)!)
			  .Select(x => KeyValuePair.Create(x.series[0].targetDisease, x))
			  .ToDictionary(x => x.Key, x => x.Value);
	}

	/// <summary>
	/// Reads the schedule supporting data from the embedded resources.
	/// </summary>
	/// <returns>Schedule Supporting data model</returns>
	public static scheduleSupportingData ReadScheduleData()
	{
		var assembly = Assembly.GetAssembly(typeof(SupportingDataHelper))!;
		var resource = assembly.GetManifestResourceStream(SCHEDULE_SUPPORTING_DATA_RESOURCE_NAME)!;
		var deserializer = new XmlSerializer(typeof(scheduleSupportingData));
		return (scheduleSupportingData)deserializer.Deserialize(resource)!;
	}
}