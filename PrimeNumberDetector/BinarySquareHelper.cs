// See https://aka.ms/new-console-template for more information
using System.Numerics;

public class BinarySquareHelper : ISquareHelper
{
	public BigInteger Square(BigInteger n)
	{
		if (n < 0)
			throw new ArgumentException("Square can't be calculated for negative numbers !");

		if (n == 0 || n == 1)
			return n;

		BigInteger left = 1;
		BigInteger right = n;
		BigInteger result = 0;

		while (left <= right)
		{
			BigInteger mid = (left + right) / 2;
			BigInteger midSquared = mid * mid;

			if (midSquared == n)
				return mid;
			else if (midSquared < n)
			{
				left = mid + 1;
				result = mid;
			}
			else
			{
				right = mid - 1;
			}
		}

		return result;
	}
}
