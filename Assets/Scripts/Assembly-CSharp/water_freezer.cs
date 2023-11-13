using UnityEngine;

public class water_freezer : MonoBehaviour
{
	public GameObject water_on_left;

	public GameObject water_on_right;

	public GameObject water_on_bottom;

	public BoxCollider2D surface;

	public float wide_mod;

	public bool is_surface;

	private bool ice;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void FreezeDown()
	{
		if (ice)
		{
			return;
		}
		if (is_surface)
		{
			if (surface.isTrigger)
			{
				surface.isTrigger = false;
			}
			surface.size = new Vector2(surface.GetComponent<ice_surface>().wide * wide_mod, surface.size.y);
		}
		ice = true;
		anim.SetTrigger("freeze_mid");
	}

	private void FreezeLeft()
	{
		if (ice)
		{
			return;
		}
		if (is_surface)
		{
			if (surface.isTrigger)
			{
				surface.isTrigger = false;
			}
			surface.size = new Vector2(surface.GetComponent<ice_surface>().wide * wide_mod, surface.size.y);
		}
		ice = true;
		anim.SetTrigger("freeze_left");
	}

	private void FreezeRight()
	{
		if (ice)
		{
			return;
		}
		if (is_surface)
		{
			if (surface.isTrigger)
			{
				surface.isTrigger = false;
			}
			surface.size = new Vector2(surface.GetComponent<ice_surface>().wide * wide_mod, surface.size.y);
		}
		ice = true;
		anim.SetTrigger("freeze_right");
	}

	private void FreezeToAll()
	{
		if (water_on_bottom != null)
		{
			water_on_bottom.SendMessage("FreezeDown");
		}
		if (water_on_right != null)
		{
			water_on_right.SendMessage("FreezeRight");
		}
		if (water_on_left != null)
		{
			water_on_left.SendMessage("FreezeLeft");
		}
	}

	private void FreezeToRight()
	{
		if (water_on_right != null)
		{
			water_on_right.SendMessage("FreezeRight");
		}
	}

	private void FreezeToLeft()
	{
		if (water_on_left != null)
		{
			water_on_left.SendMessage("FreezeLeft");
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
