// See https://aka.ms/new-console-template for more information
using Choi.ByteBuffer;
using System.Numerics;

public class PrimeDatabase
{
	private List<BigInteger> Primes = new();
	private string DbFile;

	public PrimeDatabase(string file = "Database.db")
	{
		DbFile = file;
	}

	public void Add(BigInteger value)
	{
		Primes.Add(value);
	}

	public BigInteger Last()
	{
		return Primes.Last();
	}

	public int Count()
	{
		return Primes.Count;
	}

	public void Clear()
	{
		Primes.Clear();
	}

	public List<BigInteger> ToList()
	{
		return Primes.ToList();
	}

	private BigInteger Cache = 0;
	private List<BigInteger> CachePrimes = new();

	public List<BigInteger> TakeWhileBigger(BigInteger number)
	{
		if(Cache == number)
		{
			return CachePrimes;
		}

		List<BigInteger> primes = new();
		foreach(var prime in Primes)
		{
			if(number < prime)
			{
				break;
			}
			primes.Add(prime);
		}

		Cache = number;
		CachePrimes = primes;

		return TakeWhileBigger(number);
	}

	/*
	public IEnumerable<BigInteger> TakeWhileBigger(BigInteger number)
	{
		foreach (var prime in Primes)
		{
			if(number > prime)
			{
				yield return number;
			}
			else
			{
				yield break;
			}
		}
	}
	*/

	public void LoadDatabase()
	{
		Primes.Clear();

		if(!File.Exists(DbFile) || new FileInfo(DbFile).Length == 0)
		{
			return;
		}
		using(MemoryStream stream = new MemoryStream(File.ReadAllBytes(DbFile)))
		{
			using(StreamByteBuffer buffer = new StreamByteBuffer(stream))
			{
				int c = buffer.Get<int>();
				int i = 0;
				while(c > i)
				{
					int leng = buffer.Get<int>();
					var data = buffer.Get(size: leng);

					Primes.Add(new BigInteger(data));

					i++;
				}
			}
		}
	}

	public void SaveDatabase()
	{
		using (var buffer = new InMemoryByteBuffer())
		{
			buffer.Put((int)Primes.Count);
			foreach(var prime in Primes)
			{
				var data = prime.ToByteArray();

				buffer.Put((int)data.Length);
				buffer.Put((byte[])data);
			}

			File.Create(DbFile).Dispose();
			File.WriteAllBytes(DbFile,buffer.ToArray());
		}
	}

	public void SaveDatabaseReadable(string filepath = "Database.rdb")
	{
		StringWriter writer = new StringWriter();
		foreach(var prime in Primes)
		{
			writer.WriteLine(prime.ToString());
		}

		using(var file = File.CreateText(filepath)){
			file.Write(writer.ToString());
		}
	}
}
