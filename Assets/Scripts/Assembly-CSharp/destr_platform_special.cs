using System.Collections;
using UnityEngine;

public class destr_platform_special : MonoBehaviour
{
	public cat_boss boss;

	private Animator anim;

	public int index;

	private bool ruined;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "witchy_ground" && !ruined)
		{
			boss.SetPlatform(index);
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

	public IEnumerator Fall(float amount)
	{
		yield return new WaitForSeconds(amount);
		anim.Play("dest_platform_ruin");
		GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine("Restore");
	}

	private IEnumerator Restore()
	{
		yield return new WaitForSeconds(2f);
		anim.Play("dest_platform_restore");
		yield return new WaitForSeconds(0.5f);
		GetComponent<BoxCollider2D>().enabled = true;
		ruined = false;
	}

	public void BackToIdle()
	{
		StopAllCoroutines();
		anim.Play("dest_platform_idle");
		GetComponent<BoxCollider2D>().enabled = true;
		ruined = false;
	}
}
