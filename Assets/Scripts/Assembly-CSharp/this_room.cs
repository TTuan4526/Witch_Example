using UnityEngine;

public class this_room : MonoBehaviour
{
	public Transform this_room_on_map;

	public int room_number;

	public bool boss;

	private string control_scheme;

	public void SpawnThemAll()
	{
		foreach (Transform item in base.transform.parent.Find("Mobs"))
		{
			if (item.gameObject.activeSelf)
			{
				item.SendMessage("SpawnMonster");
			}
		}
		try
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("elemental_stone");
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				gameObject.SendMessage("Reactivate");
			}
		}
		catch
		{
		}
		foreach (Transform item2 in base.transform.parent.Find("Platforms"))
		{
			if (item2.gameObject.activeSelf)
			{
				item2.SendMessage("RestorePlatform");
			}
		}
		foreach (Transform item3 in base.transform.parent.Find("RespawnObjects"))
		{
			if (item3.gameObject.activeSelf)
			{
				item3.SendMessage("Respawn");
			}
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("action");
		GameObject[] array4 = array3;
		foreach (GameObject gameObject2 in array4)
		{
			gameObject2.GetComponent<action_button>().HideAction();
		}
		GameObject[] array5 = GameObject.FindGameObjectsWithTag("tip");
		control_scheme = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().current_control_scheme;
		GameObject[] array6 = array5;
		foreach (GameObject gameObject3 in array6)
		{
			switch (gameObject3.GetComponent<Tip>().action)
			{
			case "jump":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_a;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_box;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_space;
					break;
				}
				break;
			case "doublejump":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_a;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_box;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_space;
					break;
				}
				break;
			case "attack":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_b;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_cross;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_f;
					break;
				}
				break;
			case "shoot":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_x;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_round;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_e;
					break;
				}
				break;
			case "dash":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_y;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_triangle;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_shift;
					break;
				}
				break;
			case "roll":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_y;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_triangle;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_shift;
					break;
				}
				break;
			case "element":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_lt;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_l1;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_r;
					break;
				}
				break;
			case "extra":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_rt;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_r1;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_q;
					break;
				}
				break;
			case "extra_air":
				switch (control_scheme)
				{
				case "XBOX":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().xbox_button_rt;
					break;
				case "PS":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().ps_button_r1;
					break;
				case "PC":
					gameObject3.GetComponent<Tip>().button.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>().keyboard_button_q;
					break;
				}
				break;
			}
		}
		foreach (Transform item4 in base.transform.parent.Find("RespawnItems"))
		{
			if (item4.gameObject.activeSelf)
			{
				item4.SendMessage("SpawnItem");
			}
		}
	}

	public void DespawnThemAll()
	{
		foreach (Transform item in base.transform.parent.Find("Mobs"))
		{
			if (item.gameObject.activeSelf)
			{
				item.SendMessage("DespawnMonster");
			}
		}
		if (boss)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("boss");
			foreach (GameObject gameObject in array)
			{
				gameObject.SendMessage("BossDespawn");
			}
		}
	}
}
