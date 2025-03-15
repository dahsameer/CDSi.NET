using CDSi.NET.Test.Models;

namespace CDSi.NET.Test;

public class MainTest
{

	[Theory]
	[MemberData(nameof(TestDataLoader.LoadHealthyData), MemberType = typeof(TestDataLoader))]
	internal void HealthyTest(HealthyTest test)
	{
		Assert.Equal(test.TestId, test.TestId);
	}

	[Theory]
	[MemberData(nameof(TestDataLoader.LoadConditionData), MemberType = typeof(TestDataLoader))]
	internal void ConditionTest(ConditionTest test)
	{
		Assert.Equal(test.TestId, test.TestId);
	}
}