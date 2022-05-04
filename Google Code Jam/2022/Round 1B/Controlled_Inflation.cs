using System;
using System.Collections.Generic;
using System.Linq;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			string[] tokens = Console.ReadLine().Split(" ");
			int N = int.Parse(tokens[0]);
			int P = int.Parse(tokens[1]);

			var X = new int[N][];
			for (int i = 0; i < N; ++i) {
				X[i] = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
			}

			long y = GetMinBtnPresses(N, P, X);
			Console.WriteLine($"Case #{x}: {y}");
		}
	}

	private static long GetMinBtnPresses(int N, int P, int[][] X) {
		Order[] orders = X.Select(x => new Order() {
			MinPressure = x.Min(),
			MaxPressure = x.Max(),
		}).ToArray();

		var possibilities = new Dictionary<int, long>() {
			[0] = 0,
		};

		foreach (Order order in orders) {
			int minX = order.MinPressure;
			int maxX = order.MaxPressure;

			var possibilities2 = new Dictionary<int, long>();
			foreach (KeyValuePair<int, long> kvp in possibilities) {
				int pressure = kvp.Key;
				long btnPresses = kvp.Value;

				// Left to right
				int d2Min = Math.Abs(minX - pressure);
				possibilities2.Upsert(maxX, btnPresses + d2Min + maxX - minX, Math.Min);

				// Right to left
				int d2Max = Math.Abs(maxX - pressure);
				possibilities2.Upsert(minX, btnPresses + d2Max + maxX - minX, Math.Min);
			}

			possibilities = possibilities2;
		}

		return possibilities.Values.Min();
	}
}

class Order {
	public int MinPressure { get; set; }
	public int MaxPressure { get; set; }
}

static class Helper {
	public static void Upsert<K, V>(this Dictionary<K, V> dic, K k, V v2, Func<V, V, V> merge) {
		if (dic.TryGetValue(k, out V v1)) {
			dic[k] = merge(v1, v2);
		} else {
			dic.Add(k, v2);
		}
	}
}
