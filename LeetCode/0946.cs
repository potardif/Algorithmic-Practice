using System.Collections.Generic;

public class Solution {
	public bool ValidateStackSequences(int[] pushed, int[] popped) {
		var stack = new Stack<int>();

		int j = 0;
		foreach (int push in pushed) {
			stack.Push(push);

			while (stack.Count > 0 && j < popped.Length && stack.Peek() == popped[j]) {
				stack.Pop();
				++j;
			}
		}

		return stack.Count == 0;
	}
}
