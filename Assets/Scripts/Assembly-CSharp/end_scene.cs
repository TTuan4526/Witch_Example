using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class end_scene : MonoBehaviour
{
	public Text end_1;

	public Text end_2;

	public Text end_3;

	public Text end_4;

	private bool end;

	private string language = "english";

	private readonly Dictionary<string, string> _lang = new Dictionary<string, string>();

	private void Start()
	{
		StartCoroutine("Next");
		UpdateCrtFilter();
		SetVolume();
		if (!PlayerPrefs.HasKey("language"))
		{
			PlayerPrefs.SetString("language", "english");
			language = "english";
		}
		else
		{
			language = PlayerPrefs.GetString("language");
		}
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
		end_1.text = _lang["ending_1"];
	}

	private void SetVolume()
	{
		if (PlayerPrefs.HasKey("music_volume"))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.03f * (float)PlayerPrefs.GetInt("music_volume");
		}
	}

	private void Update()
	{
		if (Input.anyKeyDown && end)
		{
			SceneManager.LoadScene("main_menu");
		}
	}

	private IEnumerator Next()
	{
		yield return new WaitForSeconds(1f);
		end = true;
	}

	private void UpdateCrtFilter()
	{
		if (PlayerPrefs.HasKey("crt_status"))
		{
			if (PlayerPrefs.GetString("crt_status") == "disabled")
			{
				Camera.main.GetComponent<OLDTVTube>().enabled = false;
			}
			else
			{
				Camera.main.GetComponent<OLDTVTube>().enabled = true;
			}
		}
	}
}
