using System.Linq;

public class Solution {
	public string LongestCommonPrefix(string[] strs) {
		int maxPrefixLength = strs.Min(str => str.Length);

		int i = 0;
		for (; i < maxPrefixLength; ++i) {
			char c = strs[0][i];
			if (strs.Skip(1).Any(str => str[i] != c)) {
				break;
			}
		}

		return strs[0].Substring(0, i);
	}
}
