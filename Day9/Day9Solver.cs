namespace AdventOfCode2025;

internal class Day9Solver : ISolver
{
	Random mRng = new Random();

	bool IsGreenTile(List<Point2> points, Point2 pt)
	{
		bool inside = false;
		for(int i = 0; i < points.Count; i++)
		{
			// Edges
			Point2 e1 = points[i];
			Point2 e2 = points[(i + 1) % points.Count];

			if(pt.Equals(e1)) // Case 1: Is red tile? Function call not needed?
			{
				return true;
			}

			Point2 min = new Point2(Math.Min(e1.mX, e2.mX), Math.Min(e1.mY, e2.mY));
			Point2 max = new Point2(Math.Max(e1.mX, e2.mX), Math.Max(e1.mY, e2.mY));

			if (min.mX == max.mX)// Case 2: Vertical line
			{
				long eX = min.mX;
				if (pt.mX == eX && (min.mY <= pt.mY && pt.mY <= max.mY)) // Case 2.1: Point is on line.
				{
					return true;
				}

				// Case 2.2: Compare edge against horizontal ray-cast
				bool hitsRay = pt.mX <= eX && (min.mY <= pt.mY && pt.mY < max.mY);
				if(hitsRay)
				{
					inside = !inside;
				}
			}
			else if(min.mY == max.mY) // Case 3: Horiz line
			{
				long eY = min.mY;
				if (pt.mY == eY && (min.mX <= pt.mX && pt.mX <= max.mX)) // Case 3.1: Point is on line.
				{
					return true;
				}
			}

		}

		// Case 4: We are inside the boundary.
		return inside;
	}

	bool IsLikelyAcceptableRectangle(List<Point2> points, Point2 pi, Point2 pj)
	{
		// Try a few points and see if they are all green. If so it's likely a good rectangle.
		Point2 pk = new Point2(pi.mX, pj.mY);
		Point2 pl = new Point2(pj.mX, pi.mY);
		Point2 pc = new Point2((pi.mX + pj.mX) / 2, (pi.mY + pj.mY) / 2);

		Point2 pm = new Point2((pi.mX + pj.mX) / 2, pi.mY);
		Point2 pn = new Point2(pj.mX, (pi.mY + pj.mY) / 2);


		return IsGreenTile(points, pk) && 
			IsGreenTile(points, pl) && 
			IsGreenTile(points, pc) &&
			IsGreenTile(points, pm) &&
			IsGreenTile(points, pn);
	}

	bool IsTrulyAcceptableRectangle(List<Point2> points, Point2 pi, Point2 pj)
	{
		(long minX, long maxX) = (Math.Min(pi.mX, pj.mX), Math.Max(pi.mX, pj.mX));
		(long minY, long maxY) = (Math.Min(pi.mY, pj.mY), Math.Max(pi.mY, pj.mY));

		for (long x = minX+1; x < maxX; x++)
		{
			if(!IsGreenTile(points, new Point2(x, pi.mY))) // PI horiz edge
			{
				return false;
			}

			if (!IsGreenTile(points, new Point2(x, pj.mY))) // PJ horiz edge
			{
				return false;
			}
		}

		for (long y = minY + 1; y < maxY; y++)
		{
			if (!IsGreenTile(points, new Point2(pi.mX, y))) // PI vert edge
			{
				return false;
			}

			if (!IsGreenTile(points, new Point2(pj.mY, y))) // PJ vert edge
			{
				return false;
			}
		}

		return true;
	}

	static long CalcArea((Point2, Point2) corners)
	{
		return (Math.Abs(corners.Item1.mX - corners.Item2.mX) + 1) * (Math.Abs(corners.Item1.mY - corners.Item2.mY) + 1);
	}

	public string SolvePart1(string input)
	{
		List<Point2> points = InputParser.ParsePoint2List(input);

		long largestArea = 0;
		for(int i = 0; i < points.Count; i++)
		{
			for(int j = i+1; j < points.Count; j++)
			{
				Point2 pi = points[i];
				Point2 pj = points[j];

				long area = CalcArea((pi, pj));
				if (area > largestArea)
				{
					largestArea = area;
				}
			}
		}

		return largestArea.ToString();
	}

	public string SolvePart2(string input)
	{
		List<Point2> points = InputParser.ParsePoint2List(input);

		List<(Point2, Point2)> candidates = new();

		Console.WriteLine("Looking for candiates");
		for (int i = 0; i < points.Count; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				Point2 pi = points[i];
				Point2 pj = points[j];

				if(IsLikelyAcceptableRectangle(points, pi, pj))
				{
					candidates.Add((pi, pj));
				}
			}
		}

		candidates.Sort((x, y) => CalcArea(x).CompareTo(CalcArea(y)));

		while (candidates.Count > 0)
		{
			int lastIdx = candidates.Count - 1;
			(Point2, Point2) maxCand = candidates[lastIdx];
			long area = CalcArea(maxCand);
			Console.WriteLine($"    Checking candidate {maxCand.Item1.ToString()} - {maxCand.Item2.ToString()} of area {area}");
			if(IsTrulyAcceptableRectangle(points, maxCand.Item1, maxCand.Item2))
			{
				return area.ToString();
			}

			// candidate was rejected
			candidates.RemoveAt(lastIdx);
		}

		throw new Exception("Found no truly acceptable rectangle");
	}
}
