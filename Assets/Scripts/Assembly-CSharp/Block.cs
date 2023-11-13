using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
	private Animator anim;

	private AudioSource source;

	private bool explode;

	public string type = string.Empty;

	private void Start()
	{
		anim = GetComponent<Animator>();
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
			anim.Play(type + "_block_destroy");
			Collider2D[] components = GetComponents<Collider2D>();
			foreach (Collider2D obj in components)
			{
				Object.Destroy(obj);
			}
			StartCoroutine("End");
		}
		else
		{
			anim.Play(type + "_block_hit");
		}
	}

	private IEnumerator End()
	{
		yield return new WaitForSeconds(0.7f);
		Object.Destroy(base.gameObject);
	}
}
