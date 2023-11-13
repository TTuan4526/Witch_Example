using UnityEngine;

public class attack_area : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			base.transform.parent.SendMessage("Attack");
		}
	}
}
