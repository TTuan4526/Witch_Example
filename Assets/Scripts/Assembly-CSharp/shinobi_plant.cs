using System.Collections;
using UnityEngine;

public class shinobi_plant : MonoBehaviour
{
	public GameObject Firepoint;

	public GameObject Deathball;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public AudioClip death_clip;

	private Animator anim;

	private Animator fire_anim;

	private AudioSource source;

	private float fire_rate = 0.1f;

	private bool dead;

	private bool damaging;

	private bool active;

	private int health = 2;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		fire_anim = Firepoint.GetComponent<Animator>();
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
	}

	public void SpawnMonster()
	{
		active = true;
		dead = false;
		damaging = false;
		anim.Play("shinobi_plant_idle");
		GetComponent<BoxCollider2D>().enabled = true;
		fire_rate = 2f;
		health = 2;
	}

	public void DespawnMonster()
	{
		active = false;
	}

	private void Update()
	{
		if (!dead && active)
		{
			if (fire_rate > 0f)
			{
				fire_rate -= Time.deltaTime;
			}
			if (fire_rate <= 0f && !damaging)
			{
				fire_rate = 2f;
				Attack();
			}
		}
	}

	private void Attack()
	{
		anim.Play("shinobi_plant_attack");
		StartCoroutine("ThrowDeathBall");
	}

	private IEnumerator ThrowDeathBall()
	{
		yield return new WaitForSeconds(0.4f);
		fire_anim.Play("plant_effect_gerbera_shoot");
		if (base.transform.localScale == new Vector3(1f, -1f, 1f))
		{
			GameObject gameObject = Object.Instantiate(Deathball, Firepoint.transform.position, Quaternion.Euler(0f, 0f, 180f));
			gameObject.GetComponent<Rigidbody2D>().velocity = base.transform.up * -2.5f;
		}
		else if (base.transform.localScale == new Vector3(1f, 1f, 1f))
		{
			GameObject gameObject2 = Object.Instantiate(Deathball, Firepoint.transform.position, Quaternion.Euler(0f, 0f, 0f));
			gameObject2.GetComponent<Rigidbody2D>().velocity = base.transform.up * 2.5f;
		}
	}

	public void DamageFromLeft(int amount)
	{
		if (!dead)
		{
			health -= amount;
			if (health <= 0)
			{
				Dead();
				return;
			}
			source.Play();
			anim.Play("shinobi_plant_damaged");
			damaging = true;
		}
	}

	public void DamageFromRight(int amount)
	{
		if (!dead)
		{
			health -= amount;
			if (health <= 0)
			{
				Dead();
				return;
			}
			source.Play();
			anim.Play("shinobi_plant_damaged");
			damaging = true;
		}
	}

	private void Dead()
	{
		active = false;
		dead = true;
		DropShards();
		source.PlayOneShot(death_clip, 0.2f);
		anim.Play("shinobi_plant_death");
		StopAllCoroutines();
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		GetComponent<BoxCollider2D>().enabled = false;
	}

	private void DropShards()
	{
		for (int i = 0; i < 5; i++)
		{
			switch (Random.Range(1, 4))
			{
			case 1:
			{
				GameObject gameObject3 = Object.Instantiate(shard_1, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject3.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 2:
			{
				GameObject gameObject2 = Object.Instantiate(shard_2, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject2.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 3:
			{
				GameObject gameObject = Object.Instantiate(shard_3, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			}
		}
	}

	private void StopDamaging()
	{
		damaging = false;
	}
}
