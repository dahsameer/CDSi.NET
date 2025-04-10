﻿using CDSi.NET.Models.Generated;
using CDSi.NET.Utils;

namespace CDSi.NET;

public class CDSiEngine
{
	private static bool _isInitialized = false;

	private static Dictionary<string, antigenSupportingData>? _antigenData = null;
	private static scheduleSupportingData? _scheduleData = null;

	/// <summary>
	/// Initializes the CDSi engine. This method should be called before using any other methods in the library.
	/// It reads the antigen and schedule data from the supporting data files.
	/// </summary>
	public static void Initialize()
	{
		if (_isInitialized) return;
		try
		{
			_antigenData = SupportingDataHelper.ReadAntigenData();
			_scheduleData = SupportingDataHelper.ReadScheduleData();
			_isInitialized = true;
		}
		catch (Exception ex)
		{
			throw new Exception("Error initializing CDSi engine: " + ex.Message, ex);
		}
	}

	
}
