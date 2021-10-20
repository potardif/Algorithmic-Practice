using System;

public class Solution {
	public int PartitionDisjoint(int[] nums) {
		int n = nums.Length;

		int leftLength = 1;
		int max = nums[0];
		int possibleMax = max;

		for (int i = 1; i < n - 1; ++i) {
			if (nums[i] < max) {
				leftLength = i + 1;
				max = possibleMax;
			} else {
				possibleMax = Math.Max(possibleMax, nums[i]);
			}
		}

		return leftLength;
	}
}
