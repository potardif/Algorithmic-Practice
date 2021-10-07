using System.Text;

public class Solution {
	public string ShortestPalindrome(string s) {
		int[] radiuses = ManachersAlgorithm(s);

		int n; // Longest initial palindrome
		for (int i = radiuses.Length - 1; ; --i) {
			int radius = radiuses[i];
			if (radius == i) {
				n = radius;
				break;
			}
		}

		int prefix = s.Length - n;
		var sb = new StringBuilder(prefix + s.Length);
		for (int i = s.Length - 1; i >= n; --i) {
			sb.Append(s[i]);
		}
		sb.Append(s);

		return sb.ToString();
	}

	/// https://en.wikipedia.org/wiki/Longest_palindromic_substring
	private int[] ManachersAlgorithm(string s) {
		var sb = new StringBuilder();
		char sep = '|';
		sb.Append(sep);
		foreach (char c in s) {
			sb.Append(c);
			sb.Append(sep);
		}
		string s2 = sb.ToString();

		var radiuses = new int[s2.Length / 2 + 1];

		int center = 0;
		int radius = 0;
		while (center < radiuses.Length) {
			while (true) {
				int candidateRadius = radius + 1;
				int l = center - candidateRadius;
				int r = center + candidateRadius;

				if (l >= 0 && r < s2.Length && s2[l] == s2[r]) {
					radius = candidateRadius;
				} else {
					break;
				}
			}
			radiuses[center] = radius;

			int oldCenter = center;
			int oldRadius = radius;
			++center;
			radius = 0;
			while (center <= oldCenter + oldRadius && center < radiuses.Length) {
				int mirroredCenter = oldCenter - (center - oldCenter);
				int maxMirroredRadius = oldCenter + oldRadius - center;

				if (radiuses[mirroredCenter] < maxMirroredRadius) {
					radiuses[center] = radiuses[mirroredCenter];
					++center;
				} else if (radiuses[mirroredCenter] > maxMirroredRadius) {
					radiuses[center] = maxMirroredRadius;
					++center;
				} else {
					radius = maxMirroredRadius;
					break;
				}
			}
		}

		return radiuses;
	}
}
