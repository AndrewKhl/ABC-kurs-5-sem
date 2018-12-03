using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	static class ALU
	{
		public static void MOV(string val1, string val2)
		{
			if (TypeOfWord(val2) == 0)
			{
				bool error = false;
				int number = Checker.CheckedNumber(val2, ref error);
				if (error == true)
					return;

				byte typereg = TypeOfWord(val1);

				switch(typereg)
				{
					case 1:
						NumberInRegister(CPU.registors[val1[0].ToString()] as Register, number);
						break;
					default:
						NumberInSmallRegister(val1, number);
						break;
				}
					
			}
			else
			{
				RegisterInRegister(val1, val2);
			}
		}

		private static byte TypeOfWord(string word)
		{
			if (Checker.IsNumber(word)) return 0;
			switch (word[1])
			{
				case 'x':
					return 1;
				case 'h':
					return 2;
				default:
					return 3;
			}
		}

		private static void NumberInRegister(Register reg, long number)
		{
			//Register reg = CPU.registors[namereg] as Register;
			string hexstr = number.ToString("X4");
			hexstr = hexstr.Substring(hexstr.Length - 4);

			if (number < 0)
				reg.neg = true;
			else
				reg.neg = false;

			reg.h = Convert.ToInt32(hexstr.Substring(0, 2), 16);
			reg.l = Convert.ToInt32(hexstr.Substring(2), 16);
		}

		private static void NumberInSmallRegister(string namereg, int number)
		{
			Register reg = CPU.registors[namereg[0].ToString()] as Register;

			if (Math.Abs(number) > 256)
			{
				Console.WriteLine("Constant too large!");
				return;
			}

			string hexstr = number.ToString("X4");

			if (namereg[1] == 'h')
				reg.h = Convert.ToInt32(hexstr.Substring(hexstr.Length - 2, 2), 16);
			else
				reg.l = Convert.ToInt32(hexstr.Substring(hexstr.Length - 2, 2), 16);
		}

		private static void RegisterInRegister(string namefirstreg, string namesecondreg)
		{
			Register first = CPU.registors[namefirstreg[0].ToString()] as Register;
			Register second = CPU.registors[namesecondreg[0].ToString()] as Register;
			byte typefirst = TypeOfWord(namefirstreg);
			byte typesecond = TypeOfWord(namesecondreg);

			switch (typefirst)
			{
				case 1:
					switch(typesecond)
					{
						case 1:
							CopyRegister(first, second);
							break;
						default:
							Console.WriteLine("Operand types do not match!");
							break;
					}
					break;
				case 2:
					switch (typesecond)
					{
						case 2:
							first.h = second.h;
							break;
						case 3:
							first.h = second.l;
							break;
						default:
							Console.WriteLine("Operand types do not match!");
							break;
					}
					break;
				case 3:
					switch (typesecond)
					{
						case 2:
							first.l = second.h;
							break;
						case 3:
							first.l = second.l;
							break;
						default:
							Console.WriteLine("Operand types do not match!");
							break;
					}
					break;
			}
		}

		private static void CopyRegister(Register first, Register second)
		{
			first.h = second.h;
			first.l = second.l;
			first.neg = second.neg;
		}

		public static void SUB(string val1, string val2)
		{
			if (TypeOfWord(val2) == 0)
			{
				val2 = "-" + val2;
				ADD(val1, val2);
			}
			else
			{
				int v = (CPU.registors[val2[0].ToString()] as Register).ReturnNumber(val2);
				NumberInRegister(CPU.registors[val2[0].ToString()] as Register, -v);
				ADD(val1, val2);
				NumberInRegister(CPU.registors[val2[0].ToString()] as Register, v);
			}
			
		}

		public static void ADD(string val1, string val2)
		{
			if (TypeOfWord(val2) == 0)
			{
				bool error = false;
				int number = Checker.CheckedNumber(val2, ref error);
				if (error == true)
					return;

				byte typereg = TypeOfWord(val1);

				int val = (CPU.registors[val1[0].ToString()] as Register).ReturnNumber(val1);
				Visual.VisualCommandADD(val1, "number", val, Convert.ToInt32(val2));

				switch (typereg)
				{
					case 1:
						AddNumberInRegister(CPU.registors[val1[0].ToString()] as Register, number);
						break;
					default:
						AddNumberInSmallRegister(val1, number, true);
						break;
				}
			}
			else
			{
				bool ok = true;
				int v1 = (CPU.registors[val1[0].ToString()] as Register).ReturnNumber(val1);
				int v2 = (CPU.registors[val2[0].ToString()] as Register).ReturnNumber(val2);
				AddRegisterInRegister(val1, val2, ref ok);
				if (ok)
					Visual.VisualCommandADD(val1, val2, v1, v2);
			}
		}

		private static void AddNumberInRegister(Register reg, int number)
		{
			int answer = reg.ReturnNumber() + number;
			CPU.CreateFlags(answer, false);
			NumberInRegister(reg, answer);	
		}

		private static void AddNumberInSmallRegister(string namereg, int number, bool isNumber)
		{
			Register reg = CPU.registors[namereg[0].ToString()] as Register;

			if (isNumber && Math.Abs(number) > 256)
			{
				Console.WriteLine("Constant too large!");
				return;
			}

			if (namereg[1] == 'h')
				number += reg.h;
			else
				number += reg.l;

			CPU.CreateFlags(number, true);
			string hexstr = number.ToString("X4");

			if (namereg[1] == 'h')
				reg.h = Convert.ToInt32(hexstr.Substring(hexstr.Length - 2, 2), 16);
			else
				reg.l = Convert.ToInt32(hexstr.Substring(hexstr.Length - 2, 2), 16);
		}

		private static void AddRegisterInRegister(string namefirstreg, string namesecondreg,ref bool ok)
		{
			Register first = CPU.registors[namefirstreg[0].ToString()] as Register;
			Register second = CPU.registors[namesecondreg[0].ToString()] as Register;
			byte typefirst = TypeOfWord(namefirstreg);
			byte typesecond = TypeOfWord(namesecondreg);

			switch (typefirst)
			{
				case 1:
					switch (typesecond)
					{
						case 1:
							AddNumberInRegister(first, second.ReturnNumber());
							break;
						default:
							ok = false;
							Console.WriteLine("Operand types do not match!");
							break;
					}
					break;
				case 2:
					switch (typesecond)
					{
						case 2:
							AddNumberInSmallRegister(namefirstreg, second.h, false);
							break;
						case 3:
							AddNumberInSmallRegister(namefirstreg, second.l, false);
							break;
						default:
							ok = false;
							Console.WriteLine("Operand types do not match!");
							break;
					}
					break;
				case 3:
					switch (typesecond)
					{
						case 2:
							AddNumberInSmallRegister(namefirstreg, second.h, false);
							break;
						case 3:
							AddNumberInSmallRegister(namefirstreg, second.l, false);
							break;
						default:
							ok = false;
							Console.WriteLine("Operand types do not match!");
							break;
					}
					break;
			}
		}

		public static void MUL(string reg)
		{
			Visual.VisualCommandMUL(reg);
			Register mainReg = CPU.registors["a"] as Register;
			int val1 = (CPU.registors["a"] as Register).ReturnNumber();
			int val2 = (CPU.registors[reg[0].ToString()] as Register).ReturnNumber(reg);
			long ans = val1 * val2;
			string s;

			if (Math.Abs(ans) > 65535)
				s = ans.ToString("X8");
			else
				s = ans.ToString("X4");

			if (s.Length > 8)
				s = s.Substring(s.Length - 8);

			if (s.Length < 5)
			{
				ToReg(mainReg, s);
				if (mainReg.h != 0)
					CPU.flags["OF"] = true;
				(CPU.registors["d"] as Register).Clear();
				
			}
			else
			{
				ToReg(mainReg, s.Substring(4));
				ToReg(CPU.registors["d"] as Register, s.Substring(0, 4));
				if (ans < 0)
					(CPU.registors["d"] as Register).neg = true;

				if ((CPU.registors["d"] as Register).ReturnNumber() != 0)
					CPU.flags["OF"] = true;
			}

			CPU.CreateFlags(ans, false);
		}

		public static void DIV(string reg)
		{
			byte type = TypeOfWord(reg);
			if (type == 1)
			{
				Register reg1 = CPU.registors["a"] as Register;
				Register reg2 = CPU.registors["d"] as Register;
				string first;
				if (reg2.neg)
					first = (-reg2.ReturnNumber()).ToString("X4");
				else
					first = reg2.ReturnNumber().ToString("X4");
				bool isNeg = false;
				if (reg1.neg || reg2.neg)
					isNeg = true;

				long v = reg1.ReturnNumber();
				if (v < 0) v = -v;
				long val = Convert.ToInt64(first + v.ToString("X4"), 16);
				
				if (isNeg) val = -val;

				Register reg3 = CPU.registors[reg[0].ToString()] as Register;
				long valD = reg3.ReturnNumber();
				if (valD == 0)
				{
					Console.WriteLine("Divide by zero");
					return;
				}
				Visual.VisualCommandDIV(val, valD, reg);
				NumberInRegister(reg1, val / valD);
				NumberInRegister(reg2, val % valD);
				
				if (isNeg)
				{
					reg1.neg = true;
					reg2.neg = true;
				}
				else
				{
					reg1.neg = false;
					reg2.neg = false;
				}
				
			}
			else
			if(type > 1)
			{
				Register first = CPU.registors["a"] as Register;
				int val2 = (CPU.registors[reg[0].ToString()] as Register).ReturnNumber(reg);
				int val1 = first.ReturnNumber();
				if (val2 == 0)
				{
					Console.WriteLine("Divide by zero");
					return;
				}
				first.l = val1 / val2;
				first.h = val1 % val2;
				if (first.l < 0) first.l = -first.l;
				if (first.h < 0) first.h = -first.h;
				if (val1 * val2 < 0)
					first.neg = true;
				else
					first.neg = false;
				Visual.VisualCommandDIV(val1, val2, reg);
			}

		}

		public static string SUBforDiv(string val1, string val2)
		{
			if (val1.Length > val2.Length)
				val2 = "0" + val2;
			string s = "";
			StringBuilder ans = new StringBuilder(new string('0', val1.Length));
			int over = 0;
			val2 = Checker.NEG(val2);
			for (int i = val1.Length - 1; i > -1; i--)
			{
				if (val1[i] == '1' && val2[i] == '1')
				{
					if (over == 0)
						ans[i] = '0';
					else
						ans[i] = '1';
					over = 1;
				}
				else
				if (val1[i] == '0' && val2[i] == '0')
				{
					if (over == 0)
						ans[i] = '0';
					else
						ans[i] = '1';
					over = 0;
				}
				else
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
			}
			s = ans.ToString();
			s = s.TrimStart('0');
			return (s == "" ? "0" : s);
		}

		private static void ToReg(Register reg, string s)
		{
			reg.Clear();
			reg.h = Convert.ToInt32(s.Substring(0, 2), 16);
			reg.l = Convert.ToInt32(s.Substring(s.Length / 2), 16);
		}
	}
}
