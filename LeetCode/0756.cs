using System.Collections.Generic;

public class Solution {
	public bool PyramidTransition(string bottom, IList<string> allowed) {
		var dicAllowed = new Dictionary<string, List<char>>();
		foreach (string allowedElem in allowed) {
			string key = allowedElem.Substring(0, 2);

			List<char> outgoing;
			if (!dicAllowed.TryGetValue(key, out outgoing)) {
				outgoing = new List<char>();
				dicAllowed.Add(key, outgoing);
			}

			outgoing.Add(allowedElem[2]);
		}

		var stack = new Stack<(string, string)>();
		stack.Push((bottom, ""));

		var visited = new HashSet<string>();

		while (stack.Count > 0) {
			(string curBottom, string nextBottom) = stack.Pop();

			if (curBottom.Length == 1) {
				return true;
			} else if (nextBottom.Length == curBottom.Length - 1) {
				if (visited.Add(nextBottom)) {
					stack.Push((nextBottom, ""));
				}
			} else {
				string key = curBottom.Substring(nextBottom.Length, 2);
				if (dicAllowed.TryGetValue(key, out List<char> cs)) {
					foreach (char c in cs) {
						stack.Push((curBottom, nextBottom + c));
					}
				}
			}
		}

		return false;
	}
}
