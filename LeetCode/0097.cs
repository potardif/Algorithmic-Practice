public class Solution {
	public bool IsInterleave(string s1, string s2, string s3) {
		if (s1.Length + s2.Length != s3.Length) {
			return false;
		} else if (s2.Length < s1.Length) {
			string tmpS1 = s1;
			s1 = s2;
			s2 = tmpS1;
		}
		
		var dp = new bool[s1.Length + 1];

		dp[0] = true;
		for (int len1 = 1; len1 <= s1.Length && s1[len1 - 1] == s3[len1 - 1]; ++len1) {
			dp[len1] = true;
		}

		for (int len2 = 1; len2 <= s2.Length; ++len2) {
			dp[0] &= s2[len2 - 1] == s3[len2 - 1];

			for (int len1 = 1; len1 <= s1.Length; ++len1) {
				dp[len1] =
					dp[len1 - 1] && s1[len1 - 1] == s3[len1 + len2 - 1]
					|| dp[len1] && s2[len2 - 1] == s3[len1 + len2 - 1];
			}
		}

		return dp[s1.Length];
	}
}
