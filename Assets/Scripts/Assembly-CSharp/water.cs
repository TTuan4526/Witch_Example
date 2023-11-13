using System.Collections;
using UnityEngine;

public class water : MonoBehaviour
{
	private GameObject player;

	public Transform respawn_point;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			player = col.gameObject;
			player.GetComponent<PlayerController>().StartSinking();
			StartCoroutine("RespawnPlayer", player.GetComponent<PlayerController>().health);
		}
	}

	private IEnumerator RespawnPlayer(int hp)
	{
		yield return new WaitForSeconds(2f);
		if (hp >= 2)
		{
			player.GetComponent<PlayerController>().FallDrawnDamage(respawn_point.position, "drawn");
			yield break;
		}
		player.GetComponent<PlayerController>().Unblock();
		player.GetComponent<PlayerController>().Damage(1);
	}
}
