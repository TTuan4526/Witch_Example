using System.Collections;
using UnityEngine;

public class boss_goblins : MonoBehaviour
{
	public Transform[] top_places;

	public Transform[] down_places;

	public goblins_boss_area boss_area;

	public GameObject circle;

	public AudioClip death_sound;

	public bool frost;

	public string state = string.Empty;

	private bool dead;

	private bool damaging;

	private bool attacking;

	private bool down = true;

	public int hp;

	public int damage_count;

	public int spell_count;

	private float fire_rate;

	public Vector3 default_position;

	private AudioSource source;

	private Rigidbody2D rbody;

	private Animator anim;

	private Data data;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		rbody = GetComponent<Rigidbody2D>();
		default_position = base.transform.localPosition;
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	private void Update()
	{
		if (state == "fight" && !dead)
		{
			if (fire_rate > 0f)
			{
				fire_rate -= Time.deltaTime;
			}
			if (fire_rate <= 0f && !attacking && !damaging)
			{
				Attack();
			}
		}
	}

	public void DamageFromLeft(int amount)
	{
		Damage(amount);
	}

	public void DamageFromRight(int amount)
	{
		Damage(amount);
	}

	private void Damage(int amount)
	{
		if (dead || !(state != "teleport") || !(state != "spell"))
		{
			return;
		}
		hp -= amount;
		damage_count++;
		spell_count++;
		if (spell_count % 4 == 0)
		{
			state = "spell";
		}
		else if (damage_count >= 2)
		{
			state = "teleport";
		}
		if (attacking)
		{
			fire_rate = 2f;
			attacking = false;
		}
		if (hp <= 0)
		{
			Dead();
			if (frost)
			{
				boss_area.UpdateHP(true, hp);
			}
			else
			{
				boss_area.UpdateHP(false, hp);
			}
			return;
		}
		source.Play();
		if (frost)
		{
			boss_area.UpdateHP(true, hp);
			anim.Play("goblin_mage_frost_damage");
		}
		else
		{
			boss_area.UpdateHP(false, hp);
			anim.Play("goblin_mage_fire_damaged");
		}
		damaging = true;
		rbody.velocity = new Vector2(0f, 0f);
	}

	private void BossDespawn()
	{
		hp = 12;
		dead = false;
		GetComponent<BoxCollider2D>().enabled = true;
		damage_count = 0;
		spell_count = 0;
		state = string.Empty;
		attacking = false;
		damaging = false;
		down = true;
		rbody.velocity = new Vector2(0f, 0f);
		StopAllCoroutines();
		base.transform.localPosition = default_position;
		boss_area.Respawn();
		GameObject[] array = GameObject.FindGameObjectsWithTag("gbl");
		foreach (GameObject obj in array)
		{
			Object.Destroy(obj);
		}
		if (frost)
		{
			anim.Play("goblin_mage_frost_idle");
		}
		else
		{
			anim.Play("golbin_mage_fire_idle");
		}
	}

	private void Dead()
	{
		dead = true;
		rbody.velocity = new Vector2(0f, 0f);
		try
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreMana(1);
		}
		catch
		{
		}
		GetComponent<AudioSource>().Play();
		if (frost)
		{
			anim.Play("goblin_mage_frost_dead");
		}
		else
		{
			anim.Play("goblin_mage_fire_dead");
		}
		source.PlayOneShot(death_sound);
		GetComponent<BoxCollider2D>().enabled = false;
	}

	private void Attack()
	{
		attacking = true;
		rbody.velocity = new Vector2(0f, 0f);
		if (frost)
		{
			anim.Play("goblin_mage_frost_cast_start");
		}
		else
		{
			anim.Play("goblin_mage_fire_cast_start");
		}
		Object.Instantiate(circle, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z + 0.1f), base.transform.rotation);
		StartCoroutine("AttackDelay");
	}

	private void Teleport()
	{
		if (!dead)
		{
			if (frost)
			{
				anim.Play("goblin_mage_frost_teleport");
			}
			else
			{
				anim.Play("goblin_mage_fire_teleport");
			}
		}
	}

	public void BackAfterSpell()
	{
		if (!dead)
		{
			state = "teleport";
			fire_rate = 0.5f;
			attacking = false;
			damaging = false;
			Teleport();
		}
	}

	private void Transition()
	{
		if (state == "spell")
		{
			base.transform.position = new Vector3(top_places[top_places.Length - 1].position.x, top_places[top_places.Length - 1].position.y, base.transform.position.z);
			down = true;
		}
		else if (down)
		{
			int num = Random.Range(0, top_places.Length - 1);
			base.transform.position = new Vector3(top_places[num].position.x, top_places[num].position.y, base.transform.position.z);
			down = false;
		}
		else
		{
			int num2 = Random.Range(0, down_places.Length);
			base.transform.position = new Vector3(down_places[num2].position.x, down_places[num2].position.y, base.transform.position.z);
			down = true;
		}
		if (frost)
		{
			anim.Play("goblin_mage_frost_appear");
		}
		else
		{
			anim.Play("goblin_mage_fire_appear");
		}
	}

	private IEnumerator AttackDelay()
	{
		yield return new WaitForSeconds(2.5f);
		BackToNormal();
	}

	private void AfterTeleport()
	{
		if (state == "spell")
		{
			if (frost)
			{
				anim.Play("goblin_mage_frost_cast_start");
			}
			else
			{
				anim.Play("goblin_mage_fire_cast_start");
			}
			StartCoroutine(DropSpikes());
		}
		else
		{
			state = "fight";
			fire_rate = 0.5f;
		}
	}

	private void BackToNormal()
	{
		if (attacking)
		{
			if (frost)
			{
				anim.Play("goblin_mage_frost_cast_end");
			}
			else
			{
				anim.Play("goblin_mage_fire_cast_end");
			}
			fire_rate = 2f;
			attacking = false;
		}
	}

	private IEnumerator DropSpikes()
	{
		boss_area.SpikesAppear();
		yield return new WaitForSeconds(0.75f);
		boss_area.SpikesDrop();
		boss_area.GetComponent<goblins_boss_area>().FallToDefault();
	}

	private void StopDamaging()
	{
		damaging = false;
		if (spell_count % 4 == 0)
		{
			damage_count = 0;
			spell_count = 0;
			CommonSpell();
		}
		else if (damage_count >= 2)
		{
			damage_count = 0;
			Teleport();
		}
	}

	private void CommonSpell()
	{
		if (!dead)
		{
			boss_area.CastCommonSpell();
		}
	}

	public void CastSpell()
	{
		if (!dead)
		{
			state = "spell";
			StopAllCoroutines();
			Teleport();
		}
	}

	public void CheckBossStatus()
	{
		if (GameObject.Find("boss_area").GetComponent<goblins_boss_area>().on_what_story_phase <= data.story_progress)
		{
			if (frost)
			{
				anim.Play("goblin_mage_frost_defeated");
			}
			else
			{
				anim.Play("goblin_mage_fire_defeated");
			}
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void Starter()
	{
		fire_rate = 0.5f;
		hp = 12;
		base.transform.localPosition = default_position;
		state = "teleport";
		if (frost)
		{
			anim.Play("goblin_mage_frost_idle");
		}
		else
		{
			anim.Play("golbin_mage_fire_idle");
		}
		Teleport();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!dead && col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerController>().fire_dash)
		{
			Damage(2);
		}
	}
}
