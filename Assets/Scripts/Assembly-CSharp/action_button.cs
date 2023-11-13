using UnityEngine;

public class action_button : MonoBehaviour
{
	public bool book;

	public bool dialog;

	public bool save;

	public bool home;

	public bool fly;

	public GameObject button;

	public GameObject text;

	public GameObject holder;

	public string action;

	public GameObject obj;

	private bool active;

	private Transform player;

	private Data data;

	private CheckControlScheme scheme;

	private void Start()
	{
		scheme = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		if (PlayerPrefs.HasKey("control_scheme"))
		{
			switch (PlayerPrefs.GetString("control_scheme"))
			{
			case "XBOX":
				button.GetComponent<SpriteRenderer>().sprite = scheme.xbox_button_a;
				break;
			case "PS":
				button.GetComponent<SpriteRenderer>().sprite = scheme.ps_button_box;
				break;
			case "PC":
				button.GetComponent<SpriteRenderer>().sprite = scheme.keyboard_button_space;
				break;
			}
		}
		else
		{
			button.GetComponent<SpriteRenderer>().sprite = scheme.xbox_button_a;
		}
	}

	public void HideAction()
	{
		active = false;
		holder.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (!fly)
			{
				col.gameObject.SendMessage("PrepareForActionOn");
				active = true;
				holder.SetActive(true);
			}
			else if (data.story_progress >= 8 && fly)
			{
				col.gameObject.SendMessage("PrepareForActionOn");
				active = true;
				holder.SetActive(true);
				GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().ready_to_fly = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (!fly)
			{
				col.gameObject.SendMessage("PrepareForActionOff");
				active = false;
				holder.SetActive(false);
			}
			else if (data.story_progress >= 8 && fly)
			{
				col.gameObject.SendMessage("PrepareForActionOff");
				active = false;
				holder.SetActive(false);
				GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().ready_to_fly = false;
			}
		}
	}

	private void Update()
	{
		if (!active)
		{
			return;
		}
		holder.transform.position = new Vector3(player.position.x - 0.725f, player.position.y + 0.575f, base.transform.position.z);
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("ControllerA"))
		{
			if (dialog)
			{
				holder.SetActive(false);
				active = false;
				obj.SendMessage("StartDialogAnyway");
			}	
			else if (book)
			{
				obj.SendMessage("StartDialogAnyway");
				string key = "book_" + GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl + "_read";
				PlayerPrefs.SetInt(key, 1);
			}
			else if (save)
			{
				obj.SendMessage("SaveCheckpoint");
			}
			else if (home)
			{
				GameObject.FindGameObjectWithTag("common").GetComponent<WorldScriptHome>().LoadFirstLvl();
			}
		}
	}
}
