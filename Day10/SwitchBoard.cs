namespace AdventOfCode2025;

class SwitchBoard
{
	static long[] sButtonScratchPad = new long[32];

	int mDimension;
	List<long[]> mButtons;
	long[] mRequiredConfig;
	long[] mJoltRequirements;

	public SwitchBoard(string inputLine)
	{
		// Ex: [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}

		int configBeginIdx = inputLine.IndexOf('[');
		int configEndIdx = inputLine.IndexOf(']');

		int joltBeginIdx = inputLine.IndexOf('{');
		int joltEndIdx = inputLine.IndexOf('}');

		int buttonsBeginIdx = configEndIdx + 1;
		int buttonsEndIdx = joltBeginIdx - 1;

		// Get strings chop off ends
		string configStr = inputLine.Substring(configBeginIdx + 1, configEndIdx - configBeginIdx - 1);
		string buttonsStr = inputLine.Substring(buttonsBeginIdx + 1, buttonsEndIdx - buttonsBeginIdx);
		string joltsStr = inputLine.Substring(joltBeginIdx+1, joltEndIdx - joltBeginIdx - 1);

		mDimension = configStr.Length;

		// Construct req config
		mRequiredConfig = new long[mDimension];
		for(int c = 0; c < configStr.Length; c++)
		{
			switch (configStr[c])
			{
				case '.':
					mRequiredConfig[c] = 0;
					break;
				case '#':
					mRequiredConfig[c] = 1;
					break;
				default:
					break;
			}
		}

		// Construct buttons
		mButtons = new List<long[]>();
		string[] buttonStrs = buttonsStr.Split(')');
		for(int b = 0; b < buttonStrs.Length - 1; b++)
		{
			string[] numberStrs = buttonStrs[b].Split(",");
			long[] buttonVec = new long[mDimension];

			for (int n = 0; n < numberStrs.Length; n++)
			{
				string numberStr = numberStrs[n].Replace('(', '0'); // Hack to remove opening bracket.
				int number = int.Parse(numberStr);
				buttonVec[number] = 1;
			}

			mButtons.Add(buttonVec);
		}

		// Construct jolts
		string[] joltNumStrs = joltsStr.Split(",");
		mJoltRequirements = new long[joltNumStrs.Length]; 
		for(int j = 0; j < joltNumStrs.Length; j++)
		{
			mJoltRequirements[j] = long.Parse(joltNumStrs[j]);
		}
	}

	public long FindMinButtonPressesToSolve()
	{
		long[] buttonStates = new long[mButtons.Count];
		for(int i = 0; i < buttonStates.Length; i++)
		{
			buttonStates[i] = -1;
		}
		return FindMinButtonPressesRec(0, buttonStates, long.MaxValue);
	}

	long FindMinButtonPressesRec(int depth, long[] buttonStates, long bestSoFar)
	{
		long buttonPresses = CountButtonPresses(buttonStates);
		if(buttonPresses >= bestSoFar)
		{
			return long.MaxValue; // We have found better than this already.
		}

		if(ButtonStatesDoesSolve(buttonStates))
		{
			return buttonPresses; // This is a solution, push up tree
		}
		else if(depth >= buttonStates.Length)
		{
			return long.MaxValue; // Not a solution but also complete, give up.
		}

		buttonStates[depth] = 0; // Explore non-press branches first
		long branch1 = FindMinButtonPressesRec(depth + 1, buttonStates, bestSoFar);
		bestSoFar = Math.Min(branch1, bestSoFar);

		buttonStates[depth] = 1;
		long branch2 = FindMinButtonPressesRec(depth + 1, buttonStates, bestSoFar);

		// Reset state
		buttonStates[depth] = -1;

		return Math.Min(branch1, branch2);
	}

	bool ButtonStatesDoesSolve(long[] buttonStates)
	{
		Array.Clear(sButtonScratchPad);
		Debug.Assert(buttonStates.Length == mButtons.Count, "Button states mismatch");	
		for (int i = 0; i < buttonStates.Length; i++)
		{
			long coef = buttonStates[i];
			if (coef <= 0) // -1 is unknown
				continue;

			long[] button = mButtons[i];
			for (int d = 0; d < mDimension; d++)
			{
				sButtonScratchPad[d] += button[d] * coef;
			}
		}

		for(int i = 0; i < mDimension; i++)
		{
			if (sButtonScratchPad[i] % 2 != mRequiredConfig[i])
			{
				return false;
			}
		}


		return true;
	}

	long CountButtonPresses(long[] buttonStates)
	{
		long sum = 0;
		for (int i = 0; i < buttonStates.Length; i++)
		{
			if (buttonStates[i] >= 0) sum += buttonStates[i];
		}
		return sum;
	}
}
