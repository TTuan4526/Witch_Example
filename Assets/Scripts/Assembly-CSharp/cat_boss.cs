using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cat_boss : MonoBehaviour
{
	public GameObject boss_area;

	public Slider boss_hp;

	public GameObject boss_name;

	public GameObject fireball;

	public Transform[] spawns;

	public Transform[] platforms;

	public GameObject nimbus;

	private AudioSource source;

	private Animator anim;

	private Rigidbody2D rbody;

	private int hp = 12;

	private int stamina = 3;

	private int damage_count = 4;

	private int rnd = -1;

	private int player_platform;

	private bool dead;

	private Data data;

	private string state = string.Empty;

	private Vector3 default_position;

	private Vector3 default_scale;

	private void Start()
	{
		anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
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

	private void Update()
	{
		if (!(state == "idle"))
		{
			return;
		}
		int num = rnd;
		while (rnd == num)
		{
			rnd = Random.Range(0, 4);
		}
		state = "attack";
		stamina--;
		if (rnd == 3)
		{
			rnd = 2;
		}
		switch (rnd)
		{
		case 0:
			StartCoroutine(SpawnFireball(0f, spawns[2]));
			StartCoroutine(SpawnFireball(0.5f, spawns[1]));
			StartCoroutine(SpawnFireball(0.5f, spawns[0]));
			StartCoroutine(SpawnFireball(1f, spawns[3]));
			StartCoroutine(SpawnFireball(1.5f, spawns[2]));
			StartCoroutine(SpawnFireball(2f, spawns[1]));
			StartCoroutine(SpawnFireball(2f, spawns[0]));
			StartCoroutine(SpawnFireball(2.5f, spawns[3]));
			StartCoroutine(SpawnFireball(3f, spawns[2]));
			StartCoroutine(SpawnFireball(3f, spawns[1]));
			StartCoroutine(SpawnFireball(3f, spawns[0]));
			StartCoroutine(SpawnFireball(3.5f, spawns[1]));
			StartCoroutine(SpawnFireball(3.5f, spawns[0]));
			StartCoroutine(SpawnFireball(4f, spawns[3]));
			StartCoroutine(SpawnFireball(4.5f, spawns[2]));
			StartCoroutine(SpawnFireball(4.5f, spawns[3]));
			StartCoroutine(SpawnFireball(5f, spawns[1]));
			StartCoroutine(SpawnFireball(5f, spawns[0]));
			StartCoroutine(SpawnFireball(5.5f, spawns[3]));
			StartCoroutine(SpawnFireball(6f, spawns[2]));
			StartCoroutine(SpawnFireball(6f, spawns[3]));
			StartCoroutine(SpawnFireball(6.5f, spawns[0]));
			StartCoroutine(SpawnFireball(6.5f, spawns[1]));
			StartCoroutine(BackToIdle(9f));
			break;
		case 1:
			StartCoroutine(SpawnFireball(0f, spawns[6]));
			StartCoroutine(SpawnFireball(0.5f, spawns[4]));
			StartCoroutine(SpawnFireball(0.5f, spawns[5]));
			StartCoroutine(SpawnFireball(1f, spawns[7]));
			StartCoroutine(SpawnFireball(2f, spawns[4]));
			StartCoroutine(SpawnFireball(2f, spawns[5]));
			StartCoroutine(SpawnFireball(1.5f, spawns[6]));
			StartCoroutine(SpawnFireball(2.5f, spawns[7]));
			StartCoroutine(SpawnFireball(3f, spawns[6]));
			StartCoroutine(SpawnFireball(4.5f, spawns[4]));
			StartCoroutine(SpawnFireball(4.5f, spawns[5]));
			StartCoroutine(SpawnFireball(4f, spawns[6]));
			StartCoroutine(SpawnFireball(3.5f, spawns[7]));
			StartCoroutine(SpawnFireball(5f, spawns[6]));
			StartCoroutine(SpawnFireball(5.5f, spawns[6]));
			StartCoroutine(SpawnFireball(6f, spawns[6]));
			StartCoroutine(SpawnFireball(6f, spawns[4]));
			StartCoroutine(SpawnFireball(6f, spawns[5]));
			StartCoroutine(BackToIdle(8.5f));
			break;
		case 2:
			switch (player_platform)
			{
			case 0:
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[2].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[3].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			case 1:
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[1].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[2].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[3].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			case 2:
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[1].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[2].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[0].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			case 3:
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[2].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[3].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			case 4:
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[2].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				platforms[3].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			case 5:
				platforms[0].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[1].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[3].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			case 6:
				platforms[0].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[1].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[2].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[4].GetComponent<destr_platform_special>().StartCoroutine("Fall", 0f);
				platforms[5].GetComponent<destr_platform_special>().StartCoroutine("Fall", 1f);
				StartCoroutine(BackToIdle(4f));
				break;
			}
			break;
		}
	}

	public void SetPlatform(int index)
	{
		player_platform = index;
	}

	private IEnumerator SpawnFireball(float delay, Transform spawner)
	{
		yield return new WaitForSeconds(delay);
		GameObject new_f = Object.Instantiate(fireball, spawner.position, base.transform.rotation);
		new_f.transform.localScale = spawner.localScale;
	}

	private IEnumerator BackToIdle(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (stamina <= 0)
		{
			state = "rest";
			stamina = 3;
			anim.Play("boss_cat_fire_fade");
			yield break;
		}
		if (state == "rest")
		{
			anim.Play("boss_cat_fire_out");
		}
		state = "idle";
	}

	public void CheckBossStatus()
	{
		if (GameObject.Find("boss_area").GetComponent<cat_boss_area>().on_what_story_phase <= data.story_progress)
		{
			anim.Play("boss_cat_defeated");
			GetComponent<CircleCollider2D>().enabled = false;
			nimbus.transform.localPosition = new Vector3(0f, 0f, -2f);
			nimbus.GetComponent<end_nimbus>().state = "end";
		}
	}

	public void Starter()
	{
		hp = 12;
		stamina = 3;
		damage_count = 0;
		rnd = -1;
		base.transform.localPosition = default_position;
		base.transform.localScale = default_scale;
		anim.Play("boss_cat_transform");
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
		if (dead || !(state == "rest"))
		{
			return;
		}
		damage_count++;
		if (damage_count > 2)
		{
			damage_count = 0;
			StartCoroutine(BackToIdle(0f));
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
			boss_hp.value = (float)hp / 12f;
			anim.Play("boss_cat_damage");
			source.Play();
		}
	}

	private void Dead()
	{
		dead = true;
		state = "dead";
		StopAllCoroutines();
		rbody.velocity = new Vector2(0f, 0f);
		GetComponent<CircleCollider2D>().enabled = false;
		Down();
	}

	private void Defeat()
	{
		GameObject.Find("boss_area").GetComponent<cat_boss_area>().BossDefeated();
	}

	private void Down()
	{
		rbody.velocity = new Vector2(0f, -0.8f);
		StartCoroutine("StopMovementDown");
	}

	private void Up()
	{
		rbody.velocity = new Vector2(0f, 0.8f);
		StartCoroutine("StopMovement");
	}

	private void BossDespawn()
	{
		if (!dead)
		{
			hp = 12;
			boss_hp.value = 1f;
			stamina = 3;
			damage_count = 0;
			rnd = -1;
			state = string.Empty;
			player_platform = 0;
			rbody.velocity = new Vector2(0f, 0f);
			StopAllCoroutines();
			base.transform.localPosition = default_position;
			base.transform.localScale = default_scale;
			boss_area.SendMessage("Respawn");
			anim.Play("boss_cat_idle");
			GameObject[] array = GameObject.FindGameObjectsWithTag("fireball");
			foreach (GameObject obj in array)
			{
				Object.Destroy(obj);
			}
			for (int j = 0; j < platforms.Length; j++)
			{
				platforms[j].GetComponent<destr_platform_special>().BackToIdle();
			}
		}
	}

	private IEnumerator StopMovement()
	{
		yield return new WaitForSeconds(0.85f);
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("boss_cat_fire_out");
		state = "idle";
	}

	private IEnumerator StopMovementDown()
	{
		yield return new WaitForSeconds(0.85f);
		rbody.velocity = new Vector2(0f, 0f);
		anim.Play("boss_cat_dead");
	}
}
