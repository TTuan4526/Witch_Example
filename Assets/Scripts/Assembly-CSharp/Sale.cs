using System.Collections;
using UnityEngine;

public class Sale : MonoBehaviour
{
	public GameObject button;

	public GameObject text;

	public GameObject item;

	public int value;

	public bool hp_1;

	public bool mp_1;

	public bool hp_2;

	public bool mp_2;

	private bool active;

	public bool collected;

	private CheckControlScheme scheme;

	private WorldScript world_script;

	private void Start()
	{
		scheme = GameObject.FindGameObjectWithTag("common").GetComponent<CheckControlScheme>();
		world_script = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
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

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player") || collected)
		{
			return;
		}
		col.gameObject.SendMessage("PrepareForActionOn");
		active = true;
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(true);
		}
	}

	private void ResetItemState()
	{
		if (collected && ((hp_1 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().hp_1_sold) || (hp_2 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().hp_2_sold) || (mp_1 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().mp_1_sold) || (mp_2 && !GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().mp_2_sold)))
		{
			collected = false;
			item.GetComponent<Animator>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = true;
		}
	}

	private void SaveObjectState()
	{
		if (collected)
		{
			GetComponent<BoxCollider2D>().enabled = false;
			item.GetComponent<Animator>().enabled = false;
			if (hp_1)
			{
				item.GetComponent<Animator>().Play("health_shard_hide");
			}
			else if (hp_2)
			{
				item.GetComponent<Animator>().Play("health_shard_hide");
			}
			else if (mp_1)
			{
				item.GetComponent<Animator>().Play("mana_shard_hide");
			}
			else if (mp_2)
			{
				item.GetComponent<Animator>().Play("mana_shard_hide");
			}
		}
	}

	private void SpawnItem()
	{
		if ((hp_1 && GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().hp_1_sold) || (hp_2 && GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().hp_2_sold) || (mp_1 && GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().mp_1_sold) || (mp_2 && GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().mp_2_sold))
		{
			collected = true;
			GetComponent<BoxCollider2D>().enabled = false;
			if (hp_1 || hp_2)
			{
				item.GetComponent<Animator>().Play("health_shard_hidden");
			}
			else if (mp_1 || mp_2)
			{
				item.GetComponent<Animator>().Play("mana_shard_hidden");
			}
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player"))
		{
			return;
		}
		col.gameObject.SendMessage("PrepareForActionOff");
		active = false;
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if ((!Input.GetKeyDown(KeyCode.Space) && !Input.GetButtonDown("ControllerA")) || !active || world_script.crystal < value || collected)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
		active = false;
		collected = true;
		GetComponent<AudioSource>().Play();
		world_script.SpendCrystal(value);
		if (hp_1)
		{
			world_script.GetHPShard();
			this.item.GetComponent<Animator>().Play("health_shard_hide");
			GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().hp_1_sold = true;
		}
		else if (hp_2)
		{
			world_script.GetHPShard();
			this.item.GetComponent<Animator>().Play("health_shard_hide");
			GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().hp_2_sold = true;
		}
		else if (mp_1)
		{
			world_script.GetMPShard();
			this.item.GetComponent<Animator>().Play("mana_shard_hide");
			GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().mp_1_sold = true;
		}
		else if (mp_2)
		{
			world_script.GetMPShard();
			this.item.GetComponent<Animator>().Play("mana_shard_hide");
			GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().mp_2_sold = true;
		}
		StartCoroutine("ShutOffAnimator");
	}

	private IEnumerator ShutOffAnimator()
	{
		yield return new WaitForSeconds(0.75f);
		item.GetComponent<Animator>().enabled = false;
	}
}
