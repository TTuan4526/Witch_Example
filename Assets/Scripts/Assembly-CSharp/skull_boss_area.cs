using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class skull_boss_area : MonoBehaviour
{
	public GameObject boss;

	public GameObject hp_bar;

	public GameObject boss_name;

	public GameObject border_1;

	public GameObject border_2;

	public int on_what_story_phase;

	public GameObject dialog_window;

	public GameObject Portrait;

	public Text dialog_text;

	public string[] texts;

	public string[] emotion;

	public AudioClip end_music;

	private float tick = 0.05f;

	private float tick_value = 0.05f;

	private int count;

	private int current_phrase;

	private string current_text = string.Empty;

	private GameObject Witch;

	private Localization local;

	private Data data;

	private bool conv_started;

	private bool conv_ended;

	private bool color_mode;

	private bool finished;

	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("music_volume"))
		{
			GameObject.Find("BossMusic").GetComponent<AudioSource>().volume = 0.03f * (float)PlayerPrefs.GetInt("music_volume");
		}
		Witch = GameObject.FindGameObjectWithTag("Player");
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		local = GameObject.FindGameObjectWithTag("common").GetComponent<Localization>();
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
			Witch.GetComponent<PlayerController>().Unblock();
			conv_ended = true;
			boss.GetComponent<skull_boss>().Starter();
			finished = true;
			hp_bar.SetActive(true);
			boss_name.SetActive(true);
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

	public void Respawn()
	{
		border_1.GetComponent<BoxCollider2D>().enabled = false;
		border_1.GetComponent<Animator>().Play("spikewall_idle");
		border_2.GetComponent<BoxCollider2D>().enabled = false;
		border_2.GetComponent<Animator>().Play("spikewall_idle");
		finished = false;
		hp_bar.SetActive(false);
		boss_name.SetActive(false);
		if (!GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().isPlaying)
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Play();
		}
		GameObject.Find("BossMusic").GetComponent<AudioSource>().Stop();
		boss.GetComponent<skull_boss>().eyes.GetComponent<boss_eyes>().RestoreAll();
	}

	public void BossDefeated()
	{
		border_1.GetComponent<BoxCollider2D>().enabled = false;
		border_1.GetComponent<Animator>().Play("spikewall_ungrow");
		border_2.GetComponent<BoxCollider2D>().enabled = false;
		border_2.GetComponent<Animator>().Play("spikewall_ungrow");
		hp_bar.SetActive(false);
		boss_name.SetActive(false);
		data.story_progress = on_what_story_phase + 1;
		GameObject.Find("BossMusic").GetComponent<AudioSource>().Stop();
		GameObject.Find("BossMusic").GetComponent<AudioSource>().PlayOneShot(end_music);
		StartCoroutine("PlayMusicAgain");
		GameObject[] array = GameObject.FindGameObjectsWithTag("respawn_items");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			foreach (Transform item in gameObject.transform)
			{
				if (item.gameObject.activeSelf)
				{
					item.SendMessage("SaveObjectState");
				}
			}
		}
		SaveSystem.SaveData(GameObject.FindGameObjectWithTag("Data").GetComponent<Data>());
	}

	private IEnumerator PlayMusicAgain()
	{
		yield return new WaitForSeconds(4.5f);
		GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Play();
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
			Witch.GetComponent<PlayerController>().Block();
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
		if (col.gameObject.tag == "Player" && data.story_progress < on_what_story_phase && !finished)
		{
			border_1.GetComponent<BoxCollider2D>().enabled = true;
			border_1.GetComponent<Animator>().Play("spikewall_grow");
			border_2.GetComponent<BoxCollider2D>().enabled = true;
			border_2.GetComponent<Animator>().Play("spikewall_grow");
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Pause();
			GameObject.Find("BossMusic").GetComponent<AudioSource>().Play();
			if (!conv_started && !conv_ended)
			{
				data.story_progress = on_what_story_phase;
				StartDialog();
				return;
			}
			boss.GetComponent<skull_boss>().Starter();
			finished = true;
			hp_bar.SetActive(true);
			boss_name.SetActive(true);
		}
		else if (data.story_progress < on_what_story_phase)
		{
		}
	}
}
