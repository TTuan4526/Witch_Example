using System.Collections;
using UnityEngine;

public class EnemyCykuta : MonoBehaviour
{
	private float tick = 0.5f;

	private int hp = 2;

	private bool dead = true;

	private bool damaging;

	public GameObject death_ball;

	public GameObject firepoint;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	private Animator anim;

	private Animator fire_anim;

	private AudioSource source;

	public AudioClip destruction_sound;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		fire_anim = firepoint.GetComponent<Animator>();
	}

	public void SpawnMonster()
	{
		dead = false;
		damaging = false;
		GetComponent<Animator>().Play("enemy_cykuta_idle");
		GetComponent<CircleCollider2D>().enabled = true;
		tick = 0.5f;
		hp = 2;
	}

	public void DespawnMonster()
	{
		StopAllCoroutines();
		dead = true;
	}

	private void Update()
	{
		if (!dead)
		{
			if (tick > 0f)
			{
				tick -= Time.deltaTime;
			}
			if (tick <= 0f && !damaging)
			{
				tick = 2f;
				Attack();
			}
		}
	}

	private void Attack()
	{
		anim.Play("enemy_cykuta_attack");
		StartCoroutine("ThrowDeathBalls");
	}

	public void DamageFromLeft(int amount)
	{
		if (!dead)
		{
			hp -= amount;
			if (hp <= 0)
			{
				Dead();
				return;
			}
			anim.Play("enemy_cykuta_damaged");
			damaging = true;
		}
	}

	public void DamageFromRight(int amount)
	{
		if (!dead)
		{
			hp -= amount;
			if (hp <= 0)
			{
				Dead();
				return;
			}
			anim.Play("enemy_cykuta_damaged");
			source.Play();
			damaging = true;
		}
	}

	private void Damage(string type)
	{
		if (!dead)
		{
			hp--;
			if (hp <= 0)
			{
				Dead();
				return;
			}
			anim.Play("enemy_cykuta_damaged");
			source.Play();
			damaging = true;
			StartCoroutine("StopDamaging");
		}
	}

	private void Dead()
	{
		dead = true;
		DropShards();
		GetComponent<CircleCollider2D>().enabled = false;
		StopAllCoroutines();
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		anim.Play("enemy_cykuta_death");
		source.PlayOneShot(destruction_sound);
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

	private IEnumerator ThrowDeathBalls()
	{
		yield return new WaitForSeconds(0.25f);
		fire_anim.Play("plant_effect_shoot");
		GameObject new_death_ball = Object.Instantiate(death_ball, new Vector3(base.transform.position.x, base.transform.position.y + 0.15f, base.transform.position.z - 0.5f), base.transform.rotation);
		new_death_ball.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.5f, 2f, 0f), ForceMode2D.Impulse);
		GameObject new_death_ball_1 = Object.Instantiate(death_ball, new Vector3(base.transform.position.x, base.transform.position.y + 0.15f, base.transform.position.z - 0.5f), base.transform.rotation);
		new_death_ball_1.GetComponent<Rigidbody2D>().AddForce(new Vector3(-0.5f, 2f, 0f), ForceMode2D.Impulse);
		GameObject new_death_ball_2 = Object.Instantiate(death_ball, new Vector3(base.transform.position.x, base.transform.position.y + 0.15f, base.transform.position.z - 0.5f), base.transform.rotation);
		new_death_ball_2.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.5f, 3.5f, 0f), ForceMode2D.Impulse);
		GameObject new_death_ball_3 = Object.Instantiate(death_ball, new Vector3(base.transform.position.x, base.transform.position.y + 0.15f, base.transform.position.z - 0.5f), base.transform.rotation);
		new_death_ball_3.GetComponent<Rigidbody2D>().AddForce(new Vector3(-0.5f, 3.5f, 0f), ForceMode2D.Impulse);
		GameObject new_death_ball_4 = Object.Instantiate(death_ball, new Vector3(base.transform.position.x, base.transform.position.y + 0.15f, base.transform.position.z - 0.5f), base.transform.rotation);
		new_death_ball_4.GetComponent<Rigidbody2D>().AddForce(new Vector3(2f, 2.5f, 0f), ForceMode2D.Impulse);
		GameObject new_death_ball_5 = Object.Instantiate(death_ball, new Vector3(base.transform.position.x, base.transform.position.y + 0.15f, base.transform.position.z - 0.5f), base.transform.rotation);
		new_death_ball_5.GetComponent<Rigidbody2D>().AddForce(new Vector3(-2f, 2.5f, 0f), ForceMode2D.Impulse);
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

	private void StopDamaging()
	{
		damaging = false;
	}
}
