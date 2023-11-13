using UnityEngine;

public class Thorns : MonoBehaviour
{
	private float tick;

	private void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Player" && tick > 0f)
		{
			tick -= Time.deltaTime;
		}
		else if (col.tag == "Player" && tick <= 0f)
		{
			col.gameObject.GetComponent<PlayerController>().Damage(1);
			tick = 1f;
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		tick = 0f;
	}
}
