using System;
using System.Collections.Generic;

namespace UnityEngine
{
	namespace Config
	{
		public class FileMap
		{
			public bool IsEmpty()
			{
				return String.IsNullOrEmpty(guid) && (String.IsNullOrEmpty(fileID) || fileID == "0");
			}

			public string Key()
			{
				return guid + fileID;
			}

			public string fileID;
			public string guid;
			public int type;

			public FileMap()
			{
			}

			public void FileMap_Set(string fileID, string guid, int type)
			{
				this.fileID = fileID;
				this.guid = guid;
				this.type = type;
			}

			public static FileMap FromJson(string json)
			{
				var dict = CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
				return FromJson(dict);
			}

			public static FileMap FromJson(Dictionary<string, System.Object> json)
			{
				var t = new FileMap();
				t.FileMap_SetFromJson(json);
				return t;
			}

			public void FileMap_SetFromJson(Dictionary<string, System.Object> json)
			{
				if (json.ContainsKey("fileID")) fileID = json["fileID"].ToString();
				if (json.ContainsKey("guid")) guid = json["guid"].ToString();
				if (json.ContainsKey("type")) type = Logic.Convert.ToInt32(json["type"]);
			}
		}

		public class ImporterDictionaryStringString
		{
			public PairStringString[] data;
			public void ImporterDictionaryStringString_Set(PairStringString[] data)
			{
				this.data = data;
			}
			public static ImporterDictionaryStringString FromJson(string json)
			{
				var dict = CyberyJson.Deserialize(json) as Dictionary<string, System.Object>;
				return FromJson(dict);
			}

			public static ImporterDictionaryStringString FromJson(Dictionary<string, System.Object> json)
			{
				var t = new ImporterDictionaryStringString();
				t.ImporterDictionaryStringString_SetFromJson(json);
				return t;
			}

			protected void ImporterDictionaryStringString_SetFromJson(Dictionary<string, System.Object> json)
			{
				List<PairStringString> t = new List<PairStringString>();
				foreach (var kvp in json)
				{
					PairStringString pss = new PairStringString();
					pss.PairStringString_Set(kvp.Key, kvp.Value.ToString());
					t.Add(pss);
				}
				data = t.ToArray();
			}

			public string GetValueFromKey(string key)
			{
				foreach (var pss in data)
				{
					if (pss.first == key) return pss.second;
				}
				return "";
			}
		}

	}
}

