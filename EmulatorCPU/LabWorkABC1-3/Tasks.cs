using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWorkABC1_3
{
	public static class Tasks
	{
		static public void FirstTasks()
		{
			double a = InputValue("a"), b = InputValue("b"), c = InputValue("c");
			SCPU.Task1(a, b, c);
		}

		static public void SecondTasks()
		{
			double a = InputValue("A"), b = InputValue("B"), h = InputValue("H");

			if (a > b)
			{
				Console.WriteLine("Error a > b");
				return;
			}

			if (a + h < a)
			{
				Console.WriteLine("Error a + n < a");
				return;
			}

			double e = InputValue("e");

			string ans = "";

			for (double i = a; i <= b; i += h)
			{
				ans += SCPU.Task2(i, e);
				ans += "\n";
			}

			Console.WriteLine(ans);
		}

		static public void KoshiTask(string method)
		{
			Dictionary<string, double> data = InputDataForKoshi();

			//Dictionary<string, double> data = TestData();

			if (data == null)
				return;

			List<double> yValues = new List<double> { data["y0"] };
			Console.WriteLine("y(0) = {0}, x(0) = {1}", data["y0"], data["a"]);

			double yCurr = data["y0"];
	
			if (method == "Adams")
			{
				double fxy = SCPU.FindFXY(data, data["a"], data["y0"]);
				yValues.Add(yValues[0] + data["h"] / 2 * 3 * fxy);
			}
			
			for (double xCurr = data["a"]; xCurr <= data["b"]; xCurr += data["h"])
			{
				if (method == "Eiler")
				{
					double fxy = SCPU.FindFXY(data, xCurr, yCurr);
					yCurr = SCPU.Eiler(yCurr, data["h"], fxy, data["visual"]);
					yValues.Add(yCurr);
				}
				else
				if (method == "Adams" && xCurr != data["a"])
				{
					double fxy0 = SCPU.FindFXY(data, xCurr - data["h"], yValues[yValues.Count() - 2]);
					double fxy1 = SCPU.FindFXY(data, xCurr, yValues[yValues.Count() - 1]);
					yCurr = SCPU.Adams(fxy0, fxy1, yValues[yValues.Count() - 1], data["h"], data["visual"]);
					//Console.WriteLine("\n Answer" + ' ' + fxy0+ ' ' + fxy1 + ' ' + yCurr);
					yValues.Add(yCurr);

				}
			}

			int i = 0;
			Console.WriteLine("\nTable calculations of method " + method + ":");
			for (double x = data["a"]; x <= data["b"]; x += data["h"], i++)
			{
				Console.WriteLine("y({0}) = {1})", x, yValues[i]);
			}
		}

		private static double InputValue(string varible)
		{
			double value;
			while (true)
			{
				Console.Write("Please input " + varible + " = ");
				string s = Console.ReadLine();
				if (double.TryParse(s, out value))
					break;
				else
					Console.WriteLine("Repeat the command!");
			}

			return value;
		}

		private static Dictionary<string, double> InputDataForKoshi()
		{
			Dictionary<string, double> data = new Dictionary<string, double>();
			Console.WriteLine("\nODU: k1 * y' + k2 * p(x)y = k3 * q(x)y^n");

			data.Add("px", InputValue("step. x in p(x)"));
			data.Add("qx", InputValue("step. x in q(x)"));
			data.Add("yn", InputValue("step. y in y^n"));
			data.Add("k1", InputValue("k1"));
			data.Add("k2", InputValue("k2"));
			data.Add("k3", InputValue("k3"));
			data.Add("y0", InputValue("y0"));
			data.Add("a", InputValue("a"));
			data.Add("b", InputValue("b"));
			data.Add("h", InputValue("h (recom. h = 0.001)"));

			if (data["a"] > data["b"])
			{
				Console.WriteLine("Error a > b");
				return null;
			}

			if (data["a"] + data["h"] < data["a"])
			{
				Console.WriteLine("Error a + n < a");
				return null;
			}

			if (Math.Abs(data["k1"]) < 0.0000000001)
			{
				Console.WriteLine("Error k1 cannot be equal 0");
				return null;
			}

			string pxStr = data["px"] == 0 ? "" : "* x^" + data["px"].ToString();
			string qxStr = data["qx"] == 0 ? "" : "* x^" + data["qx"].ToString();
			string ynStr = data["yn"] == 0 ? "" : "* y^" + data["yn"].ToString();

			pxStr = data["px"] == 1 ? "* x" : pxStr;
			qxStr = data["qx"] == 1 ? "* x" : qxStr;
			ynStr = data["yn"] == 1 ? "* y" : ynStr;

			data["k2"] /= data["k1"];
			data["k3"] /= data["k1"];

			Console.WriteLine("The equation: y' + {0} {1} * y = {2} {3} {4}  x = [{5}..{6}] with step {7} and y(0)={8}", 
				data["k2"].ToString(), pxStr, data["k3"].ToString(), qxStr, ynStr, 
				data["a"].ToString(), data["b"].ToString(), data["h"].ToString(), data["y0"].ToString());

			Console.WriteLine("Visualize the calculations? (y/n)");
			char visual = (char)Console.Read();
			data["visual"] = visual == 'n' ? 0 : 1;

			return data;
		}

		private static Dictionary<string, double> TestData()
		{
			Dictionary<string, double> data = new Dictionary<string, double>
			{
				{ "px", -1 },
				{ "qx", 0},
				{ "yn", 2},
				{ "k1", 3},
				{ "k2", 3},
				{ "k3", 1},
				{ "y0", 1},
				{ "a", 1},
				{ "b", 5},
				{ "h", 0.0015625}
			};

			data["k2"] /= data["k1"];
			data["k3"] /= data["k1"];
			data["visual"] = 0;

			return data;
		}
	}
}
