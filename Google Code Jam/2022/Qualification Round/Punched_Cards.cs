using System;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());

		for (int x = 1; x <= T; ++x) {
			string[] tokens = Console.ReadLine().Split(" ");
			int R = int.Parse(tokens[0]);
			int C = int.Parse(tokens[1]);

			Console.WriteLine($"Case #{x}:");
			PrintPunchCard(R, C);
		}
	}

	private static void PrintPunchCard(int R, int C) {
		for (int row = 0; row < 2 * R + 1; ++row) {
			for (int col = 0; col < 2 * C + 1; ++col) {
				char c = GetChar(row, col);
				Console.Write(c);
			}
			Console.WriteLine();
		}
	}

	private static char GetChar(int row, int col) {
		if (row <= 1 && col <= 1) return '.';

		if (row % 2 == 0) {
			if (col % 2 == 0) return '+';
			else return '-';
		} else {
			if (col % 2 == 0) return '|';
			else return '.';
		}
	}
}
