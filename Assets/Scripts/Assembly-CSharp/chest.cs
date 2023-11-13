using System.Collections;
using UnityEngine;

public class chest : MonoBehaviour
{
	private Animator anim;

	private AudioSource source;

	public bool collected;

	public bool picked;

	public bool hp;

	public bool mp;

	public string type = string.Empty;

	public int count;

	public Transform health_shard;

	public Transform mana_shard;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	private void Start()
	{
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	public void Open(string damage_type)
	{
		if (collected || (!(type == "wood") && (!(type == "fire") || !(damage_type == "fire")) && (!(type == "ice") || !(damage_type == "frost")) && (!(type == "lightning") || !(damage_type == "light"))))
		{
			return;
		}
		collected = true;
		GetComponent<BoxCollider2D>().enabled = false;
		anim.Play(type + "_chest_open");
		if (hp)
		{
			StartCoroutine("DropHPShard");
		}
		else if (mp)
		{
			StartCoroutine("DropMShard");
		}
		else
		{
			StartCoroutine("DropTreasure");
		}
		if ((type == "fire" || type == "ice" || type == "lightning") && (hp || mp))
		{
			switch (GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl)
			{
			case 0:
				GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_0_chests[count] = true;
				break;
			case 1:
				GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_1_chests[count] = true;
				break;
			case 2:
				GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_2_chests[count] = true;
				break;
			case 3:
				GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_3_chests[count] = true;
				break;
			case 4:
				GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_4_chests[count] = true;
				break;
			}
		}
	}

	private IEnumerator DropTreasure()
	{
		yield return new WaitForSeconds(0.1f);
		source.Play();
		yield return new WaitForSeconds(0.2f);
		DropShards();
	}

	private IEnumerator DropMShard()
	{
		yield return new WaitForSeconds(0.1f);
		source.Play();
		yield return new WaitForSeconds(0.2f);
		Object.Instantiate(mana_shard, new Vector3(base.transform.position.x, base.transform.position.y + 0.25f, base.transform.position.z), Quaternion.identity);
	}

	private IEnumerator DropHPShard()
	{
		yield return new WaitForSeconds(0.1f);
		source.Play();
		yield return new WaitForSeconds(0.2f);
		Object.Instantiate(health_shard, new Vector3(base.transform.position.x, base.transform.position.y + 0.25f, base.transform.position.z), Quaternion.identity);
	}

	public void ResetItemState()
	{
		if (!picked && collected)
		{
			collected = false;
		}
	}

	public void SpawnItem()
	{
		if (hp || mp)
		{
			switch (GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl)
			{
			case 0:
				if (GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_0_chests[count])
				{
					anim.Play(type + "_chest_opened");
					collected = true;
					picked = true;
					GetComponent<BoxCollider2D>().enabled = false;
				}
				else if (!picked && !collected)
				{
					anim.Play(type + "_chest_idle");
					GetComponent<BoxCollider2D>().enabled = true;
				}
				break;
			case 1:
				if (GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_1_chests[count])
				{
					anim.Play(type + "_chest_opened");
					collected = true;
					picked = true;
					GetComponent<BoxCollider2D>().enabled = false;
				}
				else if (!picked && !collected)
				{
					anim.Play(type + "_chest_idle");
					GetComponent<BoxCollider2D>().enabled = true;
				}
				break;
			case 2:
				if (GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_2_chests[count])
				{
					anim.Play(type + "_chest_opened");
					collected = true;
					picked = true;
					GetComponent<BoxCollider2D>().enabled = false;
				}
				else if (!picked && !collected)
				{
					anim.Play(type + "_chest_idle");
					GetComponent<BoxCollider2D>().enabled = true;
				}
				break;
			case 3:
				if (GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_3_chests[count])
				{
					anim.Play(type + "_chest_opened");
					collected = true;
					picked = true;
					GetComponent<BoxCollider2D>().enabled = false;
				}
				else if (!picked && !collected)
				{
					anim.Play(type + "_chest_idle");
					GetComponent<BoxCollider2D>().enabled = true;
				}
				break;
			case 4:
				if (GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_4_chests[count])
				{
					anim.Play(type + "_chest_opened");
					collected = true;
					picked = true;
					GetComponent<BoxCollider2D>().enabled = false;
				}
				else if (!picked && !collected)
				{
					anim.Play(type + "_chest_idle");
					GetComponent<BoxCollider2D>().enabled = true;
				}
				break;
			}
		}
		else if (!picked && !collected)
		{
			GetComponent<Animator>().Play(type + "_chest_idle");
			collected = false;
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else if (collected)
		{
			anim.Play(type + "_chest_opened");
		}
	}

	public void SaveObjectState()
	{
		if (collected && !picked)
		{
			picked = true;
		}
	}

	private void DropShards()
	{
		int num = 7;
		if (type == "fire" || type == "ice" || type == "lightning")
		{
			num = 15;
		}
		for (int i = 0; i < num; i++)
		{
			switch (Random.Range(1, 4))
			{
			case 1:
			{
				GameObject gameObject3 = Object.Instantiate(shard_1, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, -2.5f), base.transform.rotation);
				gameObject3.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 2:
			{
				GameObject gameObject2 = Object.Instantiate(shard_2, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, -2.5f), base.transform.rotation);
				gameObject2.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 3:
			{
				GameObject gameObject = Object.Instantiate(shard_3, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, -2.5f), base.transform.rotation);
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			}
		}
	}
}
