
// See https://aka.ms/new-console-template for more information
using System.Numerics;

public class NumberResult
{
	public NumberResult(int x, int y)
	{
		X = x;
		Y = y;
		var res = X * Y;

		if (Math.Log10(res) >= 1) //2 haneli
		{
			//15
			//25
			//30
			A_Handler = (res / 10); //1 - 2 - 3
			A_Sub = res % 10; //5 - 5 - 0
		}
		else
		{
			A_Handler = 0;
			A_Sub = res;
		}
	}


	public int X;
	public int Y;

	/// <summary>
	/// ikinci rakam (büyük)
	/// </summary>
	public int A_Handler;

	/// <summary>
	/// ilk rakam (küçük)
	/// </summary>
	public int A_Sub;

	public NumberResult AddHandler(int handler)
	{
		if(handler == 0)
		{
			return this;
		}

		var res = new NumberResult(X, Y);
		res.A_Handler += handler;
		return res;
	}
}

public class NumberWorker
{
	public NumberWorker(List<NumberResult> results,BigInteger targetNumber)
	{

	}
}

