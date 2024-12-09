#if DEBUG
#define PrimeLog
#endif

using System;
using System.Numerics;

//TODO: Convert Documentation, Input, Output To English
//TODO: Don't use DEBUG Directly

Console.WriteLine("Loading Requiretments");


Console.WriteLine("Square Helper Loading...");
var SquareHelper = new NxSquareHelper();
Console.WriteLine("Square Helper Loaded");

Console.WriteLine("Dynamic Square Helper Loading...");
var DynamicSquareHelper = new NxDynSquareHelper();
Console.WriteLine("Dynamic Square Helper Loaded");

Console.WriteLine("Low Dynamic Square Helper Loading...");
var LowDynamicSquareHelper = new NxLWDynSquareHelper();
Console.WriteLine("Low Dynamic Square Helper Loaded");

Console.WriteLine("Prime Database Loading...");
PrimeDatabase primeDatabase = new PrimeDatabase();
primeDatabase.LoadDatabase();

var primes = primeDatabase.ToList();
Console.WriteLine("Prime Database Loaded");

#if PrimeLog
Console.WriteLine($"{primeDatabase.Count()} number of prime loaded");
#endif

Console.WriteLine("Requiretments Loaded");


string? GetInput(string question)
{
	Console.Write(question);
	return Console.ReadLine();
}

int GetInteger(string question)
{
	var input = GetInput(question);
	if(int.TryParse(input, out var integer))
	{
		return integer;
	}
	return GetInteger(question);
}

BigInteger GetBigInteger(string question)
{
	var input = GetInput(question);
	if(BigInteger.TryParse(input, out var result))
	{
		return result;
	}
	return GetBigInteger(question);
}

double GetDouble(string question)
{
	var input = GetInput(question);
	if (double.TryParse(input, out var result))
	{
		return result;
	}
	return GetDouble(question);
}

bool GetBool(string question)
{
	var input = GetInput(question);
	if(bool.TryParse(input, out bool result))
	{
		return result;
	}
	if(input != null)
	{
		var f = input.Trim().ToLower();
		if(f == "y" || f == "e" || f == "evet" || f == "1")
		{
			return true;
		}
		else if(f == "n" || f == "h" || f == "hayır" || f == "hayir" || f == "0")
		{
			return false;
		}
	}
	return GetBool(question);
	

}

BigInteger ApproximateRoot(BigInteger N, int degree)
{
	if (N < 0) throw new ArgumentException("Number must be positive");
	if (degree <= 0) throw new ArgumentException("Degree must be positive");

	return LowDynamicSquareHelper.Square(N, degree);
}


(BigInteger[] coefficients, int degree, BigInteger root) SelectPolinom(BigInteger N)
{
	if(GetBool("Create polinom automatically: "))
	{
		var degree = GetInteger("Enter a degree for polinom (5 or 6 is ideal): "); //Polinom with degree 5/6 is ideal

		var r = ApproximateRoot(N, degree);

		BigInteger[] coefficients = new BigInteger[degree + 1];
		coefficients[degree] = 1; // Terim with most biggest degree (terim'in ingilizcesini yedim ehe)
		for (int i = degree - 1; i >= 0; i--)
		{
			coefficients[i] = -BigInteger.Pow(r, degree - i); // Creating multipliers of the terims (yine yedim ehe)
		}

		return (coefficients, degree, r);
	}
	else
	{
	//TODO: Manual Polinom Selection
		return (Array.Empty<BigInteger>(),1,1);
	}
}

BigInteger CalculatePolinomFor(BigInteger N, BigInteger[] polinom)
{
	BigInteger res = 0;
	for(int i = 0; i < polinom.Length; i++)
	{
		res += polinom[i] * BigInteger.Pow(N, i);
	}
	return res;
}

bool IsSmooth(BigInteger num, List<BigInteger> factorBase)
{
	foreach (var prime in factorBase)
	{
		while (num % prime == 0)
		{
			num /= prime;
		}
	}
	return num == 1; // Eğer kalan 1 ise, smooth number
}

List<BigInteger> IsSmoothDetailed(BigInteger num, List<BigInteger> factorBase)
{
	List<BigInteger> factors = new List<BigInteger>();
	foreach (var prime in factorBase)
	{
		while (num % prime == 0)
		{
			factors.Add(prime);
			num /= prime;
		}
	}
	return num == 1 ? factors : new List<BigInteger>(); // Eğer kalan 1 ise, smooth number
}

Dictionary<BigInteger, BigInteger> Sieve(BigInteger N, BigInteger root, BigInteger[] polinom, PrimeDatabase primeDatabase)
{
	//TODO: Auto/Manuel Range Selection
	BigInteger M = root;

	if(GetBool("Use Manual Range: "))
	{
		M = GetBigInteger("Input Range: ");
	}

	int FactorBaseCount = 1;
	if(GetBool("Use Manual Factor Base Count: "))
	{
		FactorBaseCount = GetInteger("Input Factor Base Count: ");
	}
	else
	{
		// c.{e ^^ [(logN.loglogN) ^^ 1/2]}
		double c = 0.5;
		if(GetBool("Do you want to set Automatic Factor Base Count Calculator Const (c): "))
		{
			c = GetDouble("Enter Automatic Factor Base Count Calculator Const (c): ");
		}

		double logN = BigInteger.Log(N);
		double loglogN = Math.Log(logN);

		double exponent = Math.Sqrt(logN * loglogN);

#warning it can be more good
		FactorBaseCount = (int)Math.Round(c * Math.Exp(exponent));
	}

	var factorBase = primes.Take((int)FactorBaseCount).ToList();

#warning maybe it can be optimized
	Dictionary<BigInteger,BigInteger> SieveTable = new Dictionary<BigInteger, BigInteger>();

	for (BigInteger x = -M; x <= M; x++)
	{
		BigInteger fx = CalculatePolinomFor(x, polinom);
		var absFx = BigInteger.Abs(fx);

		if (IsSmooth(absFx, factorBase)) // Mutlak değer IsSmooth içinde alınır
		{
			SieveTable[x] = absFx;
		}

		/*
		// Sayının asal tabandaki asallara bölünebilirliğini kontrol et
		List<long> factors = GetPrimeFactors(absFx, factorBase);

		// Eğer f(x) sadece asal tabandaki asallardan oluşuyorsa, bir "smooth number"
		if (factors.Sum(f => f) > 0 && IsSmooth(absFx, factorBase))
		{
			Console.WriteLine($"x = {x}, f(x) = {fx}, Faktörler: {string.Join(", ", factors)}");
		}
		*/
	}

	return SieveTable;

	/*
	int range = 20;
	for (int x = 1; x <= range; x++)
	{
		BigInteger fx = CalculatePolinomFor(x, polinom);

		Console.Write($"x = {x}, f(x) = {fx} -> ");

		// Asallarla bölünebilirliği kontrol et
		foreach (var prime in primeDatabase.ToList())
		{
			if (fx % prime == 0)
			{
				Console.Write(prime + " ");
			}
		}
		Console.WriteLine();
	}
	*/
}

void MatrixReduction()
{

}


var N = GetBigInteger("Please enter number for factorize: ");


Console.WriteLine("Selecting Polinom");
var polinom = SelectPolinom(N);
Console.WriteLine("Polinom Selected");

Console.WriteLine("Sieving...");
Sieve(N, polinom.root, polinom.coefficients, primeDatabase);
Console.WriteLine("Sieved");

Console.WriteLine("Matrix Reduction...");
MatrixReduction();
Console.WriteLine("Matrix Reduction Completed");

