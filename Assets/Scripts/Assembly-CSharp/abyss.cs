using UnityEngine;

public class abyss : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerController>().Damage(20);
		}
	}
}
