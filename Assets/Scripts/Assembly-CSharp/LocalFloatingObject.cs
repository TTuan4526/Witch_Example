using UnityEngine;

public class LocalFloatingObject : MonoBehaviour
{
	private Vector3 temp_position;

	private Vector3 start_position;

	private void Start()
	{
		temp_position = base.transform.localPosition;
		start_position = base.transform.localPosition;
	}

	private void FixedUpdate()
	{
		temp_position.y = start_position.y + Mathf.Sin(Time.realtimeSinceStartup * 2f) * 0.1f;
		base.transform.localPosition = temp_position;
	}
}
