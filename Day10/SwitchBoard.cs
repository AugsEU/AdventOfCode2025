namespace AdventOfCode2025;

class SwitchBoard
{
	#region rMember

	static long[] sButtonScratchPad = new long[32];

	int mDimension;
	List<long[]> mButtons;
	long[] mRequiredConfig;
	long[] mJoltRequirements;

	#endregion rMember



	#region rInit

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

		// Sort buttons by most populated. This means we explore the most forcing branches first.
		mButtons.Sort((v1, v2) => (v2.Sum().CompareTo(v1.Sum())));

		// Construct jolts
		string[] joltNumStrs = joltsStr.Split(",");
		mJoltRequirements = new long[joltNumStrs.Length]; 
		for(int j = 0; j < joltNumStrs.Length; j++)
		{
			mJoltRequirements[j] = long.Parse(joltNumStrs[j]);
		}

		Debug.Assert(mJoltRequirements.Length == mDimension, "Jolts don't match dimensions");
	}


	#endregion rInit


	#region rPart1

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

	#endregion rPart1


	#region rPart2

	public long FindMinJoltPresses()
	{
		Array.Clear(sButtonScratchPad);
		long[] buttonStates = new long[mButtons.Count];
		for (int i = 0; i < buttonStates.Length; i++)
		{
			buttonStates[i] = -1;
		}
		return FindMinJoltPressesRec(0, buttonStates, long.MaxValue);
	}

	long FindMinJoltPressesRec(int btnIdx, long[] buttonStates, long bestSoFar)
	{
		int origBtnIdx = btnIdx;
		long buttonPresses = CountButtonPresses(buttonStates);
		if (buttonPresses >= bestSoFar)
		{
			return long.MaxValue; // We have found better than this already.
		}

		bool checkSoln = ButtonStatesDoesSolveJolt(buttonStates); // Stores result in sButtonScratchPad
		if (checkSoln)
		{
			ClearBtnStateAfter(buttonStates, origBtnIdx);
			return buttonPresses; // This is a solution, push up tree
		}
		else if (btnIdx >= buttonStates.Length)
		{
			ClearBtnStateAfter(buttonStates, origBtnIdx);
			return long.MaxValue; // Not a solution but also complete, give up.
		}

		// See which components can be evaluated algebraically
		(bool anyForced, bool anyInvalid) = EvaluateForcedJoltStates(buttonStates);
		if(anyInvalid)
		{
			ClearBtnStateAfter(buttonStates, origBtnIdx);
			return long.MaxValue;
		}
		if (anyForced)
		{
			buttonPresses = CountButtonPresses(buttonStates);
			checkSoln = ButtonStatesDoesSolveJolt(buttonStates); // Re-evaluate solution now that we have forced states.

			if (checkSoln)
			{
				ClearBtnStateAfter(buttonStates, origBtnIdx);
				return buttonPresses;
			}

			// Cut branch if one value is too big. They can't get smaller.
			for(int d = 0; d < mDimension; d++)
			{
				if (sButtonScratchPad[d] > mJoltRequirements[d])
				{
					ClearBtnStateAfter(buttonStates, origBtnIdx);
					return long.MaxValue;
				}
			}

			// Find next unknown button since btnIdx may have been found.
			btnIdx = FindNextUnknownButton(buttonStates, btnIdx);

			if (btnIdx >= buttonStates.Length)
			{
				// No unknowns... but also not a solution.
				ClearBtnStateAfter(buttonStates, origBtnIdx);
				return long.MaxValue;
			}
		}

		// Find the most we can multiply this unknown component by
		long[] buttonArr = mButtons[btnIdx];
		long maxMultiplier = long.MaxValue;
		for(int i = 0; i < mDimension; i++)
		{
			if (buttonArr[i] == 0)
				continue;

			long currLevel = sButtonScratchPad[i];
			long coordMax = mJoltRequirements[i] - currLevel + 1;

			maxMultiplier = Math.Min(coordMax, maxMultiplier);
		}
		
		// Recursive search
		long bestSoln = long.MaxValue;
		for(long c = maxMultiplier; c >= 0; c--)
		{
			buttonStates[btnIdx] = c;
			long soln = FindMinJoltPressesRec(FindNextUnknownButton(buttonStates, btnIdx + 1), buttonStates, bestSoFar);
			bestSoln = Math.Min(soln, bestSoln);
			bestSoFar = Math.Min(soln, bestSoFar);
		}
		// Reset state
		ClearBtnStateAfter(buttonStates, origBtnIdx);
		return bestSoln;
	}

	bool ButtonStatesDoesSolveJolt(long[] buttonStates)
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

		for (int i = 0; i < mDimension; i++)
		{
			if (sButtonScratchPad[i] != mJoltRequirements[i])
			{
				return false;
			}
		}

		return true;
	}


	(bool anyForced, bool anyInvalid) EvaluateForcedJoltStates(long[] buttonStates)
	{
		bool anyFound = false;
		bool search = true;

		while (search)
		{
			search = false;
			for (int d = 0; d < mDimension; d++)
			{
				long numUnknowns = 0;
				int unknownBtnIdx = -1;
				for (int b = 0; b < buttonStates.Length; b++)
				{
					if (buttonStates[b] == -1 && mButtons[b][d] != 0)
					{
						numUnknowns += 1;
						unknownBtnIdx = b;
					}
				}

				if (numUnknowns == 1)
				{
					long currSumInD = 0;
					for (int b = 0; b < buttonStates.Length; b++)
					{
						if (buttonStates[b] <= 0)
							continue;

						currSumInD += mButtons[b][d] * buttonStates[b];
					}

					long forcedValue = mJoltRequirements[d] - currSumInD;
					if(forcedValue < 0)
					{
						return (true, true);
					}
					buttonStates[unknownBtnIdx] = forcedValue;
					search = true;// Found new value keep looking
					anyFound = true;
					break;
				}
			}
		}

		return (anyFound, false);
	}

	#endregion rPart2





	#region rUtil

	long CountButtonPresses(long[] buttonStates)
	{
		long sum = 0;
		for (int i = 0; i < buttonStates.Length; i++)
		{
			if (buttonStates[i] >= 0) sum += buttonStates[i];
		}
		return sum;
	}

	int FindNextUnknownButton(long[] buttonStates, int idx)
	{
		for (; idx < buttonStates.Length; idx++)
		{
			if (buttonStates[idx] == -1)
			{
				break;
			}
		}

		return idx;
	}


	static void ClearBtnStateAfter(long[] buttonStates, int idx)
	{
		for(int i = idx; i < buttonStates.Length; i++)
		{
			buttonStates[i] = -1;
		}
	}

	#endregion rUtil
}
