using UnityEngine;

public class mob_area : MonoBehaviour
{
	public GameObject[] mobs;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player"))
		{
			return;
		}
		GameObject[] array = mobs;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.activeSelf)
			{
				gameObject.SendMessage("PlayerEnterArea");
			}
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player"))
		{
			return;
		}
		GameObject[] array = mobs;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.activeSelf)
			{
				gameObject.SendMessage("PlayerLeaveArea");
			}
		}
	}
}
