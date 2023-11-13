using UnityEngine;

public class ui_room : MonoBehaviour
{
	public bool opened;

	public int room_number;

	public GameObject icon;

	public GameObject witch_icon;

	public GameObject chest;

	public void Open()
	{
		opened = true;
		GetComponent<Animator>().Play("map_room_opened");
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().this_room_number == room_number)
		{
			witch_icon.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z - 1f);
			if (!(icon != null))
			{
			}
		}
		else if (icon != null)
		{
			icon.SetActive(true);
			if (chest != null && (chest.GetComponent<chest>().hp || chest.GetComponent<chest>().mp) && CheckChestInRoom(GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl, chest.GetComponent<chest>().count))
			{
				icon.SetActive(false);
			}
		}
	}

	private bool CheckChestInRoom(int lvl, int count)
	{
		switch (lvl)
		{
		case 0:
			return GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_0_chests[count];
		case 1:
			return GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_1_chests[count];
		case 2:
			return GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_2_chests[count];
		case 3:
			return GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_3_chests[count];
		case 4:
			return GameObject.FindGameObjectWithTag("Data").GetComponent<Data>().lvl_4_chests[count];
		default:
			return false;
		}
	}

	public void Close()
	{
		GetComponent<Animator>().Play("map_room_closed");
		opened = false;
		if (!(icon != null))
		{
		}
	}
}
