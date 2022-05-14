using System;
using System.Collections.Generic;
using System.Linq;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			string[] tokens = Console.ReadLine().Split(" ");
			int C = int.Parse(tokens[0]); // Number of computers
			int D = int.Parse(tokens[1]); // Number of direct connections

			var X = new List<int>(C) {
				0, // The source computer receives the update at time 0.
			};
			X.AddRange(Console.ReadLine().Split(" ").Select(int.Parse));

			var U = new int[D];
			var V = new int[D];
			for (int i = 0; i < D; ++i) {
				tokens = Console.ReadLine().Split(" ");
				U[i] = int.Parse(tokens[0]) - 1;
				V[i] = int.Parse(tokens[1]) - 1;
			}

			string y = GetLatencies(C, D, X, U, V).Join(" ");
			Console.WriteLine($"Case #{x}: {y}");
		}
	}

	private static int[] GetLatencies(int C, int D, List<int> X, int[] U, int[] V) {
		// Convert all the Xs to times.

		int[] negXs =
			Enumerable.Range(0, C)
			.Where(c => X[c] < 0)
			.OrderBy(c => -X[c])
			.ToArray();
		int[] posXs =
			Enumerable.Range(0, C)
			.Where(c => X[c] >= 0)
			.OrderBy(c => X[c])
			.ToArray();

		var T = new List<int>(C);
		int i = 0;
		int j = 0;
		while (T.Count < C) {
			int? c1 = i < negXs.Length ? negXs[i] : (int?)null;
			
			if (c1 != null && -X[c1.Value] <= T.Count) {
				X[c1.Value] = T[-X[c1.Value] - 1] + 1;
				T.Add(X[c1.Value]);
				++i;
			} else {
				int c2 = posXs[j];
				T.Add(X[c2]);
				++j;
			}
		}

		// Solve the problem.
		var y = new int[D];
		for (i = 0; i < D; ++i) {
			y[i] = Math.Abs(X[U[i]] - X[V[i]]);

			if (y[i] == 0) {
				// Assign any value greater than 0 because it's impossible that this path was used
				// to get the update.
				y[i] = 1_000_000;
			}
		}
		return y;
	}
}

static class Helper {
	public static string Join<T>(this IEnumerable<T> values, string separator) {
		return String.Join(separator, values);
	}
}
