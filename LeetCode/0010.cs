public class Solution {
	public bool IsMatch(string s, string p) {
		var dp = new bool[s.Length + 1, p.Length + 1];
		dp[s.Length, p.Length] = true;

		for (int j = p.Length - 1; j >= 0; --j) {
			bool star = false;
			if (p[j] == '*') {
				star = true;
				--j;
			}

			for (int i = s.Length; i >= 0; --i) {
				if (star) {
					dp[i, j] =
						dp[i, j + 2]
						|| i < s.Length && (s[i] == p[j] || p[j] == '.') && dp[i + 1, j];
				} else {
					dp[i, j] =
						i < s.Length && (s[i] == p[j] || p[j] == '.') && dp[i + 1, j + 1];
				}
			}
		}

		return dp[0, 0];
	}
}
