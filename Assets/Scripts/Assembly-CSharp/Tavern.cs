using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tavern : MonoBehaviour
{
	public GameObject dialog;

	public GameObject Panel;

	public GameObject loading_panel;

	public Text few_text;

	private void Start()
	{
		GetComponent<LocalizationTavern>().Translate();
		StartCoroutine("FadeOutPanel");
	}

	private IEnumerator FadeOutPanel()
	{
		float counter = 0f;
		yield return new WaitForSeconds(2f);
		while (counter < 0.5f)
		{
			counter += Time.deltaTime;
			Panel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / 0.5f);
			yield return null;
		}
		Panel.SetActive(false);
		dialog.GetComponent<EndDialog>().StartDialog();
	}

	public void LoadSceneAsyncronical(string scene_name)
	{
		loading_panel.SetActive(true);
		SceneManager.LoadScene(scene_name);
	}
}
