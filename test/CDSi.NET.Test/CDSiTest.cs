using CDSi.NET.Test.Models;

namespace CDSi.NET.Test;

public class CDSiTest
{
	[Theory]
	[MemberData(nameof(TestDataLoader.LoadHealthyData), MemberType = typeof(TestDataLoader))]
	internal void HealthyTest(HealthyTest test)
	{
		CDSiEngine.Initialize();
		CDSiEngine.EvaluateSeries(new NET.Models.EvaluationRequest
		{
			Patient = new NET.Models.Patient
			{
				DOB = new DateTime(1998, 02, 08),
				Gender = NET.Models.Gender.Male,
			}
		});
		Assert.Equal(test.TestId, test.TestId);
	}

	[Theory]
	[MemberData(nameof(TestDataLoader.LoadConditionData), MemberType = typeof(TestDataLoader))]
	internal void ConditionTest(ConditionTest test)
	{
		Assert.Equal(test.TestId, test.TestId);
	}
}