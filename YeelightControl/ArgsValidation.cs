using System.Collections.Generic;

namespace YeelightControl {
	static class ArgsValidation {
		public static bool Check(List<string> args) {
			if (args.Count == 0) {
				Output.NoArgumentProvided();
				return false;
			}

			if (args.Count > 3) {
				Output.TooManyArgs();
				return false;
			}

			if (!Commands.Any(args[0])) {
				Output.InvalidInput(args[0]);
				return false;
			}

			if ((args[0] == Commands.On
				|| args[0] == Commands.Off
				|| args[0] == Commands.Toggle)
				&& args.Count > 1) {
				Output.InvalidInput(args[1]);
				return false;
			}

			if (args[0] == Commands.Set
				|| args[0] == Commands.RGB
				|| args[0] == Commands.Reset) {
				if (args.Count < 2) {
					Output.LackOfArgument();
					return false;
				}

				if (!int.TryParse(args[1], out _)) {
					Output.InvalidInput(args[1]);
					return false;
				}
			}

			if (args[0] == Commands.Temperature) {
				if (args.Count < 3) {
					Output.LackOfArgument();
					return false;
				}
				for (int i = 1; i < args.Count; i++) {
					if (!int.TryParse(args[i], out _)) {
						Output.InvalidInput(args[i]);
						return false;
					}	
				}
			}

			return true;
		}
	}
}
