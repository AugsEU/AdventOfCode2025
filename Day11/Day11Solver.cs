namespace AdventOfCode2025;

internal class Day11Solver : ISolver
{
	const uint VISITED_FFT = 0b0001;
	const uint VISITED_DAC = 0b0010;

	record NodeExploreData(string node, uint flags);

	Dictionary<string, List<string>> mNodeToOutputs = new();
	Dictionary<NodeExploreData, long> mSearchCache = new();

	void ParseGraph(string input)
	{
		foreach (string line in InputParser.GetNonEmptyLines(input))
		{
			// E.g. aaa: you hhh
			string[] lineSplit = line.Split(":");
			Debug.Assert(lineSplit.Length == 2, "Invalid input line");
			string source = lineSplit[0];
			string targets = lineSplit[1].Trim();
			List<string> outputs = new();
			foreach (string trg in targets.Split(' '))
			{
				outputs.Add(trg);
			}

			mNodeToOutputs[source] = outputs;
		}
	}

	public string SolvePart1(string input)
	{
		ParseGraph(input);

		Stack<string> nodesToExplore = new();
		nodesToExplore.Push("you");

		int numEnds = 0;
		while (nodesToExplore.Count > 0)
		{
			string node = nodesToExplore.Pop();

			if(node == "out")
			{
				numEnds++;
			}

			List<string>? outputs = mNodeToOutputs.GetValueOrDefault(node);
			if (outputs is null)
				continue;

			foreach(string output in outputs)
			{
				nodesToExplore.Push(output);
			}
		}

		return numEnds.ToString();
	}

	public string SolvePart2(string input)
	{
		ParseGraph(input);

		return GetNumWaysToReachGraphPart2(new NodeExploreData("svr", 0)).ToString();
	}

	long GetNumWaysToReachGraphPart2(NodeExploreData from)
	{
		if(mSearchCache.TryGetValue(from, out long value))
		{
			return value;
		}

		long numWays = 0;
		if (from.node == "out")
		{
			if ((from.flags & VISITED_FFT) != 0 &&
				(from.flags & VISITED_DAC) != 0)
			{
				numWays = 1;
			}
		}
		else
		{
			if(mNodeToOutputs.TryGetValue(from.node, out var outputs))
			{
				foreach(string output in outputs)
				{
					uint newFlags = from.flags;
					if (from.node == "dac")
					{
						newFlags |= VISITED_DAC;
					}
					else if (from.node == "fft")
					{
						newFlags |= VISITED_FFT;
					}

					NodeExploreData nextExploreData = new NodeExploreData(output, newFlags);
					numWays += GetNumWaysToReachGraphPart2(nextExploreData);
				}
			}
		}

		mSearchCache[from] = numWays;
		return numWays;
	}
}
