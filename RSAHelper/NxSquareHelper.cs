// See https://aka.ms/new-console-template for more information
using System.Numerics;

public class NxLWDynSquareHelper
{
	public BigInteger Square(BigInteger n, int lw_d)
	{
		double logN = BigInteger.Log(n);
		double root = Math.Exp(logN / lw_d);

		BigInteger x = (BigInteger)root;
		BigInteger lastX;

		// Newton-Raphson iteration
		do
		{
			lastX = x;
			BigInteger numerator = BigInteger.Pow(x, lw_d - 1) * x - n; // x^d - N
			BigInteger denominator = lw_d * BigInteger.Pow(x, lw_d - 1);  // d * x^(d-1)
			x = x - numerator / denominator; // new res
		}
		while (x != lastX); // if no change exit

		return x;
	}
}

public class NxDynSquareHelper : IDynamicSquareHelper
{
	public BigInteger Square(BigInteger n, int d)
	{
		if (d <= 0) throw new ArgumentException("Degree must be positive");
		if (n < 0) throw new ArgumentException("Number must be positive");

		int Nx = (int)n.GetBitLength() / d;

		BigInteger x = n >> Nx;
		BigInteger lastX;

		// Newton-Raphson iteration
		do
		{
			lastX = x;
			BigInteger numerator = BigInteger.Pow(x, d - 1) * x - n; // x^d - N
			BigInteger denominator = d * BigInteger.Pow(x, d - 1);  // d * x^(d-1)
			x = x - numerator / denominator;
		}
		while (x != lastX); // if no change exit

		return x;
	}
}

public class NxSquareHelper : ISquareHelper
{
	public BigInteger Square(BigInteger n)
	{
		if (n < 0)
			throw new ArgumentOutOfRangeException("Square can't be calculated for negative numbers !");

		if (n == 0 || n == 1)
			return n;

		var Nx = n.GetBitLength() / 2;
		var INx = (int)Nx;
		if(Nx != INx)
		{
			Console.WriteLine("Square Root Warning Nx is not equal to Int Nx");
		}

		BigInteger x = n >> INx;
		BigInteger lastX;

		do
		{
			lastX = x;
			x = (x + n / x) >> 1;
		} while (x < lastX);

		if (x * x > n)
			x--;

		return x;
	}
}
