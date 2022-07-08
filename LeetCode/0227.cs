public class Solution {
	public int Calculate(string s) {
		List<object> tokens = Tokenize(s);
		return ParseAndEvaluate(tokens);
	}

	// Lexer
	private List<object> Tokenize(string s) {
		var tokens = new List<object>();

		int i = 0;
		while (i < s.Length) {
			char c = s[i];

			if (c == '+' || c == '-' || c == '*' || c == '/') {
				tokens.Add(c);
			} else if (char.IsDigit(c)) {
				int n = 0;
				while (i < s.Length && char.IsDigit(s[i])) {
					n *= 10;
					n += int.Parse(s[i].ToString());
					++i;
				}

				tokens.Add(n);
				--i;
			} else if (c != ' ') {
				throw new NotImplementedException($"{c}");
			}

			++i;
		}

		return tokens;
	}

	// https://en.wikipedia.org/wiki/Shunting_yard_algorithm
	private int ParseAndEvaluate(List<object> tokens) {
		var outputQueue = new Stack<int>();
		var operatorStack = new Stack<char>();

		foreach (object token in tokens) {
			if (token is int number) {
				outputQueue.Push(number);
			} else if (token is char o1) {
				while (
					operatorStack.TryPeek(out char o2)
					&& Priority(o2) >= Priority(o1)
				) {
					Evaluate(outputQueue, operatorStack);
				}

				operatorStack.Push(o1);
			} else {
				throw new NotImplementedException($"{token.GetType()}");
			}
		}

		while (operatorStack.Count > 0) {
			Evaluate(outputQueue, operatorStack);
		}

		return outputQueue.Pop();
	}

	private void Evaluate(Stack<int> outputQueue, Stack<char> operatorStack) {
		char op = operatorStack.Pop();

		int r = outputQueue.Pop();
		int l = outputQueue.Pop();

		int result = op switch {
			'+' => l + r,
			'-' => l - r,
			'*' => l * r,
			'/' => l / r,
			_ => throw new NotImplementedException($"{op}"),
		};

		outputQueue.Push(result);
	}

	private int Priority(char op) {
		return op switch {
			'+' => 0,
			'-' => 0,
			'*' => 1,
			'/' => 1,
			_ => throw new NotImplementedException($"{op}"),
		};
	}
}
