using UnityEngine;

public class ice_thorn : MonoBehaviour
{
	private bool broken;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!broken && col.transform.tag == "Player")
		{
			col.gameObject.SendMessage("Damage", 1);
			broken = true;
		}
	}

	private void Destr()
	{
		Object.Destroy(base.gameObject);
	}
}
