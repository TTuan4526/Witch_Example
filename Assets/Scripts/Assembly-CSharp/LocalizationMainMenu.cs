using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationMainMenu : MonoBehaviour
{
	public Font Font_English_Russian;

	public Font Font_Japanese;

	public Font Font_Chinese;

	public Font Font_Korean;

	public Font Font_OtherEu;

	public Text crt_settings;

	public Text language_settings;

	public Text music_volume_settings;

	public Text effects_volume_settings;

	public Text fullscreen_settings;

	public Text controller_settings;

	public Text resolution_settings;

	public Text select_text;

	public Text start_button;

	public Text loading_text;

	public Text conf_back_text;

	public Text confirmation_text;

	public Text confirmation_yes;

	public Text confirmation_no;

	public Text settings_button;

	public Text settings_ctrl_button;

	public Text controls_button;

	public Text continue_button;

	public Text exit_button;

	public Text back_button;

	public Text back_controls_button;

	public Text cn_dc_attack;

	public Text cn_dc_move;

	public Text cn_dc_jump;

	public Text cn_dc_magic;

	public Text cn_dc_spell;

	public Text cn_dc_dash;

	public Text cn_dc_element;

	public Text cn_dc_pause;

	public Text pc_dc_attack;

	public Text pc_dc_jump;

	public Text pc_dc_magic;

	public Text pc_dc_spell;

	public Text pc_dc_dash;

	public Text pc_dc_element;

	public Text pc_dc_pause;

	private readonly Dictionary<string, string> _lang = new Dictionary<string, string>();

	private string language = "english";

	private main_menu menu_main;

	public void Translate()
	{
		menu_main = GetComponent<main_menu>();
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
		TextAsset textAsset = Resources.Load<TextAsset>("menu_" + language);
		if (textAsset == null)
		{
			textAsset = Resources.Load<TextAsset>("menu_english");
		}
		string[] array = textAsset.text.Split('\n');
		foreach (string text in array)
		{
			string[] array2 = text.Split('=');
			_lang[array2[0]] = array2[1];
		}
		start_button.text = _lang["start_button"];
		settings_button.text = _lang["settings_button"];
		controls_button.text = _lang["controls_button"];
		settings_ctrl_button.text = _lang["settings_button"];
		continue_button.text = _lang["continue_button"];
		exit_button.text = _lang["exit_button"];
		back_button.text = _lang["back_button"];
		back_controls_button.text = _lang["back_button"];
		confirmation_text.text = _lang["confirmation_text"];
		confirmation_yes.text = _lang["yes_phrase"];
		confirmation_no.text = _lang["no_phrase"];
		loading_text.text = _lang["loading"];
		select_text.text = _lang["select_button"];
		conf_back_text.text = _lang["back_button"];
		crt_settings.text = _lang["crt_status"];
		language_settings.text = _lang["lang_status"];
		music_volume_settings.text = _lang["music_volume"];
		effects_volume_settings.text = _lang["effects_volume"];
		fullscreen_settings.text = _lang["fullscreen_status"];
		controller_settings.text = _lang["controller_status"];
		resolution_settings.text = _lang["resolution_status"];
		cn_dc_attack.text = _lang["controls_attack"];
		cn_dc_move.text = _lang["controls_move"];
		cn_dc_jump.text = _lang["controls_jump"];
		cn_dc_magic.text = _lang["controls_magic"];
		cn_dc_spell.text = _lang["controls_spell"];
		cn_dc_dash.text = _lang["controls_dash"];
		cn_dc_element.text = _lang["controls_element"];
		cn_dc_pause.text = _lang["controls_pause"];
		pc_dc_attack.text = _lang["controls_attack"];
		pc_dc_jump.text = _lang["controls_jump"];
		pc_dc_magic.text = _lang["controls_magic"];
		pc_dc_spell.text = _lang["controls_spell"];
		pc_dc_dash.text = _lang["controls_dash"];
		pc_dc_element.text = _lang["controls_element"];
		pc_dc_pause.text = _lang["controls_pause"];
		menu_main.controls_phrase = _lang["controls_button"];
		menu_main.settings_phrase = _lang["settings_button"];
		menu_main.lang_phrase = _lang["lang_phrase"];
		menu_main.on_phrase = _lang["on_phrase"];
		menu_main.off_phrase = _lang["off_phrase"];
		menu_main.crt_status = _lang["crt_status"];
		menu_main.fullscreen_status = _lang["fullscreen_status"];
	}

	public void ChangeFonts()
	{
		if (language == "russian" || language == "english")
		{
			start_button.font = Font_English_Russian;
			settings_button.font = Font_English_Russian;
			continue_button.font = Font_English_Russian;
			exit_button.font = Font_English_Russian;
			back_button.font = Font_English_Russian;
			menu_main.setting_values[0].font = Font_English_Russian;
			menu_main.setting_values[1].font = Font_English_Russian;
			menu_main.setting_values[2].font = Font_English_Russian;
			menu_main.setting_values[3].font = Font_English_Russian;
			menu_main.setting_values[4].font = Font_English_Russian;
			menu_main.setting_values[5].font = Font_English_Russian;
		}
		else if (language == "french" || language == "spanish" || language == "german" || language == "italian" || language == "portuguese" || language == "turkish" || language == "danish" || language == "dutch" || language == "polish" || language == "turkish")
		{
			start_button.font = Font_OtherEu;
			settings_button.font = Font_OtherEu;
			continue_button.font = Font_OtherEu;
			exit_button.font = Font_OtherEu;
			back_button.font = Font_OtherEu;
			controls_button.font = Font_OtherEu;
			crt_settings.font = Font_OtherEu;
			language_settings.font = Font_OtherEu;
			music_volume_settings.font = Font_OtherEu;
			effects_volume_settings.font = Font_OtherEu;
			fullscreen_settings.font = Font_OtherEu;
		}
		else if (language == "japanese")
		{
			start_button.font = Font_Japanese;
			settings_button.font = Font_Japanese;
			continue_button.font = Font_Japanese;
			exit_button.font = Font_Japanese;
			back_button.font = Font_Japanese;
			controls_button.font = Font_Japanese;
			crt_settings.font = Font_Japanese;
			language_settings.font = Font_Japanese;
			music_volume_settings.font = Font_Japanese;
			effects_volume_settings.font = Font_Japanese;
			fullscreen_settings.font = Font_Japanese;
		}
		else if (language == "simplchinese")
		{
			start_button.font = Font_Chinese;
			settings_button.font = Font_Chinese;
			continue_button.font = Font_Chinese;
			exit_button.font = Font_Chinese;
			back_button.font = Font_Chinese;
			controls_button.font = Font_Chinese;
			crt_settings.font = Font_Chinese;
			language_settings.font = Font_Chinese;
			music_volume_settings.font = Font_Chinese;
			effects_volume_settings.font = Font_Chinese;
			fullscreen_settings.font = Font_Chinese;
		}
		else if (language == "tradchinese")
		{
			start_button.font = Font_Chinese;
			settings_button.font = Font_Chinese;
			continue_button.font = Font_Chinese;
			exit_button.font = Font_Chinese;
			back_button.font = Font_Chinese;
			controls_button.font = Font_Chinese;
			crt_settings.font = Font_Chinese;
			language_settings.font = Font_Chinese;
			music_volume_settings.font = Font_Chinese;
			effects_volume_settings.font = Font_Chinese;
			fullscreen_settings.font = Font_Chinese;
		}
		else if (language == "korean")
		{
			start_button.font = Font_Korean;
			settings_button.font = Font_Korean;
			continue_button.font = Font_Korean;
			exit_button.font = Font_Korean;
			back_button.font = Font_Korean;
			controls_button.font = Font_Korean;
			crt_settings.font = Font_Korean;
			language_settings.font = Font_Korean;
			music_volume_settings.font = Font_Korean;
			effects_volume_settings.font = Font_Korean;
			fullscreen_settings.font = Font_Korean;
		}
		else
		{
			start_button.font = Font_English_Russian;
			settings_button.font = Font_English_Russian;
			continue_button.font = Font_English_Russian;
			exit_button.font = Font_English_Russian;
			back_button.font = Font_English_Russian;
			controls_button.font = Font_English_Russian;
			crt_settings.font = Font_English_Russian;
			language_settings.font = Font_English_Russian;
			music_volume_settings.font = Font_English_Russian;
			effects_volume_settings.font = Font_English_Russian;
			fullscreen_settings.font = Font_English_Russian;
		}
	}
}
