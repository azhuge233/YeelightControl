using System;

namespace YeelightControl {
	class Output {
		public static void Usage() {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Usage (Ignore cases) :");
			Console.WriteLine(
				"\ton - trun lights on\n" +
				"\toff - turn off lights\n" +
				"\tset [0-100] - set [brightness]\n" +
				"\ttoggle - toggle bulb on/off\n" +
				"\trgb [1-100] - trun on RGB flow to [target brightness]\n" +
				"\ttemp [temperature in k (e.g. 4500)] [1-100] - change temperature to [target brightness]\n" +
				"\treset [1-100] - reset (4500k temperature) to [target brightness]"
			);
			Console.ResetColor();
		}

		public static void LackOfArgument() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Lack of argument");
			Console.ResetColor();
			Usage();
		}

		public static void NoArgumentProvided() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("No argument provided");
			Console.ResetColor();
			Usage();
		}

		public static void TooManyArgs() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Too many arguments");
			Console.ResetColor();
			Usage();
		}

		public static void InvalidInput() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Invalid input");
			Console.ResetColor();
			Usage();
		}

		public static void InvalidInput(string str) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Invalid arg: {0}", str);
			Console.ResetColor();
			Usage();
		}

		public static void NoBulb() {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("No bulb found, add IP addresses to config.json first.");
			Console.ResetColor();
		}

		public static void Error(string msg) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Task failed.\n" + "Error message: " + msg);
			Console.ResetColor();
		}
	}
}
