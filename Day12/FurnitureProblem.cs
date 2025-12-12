namespace AdventOfCode2025;

struct FurnitureProblem : IEquatable<FurnitureProblem>
{
	public long[] mChairCounts;
	public FurnitureSpace mSpace;

	public FurnitureProblem(long[] chairCounts, int width, int height)
	{
		mChairCounts = chairCounts;
		mSpace = new FurnitureSpace(width, height);
	}

	public FurnitureProblem(long[] chairCounts, FurnitureSpace space)
	{
		mChairCounts = chairCounts;
		mSpace = space;
	}

	public bool Equals(FurnitureProblem other)
	{
		for (int i = 0; i < mChairCounts.Length; i++)
		{
			if (other.mChairCounts[i] != mChairCounts[i])
				return false;
		}

		return mSpace.Equals(other.mSpace);
	}

	public override int GetHashCode()
	{
		int hash = mChairCounts.GetHashCode();
		hash ^= mSpace.GetHashCode();

		return hash;
	}

	public FurnitureProblem CreateCopy()
	{
		long[] newArr = new long[mChairCounts.Length];
		Array.Copy(mChairCounts, newArr, mChairCounts.Length);

		FurnitureSpace newSpace = new FurnitureSpace(mSpace.CreateCopy());
		return new FurnitureProblem(newArr, newSpace);
	}
}
