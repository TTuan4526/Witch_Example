using UnityEngine;

public class mana_shard : MonoBehaviour
{
	private Animator anim;

	private AudioSource source;

	private bool collected;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !collected)
		{
			collected = true;
			source.Play();
			anim.Play("mana_shard_collect");
			GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>().GetMPShard();
		}
	}

	private void End()
	{
		Object.Destroy(base.gameObject);
	}
}
