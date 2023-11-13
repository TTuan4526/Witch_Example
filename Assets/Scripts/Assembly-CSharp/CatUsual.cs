using UnityEngine;

public class CatUsual : MonoBehaviour
{
	private void Respawn()
	{
		if (GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().story_progress > 13)
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
