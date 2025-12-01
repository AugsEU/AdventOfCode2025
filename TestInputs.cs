namespace AdventOfCode2025;

record TestData(string input, string expected);

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
			expected: "3"),
	};

	public static TestData GetTest(int day, int part)
	{
		return sTestInputs[2*(day-1) + part-1];
	}
}
