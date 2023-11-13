using UnityEngine;

public class moving_platform : MonoBehaviour
{
	public bool going_next = true;

	public bool horizontal = true;

	public Vector3 end;

	private Vector3 start;

	private Vector3 temp_position;

	private void Start()
	{
		start = base.transform.localPosition;
	}

	private void Update()
	{
		if (horizontal)
		{
			if (end.x > start.x && going_next)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x + 1f * Time.deltaTime, base.transform.localPosition.y, base.transform.localPosition.z);
			}
			else if (end.x < start.x && !going_next)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x - 1f * Time.deltaTime, base.transform.localPosition.y, base.transform.localPosition.z);
			}
		}
		else if (end.y > start.y && going_next)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 1f * Time.deltaTime, base.transform.localPosition.z);
		}
		else if (end.y < start.y && !going_next)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - 1f * Time.deltaTime, base.transform.localPosition.z);
		}
	}

	private void FixedUpdate()
	{
		if (Vector2.Distance(base.transform.localPosition, end) < 0.1f)
		{
			going_next = !going_next;
			temp_position = new Vector3(end.x, end.y, base.transform.position.z);
			end = start;
			start = temp_position;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.tag == "Player")
		{
			collision.collider.transform.SetParent(base.transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.gameObject.tag == "Player")
		{
			collision.collider.transform.SetParent(null);
		}
	}
}
