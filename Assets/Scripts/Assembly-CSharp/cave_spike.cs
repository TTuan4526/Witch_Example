using System.Collections;
using UnityEngine;

public class cave_spike : MonoBehaviour
{
	private Animator anim;

	private Rigidbody2D rbody;

	private bool fallen;

	private bool broken;

	public int index;

	private Vector3 default_position;

	private void Start()
	{
		anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		default_position = base.transform.localPosition;
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.025f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	public void Fall()
	{
		if (!fallen)
		{
			StartCoroutine(FinallyFall());
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!broken)
		{
			if (col.transform.tag == "Player")
			{
				col.gameObject.SendMessage("Damage", 1);
				GetComponent<AudioSource>().Play();
				Broke();
			}
			else if (col.gameObject.tag != "Sense" && !col.name.Contains("Platform") && col.gameObject.tag != "Collectible" && col.gameObject.tag != "PlayerGround" && col.gameObject.tag != "misc" && col.gameObject.tag != "dialog" && col.gameObject.tag != "trigger" && col.gameObject.tag != "enemy" && col.gameObject.tag != "collect_area" && fallen)
			{
				GetComponent<AudioSource>().Play();
				Broke();
			}
		}
	}

	public void SpawnItem()
	{
		fallen = false;
		broken = false;
		rbody.velocity = new Vector3(0f, 0f, 0f);
		base.transform.localPosition = default_position;
		anim.Play("cave_spike_" + index + "_idle");
		StopAllCoroutines();
		GetComponent<BoxCollider2D>().enabled = true;
	}

	public void SpawnItemWithoutIdle()
	{
		fallen = false;
		broken = false;
		rbody.velocity = new Vector3(0f, 0f, 0f);
		base.transform.localPosition = default_position;
		StopAllCoroutines();
		GetComponent<BoxCollider2D>().enabled = true;
	}

	private void Broke()
	{
		GetComponent<BoxCollider2D>().enabled = false;
		broken = true;
		rbody.velocity = new Vector3(0f, 0f, 0f);
		anim.Play("cave_spike_" + index + "_broke");
	}

	private IEnumerator FinallyFall()
	{
		if (!broken)
		{
			anim.Play("cave_spike_" + index + "_shake");
		}
		yield return new WaitForSeconds(0.2f);
		if (!broken)
		{
			anim.Play("cave_spike_" + index + "_idle");
			rbody.velocity = new Vector3(0f, -2f, 0f);
			fallen = true;
		}
	}

	public void ResetItemState()
	{
	}

	public void SaveObjectState()
	{
	}
}
