namespace AdventOfCode2025;

/// <summary>
/// Solver for the first day.
/// </summary>
internal class Day1Solver : ISolver
{
	#region rTypes

	/// <summary>
	/// Represents a movement of the dial.
	/// </summary>
	struct DialMove(int amount, bool isRight)
	{
		public int mMovement = isRight ? amount : -amount;
	}

	#endregion rTypes





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
		List<DialMove> moves = ParseDialMovements(lines);

		Console.WriteLine($"Start at {mDialPosition} with {moves.Count} moves");
		int numZeroes = 0;
		foreach(DialMove move in moves)
		{
			ApplyMovement(move);
			//

			if (mDialPosition == 0)
			{
				Console.WriteLine($"Dial {mDialPosition} from move {move.mMovement}");
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
	private List<DialMove> ParseDialMovements(List<string> list)
	{
		List<DialMove> result = new();
		foreach (string line in list)
		{
			char dirChar = line[0];
			string moveStr = line.Substring(1);
			int moveNum = int.Parse(moveStr); // not safe on different languages..

			result.Add(new DialMove(moveNum, isRight: dirChar == 'R'));
		}

		return result;
	}




	/// <summary>
	/// Apply a dial movement to the current dial position
	/// </summary>
	private void ApplyMovement(DialMove move)
	{
		mDialPosition += move.mMovement;

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
