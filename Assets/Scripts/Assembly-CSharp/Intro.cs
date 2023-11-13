using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	public Font Font_English_Russian;

	public Font Font_Japanese;

	public Font Font_Chinese;

	public Font Font_Korean;

	public Font Font_OtherEu;

	public Text ui_text;

	public GameObject loading_panel;

	public GameObject book;

	private AudioSource source;

	private LocalizationIntro local;

	private float tick = 0.05f;

	private float tick_value = 0.05f;

	private float rest_tick = 1f;

	private float rest_tick_value = 1f;

	private int count;

	private int current_phrase;

	public string[] texts;

	private bool conv_started;

	private bool conv_ended;

	private string language = "english";

	private string current_text = string.Empty;

	private int chapter;

	private readonly Dictionary<string, string> _lang = new Dictionary<string, string>();

	private void Start()
	{
		source = GetComponent<AudioSource>();
		local = GetComponent<LocalizationIntro>();
		conv_started = true;
		local.Localizate();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		if (PlayerPrefs.HasKey("music_volume"))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.03f * (float)PlayerPrefs.GetInt("music_volume");
		}
		for (int i = 0; i < texts.Length; i++)
		{
			string dialogLine = local.GetDialogLine(texts[i]);
			texts[i] = dialogLine;
		}
		Cursor.visible = false;
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
	}

	private void Update()
	{
		if (conv_started && !conv_ended && current_phrase < texts.Length)
		{
			if (count != texts[current_phrase].Length)
			{
				if (tick > 0f)
				{
					tick -= Time.deltaTime;
				}
				if (tick <= 0f)
				{
					tick = tick_value;
					UpdateText();
					count++;
				}
				if (Input.anyKeyDown)
				{
					ShowWholeLine();
				}
				return;
			}
			if (rest_tick > 0f)
			{
				rest_tick -= Time.deltaTime;
			}
			if (Input.anyKeyDown || rest_tick <= 0f)
			{
				chapter++;
				if (chapter == 1)
				{
					book.GetComponent<Animator>().Play("intro_book_opening");
				}
				else if (chapter == 3)
				{
					book.GetComponent<Animator>().SetBool("to_map", true);
				}
				current_phrase++;
				current_text = string.Empty;
				count = 0;
				rest_tick = rest_tick_value;
				tick_value = 0.05f;
			}
		}
		else if (current_phrase == texts.Length && !conv_ended && conv_started)
		{
			StartCoroutine("LoadGame");
		}
	}

	private IEnumerator LoadGame()
	{
		conv_ended = true;
		yield return new WaitForSeconds(2.5f);
		book.SetActive(false);
		loading_panel.SetActive(true);
		SceneManager.LoadScene("lvl_home");
	}

	private void ChangeFonts()
	{
		if (language == "russian" || language == "english")
		{
			ui_text.font = Font_English_Russian;
		}
		else if (language == "french" || language == "spanish" || language == "german" || language == "italian" || language == "portuguese" || language == "turkish" || language == "danish" || language == "dutch" || language == "polish" || language == "turkish")
		{
			ui_text.font = Font_OtherEu;
		}
		else if (language == "japanese")
		{
			ui_text.font = Font_Japanese;
		}
		else if (language == "simplchinese")
		{
			ui_text.font = Font_Chinese;
		}
		else if (language == "tradchinese")
		{
			ui_text.font = Font_Chinese;
		}
		else if (language == "korean")
		{
			ui_text.font = Font_Korean;
		}
		else
		{
			ui_text.font = Font_English_Russian;
		}
	}

	private void ShowWholeLine()
	{
		while (count != texts[current_phrase].Length)
		{
			UpdateText();
			count++;
		}
	}

	private void UpdateText()
	{
		source.Play();
		current_text += texts[current_phrase].Substring(count, 1);
		ui_text.text = current_text;
	}
}
