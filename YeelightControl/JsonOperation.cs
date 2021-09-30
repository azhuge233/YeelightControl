using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace YeelightControl {
	class JsonOperation : IDisposable {
		private static readonly string configPath = $"{AppContext.BaseDirectory}config.json";

		public static List<string> LoadData() {
			var content = File.ReadAllText(configPath);
			return JsonConvert.DeserializeObject<List<string>>(content);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
