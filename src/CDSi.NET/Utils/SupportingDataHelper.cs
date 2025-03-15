using System.Reflection;
using System.Xml.Serialization;
using CDSi.NET.Models.Generated;

namespace CDSi.NET.Utils;

public static class SupportingDataHelper
{
	private const string SCHEDULE_SUPPORTING_DATA_RESOURCE_NAME = "CDSi.NET.Resources.ScheduleSupportingData.xml";
	private const string ANTIGEN_SUPPORTING_DATA_RESOURCE_PATTERN = "CDSi.NET.Resources.AntigenSupportingData- ";

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

	public static scheduleSupportingData ReadScheduleData()
	{
		var assembly = Assembly.GetAssembly(typeof(SupportingDataHelper))!;
		var resource = assembly.GetManifestResourceStream(SCHEDULE_SUPPORTING_DATA_RESOURCE_NAME)!;
		var deserializer = new XmlSerializer(typeof(scheduleSupportingData));
		return (scheduleSupportingData)deserializer.Deserialize(resource)!;
	}
}