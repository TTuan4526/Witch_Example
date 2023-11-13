using UnityEngine;

public class zombie_mushroom_splitter : MonoBehaviour
{
	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public GameObject split;

	public AudioClip death_clip;

	public Transform attack_position;

	private int health = 3;

	public string state = string.Empty;

	private float speed = 0.5f;

	private float fire_rate;

	private Vector2 current_speed;

	private Vector3 start;

	public Vector3 end;

	private Vector3 temp_position;

	private Vector3 default_start;

	private Vector3 default_end;

	private Vector3 default_scale;

	private Transform witch;

	private Animator anim;

	private Rigidbody2D rbody;

	private AudioSource source;

	private bool damaging;

	private bool dead;

	private bool facing_right;

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
		witch = GameObject.FindGameObjectWithTag("Player").transform;
		default_start = base.transform.localPosition;
		default_end = end;
		default_scale = base.transform.localScale;
		current_speed = new Vector2(-1.5f * speed, 0f);
	}

	public void SpawnMonster()
	{
		anim.Play("zombie_mushroom_idle");
		GetComponent<EdgeCollider2D>().enabled = true;
		dead = false;
		damaging = false;
		facing_right = false;
		attacking = false;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		end = default_end;
		start = default_start;
		current_speed = new Vector2(-1.5f * speed, 0f);
		rbody.velocity = current_speed;
		health = 3;
		state = "patrol";
	}

	public void DespawnMonster()
	{
		rbody.velocity = new Vector3(0f, 0f, 0f);
		anim.Play("zombie_mushroom_stay");
		StopAllCoroutines();
		current_speed = new Vector2(-1.5f * speed, 0f);
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
		else if (state == "armed")
		{
			if ((witch.transform.position.x < base.transform.position.x && facing_right) || (witch.transform.position.x > base.transform.position.x && !facing_right))
			{
				facing_right = !facing_right;
				Flip();
			}
			if (fire_rate <= 0f && !damaging)
			{
				Attack();
			}
		}
		else if (state == "stay" && ((witch.transform.position.x < base.transform.position.x && facing_right) || (witch.transform.position.x > base.transform.position.x && !facing_right)))
		{
			facing_right = !facing_right;
			Flip();
		}
	}

	private void ThrowArrow()
	{
		if (base.transform.localScale.x == -1f)
		{
			GameObject gameObject = Object.Instantiate(split, attack_position.transform.position, Quaternion.Euler(0f, 0f, 180f));
			gameObject.GetComponent<Rigidbody2D>().velocity = base.transform.right * 3.75f;
		}
		else if (base.transform.localScale.x == 1f)
		{
			GameObject gameObject2 = Object.Instantiate(split, attack_position.transform.position, Quaternion.Euler(0f, 0f, 0f));
			gameObject2.GetComponent<Rigidbody2D>().velocity = base.transform.right * -3.75f;
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
		if (!dead && (state == "patrol" || state == "stay"))
		{
			if (fire_rate > 1f)
			{
				fire_rate = 1f;
			}
			state = "armed";
			anim.Play("zombie_mushroom_stay");
		}
	}

	public void PlayerLeaveArea()
	{
		if (!dead && state == "armed")
		{
			state = "stay";
		}
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
		GetComponent<AudioSource>().Play();
		anim.Play("zombie_mushroom_dead");
		source.PlayOneShot(death_clip, 0.2f);
		GetComponent<EdgeCollider2D>().enabled = false;
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
			if (health <= 0)
			{
				Dead();
				return;
			}
			fire_rate = 0f;
			source.Play();
			anim.Play("zombie_mushroom_damaged");
			damaging = true;
			rbody.velocity = new Vector2(0f, 0f);
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
			fire_rate = 0f;
			source.Play();
			anim.Play("zombie_mushroom_damaged");
			damaging = true;
			rbody.velocity = new Vector2(0f, 0f);
		}
	}

	private void Attack()
	{
		fire_rate = 2f;
		attacking = true;
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("zombie_mushroom_shooter");
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

	private void AttackDamage()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(attack_position.position, 1f);
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
	}

	private void StopDamaging()
	{
		damaging = false;
	}
}
