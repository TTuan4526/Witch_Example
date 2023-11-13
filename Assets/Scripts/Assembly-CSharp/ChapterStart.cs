using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChapterStart : MonoBehaviour
{
	public GameObject ChapterPanel;

	public Text ChapterNumber;

	public Text ChapterText;

	public void StartChapter()
	{
		ChapterPanel.SetActive(true);
		StartCoroutine("FadeInPanel");
	}

	private IEnumerator FadeInPanel()
	{
		float counter = 0f;
		while (counter < 0.75f)
		{
			counter += Time.deltaTime;
			ChapterPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, counter / 0.75f);
			yield return null;
		}
		StartCoroutine("FadeStayPanel");
	}

	private IEnumerator FadeStayPanel()
	{
		yield return new WaitForSeconds(1f);
		StartCoroutine("FadeOutPanel");
	}

	private IEnumerator FadeOutPanel()
	{
		float counter = 0f;
		while (counter < 0.5f)
		{
			counter += Time.deltaTime;
			ChapterPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / 0.5f);
			yield return null;
		}
		ChapterPanel.SetActive(false);
	}
}
