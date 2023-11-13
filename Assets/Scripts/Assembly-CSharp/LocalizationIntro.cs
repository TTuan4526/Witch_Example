using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationIntro : MonoBehaviour
{
	public Font Font_English_Russian;

	public Font Font_Japanese;

	public Font Font_Chinese;

	public Font Font_Korean;

	public Font Font_OtherEu;

	private readonly Dictionary<string, string> _lang = new Dictionary<string, string>();

	private string language = "english";

	public Text dialog_text;

	public void Localizate()
	{
		if (!PlayerPrefs.HasKey("language"))
		{
			PlayerPrefs.SetString("language", "english");
			language = "english";
		}
		else
		{
			language = PlayerPrefs.GetString("language");
		}
		TextAsset textAsset = Resources.Load<TextAsset>("game_" + language);
		if (textAsset == null)
		{
			textAsset = Resources.Load<TextAsset>("game_english");
		}
		string[] array = textAsset.text.Split('\n');
		foreach (string text in array)
		{
			string[] array2 = text.Split('=');
			_lang[array2[0]] = array2[1];
		}
	}

	public string GetDialogLine(string line_name)
	{
		return _lang[line_name];
	}

	public void ChangeFonts()
	{
		if (language == "russian" || language == "english")
		{
			dialog_text.font = Font_English_Russian;
		}
		else if (language == "french" || language == "spanish" || language == "german" || language == "italian" || language == "portuguese" || language == "turkish" || language == "danish" || language == "dutch" || language == "polish" || language == "turkish")
		{
			dialog_text.font = Font_OtherEu;
		}
		else if (language == "japanese")
		{
			dialog_text.font = Font_Japanese;
		}
		else if (language == "simplchinese")
		{
			dialog_text.font = Font_Chinese;
		}
		else if (language == "tradchinese")
		{
			dialog_text.font = Font_Chinese;
		}
		else if (language == "korean")
		{
			dialog_text.font = Font_Korean;
		}
		else
		{
			dialog_text.font = Font_English_Russian;
		}
	}
}
