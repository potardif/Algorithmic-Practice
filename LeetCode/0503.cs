using System;
using System.Collections.Generic;

public class Solution {
	public int[] NextGreaterElements(int[] nums) {
		int n = nums.Length;
		var result = new int[n];
		Array.Fill(result, -1);

		var stack = new Stack<int>();
		stack.Push(0);

		for (int i = 1; i <= 2 * n; ++i) {
			int iModN = i % n;

			while (stack.Count > 0 && nums[stack.Peek()] < nums[iModN]) {
				result[stack.Pop()] = nums[iModN];
			}

			stack.Push(iModN);
		}

		return result;
	}
}
