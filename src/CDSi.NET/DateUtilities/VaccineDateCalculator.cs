namespace CDSi.NET.DateUtilities;

/// <summary>
/// Contains methods for calculating various dates related to patient vaccinations
/// based on the logical component date rules.
/// </summary>
public static class VaccinationDateCalculator
{
	/// <summary>
	/// Calculates a patient's age-related date based on the rule type and patient's birth date
	/// </summary>
	/// <param name="ruleId">The business rule ID to apply</param>
	/// <param name="dateOfBirth">Patient's date of birth</param>
	/// <param name="ageValue">The age value in years to apply (maximum, minimum, etc.)</param>
	/// <returns>The calculated date based on the rule</returns>
	public static DateTime CalculateAgeBasedDate(string ruleId, DateTime dateOfBirth, int ageValue)
	{
		// Validate inputs
		if (string.IsNullOrWhiteSpace(ruleId))
			throw new ArgumentException("Rule ID cannot be empty", nameof(ruleId));

		switch (ruleId.ToUpperInvariant())
		{
			// CALCDTAGE-1: A patient's maximum age date must be calculated as the patient's date of birth plus the maximum age.
			case "CALCDTAGE-1":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTAGE-2: A patient's latest recommended age date must be calculated as the patient's date of birth plus the latest recommended age.
			case "CALCDTAGE-2":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTAGE-3: A patient's earliest recommended age date must be calculated as the patient's date of birth plus the earliest recommended age.
			case "CALCDTAGE-3":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTAGE-4: A patient's minimum age date must be calculated as the patient's date of birth plus the minimum age.
			case "CALCDTAGE-4":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTAGE-5: A patient's absolute minimum age date must be calculated as the patient's date of birth plus the absolute minimum age.
			case "CALCDTAGE-5":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTALLOW-1: A patient's allowable vaccine type begin age date must be calculated as the patient's date of birth plus the vaccine type begin age of an allowable vaccine.
			case "CALCDTALLOW-1":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTALLOW-2: A patient's allowable vaccine type end age date must be calculated as the patient's date of birth plus the vaccine type end age of an allowable vaccine.
			case "CALCDTALLOW-2":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTCI-1: A patient's contraindication begin age date must be calculated as the patient's date of birth plus the contraindication begin age of a contraindication.
			case "CALCDTCI-1":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTCI-2: A patient's contraindication end age date must be calculated as the patient's date of birth plus the contraindication end age of a contraindication.
			case "CALCDTCI-2":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTIND-1: A patient's indication begin age date must be calculated as the patient's date of birth plus the indication begin age of an indication.
			case "CALCDTIND-1":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTIND-2: A patient's indication end age date must be calculated as the patient's date of birth plus the indication end age of an indication.
			case "CALCDTIND-2":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTPREF-1: A patient's preferable vaccine type begin age date must be calculated as the patient's date of birth plus the vaccine type begin age of a preferable vaccine.
			case "CALCDTPREF-1":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTPREF-2: A patient's preferable vaccine type end age date must be calculated as the patient's date of birth plus the vaccine type end age of a preferable vaccine.
			case "CALCDTPREF-2":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTSKIP-3: A patient's conditional skip begin age date must be calculated as the patient's date of birth plus the conditional skip begin age of a conditional skip.
			case "CALCDTSKIP-3":
				return AddYearsToDate(dateOfBirth, ageValue);

			// CALCDTSKIP-4: A patient's conditional skip end age date must be calculated as the patient's date of birth plus the conditional skip end age of a conditional skip.
			case "CALCDTSKIP-4":
				return AddYearsToDate(dateOfBirth, ageValue);

			default:
				throw new ArgumentException($"Rule ID '{ruleId}' is not a valid age-based rule", nameof(ruleId));
		}
	}

	/// <summary>
	/// Calculates conflict interval dates for vaccines based on previous administration
	/// </summary>
	/// <param name="ruleId">The business rule ID to apply</param>
	/// <param name="previousDoseDate">Date of previous dose administration</param>
	/// <param name="conflictInterval">Interval in days for conflict rule</param>
	/// <param name="isImpactedVaccineType">Is the current vaccine an impacted type</param>
	/// <param name="isConflictingVaccineType">Is the previous dose a conflicting type</param>
	/// <param name="evaluationStatus">Evaluation status of previous dose</param>
	/// <returns>The calculated conflict date</returns>
	public static DateTime CalculateConflictDate(
		string ruleId,
		DateTime previousDoseDate,
		int conflictInterval,
		bool isImpactedVaccineType = false,
		bool isConflictingVaccineType = false,
		string? evaluationStatus = null)
	{
		// Validate inputs
		if (string.IsNullOrWhiteSpace(ruleId))
			throw new ArgumentException("Rule ID cannot be empty", nameof(ruleId));

		switch (ruleId.ToUpperInvariant())
		{
			// CALCDTCONFLICT-1: The conflict begin interval date for a previous vaccine dose administered
			case "CALCDTCONFLICT-1":
				if (isImpactedVaccineType && isConflictingVaccineType)
				{
					return previousDoseDate.AddDays(conflictInterval);
				}
				throw new ArgumentException("Conditions not met for CALCDTCONFLICT-1");

			// CALCDTCONFLICT-2: The conflict end interval date for a previous vaccine dose administered
			case "CALCDTCONFLICT-2":
				bool hasValidEvaluation = evaluationStatus == "Valid" || evaluationStatus == "Valid'";

				if (isImpactedVaccineType && isConflictingVaccineType)
				{
					if (hasValidEvaluation)
					{
						return previousDoseDate.AddDays(conflictInterval);
					}
				}
				throw new ArgumentException("Conditions not met for CALCDTCONFLICT-2");

			// CALCDTCONFLICT-3: The forecast conflict end date of a vaccine type conflict
			case "CALCDTCONFLICT-3":
				if (isImpactedVaccineType && isConflictingVaccineType)
				{
					return previousDoseDate.AddDays(conflictInterval);
				}
				throw new ArgumentException("Conditions not met for CALCDTCONFLICT-3");

			default:
				throw new ArgumentException($"Rule ID '{ruleId}' is not a valid conflict rule", nameof(ruleId));
		}
	}

	/// <summary>
	/// Calculates interval-based dates for vaccines based on reference dose
	/// </summary>
	/// <param name="ruleId">The business rule ID to apply</param>
	/// <param name="referenceDoseDate">Date of reference dose</param>
	/// <param name="intervalValue">The interval value in days to apply</param>
	/// <returns>The calculated interval date</returns>
	public static DateTime CalculateIntervalDate(string ruleId, DateTime referenceDoseDate, int intervalValue)
	{
		// Validate inputs
		if (string.IsNullOrWhiteSpace(ruleId))
			throw new ArgumentException("Rule ID cannot be empty", nameof(ruleId));

		switch (ruleId.ToUpperInvariant())
		{
			// CALCDTINT-3: A patient's absolute minimum interval date
			case "CALCDTINT-3":
				return referenceDoseDate.AddDays(intervalValue);

			// CALCDTINT-4: A patient's minimum interval date
			case "CALCDTINT-4":
				return referenceDoseDate.AddDays(intervalValue);

			// CALCDTINT-5: A patient's earliest recommended interval date
			case "CALCDTINT-5":
				return referenceDoseDate.AddDays(intervalValue);

			// CALCDTINT-6: A patient's latest recommended interval date
			case "CALCDTINT-6":
				return referenceDoseDate.AddDays(intervalValue);

			// CALCDTSKIP-5: A patient's conditional skip interval date
			case "CALCDTSKIP-5":
				return referenceDoseDate.AddDays(intervalValue);

			default:
				throw new ArgumentException($"Rule ID '{ruleId}' is not a valid interval rule", nameof(ruleId));
		}
	}

	/// <summary>
	/// Determines the reference dose date for calculating intervals
	/// </summary>
	/// <param name="ruleId">The business rule ID to apply</param>
	/// <param name="vaccineHistory">List of previous vaccine doses with their properties</param>
	/// <param name="currentVaccineType">The type of the current vaccine</param>
	/// <param name="targetDoseNumber">The target dose number</param>
	/// <param name="observationDate">Date of most recent observation if applicable</param>
	/// <param name="observationCode">Observation code if applicable</param>
	/// <returns>The reference dose date</returns>
	public static DateTime GetReferenceDoseDate(
		string ruleId,
		List<VaccineDose> vaccineHistory,
		string? currentVaccineType = null,
		int? targetDoseNumber = null,
		DateTime? observationDate = null,
		string? observationCode = null)
	{
		// Validate inputs
		if (string.IsNullOrWhiteSpace(ruleId))
			throw new ArgumentException("Rule ID cannot be empty", nameof(ruleId));

		if (vaccineHistory == null || vaccineHistory.Count == 0)
			throw new ArgumentException("Vaccine history cannot be empty", nameof(vaccineHistory));

		switch (ruleId.ToUpperInvariant())
		{
			// CALCDTINT-1: Reference dose date for an interval - from immediate previous dose
			case "CALCDTINT-1":
				var immediateDosei1 = FindImmediatePreviousDose(vaccineHistory, isInadvertent: false);
				if (immediateDosei1 != null && immediateDosei1.EvaluationStatus == "Valid" || immediateDosei1!.EvaluationStatus == "Not Valid")
				{
					return immediateDosei1.DateAdministered;
				}
				throw new ArgumentException("No qualifying dose found for CALCDTINT-1");

			// CALCDTINT-2: Reference dose date for an interval - satisfies target dose
			case "CALCDTINT-2":
				if (!targetDoseNumber.HasValue)
					throw new ArgumentException("Target dose number is required for CALCDTINT-2");

				var matchingDosei2 = FindMatchingTargetDose(vaccineHistory, targetDoseNumber.Value);
				if (matchingDosei2 != null)
				{
					var immediateDosei2 = FindImmediatePreviousDose(vaccineHistory, isInadvertent: false);
					if (immediateDosei2 != null && immediateDosei2.FromImmediatePrevious == "N")
					{
						return matchingDosei2.DateAdministered;
					}
				}
				throw new ArgumentException("No qualifying dose found for CALCDTINT-2");

			// CALCDTINT-8: Reference dose date for an interval - same vaccine type
			case "CALCDTINT-8":
				if (string.IsNullOrWhiteSpace(currentVaccineType))
					throw new ArgumentException("Current vaccine type is required for CALCDTINT-8");

				var matchingDosei8 = FindMostRecentSameVaccineType(vaccineHistory, currentVaccineType);
				if (matchingDosei8 != null)
				{
					var immediateDosei8 = FindImmediatePreviousDose(vaccineHistory, isInadvertent: false);
					if (immediateDosei8 != null &&
						immediateDosei8.FromImmediatePrevious == "N" &&
						matchingDosei8.VaccineType != "n/a")
					{
						return matchingDosei8.DateAdministered;
					}
				}
				throw new ArgumentException("No qualifying dose found for CALCDTINT-8");

			// CALCDTINT-9: Reference dose date for an interval - observation date
			case "CALCDTINT-9":
				if (!observationDate.HasValue)
					throw new ArgumentException("Observation date is required for CALCDTINT-9");

				var immediateDosei9 = FindImmediatePreviousDose(vaccineHistory, isInadvertent: false);
				if (immediateDosei9 != null &&
					immediateDosei9.FromImmediatePrevious == "N" &&
					observationCode != "n/a")
				{
					return observationDate.Value;
				}
				throw new ArgumentException("Conditions not met for CALCDTINT-9");

			default:
				throw new ArgumentException($"Rule ID '{ruleId}' is not a valid reference dose rule", nameof(ruleId));
		}
	}

	/// <summary>
	/// Calculates the lot number expiration date based on available information
	/// </summary>
	/// <param name="month">Month value (1-12)</param>
	/// <param name="year">Year value</param>
	/// <param name="day">Optional day value</param>
	/// <returns>The calculated expiration date</returns>
	public static DateTime CalculateLotExpiration(int month, int year, int? day = null)
	{
		// CALCDTLOTEXP-1: The lot number expiration date must be one of the following:
		// - The lot number expiration date if the month, day, and year are all known.
		// - Last day of the month if only the month and year are known.

		if (day.HasValue)
		{
			// Full date is known
			return new DateTime(year, month, day.Value);
		}
		else
		{
			// Only month and year are known, use last day of month
			return new DateTime(year, month, DateTime.DaysInMonth(year, month));
		}
	}

	#region Helper Methods

	/// <summary>
	/// Helper method to add years to a date, following business rules for date calculations
	/// </summary>
	private static DateTime AddYearsToDate(DateTime baseDate, int years)
	{
		// Try to maintain the same day of month
		int targetYear = baseDate.Year + years;
		int targetMonth = baseDate.Month;
		int targetDay = baseDate.Day;

		// Check if the target date is valid (e.g., handle Feb 29 in non-leap years)
		int daysInMonth = DateTime.DaysInMonth(targetYear, targetMonth);
		if (targetDay > daysInMonth)
		{
			targetDay = daysInMonth;
		}

		return new DateTime(targetYear, targetMonth, targetDay);
	}

	/// <summary>
	/// Finds the immediate previous dose from vaccine history
	/// </summary>
	private static VaccineDose? FindImmediatePreviousDose(List<VaccineDose> vaccineHistory, bool isInadvertent)
	{
		// Sort by date administered descending
		vaccineHistory.Sort((a, b) => b.DateAdministered.CompareTo(a.DateAdministered));

		// Return the most recent dose that matches criteria
		foreach (var dose in vaccineHistory)
		{
			// Skip inadvertent doses if specified
			if (isInadvertent && dose.IsInadvertent)
				continue;

			return dose;
		}

		return null;
	}

	/// <summary>
	/// Finds a dose that satisfies the target dose number
	/// </summary>
	private static VaccineDose? FindMatchingTargetDose(List<VaccineDose> vaccineHistory, int targetDoseNumber)
	{
		// Sort by date administered descending
		vaccineHistory.Sort((a, b) => b.DateAdministered.CompareTo(a.DateAdministered));

		// Return the most recent dose that matches criteria
		foreach (var dose in vaccineHistory)
		{
			if (dose.TargetDoseNumber == targetDoseNumber)
			{
				return dose;
			}
		}

		return null;
	}

	/// <summary>
	/// Finds the most recent dose with the same vaccine type
	/// </summary>
	private static VaccineDose? FindMostRecentSameVaccineType(List<VaccineDose> vaccineHistory, string vaccineType)
	{
		// Sort by date administered descending
		vaccineHistory.Sort((a, b) => b.DateAdministered.CompareTo(a.DateAdministered));

		// Return the most recent dose that matches criteria
		foreach (var dose in vaccineHistory)
		{
			if (dose.VaccineType == vaccineType && !dose.IsInadvertent)
			{
				return dose;
			}
		}

		return null;
	}

	#endregion
}

/// <summary>
/// Represents a vaccine dose in a patient's history
/// </summary>
public class VaccineDose
{
	/// <summary>
	/// Date the vaccine was administered
	/// </summary>
	public DateTime DateAdministered { get; set; }

	/// <summary>
	/// Type of vaccine administered
	/// </summary>
	public string? VaccineType { get; set; }

	/// <summary>
	/// Evaluation status of the dose (Valid, Not Valid, etc.)
	/// </summary>
	public string? EvaluationStatus { get; set; }

	/// <summary>
	/// Whether the dose is from an immediate previous administration
	/// </summary>
	public string? FromImmediatePrevious { get; set; }

	/// <summary>
	/// Target dose number in the series
	/// </summary>
	public int TargetDoseNumber { get; set; }

	/// <summary>
	/// Whether this was an inadvertent administration
	/// </summary>
	public bool IsInadvertent { get; set; }
}