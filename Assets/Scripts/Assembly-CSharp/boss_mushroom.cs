using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class boss_mushroom : MonoBehaviour
{
	public GameObject slam_effect;

	public GameObject boss_area;

	public Slider boss_hp;

	public GameObject boss_name;

	public Transform left_corner;

	public Transform right_corner;

	public GameObject[] ground_mushrooms;

	public GameObject[] left_mushrooms;

	public GameObject[] central_mushrooms;

	public GameObject[] right_mushrooms;

	private Animator anim;

	private AudioSource source;

	private Rigidbody2D rbody;

	public AudioClip slam_sound;

	private bool dead;

	private int hp = 10;

	private int stamina = 4;

	private int damage_count;

	private Vector3 default_position;

	private Vector3 default_scale;

	private Vector3 target;

	private Vector2 current_speed;

	public string player_position = "ground";

	public bool facing_right = true;

	public string state = string.Empty;

	private Data data;

	private Transform witch;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		rbody = GetComponent<Rigidbody2D>();
		default_position = base.transform.localPosition;
		default_scale = base.transform.localScale;
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		witch = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			source.volume = 0.05f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	private void Update()
	{
		switch (state)
		{
		case "idle":
		{
			if (Vector2.Distance(base.transform.position, witch.transform.position) <= 1f)
			{
				CheckDirection();
				state = "melee";
				anim.Play("boss_mushroom_slam");
				break;
			}
			if (player_position == "left" || player_position == "central" || player_position == "right")
			{
				state = "magic";
				PlantMushrooms(player_position);
				CheckDirection();
				anim.Play("boss_mushroom_rise_hand");
				break;
			}
			int num = Random.Range(0, 3);
			if (num == 0 || num == 1)
			{
				if (facing_right)
				{
					target = right_corner.localPosition;
					current_speed = new Vector2(2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("boss_mushroom_run");
					state = "run";
				}
				else if (!facing_right)
				{
					target = left_corner.localPosition;
					current_speed = new Vector2(-2f, 0f);
					rbody.velocity = current_speed;
					CheckDirection();
					anim.Play("boss_mushroom_run");
					state = "run";
				}
			}
			else
			{
				state = "magic";
				PlantMushrooms("central");
				CheckDirection();
				anim.Play("boss_mushroom_rise_hand");
			}
			break;
		}
		case "run":
			if ((facing_right && base.transform.localPosition.x >= target.x) || (!facing_right && base.transform.localPosition.x <= target.x))
			{
				rbody.velocity = new Vector2(0f, 0f);
				anim.Play("boss_mushroom_turn");
				facing_right = !facing_right;
				state = "turn";
				NewAction();
			}
			break;
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
		damage_count++;
		if (damage_count > 3)
		{
			damage_count = 0;
			StopCoroutine("TiredEnd");
			BackToIdle();
		}
		else if (!dead && state == "tired")
		{
			hp -= amount;
			if (hp <= 0)
			{
				Dead();
				boss_hp.value = 0f;
			}
			else
			{
				boss_hp.value = (float)hp / 10f;
				anim.Play("boss_mushroom_damage");
				source.Play();
			}
		}
	}

	private void Dead()
	{
		dead = true;
		state = "dead";
		StopAllCoroutines();
		rbody.velocity = new Vector2(0f, 0f);
		GetComponent<BoxCollider2D>().enabled = false;
		anim.Play("boss_mushroom_dead");
	}

	private void PlantMushrooms(string pl_pos)
	{
		for (int i = 0; i < ground_mushrooms.Length; i++)
		{
			ground_mushrooms[i].GetComponent<small_mushroom>().Grow();
		}
		switch (pl_pos)
		{
		case "right":
		{
			for (int l = 0; l < central_mushrooms.Length; l++)
			{
				central_mushrooms[l].GetComponent<small_mushroom>().Grow();
			}
			for (int m = 0; m < right_mushrooms.Length; m++)
			{
				right_mushrooms[m].GetComponent<small_mushroom>().Grow();
			}
			break;
		}
		case "central":
		{
			for (int n = 0; n < left_mushrooms.Length; n++)
			{
				left_mushrooms[n].GetComponent<small_mushroom>().Grow();
			}
			for (int num = 0; num < central_mushrooms.Length; num++)
			{
				central_mushrooms[num].GetComponent<small_mushroom>().Grow();
			}
			break;
		}
		case "left":
		{
			for (int j = 0; j < left_mushrooms.Length; j++)
			{
				left_mushrooms[j].GetComponent<small_mushroom>().Grow();
			}
			for (int k = 0; k < central_mushrooms.Length; k++)
			{
				central_mushrooms[k].GetComponent<small_mushroom>().Grow();
			}
			break;
		}
		}
		StartCoroutine(Explode(pl_pos));
	}

	private void ExplodeMushrooms(string pl_pos)
	{
		CheckDirection();
		anim.Play("boss_mushroom_down_hand");
		NewAction();
		StartCoroutine(MushroomsGroundExplosionSounds(0f));
		switch (pl_pos)
		{
		case "right":
			StartCoroutine(MushroomsCentralExplosionSounds(0.2f));
			StartCoroutine(MushroomsRightExplosionSounds(0.4f));
			break;
		case "central":
			StartCoroutine(MushroomsLeftExplosionSounds(0.2f));
			StartCoroutine(MushroomsCentralExplosionSounds(0.4f));
			break;
		case "left":
			StartCoroutine(MushroomsLeftExplosionSounds(0.2f));
			StartCoroutine(MushroomsCentralExplosionSounds(0.4f));
			break;
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
			else if (!col.gameObject.GetComponent<PlayerController>().fire_dash && state == "run")
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
	}

	private void BossDespawn()
	{
		if (!dead)
		{
			hp = 10;
			boss_hp.value = 1f;
			damage_count = 0;
			stamina = 4;
			state = string.Empty;
			rbody.velocity = new Vector2(0f, 0f);
			StopAllCoroutines();
			base.transform.localPosition = default_position;
			base.transform.localScale = default_scale;
			boss_area.SendMessage("Respawn");
			anim.Play("boss_mushroom_rest");
			target = right_corner.localPosition;
		}
	}

	private void Defeat()
	{
		GameObject.Find("boss_area").GetComponent<mushroom_boss_area>().BossDefeated();
	}

	public void CheckBossStatus()
	{
		if (GameObject.Find("boss_area").GetComponent<mushroom_boss_area>().on_what_story_phase <= data.story_progress)
		{
			anim.Play("boss_mushroom_defeated");
			GetComponent<BoxCollider2D>().enabled = false;
		}
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

	public void Starter()
	{
		facing_right = true;
		damage_count = 0;
		hp = 10;
		base.transform.localPosition = default_position;
		base.transform.localScale = default_scale;
		GetComponent<Animator>().SetTrigger("awake");
	}

	private void AttackEffect()
	{
		slam_effect.transform.position = new Vector3(base.transform.position.x - 0.1f, base.transform.position.y - 0.15f, base.transform.position.z - 1f);
		slam_effect.GetComponent<Animator>().Play("mushroom_slam_do");
		source.PlayOneShot(slam_sound);
		NewAction();
		Collider2D[] array = Physics2D.OverlapCircleAll(new Vector3(base.transform.position.x, base.transform.position.y - 0.7f, base.transform.position.z), 1.2f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "Player")
			{
				array[i].gameObject.SendMessage("Damage", 2);
			}
		}
	}

	private IEnumerator Explode(string pl_pos)
	{
		yield return new WaitForSeconds(3f);
		ExplodeMushrooms(pl_pos);
	}

	private IEnumerator MushroomsGroundExplosionSounds(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < ground_mushrooms.Length; i++)
		{
			ground_mushrooms[i].GetComponent<small_mushroom>().Expl();
		}
		ground_mushrooms[0].GetComponent<AudioSource>().Play();
	}

	private IEnumerator MushroomsCentralExplosionSounds(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < central_mushrooms.Length; i++)
		{
			central_mushrooms[i].GetComponent<small_mushroom>().Expl();
		}
		central_mushrooms[0].GetComponent<AudioSource>().Play();
	}

	private IEnumerator MushroomsLeftExplosionSounds(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < left_mushrooms.Length; i++)
		{
			left_mushrooms[i].GetComponent<small_mushroom>().Expl();
		}
		left_mushrooms[0].GetComponent<AudioSource>().Play();
	}

	private IEnumerator MushroomsRightExplosionSounds(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < right_mushrooms.Length; i++)
		{
			right_mushrooms[i].GetComponent<small_mushroom>().Expl();
		}
		right_mushrooms[0].GetComponent<AudioSource>().Play();
	}

	private IEnumerator TiredEnd()
	{
		yield return new WaitForSeconds(5f);
		BackToIdle();
	}

	private IEnumerator BackToIdleWithDelay()
	{
		yield return new WaitForSeconds(1f);
		BackToIdle();
	}

	private void BackToIdle()
	{
		state = "idle";
	}

	private void Tired()
	{
		state = "tired";
		stamina = 4;
		StopAllCoroutines();
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("boss_mushroom_rest_start");
		StartCoroutine("TiredEnd");
	}
}
