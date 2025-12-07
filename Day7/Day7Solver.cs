using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2025;

internal class Day7Solver : ISolver
{
	(Grid<char> next, int numSplits) GetNextBeamStep(Grid<char> start, int y)
	{
		Grid<char> next = start.CreateCopy();

		int numSplits = 0;
		for(int x = 0; x < next.GetWidth(); x++)
		{
			if (next.Get(x, y) == '|')
			{
				char below = next.Get(x, y + 1);
				if (below == '.')//Case 1: Value below is empty
				{
					// Propagate wave
					next.Set(x, y + 1, '|');
				}
				else if(below == '|') // Case 2: Do nothing wave is already propageted
				{
					// Nothing...
				}
				else if (below == '^') // Case 3: Split beam
				{
					next.Set(x - 1, y + 1, '|');
					next.Set(x + 1, y + 1, '|');
					numSplits += 1;
				}
			}
		}

		return (next, numSplits);
	}

	public string SolvePart1(string input)
	{
		Grid<char> space = Grid<char>.ParseToChars(input, '.');
		(int, int)? startIt = space.FindFirst('S');
		if(startIt == null)
		{
			throw new Exception("Can't find start pos");
		}

		(int, int) start = startIt.Value;
		space.Set(start, '|');

		int totalSplits = 0;
		for(int y = 0; y < space.GetHeight()-1; y++)
		{
			(Grid<char> next, int numSplits) = GetNextBeamStep(space, y);
			totalSplits += numSplits;
			space = next;
		}

		return totalSplits.ToString();
	}

	public string SolvePart2(string input)
	{
		Grid<char> space = Grid<char>.ParseToChars(input, '.');

		// Similar to above be we keep track of possiblities of each beam instead of if the beam is there.
		Grid<long> possSpace = new(new long[space.GetWidth(), space.GetHeight()], 0);
		foreach ((int x, int y) in space.ColsRows())
		{
			char c = space.Get(x, y);
			long value = 0;
			switch (c)
			{
				case '.':
					value = 0;
					break;
				case 'S':
					value = 1; // Start value has 1 possibility
					break;
				case '^':
					value = -1; // represent splitters as -1
					break;
				default:
					throw new Exception($"Invalid input, cannot parse char |{c}|");
			}

			possSpace.Set(x, y, value);
		}

		// Propagate beams
		for (int y = 0; y < possSpace.GetHeight()-1; y++)
		{
			for (int x = 0; x < possSpace.GetWidth(); x++)
			{
				long curr = possSpace.Get(x, y);
				if (curr > 0) // Beam is here
				{
					long below = possSpace.Get(x, y + 1);
					if (below == -1) // Splitter
					{
						long left = possSpace.Get(x - 1, y + 1);
						long right = possSpace.Get(x + 1, y + 1);

						possSpace.Set(x - 1, y + 1, left + curr);
						possSpace.Set(x + 1, y + 1, right + curr);
					}
					else // Usual case
					{
						possSpace.Set(x, y + 1, below + curr);
					}
				}
			}
		}

		// Count amounts at bottom row
		long total = 0;
		for (int x = 0; x < possSpace.GetWidth(); x++)
		{
			long bot = possSpace.Get(x, possSpace.GetHeight() - 1);
			Debug.Assert(bot >= 0, "Counting negative number of possibilities?");
			total += bot;
		}

		return total.ToString();
	}
}
