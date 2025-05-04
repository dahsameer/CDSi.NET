namespace CDSi.NET.Models;


public record EvaluationRequest
{
	public required Patient Patient { get; set; }
	public List<VaccineDoseAdministered>? Immunizations { get; set; }
	public DateTime AssessmentDate { get; set; }
}

public record Patient
{
	public string? Id { get; set; } // if needed to track later on
	public DateTime DOB { get; set; }
	public Gender Gender { get; set; }
	public Observation[] Observations { get; set; } = [];
}

public enum Gender
{
	Male,
	Female,
	Transgender,
	Unknown
}

public record VaccineDoseAdministered
{
	public string? Name { get; set; }
	public required string CVX { get; set; }
	public string? MVX { get; set; }
	public DateTime AdministeredDate { get; set; }
}

public record Observation
{
	public required string ObservationCode { get; set; }
	public string? ObservationCodingSystem { get; set; }
	public DateTime? ObservationDate { get; set; }
	public string? ObservationValue { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
}

public enum EvaluationStatus
{
	/// <summary>
	/// An evaluation status that indicates the vaccine dose administered was not administered according to ACIP 
	/// recommendations, but the dose does not need to be repeated (including maximum age and extra doses)
	/// </summary>
	Extraneous,
	/// <summary>
	/// An evaluation status that indicates the vaccine dose administered was not administered according to ACIP 
	/// recommendations and must be repeated at an appropriate time in the future 
	/// </summary>
	Not_Valid,
	/// <summary>
	/// An evaluation status that indicates the vaccine dose administered has a known dose condition 
	/// (e.g., expired, sub-potent, and recall) which requires the dose to be repeated at an appropriate time in the future
	/// </summary>
	Sub_Standard,
	/// <summary>
	/// An evaluation status that indicates the vaccine dose administered was administered according to ACIP recommendations
	/// </summary>
	Valid
}

public enum TargetDoseStatus
{
	/// <summary>
	/// A target dose status that indicates no vaccine dose administered has met the goals of the target dose
	/// </summary>
	Not_Satisfied,
	/// <summary>
	/// A target dose status that indicates a vaccine dose administered has met the goals of the target dose
	/// </summary>
	Satisfied,
	/// <summary>
	/// A target dose status that indicates no vaccine dose administered has met the goals of the target dose. 
	/// Due to the patient's age and/or interval from a previous dose, the target dose does not need to be satisfied
	/// </summary>
	Skipped
}

public enum PatientSeriesStatus
{
	/// <summary>
	/// A patient series status that indicates the patient exceeded the maximum age prior to completing the patient series 
	/// </summary>
	Aged_Out,
	/// <summary>
	/// A patient series status that indicates the patient has met all of the ACIP recommendations for the patient series 
	/// </summary>
	Complete,
	/// <summary>
	/// A patient series status that indicates no further vaccines should be administered at this time for the patient series
	/// </summary>
	Contraindicated,
	/// <summary>
	/// A patient series status that indicates the patient has evidence of immunity indicating no 
	/// further vaccines are needed for the patient series 
	/// </summary>
	Immune,
	/// <summary>
	/// A patient series status that indicates the patient has not yet met all of the ACIP recommendations for the patient series
	/// </summary>
	Not_Complete,
	/// <summary>
	/// A patient series status that indicates the patient's immunization history provides 
	/// sufficient protection against a target disease and there's no recommended action at this time
	/// </summary>
	Not_Recommended
}