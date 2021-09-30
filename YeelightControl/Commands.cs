using System.Collections.Generic;
using System.Linq;

namespace YeelightControl {
	public static class Commands {
		public const string On = "on";
		public const string Off = "off";
		public const string Set = "set";
		public const string Toggle = "toggle";
		public const string RGB = "rgb";
		public const string Temperature = "temp";
		public const string Reset = "reset";

		public static bool Any(string arg) {
			return new List<string> { On, Off, Set, Toggle, RGB, Temperature, Reset }.Any(x => x == arg);
		}
	}
}
