using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Diagnostics;
using System.Numerics;

namespace RSAAnalyzer
{
	public partial class Form1 : Form
	{
		PrimeDatabase db;
		List<BigInteger> primes;

		public Form1()
		{
			InitializeComponent();

			Control.CheckForIllegalCrossThreadCalls = false;

			cartesianChart1.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.Both;

			SetNumerics(numericUpDown1, numericUpDown2, numericUpDown3);

			db = new PrimeDatabase();
			db.LoadDatabase();
			primes = db.ToList();
		}

		private void SetNumerics(params NumericUpDown[] numerics)
		{
			foreach (var numeric in numerics)
			{
				numeric.Minimum = 0;
				numeric.Maximum = decimal.MaxValue;
			}
		}

		BigInteger EncryptIt(BigInteger x, out bool anyProblem)
		{
			BigInteger enc = BigInteger.Pow(x, (int)e) % n;
			BigInteger dec = BigInteger.Pow(enc, (int)d) % n;

			var problem = dec != x;
			if (problem)
			{
				richTextBox1.AppendText($"Þifreleme Sorunu Algýlandý !{Environment.NewLine}");
			}

			anyProblem = dec != x;
			return enc;
		}

		BigInteger DecryptIt(BigInteger x_enc, BigInteger selfD)
		{
			return BigInteger.Pow(x_enc, (int)selfD) % n;
		}

		private async void button1_ClickAsync(object sender, EventArgs eventArgs)
		{
			await Task.Run(() => Work());
		}

		private CancellationTokenSource tokenSource;
		private bool Working = false;
		private Dictionary<BigInteger, List<BigInteger>> LastDecs = new();

		private void Work()
		{
			if (Working)
			{
				tokenSource.Cancel();
				return;
			}

			Working = true;
			tokenSource = new();
			richTextBox1.Clear();
			cartesianChart1.Series = new ISeries[] {};
			richTextBox1.AppendText($"p => {p} | q => {q} | n => {n} | fi => {fi} | e => {e} | d => {d} {Environment.NewLine}");

			Dictionary<BigInteger, List<BigInteger>> decs = new();

			CancellationToken token = tokenSource.Token;
			Stopwatch watch = new();

			BigInteger i = 2;
			BigInteger limit = 60;

			BigInteger dLimit = d + 1;


			while (i < limit && !token.IsCancellationRequested)
			{
				watch.Start();

				richTextBox1.AppendText($"Encryption Value {i} in {dLimit} {Environment.NewLine}");

				List<BigInteger> decryptors = new();

				BigInteger enc = EncryptIt(i, out var problem);

				if (problem)
				{
					richTextBox1.AppendText($"Encryption problem accoured, skipped {Environment.NewLine}");
					continue;
				}

				Parallel.For(2, (int)dLimit, (fakeD) =>
				{
					BigInteger x_dec_t = DecryptIt(enc, fakeD);

					if (x_dec_t == i)
					{
						richTextBox1.AppendText($"-Decrypter found {fakeD} {Environment.NewLine}");
						decryptors.Add(fakeD);
					}
				});

				decs.Add(i, decryptors);

				watch.Stop();

				this.Text = $"{d} processed in {watch.ElapsedMilliseconds}ms | {(double)d / watch.Elapsed.TotalSeconds} per second | {watch.ElapsedMilliseconds * (limit - i)}ms left | {i} / {limit}";

				watch.Reset();

				i++;
			}


			richTextBox1.AppendText($"Creating Graph {Environment.NewLine}");

			ScatterSeries<ObservablePoint> series = new()
			{
				Values = new List<ObservablePoint>()
			};

			foreach (var dec in decs)
			{
				foreach (var value in dec.Value)
				{
					series.Values.Add(new((double)dec.Key, (double)value));
				}
			}

			cartesianChart1.Series = new ISeries[]
			{
				series
			};

			using (var log = File.CreateText("log.txt"))
			{
				log.Write(richTextBox1.Text);
			}

			LastDecs = decs;
			Working = false;
		}

		private void SetRnd(NumericUpDown numeric)
		{
			numeric.Value = (long)primes[Random.Shared.Next(10, 100)];
		}

		private void button2_Click(object sender, EventArgs e)
		{
			SetRnd(numericUpDown1);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{
			SetRnd(numericUpDown2);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			numericUpDown3.Value = (decimal)primes.Where(y => fi > y).ToList()[Random.Shared.Next(10, 20)];
		}

		private BigInteger p = 2;
		private BigInteger q = 5;

		private BigInteger n;
		private BigInteger fi;
		private BigInteger e = 3;
		private BigInteger d;

		private void Reanalyze()
		{
			n = p * q;
			fi = (p - 1) * (q - 1);
			try
			{
				d = ModInverse(e, fi);
			}
			catch (Exception)
			{

			}

		}

		private BigInteger P
		{
			get
			{
				return p;
			}
			set
			{
				p = value;
				Reanalyze();
			}
		}
		private BigInteger Q
		{
			get
			{
				return q;
			}
			set
			{
				q = value;
				Reanalyze();
			}
		}

		private BigInteger E
		{
			get
			{
				return e;
			}
			set
			{
				e = value;
				Reanalyze();
			}
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (BigInteger.TryParse(numericUpDown1.Value.ToString(), out var val))
			{
				P = val;
			}
		}

		#region Encryption Algorithms
		static (BigInteger, BigInteger, BigInteger) Egcd(BigInteger a, BigInteger b)
		{
			if (a == 0)
			{
				return (b, 0, 1);
			}

			var (g, x1, y1) = Egcd(b % a, a);
			BigInteger x = y1 - (b / a) * x1;
			BigInteger y = x1;
			return (g, x, y);
		}

		// Modüler ters hesaplama
		static BigInteger ModInverse(BigInteger e, BigInteger k)
		{
			var (g, x, y) = Egcd(e, k);

			// Eðer e ve k birbirine asal deðilse, modüler ters yoktur.
			if (g != 1)
			{
				throw new Exception($"Modüler ters yok: {e} ve {k} birbirine asal deðil.");
			}

			// x mod k, negatifse pozitif yapmak için k ekleyelim
			return (x % k + k) % k;
		}
		#endregion

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			if (BigInteger.TryParse(numericUpDown2.Value.ToString(), out var val))
			{
				Q = val;
			}
		}

		private void numericUpDown3_ValueChanged(object sender, EventArgs eventArgs)
		{
			if (BigInteger.TryParse(numericUpDown3.Value.ToString(), out var val))
			{
				E = val;
			}
		}

		private const string Splitter = ",";
		private const string OutputSplitter = ";";
		private const string DataFile = "analyze.rdb";

		private void button5_Click(object sender, EventArgs eventArgs)
		{
			StringWriter sw = new StringWriter();
			foreach(var dec in LastDecs)
			{
				sw.Write(dec.Key);
				sw.Write(Splitter);
				sw.Write(e.ToString());
				sw.Write(Splitter);
				sw.Write(n.ToString());
				sw.Write(OutputSplitter);
				bool first = true;
				foreach(var value in dec.Value)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						sw.Write(Splitter);
					}
					sw.Write(value);
					
				}
				sw.Write(Environment.NewLine);
			}

			if (!File.Exists(DataFile))
			{
				File.CreateText(DataFile).Dispose();
				//File.AppendAllText(DataFile, $"[raw,e,n]{Environment.NewLine}");
			}
			File.AppendAllText(DataFile, sw.ToString());
		}
	}
}
