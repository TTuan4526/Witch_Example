using UnityEngine;

public class mushroom_boss_areas : MonoBehaviour
{
	private boss_mushroom Boss;

	public bool ground;

	public bool left_pl;

	public bool central_pl;

	public bool right_pl;

	private void Start()
	{
		Boss = GameObject.Find("boss_mushroom").GetComponent<boss_mushroom>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (ground)
			{
				Boss.player_position = "ground";
			}
			else if (left_pl)
			{
				Boss.player_position = "left";
			}
			else if (central_pl)
			{
				Boss.player_position = "central";
			}
			else if (right_pl)
			{
				Boss.player_position = "right";
			}
		}
	}
}
