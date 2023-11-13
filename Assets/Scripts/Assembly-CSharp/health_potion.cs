using UnityEngine;

public class health_potion : MonoBehaviour
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
		if (!collected && col.gameObject.tag == "Player" && player.health < player.health_max)
		{
			collected = true;
			GetComponent<Animator>().Play("health_potion_collect");
			GetComponent<CircleCollider2D>().enabled = false;
			GetComponent<FloatingObject>().enabled = false;
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
			GetComponent<Animator>().Play("health_potion_idle");
			collected = false;
			GetComponent<FloatingObject>().enabled = true;
			GetComponent<CircleCollider2D>().enabled = true;
		}
		else
		{
			GetComponent<Animator>().Play("health_potion_empty");
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
