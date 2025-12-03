namespace AdventOfCode2025;

internal class Day3Solver : ISolver
{
	#region rMembers

	#endregion rMembers


	#region rUtil

	int FindBestDigitIndex(int startIdx, int numDigitsInNumber, string bank)
	{
		int bestCharIdx = startIdx;
		char bestChar = bank[startIdx];
		for(int i = startIdx+1; i <= bank.Length - numDigitsInNumber; i++)
		{
			char thisChar = bank[i];
			if(thisChar > bestChar)
			{
				bestChar = thisChar;
				bestCharIdx = i;
			}
		}

		return bestCharIdx;
	}

	long FindNDigitBank(string bank, int numDigits)
	{
		List<char> bestDigits = new List<char>();
		int currSearchHead = 0;

		for (int d = 0; d < numDigits; d++)
		{
			int bestDigitIdx = FindBestDigitIndex(currSearchHead, numDigits - d, bank);
			currSearchHead = bestDigitIdx + 1;
			bestDigits.Add(bank[bestDigitIdx]);
		}

		string fullNumStr = String.Concat(bestDigits);
		return long.Parse(fullNumStr);
	}

	#endregion rUtil




	#region rSolver

	public string SolvePart1(string input)
	{
		const int NUM_DIGITS_TO_FIND = 2;
		List<string> banks = InputParser.GetNonEmptyLines(input);

		long sum = 0;
		foreach(string bank in banks)
		{
			long maxVolts = FindNDigitBank(bank, NUM_DIGITS_TO_FIND);

			sum += maxVolts;
		}

		return sum.ToString();
	}

	public string SolvePart2(string input)
	{
		const int NUM_DIGITS_TO_FIND = 12;
		List<string> banks = InputParser.GetNonEmptyLines(input);

		long sum = 0;
		foreach (string bank in banks)
		{
			long maxVolts = FindNDigitBank(bank, NUM_DIGITS_TO_FIND);

			sum += maxVolts;
		}

		return sum.ToString();
	}

	#endregion rSolver




}
