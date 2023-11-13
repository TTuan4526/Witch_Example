using UnityEngine;

public class herb : MonoBehaviour
{
	public string type;

	public bool collected;

	public bool picked;

	private AudioSource source;

	private PlayerController player;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!collected && col.gameObject.tag == "Player" && player.health < player.health_max)
		{
			collected = true;
			GetComponent<Animator>().Play("herb_" + type + "_collect");
			GetComponent<BoxCollider2D>().enabled = false;
			source.Play();
			try
			{
				player.Heal(1);
			}
			catch
			{
			}
		}
	}

	public void SpawnItem()
	{
		if (!picked && !collected)
		{
			GetComponent<Animator>().Play("herb_" + type + "_idle");
			collected = false;
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			GetComponent<Animator>().Play("herb_" + type + "_empty");
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
