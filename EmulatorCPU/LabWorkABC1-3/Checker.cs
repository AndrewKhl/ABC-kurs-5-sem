using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	static class Checker
	{
		public static void Print(string text)
		{
			var width = Console.WindowWidth;
			var padding = width / 2 + text.Length / 2;
			Console.WriteLine("{0," + padding + "}", text);
		}


		public static bool IsConsoleCommand(string text)
		{
			if (text[0] == '>')
				return true;
			else
				return false;
		}

		public static string[] FormattingMyCommand(string command)
		{
			command = command.ToLower();
			string[] words = command.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			return words;
		}

		public static string FormattingMyConsoleCommand(string command)
		{
			command = command.Remove(0, 1);
			command = command.Replace(" ", "");
			command = command.ToLower();
			return command;
		}

		public static bool CheckCommandMOVandADD(string[] words)
		{
			if (words.Length != 3)
				return false;

			if (IsRegister(words[1]) == false)
				return false;

			if (IsNumber(words[2]) == false && IsRegister(words[2]) == false)
				return false;
				
			return true;
		}

		public static bool CheckCommandFMOV(string[] words)
		{
			if (words.Length != 3)
				return false;
			if (IsSReg(words[1]) == false)
				return false;
			if (IsSReg(words[2]) == true)
				return true;
			
			if (double.TryParse(words[2], out double val))
				return true;
			else
				return false;
		}

		public static bool CheckCommandFloat(string[] words)
		{
			if (words.Length != 3)
				return false;
			if (!IsSReg(words[1]) || !IsSReg(words[2]))
				return false;

			return true;
		}

		public static bool IsSReg(string s)
		{
			if (s.Length != 2)
				return false;
			if (s[0] != 'r')
				return false;
			if (s[1] < '0' || s[1] > '7')
				return false;

			return true;
		}

		public static bool CheckCommandMULandDIV(string[] words)
		{
			if (words.Length != 2)
				return false;

			if (IsRegister(words[1]) == false)
				return false;

			return true;
		}

		public static bool IsRegister(string text)
		{
			if (text.Length != 2)
				return false;

			if ((text[0] == 'a' || text[0] == 'b' || text[0] == 'c' || text[0] == 'd') && (text[1] == 'x' || text[1] == 'h' || text[1] == 'l'))
				return true;
			else
				return false;
		}

		public static bool IsNumber(string text)
		{
			return int.TryParse(text, out int number);
		}

		public static int CheckedNumber(string text, ref bool ok)
		{
			int number = int.Parse(text);
			if (number > 65535 || number < -32767)
			{
				Console.WriteLine("Out range value!");
				ok = true;
				return 0;
			}
			else
				return number;
		}

		public static bool IsNegativResultForADD(int val1, int val2)
		{
			if (val1 < 0 && val2 < 0)
				return true;
			if (val1 < 0 && -val1 > val2)
				return true;
			if (val2 < 0 && -val2 > val1)
				return true;
			return false;
		}

		public static bool IsNegativeResultForMUL(long val1, long val2)
		{
			if (val1 * val2 > 0)
				return false;
			else
				return true;
		}

		public static string GiveMeMyBinaryValue(int val)
		{
			if (val >=0)
				return Convert.ToString(val, 2);

				StringBuilder s = new StringBuilder(Convert.ToString(-val, 2));
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == '1')
						s[i] = '0';
					else
						s[i] = '1';
				}

				return new string('1', 17 - s.Length) + s.ToString();
		}

		public static void ConvertBinaryValueForDIV(long val1, long val2, ref string first, ref string second)
		{
			first = Convert.ToString(Math.Abs(val1), 2);
			second = Convert.ToString(Math.Abs(val2), 2);
		}

		private static string GiveMeMyBinaryValueForMUL(int val)
		{
			if (val >= 0)
				return Convert.ToString(val, 2);

			StringBuilder s = new StringBuilder(Convert.ToString(-val, 2));
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '1')
					s[i] = '0';
				else
					s[i] = '1';
			}

			int k = s.Length - 1;
			while (s[k] == '1')
				s[k--] = '0';

			if (k >= 0)
				s[k] = '1';
			else
				return "1" + s.ToString();

			return s.ToString();
		}

		public static int GiveMeMyNegativeValue(string val)
		{
			StringBuilder s = new StringBuilder(val);
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '1')
					s[i] = '0';
				else
					s[i] = '1';
			}

			return -Convert.ToInt32(s.ToString(), 2);
		}

		public static string NEG(string val)
		{
			StringBuilder s = new StringBuilder(val);
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '1')
					s[i] = '0';
				else
					s[i] = '1';
			}

			int k = s.Length - 1;
			while (s[k] == '1')
				s[k--] = '0';

			if (k >= 0)
				s[k] = '1';
			else
				return "1" + s.ToString();

			return s.ToString();
		}

		public static bool Equals(string val1, string val2)
		{
			if (val1.Length < val2.Length)
				return false;
			if (val1.Length > val2.Length)
				return true;
			for (int i = 0; i < val1.Length; i++)
				if (val1[i] == val2[i])
					continue;
				else
				{
					return val1[i] > val2[i];
				}
			return true;
		}
	}
}
