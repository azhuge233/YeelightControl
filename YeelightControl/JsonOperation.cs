using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace YeelightControl {
	class JsonOperation : IDisposable {

		public static void WriteData(List<string> data, string path) {
			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			File.WriteAllText(path, string.Empty);
			File.WriteAllText(path, json);
		}

		public static List<string> LoadData(string path) {
			var content = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<List<string>>(content);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
