using System;
using System.Collections.Generic;
using System.Linq;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());

		for (int x = 1; x <= T; ++x) {
			int N = int.Parse(Console.ReadLine());
			int[] F = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
			int?[] P = Console.ReadLine().Split(" ").Select(s => {
				int Pi = int.Parse(s);
				return Pi == 0 ? (int?)null : Pi - 1;
			}).ToArray();

			long y = GetMaxFun(N, F, P);
			Console.WriteLine($"Case #{x}: {y}");
		}
	}

	private static long GetMaxFun(int N, int[] F, int?[] P) {
		var nbParents = new int[N];
		for (int i = 0; i < N; ++i) {
			int? next = P[i];
			if (next != null) {
				++nbParents[next.Value];
			}
		}

		var q = new Dictionary<int, List<int>>();
		for (int i = 0; i < N; ++i) {
			if (nbParents[i] == 0) {
				q.Add(i, new List<int>() { int.MinValue });
			}
		}

		long totalFun = 0;
		while (q.Count > 0) {
			int[] curs = q.Keys.ToArray();
			foreach (int cur in curs) {
				List<int> maxFuns = q[cur];
				if (maxFuns.Count >= nbParents[cur]) {
					int minMaxFunIndex = -1;
					int minMaxFun = int.MaxValue;
					for (int i = 0; i < maxFuns.Count; ++i) {
						if (maxFuns[i] < minMaxFun) {
							minMaxFunIndex = i;
							minMaxFun = maxFuns[i];
						}
					}

					for (int i = 0; i < maxFuns.Count; ++i) {
						if (i != minMaxFunIndex) {
							totalFun += maxFuns[i];
						}
					}

					int maxFun = Math.Max(minMaxFun, F[cur]);

					int? next = P[cur];
					if (next == null) {
						totalFun += maxFun;
					} else {
						List<int> nextMaxFuns;
						if (!q.TryGetValue(next.Value, out nextMaxFuns)) {
							nextMaxFuns = new List<int>();
							q.Add(next.Value, nextMaxFuns);
						}

						nextMaxFuns.Add(maxFun);
					}

					q.Remove(cur);
				}
			}
		}

		return totalFun;
	}
}
