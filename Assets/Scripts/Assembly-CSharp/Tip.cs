using UnityEngine;

public class Tip : MonoBehaviour
{
	public string action;

	public GameObject button;

	public GameObject text;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			button.SetActive(true);
			text.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			button.SetActive(false);
			text.SetActive(false);
		}
	}
}
