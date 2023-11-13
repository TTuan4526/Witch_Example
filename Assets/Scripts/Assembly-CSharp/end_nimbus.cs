using UnityEngine;

public class end_nimbus : MonoBehaviour
{
	public string state = "end";

	public void FlyToThePoint()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 0f);
		state = "fly";
	}

	private void Update()
	{
		if (state == "fly" && (double)base.transform.localPosition.x <= 0.1)
		{
			state = "end";
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && state == "end")
		{
			GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().LoadSceneAsyncronical("lvl_tavern");
		}
	}
}
