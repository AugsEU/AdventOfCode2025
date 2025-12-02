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
		new TestData("""
			11-22,95-115,998-1012,1188511880-1188511890,222220-222224,
			1698522-1698528,446443-446449,38593856-38593862,565653-565659,
			824824821-824824827,2121212118-2121212124
			""",
			expectedP1: "1227775554",
			expectedP2: "4174379265"),
	};

	public static TestData GetTest(int day)
	{
		return sTestInputs[day-1];
	}
}
