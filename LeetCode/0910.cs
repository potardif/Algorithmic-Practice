using System;

public class Solution {
	public int SmallestRangeII(int[] nums, int k) {
		Array.Sort(nums);

		for (int i = 0; i < nums.Length; ++i) {
			nums[i] -= k;
		}

		int min = nums[0];
		int max = nums[nums.Length - 1];
		int minScore = max - min;

		for (int i = 0; i < nums.Length - 1; ++i) {
			nums[i] += 2 * k;
			min = Math.Min(nums[0], nums[i + 1]);
			max = Math.Max(nums[i], nums[nums.Length - 1]);
			minScore = Math.Min(minScore, max - min);
		}
		return minScore;
	}
}
