// See https://aka.ms/new-console-template for more information
using System.Numerics;

public interface ISquareHelper
{
	BigInteger Square(BigInteger value);
}

public interface IDynamicSquareHelper
{
	BigInteger Square(BigInteger value, int degree);
}
