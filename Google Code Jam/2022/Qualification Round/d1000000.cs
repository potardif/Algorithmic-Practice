using System;
using System.Linq;

class Program {

	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());

		for (int x = 1; x <= T; ++x) {
			int N = int.Parse(Console.ReadLine());
			int[] S = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();

			int y = GetMaxStraightLength(N, S);
			Console.WriteLine($"Case #{x}: {y}");
		}
	}

	private static int GetMaxStraightLength(int N, int[] S) {
		Array.Sort(S);

		int max = Math.Min(N, S[N - 1]);

		for (int i = 0; i < N; ++i) {
			int height = S[N - 1 - i];
			int wantedHeight = max - i;

			if (height < wantedHeight) {
				max -= wantedHeight - height;
			}
		}

		return max;
	}
}
