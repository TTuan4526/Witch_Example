using UnityEngine;

public class GoblinHalberd : MonoBehaviour
{
	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public AudioClip death_clip;

	public Transform attack_position;

	public Vector3 end;

	public int max_hp;

	private int health = 3;

	public string state = string.Empty;

	private float tick;

	private float attack_timer = 1f;

	private float speed = 0.5f;

	private Vector2 current_speed;

	private Vector3 start;

	private Vector3 temp_position;

	private Vector3 default_start;

	private Vector3 default_end;

	private Vector3 default_scale;

	private Transform witch;

	private Animator anim;

	private Rigidbody2D rbody;

	private AudioSource source;

	public bool attacking;

	private bool damaging;

	private bool dead;

	private bool facing_right = true;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		start = base.transform.localPosition;
		source = GetComponent<AudioSource>();
		anim = base.gameObject.GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		witch = GameObject.FindGameObjectWithTag("Player").transform;
		default_start = base.transform.localPosition;
		default_end = end;
		default_scale = base.transform.localScale;
		base.transform.localScale = default_scale;
		current_speed = new Vector2(1.5f * speed, 0f);
	}

	public void SpawnMonster()
	{
		dead = false;
		damaging = false;
		facing_right = true;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		end = default_end;
		GetComponent<Animator>().Play("goblin_halberd_idle");
		GetComponent<Rigidbody2D>().velocity = current_speed;
		GetComponent<BoxCollider2D>().enabled = true;
		tick = 1.5f;
		health = max_hp;
		state = "patrol";
		attacking = false;
	}

	public void DespawnMonster()
	{
		rbody.velocity = new Vector3(0f, 0f, 0f);
		anim.Play("goblin_halberd_idle");
		start = default_start;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		end = default_end;
		facing_right = true;
		current_speed = new Vector2(1.5f * speed, 0f);
		state = string.Empty;
	}

	private void Update()
	{
		if (!dead && !damaging)
		{
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
			else if (state == "follow" && !attacking)
			{
				if (Vector2.Distance(base.transform.position, witch.position) < 0.75f && anim.GetCurrentAnimatorStateInfo(0).IsName("goblin_halberd_idle"))
				{
					Attack();
				}
				else if (witch.transform.position.x < base.transform.position.x)
				{
					if (facing_right)
					{
						facing_right = !facing_right;
						Flip();
					}
					current_speed = new Vector2(-1.5f * speed, 0f);
					rbody.velocity = current_speed;
				}
				else if (witch.transform.position.x > base.transform.position.x)
				{
					if (!facing_right)
					{
						facing_right = !facing_right;
						Flip();
					}
					current_speed = new Vector2(1.5f * speed, 0f);
					rbody.velocity = current_speed;
				}
			}
		}
		if (tick > 0f)
		{
			tick -= Time.deltaTime;
		}
	}

	public void Flip()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}

	public void PlayerEnterArea()
	{
		state = "follow";
		rbody.velocity = new Vector2(0f, 0f);
	}

	public void PlayerLeaveArea()
	{
		state = "patrol";
		if (!attacking)
		{
			if (base.transform.localPosition.x >= end.x && base.transform.localPosition.x >= start.x)
			{
				if (facing_right)
				{
					facing_right = !facing_right;
					Flip();
				}
				if (start.x < end.x)
				{
					temp_position = end;
					end = start;
					start = temp_position;
				}
				current_speed = new Vector2(-1.5f * speed, 0f);
				rbody.velocity = current_speed;
			}
			else if (base.transform.localPosition.x <= end.x && base.transform.localPosition.x <= start.x)
			{
				if (!facing_right)
				{
					facing_right = !facing_right;
					Flip();
				}
				if (start.x > end.x)
				{
					temp_position = end;
					end = start;
					start = temp_position;
				}
				current_speed = new Vector2(1.5f * speed, 0f);
				rbody.velocity = current_speed;
			}
			else if (end.x < base.transform.localPosition.x)
			{
				if (facing_right)
				{
					facing_right = !facing_right;
					Flip();
				}
				current_speed = new Vector2(-1.5f * speed, 0f);
				rbody.velocity = current_speed;
			}
			else if (end.x > base.transform.localPosition.x)
			{
				if (!facing_right)
				{
					facing_right = !facing_right;
					Flip();
				}
				current_speed = new Vector2(1.5f * speed, 0f);
				rbody.velocity = current_speed;
			}
		}
		else if (base.transform.localPosition.x >= end.x && base.transform.localPosition.x >= start.x)
		{
			if (start.x < end.x)
			{
				temp_position = end;
				end = start;
				start = temp_position;
			}
		}
		else if (base.transform.localPosition.x <= end.x && base.transform.localPosition.x <= start.x)
		{
			if (start.x > end.x)
			{
				temp_position = end;
				end = start;
				start = temp_position;
			}
		}
		else if (!(end.x < base.transform.localPosition.x) && !(end.x > base.transform.localPosition.x))
		{
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
		source.Play();
		anim.Play("goblin_halberd_death");
		source.PlayOneShot(death_clip, 0.2f);
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

	public void DamageFromRight(int amount)
	{
		if (dead)
		{
			return;
		}
		health -= amount;
		if (health <= 0)
		{
			Dead();
			return;
		}
		source.Play();
		anim.Play("goblin_halberd_damage");
		damaging = true;
		attacking = false;
		rbody.velocity = new Vector2(0f, 0f);
		if (facing_right)
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
		health -= amount;
		if (health <= 0)
		{
			Dead();
			return;
		}
		source.Play();
		anim.Play("goblin_halberd_damage");
		damaging = true;
		attacking = false;
		rbody.velocity = new Vector2(0f, 0f);
		if (facing_right)
		{
			rbody.AddForce(new Vector2(0.5f, 0f), ForceMode2D.Impulse);
		}
	}

	private void Attack()
	{
		tick = attack_timer;
		attacking = true;
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("goblin_halberd_attack");
	}

	private void AttackDamage()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(attack_position.position, 0.35f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "Player")
			{
				array[i].gameObject.SendMessage("Damage", 2);
			}
		}
	}

	private void StopAttack()
	{
		attacking = false;
		if (dead)
		{
			return;
		}
		anim.Play("goblin_halberd_idle");
		if (!(state == "patrol"))
		{
			return;
		}
		rbody.velocity = current_speed;
		if (end.x < base.transform.localPosition.x)
		{
			if (facing_right)
			{
				facing_right = !facing_right;
				Flip();
			}
			current_speed = new Vector2(-1.5f * speed, 0f);
			rbody.velocity = current_speed;
		}
		else if (end.x > base.transform.localPosition.x)
		{
			if (!facing_right)
			{
				facing_right = !facing_right;
				Flip();
			}
			current_speed = new Vector2(1.5f * speed, 0f);
			rbody.velocity = current_speed;
		}
	}

	private void StopDamaging()
	{
		rbody.velocity = current_speed;
		damaging = false;
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
}
