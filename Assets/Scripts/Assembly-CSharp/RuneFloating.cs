using System.Collections;
using UnityEngine;

public class RuneFloating : MonoBehaviour
{
	private Vector3 temp_position;

	private Vector3 start_position;

	public float temp;

	private bool floating;

	private void Start()
	{
		temp_position = base.transform.localPosition;
		start_position = base.transform.localPosition;
		StartCoroutine("StartFloating");
	}

	private IEnumerator StartFloating()
	{
		yield return new WaitForSeconds(temp);
		floating = true;
	}

	private void FixedUpdate()
	{
		if (floating)
		{
			temp_position.y = start_position.y + Mathf.Sin((Time.realtimeSinceStartup + temp) * 4f) * 0.1f;
			base.transform.localPosition = temp_position;
		}
	}
}
