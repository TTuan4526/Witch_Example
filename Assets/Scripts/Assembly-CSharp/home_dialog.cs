using UnityEngine;
using UnityEngine.UI;

public class home_dialog : MonoBehaviour
{
	public GameObject dialog_window;

	public GameObject Portrait;

	public Text dialog_text;

	public string[] texts;

	public string[] emotion;

	private float tick = 0.05f;

	private float tick_value = 0.05f;

	private int count;

	private int current_phrase;

	private string current_text = string.Empty;

	private GameObject Witch;

	private LocalizationHome local;

	private bool conv_started;

	private bool conv_ended;

	private bool color_mode;

	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		Witch = GameObject.FindGameObjectWithTag("Player");
		local = GameObject.FindGameObjectWithTag("common").GetComponent<LocalizationHome>();
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
			}
			else if (Input.anyKeyDown)
			{
				current_phrase++;
				current_text = string.Empty;
				count = 0;
				tick_value = 0.05f;
				if (current_phrase < texts.Length)
				{
					Portrait.GetComponent<Animator>().Play("dialog_" + emotion[current_phrase]);
				}
			}
		}
		else if (current_phrase == texts.Length && !conv_ended && conv_started)
		{
			Witch.GetComponent<PlayerControllerHome>().Unblock();
			conv_ended = true;
			dialog_window.SetActive(false);
		}
	}

	private void UpdateText()
	{
		if (count == 0 && color_mode)
		{
			color_mode = false;
		}
		if (color_mode)
		{
			if (texts[current_phrase].Substring(count, 1) == "1" || texts[current_phrase].Substring(count, 1) == "2" || texts[current_phrase].Substring(count, 1) == "3" || texts[current_phrase].Substring(count, 1) == "4" || texts[current_phrase].Substring(count, 1) == "5")
			{
				color_mode = false;
				current_text += "</color>";
				dialog_text.text = current_text;
			}
			else if (texts[current_phrase].Substring(count, 1) != " ")
			{
				current_text += texts[current_phrase].Substring(count, 1);
				dialog_text.text = current_text + "</color>";
			}
			else
			{
				color_mode = false;
				current_text = current_text + texts[current_phrase].Substring(count, 1) + "</color>";
				dialog_text.text = current_text;
			}
		}
		else if (texts[current_phrase].Substring(count, 1) == "1")
		{
			color_mode = true;
			count++;
			current_text = current_text + "<color=#A76DE2>" + texts[current_phrase].Substring(count, 1);
			dialog_text.text = current_text + "</color>";
		}
		else if (texts[current_phrase].Substring(count, 1) == "2")
		{
			color_mode = true;
			count++;
			current_text = current_text + "<color=#34C427>" + texts[current_phrase].Substring(count, 1);
			dialog_text.text = current_text + "</color>";
		}
		else if (texts[current_phrase].Substring(count, 1) == "3")
		{
			color_mode = true;
			count++;
			current_text = current_text + "<color=#E4C442>" + texts[current_phrase].Substring(count, 1);
			dialog_text.text = current_text + "</color>";
		}
		else if (texts[current_phrase].Substring(count, 1) == "4")
		{
			color_mode = true;
			count++;
			current_text = current_text + "<color=#0094FF>" + texts[current_phrase].Substring(count, 1);
			dialog_text.text = current_text + "</color>";
		}
		else if (texts[current_phrase].Substring(count, 1) == "5")
		{
			color_mode = true;
			count++;
			current_text = current_text + "<color=#FF1900>" + texts[current_phrase].Substring(count, 1);
			dialog_text.text = current_text + "</color>";
		}
		else
		{
			current_text += texts[current_phrase].Substring(count, 1);
			dialog_text.text = current_text;
		}
		source.Play();
	}

	public void StartDialog()
	{
		if (!conv_started && !conv_ended)
		{
			for (int i = 0; i < texts.Length; i++)
			{
				string dialogLine = local.GetDialogLine(texts[i]);
				texts[i] = dialogLine;
			}
			conv_started = true;
			dialog_window.SetActive(true);
			dialog_text.text = string.Empty;
			Portrait.GetComponent<Animator>().Play("dialog_" + emotion[0]);
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

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !conv_started && !conv_ended)
		{
			StartDialog();
		}
	}
}
