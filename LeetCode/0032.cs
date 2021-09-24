using System;
using System.Collections.Generic;
using System.Linq;

public class Solution {
	public int LongestValidParentheses(string s) {
		int max = 0;

		foreach (bool reversed in new[] { false, true }) {
			int left;
			IEnumerable<int> indexes = Enumerable.Range(0, s.Length);
			char up;

			if (!reversed) {
				left = -1;
				up = '(';
			} else {
				left = s.Length;
				indexes = indexes.Reverse();
				up = ')';
			}

			int height = 0;
			foreach (int i in indexes) {
				char c = s[i];
				if (c == up) {
					++height;
				} else {
					if (height == 0) {
						left = i;
					} else {
						--height;

						if (height == 0) {
							int right = i;
							max = Math.Max(max, Math.Abs(left - right));
						}
					}
				}
			}
		}

		return max;
	}
}
