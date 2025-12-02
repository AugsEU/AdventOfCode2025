using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025;

static class Debug
{
	public static void Assert(bool condition, string msg)
	{
		if(!condition)
		{
			Console.WriteLine(msg);
			throw new Exception(msg);
		}
	}
}
