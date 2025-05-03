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
	public static Dictionary<string, List<VaccineDoseAdministered>> OrganizeImmunizationHistory(IEnumerable<VaccineDoseAdministered> vaccines, scheduleSupportingData scheduleSupportingData)
	{
		if (vaccines == null || scheduleSupportingData?.cvxToAntigenMap == null)
			return []; // Return empty list if input is invalid

		// 1. Create a flat list pairing each relevant antigen with the original dose administered.
		var antigenDosePairs = new Dictionary<string, List<VaccineDoseAdministered>>();
		vaccines = vaccines.OrderBy(x => x.AdministeredDate);
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
