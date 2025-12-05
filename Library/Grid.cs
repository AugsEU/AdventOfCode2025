namespace AdventOfCode2025;

internal class Grid<T> where T : struct
{
	T[,] mValues;
	T mOobDefault;

	public Grid(T[,] values, T oobDefault)
	{
		mValues = values;
		mOobDefault = oobDefault;
	}

	public static Grid<char>ParseToChars(string input, char oobDefault = '.')
	{
		int width = -1;
		List<string> lines = InputParser.GetNonEmptyLines(input);
		int height = lines.Count;

		// Find width and check it's consistent.
		for (int i = 0; i < height; i++)
		{
			if (width == -1)
			{
				width = lines[i].Length;
			}

			Debug.Assert(width == lines[i].Length, "Mixed length grid");
		}

		char[,] chars = new char[width, height];
		for (int y = 0; y != lines.Count; y++)
		{
			string row = lines[y];
			for(int x = 0; x < row.Length; x++)
			{
				chars[x, y] = lines[y][x];
			}
		}

		return new Grid<char>(chars, oobDefault);
	}

	public static Grid<int> ParseToInt(string input, int oobDefault = 0)
	{
		Grid<char> charGrid = ParseToChars(input, '0');
		int w = charGrid.GetWidth();
		int h = charGrid.GetHeight();
		int[,] ints = new int[w, h];
		for(int x = 0; x < w; x++)
		{
			for(int y = 0; y < h; y++)
			{
				ints[x, y] = charGrid.GetAt(x, y) - '0';
			}
		}

		return new Grid<int>(ints, oobDefault);
	}

	public int GetWidth()
	{
		return mValues.GetLength(0);
	}

	public int GetHeight()
	{
		return mValues.GetLength(1);
	}

	public T GetAt(int x, int y)
	{
		if(x < 0 || x >= GetWidth())
		{
			return mOobDefault;
		}
		if (y < 0 || y >= GetWidth())
		{
			return mOobDefault;
		}

		return mValues[x, y];
	}

	public T GetAt((int, int) coords)
	{
		return GetAt(coords.Item1, coords.Item2);
	}

	public void Set(int x, int y, T value)
	{
		mValues[x, y] = value;
	}

	public void Set((int, int) coords, T value)
	{
		mValues[coords.Item1, coords.Item2] = value;
	}

	public override string ToString()
	{
		string gridStr = "";

		// Really bad but just for debug.
		for (int y = 0; y < GetHeight(); y++)
		{
			for (int x = 0; x < GetWidth(); x++)
			{
				gridStr += mValues[x, y].ToString();
			}
			gridStr += "\n";
		}
		return gridStr;
	}


	public IEnumerable<(int, int)> ColsRows()
	{
		for(int x = 0; x < GetWidth(); x++)
		{
			for(int y = 0; y < GetHeight(); y++)
			{
				yield return (x, y);
			}
		}
	}

	public IEnumerable<T> GetAdjacent(int x, int y)
	{
		yield return GetAt(x - 1, y);
		yield return GetAt(x - 1, y - 1);
		yield return GetAt(x + 0, y - 1);
		yield return GetAt(x + 1, y - 1);
		yield return GetAt(x + 1, y);
		yield return GetAt(x + 1, y + 1);
		yield return GetAt(x + 0, y + 1);
		yield return GetAt(x - 1, y + 1);
	}

	public IEnumerable<T> GetAdjacent((int, int) coords)
	{
		yield return GetAt(coords.Item1 - 1, coords.Item2);
		yield return GetAt(coords.Item1 - 1, coords.Item2 - 1);
		yield return GetAt(coords.Item1 + 0, coords.Item2 - 1);
		yield return GetAt(coords.Item1 + 1, coords.Item2 - 1);
		yield return GetAt(coords.Item1 + 1, coords.Item2);
		yield return GetAt(coords.Item1 + 1, coords.Item2 + 1);
		yield return GetAt(coords.Item1 + 1, coords.Item2 + 1);
		yield return GetAt(coords.Item1 - 1, coords.Item2 + 1);
	}
}
