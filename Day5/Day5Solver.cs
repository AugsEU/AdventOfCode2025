using System.Xml.Linq;

namespace AdventOfCode2025;

internal class Day5Solver : ISolver
{
	List<(long, long)> mRanges = new();
	List<long> mIngredients = new();

	void ParseInput(string input)
	{
		mRanges = new();
		mIngredients = new();

		string[] parts = input.Split("\n\n");
		Debug.Assert(parts.Length == 2, "Can't parse input string");

		List<string> rangeLines = InputParser.GetNonEmptyLines(parts[0]);
		List<string> ingredientLines = InputParser.GetNonEmptyLines(parts[1]);

		foreach (string rangeLine in rangeLines)
		{
			string[] upperLowerStr = rangeLine.Split("-");
			Debug.Assert(upperLowerStr.Length == 2, "Can't parse range string");

			long min = long.Parse(upperLowerStr[0]);
			long max = long.Parse(upperLowerStr[1]);

			Debug.Assert(min <= max, $"Min {min} is not less than max {max}");
			mRanges.Add((min, max));
		}

		foreach (string ingredientLine in ingredientLines)
		{
			mIngredients.Add(long.Parse(ingredientLine));
		}
	}

	bool IngredientIsValid(long id)
	{
		foreach ((long, long) range in mRanges)
		{
			if (InRange(id, range))
			{
				return true;
			}
		}

		return false;
	}

	bool InRange(long v, (long, long) r)
	{
		return r.Item1 <= v && v <= r.Item2;
	}

	(long, long) MergeRanges((long, long) r0, (long, long) r1)
	{
		return (Math.Min(r0.Item1, r1.Item1), Math.Max(r0.Item2, r1.Item2));
	}

	bool TryMergeRanges()
	{
		for(int r0 = 0; r0 < mRanges.Count; r0++)
		{
			for(int r1 = r0+1; r1 < mRanges.Count; r1++)
			{
				(long min0, long max0) range0 = mRanges[r0];
				(long min1, long max1) range1 = mRanges[r1];

				if(InRange(range0.min0, range1) || InRange(range0.max0, range1) ||
					InRange(range1.min1, range0) || InRange(range1.max1, range0))
				{
					// Remove both(later index first)
					mRanges.RemoveAt(r1);
					mRanges.RemoveAt(r0);

					// Add merged
					mRanges.Add(MergeRanges(range0, range1));
					return true;
				}
			}
		}

		return false;
	}

	public string SolvePart1(string input)
	{
		ParseInput(input);

		long numValid = 0;
		foreach(long ingredient in mIngredients)
		{
			if(IngredientIsValid(ingredient))
				numValid++;
		}

		return numValid.ToString();
	}

	public string SolvePart2(string input)
	{
		ParseInput(input);

		while(TryMergeRanges())
		{
			// Wait for all ranges to be merged...
		}

		long total = 0;
		foreach((long min, long max) in mRanges)
		{
			total += (max - min + 1);
		}

		return total.ToString();
	}
}
