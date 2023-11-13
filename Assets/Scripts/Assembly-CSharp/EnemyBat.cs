using System.Collections;
using Pathfinding;
using UnityEngine;

public class EnemyBat : MonoBehaviour
{
	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	public AudioClip death_clip;

	private GameObject player;

	public int max_hp;

	private int health = 2;

	private int current_waypoint;

	private string state = "idle";

	private float speed = 2f;

	private float next_waypoint_distance = 3f;

	private Vector2 current_speed;

	private Vector3 temp_position;

	private Vector3 default_start;

	private Vector3 default_end;

	private Vector3 default_scale;

	private RaycastHit2D hit;

	private Animator anim;

	private AudioSource source;

	private Rigidbody2D rbody;

	private Path path;

	private Seeker seeker;

	private bool pathfinding = true;

	private bool dead;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		default_start = base.transform.localPosition;
		state = string.Empty;
		health = max_hp;
		default_scale = base.transform.localScale;
		anim = base.gameObject.GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		rbody = GetComponent<Rigidbody2D>();
		seeker = GetComponent<Seeker>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void SpawnMonster()
	{
		dead = false;
		base.transform.localPosition = default_start;
		base.transform.localScale = default_scale;
		anim.Play("enemy_bat_idle");
		GetComponent<CircleCollider2D>().enabled = true;
		health = max_hp;
		state = "idle";
	}

	public void DespawnMonster()
	{
		dead = true;
		rbody.velocity = new Vector3(0f, 0f, 0f);
		CancelInvoke("UpdatePath");
		anim.Play("enemy_bat_death");
		GetComponent<CircleCollider2D>().enabled = false;
	}

	private void UpdatePath()
	{
		if (!dead && seeker.IsDone())
		{
			seeker.StartPath(rbody.position, player.transform.position, OnPathComplete);
		}
	}

	private void FixedUpdate()
	{
		if (state == "idle")
		{
			hit = Physics2D.Raycast(base.transform.position, player.transform.position - base.transform.position);
			if (hit.collider.tag == "Player" || hit.collider.tag == "collect_area")
			{
				state = "follow";
				InvokeRepeating("UpdatePath", 0f, 1f);
			}
		}
		else if (state == "follow" && pathfinding && !dead && path != null && current_waypoint < path.vectorPath.Count)
		{
			Vector2 normalized = ((Vector2)path.vectorPath[current_waypoint] - rbody.position).normalized;
			Vector2 vector = normalized * speed * Time.deltaTime;
			rbody.velocity = normalized * speed;
			float num = Vector2.Distance(rbody.position, path.vectorPath[current_waypoint]);
			if (num < next_waypoint_distance)
			{
				current_waypoint++;
			}
			if (vector.x >= 0.01f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (vector.x <= -0.01f)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	private void StopDamaging()
	{
		InvokeRepeating("UpdatePath", 1f, 1f);
	}

	private void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			StartCoroutine("MovementDelay");
			path = p;
			current_waypoint = 0;
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
		rbody.velocity = new Vector3(0f, 0f, 0f);
		CancelInvoke("UpdatePath");
		anim.Play("enemy_bat_death");
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		GetComponent<CircleCollider2D>().enabled = false;
		source.PlayOneShot(death_clip);
	}

	private void DropShards()
	{
		for (int i = 0; i < 5; i++)
		{
			switch (Random.Range(1, 4))
			{
			case 1:
			{
				GameObject gameObject3 = Object.Instantiate(shard_1, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, -1f), base.transform.rotation);
				gameObject3.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 2:
			{
				GameObject gameObject2 = Object.Instantiate(shard_2, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, -1f), base.transform.rotation);
				gameObject2.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 3:
			{
				GameObject gameObject = Object.Instantiate(shard_3, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, -1f), base.transform.rotation);
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
		anim.Play("enemy_bat_damaged");
		source.Play();
		CancelInvoke("UpdatePath");
		rbody.velocity = new Vector2(0f, 0f);
		if (base.transform.localScale.x == -1f)
		{
			rbody.AddForce(new Vector2(-1.25f, 0f), ForceMode2D.Impulse);
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
		anim.Play("enemy_bat_damaged");
		source.Play();
		rbody.velocity = new Vector2(0f, 0f);
		if (base.transform.localScale.x == 1f)
		{
			rbody.AddForce(new Vector2(1.25f, 0f), ForceMode2D.Impulse);
		}
		CancelInvoke("UpdatePath");
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
		else if (col.gameObject.tag == "Player" && !col.gameObject.GetComponent<PlayerController>().meteor_attack)
		{
			col.gameObject.SendMessage("Damage", 1);
		}
	}

	private IEnumerator MovementDelay()
	{
		yield return new WaitForSeconds(0.5f);
	}
}
