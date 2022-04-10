using System;
using System.Linq;

class Program {
	private const int nbPrinters = 3;
	private const int nbColors = 4;

	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());

		for (int x = 1; x <= T; ++x) {
			var ink = new int[nbPrinters][];
			for (int printer = 0; printer < nbPrinters; ++printer) {
				ink[printer] = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
			}

			int[] color = GetColor(ink);
			string r = color == null ? "IMPOSSIBLE" : string.Join(" ", color);
			Console.WriteLine($"Case #{x}: {r}");
		}
	}

	private static int[] GetColor(int[][] ink) {
		var mins = new int[nbColors];
		for (int color = 0; color < nbColors; ++color) {
			int min = int.MaxValue;
			for (int printer = 0; printer < nbPrinters; ++printer) {
				min = Math.Min(min, ink[printer][color]);
			}

			mins[color] = min;
		}

		const int target = 1_000_000;
		if (mins.Sum() < target) return null;
		else
		{
			var result = new int[nbColors];
			int totalUnits = 0;

			for (int color = 0; totalUnits < target; ++color) {
				int units = Math.Min(mins[color], target - totalUnits);
				result[color] = units;
				totalUnits += units;
			}

			return result;
		}
	}
}
