public class Solution {
	public bool ValidPalindrome(string s) {
		return IsPalindrome(s, 0, s.Length - 1, out int i, out int j)
			|| IsPalindrome(s, i + 1, j, out _, out _)
			|| IsPalindrome(s, i, j - 1, out _, out _);
	}

	private bool IsPalindrome(string s, int i, int j, out int i2, out int j2) {
		while (i < j) {
			if (s[i] != s[j]) {
				i2 = i;
				j2 = j;
				return false;
			} else {
				++i;
				--j;
			}
		}

		i2 = i;
		j2 = j;
		return true;
	}
}
