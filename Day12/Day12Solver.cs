namespace AdventOfCode2025;

internal class Day12Solver : ISolver
{
	List<Chair> mChairs = new List<Chair>();
	List<FurnitureProblem> mProblems = new List<FurnitureProblem>();
	HashSet<FurnitureProblem> mImpossibleProblems = new HashSet<FurnitureProblem>();

	void ParseInput(string input)
	{
		string[] sections = input.Split("\n\n");
		foreach(string section in sections)
		{
			if(section.Contains("x"))
			{
				foreach(string problemStr in section.Split('\n'))
				{
					string[] dimNumbs = problemStr.Split(':');
					Debug.Assert(dimNumbs.Length == 2, "Invalid furniture problem");

					string[] dimensions = dimNumbs[0].Split("x");
					Debug.Assert(dimensions.Length == 2, "Invalid dimensions");
					int width = int.Parse(dimensions[0]);
					int height = int.Parse(dimensions[1]);

					string[] numbers = dimNumbs[1].Trim().Split(' ');
					long[] chairCounts = new long[numbers.Length];
					for(int i = 0; i < chairCounts.Length; i++)
					{
						chairCounts[i] = long.Parse(numbers[i]);
					}

					mProblems.Add(new FurnitureProblem(chairCounts, width, height));
				}
			}
			else
			{
				mChairs.Add(new Chair(section.Trim()));
			}
		}
	}

	bool CanSolveFurnitureProblem(FurnitureProblem problem)
	{
		//if (mImpossibleProblems.Contains(problem))
		//{
		//	return false;
		//}

		// Find next chair to place:
		int chairIdx = -1;
		for(int i = 0; i < problem.mChairCounts.Length; i++)
		{
			if(problem.mChairCounts[i] > 0)
			{
				chairIdx = i;
				break;
			}
		}

		if(chairIdx == -1)
		{
			// No more chairs to place.
			return true;
		}

		int totalChairDotsToPlace = 0;
		for(int i = 0; i < problem.mChairCounts.Length; i++)
		{
			totalChairDotsToPlace += (int)problem.mChairCounts[i] * mChairs[i].GetNumDots();
		}

		Chair chairToPlace = mChairs[chairIdx];
		int chairSize = chairToPlace.GetSize();

		problem.mChairCounts[chairIdx] -= 1;
		FurnitureProblem nextProblem = problem.CreateCopy();
		problem.mChairCounts[chairIdx] += 1;

		// First find impossible squares.
		nextProblem.mSpace.FindReplace('.', 'S'); // Scrub
		bool anyChairImpossible = false;
		for (int c = 0; c < mChairs.Count; c++)
		{
			Chair scrubChair = mChairs[c];
			if (problem.mChairCounts[c] == 0)
			{
				continue;
			}

			bool chairImpossible = true;
			for (int x = 0; x <= nextProblem.mSpace.GetWidth() - chairSize; x++)
			{
				for (int y = 0; y <= nextProblem.mSpace.GetHeight() - chairSize; y++)
				{
					for (int o = 0; o < Chair.NUM_ORIENTATIONS; o++)
					{
						// Placing and removing a chair will scrub any S away
						if (nextProblem.mSpace.TryPlaceChair(scrubChair, x, y, o))
						{
							chairImpossible = false;
							nextProblem.mSpace.RemoveChair(scrubChair, x, y, o);
						}
					}
				}
			}

			if(chairImpossible)
			{
				anyChairImpossible = true;
				break;
			}
		}
		nextProblem.mSpace.FindReplace('S', '#'); // Any S still remaining must be inaccessible.

		if (nextProblem.mSpace.GetNumFreeSpaces() >= totalChairDotsToPlace && !anyChairImpossible)
		{
			for (int x = 0; x <= nextProblem.mSpace.GetWidth() - chairSize; x++)
			{
				for (int y = 0; y <= nextProblem.mSpace.GetHeight() - chairSize; y++)
				{
					for (int o = 0; o < Chair.NUM_ORIENTATIONS; o++)
					{
						if (nextProblem.mSpace.TryPlaceChair(chairToPlace, x, y, o))
						{
							bool canSolve = CanSolveFurnitureProblem(nextProblem);
							nextProblem.mSpace.RemoveChair(chairToPlace, x, y, o);
							if (canSolve)
							{
								return true;
							}
						}
					}
				}
			}
		}

		
		//mImpossibleProblems.Add(problem.CreateCopy());
		return false;
	}

	public string SolvePart1(string input)
	{
		/*E.g.
		 
		0:
		###
		##.
		##.

		1:
		###
		##.
		.##

		4x4: 0 0 0 0 2 0
		12x5: 1 0 1 0 2 2*/

		ParseInput(input);

		int numSolvable = 0;
		for(int i = 0; i < mProblems.Count; i++)
		{
			mImpossibleProblems.Clear();
			Console.WriteLine($"Solving problem {i}/{mProblems.Count}");
			if (CanSolveFurnitureProblem(mProblems[i]))
			{
				Console.WriteLine($"    Solved");
				numSolvable++;
			}
			else
			{
				Console.WriteLine($"    Not possible");
			}
		}

		return numSolvable.ToString();
	}

	public string SolvePart2(string input)
	{
		throw new NotImplementedException();
	}
}
