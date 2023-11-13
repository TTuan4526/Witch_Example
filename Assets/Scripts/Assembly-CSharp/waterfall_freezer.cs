using UnityEngine;

public class waterfall_freezer : MonoBehaviour
{
	public GameObject water_on_top;

	public GameObject water_on_bottom;

	private bool ice;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "frostbolt")
		{
			FreezeMid();
		}
	}

	public void FreezeMid()
	{
		if (!ice)
		{
			ice = true;
			anim.SetTrigger("freeze_mid");
		}
	}

	public void FreezeUp()
	{
		if (!ice)
		{
			ice = true;
			anim.SetTrigger("freeze_up");
		}
	}

	public void FreezeDown()
	{
		if (!ice)
		{
			ice = true;
			anim.SetTrigger("freeze_down");
		}
	}

	private void FreezeToBoth()
	{
		if (water_on_bottom != null)
		{
			water_on_bottom.SendMessage("FreezeDown");
		}
		if (water_on_top != null)
		{
			water_on_top.SendMessage("FreezeUp");
		}
	}

	private void FreezeToUp()
	{
		if (water_on_top != null)
		{
			water_on_top.SendMessage("FreezeUp");
		}
	}

	private void FreezeToDown()
	{
		if (water_on_bottom != null)
		{
			water_on_bottom.SendMessage("FreezeDown");
		}
	}

	public void Respawn()
	{
		if (ice)
		{
			anim.SetTrigger("freeze_end");
		}
	}
}
