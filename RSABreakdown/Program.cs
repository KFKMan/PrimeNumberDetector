// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

using(var file = File.OpenText("analyze.rdb"))
{
	int max_total = 0;

	var line = file.ReadLine();
	while (line != null)
	{
		string[] p = line.Split(';');

		Console.WriteLine($"Processing => {line}");

		if(p.Length > 1 && !string.IsNullOrEmpty(p[1].Trim())){
			var variables = p[0].Split(",").Select(x => int.Parse(x)).ToArray();
			var decs = p[1].Split(",").Select(x => int.Parse(x)).OrderBy(y => y).ToArray();

			if(decs.Length > 2)
			{
				int raw = variables[0];
				int e = variables[1];
				int n = variables[2];

				Console.WriteLine($"raw {raw} | e {e} | n {n}");

				var decs_str = string.Join(' ', decs.Select(x => x.ToString()));
				Console.WriteLine(decs_str);

				List<int> diffs = new();

				int i = 0;
				while (decs.Length - 1 > i)
				{
					var current = decs[i];
					var next = decs[i + 1];

					var diff = next - current;

					diffs.Add(diff);

					Console.WriteLine(diff);

					i++;
				}

				var total = diffs.Sum(x => x);
				var average = total / diffs.Count;

				Console.WriteLine($"Total {total} | Average {average} | Count {diffs.Count}");

				int error = 0;

				foreach (var diff in diffs)
				{
					var err = Math.Abs(100 - ((diff * 100 / average)));
					if (err > error)
					{
						error = err;
					}
				}

				Console.WriteLine($"Max Error Percent %{error} | Max Numeric Error {average * error}");

				if(error > max_total)
				{
					max_total = error;
				}
			}
		}

		line = file.ReadLine();
	}

	Console.WriteLine($"Most Big Error Percent %{max_total}");
}


Console.ReadLine();