using UnityEngine;

public class sanctuary : MonoBehaviour
{
	public GameObject button_icon;

	public TextMesh button_text;

	private Animator anim;

	public bool active;

	public bool active_buttons;

	public int spawn;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		if (PlayerPrefs.HasKey("control_scheme"))
		{
			switch (PlayerPrefs.GetString("control_scheme"))
			{
			case "XBOX":
				button_icon.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_a;
				break;
			case "PS":
				button_icon.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_box;
				break;
			case "PC":
				button_icon.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_space;
				break;
			}
		}
		else
		{
			button_icon.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_a;
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player") || active_buttons || active)
		{
			return;
		}
		button_text.text = GameObject.FindGameObjectWithTag("common").GetComponent<Localization>().GetLineDescription("save");
		col.gameObject.SendMessage("PrepareForActionOn");
		active_buttons = true;
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player") || !active_buttons)
		{
			return;
		}
		col.gameObject.SendMessage("PrepareForActionOff");
		active_buttons = false;
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
	}

	public void SaveCheckpoint()
	{
		if (active)
		{
			return;
		}
		anim.Play("sanctuary_activate");
		GetComponent<AudioSource>().Play();
		if (!active)
		{
			active = true;
			GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_spawn = spawn;
		}
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

	private void Update()
	{
		if ((!Input.GetKeyDown(KeyCode.Space) && !Input.GetButtonDown("ControllerA")) || !active_buttons || active)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
		SaveCheckpoint();
	}

	public void DisableCheckpoint()
	{
		GetComponent<Animator>().Play("sanctuary_idle");
		active = false;
	}
}
