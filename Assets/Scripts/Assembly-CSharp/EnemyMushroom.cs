using System.Collections;
using UnityEngine;

public class EnemyMushroom : MonoBehaviour
{
	public Vector3 end;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public AudioClip death_clip;

	public Transform attack_position;

	public GameObject attack_area;

	private int health = 2;

	private float speed = 1f;

	private Vector2 current_speed;

	private Vector3 start;

	private Vector3 temp_position;

	private Vector3 default_start;

	private Vector3 default_end;

	private Vector3 default_scale;

	private Animator anim;

	private AudioSource source;

	private Rigidbody2D rbody;

	private bool damaging;

	private bool dead;

	private bool attacking;

	private bool attack_ready = true;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		source = GetComponent<AudioSource>();
		start = base.transform.localPosition;
		anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		current_speed = new Vector2(-1f * speed, 0f);
		default_start = base.transform.localPosition;
		default_end = new Vector3(end.x, end.y, base.transform.position.z);
		default_scale = base.transform.localScale;
		base.transform.localScale = default_scale;
	}

	public void SpawnMonster()
	{
		dead = false;
		damaging = false;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		end = default_end;
		health = 2;
		GetComponent<Animator>().Play("enemy_mushroom_idle");
		GetComponent<Rigidbody2D>().velocity = current_speed;
		GetComponent<EdgeCollider2D>().enabled = true;
		attack_ready = true;
		attacking = false;
		start = default_start;
		current_speed = new Vector2(-1f * speed, 0f);
	}

	public void DespawnMonster()
	{
		rbody.velocity = new Vector3(0f, 0f, 0f);
		anim.Play("enemy_mushroom_idle");
		start = default_start;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		end = default_end;
		attack_area.SetActive(true);
		current_speed = new Vector2(-1f * speed, 0f);
		StopAllCoroutines();
	}

	private void Update()
	{
		if (!dead && !damaging && ((base.transform.localScale.x == -1f && base.transform.localPosition.x >= end.x) || (base.transform.localScale.x == 1f && base.transform.localPosition.x <= end.x)))
		{
			temp_position = end;
			end = start;
			start = temp_position;
			current_speed *= -1f;
			rbody.velocity = current_speed;
			Flip();
		}
	}

	public void Flip()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}

	private void Dead()
	{
		dead = true;
		DropShards();
		rbody.velocity = new Vector2(0f, 0f);
		StopAllCoroutines();
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		GetComponent<EdgeCollider2D>().enabled = false;
		source.PlayOneShot(death_clip, 1f);
		anim.Play("enemy_mushroom_death");
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
				gameObject3.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3.5f), 0f), ForceMode2D.Impulse);
				break;
			}
			case 2:
			{
				GameObject gameObject2 = Object.Instantiate(shard_2, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject2.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3.5f), 0f), ForceMode2D.Impulse);
				break;
			}
			case 3:
			{
				GameObject gameObject = Object.Instantiate(shard_3, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3.5f), 0f), ForceMode2D.Impulse);
				break;
			}
			}
		}
	}

	public void DamageFromRight(int amount)
	{
		if (dead)
		{
			return;
		}
		if (attacking)
		{
			attack_area.SetActive(true);
			attacking = false;
			attack_ready = false;
			anim.ResetTrigger("attack_end");
			StopCoroutine("AttackEndDelay");
			StartCoroutine("NewAttackDelay");
		}
		health -= amount;
		if (health <= 0)
		{
			Dead();
			return;
		}
		source.Play();
		anim.Play("enemy_mushroom_damaged");
		damaging = true;
		rbody.velocity = new Vector2(0f, 0f);
		if (base.transform.localScale.x == 1f)
		{
			rbody.AddForce(new Vector2(-0.5f, 0f), ForceMode2D.Impulse);
		}
	}

	public void DamageFromLeft(int amount)
	{
		if (dead)
		{
			return;
		}
		if (attacking)
		{
			attack_area.SetActive(true);
			attacking = false;
			attack_ready = false;
			anim.ResetTrigger("attack_end");
			StopCoroutine("AttackEndDelay");
			StartCoroutine("NewAttackDelay");
		}
		health -= amount;
		if (health <= 0)
		{
			Dead();
			return;
		}
		source.Play();
		anim.Play("enemy_mushroom_damaged");
		damaging = true;
		rbody.velocity = new Vector2(0f, 0f);
		if (base.transform.localScale.x == -1f)
		{
			rbody.AddForce(new Vector2(0.5f, 0f), ForceMode2D.Impulse);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (dead)
		{
			return;
		}
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
		else if (attacking && col.gameObject.tag == "Player" && !col.gameObject.GetComponent<PlayerController>().meteor_attack)
		{
			col.SendMessage("Damage", 2);
		}
	}

	private void StopDamaging()
	{
		rbody.velocity = current_speed;
		damaging = false;
	}

	private void Attack()
	{
		if (attack_ready && !dead && !attacking)
		{
			if (damaging)
			{
				StopDamaging();
			}
			attacking = true;
			attack_ready = false;
			attack_area.SetActive(false);
			anim.Play("enemy_mushroom_attack_start");
			StartCoroutine("AttackEndDelay");
		}
	}

	private IEnumerator AttackEndDelay()
	{
		yield return new WaitForSeconds(2.5f);
		anim.SetTrigger("attack_end");
		attack_area.SetActive(true);
		attacking = false;
		StartCoroutine("NewAttackDelay");
	}

	private IEnumerator NewAttackDelay()
	{
		yield return new WaitForSeconds(1.5f);
		attack_ready = true;
	}

	public void MeleeAttack()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(attack_position.position, 0.5f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "Player")
			{
				array[i].gameObject.SendMessage("Damage", 1);
			}
		}
	}
}
