using UnityEngine;

public class Crystal : MonoBehaviour
{
	private AudioSource source;

	private Animator anim;

	private WorldScript world_script;

	public bool collected;

	public bool picked;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		world_script = GameObject.Find("WorldObject").GetComponent<WorldScript>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !collected)
		{
			source.Play();
			GetComponent<FloatingObject>().enabled = false;
			GetComponent<CircleCollider2D>().enabled = false;
			world_script.GetCrystal(1);
			anim.Play("crystal_collect");
			collected = true;
		}
	}

	public void SpawnItem()
	{
		if (!picked && !collected)
		{
			GetComponent<Animator>().Play("crystal_idle");
			collected = false;
			GetComponent<CircleCollider2D>().enabled = true;
			GetComponent<FloatingObject>().enabled = true;
		}
		else
		{
			GetComponent<Animator>().Play("crystal_empty");
		}
	}

	public void ResetItemState()
	{
		if (!picked && collected)
		{
			collected = false;
		}
	}

	public void SaveObjectState()
	{
		if (collected && !picked)
		{
			picked = true;
		}
	}
}
