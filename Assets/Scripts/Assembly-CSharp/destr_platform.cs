using System.Collections;
using UnityEngine;

public class destr_platform : MonoBehaviour
{
	private Animator anim;

	private bool ruined;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "witchy_ground" && !ruined)
		{
			ruined = true;
			anim.Play("dest_platform_shake");
			StartCoroutine("Fall");
		}
	}

	private void SaveObjectState()
	{
	}

	private void ResetItemState()
	{
	}

	private void SpawnItem()
	{
		StopAllCoroutines();
		ruined = false;
		anim.Play("dest_platform_idle");
		GetComponent<BoxCollider2D>().enabled = true;
	}

	private IEnumerator Fall()
	{
		yield return new WaitForSeconds(0.5f);
		anim.Play("dest_platform_ruin");
		GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine("Restore");
	}

	private IEnumerator Restore()
	{
		yield return new WaitForSeconds(2f);
		anim.Play("dest_platform_restore");
		GetComponent<BoxCollider2D>().enabled = true;
		ruined = false;
	}
}
