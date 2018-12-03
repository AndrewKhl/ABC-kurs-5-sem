using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			Checker.Print("Emulator ALU v. 3.0.1");
			MyConsole();
		}

		private static void MyConsole()
		{
			while(true)
			{
				Console.Write(".");
				string command = Console.ReadLine();
				if (command != "") 
				{
					if (Checker.IsConsoleCommand(command) == true) MyConsoleCommand(command);
					else
						MyCommand(command);
				}
			}
		}

		private static void MyConsoleCommand(string command)
		{

			command = Checker.FormattingMyConsoleCommand(command);

			switch(command)
			{
				case "status":
					CPU.Status();
					break;
				case "clear":
					CPU.Clear();
					break;
				case "exit":				
					Environment.Exit(0);
					break; 
				default:
					Console.WriteLine("Error console command!");
					break;
			}
		}

		private static void MyCommand(string command)
		{

			string[] words = Checker.FormattingMyCommand(command);
			switch(words[0])
			{
				case "mov":

					if (!Checker.CheckCommandMOVandADD(words))
						Console.WriteLine("Error entering command MOV");
					else
						ALU.MOV(words[1], words[2]);
					break;

				case "add":
					CPU.ClearFlags();
					if (!Checker.CheckCommandMOVandADD(words))
						Console.WriteLine("Error entering command ADD");
					else
						ALU.ADD(words[1], words[2]);
					break;

				case "sub":
					CPU.ClearFlags();
					if (!Checker.CheckCommandMOVandADD(words))
						Console.WriteLine("Error entering command SUB");
					else
						ALU.SUB(words[1], words[2]);
					break;

				case "mul":
					CPU.ClearFlags();
					if (!Checker.CheckCommandMULandDIV(words))
						Console.WriteLine("Error entering command MUL");
					else
						ALU.MUL(words[1]);
					break;

				case "div":
					CPU.ClearFlags();
					if (!Checker.CheckCommandMULandDIV(words))
						Console.WriteLine("Error entering command DIV");
					else
						ALU.DIV(words[1]);
					break;

				case "fmov":
					if (!Checker.CheckCommandFMOV(words))
						Console.WriteLine("Error entering command FMOV");
					else
						SCPU.FMOV(words[1], words[2]);
					break;

				case "fadd":
					if (!Checker.CheckCommandFloat(words))
						Console.WriteLine("Error entering command FADD");
					else
						SCPU.FADD(words[1], words[2]);
					break;

				case "fsub":
					if (!Checker.CheckCommandFloat(words))
						Console.WriteLine("Error entering command FSUB");
					else
						SCPU.FSUB(words[1], words[2]);
					break;

				case "fmul":
					if (!Checker.CheckCommandFloat(words))
						Console.WriteLine("Error entering command FMUL");
					else
						SCPU.FMUL(words[1], words[2]);
					break;

				case "fdiv":
					if (!Checker.CheckCommandFloat(words))
						Console.WriteLine("Error entering command FDIV");
					else
						SCPU.FDIV(words[1], words[2]);
					break;

				case "tasks":
					if (words.Length == 1)
						MyTasks();
					else
						Console.WriteLine("Error entering command TASKS");
					break;

				default:
					Console.WriteLine("Error command!");
					break;
			}
		}

		private static void MyTasks()
		{
			Console.WriteLine("Please, choose number tasks:");
			Console.WriteLine("1) a*x^2 + b*x + c = 0");
			Console.WriteLine("2) S(x) = Sum((-1)^k * x^(2 * k + 1) / (2 * k + 1)!)\n   Y(x) = sin(x)\n   while e < |Y(x) - S(x)|\n   On A to B, step = H");
			Console.WriteLine("3) Koshi task for ODU (Eiler method)");
			Console.WriteLine("4) Koshi task for ODU (Adams method)");
			string s = Console.ReadLine();
			int x;
			try
			{
				x = Int32.Parse(s);
				if (x < 1 && x > 4)
					throw new Exception();
			}
			catch
			{
				Console.WriteLine("Error number!");
				return;
			}

			switch (x)
			{
				case 1:
					Tasks.FirstTasks();
					break;
				case 2:
					Tasks.SecondTasks();
					break;
				case 3:
					Tasks.KoshiTask("Eiler");
					break;
				case 4:
					Tasks.KoshiTask("Adams");
					break;
				default:
					break;
			}
		}
	}
}
