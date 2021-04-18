using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using YeelightAPI;

namespace YeelightControl {
	class Program {
		#region variables
		static readonly string path = @$"{AppContext.BaseDirectory}config.json";
		static readonly List<string> availCommands = new () {
			"on", "off", "set"
		};
		#endregion

		static bool CheckValid(List<string> args) {
			if (args.Count == 0) {
				Output.InvalidInput("No argument provided");
				Output.Usage();
				return false;
			}

			if (args.Count > 2) {
				Output.InvalidInput("Too many arguments");
				Output.Usage();
				return false;
			}

			if (!availCommands.Any(x => x == args[0])) {
				Output.InvalidInput(args[0]);
				Output.Usage();
				return false;
			}

			if ((args[0] == availCommands[0] || args[0] == availCommands[1]) && args.Count > 1) {
				Output.InvalidInput(args[1]);
				Output.Usage();
				return false;
			}

			if (args[0] == availCommands[2]) {
				if (args.Count < 2) {
					Output.InvalidInput("lack of argument");
					Output.Usage();
					return false;
				}
				if (!int.TryParse(args[1], out int tmp)) {
					Output.InvalidInput(args[1]);
					Output.Usage();
					return false;
				}
			}

			return true;
		}

		static List<string> LoadBulbs() {
			var bulbs = new List<string>();
			try {
				bulbs = JsonOperation.LoadData(path: path);
			} catch (Exception ex) {
				Output.Error(ex.Message);
			}
			return bulbs;
		}

		static DeviceGroup GetDevices() {
			var bulbs = LoadBulbs();
			var devicesGroup = new DeviceGroup();
			foreach (var bulb in bulbs) {
				var device = new Device(bulb);
				devicesGroup.Add(device);
			}
			return devicesGroup;
		}

		static async Task Run(List<string> args) {
			if (!CheckValid(args)) return;

			var devicesGroup = GetDevices();

			if (devicesGroup.Count == 0) {
				Output.NoBulb();
				return;
			}

			try {
				await devicesGroup.Connect();

				if (args[0] == availCommands[0]) {
					await devicesGroup.TurnOn(smooth: 1000);
				} else if (args[0] == availCommands[1]) {
					await devicesGroup.TurnOff(smooth: 1000);
				} else if (args[0] == availCommands[2]) {
					var bright = Convert.ToInt32(args[1]);
					await devicesGroup.SetBrightness(value: bright, smooth: 1000);
				}
			} catch (Exception ex) {
				Output.Error(ex.Message);
			}

		}

		static async Task Main(string[] args) {
			var argsl = args.ToList();
			await Run(argsl);
		}
	}
}
