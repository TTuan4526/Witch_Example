using System;
using System.Collections;
using UnityEngine;

public class wall : MonoBehaviour
{
	private bool explode;

	public string type = string.Empty;

	public GameObject wall_on_left;

	public GameObject wall_on_right;

	private AudioSource source;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		source = GetComponent<AudioSource>();
	}

	public void Explode(string damage_type)
	{
		if (explode)
		{
			return;
		}
		if ((type == "wood" && damage_type == "fire") || (type == "ice" && damage_type == "fire") || (type == "ice" && damage_type == "light") || (type == "stone" && damage_type == "light"))
		{
			source.Play();
			explode = true;
			GetComponent<BoxCollider2D>().enabled = false;
			foreach (Transform item in base.transform)
			{
				item.GetComponent<Animator>().Play(type + "_block_destroy");
			}
			StartCoroutine(NextWall(damage_type));
			return;
		}
		foreach (Transform item2 in base.transform)
		{
			item2.GetComponent<Animator>().Play(type + "_block_hit");
		}
	}

	public void SecondExplode(string damage_type)
	{
		if (explode)
		{
			return;
		}
		if ((type == "wood" && damage_type == "fire") || (type == "ice" && damage_type == "fire") || (type == "ice" && damage_type == "light") || (type == "stone" && damage_type == "light"))
		{
			source.Play();
			explode = true;
			GetComponent<BoxCollider2D>().enabled = false;
			foreach (Transform item in base.transform)
			{
				item.GetComponent<Animator>().Play(type + "_block_destroy");
			}
			StartCoroutine(NextWall(damage_type));
			return;
		}
		foreach (Transform item2 in base.transform)
		{
			item2.GetComponent<Animator>().Play(type + "_block_hit");
		}
	}

	public void Respawn()
	{
		if (!explode)
		{
			return;
		}
		GetComponent<BoxCollider2D>().enabled = false;
		foreach (Transform item in base.transform)
		{
			item.GetComponent<Animator>().Play(type + "_block_destroed");
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerController>().fire_dash)
		{
			Explode("fire");
		}
	}

	private IEnumerator DestroyOneAfterAnother()
	{
		int i = 0;
		Transform[] blocks = new Transform[base.transform.childCount];
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				blocks[i] = transform;
				i++;
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = (disposable = enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
		for (int k = 0; k < base.transform.childCount; k++)
		{
			for (int l = 0; l < base.transform.childCount - 1; l++)
			{
				if (blocks[l].localPosition.y < blocks[l + 1].localPosition.y)
				{
					Transform transform2 = blocks[l];
					blocks[l] = blocks[l + 1];
					blocks[l + 1] = transform2;
				}
			}
		}
		for (int j = 0; j < base.transform.childCount; j++)
		{
			blocks[j].GetComponent<Animator>().Play(type + "_block_destroy");
			yield return new WaitForSeconds(0.15f);
		}
	}

	private IEnumerator NextWall(string type)
	{
		yield return new WaitForSeconds(0.15f);
		if (wall_on_left != null)
		{
			wall_on_left.SendMessage("Explode", type);
		}
		if (wall_on_right != null)
		{
			wall_on_right.SendMessage("Explode", type);
		}
	}

	private IEnumerator End()
	{
		yield return new WaitForSeconds(0.7f);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
