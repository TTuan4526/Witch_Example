using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class skull_boss : MonoBehaviour
{
	public GameObject boss_area;

	public Slider boss_hp;

	public GameObject boss_name;

	public GameObject bone;

	public Transform central_point;

	public Transform eyes;

	private Animator anim;

	private AudioSource source;

	private Rigidbody2D rbody;

	private bool dead;

	private int hp = 15;

	private int queue;

	private int damage_count;

	private float tick;

	private Vector3 default_position;

	private Vector2 current_speed;

	public string state = string.Empty;

	private Data data;

	private Transform witch;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		rbody = GetComponent<Rigidbody2D>();
		default_position = base.transform.localPosition;
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		witch = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		if (state == "to_center")
		{
			if (Vector3.Distance(base.transform.position, central_point.position) <= 0.1f)
			{
				state = "throw";
				StartCoroutine("BackToIdleWithDelay");
				rbody.velocity = new Vector2(0f, 0f);
			}
		}
		else if (state == "to_center_eyes")
		{
			if (Vector3.Distance(base.transform.position, central_point.position) <= 0.1f)
			{
				state = "eyes";
				rbody.velocity = new Vector2(0f, 0f);
			}
		}
		else if (state == "throw")
		{
			if (tick > 0f)
			{
				tick -= Time.deltaTime;
			}
			if (tick <= 0f)
			{
				tick = 0.1f;
				ThrowBone();
			}
		}
		else if (state == "whirlwind")
		{
			rbody.velocity = new Vector2(witch.transform.position.x - base.transform.position.x, witch.transform.position.y - base.transform.position.y);
		}
		else if (state == "eyes")
		{
			eyes.GetComponent<boss_eyes>().GetDown();
			state = "after_eyes";
		}
	}

	private void NextAction()
	{
		if (queue == 2)
		{
			queue = 0;
		}
		else
		{
			queue++;
		}
		switch (queue)
		{
		case 0:
			GetComponent<Animator>().Play("boss_skull_spin_start");
			rbody.velocity = new Vector2(central_point.position.x - base.transform.position.x, central_point.position.y - base.transform.position.y);
			state = "to_center";
			break;
		case 1:
			StartCoroutine("BackToIdleWithDelay");
			GetComponent<Animator>().Play("boss_skull_spin_start");
			state = "whirlwind";
			break;
		case 2:
			GetComponent<Animator>().Play("boss_skull_spin_start");
			rbody.velocity = new Vector2(central_point.position.x - base.transform.position.x, central_point.position.y - base.transform.position.y);
			state = "to_center_eyes";
			break;
		}
	}

	private void ThrowBone()
	{
		GameObject gameObject = Object.Instantiate(bone, new Vector3(base.transform.position.x, base.transform.position.y - 0.15f, base.transform.position.z + 0.5f), base.transform.rotation);
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(3.5f, 6.5f), 0f), ForceMode2D.Impulse);
	}

	public void DamageFromRight(int amount)
	{
		Damage(amount);
	}

	public void DamageFromLeft(int amount)
	{
		Damage(amount);
	}

	private void Damage(int amount)
	{
		if (dead || !(state == "idle"))
		{
			return;
		}
		damage_count++;
		if (damage_count > 3)
		{
			damage_count = 0;
			StopCoroutine("NextActionWithDelay");
			NextAction();
			return;
		}
		hp -= amount;
		if (hp <= 0)
		{
			Dead();
			boss_hp.value = 0f;
		}
		else
		{
			boss_hp.value = (float)hp / 15f;
			anim.Play("boss_skull_damage");
			source.Play();
		}
	}

	private void Dead()
	{
		GetComponent<AudioSource>().Play();
		dead = true;
		state = "dead";
		StopAllCoroutines();
		rbody.velocity = new Vector2(0f, 0f);
		GetComponent<CircleCollider2D>().enabled = false;
		anim.Play("boss_skull_dead");
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!dead && col.gameObject.tag == "Player")
		{
			if (col.gameObject.GetComponent<PlayerController>().fire_dash && state == "idle")
			{
				Damage(2);
			}
			else if (col.gameObject.GetComponent<PlayerController>().meteor_attack && state == "idle")
			{
				Damage(3);
			}
			else if (!col.gameObject.GetComponent<PlayerController>().fire_dash && (state == "whirlwind" || state == "throw" || state == "to_center"))
			{
				col.SendMessage("Damage", 2);
			}
		}
	}

	private void BossDespawn()
	{
		if (!dead)
		{
			hp = 15;
			boss_hp.value = 1f;
			state = string.Empty;
			rbody.velocity = new Vector2(0f, 0f);
			StopAllCoroutines();
			base.transform.localPosition = default_position;
			boss_area.SendMessage("Respawn");
			anim.Play("boss_skull_idle");
		}
	}

	private void Defeat()
	{
		GameObject.Find("boss_area").GetComponent<skull_boss_area>().BossDefeated();
	}

	public void CheckBossStatus()
	{
		if (GameObject.Find("boss_area").GetComponent<skull_boss_area>().on_what_story_phase <= data.story_progress)
		{
			anim.Play("boss_skull_defeated");
			GetComponent<CircleCollider2D>().enabled = false;
		}
	}

	public void AfterEyes()
	{
		NextAction();
	}

	public void Starter()
	{
		hp = 15;
		queue = 2;
		base.transform.localPosition = default_position;
		NextAction();
	}

	private IEnumerator BackToIdleWithDelay()
	{
		yield return new WaitForSeconds(5f);
		BackToIdle();
	}

	private IEnumerator NextActionWithDelay()
	{
		yield return new WaitForSeconds(3f);
		NextAction();
	}

	private void BackToIdle()
	{
		state = "idle";
		rbody.velocity = new Vector2(0f, 0f);
		StartCoroutine("NextActionWithDelay");
		GetComponent<Animator>().Play("boss_skull_spin_end");
	}
}
