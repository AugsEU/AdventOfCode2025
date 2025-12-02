using System.Reflection;

namespace AdventOfCode2025;

internal class Day2Solver : ISolver
{

	#region rMembers

	#endregion rMembers





	#region rUtil

	/// <summary>
	/// Is this an int made of 2 repeating parts like 123123
	/// </summary>
	public bool IsDoubleRepeatInt(Int64 x)
	{
		string xs = x.ToString();
		int len = xs.Length;
		if (len % 2 != 0)
		{
			return false;
		}

		string first = xs.Substring(0, len/2);
		string second = xs.Substring(len/2);

		return first == second;
	}



	/// <summary>
	/// Is this string made of N repitions of digits?
	/// Like 121212 is a 3-string
	/// </summary>
	public bool IsNRepeatInt(Int64 x, int n)
	{
		string xs = x.ToString();
		int len = xs.Length;
		if (len % n != 0 || n == 1)
		{
			return false;
		}

		Debug.Assert(len / n > 0, $"N is too big {n} {x}");

		int step = len / n;
		string first = xs.Substring(0, step);
		for (int i = 1; i < n; i++)
		{
			string sub = xs.Substring(i * step, step);
			if(sub != first)
			{
				return false;
			}
		}

		return true;
	}

	#endregion rUtil





	#region rMain

	/// <summary>
	/// Solve part 1
	/// </summary>
	public string SolvePart1(string input)
	{
		List<string> rangeStrs = InputParser.GetCommaSeparated(input);
		Int64 sum = 0;
		foreach (string rangeStr in rangeStrs)
		{
			(Int64 min, Int64 max) = InputParser.ParseRange(rangeStr);
			for(Int64 i = min; i <= max; i++)
			{
				bool isRepeat = IsDoubleRepeatInt(i);
				if (isRepeat) sum += i;
			}
		}

		return sum.ToString();
	}

	/// <summary>
	/// Solve part 2
	/// </summary>
	public string SolvePart2(string input)
	{
		Debug.Assert(IsNRepeatInt(2121212121, 5), "Not N repeater");

		List<string> rangeStrs = InputParser.GetCommaSeparated(input);
		Int64 sum = 0;
		foreach (string rangeStr in rangeStrs)
		{
			(Int64 min, Int64 max) = InputParser.ParseRange(rangeStr);
			for (Int64 i = min; i <= max; i++)
			{
				int digitLen = (int)(Math.Log10((double)i) + 1.0);

				for(int n = 1; n <= digitLen; n++)
				{
					bool isRepeat = IsNRepeatInt(i, n);
					if (isRepeat)
					{
						Console.WriteLine($"Found {i} as a {n} repeater");
						sum += i;
						break;
					}
				}
				
			}
		}

		return sum.ToString();
	}

	#endregion rMain
}
