using UnityEngine;

public class ghost : MonoBehaviour
{
	private string state = string.Empty;

	private Rigidbody2D rbody;

	private Animator anim;

	private GameObject player;

	private AudioSource source;

	private Vector3 default_start;

	private bool dead;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		default_start = base.transform.localPosition;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (state == "follow" && !dead)
		{
			Vector2 vector = player.transform.position - base.transform.position;
			rbody.velocity = vector.normalized / 1.5f;
			if (rbody.velocity.x >= 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (rbody.velocity.x < 0f)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
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
		else if (col.gameObject.tag == "Player" && !col.gameObject.GetComponent<PlayerController>().meteor_attack)
		{
			col.SendMessage("Damage", 4);
		}
	}

	public void SpawnMonster()
	{
		state = "follow";
		anim.Play("enemy_ghost_idle");
		GetComponent<BoxCollider2D>().enabled = true;
		base.transform.localPosition = default_start;
		dead = false;
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
		state = "stay";
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("enemy_ghost_dead");
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

	public void DespawnMonster()
	{
		state = "stay";
		rbody.velocity = new Vector2(0f, 0f);
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
