namespace AdventOfCode2025;

record TestData(string input, string expectedP1, string expectedP2);

static class TestInputs
{
	static TestData[] sTestInputs =
	{
		new TestData("""
			L68
			L30
			R48
			L5
			R60
			L55
			L1
			L99
			R14
			L82
			""", 
			expectedP1: "3",
			expectedP2: "6"),
	};

	public static TestData GetTest(int day)
	{
		return sTestInputs[day-1];
	}
}
