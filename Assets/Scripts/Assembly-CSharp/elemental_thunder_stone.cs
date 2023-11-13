using System.Collections;
using UnityEngine;

public class elemental_thunder_stone : MonoBehaviour
{
	public GameObject button_icon;

	public TextMesh button_text;

	public bool lightning_skill_1;

	public bool lightning_skill_2;

	public GameObject zap;

	private Animator anim;

	private Data data;

	private WorldScript world_script;

	private bool active;

	private bool active_buttons;

	private Transform player;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		world_script = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
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
		if (!(col.gameObject.tag == "Player") || active)
		{
			return;
		}
		button_text.text = GameObject.FindGameObjectWithTag("common").GetComponent<Localization>().GetLineDescription("power");
		col.gameObject.SendMessage("PrepareForActionOn");
		active_buttons = true;
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player"))
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
		active_buttons = false;
		active = true;
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetNewElement(2);
		anim.Play("thunder_stone_activate");
		zap.transform.position = new Vector3(player.position.x, zap.transform.position.y, zap.transform.position.z);
		StartCoroutine("ZapEffect");
		GetComponent<AudioSource>().Play();
		if (lightning_skill_1)
		{
			data.lightning_skill_1 = true;
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().lightning_skill_1 = true;
			StartCoroutine(SendMessageWithDelay("lightning_skill_1_get"));
		}
		else if (lightning_skill_2)
		{
			data.lightning_skill_2 = true;
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().lightning_skill_2 = true;
			StartCoroutine(SendMessageWithDelay("lightning_skill_2_get"));
		}
		GetComponent<BoxCollider2D>().enabled = false;
	}

	private IEnumerator SendMessageWithDelay(string command)
	{
		yield return new WaitForSeconds(2.5f);
		world_script.ShowMessage(command);
	}

	public void Reactivate()
	{
		if ((lightning_skill_1 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lightning_skill_1) || (lightning_skill_2 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lightning_skill_2))
		{
			active = false;
			active_buttons = false;
			anim.Play("thunder_stone_idle");
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			active = true;
			active_buttons = false;
			anim.Play("thunder_stone_active");
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	private IEnumerator ZapEffect()
	{
		yield return new WaitForSeconds(1f);
		zap.GetComponent<Animator>().Play("thunder_strike");
	}
}
