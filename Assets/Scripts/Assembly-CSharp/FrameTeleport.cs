using System.Collections;
using UnityEngine;

public class FrameTeleport : MonoBehaviour
{
	public Transform camera_current;

	public Transform player_destination;

	public Transform camera_destination;

	public GameObject checkpoint;

	public bool disable_checkpoint;

	private AstarPath astar_path;

	private Transform witch;

	private GameObject TeleportPanel;

	private Data data;

	private int lvl;

	private void Start()
	{
		lvl = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl;
		TeleportPanel = GameObject.Find("ui_teleport_panel");
		witch = GameObject.FindGameObjectWithTag("Player").transform;
		astar_path = GameObject.Find("A*").GetComponent<AstarPath>();
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!(col.gameObject.tag == "Player"))
		{
			return;
		}
		foreach (Transform item in GameObject.Find("Terrain").transform)
		{
			if (!(camera_destination.transform.parent == item))
			{
				continue;
			}
			item.GetComponent<SpriteRenderer>().enabled = true;
			foreach (Transform item2 in item)
			{
				item2.gameObject.SetActive(true);
			}
		}
		camera_current.GetComponent<this_room>().DespawnThemAll();
		camera_destination.GetComponent<this_room>().SpawnThemAll();
		if (camera_destination.GetComponent<this_room>().boss)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("boss");
			foreach (GameObject gameObject in array)
			{
				gameObject.SendMessage("CheckBossStatus");
			}
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("gbl");
		GameObject[] array3 = array2;
		foreach (GameObject obj in array3)
		{
			Object.Destroy(obj);
		}
		switch (lvl)
		{
		case 0:
			data.lvl_0_rooms[camera_destination.GetComponent<this_room>().room_number] = true;
			break;
		case 1:
			data.lvl_1_rooms[camera_destination.GetComponent<this_room>().room_number] = true;
			break;
		case 2:
			data.lvl_2_rooms[camera_destination.GetComponent<this_room>().room_number] = true;
			break;
		case 3:
			data.lvl_3_rooms[camera_destination.GetComponent<this_room>().room_number] = true;
			break;
		case 4:
			data.lvl_4_rooms[camera_destination.GetComponent<this_room>().room_number] = true;
			break;
		case 5:
			data.lvl_5_rooms[camera_destination.GetComponent<this_room>().room_number] = true;
			break;
		}
		TeleportPanel.GetComponent<CanvasGroup>().alpha = 0.75f;
		witch.GetComponent<PlayerController>().this_room = camera_destination.gameObject;
		witch.GetComponent<PlayerController>().this_room_number = camera_destination.GetComponent<this_room>().room_number;
		witch.position = new Vector3(player_destination.position.x, player_destination.position.y, -3f);
		Camera.main.transform.position = new Vector3(camera_destination.position.x, camera_destination.transform.position.y, -10f);
		astar_path.data.gridGraph.center = new Vector3(camera_destination.position.x, camera_destination.transform.position.y, 0f);
		astar_path.Scan(astar_path.data.graphs[0]);
		GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().StartCoroutine("FadeOutPanel");
		if (disable_checkpoint)
		{
			checkpoint.SendMessage("DisableCheckpoint");
		}
		foreach (Transform item3 in GameObject.Find("Terrain").transform)
		{
			if (!(camera_destination.transform.parent != item3))
			{
				continue;
			}
			item3.GetComponent<SpriteRenderer>().enabled = false;
			foreach (Transform item4 in item3)
			{
				if (item4.name != "RespawnItems" && item4.name != "Mobs")
				{
					item4.gameObject.SetActive(false);
				}
			}
		}
	}

	private IEnumerator FadeOutPanel()
	{
		float counter = 0f;
		while (counter < 0.5f)
		{
			counter += Time.deltaTime;
			TeleportPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / 0.5f);
			yield return null;
		}
	}
}
