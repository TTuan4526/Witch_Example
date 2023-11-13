using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLvl : MonoBehaviour
{
	public Transform new_position;

	public GameObject EndPanel;

	public GameObject Icon;

	public GameObject Marker;

	public GameObject HPContainer;

	public GameObject minimap;

	public GameObject elements;

	public GameObject crystal_icon;

	public GameObject crystal_value;

	public GameObject leaf_icon;

	public GameObject leaf_value;

	public bool lvl_0;

	public bool to_map;

	private AudioSource source;

	private bool movement;

	private float timer;

	private void Start()
	{
		source = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (movement)
		{
			if (Vector2.Distance(new Vector2(Icon.transform.position.x, Icon.transform.position.y), new Vector2(Marker.transform.position.x, Marker.transform.position.y)) < 0.1f)
			{
				StartCoroutine("MoveToNextLvl");
				return;
			}
			if (timer > 0f)
			{
				timer -= Time.deltaTime;
				return;
			}
			source.Play();
			timer = 0.3f;
		}
	}

	private IEnumerator FadeInPanel()
	{
		float counter = 0f;
		while (counter < 0.75f)
		{
			counter += Time.deltaTime;
			EndPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, counter / 0.5f);
			yield return null;
		}
		HPContainer.SetActive(false);
		minimap.SetActive(false);
		elements.SetActive(false);
		crystal_icon.SetActive(false);
		crystal_value.SetActive(false);
		leaf_icon.SetActive(false);
		leaf_value.SetActive(false);
		GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Pause();
		StartCoroutine("FadeStayPanel");
	}

	private IEnumerator FadeStayPanel()
	{
		Camera.main.transform.position = new Vector3(new_position.position.x, new_position.position.y, Camera.main.transform.position.z);
		Camera.main.GetComponent<CameraController>().enabled = false;
		yield return new WaitForSeconds(1f);
		StartCoroutine("FadeOutPanel");
	}

	private IEnumerator MoveToNextLvl()
	{
		Icon.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("lvl_0");
	}

	private IEnumerator FadeOutPanel()
	{
		float counter = 0f;
		while (counter < 0.5f)
		{
			counter += Time.deltaTime;
			EndPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / 0.5f);
			yield return null;
		}
		movement = true;
		Icon.GetComponent<Rigidbody2D>().velocity = (Marker.transform.position - Icon.transform.position) * 0.25f;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && to_map)
		{
			col.gameObject.GetComponent<PlayerController>().enabled = false;
			StartCoroutine("FadeInPanel");
		}
	}
}
