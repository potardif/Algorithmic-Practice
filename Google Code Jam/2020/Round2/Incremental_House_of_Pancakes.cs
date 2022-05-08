using System;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			string[] tokens = Console.ReadLine().Split(" ");
			long L = long.Parse(tokens[0]);
			long R = long.Parse(tokens[1]);

			Result result = GetStateAtClosingTime(L, R);
			Console.WriteLine($"Case #{x}: {result.n} {result.l} {result.r}");
		}
	}

	private static Result GetStateAtClosingTime(long L, long R) {
		long l = L;
		long r = R;
		long i = 0;

		// Phase 1: One-way fast-forward

		Func<long, long> f = n => n * (n + 1) / 2;
		long nPhase1 = BinarySearch(left: 0, f: f, target: Math.Abs(l - r));

		if (l >= r) {
			l -= f(nPhase1);
		} else {
			r -= f(nPhase1);
		}
		i += nPhase1;

		// Phase 2: Two-way fast-forward

		Func<long, long> f1 = n => {
			long start = i + 1;
			long end = start + 2 * (n - 1);
			return (start + end) * n / 2;
		};
		long n1 = BinarySearch(left: 0, f: f1, target: Math.Max(l, r));

		Func<long, long> f2 = n => {
			long start = i + 2;
			long end = start + 2 * (n - 1);
			return (start + end) * n / 2;
		};
		long n2 = BinarySearch(left: 0, f: f2, target: Math.Min(l, r));

		if (l >= r) {
			l -= f1(n1);
			r -= f2(n2);
		} else {
			l -= f2(n2);
			r -= f1(n1);
		}
		i += n1 + n2;

		return new Result() {
			n = i,
			l = l,
			r = r,
		};
	}

	/// Binary search
	private static long BinarySearch(long left, Func<long, long> f, long target) {
		long right = left;
		while (f(right) <= target) {
			left = right;
			right = right * 2 + 1;
		}

		while (true) {
			long middle = (left + right) / 2;
			if (middle == left || middle == right) {
				return left;
			}

			if (f(middle) <= target) {
				left = middle;
			} else {
				right = middle;
			}
		}
	}
}

class Result {
	public long n;
	public long l;
	public long r;
}
