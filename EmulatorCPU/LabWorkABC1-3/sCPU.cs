using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	public static class SCPU
	{
		static public Hashtable reg;
		static public bool MExit;

		static SCPU()
		{
			MExit = false;
			reg = new Hashtable
			{
				{ "r7", new SoReg("r7") },
				{ "r6", new SoReg("r6") },
				{ "r5", new SoReg("r5") },
				{ "r4", new SoReg("r4") },
				{ "r3", new SoReg("r3") },
				{ "r2", new SoReg("r2") },
				{ "r1", new SoReg("r1") },
				{ "r0", new SoReg("r0") }
				
			};
		}

		public static void FMOV(string name, string sval)
		{
			double val = double.Parse(sval);

			if (Math.Abs(val) > 1e38 || (Math.Abs(val) < 1e-38))
			{
				Console.WriteLine("Range out value!");
				return;
			}
			(reg[name] as SoReg).In(val);
		}

		public static void Info()
		{
			foreach (DictionaryEntry v in reg)
				(v.Value as SoReg).Info();
		}

		public static void Clear()
		{
			foreach (DictionaryEntry v in reg)
				(v.Value as SoReg).Clear();
		}

		public static void FADD(string fir, string sec)
		{
			SoReg rf = reg[fir] as SoReg;
			SoReg rs = reg[sec] as SoReg;
			if (rf.val < 0 && rs.val > 0)
				Visual.VisualCommandFSub(1, fir, sec);
			else
			if (rf.val > 0 && rs.val < 0)
				Visual.VisualCommandFSub(0, fir, sec);
			else
				Visual.VisualCommandFAdd(1,fir, sec);

			double ans = rf.val + rs.val;
			rf.In(ans);		
		}

		public static void FSUB(string fir, string sec)
		{
			SoReg rf = reg[fir] as SoReg;
			SoReg rs = reg[sec] as SoReg;

			if (rf.val < rs.val)
			{
				string s = fir;
				fir = sec;
				sec = s;
			}

			if (rf.val < 0 && rs.val > 0)
				Visual.VisualCommandFAdd(1, fir, sec);
			else
			if (rf.val > 0 && rs.val < 0)
				Visual.VisualCommandFAdd(0, fir, sec);
			else
			if (rf.val < 0 && rs.val < 0)
				Visual.VisualCommandFSub(1, fir, sec);
			else
				Visual.VisualCommandFSub(0, fir, sec);

			double ans = rf.val - rs.val;
			rf.In(ans);
		}


		public static void FMUL(string fir, string sec)
		{
			Visual.VisualCommandFMul(fir, sec);
			SoReg rf = reg[fir] as SoReg;
			SoReg rs = reg[sec] as SoReg;
			
			double ans = rf.val * rs.val;
			rf.In(ans);
		}

		public static void FDIV(string fir, string sec)
		{
			SoReg rf = reg[fir] as SoReg;
			SoReg rs = reg[sec] as SoReg;

			if (Math.Abs(rs.val) <= 0.00000001)
			{
				Console.WriteLine("Divide by zero!");
				return;
			}

			Visual.VisualCommandFDiv(fir, sec);

			double ans = rf.val / rs.val;
			rf.In(ans);
		}

		public static void Task1(double a, double b, double c)
		{
			(reg["r0"] as SoReg).In(a);
			(reg["r1"] as SoReg).In(b);
			(reg["r2"] as SoReg).In(c);

			Console.WriteLine("\nCount: b^2\n");
			Visual.VisualCommandFMul("r1", "r1");
			(reg["r3"] as SoReg).In((reg["r1"] as SoReg).val * (reg["r1"] as SoReg).val);

		
			Console.WriteLine("\nCount: 4 * a\n");
			(reg["r4"] as SoReg).In(4);
			Visual.VisualCommandFMul("r4", "r0");
			(reg["r4"] as SoReg).In((reg["r4"] as SoReg).val * (reg["r0"] as SoReg).val);

			Console.WriteLine("\nCount: 4 * a * c\n");
			Visual.VisualCommandFMul("r4", "r2");
			(reg["r4"] as SoReg).In((reg["r4"] as SoReg).val * (reg["r2"] as SoReg).val);

			Console.WriteLine("\nCount: D = b^2 - 4 * a * c\n");
			byte x = 0;
			if ((reg["r3"] as SoReg).val - (reg["r4"] as SoReg).val < 0)
				x = 1;
			Visual.VisualCommandFSub(x, "r3", "r4");
			(reg["r3"] as SoReg).In((reg["r3"] as SoReg).val - (reg["r4"] as SoReg).val);

			if ((reg["r3"] as SoReg).val > 0)
			{
				Console.WriteLine("\nD > 0");
				(reg["r3"] as SoReg).In(Math.Sqrt((reg["r3"] as SoReg).val));
				(reg["r1"] as SoReg).In(-(reg["r1"] as SoReg).val);

				Console.WriteLine("\nCount: 2 * a\n");
				(reg["r4"] as SoReg).In(2);
				Visual.VisualCommandFMul("r4", "r0");
				(reg["r0"] as SoReg).In((reg["r4"] as SoReg).val * (reg["r0"] as SoReg).val);

				Console.WriteLine("\nCount: -b + sqrt(D)\n");
			    x = 0;
				if ((reg["r1"] as SoReg).val + (reg["r3"] as SoReg).val < 0)
					x = 1;
				Visual.VisualCommandFAdd(x, "r1", "r3");
				(reg["r5"] as SoReg).In((reg["r1"] as SoReg).val + (reg["r3"] as SoReg).val);

				Console.WriteLine("\nCount: x1 = (- b + sqrt(D)) / 2 * a \n");
				Visual.VisualCommandFDiv("r5", "r0");
				(reg["r5"] as SoReg).In((reg["r5"] as SoReg).val / (reg["r0"] as SoReg).val);

				Console.WriteLine("\nCount: -b - sqrt(D)\n");
				x = 0;
				if ((reg["r1"] as SoReg).val - (reg["r3"] as SoReg).val < 0)
					x = 1;
				Visual.VisualCommandFSub(x, "r1", "r3");
				(reg["r6"] as SoReg).In((reg["r1"] as SoReg).val - (reg["r3"] as SoReg).val);

				Console.WriteLine("\nCount: x2 = (- b - sqrt(D)) / 2 * a \n");
				Visual.VisualCommandFDiv("r6", "r0");
				(reg["r6"] as SoReg).In((reg["r6"] as SoReg).val / (reg["r0"] as SoReg).val);

				(reg["r0"] as SoReg).In((reg["r5"] as SoReg).val);
				(reg["r1"] as SoReg).In((reg["r6"] as SoReg).val);
				Console.WriteLine("\nAnswer x1 = {0}(r0), x2 = {1}(r1)\n", (reg["r0"] as SoReg).val, (reg["r1"] as SoReg).val);

			}
			else
			if (Math.Abs((reg["r3"] as SoReg).val) <= 0.0000001)
			{
				Console.WriteLine("\nD = 0");
				(reg["r1"] as SoReg).In(-(reg["r1"] as SoReg).val);

				Console.WriteLine("\nCount: 2 * a\n");
				(reg["r4"] as SoReg).In(2);
				Visual.VisualCommandFMul("r4", "r0");
				(reg["r0"] as SoReg).In((reg["r4"] as SoReg).val * (reg["r0"] as SoReg).val);

				Console.WriteLine("\nCount: x1,2 = -b / 2 * a \n");
				Visual.VisualCommandFDiv("r1", "r0");
				(reg["r1"] as SoReg).In((reg["r1"] as SoReg).val / (reg["r0"] as SoReg).val);

				(reg["r0"] as SoReg).In((reg["r1"] as SoReg).val);
				Console.WriteLine("\nAnswer x1 = {0}(r0), x2 = {1}(r1)\n", (reg["r0"] as SoReg).val, (reg["r1"] as SoReg).val);
			}
			else
			{
				Console.WriteLine("\nRoots are not present!");
				return;
			}
		}

		public static string Task2(double x, double e)
		{
			Clear();
			MExit = true;
			double y = Math.Sin(x);
			int n = 0;
			bool sign = true;

			(reg["r0"] as SoReg).In(x);
			(reg["r1"] as SoReg).In(x);
			(reg["r2"] as SoReg).In(x);
			(reg["r3"] as SoReg).In(1);
			(reg["r4"] as SoReg).In(1);
			(reg["r5"] as SoReg).In(1);
			while (true)
			{ 
				Console.WriteLine("\nS{0} = {1}x^{2} / {2}! = {3}", ++n, sign ? "+" : "-", (int)(reg["r3"] as SoReg).val, (reg["r0"] as SoReg).val);
				if (Math.Abs(y - (reg["r0"] as SoReg).val) <= e)
					break;
				sign = sign ? false : true;

				Console.WriteLine("\nCount: x^{0} \n", (int)(reg["r3"] as SoReg).val + 2);

				Visual.VisualCommandFMul("r2", "r1");
				(reg["r2"] as SoReg).In((reg["r2"] as SoReg).val * (reg["r1"] as SoReg).val);

				Visual.VisualCommandFMul("r2", "r1");
				(reg["r2"] as SoReg).In((reg["r2"] as SoReg).val * (reg["r1"] as SoReg).val);


				Console.WriteLine("\nCount: ({0} + 2)! \n", (int)(reg["r3"] as SoReg).val);

				Visual.VisualCommandFAdd(0, "r3", "r5");
				(reg["r3"] as SoReg).In((reg["r3"] as SoReg).val + (reg["r5"] as SoReg).val);

				Visual.VisualCommandFMul("r4", "r3");
				(reg["r4"] as SoReg).In((reg["r4"] as SoReg).val * (reg["r3"] as SoReg).val);

				Visual.VisualCommandFAdd(0, "r3", "r5");
				(reg["r3"] as SoReg).In((reg["r3"] as SoReg).val + (reg["r5"] as SoReg).val);

				Visual.VisualCommandFMul("r4", "r3");
				(reg["r4"] as SoReg).In((reg["r4"] as SoReg).val * (reg["r3"] as SoReg).val);

				Console.WriteLine("\nCount: S{0} \n", n);
				Visual.VisualCommandFDiv("r2", "r4");
				(reg["r6"] as SoReg).In((reg["r2"] as SoReg).val / (reg["r4"] as SoReg).val);

				Console.WriteLine("\nCount: S += S{0} \n", n);

				int v = 1;
				if (sign)
					v = 1;
				else
					v = -1;

				byte z = 0;
				if ((reg["r0"] as SoReg).val + v * (reg["r6"] as SoReg).val < 0)
					z = 1;
				Visual.VisualCommandFAdd(z, "r0", "r6");
				

				(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val + v * (reg["r6"] as SoReg).val);

				
				if (n == 1000)
				{
					MExit = false;
					return "Answer does not exist!"; ;
				}
			}

			MExit = false;
			string f = String.Format("{0: 0.0000}", Math.Round((reg["r0"] as SoReg).val, 5));
			string s = String.Format("{0: 0.0000}", y);
			//return (reg["r0"] as SoReg).val.ToString() + " " + y.ToString();
			return String.Format("{0, -6} = {1} | {4, -6} = {2} | Steps n = {3}", "S(" + x + ")", (reg["r0"] as SoReg).val > 0 ? " " + f : f, y > 0 ? " " + s : s, n, "Y(" + x + ")");
		}

		public static double FindFXY(Dictionary<string, double> data, double x, double y)
		{
			Console.WriteLine("\nFind f(x, y) = x^{0} * y^{1} * {2} - y * x^{3} * {4}", data["qx"], data["yn"], data["k3"], data["px"], data["k2"]);

			//y^n
			(reg["r0"] as SoReg).In(Math.Pow(y, data["yn"]));

			//y^n * k3
			(reg["r1"] as SoReg).In(data["k3"]);
			if (data["visual"] > 0)
				Visual.VisualCommandFMul("r0", "r1");
			(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val * (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{0}^{1} * {2} = {3}", y, data["yn"], data["k3"], (reg["r0"] as SoReg).val);

			//y^n * k3 * q(x)
			
			(reg["r1"] as SoReg).In(Math.Pow(x, data["qx"]));
			if (data["visual"] > 0)
				Visual.VisualCommandFMul("r0", "r1");
			(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val * (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{0}^{1} * {2} * {4}^{3} = {5}", y, data["yn"], data["k3"], data["qx"], x, (reg["r0"] as SoReg).val);

			//y
			(reg["r2"] as SoReg).In(y);

			//y * p(x)
			(reg["r1"] as SoReg).In(Math.Pow(x, data["px"]));
			if (data["visual"] > 0)
				Visual.VisualCommandFMul("r2", "r1");
			(reg["r2"] as SoReg).In((reg["r2"] as SoReg).val * (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{0} * {2}^{1} = {3}", y, data["px"], x, (reg["r2"] as SoReg).val);

			//y * p(x) * k2
			(reg["r1"] as SoReg).In(data["k2"]);
			if (data["visual"] > 0)
				Visual.VisualCommandFMul("r2", "r1");
			(reg["r2"] as SoReg).In((reg["r2"] as SoReg).val * (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{0} * {3}^{1} * {2} = {3}", y, data["px"], data["k2"], x, (reg["r2"] as SoReg).val);

			//y^n * k3 * q(x) - y * k2 * p(x)
			byte sign = (reg["r0"] as SoReg).val - (reg["r2"] as SoReg).val < 0 ? (byte)1 : (byte)0;
			if (data["visual"] > 0)
				Visual.VisualCommandFSub(sign, "r0", "r2");
			(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val - (reg["r2"] as SoReg).val);
			Console.WriteLine("\nf(x, y) = x^{0} * y^{1} * {2} - y * x^{3} * {4}", data["qx"], data["yn"], data["k3"], data["px"], data["k2"], (reg["r0"] as SoReg).val);

			return (reg["r0"] as SoReg).val;
		}

		public static double Eiler(double y, double h, double fxy, double visual)
		{
			(reg["r0"] as SoReg).In(y);
			(reg["r1"] as SoReg).In(h);
			(reg["r2"] as SoReg).In(fxy);

			//h * fxy
			if (visual > 0)
				Visual.VisualCommandFMul("r1", "r2");
			(reg["r1"] as SoReg).In((reg["r1"] as SoReg).val * (reg["r2"] as SoReg).val);
			Console.WriteLine("\n{0} * {1} = {2}", h, fxy, (reg["r1"] as SoReg).val);

			//y + h * fxy
			byte sign = (reg["r0"] as SoReg).val + (reg["r1"] as SoReg).val < 0 ? (byte)1 : (byte)0;
			if (visual > 0)
				Visual.VisualCommandFAdd(sign, "r0", "r1");
			(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val + (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{3} + {0} * {1} = {2}", h, fxy, (reg["r0"] as SoReg).val, y);

			return (reg["r0"] as SoReg).val;
		}

		public static double Adams(double fxy0, double fxy1, double y, double h, double visual)
		{
			(reg["r0"] as SoReg).In(fxy1);
			(reg["r1"] as SoReg).In(3);

			//f(x1, y1) * 3
			if (visual > 0)
				Visual.VisualCommandFMul("r0", "r1");
			(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val * (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{0} * 3 = {1}", fxy1, (reg["r0"] as SoReg).val);

			//3 * f(x1,y1) - f(x0, y0)
			(reg["r1"] as SoReg).In(fxy0);
			byte sign = (reg["r0"] as SoReg).val - (reg["r1"] as SoReg).val < 0 ? (byte)1 : (byte)0;
			if (visual > 0)
				Visual.VisualCommandFSub(sign, "r0", "r1");
			(reg["r0"] as SoReg).In((reg["r0"] as SoReg).val - (reg["r1"] as SoReg).val);
			Console.WriteLine("\n{0} * 3 - {2} = {1}", fxy1, (reg["r0"] as SoReg).val, fxy0);

			//h / 2
			(reg["r2"] as SoReg).In(h);
			(reg["r3"] as SoReg).In(2);
			if (visual > 0)
				Visual.VisualCommandFDiv("r2", "r3");
			(reg["r2"] as SoReg).In((reg["r2"] as SoReg).val / (reg["r3"] as SoReg).val);
			Console.WriteLine("\n{0} / 2 = {1}", h, (reg["r2"] as SoReg).val);

			//h / 2 * 3 * f(x1,y1) - f(x0, y0)
			if (visual > 0)
				Visual.VisualCommandFMul("r2", "r0");
			(reg["r0"] as SoReg).In((reg["r2"] as SoReg).val * (reg["r0"] as SoReg).val);
			Console.WriteLine("\n{3} / 2 * {0} * 3 - {2} = {1}", fxy1, (reg["r0"] as SoReg).val, fxy0, h);

			(reg["r1"] as SoReg).In(y);
			sign = (reg["r1"] as SoReg).val + (reg["r0"] as SoReg).val < 0 ? (byte)1 : (byte)0;
			if (visual > 0)
				Visual.VisualCommandFAdd(sign, "r1", "r0");
			(reg["r0"] as SoReg).In((reg["r1"] as SoReg).val + (reg["r0"] as SoReg).val);
			Console.WriteLine("\n{4} + {3} / 2 * {0} * 3 - {2} = {1}", fxy1, (reg["r0"] as SoReg).val, fxy0, h, y);

			return (reg["r0"] as SoReg).val;
		}
	}

	public class SoReg
	{
		public double val;
		public int exp;
		public byte sign;
		public string or;
		public string man;
		string name;

		public SoReg(string name)
		{
			sign = 0;
			or = new string('0', 8);
			man = new string('0', 23);
			val = 0;
			exp = 0;
			this.name = name;
		}

		public void Info()
		{
			Console.Write(name + " S " + sign + " E ");
			Console.Write(or);
			Console.Write(" M ");
			Console.Write(man);
			Console.Write(" = " + val + '\n');
		}

		public void Clear()
		{
			sign = 0;
			or = new string('0', 8);
			man = new string('0', 23);
			val = 0;
		}

		public void In(double ch)
		{
			Clear();
			string s = ch.ToString(); ;
			val = ch;

			if (ch < 0)
			{
				sign = 1;
				ch *= -1;
			}

			int fir = 0;

			fir = (int)Math.Truncate(ch);
			exp = Convert.ToString(fir, 2).Length;
			int len = exp - 1 + 127;
			if (fir < 1)
			{
				double d = ch - fir;
				double c = ch;
				int x = 0;
				while ((int)Math.Truncate(c) == 0)
				{
					c *= 10;
					x++;
				}
				len = -x + 127;
			}
			man = Convert.ToString(fir, 2);
			if (fir != ch)
				man += Help.ToBinFrac(ch - fir, 23 - man.Length);
		
			man = man.TrimStart('0');

			or = Convert.ToString(len, 2);
			string help = new string('0', 8 - or.Length);
			or = help + or;
			if (man.Length < 23)
			{
				help = new string('0', 23 - man.Length);
				man += help;
			}
		}
	}

	static class Help
	{
		public static string ToBinFrac(double frac, int len)
		{
			string str = "";
			int c;
			int n = 0;
			while (n < len)
			{
				frac *= 2;
				c = Convert.ToInt32(Math.Truncate(frac));
				str = String.Concat(str, Convert.ToString(c));
				frac -= c;
				n++;
			}
			return str;
		}
	}
}
