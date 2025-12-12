namespace AdventOfCode2025;

class Chair
{
	public const int NUM_ORIENTATIONS = 8;

	int mSize = 0;
	int mNumDots = 0;
	Grid<char> mDefn;

	public Chair(string subStr)
	{
		// e.g.
		//0:
		//###
		//##.
		//##.

		string[] lines = subStr.Split('\n');
		mSize = lines.Length - 1;

		mDefn = new Grid<char>(new char[mSize, mSize], ' ');
		for (int y = 0; y < mSize; y++)
		{
			string line = lines[y + 1];
			for (int x = 0; x < line.Length; x++)
			{
				if (line[x] == '#') mNumDots++;

				mDefn.Set(x, y, line[x]);
			}
		}
	}

	public Grid<char> GetRelative()
	{
		return mDefn;
	}

	public char GetAt(int x, int y, int orientation)
	{
		int fx = mSize - x - 1;
		int fy = mSize - y - 1;

		switch (orientation)
		{
			case 0:
				return mDefn.Get(x, y);
			case 1:
				return mDefn.Get(fx, y);
			case 2:
				return mDefn.Get(x, fy);
			case 3:
				return mDefn.Get(fx, fy);
			case 4:
				return mDefn.Get(y, x);
			case 5:
				return mDefn.Get(fy, x);
			case 6:
				return mDefn.Get(y, fx);
			case 7:
				return mDefn.Get(fy, fx);
			default:
				break;
		}

		throw new Exception("Invalid orientation");
	}

	public int GetSize() { return mSize; }

	public int GetNumDots() { return mNumDots; }
}
