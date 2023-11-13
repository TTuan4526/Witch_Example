using UnityEngine;

public class evil_book : MonoBehaviour
{
	private bool dead;

	private AudioSource source;

	public Transform attack_position;

	private Vector3 temp_position;

	private Vector3 start_position;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		temp_position = base.transform.position;
		start_position = base.transform.position;
	}

	private void FixedUpdate()
	{
		temp_position.y = start_position.y + Mathf.Sin(Time.realtimeSinceStartup * 2f) * 0.05f;
		base.transform.position = temp_position;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			GetComponent<Animator>().Play("evil_book_strike");
		}
		else if (col.gameObject.tag == "Player" && !col.gameObject.GetComponent<PlayerController>().meteor_attack)
		{
			col.SendMessage("Damage", 1);
		}
	}

	public void SpawnMonster()
	{
		dead = false;
		GetComponent<BoxCollider2D>().enabled = true;
		GetComponent<Animator>().Play("evil_book_idle");
		temp_position = start_position;
		base.transform.position = start_position;
	}

	private void Dead()
	{
		dead = true;
		GetComponent<Animator>().Play("evil_book_dead");
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		GetComponent<BoxCollider2D>().enabled = false;
		source.Play();
	}

	public void MeleeAttack()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(attack_position.position, 0.25f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "Player")
			{
				array[i].gameObject.SendMessage("Damage", 2);
			}
		}
	}

	public void DespawnMonster()
	{
	}

	public void DamageFromRight(int amount)
	{
		if (!dead)
		{
			Dead();
		}
	}

	public void DamageFromLeft(int amount)
	{
		if (!dead)
		{
			Dead();
		}
	}
}
