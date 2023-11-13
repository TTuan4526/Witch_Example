using System.Collections;
using UnityEngine;

public class EnemyGerbera : MonoBehaviour
{
	public GameObject Firepoint;

	public GameObject Deathball;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public AudioClip death_sound;

	private Animator anim;

	private Animator fire_anim;

	private AudioSource source;

	public int max_hp;

	private float fire_rate = 0.1f;

	private bool dead;

	private bool damaging;

	private bool active;

	private int health = 3;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		source = GetComponent<AudioSource>();
		fire_anim = Firepoint.GetComponent<Animator>();
		anim = base.gameObject.GetComponent<Animator>();
	}

	public void SpawnMonster()
	{
		active = true;
		dead = false;
		damaging = false;
		GetComponent<Animator>().Play("enemy_gerbera_idle");
		GetComponent<BoxCollider2D>().enabled = true;
		fire_rate = 0.1f;
		health = max_hp;
	}

	public void DespawnMonster()
	{
		active = false;
		StopAllCoroutines();
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
		anim.Play("enemy_gerbera_attack");
		StartCoroutine("ThrowDeathBall");
	}

	private IEnumerator ThrowDeathBall()
	{
		yield return new WaitForSeconds(0.4f);
		fire_anim.Play("plant_effect_gerbera_shoot");
		GameObject new_deathball = Object.Instantiate(Deathball, Firepoint.transform.position, base.transform.rotation);
		new_deathball.GetComponent<Rigidbody2D>().velocity = base.transform.right * -2.5f;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerController>().fire_dash)
		{
			if (col.transform.position.x < base.transform.position.x)
			{
				DamageFromRight(2);
			}
			else if (col.transform.position.x > base.transform.position.x)
			{
				DamageFromLeft(2);
			}
			else
			{
				DamageFromLeft(2);
			}
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
			anim.Play("enemy_gerbera_damaged");
			source.Play();
			damaging = true;
			StartCoroutine("StopDamaging");
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
			anim.Play("enemy_gerbera_damaged");
			source.Play();
			damaging = true;
			StartCoroutine("StopDamaging");
		}
	}

	private void Dead()
	{
		dead = true;
		active = false;
		DropShards();
		GetComponent<BoxCollider2D>().enabled = false;
		StopAllCoroutines();
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		source.PlayOneShot(death_sound);
		anim.Play("enemy_gerbera_death");
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
