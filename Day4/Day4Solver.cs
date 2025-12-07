using System.Numerics;

namespace AdventOfCode2025;

internal class Day4Solver : ISolver
{
	public bool CanBeAccessed(Grid<char> grid, int x, int y, int maxAdj)
	{
		int boxCount = 0;
		foreach (char adjChar in grid.GetAdjacent(x, y))
		{
			if (adjChar == '@')
				boxCount++;

			if (boxCount >= maxAdj)
			{
				return false;
			}
		}

		return true;
	}

	public string SolvePart1(string input)
	{
		Grid<char> grid = Grid<char>.ParseToChars(input);
		int total = 0;
		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				if(grid.Get(x,y) == '@' && CanBeAccessed(grid, x, y, 4))
				{
					total++;
				}
			}
		}

		return total.ToString();
	}

	public string SolvePart2(string input)
	{
		Grid<char> grid = Grid<char>.ParseToChars(input);

		// Unga bunga: just repeat part 1 until done
		int total = 0;
		List<(int, int)> toRemove = new();
		do
		{
			toRemove.Clear();

			for (int x = 0; x < grid.GetWidth(); x++)
			{
				for (int y = 0; y < grid.GetHeight(); y++)
				{
					if (grid.Get(x, y) == '@' && CanBeAccessed(grid, x, y, 4))
					{
						toRemove.Add((x, y));
					}
				}
			}

			foreach ((int x, int y) in toRemove)
			{
				grid.Set(x, y, '.');
				total++;
			}

		} while (toRemove.Count > 0);

		return total.ToString();
	}
}
