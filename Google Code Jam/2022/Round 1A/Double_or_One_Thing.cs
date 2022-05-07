using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			string S = Console.ReadLine();
			string y = GetFirstString(S);
			Console.WriteLine($"Case #{x}: {y}");
		}
	}

	private static string GetFirstString(string S) {
		var charGroups = new List<CharGroup>();
		for (int i = 0; i < S.Length; ) {
			char c = S[i];

			int n = 1;
			for (i = i + 1; i < S.Length && c == S[i]; ++i) {
				++n;
			}

			charGroups.Add(new CharGroup() {
				c = c,
				n = n,
			});
		}

		var sb = new StringBuilder();
		for (int i = 0; i < charGroups.Count; ++i) {
			char c = charGroups[i].c;
			int n = charGroups[i].n;

			bool double_ = i + 1 < charGroups.Count && c < charGroups[i + 1].c;

			if (double_) n *= 2;
			foreach (var _ in Enumerable.Range(0, n)) {
				sb.Append(c);
			}
		}

		return sb.ToString();
	}
}

class CharGroup {
	public char c;
	public int n;
}
