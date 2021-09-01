using System;

public class Solution {
	public int Reverse(int x) {
		int reversed = 0;

		while (x != 0) {
			x = Math.DivRem(x, 10, out int digit);

			try {
				reversed = checked(reversed * 10 + digit);
			} catch (OverflowException) {
				return 0;
			}
		}
		
		return reversed;
	}
}
