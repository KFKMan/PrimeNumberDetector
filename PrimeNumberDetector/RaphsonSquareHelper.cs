// See https://aka.ms/new-console-template for more information
using System.Numerics;

[Obsolete("It's unsafe (accuracy can be broken) and slow, instead of this use NxSquareHelper")]
public class RaphsonSquareHelper : ISquareHelper
{
	public BigInteger Square(BigInteger n)
	{
		if (n < 0)
			throw new ArgumentOutOfRangeException("Square can't be calculated for negative numbers !");

		if (n == 0 || n == 1)
			return n;

		BigInteger x = n >> 1; //it's equal to n/2
		BigInteger lastX = 0;

		// Newton-Raphson iteration
		while (x != lastX)
		{
			lastX = x;
			x = (x + n / x) >> 1;
		}

		return x;
	}
}
