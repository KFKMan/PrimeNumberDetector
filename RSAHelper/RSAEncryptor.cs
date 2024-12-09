using System.Drawing;
using System.Numerics;

namespace RSAHelper
{
	public class RSAEncryptor
	{
		public RSAEncryptor(int e, BigInteger n)
		{
			E = e;
			N = n;
		}

		public int E;
		public BigInteger N;

		public BigInteger EncryptIt(BigInteger raw)
		{ 
			return BigInteger.Pow(raw, E) % N;
		}
	}
}
