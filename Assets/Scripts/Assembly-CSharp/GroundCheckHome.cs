using UnityEngine;

public class GroundCheckHome : MonoBehaviour
{
	private PlayerControllerHome player;

	private void Start()
	{
		player = base.gameObject.GetComponentInParent<PlayerControllerHome>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "ground" || col.tag == "boxes" || col.tag == "brick")
		{
			player.grounded = true;
			player.air_jump = true;
			player.GetComponent<Animator>().SetBool("grounded", true);
		}
		if (col.tag == "MovingPlatform")
		{
			player.transform.SetParent(col.transform);
		}
	}

	private void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "ground" || col.tag == "boxes" || col.tag == "brick")
		{
			player.grounded = true;
			player.GetComponent<Animator>().SetBool("grounded", true);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag != "misc" && col.tag != "turnip")
		{
			player.grounded = false;
			player.GetComponent<Animator>().SetBool("grounded", false);
			if (player.transform.parent != null && player.transform.parent.tag == "MovingPlatform")
			{
				player.transform.SetParent(null);
			}
		}
	}
}
