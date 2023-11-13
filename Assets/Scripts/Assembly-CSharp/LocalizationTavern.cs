using System.Collections.Generic;
using UnityEngine;

public class LocalizationTavern : MonoBehaviour
{
	public Font Font_English_Russian;

	public Font Font_Japanese;

	public Font Font_Chinese;

	public Font Font_Korean;

	public Font Font_OtherEu;

	private readonly Dictionary<string, string> _lang = new Dictionary<string, string>();

	private readonly Dictionary<string, string> _dialog_lang = new Dictionary<string, string>();

	private string language = "english";

	private Tavern this_tavern;

	public void Translate()
	{
		this_tavern = GetComponent<Tavern>();
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
		this_tavern.few_text.text = _lang["few_days"];
		TextAsset textAsset2 = Resources.Load<TextAsset>("dialog_" + language);
		if (textAsset2 == null)
		{
			textAsset2 = Resources.Load<TextAsset>("dialog_english");
		}
		string[] array3 = textAsset2.text.Split('\n');
		foreach (string text2 in array3)
		{
			string[] array4 = text2.Split('=');
			_dialog_lang[array4[0]] = array4[1];
		}
		GameObject[] array5 = GameObject.FindGameObjectsWithTag("action");
		GameObject[] array6 = array5;
		foreach (GameObject gameObject in array6)
		{
			gameObject.GetComponent<action_button>().text.GetComponent<TextMesh>().text = _lang[gameObject.GetComponent<action_button>().action];
		}
	}

	public string GetLineDescription(string line)
	{
		return _lang[line];
	}

	public string GetDialogLine(string line_name)
	{
		return _dialog_lang["dialog_" + line_name];
	}
}
