using System.Collections;
using UnityEngine;

public class FlyAwayWithConditions : MonoBehaviour
{
	private AudioSource source;

	public Transform map;

	public Transform portrait;

	public Transform destination;

	public int stage;

	private bool start;

	private Data data;

	public int what_story_progress;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	private void Update()
	{
		if (start && Vector2.Distance(portrait.position, destination.position) <= 0.1f)
		{
			portrait.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			StartCoroutine("EndLvl");
			CancelInvoke("MakeSound");
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !start && what_story_progress > data.story_progress)
		{
			start = true;
			GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().HideUI();
			GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().StartCoroutine("FadeOutPanel");
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Stop();
			InvokeRepeating("MakeSound", 0f, 0.5f);
			col.gameObject.GetComponent<PlayerController>().paused = true;
			Camera.main.transform.position = new Vector3(map.position.x, map.position.y, Camera.main.transform.position.z);
			portrait.GetComponent<Rigidbody2D>().velocity = (destination.position - portrait.position) / 3f;
		}
	}

	private void MakeSound()
	{
		source.Play();
	}

	private IEnumerator EndLvl()
	{
		switch (GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl)
		{
		}
		data.lvl_spawn = 0;
		data.lvl = stage;
		data.story_progress = what_story_progress;
		if (data.lvl_last < GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl + 1)
		{
			data.lvl_last = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().this_lvl + 1;
		}
		SaveSystem.SaveData(data);
		yield return new WaitForSeconds(1.5f);
		GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().LoadSceneAsyncronical("lvl_" + stage);
	}
}
