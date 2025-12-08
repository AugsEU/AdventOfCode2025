namespace AdventOfCode2025;

class Point3EdgeComparer : IComparer<(Point3, Point3)>
{
	public int Compare((Point3, Point3) x, (Point3, Point3) y)
	{
		return x.Item1.DistSqI(x.Item2).CompareTo(y.Item1.DistSqI(y.Item2));
	}
}

struct Point3(long x, long y, long z) : IEquatable<Point3>
{
	#region rMembers

	public long mX = x;
	public long mY = y;
	public long mZ = z;

	#endregion rMembers





	#region rOperators

	public static Point3 operator +(Point3 left, Point3 right) => new Point3(left.mX + right.mX, left.mY + right.mY, left.mZ + right.mZ);
	public static Point3 operator -(Point3 left, Point3 right) => new Point3(left.mX - right.mX, left.mY - right.mY, left.mZ - right.mZ);
	public static Point3 operator -(Point3 pt) => new Point3(-pt.mX, -pt.mY, -pt.mZ);

	#endregion rOperators





	#region rUtil

	public float DistF(Point3 other)
	{
		Point3 dt = this - other;

		return MathF.Sqrt(dt.mX * dt.mX + dt.mY * dt.mY + dt.mZ * dt.mZ);
	}

	public long DistSqI(Point3 other)
	{
		Point3 dt = this - other;
		return dt.mX * dt.mX + dt.mY * dt.mY + dt.mZ * dt.mZ;
	}

	public override string ToString()
	{
		return $"({mX}, {mY}, {mZ})";
	}

	#endregion rUtil



	#region rHash

	public override int GetHashCode()
	{
		int hash = mX.GetHashCode() * 213;
		hash += mY.GetHashCode() ^ 0x213;
		hash *= mZ.GetHashCode();

		return hash;
	}

	public bool Equals(Point3 other)
	{
		return other.mX == mX && other.mY == mY && other.mZ == mZ;
	}

	#endregion rHash
}
