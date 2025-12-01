namespace AdventOfCode2025;

/// <summary>
/// Solver for the first day.
/// </summary>
internal class Day1Solver : ISolver
{
	#region rConstants

	const int NUM_DIAL_POSITIONS = 100;

	#endregion rConstants





	#region rMembers

	int mDialPosition = 50;

	#endregion rMembers




	#region rSolverMain

	/// <summary>
	/// Solve day 1 part 1
	/// </summary>
	public string SolvePart1(string input)
	{
		List<string> lines = InputParser.GetNonEmptyLines(input);
		List<int> moves = ParseDialMovements(lines);

		Console.WriteLine($"Start at {mDialPosition} with {moves.Count} moves");
		int numZeroes = 0;
		foreach(int move in moves)
		{
			int prevDial = mDialPosition;
			ApplyMovement(move);

			if (mDialPosition == 0)
			{
				Console.WriteLine($"Dial {prevDial} -> {mDialPosition} from move {move}");
				numZeroes++;
			}
		}

		return numZeroes.ToString();
	}



	/// <summary>
	/// Solve day 1 part 2
	/// </summary>
	public string SolvePart2(string input)
	{
		throw new NotImplementedException();
	}

	#endregion rSolverMain





	#region rUtil

	/// <summary>
	/// Parse the dial movements
	/// </summary>
	private List<int> ParseDialMovements(List<string> list)
	{
		List<int> result = new();
		foreach (string line in list)
		{
			string sanLine = line.Trim().ToLower();

			char dirChar = sanLine[0];
			string moveStr = sanLine.Substring(1);
			int moveNum = int.Parse(moveStr); // not safe on different languages..

			result.Add(dirChar == 'r' ? moveNum : -moveNum);
		}

		return result;
	}




	/// <summary>
	/// Apply a dial movement to the current dial position
	/// </summary>
	private void ApplyMovement(int move)
	{
		mDialPosition += move;

		// Rotate under 0
		while (mDialPosition < 0) mDialPosition += NUM_DIAL_POSITIONS;

		// Rotate above 99
		while (mDialPosition >= NUM_DIAL_POSITIONS) mDialPosition -= NUM_DIAL_POSITIONS;

		if(mDialPosition < 0 || mDialPosition >= NUM_DIAL_POSITIONS)
		{
			throw new Exception("Dial out of bounds...");
		}
	}

	#endregion rUtil
}
