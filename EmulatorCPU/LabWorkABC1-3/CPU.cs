using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	static class CPU
	{
		public static Hashtable registors;
		public static Dictionary<string, bool> flags;
		static string[] nameflags = { "OF", "SF", "ZF", "AF", "PF", "CF" };

		static CPU()
		{
			Register a = new Register("A");
			Register b = new Register("B");
			Register c = new Register("C");
			Register d = new Register("D");

			registors = new Hashtable
			{
				{ "a", a },
				{ "b", b },
				{ "c", c },
				{ "d", d }
			};
			flags = new Dictionary<string, bool>();
			foreach(var name in nameflags)
			{
				flags.Add(name, false);
			}
			
		}

		public static void Status()
		{
			foreach(DictionaryEntry reg in registors)
			{
				(reg.Value as Register).Info();
			}
			Console.WriteLine();

			foreach (var fg in flags)
			{
				Console.Write("{0}={1} ", fg.Key, Convert.ToSByte(fg.Value));
			}
			Console.Write("\n\n");
			SCPU.Info();
			Console.Write("\n\n");
		}

		public static void ClearFlags()
		{
			foreach (var name in nameflags)
			{
				flags[name] = false;
			}
		}

		public static void Clear()
		{
			foreach (DictionaryEntry reg in registors)
			{
				(reg.Value as Register).Clear();
			}

			ClearFlags();
			SCPU.Clear();
		}

		public static void CreateFlags(long number, bool isSmall)
		{
			if (number == 0)
				flags["ZF"] = true;
			if (!isSmall && number > 65535)
				flags["CF"] = true;
			else
				if (isSmall && number > 256)
				flags["CF"] = true;

			int countone = Convert.ToString(number, 2).Count(x => x == '1');
			if (countone % 2 == 0)
				flags["PF"] = true;

			if (number < 0)
				flags["SF"] = true;
		}
	}

	class Register
	{
		public int h, l;
		public bool neg;
		string name;

		public void Info()
		{
			Console.WriteLine(name + "X");
			Console.WriteLine("{0} {1} = {2}", h.ToString("X2"), l.ToString("X2"), ReturnNumber());
		}

		public void Clear()
		{
			h = 0;
			l = 0;
			neg = false;
		}

		public Register(string name)
		{
			neg = false;
			h = 0;
			l = 0;
			this.name = name;
		}

		public int ReturnNumber()
		{
			string val = h.ToString("X2") + l.ToString("X2");
			if (!neg)
				return Convert.ToInt32(val, 16);
			else
				return Convert.ToInt32(val, 16) - 65535 - 1;
		}

		public int ReturnNumber(string word)
		{
			switch(word[1])
			{
				case 'x':
					return ReturnNumber();
				case 'h':
					return h;
				default:
					return l;
			}
		}
	}
}
