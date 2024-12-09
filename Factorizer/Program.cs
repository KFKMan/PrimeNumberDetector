#if DEBUG
#define Log
#endif

using System.Diagnostics;
using System.Numerics;

List<NumberResult> Results = new();

for(int i = 0; i<10; i++)
{
	for(int j = 0; j < 10; j++)
	{
		Results.Add(new NumberResult(i, j));
	}
}

string? GetInput(string question)
{
	Console.Write(question);
	return Console.ReadLine();
}

BigInteger GetBigInteger(string question)
{
	var str = GetInput(question);
	if(BigInteger.TryParse(str, out var result))
	{
		return result;
	}
	return GetBigInteger(question);
}

#if Log
Console.WriteLine($"Workers {string.Join("  ", Results.Select(x => $"{x.X}x{x.Y}={x.A_Handler}{x.A_Sub}"))}");
#endif

while (true) {
	var target = GetBigInteger("Please enter number for factorizing: ");

	List<int> GetCharactersOfNumber(BigInteger num)
	{
		List<int> chars = new List<int>();

		BigInteger f = num;

		while (f > 0)
		{
			int digit = (int)(f % 10); // Son haneyi al
			chars.Add(digit);
			f /= 10; // Sayıyı bir basamak küçült
		}

		return chars;
	}

	List<int> chars = GetCharactersOfNumber(target);

	Console.WriteLine($"Characters => {string.Join(" ", chars.Reverse<int>().Select(x => x.ToString()))}");

	//5 * 5 => 25
	//15 * 10 => 150

#warning it's must be have left - right in the same time

	List<BigInteger> Work(int Index, NumberResult lastRes, string number)
	{
#if Log
		Console.WriteLine($"Analyze for {number}");
#endif

		List<BigInteger> numbers = new List<BigInteger>();

		NumberResult last = lastRes;
		int k = Index;

		if (chars.Count <= k)
		{
			return new List<BigInteger> { BigInteger.Parse(number) };
		}

		var currentChar = chars[k];

		//var add_handler = 0;
		var req = currentChar - last.A_Handler; //9-0 => 9 | 9-1 => 8 | 1-4 => 7

		if (req < 0)
		{
			req += 10;
		}

		/*
		if (req == 0)
		{
			var res = Work(k + 1, new NumberResult(0, 0), number);
			numbers.AddRange(res);
		}
		else
		{
			var query = Results.Where(x => x.A_Sub == req);
			
			if(chars.Count == k - 1)
			{
				query = query.Where(x => x.A_Handler == 0);
			}

			foreach (var prob in query)
			{
				var res = Work(k + 1, prob, prob.X + number);
				numbers.AddRange(res);

				if (prob.X != prob.Y)
				{
					var res2 = Work(k + 1, prob, prob.Y + number);
					numbers.AddRange(res2);
				}
			}
		}*/

		var query = Results.Where(x => x.A_Sub == req);

		if (chars.Count == k - 1)
		{
			query = query.Where(x => x.A_Handler == 0);
		}

		foreach (var prob in query)
		{
			var res = Work(k + 1, prob, prob.X + number);
			numbers.AddRange(res);

			if (prob.X != prob.Y)
			{
				var res2 = Work(k + 1, prob, prob.Y + number);
				numbers.AddRange(res2);
			}
		}

		return numbers;
	}


	Stopwatch st = new Stopwatch();
	st.Start();

	var tg = Work(0, new NumberResult(0, 0), "");

	Console.WriteLine("Analyze 1 Finished");

	BigInteger div = 0;

#if Log
	Console.WriteLine($"Numbers {string.Join(" ", tg.OrderBy(tg => tg).Select(tg => tg.ToString()))}");
#endif

	foreach (var t in tg.OrderBy(k => k))
	{
#if Log
		Console.WriteLine($"Working for {t}");
#endif
		var f = target / t;
		if (t * f == target)
		{
			//Console.WriteLine($"{t} is a divider !");
			div = t;
			break;
		}
	}

	st.Stop();
	Console.WriteLine($"Done All In {st.Elapsed.TotalSeconds} Seconds");

	Console.WriteLine(div.ToString());
}





