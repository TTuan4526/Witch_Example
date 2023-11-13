using UnityEngine;

public class elemental_ice_stone : MonoBehaviour
{
	public GameObject button_icon;

	public TextMesh button_text;

	public bool ice_skill_1;

	private Animator anim;

	private Data data;

	private WorldScript world_script;

	private bool active;

	private bool active_buttons;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		world_script = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>();
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
		anim.Play("ice_stone_activate");
		GetComponent<AudioSource>().Play();
		if (ice_skill_1)
		{
			data.frost_skill_1 = true;
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().frost_skill_1 = true;
			world_script.ShowMessage("frost_skill_1_get");
		}
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetNewElement(3);
		GetComponent<BoxCollider2D>().enabled = false;
	}

	public void Reactivate()
	{
		if (ice_skill_1 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().frost_skill_1)
		{
			active = false;
			active_buttons = false;
			anim.Play("ice_stone_idle");
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			active = true;
			active_buttons = false;
			anim.Play("ice_stone_active");
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
