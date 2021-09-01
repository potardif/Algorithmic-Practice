using System.Collections.Generic;

public class Solution {
	public IList<IList<int>> CombinationSum2(int[] candidates, int target) {
		Array.Sort(candidates);

		var stack = new Stack<(List<int> combination, int i, int target)>();
		stack.Push((new List<int>(), 0, target));

		var combinations = new List<IList<int>>();
		while (stack.Count > 0) {
			var case_ = stack.Pop();

			if (case_.target < 0) {
				// No solution
			} else if (case_.target == 0) {
				combinations.Add(case_.combination);
			} else {
				int? prevCandidate = null;
				for (int i = case_.i; i < candidates.Length; ++i) {
					int candidate = candidates[i];
					if (prevCandidate == null || candidate > prevCandidate) {
						var combination = new List<int>(case_.combination) { candidate };
						stack.Push((combination, i + 1, case_.target - candidate));
						prevCandidate = candidate;
					}
				}
			}
		}

		return combinations;
	}
}
