using UnityEngine;

public class Turnip : MonoBehaviour
{
	public Vector3 end;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

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

	private bool dead;

	private bool facing_right = true;

	private void Start()
	{
		default_start = base.transform.localPosition;
		default_end = new Vector3(end.x, end.y, base.transform.position.z);
		default_scale = base.transform.localScale;
		anim = base.gameObject.GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	public void SpawnMonster()
	{
		start = default_start;
		end = default_end;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		facing_right = true;
		dead = false;
		anim.Play("turnip_idle");
		GetComponent<BoxCollider2D>().enabled = true;
		current_speed = new Vector2(0.75f * speed, 0f);
		rbody.velocity = current_speed;
	}

	public void DespawnMonster()
	{
	}

	private void Update()
	{
		if (!dead && ((facing_right && base.transform.localPosition.x >= end.x) || (!facing_right && base.transform.localPosition.x <= end.x)))
		{
			temp_position = end;
			end = start;
			start = temp_position;
			facing_right = !facing_right;
			if (end.x < start.x)
			{
				current_speed = new Vector2(-0.75f * speed, 0f);
				rbody.velocity = current_speed;
			}
			else if (end.x > start.x)
			{
				current_speed = new Vector2(0.75f * speed, 0f);
				rbody.velocity = current_speed;
			}
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
		anim.Play("turnip_death");
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
