using CDSi.NET.DateUtilities;
using CDSi.NET.Models;
using CDSi.NET.Models.Generated;
using CDSi.NET.Utils;

namespace CDSi.NET;

public class CDSiEngine
{
	private static bool _isInitialized = false;

	private static Dictionary<string, antigenSupportingData> _antigenData = null!;
	private static scheduleSupportingData _scheduleData = null!;

	/// <summary>
	/// Initializes the CDSi engine. This method should be called before using any other methods in the library.
	/// It reads the antigen and schedule data from the supporting data files and sets in the static variables.
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

	public static void EvaluateSeries(EvaluationRequest request)
	{
		if (!_isInitialized)
		{
			throw new Exception("CDSi engine is not initialized. Call Initialize() method before using this method.");
		}
		var organizedHistory = EvaluationHelper.OrganizeImmunizationHistory(request, _scheduleData!);
		List<antigenSupportingDataSeries> relevant_series = FindRelevantSeries(request.Patient);

		foreach (var series in relevant_series)
		{

		}
	}

	/// <summary>
	/// 4.3 CREATE RELEVANT PATIENT SERIES 
	/// </summary>
	/// <param name="patient"></param>
	/// <param name="relevant_series"></param>
	private static List<antigenSupportingDataSeries> FindRelevantSeries(Patient patient)
	{
		List<antigenSupportingDataSeries> relevant_series = [];
		foreach (var antigen in _antigenData)
		{
			foreach (var series in antigen.Value.series)
			{
				// TABLE 5-4 DOES THE INDICATION APPLY TO THE PATIENT? 
				var indication_apply = DoesIndicationApplyToPatient(patient, series.indication);

				// TABLE 5-5 IS AN ANTIGEN SERIES A RELEVANT PATIENT SERIES FOR A PATIENT?
				var is_gender_valid = series.requiredGender.Any(x => MiscUtils.GetGender(x) == patient.Gender);
				var is_series_type_standard_or_evaluationonly = series.seriesType == "Standard" || series.seriesType == "Evaluation Only";
				var is_antigen_series_relevant = IsAntigenSeriesRelevant(is_gender_valid, is_series_type_standard_or_evaluationonly, indication_apply);
				if (is_antigen_series_relevant)
				{
					relevant_series.Add(series);
				}
			}
		}
		return relevant_series;
	}

	/// <summary>
	/// TABLE 5-4 DOES THE INDICATION APPLY TO THE PATIENT? 
	/// </summary>
	/// <param name="patient"></param>
	/// <param name="indications"></param>
	/// <returns></returns>
	private static bool DoesIndicationApplyToPatient(
		Patient patient,
		antigenSupportingDataSeriesIndication[] indications
		)
	{
		foreach (var indication in indications)
		{
			var patient_has_indication = patient.Observations.Any(o => string.Equals(o.ObservationCode, indication.observationCode.code, StringComparison.OrdinalIgnoreCase));
			if (!patient_has_indication)
				continue;
			var validBeginAge = DateEvaluator.CalculateDate(patient.DOB, indication.beginAge);
			var validEndAge = DateEvaluator.CalculateDate(patient.DOB, indication.endAge);
			if (DateTime.Compare(validBeginAge, DateTime.Now) > 0 || DateTime.Compare(validEndAge, DateTime.Now) < 0)
				continue;
			// HAS INDICATION
			return true;
		}
		return false;
	}

	/// <summary>
	/// TABLE 5-5 IS AN ANTIGEN SERIES A RELEVANT PATIENT SERIES FOR A PATIENT?
	/// </summary>
	/// <param name="isGenderValid"></param>
	/// <param name="isSeriesTypeStandardOrEvaluationOnly"></param>
	/// <param name="hasIndication"></param>
	/// <returns></returns>
	private static bool IsAntigenSeriesRelevant(
		bool isGenderValid,
		bool isSeriesTypeStandardOrEvaluationOnly,
		bool hasIndication)
	{
		if (!isGenderValid)
		{
			return false;
		}

		if (isSeriesTypeStandardOrEvaluationOnly && isGenderValid)
		{
			return true;
		}

		if (!isSeriesTypeStandardOrEvaluationOnly && isGenderValid && hasIndication)
		{
			return true;
		}

		return false;
	}
}
