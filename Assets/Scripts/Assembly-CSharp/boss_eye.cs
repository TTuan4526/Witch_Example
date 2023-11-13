using UnityEngine;

public class boss_eye : MonoBehaviour
{
	private string state = "stay";

	private Transform new_pos;

	private void Update()
	{
		if (state == "fly" && Vector3.Distance(base.transform.localPosition, new_pos.localPosition) <= 0.05f)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			state = "stay";
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.SendMessage("Damage", 2);
		}
	}

	public void FlyToNextPoint(Transform pos)
	{
		new_pos = pos;
		state = "fly";
	}
}
