using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
	private PlatformEffector2D effector;

	private string current_control_scheme;

	private void Start()
	{
		effector = GetComponent<PlatformEffector2D>();
		current_control_scheme = PlayerPrefs.GetString("control_scheme");
	}

	private void Update()
	{
		if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("ControllerA")) && (Input.GetKey(KeyCode.DownArrow) || ((current_control_scheme == "XBOX" || current_control_scheme == "PC") && (double)Input.GetAxis("ControllerAxis7") < -0.8) || (current_control_scheme == "PS" && (double)Input.GetAxis("ControllerAxis8") < -0.8) || Input.GetKey(KeyCode.S) || (double)Input.GetAxis("Vertical") > 0.8))
		{
			StartCoroutine("ReversePlatform");
		}
	}

	public void RestorePlatform()
	{
		GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
	}

	private IEnumerator ReversePlatform()
	{
		effector.rotationalOffset = 180f;
		yield return new WaitForSeconds(0.3f);
		effector.rotationalOffset = 0f;
	}
}
