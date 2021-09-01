public class Solution {
	public int MyAtoi(string s) {
		int i = 0;

		for (; i < s.Length && s[i] == ' '; ++i);

		bool negative = false;
		if (i < s.Length) {
			switch (s[i]) {
			case '+':
				++i;
				break;
			case '-':
				negative = true;
				++i;
				break;
			}
		}

		long result = 0;

		long maxValue = negative ? -(long)Int32.MinValue : (long)Int32.MaxValue;
		for (; i < s.Length; ++i) {
			char c = s[i];
			if ('0' <= c && c <= '9') {
				int digit = c - '0';
				result = result * 10 + digit;
				if (result > maxValue) {
					result = maxValue;
					break;
				}
			} else {
				break;
			}
		}

		return negative ? (int)-result : (int)result;
	}
}
