namespace AdventOfCode2025;

internal class Day6Solver : ISolver
{
	enum Operator
	{
		Add,
		Multiply
	}

	struct MathsProblem(List<long> nums, Operator op)
	{
		public List<long> mNums = nums;
		public Operator mOp = op;

		public long Eval()
		{
			long result = mNums[0];
			for(int i = 1; i < mNums.Count; i++)
			{
				long nextNum = mNums[i];
				switch (mOp)
				{
					case Operator.Add:
						result += nextNum;
						break;
					case Operator.Multiply:
						result *= nextNum;
						break;
					default:
						break;
				}
			}

			return result;
		}
	}

	List<MathsProblem> mProblems = new();

	void ParseProblems(string input)
	{
		string[] lines = input.Split('\n');
		string opLine = lines[lines.Length - 1];

		// Verify lengths
		for (int l = 0; l < lines.Length - 1; l++)
		{
			Debug.Assert(lines[l].Length == opLine.Length, "Lengths of lines do not match");
		}

		int readHead = 0;
		while (readHead < opLine.Length)
		{
			Operator op;
			char opChar = (char)opLine[readHead];
			if (opChar == '+') op = Operator.Add;
			else if (opChar == '*') op = Operator.Multiply;
			else throw new Exception($"Read head does not recognise char |{opChar}|");

			// Scan forwards to find the length of this line.
			int nextReadHead;
			for(nextReadHead = readHead + 1; nextReadHead <= opLine.Length; nextReadHead++)
			{
				if (nextReadHead == opLine.Length || opLine[nextReadHead] != ' ')
					break;
			}

			int probLen = nextReadHead - readHead;
			
			List<long> numbs = new();
			for(int l = 0; l < lines.Length - 1; l++)
			{
				string numStr = lines[l].Substring(readHead, probLen).Trim();
				long num = long.Parse(numStr);

				Debug.Assert(num < int.MaxValue, "Number too big to parse");
				numbs.Add(long.Parse(numStr));
			}

			mProblems.Add(new MathsProblem(numbs, op));
			readHead= nextReadHead;
		}
	}

	void ParseProblemsCephalopod(string input)
	{
		string[] lines = input.Split('\n');
		string opLine = lines[lines.Length - 1];

		// Verify lengths
		for (int l = 0; l < lines.Length - 1; l++)
		{
			Debug.Assert(lines[l].Length == opLine.Length, "Lengths of lines do not match");
		}

		List<string> columnStrings = new();

		int readHead = 0;
		while (readHead < opLine.Length)
		{
			Operator op;
			char opChar = (char)opLine[readHead];
			if (opChar == '+') op = Operator.Add;
			else if (opChar == '*') op = Operator.Multiply;
			else throw new Exception($"Read head does not recognise char |{opChar}|");

			// Scan forwards to find the length of this line.
			int nextReadHead;
			for (nextReadHead = readHead + 1; nextReadHead <= opLine.Length; nextReadHead++)
			{
				if (nextReadHead == opLine.Length || opLine[nextReadHead] != ' ')
					break;
			}

			int probLen = nextReadHead - readHead;

			columnStrings.Clear();

			List<long> numbs = new();
			for (int l = 0; l < lines.Length - 1; l++)
			{
				string numStr = lines[l].Substring(readHead, probLen);
				for(int c = 0; c < numStr.Length; c++)
				{
					if(l == 0)
					{
						columnStrings.Add("");
					}
					char digit = numStr[c];
					if(digit >= '0' && digit <= '9')
					{
						columnStrings[c] += digit;
					}
				}
			}

			foreach(string col in columnStrings)
			{
				if (col.Length == 0) continue;

				numbs.Add(long.Parse(col.Trim()));
			}

			mProblems.Add(new MathsProblem(numbs, op));
			readHead = nextReadHead;
		}
	}

	public string SolvePart1(string input)
	{
		ParseProblems(input);
		long sum = 0;

		for(int i = 0; i < mProblems.Count; i++)
		{
			sum += mProblems[i].Eval();
		}

		return sum.ToString();
	}

	public string SolvePart2(string input)
	{
		ParseProblemsCephalopod(input);
		long sum = 0;

		for (int i = 0; i < mProblems.Count; i++)
		{
			sum += mProblems[i].Eval();
		}

		return sum.ToString();
	}
}
