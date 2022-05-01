using System;
using System.Linq;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			int N = int.Parse(Console.ReadLine());
			int[] D = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();

			long y = MaxPaidPancakes(N, D);
			Console.WriteLine($"Case #{x}: {y}");
		}
	}

	private static long MaxPaidPancakes(int N, int[] D) {
		int i = 0;
		int j = D.Length - 1;

		long paidPancakes = 0;
		int maxPrevD = int.MinValue;
		while (i <= j) {
			int curD;
			if (D[i] <= D[j]) {
				// Serve the left pancake.
				curD = D[i];
				++i;
			} else {
				// Serve the right pancake.
				curD = D[j];
				--j;
			}

			if (curD >= maxPrevD) {
				++paidPancakes;
				maxPrevD = curD;
			}
		}

		return paidPancakes;
	}
}
