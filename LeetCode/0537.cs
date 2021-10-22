public class Solution {
	public string ComplexNumberMultiply(string num1, string num2) {
		(int r1, int i1) = ParseImaginaryNumber(num1);
		(int r2, int i2) = ParseImaginaryNumber(num2);

		int r3 = r1 * r2 - i1 * i2;
		int i3 = r1 * i2 + r2 * i1;
		return $"{r3}+{i3}i";
	}

	private (int, int) ParseImaginaryNumber(string s)
	{
		int i = s.IndexOf('+');
		int real = int.Parse(s.Substring(0, i));
		int imaginary = int.Parse(s.Substring(i + 1, s.Length - i - 2));
		return (real, imaginary);
	}
}
