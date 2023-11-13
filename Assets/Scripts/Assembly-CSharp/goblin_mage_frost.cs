using System.Collections;
using UnityEngine;

public class goblin_mage_frost : MonoBehaviour
{
	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public GameObject fire_circle;

	public AudioClip death_sound;

	public int max_hp;

	private int health = 3;

	private string state = string.Empty;

	private float speed = 0.5f;

	private float fire_rate;

	private Vector2 current_speed;

	private Vector3 start;

	public Vector3 end;

	private Vector3 temp_position;

	private Vector3 default_start;

	private Vector3 default_end;

	private Vector3 default_scale;

	private Animator anim;

	private Rigidbody2D rbody;

	private AudioSource source;

	private bool damaging;

	private bool dead;

	private bool facing_right = true;

	private bool attacking;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		source = GetComponent<AudioSource>();
		anim = base.gameObject.GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		default_start = base.transform.localPosition;
		default_end = end;
		default_scale = base.transform.localScale;
		current_speed = new Vector2(1.5f * speed, 0f);
	}

	public void SpawnMonster()
	{
		GetComponent<Animator>().Play("goblin_mage_frost_walk");
		dead = false;
		damaging = false;
		facing_right = true;
		attacking = false;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		end = default_end;
		start = default_start;
		GetComponent<BoxCollider2D>().enabled = true;
		current_speed = new Vector2(1.5f * speed, 0f);
		GetComponent<Rigidbody2D>().velocity = current_speed;
		state = "patrol";
		health = max_hp;
	}

	public void DespawnMonster()
	{
		rbody.velocity = new Vector3(0f, 0f, 0f);
		StopAllCoroutines();
		current_speed = new Vector2(1.5f * speed, 0f);
		state = string.Empty;
	}

	private void Update()
	{
		if (dead || damaging)
		{
			return;
		}
		if (fire_rate > 0f)
		{
			fire_rate -= Time.deltaTime;
		}
		if (state == "patrol" && !attacking)
		{
			if ((facing_right && base.transform.localPosition.x >= end.x) || (!facing_right && base.transform.localPosition.x <= end.x))
			{
				temp_position = end;
				end = start;
				start = temp_position;
				facing_right = !facing_right;
				if (end.x < start.x)
				{
					current_speed = new Vector2(-1.5f * speed, 0f);
					rbody.velocity = current_speed;
				}
				else if (end.x > start.x)
				{
					current_speed = new Vector2(1.5f * speed, 0f);
					rbody.velocity = current_speed;
				}
				Flip();
			}
		}
		else if (state == "attack" && fire_rate <= 0f && !attacking && !damaging)
		{
			Attack();
		}
	}

	public void Flip()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}

	public void PlayerLeaveArea()
	{
	}

	public void PlayerEnterArea()
	{
		if (!dead && state == "patrol")
		{
			if (fire_rate > 1f)
			{
				fire_rate = 1f;
			}
			state = "attack";
			rbody.velocity = new Vector2(0f, 0f);
			anim.Play("goblin_mage_frost_idle");
		}
	}

	private void Dead()
	{
		dead = true;
		DropShards();
		rbody.velocity = new Vector2(0f, 0f);
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		GetComponent<AudioSource>().Play();
		anim.Play("goblin_mage_frost_dead");
		source.PlayOneShot(death_sound);
		GetComponent<BoxCollider2D>().enabled = false;
	}

	private void BackToNormal()
	{
		if (attacking)
		{
			anim.Play("goblin_mage_frost_cast_end");
			fire_rate = 4f;
			attacking = false;
		}
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

	public void DamageFromRight(int amount)
	{
		if (!dead)
		{
			health -= amount;
			if (attacking)
			{
				fire_rate = 4f;
				attacking = false;
			}
			if (health <= 0)
			{
				Dead();
				return;
			}
			source.Play();
			anim.Play("goblin_mage_frost_damage");
			damaging = true;
			rbody.velocity = new Vector2(0f, 0f);
		}
	}

	public void DamageFromLeft(int amount)
	{
		if (!dead)
		{
			health -= amount;
			if (attacking)
			{
				fire_rate = 4f;
				attacking = false;
			}
			if (health <= 0)
			{
				Dead();
				return;
			}
			source.Play();
			anim.Play("goblin_mage_frost_damage");
			damaging = true;
			rbody.velocity = new Vector2(0f, 0f);
		}
	}

	private void Attack()
	{
		attacking = true;
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("goblin_mage_frost_cast_start");
		Object.Instantiate(fire_circle, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z + 0.1f), base.transform.rotation);
		StartCoroutine("AttackDelay");
	}

	private IEnumerator AttackDelay()
	{
		yield return new WaitForSeconds(2.5f);
		BackToNormal();
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

	private void StopAttack()
	{
		attacking = false;
	}

	private void StopDamaging()
	{
		damaging = false;
	}
}
