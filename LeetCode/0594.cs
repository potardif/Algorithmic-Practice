using System;
using System.Collections.Generic;

public class Solution {
	public int FindLHS(int[] nums) {
		var counts = new Dictionary<int, int>();
		foreach (int num in nums) {
			counts.TryGetValue(num, out int count);
			counts[num] = count + 1;
		}

		int max = 0;
		foreach (KeyValuePair<int, int> kvp in counts) {
			int num1 = kvp.Key;
			int count1 = kvp.Value;

			int num2 = num1 + 1;
			if (counts.TryGetValue(num2, out int count2)) {
				max = Math.Max(max, count1 + count2);
			}
		}

		return max;
	}
}
