namespace RSAAnalyzer
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			richTextBox1 = new RichTextBox();
			cartesianChart1 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
			button1 = new Button();
			numericUpDown1 = new NumericUpDown();
			numericUpDown2 = new NumericUpDown();
			button2 = new Button();
			button3 = new Button();
			button4 = new Button();
			numericUpDown3 = new NumericUpDown();
			button5 = new Button();
			((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
			SuspendLayout();
			// 
			// richTextBox1
			// 
			richTextBox1.Location = new Point(12, 12);
			richTextBox1.Name = "richTextBox1";
			richTextBox1.Size = new Size(414, 300);
			richTextBox1.TabIndex = 0;
			richTextBox1.Text = "";
			// 
			// cartesianChart1
			// 
			cartesianChart1.Location = new Point(432, 12);
			cartesianChart1.Name = "cartesianChart1";
			cartesianChart1.Size = new Size(772, 641);
			cartesianChart1.TabIndex = 1;
			// 
			// button1
			// 
			button1.Location = new Point(12, 619);
			button1.Name = "button1";
			button1.Size = new Size(414, 34);
			button1.TabIndex = 2;
			button1.Text = "Start";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_ClickAsync;
			// 
			// numericUpDown1
			// 
			numericUpDown1.Location = new Point(12, 318);
			numericUpDown1.Name = "numericUpDown1";
			numericUpDown1.Size = new Size(301, 31);
			numericUpDown1.TabIndex = 3;
			numericUpDown1.Value = new decimal(new int[] { 2, 0, 0, 0 });
			numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
			// 
			// numericUpDown2
			// 
			numericUpDown2.Location = new Point(12, 355);
			numericUpDown2.Name = "numericUpDown2";
			numericUpDown2.Size = new Size(301, 31);
			numericUpDown2.TabIndex = 4;
			numericUpDown2.Value = new decimal(new int[] { 7, 0, 0, 0 });
			numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
			// 
			// button2
			// 
			button2.Location = new Point(319, 318);
			button2.Name = "button2";
			button2.Size = new Size(112, 34);
			button2.TabIndex = 5;
			button2.Text = "Rnd";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new Point(319, 355);
			button3.Name = "button3";
			button3.Size = new Size(112, 34);
			button3.TabIndex = 6;
			button3.Text = "Rnd";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new Point(319, 392);
			button4.Name = "button4";
			button4.Size = new Size(112, 34);
			button4.TabIndex = 8;
			button4.Text = "Rnd";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// numericUpDown3
			// 
			numericUpDown3.Location = new Point(12, 392);
			numericUpDown3.Name = "numericUpDown3";
			numericUpDown3.Size = new Size(301, 31);
			numericUpDown3.TabIndex = 7;
			numericUpDown3.Value = new decimal(new int[] { 3, 0, 0, 0 });
			numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
			// 
			// button5
			// 
			button5.Location = new Point(12, 579);
			button5.Name = "button5";
			button5.Size = new Size(414, 34);
			button5.TabIndex = 9;
			button5.Text = "Save Result";
			button5.UseVisualStyleBackColor = true;
			button5.Click += button5_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1216, 665);
			Controls.Add(button5);
			Controls.Add(button4);
			Controls.Add(numericUpDown3);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(numericUpDown2);
			Controls.Add(numericUpDown1);
			Controls.Add(button1);
			Controls.Add(cartesianChart1);
			Controls.Add(richTextBox1);
			Name = "Form1";
			Text = "Form1";
			Load += Form1_Load;
			((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
			((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
			((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private RichTextBox richTextBox1;
		private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart1;
		private Button button1;
		private NumericUpDown numericUpDown1;
		private NumericUpDown numericUpDown2;
		private Button button2;
		private Button button3;
		private Button button4;
		private NumericUpDown numericUpDown3;
		private Button button5;
	}
}
