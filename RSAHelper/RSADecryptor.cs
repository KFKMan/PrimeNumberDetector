using System.Numerics;

namespace RSAHelper
{
	public class RSADecryptor
	{
		public RSADecryptor(BigInteger EncryptedData, BigInteger n)
		{
			Enc = EncryptedData;
			N = n;
			M = Enc % n;
		}

		public BigInteger Enc;
		public BigInteger N;
		public BigInteger M;

		public BigInteger DecryptIt(BigInteger m, int d)
		{
			return BigInteger.Pow(m, d) % N;
		}

		public static BigInteger DecryptItNonConst(BigInteger x_enc, int d, BigInteger n)
		{
			return BigInteger.Pow(x_enc, d) % n;
		}
	}
}
