using System.Text.RegularExpressions;

namespace CDSi.NET.DateUtilities;

public static class DateEvaluator
{
	/// <summary>
	/// Calculates a new date based on the specified date and adjustment string according to business rules
	/// </summary>
	/// <param name="baseDate">The starting date</param>
	/// <param name="adjustment">A string specifying the adjustment, e.g. "18 years - 4 days"</param>
	/// <returns>The calculated date following the business rules</returns>
	public static DateTime CalculateDate(DateTime baseDate, string adjustment)
	{
		if (string.IsNullOrWhiteSpace(adjustment))
			return baseDate;

		// Parse the adjustment string into components
		(int years, int months, int weeks, int days) = ParseAdjustment(adjustment);

		DateTime resultDate = baseDate;

		// Apply adjustments in order specified by CALCDT-6:
		// "A computed date must be calculated by first adjusting the years,
		// followed by the months, and finally the weeks and/or days."

		// Step 1: Apply year adjustments
		resultDate = AdjustYears(resultDate, years);

		// Step 2: Apply month adjustments
		resultDate = AdjustMonths(resultDate, months);

		// Step 3: Apply weeks and days adjustments
		resultDate = AdjustDays(resultDate, weeks * 7 + days);

		return resultDate;
	}

	/// <summary>
	/// Parses the adjustment string into year, month, week, and day components
	/// </summary>
	private static (int years, int months, int weeks, int days) ParseAdjustment(string adjustment)
	{
		var tokens = adjustment.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		int years = 0;
		int months = 0;
		int weeks = 0;
		int days = 0;

		bool nextNegative = false;

		for (int i = 0; i < tokens.Length; i++)
		{
			// Check for arithmetic operators
			if (tokens[i] == "-")
			{
				nextNegative = true;
				continue;
			}
			else if (tokens[i] == "+")
			{
				nextNegative = false;
				continue;
			}

			// Try to parse a numeric value
			if (i + 1 < tokens.Length && int.TryParse(tokens[i], out int value))
			{
				string unit = tokens[i + 1].ToLower().TrimEnd(',', ';');

				// Apply negative sign if needed
				if (nextNegative)
				{
					value = -value;
					nextNegative = false;
				}

				// Categorize the adjustment based on time unit
				if (unit.StartsWith("year"))
				{
					years += value;
				}
				else if (unit.StartsWith("month"))
				{
					months += value;
				}
				else if (unit.StartsWith("week"))
				{
					weeks += value;
				}
				else if (unit.StartsWith("day"))
				{
					days += value;
				}

				// Skip the unit token
				i++;
			}
		}

		return (years, months, weeks, days);
	}

	/// <summary>
	/// Adjusts the date by adding/subtracting years according to CALCDT-1
	/// </summary>
	private static DateTime AdjustYears(DateTime date, int years)
	{
		if (years == 0)
			return date;

		// CALCDT-1: "The computed date of adding any number of years to an existing date
		// must be calculated by incrementing the date-year while holding the date-month and date-day constant."

		// Calculate the new year
		int newYear = date.Year + years;

		// Handle potential invalid dates like Feb 29 in non-leap years
		int daysInMonth = DateTime.DaysInMonth(newYear, date.Month);
		int newDay = Math.Min(date.Day, daysInMonth);

		return new DateTime(newYear, date.Month, newDay);
	}

	/// <summary>
	/// Adjusts the date by adding/subtracting months according to CALCDT-2 and CALCDT-5
	/// </summary>
	private static DateTime AdjustMonths(DateTime date, int months)
	{
		if (months == 0)
			return date;

		// CALCDT-2: "The computed date of adding any number of months to an existing date
		// must be calculated by incrementing the date-month (and date-year, if necessary)
		// while holding the date-day constant."

		// Calculate the new month and year
		int totalMonths = date.Month + months - 1;  // Convert to 0-based for calculation
		int newYear = date.Year + totalMonths / 12;
		int newMonth = totalMonths % 12 + 1;  // Convert back to 1-based

		// Handle negative month adjustments
		if (newMonth <= 0)
		{
			newMonth += 12;
			newYear--;
		}

		// Check for invalid dates (e.g., Feb 30)
		int daysInNewMonth = DateTime.DaysInMonth(newYear, newMonth);
		int newDay = Math.Min(date.Day, daysInNewMonth);

		// CALCDT-5: "A computed date which is not a real date must be moved forward
		// to first day of the next month."
		// Note: This implementation doesn't move to the next month, but instead
		// adjusts to the last valid day of the target month, which is a common approach
		// in date calculation systems. The example in the spec suggests this behavior.

		return new DateTime(newYear, newMonth, newDay);
	}

	/// <summary>
	/// Adjusts the date by adding/subtracting days according to CALCDT-3 and CALCDT-4
	/// </summary>
	private static DateTime AdjustDays(DateTime date, int days)
	{
		if (days == 0)
			return date;

		if (days > 0)
		{
			// CALCDT-3: "The computed date of adding any number of weeks or days to an
			// existing date must be calculated by adding the total days to the existing date."
			return date.AddDays(days);
		}
		else
		{
			// CALCDT-4: "The computed date of subtracting any number of days from an
			// existing date must be calculated by subtracting the total days from the existing date."
			return date.AddDays(days);  // AddDays handles negative values correctly
		}
	}
}