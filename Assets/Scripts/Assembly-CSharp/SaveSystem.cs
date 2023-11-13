using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
	public static void SaveData(Data data)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		string path = Application.dataPath + "/Saves/save.pidg";
		FileStream fileStream = new FileStream(path, FileMode.Create);
		SavedData graph = new SavedData(data);
		binaryFormatter.Serialize(fileStream, graph);
		fileStream.Close();
	}

	public static SavedData LoadData()
	{
		string text = Application.dataPath + "/Saves/save.pidg";
		if (File.Exists(text))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(text, FileMode.Open);
			SavedData result = binaryFormatter.Deserialize(fileStream) as SavedData;
			fileStream.Close();
			return result;
		}
		Debug.LogError("No save at " + text);
		return null;
	}
}
