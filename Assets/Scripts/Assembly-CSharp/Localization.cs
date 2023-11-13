using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
	public Font Font_English_Russian;

	public Font Font_Japanese;

	public Font Font_Chinese;

	public Font Font_Korean;

	public Font Font_OtherEu;

	private readonly Dictionary<string, string> _lang = new Dictionary<string, string>();

	private readonly Dictionary<string, string> _dialog_lang = new Dictionary<string, string>();

	private string language = "english";

	private WorldScript world_script;

	public void Translate()
	{
		world_script = GetComponent<WorldScript>();
		if (!PlayerPrefs.HasKey("language"))
		{
			PlayerPrefs.SetString("language", "english");
			language = "english";
		}
		else
		{
			language = PlayerPrefs.GetString("language");
		}
		ChangeFonts();
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
		world_script.ui_resume_text.text = _lang["pause_resume_text"];
		world_script.ui_restart_text.text = _lang["pause_restart_text"];
		world_script.ui_exit_text.text = _lang["pause_exit_text"];
		world_script.loading_text.text = _lang["loading"];
		world_script.ui_travel_text.text = _lang["travel"];
		world_script.ui_travel_exit_text.text = _lang["fly_exit_button"];
		world_script.ui_skill_book_button_select_text.text = _lang["select_button"];
		world_script.ui_skill_book_button_back_text.text = _lang["back_button"];
		world_script.ui_close_book_text.text = _lang["close_button"];
		world_script.ui_spell_page_text.text = _lang["spell_button"];
		world_script.ui_spell_map_text.text = _lang["map_button"];
		world_script.ui_spell_main_text.text = _lang["menu_button"];
		world_script.ui_map_spell_text.text = _lang["spell_button"];
		if (world_script.ui_boss_name != null)
		{
			world_script.ui_boss_name.text = _lang["boss_" + world_script.this_lvl];
		}
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
		GameObject[] array5 = GameObject.FindGameObjectsWithTag("tip");
		GameObject[] array6 = array5;
		foreach (GameObject gameObject in array6)
		{
			gameObject.GetComponent<Tip>().text.GetComponent<TextMesh>().text = _lang[gameObject.GetComponent<Tip>().action];
		}
		GameObject[] array7 = GameObject.FindGameObjectsWithTag("action");
		GameObject[] array8 = array7;
		foreach (GameObject gameObject2 in array8)
		{
			gameObject2.GetComponent<action_button>().text.GetComponent<TextMesh>().text = _lang[gameObject2.GetComponent<action_button>().action];
		}
		GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().ChapterText.text = _lang["chapter_" + world_script.this_lvl];
		GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().ChapterNumber.text = _lang["chapter_name"] + " " + (world_script.this_lvl + 1);
	}

	public void ChangeFonts()
	{
		if (language == "russian" || language == "english")
		{
			world_script.ui_resume_text.font = Font_English_Russian;
			world_script.ui_restart_text.font = Font_English_Russian;
			world_script.ui_exit_text.font = Font_English_Russian;
			world_script.ui_skill_book_button_select_text.font = Font_English_Russian;
			world_script.ui_skill_book_button_back_text.font = Font_English_Russian;
		}
		else if (language == "french" || language == "spanish" || language == "german" || language == "italian" || language == "portuguese" || language == "turkish" || language == "danish" || language == "dutch" || language == "polish" || language == "turkish")
		{
			world_script.ui_resume_text.font = Font_OtherEu;
			world_script.ui_restart_text.font = Font_OtherEu;
			world_script.ui_exit_text.font = Font_OtherEu;
			world_script.ui_skill_book_button_select_text.font = Font_OtherEu;
			world_script.ui_skill_book_button_back_text.font = Font_OtherEu;
		}
		else if (language == "japanese")
		{
			world_script.ui_resume_text.font = Font_Japanese;
			world_script.ui_restart_text.font = Font_Japanese;
			world_script.ui_exit_text.font = Font_Japanese;
			world_script.ui_skill_book_button_select_text.font = Font_Japanese;
			world_script.ui_skill_book_button_back_text.font = Font_Japanese;
		}
		else if (language == "simplchinese")
		{
			world_script.ui_resume_text.font = Font_Chinese;
			world_script.ui_restart_text.font = Font_Chinese;
			world_script.ui_exit_text.font = Font_Chinese;
			world_script.ui_skill_book_button_select_text.font = Font_Chinese;
			world_script.ui_skill_book_button_back_text.font = Font_Chinese;
		}
		else if (language == "tradchinese")
		{
			world_script.ui_resume_text.font = Font_Chinese;
			world_script.ui_restart_text.font = Font_Chinese;
			world_script.ui_exit_text.font = Font_Chinese;
			world_script.ui_skill_book_button_select_text.font = Font_Chinese;
			world_script.ui_skill_book_button_back_text.font = Font_Chinese;
		}
		else if (language == "korean")
		{
			world_script.ui_resume_text.font = Font_Korean;
			world_script.ui_restart_text.font = Font_Korean;
			world_script.ui_exit_text.font = Font_Korean;
			world_script.ui_skill_book_button_select_text.font = Font_Korean;
			world_script.ui_skill_book_button_back_text.font = Font_Korean;
		}
		else
		{
			world_script.ui_resume_text.font = Font_English_Russian;
			world_script.ui_restart_text.font = Font_English_Russian;
			world_script.ui_exit_text.font = Font_English_Russian;
			world_script.ui_skill_book_button_select_text.font = Font_English_Russian;
			world_script.ui_skill_book_button_back_text.font = Font_English_Russian;
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
