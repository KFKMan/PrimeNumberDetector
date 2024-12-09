#if DEBUG
#define PERF
#endif

// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;

Stopwatch watch = new Stopwatch();
watch.Start();
PrimeDatabase context = new PrimeDatabase();
context.LoadDatabase();

CancellationTokenSource source = new();
CancellationToken token = source.Token;
Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);

void CancelKeyPressHandler(object? sender, ConsoleCancelEventArgs e)
{
	source.Cancel();
	// Burada verilerinizi kaydetme işlemini gerçekleştirin
	
	// Uygulamanın kapanmasını engellemek için:
	e.Cancel = true; // Bu false olursa, program Ctrl+C sonrası kapanır
}

ISquareHelper SquareHelper = new NxSquareHelper();

bool IsPrime(BigInteger number)
{
	/* çift sayıları sistematik olarak döngü de geçtik
	if(number % 2 == 0)
	{
		return false;
	}
	*/

	var max = SquareHelper.Square(number);
	Console.WriteLine($"{number} will check {max} value | +2 for anti error will be added");

	max += 2;

#if PERF
	Stopwatch watch = new Stopwatch();
	watch.Start();
#endif

	/*
	var primeNumbers = context.PrimeNumbers.AsNoTracking().OrderBy(x => x.Id).Select(x => x.Number).ToList();

	var lastIndex = primeNumbers.BinarySearch(max);

	if(lastIndex < 0)
	{
		lastIndex = -lastIndex;
	}

	var targetPrimeNumbers = primeNumbers.Take(lastIndex);
	*/

	//var targetPrimeNumbers = context.PrimeNumbers.AsNoTracking().Select(x => x.Number).Where(x => x < max).ToList();
	var targetPrimeNumbers = context.TakeWhileBigger(max);

#if PERF
	watch.Stop();
	Console.WriteLine($"Veritabanı işlemi {watch.ElapsedTicks} tick sürdü | {targetPrimeNumbers.Count} kadar alt asal sayı bulundu");
#endif
	
	foreach(var prime in targetPrimeNumbers)
	{
#if DEBUG
		Console.WriteLine($"Checking {prime}");
#endif
		if(number % prime == 0)
		{
			return false;
		}
	}

	/*
	BigInteger i = 3;
	while(max > i)
	{
		if(number % i == 0)
		{
			return false;
		}
		i += 2;
	}
	*/

	return true;
}

bool GetInputResult()
{
	string? f = Console.ReadLine();
	return f != null && f.ReplaceLineEndings().Replace(" ", "").ToLower() == "e";
}

Console.WriteLine("Veritabanını boşaltayım mı? (E/H)");
if(GetInputResult())
{
	context.Clear();
}

var entityCount = context.Count();
Console.WriteLine($"{entityCount} adet asal sayı veritabanında bulunuyor");

BigInteger start = 45;
if (entityCount == 0)
{
	context.Add(2);
	context.Add(3);
	context.Add(5);
	context.Add(7);
	context.Add(11);
	context.Add(13);
	context.Add(17);
	context.Add(19);
	context.Add(23);
	context.Add(29);
	context.Add(31);
	context.Add(37);
	context.Add(41);
	context.Add(43);
}
else
{
	var mostBigPrime = context.Last(); //context.PrimeNumbers.AsNoTracking().Select(x => x.Number).ToList().OrderByDescending(x => x).First();
	start = mostBigPrime + 2; //+ 2 ile aynı veriyi tekrar işlememiş oluyoruz
}

BigInteger _start = start;



//Console.WriteLine($"Started from {start.ToString().Length} lenght number");

while (!token.IsCancellationRequested)
{
	if (IsPrime(start))
	{
		Console.WriteLine($"The Number Is Prime");
		context.Add(start);
	}
	else
	{
		Console.WriteLine("The Number Is Not Prime");
	}
	
	start += 2; //Çift sayıları direkt geçmek için
}

context.SaveDatabase();

watch.Stop();
Console.WriteLine($"{start - _start} işlenen sayı sayısı | {context.Count()} toplam asal sayısı | {watch.Elapsed.Days} gün {watch.Elapsed.Hours} saat {watch.Elapsed.Seconds} saniye görev yapıldı");
Console.Write("Okunabilir çıktı ister misin? (E/H): ");
if (GetInputResult())
{
	context.SaveDatabaseReadable();
}