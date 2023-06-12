using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using YeelightAPI;
using YeelightAPI.Models.ColorFlow;

namespace YeelightControl {
	class Program {
		static void StartFlow(List<Device> devices, ColorFlow flow) {
			devices.ForEach(async device => await device.StopColorFlow());
			devices.ForEach(async device => await device.StartColorFlow(flow));
		}

		static void StartFlow(DeviceGroup devices, ColorFlow flow) {
			devices.StopColorFlow();
			devices.StartColorFlow(flow);
		}

		static void RGBFlow(List<Device> devices, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowRGBExpression(255, 0, 0, brightness, 2000),
				new ColorFlowRGBExpression(0, 255, 0, brightness, 2000),
				new ColorFlowRGBExpression(0, 0, 255, brightness, 2000),
			};

			StartFlow(devices, flow);
		}

		static void RGBFlow(DeviceGroup devices, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowRGBExpression(255, 0, 0, brightness, 2000),
				new ColorFlowRGBExpression(0, 255, 0, brightness, 2000),
				new ColorFlowRGBExpression(0, 0, 255, brightness, 2000),
			};

			StartFlow(devices, flow);
		}

		static void ChangeTempFlow(List<Device> devices, int temperature, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowTemperatureExpression(temperature, brightness, 1000)
			};

			StartFlow(devices, flow);
		}

		static void ChangeTempFlow(DeviceGroup devices, int temperature, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowTemperatureExpression(temperature, brightness, 1000)
			};

			StartFlow(devices, flow);
		}

		static void ResetFlow(List<Device> devices, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowTemperatureExpression(4500, brightness, 1000)
			};

			StartFlow(devices, flow);
		}

		static void ResetFlow(DeviceGroup devices, int brightness) {
			ColorFlow flow = new(0, ColorFlowEndAction.Keep) {
				new ColorFlowTemperatureExpression(4500, brightness, 1000)
			};

			StartFlow(devices, flow);
		}

		static async Task Run(List<string> args) {
			if (!ArgsValidation.Check(args)) return;

			var deviceList = (await DeviceLocator.DiscoverAsync()).ToList();
			// var devices = new DeviceGroup(await DeviceLocator.DiscoverAsync());

			if (deviceList.Count == 0) {
				Output.NoBulb();
				return;
			}

			try {
				deviceList.ForEach(async device => await device.Connect());
				// await devices.Connect();

				switch (args[0]) {
					case (Commands.On):
						deviceList.ForEach(async device => await device.TurnOn(1000));
						// await devices.TurnOn(1000);
						break;
					case (Commands.Off):
						deviceList.ForEach(async device => await device.TurnOff(1000));
						// await devices.TurnOff(1000);
						break;
					case (Commands.Set):
						deviceList.ForEach(async device => await device.SetBrightness(Convert.ToInt32(args[1]), 1000));
						// await devices.SetBrightness(Convert.ToInt32(args[1]), 1000);
						break;
					case (Commands.Toggle):
						deviceList.ForEach(async device => await device.Toggle());
						// await devices.Toggle();
						break;
					case (Commands.RGB):
						RGBFlow(deviceList, Convert.ToInt32(args[1]));
						break;
					case (Commands.Temperature):
						ChangeTempFlow(deviceList, Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
						break;
					case (Commands.Reset):
						ResetFlow(deviceList, Convert.ToInt32(args[1]));
						break;
				}

				deviceList.ForEach(device => device.Disconnect());
				// devices.Disconnect();
			} catch (Exception ex) {
				Output.Error(ex.Message);
			}
		}

		static async Task Main(string[] args) {
			await Run(args.Select(x => x.ToLower()).ToList());
		}
	}
}
