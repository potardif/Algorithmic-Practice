using System.Linq;

public class Solution {
	public int Trap(int[] height) {
		int n = height.Length;
		int drainedArea = 0;

		int curHeight = 0;
		int leftmostWall = 0; // index
		while (true) {
			for (; leftmostWall < n; ++leftmostWall) {
				if (height[leftmostWall] > curHeight) {
					break;
				}
			}

			if (leftmostWall == n) {
				break;
			}

			drainedArea += leftmostWall * (height[leftmostWall] - curHeight);
			curHeight = height[leftmostWall];
		}

		curHeight = 0;
		int rightmostWall = n - 1; // index
		while (true) {
			for (; rightmostWall >= 0; --rightmostWall) {
				if (height[rightmostWall] > curHeight) {
					break;
				}
			}

			if (rightmostWall == -1) {
				break;
			}

			drainedArea += (n - 1 - rightmostWall) * (height[rightmostWall] - curHeight);
			curHeight = height[rightmostWall];
		}

		return curHeight * n - drainedArea - height.Sum();
	}
}
