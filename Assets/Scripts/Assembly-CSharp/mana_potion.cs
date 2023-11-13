using UnityEngine;

public class mana_potion : MonoBehaviour
{
	public bool collected;

	public bool picked;

	private AudioSource source;

	private PlayerController player;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!collected && col.gameObject.tag == "Player" && player.mana < player.mana_max)
		{
			collected = true;
			GetComponent<Animator>().Play("mana_potion_collect");
			GetComponent<CircleCollider2D>().enabled = false;
			GetComponent<FloatingObject>().enabled = false;
			source.Play();
			try
			{
				player.RestoreMana(2);
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
			GetComponent<Animator>().Play("mana_potion_idle");
			collected = false;
			GetComponent<FloatingObject>().enabled = true;
			GetComponent<CircleCollider2D>().enabled = true;
		}
		else
		{
			GetComponent<Animator>().Play("mana_potion_empty");
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
