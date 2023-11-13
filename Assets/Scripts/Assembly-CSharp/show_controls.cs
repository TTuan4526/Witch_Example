using System.Collections;
using UnityEngine;

public class show_controls : MonoBehaviour
{
	public GameObject ui_controls;

	private CanvasGroup canvas_group;

	private bool faded = true;

	private void Start()
	{
		canvas_group = ui_controls.GetComponent<CanvasGroup>();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			ui_controls.SetActive(true);
			StartCoroutine(Fade(canvas_group.alpha, faded ? 1 : 0));
			faded = !faded;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			StartCoroutine(Fade(canvas_group.alpha, faded ? 1 : 0));
			faded = !faded;
		}
	}

	private IEnumerator Fade(float start, float end)
	{
		float counter = 0f;
		while (counter < 0.5f)
		{
			counter += Time.deltaTime;
			ui_controls.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, counter / 0.5f);
			yield return null;
		}
		if (end == 0f)
		{
			ui_controls.SetActive(false);
		}
	}
}
