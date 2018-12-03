using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	static class Visual
	{
		public static void VisualCommandADD(string namefirst, string namesecond, int val1, int val2)
		{
			string first = Checker.GiveMeMyBinaryValue(val1);
			string second = Checker.GiveMeMyBinaryValue(val2);
			//Console.WriteLine(first + " " + second);
			string help = new string('0', Math.Abs(first.Length - second.Length));
			if (first.Length > second.Length)
				second = help + second;
			else
				first = help + first;
			help = new string('0', Math.Max(first.Length, second.Length) + 1);
			StringBuilder ans = new StringBuilder(help);

			int i = first.Length;
			int over = 0, step = 0;
			bool exit = false;

			while (true)
			{
				i--;
				if (i < 0) break;
				Console.WriteLine("\nStep : {0}\nOverflow = {1}",step, over == 1 ? true : false);
				Console.WriteLine("{0, -7}| {1, 20}",namefirst,first);
				Console.WriteLine("{0, -7}| {1, 20}",namesecond,second);
				Console.WriteLine(new string('-', 29));
				Console.WriteLine("answer | {0, 20}", ans);
				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;

				step++;
				if (first[i] == '1' && second[i] == '1' )
				{
					if (over == 0)
						ans[i + 1] = '0';
					else
						ans[i + 1] = '1';
					over = 1;
				}
				else
				if (first[i] == '0' && second[i] == '0')
				{
					if (over == 0)
						ans[i + 1] = '0';
					else
						ans[i + 1] = '1';
					over = 0;
				}
				else
				{
					if (over == 0)
					{
						ans[i + 1] = '1';
						over = 0;
					}
					else
					{
						ans[i + 1] = '0';
						over = 1;
					}		
				}
			}	

			if (!Checker.IsNegativResultForADD(val1, val2) && over == 1) ans[0] = '1';

			if ((!Checker.IsNegativResultForADD(val1, val2) && (val1 < 0 || val2 < 0)) || (val1 < 0 && val2 < 0))
			{
				int j = ans.Length - 1;
				while (ans[j] == '1')
					ans[j--] = '0';
				ans[j] = '1';
			}

			Console.WriteLine("\n{0, -7}| {1, 20}", namefirst, first);
			Console.WriteLine("{0, -7}| {1, 20}", namesecond, second);
			Console.WriteLine(new string('-', 29));
			Console.WriteLine("answer | {0, 20}\n", ans);
			string Ans = ans.ToString();
			int len = (namefirst[1] == 'x' ? 16 : 8);
		
			if (Ans.Length > len)
				Ans = Ans.Substring(Ans.Length - len);
			if (Checker.IsNegativResultForADD(val1, val2))
			{
				int intans = Checker.GiveMeMyNegativeValue(Ans);
				Console.WriteLine("Answer: {0} => {1} = {2} ({3})\n", ans, Ans, intans, intans.ToString("X"));
			}
			else
				Console.WriteLine("Answer: {0} => {1} = {2} ({3})\n", ans, Ans, Convert.ToInt32(Ans, 2), Convert.ToInt32(Ans, 2).ToString("X"));
			
		}


		public static void VisualCommandMUL(string val)
		{
			int val1 = (CPU.registors["a"] as Register).ReturnNumber();
			int val2 = (CPU.registors[val[0].ToString()] as Register).ReturnNumber(val);
			string first = "", second = "";
			Checker.ConvertBinaryValueForDIV(val1, val2, ref first, ref second);
			//Console.WriteLine(Convert.ToString(val2, 2));
			string help = new string('0', first.Length + second.Length);
			StringBuilder ans = new StringBuilder(help);

			int len = ans.Length;

			int step = 0;
			bool exit = false;

			Console.WriteLine("{0, -7}| {1}", "AX", first.PadLeft(len));
			Console.WriteLine("{0, -7}| {1}", val.ToUpper(), second.PadLeft(len));
			Console.WriteLine(new string('-', ans.Length + 9));

			int i = second.Length;
			//int t = second.Length;

			while (true)
			{
				i--;
				//t--;
				if (i < 0) break;
				help = "";

				for (int j = 0; j < first.Length; j++)
					if (first[j] == '0' || second[i] == '0')
						help += '0';
					else
						help += '1';

				int k = first.Length - 1;
				int over = 0, j1 = 0;
				for (int j = len - 1; k > -1; j--, k--)
				{
					int prval = ((ans[j] - '0') + over + (help[k] - '0'));
					ans[j] = (char)(prval % 2 + '0');
					over = prval / 2;
					j1 = j;
				}
				if (over > 0)
					ans[j1 - 1] = '1';

				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;

				Console.WriteLine("step {0}| {1}", (++step).ToString("D2"), help.PadLeft(len--));
			}

			Console.WriteLine(new string('-', ans.Length + 9));
			Console.WriteLine("{0, -7}| {1}", "answer", ans);

			string Ans = ans.ToString();

			if (Ans.Length > 16)
				Ans = Ans.Substring(Ans.Length - 16);

			int x;
			if (Checker.IsNegativeResultForMUL(val1, val2))
			{
				x = val1 * val2;
				Ans = Checker.NEG(Ans);
				Console.WriteLine("Answer: {0} => {1} = {2} ({3})\n", ans, Ans, x, x.ToString("X"));
			}
			else
			{
				x = Convert.ToInt32(Ans, 2);
				Console.WriteLine("Answer: {0} => {1} = {2} ({3})\n", ans, Ans, x, x.ToString("X"));
			}

			help = x.ToString("X8");
			if (help.Length > 8)
				help = help.Substring(help.Length - 7);

			Console.WriteLine("Result: {0} = {1}({4}), DX = {2}, AX = {3}\n", Ans, x, help.Substring(0, 4), help.Substring(4), help);
		}

		private static void VisualIfNothingNot(long val1, long val2, string first, string second, string reg)
		{
			string ans = "0";
			string help = first;
			long div = Convert.ToInt64(ans, 2);
			long mod = Convert.ToInt64(help, 2);

			if (val1 / val2 < 0)
			{
				div = -div;
				mod = -mod;
			}
			Console.WriteLine("{0} = {1}, {2} = {3}", val1, first, val2, second);
			Console.WriteLine("Answer = {0} = {1}({2})", ans, div, div.ToString("X4").Substring(div.ToString("X4").Length - 4));
			Console.WriteLine("Modulo = {0} = {1}({2})", help, mod, mod.ToString("X4").Substring(mod.ToString("X4").Length - 4));
			if (reg[1] == 'l' || reg[1] == 'h')
			{
				Console.WriteLine("AH = {1}, AL = {0}\n", div.ToString("X2").Substring(div.ToString("X").Length - 4), mod.ToString("X2").Substring(mod.ToString("X2").Length - 4));
			}
			else
			{
				Console.WriteLine("AX = {0}, DX = {1}\n", div.ToString("X4").Substring(div.ToString("X4").Length - 4), mod.ToString("X4").Substring(mod.ToString("X4").Length - 4));
			}
		}

		public static void VisualCommandDIV(long val1, long val2, string reg)
		{
			string first = "", second = "";
			bool isNeg = Checker.IsNegativeResultForMUL(val1, val2);
			Checker.ConvertBinaryValueForDIV(val1, val2, ref first, ref second);
			if (Math.Abs(val1) < Math.Abs(val2))
			{
				VisualIfNothingNot(val1, val2, first, second, reg);
				return;
			}
			string ans = "";

			string help = "";
			int step = 0, len = Math.Max(first.Length, second.Length), padleft = 0;
			Console.WriteLine("{0, -7}| {1}| {2}{4}{3}", "Step:", first, second, "|Answer:", new string(' ', first.Length * 2 + 2 - second.Length));
		
			bool exit = false;
			int i = 0;
			while (true)
			{
				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;

				help += first[i];
				int l = help.Length;
				help = help.TrimStart('0');
				padleft += (l - help.Length);

				Console.Write("Step{0} | {1}", (++step).ToString("D2"), (help == "" ? "0" : help).PadLeft(padleft + help.Length));
				if (!Checker.Equals(help, second))
				{
					ans += "0";
					ans = ans.TrimStart('0');
				}
				else
				{
					ans += "1";
					l = help.Length;
					help = ALU.SUBforDiv(help, second);
					padleft += (l - help.Length);
				}
				Console.WriteLine("{1}|{0}",ans, new string(' ', first.Length * 3 - help.Length - padleft + 4));
				if (++i == first.Length)
					break;
			}

			if (ans == "")
				ans = "0";
			if (help == "")
				help = "0";

			long div = Convert.ToInt64(ans, 2);
			long mod = Convert.ToInt64(help, 2);

			if (isNeg)
			{
				div = -div;
				mod = -mod;
			}

			Console.WriteLine("\nAnswer = {0} = {1}({2})", ans, div, div.ToString("X4").Substring(div.ToString("X4").Length - 4));
			Console.WriteLine("Modulo = {0} = {1}({2})", help, mod, mod.ToString("X4").Substring(mod.ToString("X4").Length - 4));
			if (reg[1] == 'l' || reg[1] == 'h')
			{
				Console.WriteLine("AH = {1}, AL = {0}", div.ToString("X2").Substring(div.ToString("X").Length - 4), mod.ToString("X2").Substring(mod.ToString("X2").Length - 4));
			}
			else
			{
				Console.WriteLine("AX = {0}, DX = {1}", div.ToString("X4").Substring(div.ToString("X4").Length - 4), mod.ToString("X4").Substring(mod.ToString("X4").Length - 4));
			}
		}

		public static void VisualCommandFAdd(byte sign, string a1, string a2)
		{
			SoReg fir = SCPU.reg[a1] as SoReg;
			SoReg sec = SCPU.reg[a2] as SoReg;

			string f = fir.man;
			string s = sec.man;

			Console.WriteLine("First  value S {0} E {1} M{2}", fir.sign, fir.or, fir.man);
			Console.WriteLine("Second value S {0} E {1} M{2}", sec.sign, sec.or, sec.man);

			int max = Math.Max(fir.exp, sec.exp);
			if (fir.exp != max)
			{
				f = new string('0', max - fir.exp) + f;
				f = f.Substring(0, 23);
			}
			else
			{
				s = new string('0', max - sec.exp) + s;
				s = s.Substring(0, 23);
			}

			max--;
		
			StringBuilder ans = new StringBuilder(new string('0', 23));
			Console.WriteLine("{0, 32}", fir.man);
			Console.WriteLine("{0, 32}", sec.man);
			int over = 0;
			int i = 23;
			int step = 0;
			bool exit = SCPU.MExit;
			while (true)
			{
				i--;
				if (i < 0) break;
				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;
				if (f[i] == '0' && s[i] == '0')
				{
					if (over == 0)
					{
						ans[i] = '0';
						over = 0;
					}
					else
					{
						ans[i] = '1';
						over = 0;
					}
				}

				if (f[i] == '0' && s[i] == '1')
				{
					if (over == 0)
					{
						ans[i] = '1';
						over = 0;
					}
					else
					{
						ans[i] = '0';
						over = 1;
					}
				}

				if (f[i] == '1' && s[i] == '0')
				{
					if (over == 0)
					{
						ans[i] = '1';
						over = 0;
					}
					else
					{
						ans[i] = '0';
						over = 1;
					}
				}

				if (f[i] == '1' && s[i] == '1')
				{
					if (over == 0)
					{
						ans[i] = '0';
						over = 1;
					}
					else
					{
						ans[i] = '1';
						over = 1;
					}
				}

				Console.WriteLine("Step {0}: {1}", (++step).ToString("D2"), ans);
			}
			if (over == 1)
			{
				ans.Insert(0, "1");
				//max++;
			}
			string Ans = ans.ToString();
			int len = Ans.Length;
			Ans = Ans.TrimStart('0');
			max += len - Ans.Length;
			//if (max == 0)
			//	max = 1;
			if (Ans.Length > 22)
				Ans = Ans.Substring(0, 23);
		
			Console.WriteLine("Answer: S {0} E {1} M {2} = {3}", sign, Convert.ToString(max + 127, 2), Ans + new string('0', 23 - Ans.Length), fir.val + sec.val);
		}

		public static void VisualCommandFSub(byte sign, string a1, string a2)
		{
			SoReg fir = SCPU.reg[a1] as SoReg;
			SoReg sec = SCPU.reg[a2] as SoReg;

			string f = fir.man;
			string s = sec.man;

			Console.WriteLine("First  value S {0} E {1} M{2}", fir.sign, fir.or, fir.man);
			Console.WriteLine("Second value S {0} E {1} M{2}", sec.sign, sec.or, sec.man);

			int max = Math.Max(fir.exp, sec.exp);
			if (fir.exp != max)
			{
				f = new string('0', max - fir.exp) + f;
				f = f.Substring(0, 23);
			}
			else
			{
				s = new string('0', max - sec.exp) + s;
				s = s.Substring(0, 23);
			}

			max--;
			if (fir.val < 0 && sec.val < 0)
				max++;

			StringBuilder ans = new StringBuilder(new string('0', 23));

			int over = 0;
			int i = 23;
			int step = 0;
			bool exit = SCPU.MExit;
			Console.WriteLine("{0, 32}", fir.man);
			Console.WriteLine("{0, 32}", sec.man);
			while (true)
			{
				i--;
				if (i < 0) break;
				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;
				if (f[i] == '1' && s[i] == '1')
				{
					if (over == 0)
					{
						ans[i] = '0';
						over = 0;
					}
					else
					{
						ans[i] = '1';
						over = 1;
					}
				}
				else
				if (f[i] == '0' && s[i] == '0')
				{
					if (over == 0)
					{
						ans[i] = '0';
						over = 0;
					}
					else
					{
						ans[i] = '1';
						over = 1;
					}
				}
				else
				if (f[i] == '1' && s[i] == '0')
				{
					if (over == 0)
					{
						ans[i] = '1';
						over = 0;
					}
					else
					{
						ans[i] = '0';
						over = 0;
					}
				}
				else
				if (f[i] == '0' && s[i] == '1')
				{
					if (over == 0)
					{
						ans[i] = '1';
						over = 1;
					}
					else
					{
						ans[i] = '0';
						over = 1;
					}
				}

				Console.WriteLine("Step {0}: {1}", (++step).ToString("D2"), ans);
			}
			if (over == 1)
			{
				max--;
			}
			string Ans = ans.ToString();
			int len = Ans.Length;
			Ans = Ans.TrimStart('0');
			max -= len - Ans.Length;
			//if (max == 0)
			//	max = 1;
			if (Ans.Length > 22)
				Ans = Ans.Substring(0, 23);
			
			Console.WriteLine("Answer: S {0} E {1} M {2} = {3}", sign, Convert.ToString(max + 127, 2), Ans + new string('0', 23 - Ans.Length), fir.val - sec.val);
		}

		public static void VisualCommandFMul(string a1, string a2)
		{
			SoReg fir = SCPU.reg[a1] as SoReg;
			SoReg sec = SCPU.reg[a2] as SoReg;

			string first = fir.man;
			string second = sec.man;
			int exp = fir.exp + sec.exp - 2;

			Console.WriteLine("First  value S {0} E {1} M{2}", fir.sign, fir.or, fir.man);
			Console.WriteLine("Second value S {0} E {1} M{2}", sec.sign, sec.or, sec.man);
			//Console.WriteLine(Convert.ToString(exp + 127, 2));
			Console.WriteLine("{0, 55}", fir.man);
			Console.WriteLine("{0, 55}", sec.man);
			string help = new string('0', first.Length + second.Length);
			StringBuilder ans = new StringBuilder(help);

			int len = ans.Length;
			int i = 23;
			int step = 0;
			bool exit = SCPU.MExit;

			while (true)
			{
				i--;
				if (i < 0) break;
				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;
				help = "";

				for (int j = 0; j < first.Length; j++)
					if (first[j] == '0' || second[i] == '0')
						help += '0';
					else
						help += '1';

				int k = first.Length - 1;
				int over = 0, j1 = 0;
				for (int j = len - 1; k > -1; j--, k--)
				{
					int prval = ((ans[j] - '0') + over + (help[k] - '0'));
					ans[j] = (char)(prval % 2 + '0');
					over = prval / 2;
					j1 = j;
				}
				if (over > 0)
					ans[j1 - 1] = '1';
				Console.WriteLine("step {0}| {1}", (++step).ToString("D2"), help.PadLeft(len--));
			}
			Console.WriteLine(new string('-', ans.Length + 9));
			Console.WriteLine("{0, -7}| {1}", "answer", ans);

			string Ans = ans.ToString();
			Ans = Ans.TrimStart('0');

			if (Ans.Length > 22)
				Ans = Ans.Substring(0, 23);

			int v = 0;
			if (fir.val * sec.val < 0)
				v = 1;

			Console.WriteLine("Answer: S {0} E {1} M {2} = {3}", v, Convert.ToString(exp + 127 + 1, 2), Ans + new string('0', 23 - Ans.Length), fir.val * sec.val);
		}

		public static void VisualCommandFDiv(string a1, string a2)
		{
			SoReg fir = SCPU.reg[a1] as SoReg;
			SoReg sec = SCPU.reg[a2] as SoReg;

			string f = fir.man;
			string s = sec.man;
			int exp = fir.exp - sec.exp;

			Console.WriteLine("First  value S {0} E {1} M{2}", fir.sign, fir.or, fir.man);
			Console.WriteLine("Second value S {0} E {1} M{2}", sec.sign, sec.or, sec.man);
			//Console.WriteLine(Convert.ToString(exp + 127, 2));
			//Console.WriteLine("{0, 55}", fir.man);
			//Console.WriteLine("{0, 55}", sec.man);

			int i = 0;
			f += new string('0', 23);

			bool exit = SCPU.MExit;
			int step = 0;
			string help = "";
			string ans = "";
			int u = 0;
			
			Console.WriteLine(f.PadLeft(f.Length + 9));
			Console.WriteLine(s.PadLeft(s.Length + 9));

			for (; u < 23; ++u)
				help += f[u];
			while (true)
			{
				Console.WriteLine("Step {0}: {1}{2}",(++step).ToString("D2"), new string(' ', i++), help);
				if (u >= f.Length)
					break;
				if (!exit)
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
						exit = true;
				
				if (Checker.Equals(help, s))
				{
					ans += "1";
					int over = 0;
					StringBuilder A = new StringBuilder(new string('0', help.Length));
					s = new String('0', help.Length - s.Length) + s;
					for (int j = help.Length - 1; j > -1; --j)
					{
						if (help[j] == '1' && s[j] == '1')
						{
							if (over == 0)
							{
								A[j] = '0';
								over = 0;
							}
							else
							{
								A[j] = '1';
								over = 1;
							}
						}
						else
						if (help[j] == '0' && s[j] == '0')
						{
							if (over == 0)
							{
								A[j] = '0';
								over = 0;
							}
							else
							{
								A[j] = '1';
								over = 1;
							}
						}
						else
						if (help[j] == '1' && s[j] == '0')
						{
							if (over == 0)
							{
								A[j] = '1';
								over = 0;
							}
							else
							{
								A[j] = '0';
								over = 0;
							}
						}
						else
						if (help[j] == '0' && s[j] == '1')
						{
							if (over == 0)
							{
								A[j] = '1';
								over = 1;
							}
							else
							{
								A[j] = '0';
								over = 1;
							}
						}
					}

					help = A.ToString();
					help = help.TrimStart('0');
					s = s.TrimStart('0');
					help += f[u++];
				}
				else
				{
					ans += "0";
					help += f[u++];
				}			
			}

			Console.WriteLine("Answer: " + ans);


			ans = ans.TrimStart('0');

			if (ans.Length > 22)
				ans = ans.Substring(0, 23);

			int v = 0;
			if (fir.val / sec.val < 0)
				v = 1;

			Console.WriteLine("Answer: S {0} E {1} M {2} = {3}", v, Convert.ToString(exp + 127, 2), ans + new string('0', 23 - ans.Length), fir.val / sec.val);
		}
	}
}
