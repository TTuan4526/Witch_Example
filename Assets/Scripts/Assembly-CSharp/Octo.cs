using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Octo : MonoBehaviour
{
	public GameObject boss_area;

	public Slider boss_hp;

	public GameObject boss_name;

	public GameObject spawner;

	public Transform spikes;

	public Transform left_cr;

	public Transform right_cr;

	public Dialog end_dialog;

	private bool facing_right;

	private Animator anim;

	private AudioSource source;

	private Rigidbody2D rbody;

	private bool dead;

	private bool spikes_dropped;

	private int hp = 15;

	private int stamina = 4;

	private int jump_tick = 3;

	private int rnd = -1;

	private int damage_count;

	private Vector3 default_position;

	private Vector2 current_speed;

	private Vector3 default_scale;

	private Vector3 target;

	public Transform left_corner;

	public Transform right_corner;

	public Transform center;

	public string state = string.Empty;

	private Data data;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		rbody = GetComponent<Rigidbody2D>();
		default_scale = base.transform.localScale;
		default_position = base.transform.localPosition;
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
	}

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Defeat()
	{
		GameObject.Find("boss_area").GetComponent<octo_area>().BossDefeated();
	}

	private void Update()
	{
		if (state == "idle")
		{
			int num = rnd;
			while (rnd == num || (num == 2 && rnd == 1) || (rnd == 2 && spikes_dropped))
			{
				rnd = UnityEngine.Random.Range(0, 3);
			}
			switch (rnd)
			{
			case 0:
				if (facing_right)
				{
					target = right_corner.localPosition;
					current_speed = new Vector2(2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run";
				}
				else if (!facing_right)
				{
					target = left_corner.localPosition;
					current_speed = new Vector2(-2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run";
				}
				break;
			case 1:
				if (facing_right)
				{
					target = center.localPosition;
					current_speed = new Vector2(2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run_center";
				}
				else if (!facing_right)
				{
					target = center.localPosition;
					current_speed = new Vector2(-2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run_center";
				}
				break;
			case 2:
				foreach (Transform spike in spikes)
				{
					spike.GetComponent<ice_cave_spike>().SpawnItem();
				}
				spikes_dropped = true;
				if (facing_right)
				{
					target = center.localPosition;
					current_speed = new Vector2(2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run_center_spikes";
				}
				else if (!facing_right)
				{
					target = center.localPosition;
					current_speed = new Vector2(-2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run_center_spikes";
				}
				break;
			case 3:
				if (facing_right)
				{
					target = right_corner.localPosition;
					current_speed = new Vector2(2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run";
				}
				else if (!facing_right)
				{
					target = left_corner.localPosition;
					current_speed = new Vector2(-2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("octo_run");
					state = "run";
				}
				break;
			}
		}
		else if (state == "run")
		{
			if ((facing_right && base.transform.localPosition.x >= target.x) || (!facing_right && base.transform.localPosition.x <= target.x))
			{
				rbody.velocity = new Vector2(0f, 0f);
				Flip();
				state = "turn";
				NewAction();
			}
		}
		else if (state == "run_center")
		{
			if ((facing_right && base.transform.localPosition.x >= target.x) || (!facing_right && base.transform.localPosition.x <= target.x))
			{
				rbody.velocity = new Vector2(0f, 0f);
				state = "jump_attack";
				anim.Play("octo_jump");
			}
		}
		else if (state == "run_center_spikes" && ((facing_right && base.transform.localPosition.x >= target.x) || (!facing_right && base.transform.localPosition.x <= target.x)))
		{
			rbody.velocity = new Vector2(0f, 0f);
			state = "jump_spikes";
			anim.Play("octo_jump");
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!dead && col.gameObject.tag == "Player")
		{
			if (col.gameObject.GetComponent<PlayerController>().fire_dash && state == "tired")
			{
				Damage(2);
			}
			else if (col.gameObject.GetComponent<PlayerController>().meteor_attack && state == "tired")
			{
				Damage(2);
			}
			else if (!col.gameObject.GetComponent<PlayerController>().fire_dash && (state == "run" || state == "run_center" || state == "run_center_spikes" || state == "jump_attack"))
			{
				col.SendMessage("Damage", 2);
			}
		}
	}

	private void NewAction()
	{
		stamina--;
		if (stamina <= 0)
		{
			Tired();
		}
		else
		{
			state = "idle";
		}
	}

	private IEnumerator TiredEnd()
	{
		yield return new WaitForSeconds(5f);
		BackToIdle();
	}

	private IEnumerator AfterJump()
	{
		yield return new WaitForSeconds(0.3f);
		if (state == "jump_attack")
		{
			jump_tick--;
			if (jump_tick <= 0)
			{
				jump_tick = 3;
				stamina--;
				if (stamina <= 0)
				{
					Tired();
				}
				else
				{
					state = "idle";
				}
			}
			else
			{
				anim.Play("octo_jump");
			}
		}
		else if (state == "jump_spikes")
		{
			stamina--;
			if (stamina <= 0)
			{
				Tired();
			}
			else
			{
				state = "idle";
			}
		}
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
		if (dead || !(state == "tired"))
		{
			return;
		}
		damage_count++;
		if (damage_count > 3)
		{
			damage_count = 0;
			StopCoroutine("TiredEnd");
			BackToIdle();
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
			anim.Play("octo_damage");
			source.Play();
		}
	}

	private void Dead()
	{
		dead = true;
		state = "dead";
		StopAllCoroutines();
		rbody.velocity = new Vector2(0f, 0f);
		GetComponent<BoxCollider2D>().enabled = false;
		anim.Play("octo_rest_start");
	}

	private void BossDespawn()
	{
		if (dead)
		{
			return;
		}
		hp = 15;
		boss_hp.value = 1f;
		stamina = 4;
		spikes_dropped = false;
		jump_tick = 3;
		damage_count = 0;
		state = string.Empty;
		rbody.velocity = new Vector2(0f, 0f);
		StopAllCoroutines();
		base.transform.localPosition = default_position;
		base.transform.localScale = default_scale;
		boss_area.SendMessage("Respawn");
		anim.Play("octo_rest");
		target = right_corner.localPosition;
		foreach (Transform spike in spikes)
		{
			spike.GetComponent<ice_cave_spike>().Broke();
		}
	}

	private void JumpEffect()
	{
		if (state == "jump_attack")
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(spawner, left_cr.position, Quaternion.identity);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(spawner, right_cr.position, Quaternion.identity);
			if (!facing_right)
			{
				gameObject.GetComponent<thorn_spawner>().facing_right = true;
				gameObject2.GetComponent<thorn_spawner>().facing_right = false;
			}
			else
			{
				gameObject.GetComponent<thorn_spawner>().facing_right = false;
				gameObject2.GetComponent<thorn_spawner>().facing_right = true;
			}
			gameObject.GetComponent<thorn_spawner>().Spawn();
			gameObject2.GetComponent<thorn_spawner>().Spawn();
		}
		else if (state == "jump_spikes")
		{
			StartCoroutine("SpikesDrop");
		}
	}

	private IEnumerator SpikesDrop()
	{
		IEnumerator enumerator = spikes.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform spike = (Transform)enumerator.Current;
				yield return new WaitForSeconds(0.05f);
				spike.GetComponent<ice_cave_spike>().Fall();
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = (disposable = enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
		yield return new WaitForSeconds(2f);
		spikes_dropped = false;
	}

	private void Tired()
	{
		state = "tired";
		stamina = 4;
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("octo_idle");
		StartCoroutine("TiredEnd");
	}

	private void CheckDirection()
	{
		if (facing_right && base.transform.localScale.x != 1f)
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}
		else if (!facing_right && base.transform.localScale.x == 1f)
		{
			Vector3 localScale2 = base.transform.localScale;
			localScale2.x *= -1f;
			base.transform.localScale = localScale2;
		}
	}

	private void BackToIdle()
	{
		state = "idle";
	}

	public void CheckBossStatus()
	{
		if (GameObject.Find("boss_area").GetComponent<octo_area>().on_what_story_phase <= data.story_progress)
		{
			anim.Play("octo_defeated");
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void Starter()
	{
		hp = 15;
		stamina = 4;
		spikes_dropped = false;
		jump_tick = 3;
		damage_count = 0;
		base.transform.localPosition = default_position;
		base.transform.localScale = default_scale;
		anim.Play("octo_ice_break");
	}

	private void Flip()
	{
		facing_right = !facing_right;
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}
}
