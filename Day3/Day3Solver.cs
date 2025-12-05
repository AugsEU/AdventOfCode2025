namespace AdventOfCode2025;

internal class Day3Solver : ISolver
{
	#region rMembers

	List<char> mBestDigitsCache = new List<char>();

	#endregion rMembers


	#region rUtil

	int FindBestDigitIndex(int startIdx, int numDigitsInNumber, string bank)
	{
		int bestCharIdx = startIdx;
		char bestChar = bank[startIdx];
		for(int i = startIdx+1; i <= bank.Length - numDigitsInNumber; i++)
		{
			char thisChar = bank[i];
			if(thisChar == '9')
			{
				return i;
			}
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
		int currSearchHead = 0;
		mBestDigitsCache.Clear();
		for (int d = 0; d < numDigits; d++)
		{
			int bestDigitIdx = FindBestDigitIndex(currSearchHead, numDigits - d, bank);
			currSearchHead = bestDigitIdx + 1;
			mBestDigitsCache.Add(bank[bestDigitIdx]);
		}

		return ParseDigitCache();
	}

	long ParseDigitCache()
	{
		long res = 0;
		for(int i = 0; i < mBestDigitsCache.Count; i++)
		{
			res += (long)(mBestDigitsCache[i] - '0');
			res *= 10;
		}
		return res/10;
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
