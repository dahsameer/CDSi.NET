using CDSi.NET.DateUtilities;
using CDSi.NET.Models;
using CDSi.NET.Models.Generated;

namespace CDSi.NET.Utils;

internal class EvaluationHelper
{
	/// <summary>
	/// 4.2 ORGANIZE IMMUNIZATION HISTORY 
	/// </summary>
	/// <param name="vaccines"></param>
	/// <param name="scheduleSupportingData"></param>
	/// <returns></returns>
	public static Dictionary<string, List<VaccineDoseAdministered>> OrganizeImmunizationHistory(EvaluationRequest request, scheduleSupportingData scheduleSupportingData)
	{
		var vaccines = request.Immunizations?.OrderBy(x => x.AdministeredDate);

		if (vaccines == null || scheduleSupportingData?.cvxToAntigenMap == null)
			return []; // Return empty list if input is invalid

		// 1. Create a flat list pairing each relevant antigen with the original dose administered.
		var antigenDosePairs = new Dictionary<string, List<VaccineDoseAdministered>>();
		foreach (var vaccineDose in vaccines)
		{
			if (string.IsNullOrEmpty(vaccineDose.CVX)) continue; // Skip doses without CVX

			// Find the mapping for the vaccine's CVX code
			var mapping = scheduleSupportingData.cvxToAntigenMap.SingleOrDefault(x => x.cvx == vaccineDose.CVX);

			if (mapping == null) continue;

			// A single CVX code can map to multiple antigens (e.g., MMR)
			if (mapping.association == null) continue;

			foreach (var association in mapping.association)
			{
				if (string.IsNullOrEmpty(association.antigen)) continue;

				if(!string.IsNullOrWhiteSpace(association.associationBeginAge))
				{
					var validBeginAge = DateEvaluator.CalculateDate(request.Patient.DOB, association.associationBeginAge);
					if(DateTime.Compare(validBeginAge, DateTime.Now) < 0)
					{
						continue; // Skip if the dose is before the association begin age
					}
				}

				if (!string.IsNullOrWhiteSpace(association.associationEndAge))
				{
					var validEndAge = DateEvaluator.CalculateDate(request.Patient.DOB, association.associationEndAge);
					if (DateTime.Compare(validEndAge, DateTime.Now) > 0)
					{
						continue; // Skip if the dose is after the association end age
					}
				}
				// TODO: Implement age filtering if AssociationBeginAge/AssociationEndAge are available
				// Requires Patient DOB to calculate age at administration.
				// For now, we add all associated antigens.
				antigenDosePairs.TryGetValue(association.antigen, out var list);
				(list ??= []).Add(vaccineDose);
				antigenDosePairs[association.antigen] = list;
			}
		}

		return antigenDosePairs.OrderBy(x => x.Key).ToDictionary();
	}
}
