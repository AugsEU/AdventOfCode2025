using System.Linq;

namespace AdventOfCode2025;

internal class Program
{
	#region rConstants

	// Program inputs
	const string INPUTS_PATH = "C:\\Users\\Augus\\Documents\\Programming\\AdventOfCode\\2025\\AdventOfCode2025\\Inputs";
	static int[] CURR_DAYS = { 1 };
	static int[] CURR_PARTS = { 1, 2 };
	static bool IGNORE_TEST_FAIL = false;
	static bool SKIP_TEST = true;

	#endregion rConstants




	#region rMembers

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Write a main thing.
	/// </summary>
	/// <param name="args"></param>
	static void Main(string[] args)
	{
		for (int d = 0; d < CURR_DAYS.Length; d++)
		{
			int day = CURR_DAYS[d];
			for (int p = 0; p < CURR_PARTS.Length; p++)
			{
				int part = CURR_PARTS[p];
				WriteTitle($"Solving Day {day} | Part {part}", ConsoleColor.Blue);
				ISolver solver = CreateSolver(day);

				// Test
				ISolver testSolver = CreateSolver(day);
				TestData testData = TestInputs.GetTest(day);

				string inputText = ReadInput(day, part);
				Console.WriteLine("");
				if (part < 2)
				{
					bool passed = SKIP_TEST || CheckTest(testData.expectedP1, testSolver.SolvePart1(testData.input));

					if (passed || IGNORE_TEST_FAIL)
					{
						string part1Soln = solver.SolvePart1(inputText);
						WriteColor($"    Part 1 solution: {part1Soln}", ConsoleColor.Yellow);
					}
				}
				else
				{
					bool passed = SKIP_TEST || CheckTest(testData.expectedP2, testSolver.SolvePart2(testData.input));

					if (passed || IGNORE_TEST_FAIL)
					{
						string part2Soln = solver.SolvePart2(inputText);
						WriteColor($"    Part 2 solution: {part2Soln}", ConsoleColor.Yellow);
					}
				}
			}
		}

		Console.ReadLine();
	}



	/// <summary>
	/// Check if a test is right and report if not
	/// </summary>
	static bool CheckTest(string expected, string solution)
	{
		WriteColor("    Running test...", ConsoleColor.DarkYellow);
		if (solution != expected)
		{
			WriteColor($"    FAILED: Got |{solution}| expected |{expected}|", ConsoleColor.Red);
			return false;
		}
		
		WriteColor("    PASSED", ConsoleColor.Green);
		return true;
	}

	#endregion rInit





	#region rUtil

	/// <summary>
	/// Write a pretty title
	/// </summary>
	static void WriteTitle(string title, ConsoleColor color)
	{
		ConsoleColor prevCol = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.WriteLine("========================================================");
		Console.WriteLine(title);
		Console.WriteLine("========================================================");
		Console.ForegroundColor = prevCol;
	}



	/// <summary>
	/// Write a pretty line
	/// </summary>
	static void WriteColor(string str, ConsoleColor color)
	{
		ConsoleColor prevCol = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.WriteLine(str);
		Console.ForegroundColor = prevCol;
	}



	/// <summary>
	/// Read a text file to get the input.
	/// </summary>
	static string ReadInput(int day, int part)
	{
		string fileName = Path.Join(INPUTS_PATH, $"/Day{day}-{part}.txt");
		List<string> lines = new();
		using (StreamReader fs = new StreamReader(fileName))
		{
			while (fs.ReadLine() is string line)
			{
				line = line.Trim();

				if (line.Length != 0)
				{
					lines.Add(line);
				}
			}
		}

		return string.Join('\n', lines);
	}



	/// <summary>
	/// Create a new solver for a particular day.
	/// </summary>
	static ISolver CreateSolver(int day)
	{
		switch (day)
		{
			case 1:
				return new Day1Solver();
			default:
				break;
		}

		throw new NotImplementedException();
	}

	#endregion rUtil
}
