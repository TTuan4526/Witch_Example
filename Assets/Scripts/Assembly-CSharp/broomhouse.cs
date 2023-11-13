using UnityEngine;

public class broomhouse : MonoBehaviour
{
	public GameObject nimbus;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			nimbus.SendMessage("Fly");
		}
	}
}
