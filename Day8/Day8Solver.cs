using System.Formats.Asn1;

namespace AdventOfCode2025;

class GraphEdgeComparer : IComparer<(int, int)>
{
	List<Point3> mPts;

	public GraphEdgeComparer(List<Point3> nodes)
	{
		mPts = nodes;
	}

	public int Compare((int, int) x, (int, int) y)
	{
		long xDistSq = mPts[x.Item1].DistSqI(mPts[x.Item2]);
		long yDistSq = mPts[y.Item1].DistSqI(mPts[y.Item2]);

		return xDistSq.CompareTo(yDistSq);
	}
}

internal class Day8Solver : ISolver
{
	class NodeGroupInfo
	{
		public long ID { get; set; } = 0;
		public int NumInGroup { get; set; } = 0;
	}

	public void BuildOpenEdges(List<Point3> nodes, SortedSet<(int, int)> allPossibleEdges)
	{
		for(int i = 0; i < nodes.Count; ++i)
		{
			for(int j = i+1; j < nodes.Count; ++j)
			{
				allPossibleEdges.Add((i, j));
			}
		}
	}

	public string SolvePart1(string input)
	{
		List<Point3> nodes = InputParser.ParsePoint3List(input);
		GraphEdgeComparer comp = new GraphEdgeComparer(nodes);
		Dictionary<int, NodeGroupInfo> nodeToInfo = new();
		SortedSet<(int, int)> openEdges = new(comp);

		// HACK: detect test input
		int numNodesToConnect = nodes.Count == 20 ? 10 : 1000;

		BuildOpenEdges(nodes, openEdges);

		long nextUnusedID = 1;
		for(int k = 0; k < numNodesToConnect; ++k)
		{
			// Get smallest edge
			(int lowi, int lowj) = openEdges.Min;

			nodeToInfo.TryGetValue(lowi, out NodeGroupInfo? infoI);
			nodeToInfo.TryGetValue(lowj, out NodeGroupInfo? infoJ);

			if (infoI is not null && infoJ is not null) // Case 1: Both are in a group
			{
				if(infoI.ID == infoJ.ID) // Case 1.1: Both are in the same group
				{
					// Nothing to do here.
				}
				else // Case 1.2: Both are in different groups, merge the groups.
				{
					// Merge infoJ group into lowJ
					foreach(int idx in nodeToInfo.Keys)
					{
						if(nodeToInfo.TryGetValue(idx, out NodeGroupInfo? infoOther) && infoOther.ID == infoJ.ID)
						{
							nodeToInfo[idx] = infoI;
						}
					}
					infoI.NumInGroup += infoJ.NumInGroup;
				}
			}
			else if(infoI is not null && infoJ is null) // Case 2: I is in group but not J
			{
				nodeToInfo[lowj] = infoI;
				infoI.NumInGroup += 1;
			}
			else if(infoI is null && infoJ is not null) // Case 3: J is in group but not I
			{
				nodeToInfo[lowi] = infoJ;
				infoJ.NumInGroup += 1;
			}
			else // Case 4: Neither are in a group, start a new one
			{
				NodeGroupInfo newGroup = new NodeGroupInfo();
				newGroup.ID = nextUnusedID++;
				newGroup.NumInGroup = 2;
				nodeToInfo[lowi] = newGroup;
				nodeToInfo[lowj] = newGroup;
			}

			openEdges.Remove((lowi, lowj));
		}


		SortedSet<NodeGroupInfo> uniqueGroups = new(Comparer<NodeGroupInfo>.Create((x,y)=>x.NumInGroup.CompareTo(y.NumInGroup)));
		foreach(NodeGroupInfo nodeGroupInfo in nodeToInfo.Values)
		{
			uniqueGroups.Add(nodeGroupInfo);
		}

		int countedIds = 0;
		long answer = 1;
		while(countedIds < 3 && uniqueGroups.Count > 0)
		{
			NodeGroupInfo? info = uniqueGroups.Max;
			if (info is null) throw new Exception("Can't find max?");

			Console.WriteLine($"Counting group {info.ID} of size {info.NumInGroup}");
			answer *= info.NumInGroup;
			countedIds++;

			uniqueGroups.Remove(info);
		}

		return answer.ToString();
	}

	public string SolvePart2(string input)
	{
		List<Point3> nodes = InputParser.ParsePoint3List(input);
		GraphEdgeComparer comp = new GraphEdgeComparer(nodes);
		Dictionary<int, NodeGroupInfo> nodeToInfo = new();
		HashSet<NodeGroupInfo> uniqueGroups = new();
		SortedSet<(int, int)> openEdges = new(comp);

		BuildOpenEdges(nodes, openEdges);

		long nextUnusedID = 1;
		while(openEdges.Count > 0)
		{
			// Get smallest edge
			(int lowi, int lowj) = openEdges.Min;

			nodeToInfo.TryGetValue(lowi, out NodeGroupInfo? infoI);
			nodeToInfo.TryGetValue(lowj, out NodeGroupInfo? infoJ);

			if (infoI is not null && infoJ is not null) // Case 1: Both are in a group
			{
				if (infoI.ID == infoJ.ID) // Case 1.1: Both are in the same group
				{
					// Nothing to do here.
				}
				else // Case 1.2: Both are in different groups, merge the groups.
				{
					// Merge infoJ group into lowJ
					foreach (int idx in nodeToInfo.Keys)
					{
						if (nodeToInfo.TryGetValue(idx, out NodeGroupInfo? infoOther) && infoOther.ID == infoJ.ID)
						{
							nodeToInfo[idx] = infoI;
						}
					}
					uniqueGroups.Remove(infoJ);
					infoI.NumInGroup += infoJ.NumInGroup;
				}
			}
			else if (infoI is not null && infoJ is null) // Case 2: I is in group but not J
			{
				nodeToInfo[lowj] = infoI;
				infoI.NumInGroup += 1;
			}
			else if (infoI is null && infoJ is not null) // Case 3: J is in group but not I
			{
				nodeToInfo[lowi] = infoJ;
				infoJ.NumInGroup += 1;
			}
			else // Case 4: Neither are in a group, start a new one
			{
				NodeGroupInfo newGroup = new NodeGroupInfo();
				newGroup.ID = nextUnusedID++;
				newGroup.NumInGroup = 2;
				nodeToInfo[lowi] = newGroup;
				nodeToInfo[lowj] = newGroup;
				uniqueGroups.Add(newGroup);
			}

			// Detect all connected: Every node is in a group and there is only 1 group
			if(uniqueGroups.Count == 1 && nodeToInfo.Count == nodes.Count)
			{
				Point3 pti = nodes[lowi];
				Point3 ptj = nodes[lowj];

				return (pti.mX * ptj.mX).ToString();
			}

			openEdges.Remove((lowi, lowj));
		}

		throw new Exception("Ran out of edges but didn't connect everything?");
	}
}
