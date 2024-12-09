using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Numerics;

Console.WriteLine("RSA Based Security Breaker V1.0");

string? GetInput(string question)
{
	Console.Write(question);
	return Console.ReadLine();
}

string GetInputNotEmpty(string question)
{
	var result = GetInput(question);

	if(result != null)
	{
		return result;
	}

	return GetInputNotEmpty(question);
}

var target = GetInputNotEmpty("Please Enter Target Website: ");

using (TcpClient client = new TcpClient(target, 443)) //443 https port
using (SslStream sslStream = new SslStream(client.GetStream(), false, null, null))
{
	// SSL handshake
	sslStream.AuthenticateAsClient(target);

	// Sertifikayı almak
	X509Certificate2 cert = new X509Certificate2(sslStream.RemoteCertificate);

	// Sertifikadaki açık anahtarı almak
	RSA? rsa = cert.GetRSAPublicKey();
	if (rsa != null)
	{
		// RSA parametrelerini almak
		RSAParameters rsaParams = rsa.ExportParameters(false);

		Console.WriteLine($"Modulus (n): {Convert.ToBase64String(rsaParams.Modulus)}");
		Console.WriteLine($"Exponent (e): {Convert.ToBase64String(rsaParams.Exponent)}");

		BigInteger n = new BigInteger(rsaParams.Modulus);
		BigInteger e = new BigInteger(rsaParams.Exponent);

		Console.WriteLine($"Modulus (n): {n}");
		Console.WriteLine($"Exponent (e): {e}");

		Console.WriteLine($"Modulus (n) Lenght: {n.GetBitLength()} bit | {n.ToString().Length} character");
		Console.WriteLine($"Modulus (e) Lenght: {e.GetBitLength()} bit | {e.ToString().Length} character");

		int e_int = (int)e;

		CancellationTokenSource tokenSource = new CancellationTokenSource();
		var token = tokenSource.Token;

		BigInteger EncryptIt(BigInteger x)
		{
			return BigInteger.Pow(x, e_int) % n;
		}

		BigInteger DecryptIt(BigInteger x_enc,int d)
		{
			return BigInteger.Pow(x_enc, d) % n;
		}

		BigInteger DecryptItOptimized(BigInteger m,int d)
		{
			//x_enc * d % n == (x_enc % n) * d % n

			//BigInteger m = x_enc % n;
			return BigInteger.Pow(m, d) % n;
		}

		int totaldec = 0;

		DateTime dt = DateTime.Now;
		var timer = new System.Timers.Timer();
		timer.Interval = 1000;
		timer.Elapsed += (sender, e) =>
		{
			Console.Title = $"{(DateTime.Now - dt).TotalSeconds.ToString()} - {totaldec} Dec Found";
		};
		timer.Start();

		const string StepSave = "steps.rdb";

		if (!File.Exists(StepSave))
		{
			File.CreateText(StepSave);
			File.AppendAllText(StepSave, $"{target}{Environment.NewLine}");
		}

		//Start - Stop (Stop exclusive)
		void Run(int x, int start, int stop)
		{
			void PrintLn(string text)
			{
				Console.WriteLine($"{x} | {text}");
			}

			PrintLn($"Job Started {start}-{stop}");

			void SaveDec(int dec)
			{
				const string SaveFile = "decs.rdb";

				if (!File.Exists(SaveFile))
				{
					File.CreateText(SaveFile).Dispose();
				}

				File.AppendAllText(SaveFile, $"{x}-{dec}");
			}

			

			var x_enc = EncryptIt(x);
			var m = x_enc % n;

			int i = start;
			while(stop > i)
			{
				PrintLn(i.ToString());

				var x_dec = DecryptItOptimized(m, i);

				if(x_dec == x)
				{
					PrintLn($"New Dec Found {i}");
					SaveDec(i);
					totaldec += 1;
				}

				i++;
			}

			File.AppendAllText(StepSave, $"{start}-{stop}{Environment.NewLine}");
		}

		int Fi = 8;
		/*
		int tx = Random.Shared.Next(e_int / 4, e_int / 2);
		if(Random.Shared.Next(0,1) == 0)
		{
			Fi = e_int + tx;
		}
		else
		{
			Fi = e_int - tx;
		} */

		const int Page = 500;
		const int Core = 10;

		int i = 2;

		var stepData = File.ReadAllLines(StepSave);
		if (stepData.Length > 2)
		{
			foreach (var line in File.ReadAllLines(StepSave).Skip(1))
			{
				if(!string.IsNullOrWhiteSpace(line) && !string.IsNullOrEmpty(line))
				{
					var numbers = line.Split("-").Select(x => int.Parse(x)).ToArray();
					var max = numbers[1];

					if(max > i)
					{
						i = max;
					}
				}
			}
		}

		var semaphore = new SemaphoreSlim(Core);
		var tasks = new List<Task>();

		
		while (true)
		{
			semaphore.Wait();

			int z = i; //Blocking "i" change

			var task = Task.Run(() =>
			{
				Console.WriteLine($"Job started.");
				Run(Fi, z, z + Page);
				Console.WriteLine($"Job completed.");
				semaphore.Release(); //Release slot
			});
			tasks.Add(task);

			i += Page;
		}

		Task.WaitAll(tasks.ToArray());

		/*
		Parallel.For(e_int - 1, e_int + 1, (x) =>
		{
			void PrintLn(string text)
			{
				Console.WriteLine($"{x} | {text}");
			}

			var x_enc = EncryptIt(x);
			var m = x_enc % n;

			List<int> decs = new();

			int i = 2;
			while (n >= i && !token.IsCancellationRequested)
			{
				PrintLn($"Trying {i}");

				var x_dec = DecryptItOptimized(m, i);
				
				if(x_dec == x)
				{
					PrintLn($"New Dec Found {i}");
					decs.Add(i);
					totaldec += 1;

					if(decs.Count > 1)
					{
						tokenSource.Cancel();
						PrintLn("Search Finished 2 or more dec found");
						PrintLn(string.Join(' ', decs.Select(x => x.ToString())));
					}
				}
				i++;
			}
		});
		*/

	}
	else
	{
		Console.WriteLine("RSA public key not found");
	}
}

Console.WriteLine("Press any key for exit");
Console.ReadLine();
