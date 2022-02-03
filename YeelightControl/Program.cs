using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using YeelightAPI;
using YeelightAPI.Models.ColorFlow;

namespace YeelightControl {
	class Program {
		static List<string> LoadBulbs() {
			var bulbs = new List<string>();
			try {
				bulbs = JsonOperation.LoadData();
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

		static async Task RGBFlow(DeviceGroup devices, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowRGBExpression(255, 0, 0, brightness, 2000),
				new ColorFlowRGBExpression(0, 255, 0, brightness, 2000),
				new ColorFlowRGBExpression(0, 0, 255, brightness, 2000),
			};

			await devices.StopColorFlow();
			await devices.StartColorFlow(flow);
		}

		static async Task ChangeTempFlow(DeviceGroup devices, int temperature, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowTemperatureExpression(temperature, brightness, 1000)
			};

			await devices.StopColorFlow();
			await devices.StartColorFlow(flow);
		}

		static async Task ResetFlow(DeviceGroup devices, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowTemperatureExpression(4500, brightness, 1000)
			};

			await devices.StopColorFlow();
			await devices.StartColorFlow(flow);
		}

		static async Task Run(List<string> args) {
			if (!ArgsValidation.Check(args)) return;

			var devices = GetDevices();

			if (devices.Count == 0) {
				Output.NoBulb();
				return;
			}

			// two try-catch in case of one or more bulb failures
			try {
				await devices.Connect();
			} catch (Exception ex) {
				Output.Error(ex.Message);
			}

			try {
				switch (args[0]) {
					case (Commands.On):
						await devices.TurnOn(1000);
						break;
					case (Commands.Off):
						await devices.TurnOff(1000);
						break;
					case (Commands.Set):
						await devices.SetBrightness(Convert.ToInt32(args[1]), 1000);
						break;
					case (Commands.Toggle):
						await devices.Toggle();
						break;
					case (Commands.RGB):
						await RGBFlow(devices, Convert.ToInt32(args[1]));
						break;
					case (Commands.Temperature):
						await ChangeTempFlow(devices, Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
						break;
					case (Commands.Reset):
						await ResetFlow(devices, Convert.ToInt32(args[1]));
						break;
				}
			} catch (Exception ex) {
				Output.Error(ex.Message);
			} finally {
				devices.Disconnect();
			}
		}

		static async Task Main(string[] args) {
			await Run(args.Select(x => x.ToLower()).ToList());
		}
	}
}
