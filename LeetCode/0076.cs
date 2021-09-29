using System;
using System.Collections.Generic;

public class Solution {
	public string MinWindow(string s, string t) {
		var dt = new Dictionary<char, int>();
		foreach (char c in t) {
			dt.TryGetValue(c, out int n);
			dt[c] = n + 1;
		}

		int minWindowStart = -1;
		int minWindowLength = Int32.MaxValue;

		int l = 0;
		int r = 0;
		int deficit = dt.Count;
		for (; r < s.Length; ++r) {
			char c = s[r];
			if (dt.TryGetValue(c, out int n)) {
				dt[c] = n - 1;

				if (n == 1) {
					--deficit;

					while (deficit == 0) {
						int windowLength = r - l + 1;
						if (windowLength < minWindowLength) {
							minWindowStart = l;
							minWindowLength = windowLength;
						}

						c = s[l++];
						if (dt.TryGetValue(c, out n)) {
							dt[c] = n + 1;

							if (n == 0) {
								++deficit;
							}
						}
					}
				}
			}
		}

		return minWindowStart == -1 ? "" : s.Substring(minWindowStart, minWindowLength);
	}
}
