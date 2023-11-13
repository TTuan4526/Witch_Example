using UnityEngine;

public class RandomFloatingObject : MonoBehaviour
{
	public float speed = 2f;

	public float range = 0.1f;

	private Vector3 temp_position;

	private Vector3 start_position;

	private void Start()
	{
		temp_position = base.transform.position;
		start_position = base.transform.position;
	}

	private void FixedUpdate()
	{
		temp_position.y = start_position.y + Mathf.Sin(Time.realtimeSinceStartup * speed) * range;
		base.transform.position = temp_position;
	}
}
