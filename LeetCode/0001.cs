using System.Collections.Generic;

public class Solution {
	public int[] TwoSum(int[] nums, int target) {
		var indexes = new Dictionary<int, int>(); // num -> index

		for (int i = 0; i < nums.Length; ++i) {
			int num1 = nums[i];
			int num2 = target - num1;

			if (indexes.TryGetValue(num2, out int j)) {
				return new int[] { i, j };
			} else {
				indexes[num1] = i;
			}
		}

		throw new Exception("No solution");
	}
}
