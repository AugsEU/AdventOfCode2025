namespace AdventOfCode2025;

/// <summary>
/// Functions for parsing the input.
/// </summary>
static class InputParser
{
	public static List<string> GetNonEmptyLines(string input)
	{
		string[] lines = input.Split('\n');
		List<string> result = new();

		foreach (string line in lines)
		{
			string newLine = line.Trim();
			if (newLine.Length != 0)
			{
				result.Add(line.Trim());
			}
		}

		return result;
	}
}
