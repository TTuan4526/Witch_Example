using UnityEngine;

public class cave_spikes_trigger : MonoBehaviour
{
	public bool left;

	public GameObject Spikes;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Player")
		{
			if (left)
			{
				Spikes.SendMessage("DropLeftToRight");
			}
			else
			{
				Spikes.SendMessage("DropRightToLeft");
			}
		}
	}
}
