using System;

namespace YeelightControl {
	class Output {
		public static void Usage() {
			Console.WriteLine("Usage: \n" + "\ton - trun lights on\n" + "\toff - turn off lights\n" + "\tset [0-100] - set brightness");
		}

		public static void InvalidInput() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Invalid input");
			Console.ResetColor();
		}

		public static void InvalidInput(string str) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Invalid input: {0}", str);
			Console.ResetColor();
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
