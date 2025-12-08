namespace AdventOfCode2025;

struct Point2(long x, long y) : IEquatable<Point2>
{
	#region rMembers

	public long mX = x;
	public long mY = y;

	#endregion rMembers





	#region rOperators

	public static Point2 operator +(Point2 left, Point2 right) => new Point2(left.mX + right.mX, left.mY + right.mY);
	public static Point2 operator -(Point2 left, Point2 right) => new Point2(left.mX - right.mX, left.mY - right.mY);
	public static Point2 operator -(Point2 pt) => new Point2(-pt.mX, -pt.mY);

	#endregion rOperators





	#region rUtil

	public float DistF(Point2 other)
	{
		return MathF.Sqrt(mX * other.mX + mY * other.mY);
	}

	public long DistSqI(Point2 other)
	{
		return mX * other.mX + mY * other.mY;
	}

	#endregion rUtil




	#region rHash

	public override int GetHashCode()
	{
		int hash = mX.GetHashCode() * 213;
		hash += mY.GetHashCode() ^ 0x213;

		return hash;
	}

	public bool Equals(Point2 other)
	{
		return other.mX == mX && other.mY == mY;
	}

	#endregion rHash
}
