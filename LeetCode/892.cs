public class Solution {
	public int SurfaceArea(int[][] grid) {
		int n = grid.Length;
		
		int surface = 0;
		for (int i = 0; i < n; ++i) {
			for (int j = 0; j < n; ++j) {
				int v = grid[i][j];
				if (v != 0) {
					surface += 4 * v + 2;
				}
			}
		}

		for (int i = 0; i < n; ++i) {
			for (int j1 = 0, j2 = 1; j2 < n; ++j1, ++j2) {
				int v1 = grid[i][j1];
				int v2 = grid[i][j2];
				surface -= 2 * Math.Min(v1, v2);
			}
		}

		for (int i1 = 0, i2 = 1; i2 < n; ++i1, ++i2) {
			for (int j = 0; j < n; ++j) {
				int v1 = grid[i1][j];
				int v2 = grid[i2][j];
				surface -= 2 * Math.Min(v1, v2);
			}
		}

		return surface;
	}
}
