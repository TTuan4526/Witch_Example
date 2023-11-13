using UnityEngine;

public class GroundCheck : MonoBehaviour
{
	public PlayerController player;

	private void Start()
	{
		player = base.gameObject.GetComponentInParent<PlayerController>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "ground" || col.tag == "boxes" || col.tag == "brick")
		{
			player.grounded = true;
			player.air_jump = true;
			player.air_dash = false;
			player.air_attack_first = false;
			player.air_attack_second = false;
			player.GetComponent<Animator>().SetBool("grounded", true);
			player.run_dust.GetComponent<Animator>().SetBool("ground", true);
			player.Landing();
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
			player.run_dust.GetComponent<Animator>().SetBool("ground", true);
			player.GetComponent<Animator>().SetBool("grounded", true);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag != "misc" && col.tag != "turnip" && col.tag != "enemy" && col.tag != "Untagged" && col.tag != "action" && col.tag != "dialog")
		{
			player.grounded = false;
			player.GetComponent<Animator>().SetBool("grounded", false);
			player.run_dust.GetComponent<Animator>().SetBool("ground", false);
			if (player.transform.parent != null && player.transform.parent.tag == "MovingPlatform")
			{
				player.transform.SetParent(null);
			}
		}
	}
}
