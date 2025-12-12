
namespace AdventOfCode2025;

internal class FurnitureSpace : Grid<char>
{
	public FurnitureSpace(int width, int height) : base(new char[width, height], '.')
	{
		foreach(Point2 pt in ColsRowsPoints())
		{
			Set(pt, '.');
		}
	}

	public FurnitureSpace(Grid<char> grid) : base(grid)
	{
	}

	public bool TryPlaceChair(Chair chair, int x, int y, int orientation)
	{
		int size = chair.GetSize();
		if(x < 0 || y < 0)
			return false;
		else if(x + size > GetWidth() || y + size > GetHeight())
			return false;

		// First check if we can place it.
		for (int cx = 0; cx < chair.GetSize(); cx++)
		{
			for (int cy = 0; cy < chair.GetSize(); cy++)
			{
				char chairChar = chair.GetAt(cx, cy, orientation);

				int sx = x + cx;
				int sy = y + cy;

				char spaceChar = Get(sx, sy);

				if(chairChar == '#' && spaceChar == '#')
				{
					return false;
				}
			}
		}

		// Place it
		for (int cx = 0; cx < chair.GetSize(); cx++)
		{
			for (int cy = 0; cy < chair.GetSize(); cy++)
			{
				char chairChar = chair.GetAt(cx, cy, orientation);

				int sx = x + cx;
				int sy = y + cy;

				if(chairChar == '#')
					Set(sx, sy, '#');
			}
		}

		return true;
	}

	public void RemoveChair(Chair chair, int x, int y, int orientation)
	{
		for (int cx = 0; cx < chair.GetSize(); cx++)
		{
			for (int cy = 0; cy < chair.GetSize(); cy++)
			{
				char chairChar = chair.GetAt(cx, cy, orientation);

				int sx = x + cx;
				int sy = y + cy;

				if (chairChar == '#')
				{
					Debug.Assert(Get(sx, sy) == '#', "Deleting chair but nothing was here....");
					Set(sx, sy, '.');
				}
			}
		}
	}


	public void FindReplace(char find, char replace)
	{
		foreach (Point2 pt in ColsRowsPoints())
		{
			if(GetAt(pt) == find)
				Set(pt, replace);
		}
	}

	public int GetNumFreeSpaces()
	{
		int total = 0;
		foreach(Point2 pt in ColsRowsPoints())
		{
			if(GetAt(pt) != '#')
			{
				total++;
			}
		}
		return total;
	}
}
