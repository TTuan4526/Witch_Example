using UnityEngine;

public class Nimbus : MonoBehaviour
{
	private bool active;

	public bool then_fly;

	public GameObject fly;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !active)
		{
			active = true;
			GetComponent<Animator>().Play("nimbus_fly");
			if (then_fly)
			{
				fly.SetActive(true);
			}
		}
	}
}
