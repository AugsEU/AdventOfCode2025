namespace AdventOfCode2025;

internal class Day10Solver : ISolver
{


	public string SolvePart1(string input)
	{
		long sum = 0;
		foreach(string line in InputParser.GetNonEmptyLines(input))
		{
			SwitchBoard sb = new SwitchBoard(line);
			sum += sb.FindMinButtonPressesToSolve();
		}

		return sum.ToString();
	}

	public string SolvePart2(string input)
	{
		throw new NotImplementedException();
	}
}
