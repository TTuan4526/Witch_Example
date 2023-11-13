using UnityEngine;

public class JumpOffNimbus : MonoBehaviour
{
	public GameObject border;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerController>().JumpOffNimbus();
			border.SetActive(true);
			Object.Destroy(base.gameObject);
		}
	}
}
