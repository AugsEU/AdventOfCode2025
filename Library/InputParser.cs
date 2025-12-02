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

	public static List<string> GetCommaSeparated(string input)
	{
		// really slow but who cares
		List<string> result = new();
		foreach (string line in GetNonEmptyLines(input))
		{
			string[] sep = line.Split(',');
			foreach (string strItem in sep)
			{
				string sanStrItem = strItem.Trim();
				if(sanStrItem.Length != 0)
				{
					result.Add(sanStrItem);
				}
			}
		}

		return result;
	}

	public static (Int64, Int64) ParseRange(string rangeStr)
	{
		string[] ranges = rangeStr.Split('-');

		if (ranges.Length != 2) throw new Exception($"Invalid range {rangeStr}");

		return (Int64.Parse(ranges[0]), Int64.Parse(ranges[1]));
	}
}
