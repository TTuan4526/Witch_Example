using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool grounded;

	public AudioClip roll_clip;

	public AudioClip jump_1_clip;

	public AudioClip jump_2_clip;

	public AudioClip damage_clip;

	public AudioClip fireball_clip;

	public AudioClip frostbolt_clip;

	public AudioClip lightning_clip;

	public AudioClip fire_dash_clip;

	public AudioClip dash_clip;

	public AudioClip death_clip;

	public AudioClip melee_clip;

	public AudioClip melee_2_clip;

	public GameObject[] ui_health;

	public GameObject[] ui_mana;

	public GameObject hp_frame;

	public GameObject dust;

	public GameObject run_dust;

	public GameObject pentagram;

	public GameObject dash_effect;

	public GameObject cast_effect;

	public GameObject meteor_explosion;

	public GameObject Fireball;

	public GameObject Frostbolt;

	public GameObject Lightning;

	public GameObject firepoint;

	public GameObject this_room;

	public Transform attack_position;

	public Transform cast_effect_position;

	public int last_element;

	public int element;

	public int mana = 2;

	public int mana_max = 2;

	public int health = 2;

	public int health_max = 2;

	public int this_room_number;

	public bool paused;

	public bool blocked;

	public bool attack_blocked;

	public bool air_jump = true;

	public bool air_dash;

	public bool air_attack_first;

	public bool air_attack_second;

	public bool iframe;

	public bool second_jump;

	public bool prepare_for_action;

	public bool meteor_attack;

	public bool fire_dash;

	public bool roll;

	public bool new_roll;

	public bool dash;

	public bool fire_skill_1;

	public bool fire_skill_2;

	public bool frost_skill_1;

	public bool damaging;

	public bool lightning_skill_1;

	public bool lightning_skill_2;

	private float max_speed = 1.5f;

	private float jump_force = 24f;

	private float move;

	private bool fly;

	private bool facing_right = true;

	private bool melee_attack;

	private bool cast_attack;

	private bool stop_attack_first;

	private bool stop_attack_second;

	private bool run_blocking;

	private bool dead;

	private string current_control_scheme = "PC";

	private Vector3 temp_position;

	private RaycastHit2D hit;

	private Animator anim;

	private Animator elemental_anim;

	private Animator run_dust_anim;

	private Animator roll_dust_anim;

	private Rigidbody2D rbody;

	private AudioSource source;

	private WorldScript world_script;

	private Data data;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		current_control_scheme = PlayerPrefs.GetString("control_scheme");
		world_script = GameObject.FindGameObjectWithTag("common").GetComponent<WorldScript>();
		elemental_anim = GameObject.Find("ui_elements").GetComponent<Animator>();
		elemental_anim = GameObject.Find("ui_elements").GetComponent<Animator>();
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		anim = GetComponent<Animator>();
		run_dust_anim = run_dust.GetComponent<Animator>();
		rbody = base.gameObject.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (blocked || dead || damaging || fly || dash || roll || fire_dash)
		{
			return;
		}
		if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("ControllerA")) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)
			&& !((double)Input.GetAxis("Vertical") > 0.8) && ((!(current_control_scheme == "XBOX") &&
			!(current_control_scheme == "PC")) || !((double)Input.GetAxis("ControllerAxis7") < -0.8)) && (!(current_control_scheme == "PS") ||
			!((double)Input.GetAxis("ControllerAxis8") < -0.8)) && !dead && !paused && !meteor_attack && !prepare_for_action)
		{
			anim.ResetTrigger("end_dash");
			run_blocking = false;
			if (melee_attack)
			{
				melee_attack = false;
			}
			if (grounded)
			{
				MakeDusty();
				rbody.velocity = new Vector2(rbody.velocity.x, 0f);
				rbody.AddForce(new Vector2(0f, jump_force));
				second_jump = true;
				source.PlayOneShot(jump_1_clip);
			}
			else if (second_jump || (rbody.velocity.y < 0f && air_jump))
			{
				MakePentagram();
				anim.Play("witchy_jump");
				rbody.velocity = new Vector2(rbody.velocity.x, 0f);
				rbody.AddForce(new Vector2(0f, jump_force * 0.85f));
				second_jump = false;
				air_jump = false;
				source.PlayOneShot(jump_2_clip);
			}
		}
		else if ((Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("ControllerX")) && !attack_blocked && last_element > 0 && !prepare_for_action && mana > 0)
		{
			if (grounded)
			{
				rbody.velocity = new Vector2(0f, 0f);
				StopCoroutine("AttackDelay");
				melee_attack = false;
				anim.ResetTrigger("no_attack");
				StopCoroutine("CastDelay");
				StartCoroutine("CastDelay");
				StopRollNow();
				if (anim.GetBool("run") && !cast_attack)
				{
					anim.Play("witchy_cast_fire_1");
				}
				else
				{
					anim.SetTrigger("attack_cast");
				}
				cast_attack = true;
				run_blocking = true;
			}
			else if (!second_jump && !stop_attack_first)
			{
				anim.Play("witchy_air_cast");
				stop_attack_first = true;
			}
			else if (second_jump && !stop_attack_second)
			{
				anim.Play("witchy_air_cast");
				stop_attack_second = true;
			}
		}
		else if ((Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("ControllerRightTrigger")) && !attack_blocked && fire_skill_2 && last_element > 0 && !prepare_for_action && mana > 0)
		{
			if (grounded)
			{
				if (!fire_dash && fire_skill_2)
				{
					StopCoroutine("AttackDelay");
					anim.ResetTrigger("no_attack");
					StopCoroutine("CastDelay");
					anim.ResetTrigger("no_cast");
					source.PlayOneShot(fire_dash_clip);
					DrainMana(1);
					fire_dash = true;
					iframe = true;
					rbody.velocity = new Vector2(4f, 0f) * base.transform.localScale.x;
					anim.Play("witchy_fire_dash");
					StartCoroutine("InvincibleFrames");
				}
			}
			else if (!meteor_attack && mana >= 2 && lightning_skill_2)
			{
				DrainMana(2);
				meteor_attack = true;
				rbody.gravityScale = 0f;
				anim.Play("witchy_meteor_start");
			}
		}
		else if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("ControllerY")) && ((!grounded && !dash && !air_dash) || (grounded && !new_roll && !fire_dash)) && base.transform.parent == null && !run_blocking)
		{
			if (grounded)
			{
				StopCoroutine("AttackDelay");
				anim.ResetTrigger("no_attack");
				StopCoroutine("CastDelay");
				anim.ResetTrigger("no_cast");
				roll = true;
				new_roll = true;
				iframe = true;
				source.PlayOneShot(roll_clip, 0.5f);
				rbody.velocity = new Vector2(3f, 0f) * base.transform.localScale.x;
				anim.Play("witchy_roll");
				MakeDustyRoll();
				StartCoroutine("InvincibleFrames");
			}
			else
			{
				rbody.gravityScale = 0f;
				dash = true;
				iframe = true;
				source.PlayOneShot(dash_clip, 0.35f);
				air_dash = true;
				rbody.velocity = new Vector2(4.5f, 0f) * base.transform.localScale.x;
				anim.Play("witchy_dash");
				MakeDash();
				StartCoroutine("InvincibleFrames");
			}
		}
		else if ((Input.GetKeyDown(KeyCode.V) || Input.GetButtonDown("ControllerLeftTrigger")) && !attack_blocked && element != 0)
		{
			if (element + 1 <= last_element)
			{
				element++;
			}
			else
			{
				element = 1;
			}
			elemental_anim.SetTrigger("change_to_" + element);
		}
		else
		{
			if ((!Input.GetKeyDown(KeyCode.Z) && !Input.GetButtonDown("ControllerB")) || prepare_for_action)
			{
				return;
			}
			if (grounded)
			{
				rbody.velocity = new Vector2(0f, 0f);
				StopCoroutine("AttackDelay");
				StartCoroutine("AttackDelay");
				StopCoroutine("ResetAttack");
				StartCoroutine("ResetAttack");
				StopRollNow();
				if (anim.GetBool("run") && !melee_attack)
				{
					anim.Play("witchy_melee_attack_1");
				}
				else
				{
					anim.SetTrigger("attack_melee");
				}
				melee_attack = true;
				run_blocking = true;
			}
			else if (!air_attack_first && !air_attack_second)
			{
				anim.Play("witchy_air_melee");
				air_attack_first = true;
			}
			else if (air_attack_first && !air_attack_second)
			{
				anim.Play("witchy_air_melee_2");
				air_attack_second = true;
			}
		}
	}

	private void FixedUpdate()
	{
		if (dead || paused || damaging || fly || dash || roll || fire_dash || run_blocking || meteor_attack)
		{
			return;
		}
		if (!blocked)
		{
			if (Input.GetAxisRaw("Horizontal") != 0f)
			{
				move = Input.GetAxisRaw("Horizontal");
			}
			else if (current_control_scheme == "XBOX" || current_control_scheme == "PC")
			{
				move = Input.GetAxisRaw("ControllerAxis6");
			}
			else if (current_control_scheme == "PS")
			{	
				move = Input.GetAxisRaw("ControllerAxis7");
			}
		}
		else
		{
			move = 0f;
		}
		if (move != 0f)
		{
			anim.SetBool("run", true);
			run_dust_anim.SetBool("run", true);
			if (melee_attack)
			{
				anim.Play("witchy_run");
				StopCoroutine("AttackDelay");
				melee_attack = false;
			}
			if (cast_attack)
			{
				anim.Play("witchy_run");
				cast_attack = false;
				anim.SetBool("attack_cast", false);
			}
		}
		else
		{
			anim.SetBool("run", false);
			run_dust_anim.SetBool("run", false);
		}
		if (base.transform.parent != null && base.transform.parent.GetComponent<moving_platform>().horizontal && ((move > 0f && base.transform.parent.GetComponent<moving_platform>().going_next) || (move < 0f && !base.transform.parent.GetComponent<moving_platform>().going_next)))
		{
			move *= 0.5f;
		}
		rbody.velocity = new Vector2(move * max_speed, rbody.velocity.y);
		if ((move > 0f && !facing_right) || (move < 0f && facing_right))
		{
			Flip();
		}
		if (!grounded)
		{
			anim.SetFloat("vspeed", rbody.velocity.y);
			return;
		}
		if (stop_attack_first)
		{
			stop_attack_first = false;
		}
		if (stop_attack_second)
		{
			stop_attack_second = false;
		}
	}

	public void SetMaxHP(int amount)
	{
		hp_frame.GetComponent<Animator>().Play("ui_hp_frame_" + amount);
		health_max = amount;
		health = health_max;
		for (int i = 0; i <= 6; i++)
		{
			if (i >= amount)
			{
				ui_health[i].SetActive(false);
			}
			else
			{
				ui_health[i].SetActive(true);
			}
		}
		for (int j = 0; j <= 6; j++)
		{
			if (ui_health[j].activeSelf)
			{
				if (j <= health - 1)
				{
					ui_health[j].GetComponent<Animator>().Play("hp_full");
				}
				else
				{
					ui_health[j].GetComponent<Animator>().Play("hp_empty");
				}
			}
		}
	}

	public void SetMaxMana(int amount)
	{
		mana_max = amount;
		mana = mana_max;
		for (int i = 0; i <= 3; i++)
		{
			if (i >= amount)
			{
				ui_mana[i].SetActive(false);
			}
			else
			{
				ui_mana[i].SetActive(true);
			}
		}
		for (int j = 0; j <= 3; j++)
		{
			if (ui_mana[j].activeSelf)
			{
				if (j <= mana - 1)
				{
					ui_mana[j].GetComponent<Animator>().Play("ui_mana_bar_" + (j + 1) + "_full");
				}
				else
				{
					ui_mana[j].GetComponent<Animator>().Play("ui_mana_bar_" + (j + 1) + "_empty");
				}
			}
		}
	}

	public void SetMaxElement(bool fire_1, bool fire_2, bool lightning_1, bool lightning_2, bool frost_1)
	{
		fire_skill_1 = fire_1;
		fire_skill_2 = fire_2;
		lightning_skill_1 = lightning_1;
		lightning_skill_2 = lightning_2;
		frost_skill_1 = frost_1;
		if (frost_1)
		{
			last_element = 3;
			element = 1;
			GameObject.Find("ui_elements").GetComponent<Animator>().SetTrigger("change_to_1");
		}
		else if (lightning_1)
		{
			last_element = 2;
			element = 1;
			GameObject.Find("ui_elements").GetComponent<Animator>().SetTrigger("change_to_1");
		}
		else if (fire_1)
		{
			last_element = 1;
			element = 1;
			GameObject.Find("ui_elements").GetComponent<Animator>().SetTrigger("change_to_1");
		}
		else
		{
			last_element = 0;
			element = 0;
			GameObject.Find("ui_elements").GetComponent<Animator>().Play("elements_none");
		}
	}

	private void EndRunBlocking()
	{
		run_blocking = false;
	}

	public void Block()
	{
		blocked = true;
	}

	public void Unblock()
	{
		blocked = false;
	}

	public void GetNewElement(int new_element)
	{
		last_element = new_element;
		element = new_element;
		switch (new_element)
		{
		case 1:
			Block();
			anim.Play("witchy_get_fire");
			break;
		case 2:
			Block();
			anim.Play("witchy_get_thunder");
			break;
		case 3:
			Block();
			anim.Play("witchy_get_ice");
			break;
		}
		elemental_anim.SetTrigger("change_to_" + new_element);
		PlayerPrefs.SetInt("last_element", new_element);
	}

	private void AfterGettingElement()
	{
		Unblock();
	}

	public void StartSinking()
	{
		blocked = true;
		anim.Play("witchy_drawn_1");
		rbody.velocity = new Vector2(0f, 0f);
		rbody.gravityScale = 0.05f;
	}

	private void CastSpell()
	{
		if (element == 1 && fire_skill_1)
		{
			DrainMana(1);
			GameObject gameObject = Object.Instantiate(Fireball, firepoint.transform.position, base.transform.rotation);
			source.PlayOneShot(fireball_clip);
			if (base.transform.localScale.x < 0f)
			{
				gameObject.transform.localScale = base.transform.localScale;
				gameObject.GetComponent<Rigidbody2D>().velocity = base.transform.right * -4.5f;
			}
			else
			{
				gameObject.GetComponent<Rigidbody2D>().velocity = base.transform.right * 4.5f;
			}
		}
		else if (element == 2 && lightning_skill_1)
		{
			DrainMana(1);
			GameObject gameObject2 = Object.Instantiate(Lightning, firepoint.transform.position, base.transform.rotation);
			source.PlayOneShot(lightning_clip, 0.5f);
			if (base.transform.localScale.x < 0f)
			{
				gameObject2.transform.localScale = base.transform.localScale;
				gameObject2.GetComponent<Rigidbody2D>().velocity = base.transform.right * -5f;
			}
			else
			{
				gameObject2.GetComponent<Rigidbody2D>().velocity = base.transform.right * 5f;
			}
		}
		else if (element == 3 && frost_skill_1)
		{
			DrainMana(1);
			GameObject gameObject3 = Object.Instantiate(Frostbolt, firepoint.transform.position, base.transform.rotation);
			source.PlayOneShot(frostbolt_clip);
			if (base.transform.localScale.x < 0f)
			{
				gameObject3.transform.localScale = base.transform.localScale;
				gameObject3.GetComponent<Rigidbody2D>().velocity = base.transform.right * -6f;
			}
			else
			{
				gameObject3.GetComponent<Rigidbody2D>().velocity = base.transform.right * 6f;
			}
		}
	}

	private void ShowCastEffect()
	{
		GameObject gameObject = Object.Instantiate(cast_effect, cast_effect_position.position, base.transform.rotation);
		gameObject.transform.localScale = base.transform.localScale;
		if (element == 1 && fire_skill_1)
		{
			gameObject.GetComponent<Animator>().Play("cast_effect_fire");
		}
		else if (element == 2 && lightning_skill_1)
		{
			gameObject.GetComponent<Animator>().Play("cast_effect_light");
		}
		else if (element == 3 && frost_skill_1)
		{
			gameObject.GetComponent<Animator>().Play("cast_effect_frost");
		}
	}

	public void PrepareForActionOn()
	{
		prepare_for_action = true;
	}

	public void PrepareForActionOff()
	{
		prepare_for_action = false;
	}

	private void AttackMelee()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(attack_position.position, 0.35f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "enemy" || array[i].gameObject.tag == "turnip" || array[i].gameObject.tag == "boss")
			{
				if (array[i].transform.position.x < base.transform.position.x)
				{
					array[i].gameObject.SendMessage("DamageFromRight", 1);
				}
				else if (array[i].transform.position.x > base.transform.position.x)
				{
					array[i].gameObject.SendMessage("DamageFromLeft", 1);
				}
				else
				{
					array[i].gameObject.SendMessage("DamageFromLeft", 1);
				}
			}
			else if (array[i].gameObject.tag == "chest")
			{
				array[i].gameObject.GetComponent<chest>().Open("none");
			}
			else if (array[i].gameObject.tag == "crystal")
			{
				array[i].gameObject.GetComponent<farm_ccrystal>().HitMeBabyOneMoreTime();
			}
		}
	}

	public void JumpAtNimbus()
	{
		anim.Play("witchy_fly");
		fly = true;
		rbody.velocity = new Vector3(0f, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		StartCoroutine("FlyAway");
	}

	public void JumpOffNimbus()
	{
		anim.Play("witchy_jump");
		fly = false;
		rbody.AddForce(new Vector2(0f, 0.4f), ForceMode2D.Impulse);
		source.PlayOneShot(jump_1_clip);
	}

	public void StopFly()
	{
		fly = true;
		rbody.velocity = new Vector3(0f, 0f);
	}

	public void StopMoving()
	{
		rbody.velocity = new Vector2(0f, 0f);
		paused = true;
	}

	private void MakePentagram()
	{
		pentagram.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.425f, base.transform.position.z);
		pentagram.GetComponent<Animator>().Play("pentagram_jump");
	}

	private void MakeDash()
	{
		dash_effect.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z + 0.1f);
		dash_effect.GetComponent<Animator>().Play("dash_effect_dash");
	}

	private void MakeDusty()
	{
		dust.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.09f, base.transform.position.z);
		dust.GetComponent<Animator>().Play("dust_jump");
	}

	public void MakeDustyRoll()
	{
		dust.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.17f, base.transform.position.z);
		dust.transform.localScale = base.transform.localScale;
		dust.GetComponent<Animator>().Play("dust_land");
	}

	public void Damage(int amount)
	{
		if (dead || damaging || iframe)
		{
			return;
		}
		cast_attack = false;
		run_blocking = false;
		int old_hp = health;
		health -= amount;
		if (health < 0)
		{
			health = 0;
		}
		source.PlayOneShot(damage_clip);
		UpdateHealthUI(old_hp);
		if (health <= 0)
		{
			Dead();
			return;
		}
		anim.Play("witchy_damaged");
		anim.SetBool("run", false);
		anim.SetBool("end_roll", false);
		anim.SetBool("no_attack", false);
		anim.SetBool("fire", false);
		anim.SetBool("attack_melee", false);
		anim.SetBool("attack_cast", false);
		StopAllCoroutines();
		roll = false;
		new_roll = false;
		damaging = true;
		iframe = true;
		dash = false;
		air_jump = true;
		second_jump = false;
		melee_attack = false;
		cast_attack = false;
		stop_attack_first = false;
		fire_dash = false;
		rbody.velocity = new Vector2(0f, 0f);
		if (base.transform.localScale.x == -1f)
		{
			rbody.AddForce(new Vector2(0.15f, 0f), ForceMode2D.Impulse);
		}
		else
		{
			rbody.AddForce(new Vector2(-0.15f, 0f), ForceMode2D.Impulse);
		}
		StartCoroutine("StopDamaging");
	}

	public void DamageAnyway(int amount)
	{
		if (dead || damaging)
		{
			return;
		}
		cast_attack = false;
		run_blocking = false;
		int old_hp = health;
		health -= amount;
		source.PlayOneShot(damage_clip);
		UpdateHealthUI(old_hp);
		if (health <= 0)
		{
			Dead();
			return;
		}
		anim.Play("witchy_damaged");
		StopAllCoroutines();
		roll = false;
		new_roll = false;
		damaging = true;
		iframe = false;
		dash = false;
		rbody.velocity = new Vector2(0f, 0f);
		if (base.transform.localScale.x == -1f)
		{
			rbody.AddForce(new Vector2(0.15f, 0f), ForceMode2D.Impulse);
		}
		else
		{
			rbody.AddForce(new Vector2(-0.15f, 0f), ForceMode2D.Impulse);
		}
		StartCoroutine("StopDamaging");
	}

	public void FallDrawnDamage(Vector3 respawn_position, string type)
	{
		int old_hp = health;
		health--;
		source.PlayOneShot(death_clip);
		UpdateHealthUI(old_hp);
		rbody.gravityScale = 0f;
		GetComponent<Animator>().Play("witchy_death_air");
		if (type == "drawn")
		{
			StartCoroutine("RespawnAfterDrawning", respawn_position);
		}
		else
		{
			StartCoroutine("RespawnAfterFalling", respawn_position);
		}
	}

	public void Heal(int amount)
	{
		int old_hp = health;
		health += amount;
		if (health > health_max)
		{
			health = health_max;
		}
		UpdateHealthUI(old_hp);
	}

	public void RestoreMana(int amount)
	{
		int old_mana = mana;
		mana += amount;
		if (mana > mana_max)
		{
			mana = mana_max;
		}
		UpdateManaUI(old_mana);
	}

	public void DrainMana(int amount)
	{
		int old_mana = mana;
		mana -= amount;
		if (mana > mana_max)
		{
			mana = mana_max;
		}
		UpdateManaUI(old_mana);
	}

	public void Landing()
	{
		if (meteor_attack)
		{
			hit = Physics2D.Raycast(base.transform.position, Vector2.down, 1000f, LayerMask.GetMask("not_walk", "platform"));
			if ((bool)hit && hit.transform.tag == "ground")
			{
				Object.Instantiate(meteor_explosion, new Vector3(hit.point.x, hit.point.y, base.transform.position.z - 0.5f), base.transform.rotation);
			}
			else
			{
				Object.Instantiate(meteor_explosion, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.5f), base.transform.rotation);
			}
			anim.Play("witchy_meteor_explosion");
			StartCoroutine("JumpAfterMeteor");
		}
	}

	private void UpdateHealthUI(int old_hp)
	{
		int num = health;
		if (old_hp > num)
		{
			for (int i = num + 1; i <= old_hp; i++)
			{
				if (ui_health[i - 1].activeSelf)
				{
					ui_health[i - 1].GetComponent<Animator>().Play("hp_minus");
				}
			}
		}
		else
		{
			if (old_hp >= num)
			{
				return;
			}
			for (int j = old_hp + 1; j <= num; j++)
			{
				if (ui_health[j - 1].activeSelf)
				{
					ui_health[j - 1].GetComponent<Animator>().Play("hp_plus");
				}
			}
		}
	}

	private void UpdateManaUI(int old_mana)
	{
		if (old_mana > mana)
		{
			for (int i = mana + 1; i <= old_mana; i++)
			{
				if (ui_mana[i - 1].activeSelf)
				{
					ui_mana[i - 1].GetComponent<Animator>().Play("ui_mana_bar_" + i + "_drain");
				}
			}
			if (mana >= mana_max - 1)
			{
				return;
			}
			for (int j = mana + 2; j <= mana_max; j++)
			{
				if (ui_mana[j - 1].activeSelf)
				{
					ui_mana[j - 1].GetComponent<Animator>().Play("ui_mana_bar_" + j + "_zero");
				}
			}
		}
		else
		{
			if (old_mana >= mana)
			{
				return;
			}
			for (int k = old_mana + 1; k <= mana; k++)
			{
				if (ui_mana[k - 1].activeSelf)
				{
					ui_mana[k - 1].GetComponent<Animator>().Play("ui_mana_bar_" + k + "_restore");
				}
			}
		}
	}

	private void PushMeteor()
	{
		rbody.AddForce(Vector2.down * 1.2f, ForceMode2D.Impulse);
	}

	private void Flip()
	{
		facing_right = !facing_right;
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
		run_dust.transform.localScale = base.transform.localScale;
	}

	private void Dead()
	{
		StopAllCoroutines();
		anim.SetBool("run", false);
		anim.SetBool("end_roll", false);
		anim.SetBool("no_attack", false);
		anim.SetBool("fire", false);
		anim.SetBool("attack_melee", false);
		anim.SetBool("attack_cast", false);
		roll = false;
		new_roll = false;
		damaging = false;
		iframe = false;
		dash = false;
		air_jump = true;
		second_jump = false;
		melee_attack = false;
		cast_attack = false;
		stop_attack_first = false;
		fire_dash = false;
		source.PlayOneShot(death_clip);
		anim.Play("witchy_death_air");
		RestartAfterDeath(1.75f);
	}

	public void RestartAfterDeath(float delay)
	{
		dead = true;
		rbody.velocity = new Vector2(0f, 0f);
		rbody.gravityScale = 0f;
		meteor_attack = false;
		fire_dash = false;
		base.transform.GetChild(0).gameObject.SetActive(false);
		Block();
		StartCoroutine("ResetStage", delay);
	}

	private void StopRollNow()
	{
		StopCoroutine("StopRoll");
		anim.ResetTrigger("end_roll");
		roll = false;
		rbody.gravityScale = 1f;
		StartCoroutine("MakeAnotherRoll");
	}

	private void PlayMeleeSound()
	{
		source.PlayOneShot(melee_clip, 0.25f);
	}

	private void PlaySecondMeleeSound()
	{
		source.PlayOneShot(melee_2_clip, 0.25f);
	}

	private void SetGravityToDefault()
	{
		rbody.gravityScale = 1f;
	}

	private void StopRollo()
	{
		anim.SetTrigger("end_roll");
		roll = false;
		StartCoroutine("MakeAnotherRoll");
		rbody.gravityScale = 1f;
	}

	private void StopFireDash()
	{
		anim.SetTrigger("end_fire_dash");
		fire_dash = false;
		rbody.gravityScale = 1f;
	}

	private void StopDash()
	{
		anim.SetTrigger("end_dash");
		dash = false;
		rbody.gravityScale = 1f;
	}

	public void RestoreParameters()
	{
		attack_blocked = false;
		air_jump = true;
		air_dash = false;
		air_attack_first = false;
		air_attack_second = false;
		iframe = false;
		second_jump = false;
		prepare_for_action = false;
		meteor_attack = false;
		fire_dash = false;
		roll = false;
		new_roll = false;
		dash = false;
		damaging = false;
		melee_attack = false;
		cast_attack = false;
		stop_attack_first = false;
		stop_attack_second = false;
		run_blocking = false;
		dead = false;
	}

	private IEnumerator RespawnAfterDrawning(Vector3 respawn_position)
	{
		yield return new WaitForSeconds(1f);
		GameObject.FindGameObjectWithTag("common").GetComponent<AudioSource>().PlayOneShot(world_script.respawn_sound);
		GetComponent<Animator>().Play("witchy_water_respawn");
		base.transform.position = new Vector3(respawn_position.x, respawn_position.y, -3f);
	}

	private IEnumerator JumpAfterMeteor()
	{
		yield return new WaitForSeconds(0.1f);
		meteor_attack = false;
		rbody.gravityScale = 1f;
		anim.Play("witchy_jump");
		rbody.velocity = new Vector2(rbody.velocity.x, 0f);
		rbody.AddForce(new Vector2(0f, jump_force));
		second_jump = true;
		air_jump = false;
		source.PlayOneShot(jump_1_clip);
	}

	private IEnumerator RespawnAfterFalling(Vector3 respawn_position)
	{
		yield return new WaitForSeconds(1f);
		GetComponent<Animator>().Play("witchy_respawn");
		base.transform.position = new Vector3(respawn_position.x, respawn_position.y, -3f);
	}

	private IEnumerator MakeAnotherRoll()
	{
		yield return new WaitForSeconds(0.5f);
		new_roll = false;
	}

	private IEnumerator CastDelay()
	{
		yield return new WaitForSeconds(0.65f);
		if (cast_attack)
		{
			cast_attack = false;
			run_blocking = false;
			anim.SetTrigger("no_cast");
		}
	}

	private IEnumerator AttackDelay()
	{
		yield return new WaitForSeconds(0.75f);
		if (melee_attack)
		{
			melee_attack = false;
			run_blocking = false;
			anim.SetTrigger("no_attack");
		}
	}

	private IEnumerator StopDamaging()
	{
		yield return new WaitForSeconds(0.2f);
		rbody.velocity = new Vector2(0f, 0f);
		rbody.gravityScale = 1f;
		air_attack_first = false;
		air_attack_second = false;
		damaging = false;
		StartCoroutine("InvincibleFrames");
	}

	private IEnumerator InvincibleFrames()
	{
		yield return new WaitForSeconds(0.35f);
		iframe = false;
	}

	private IEnumerator ResetAttack()
	{
		yield return new WaitForSeconds(0.5f);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("witchy_melee_attack_1") || anim.GetCurrentAnimatorStateInfo(0).IsName("witchy_air_melee"))
		{
			melee_attack = false;
			anim.Play("witchy_idle");
		}
	}

	private IEnumerator ResetStage(float delay)
	{
		yield return new WaitForSeconds(delay);
		world_script.LoadSave();
		this_room.GetComponent<this_room>().DespawnThemAll();
		GameObject[] array = GameObject.FindGameObjectsWithTag("respawn_items");
		foreach (GameObject gameObject in array)
		{
			foreach (Transform item in gameObject.transform)
			{
				if (item.gameObject.activeSelf)
				{
					item.SendMessage("ResetItemState");
				}
			}
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("precious");
		foreach (GameObject obj in array2)
		{
			Object.Destroy(obj);
		}
		SetMaxHP(data.hp);
		SetMaxMana(data.mana);
		SetMaxElement(data.fire_skill_1, data.fire_skill_2, data.lightning_skill_1, data.lightning_skill_2, data.frost_skill_1);
		world_script.SpawnHero();
		rbody.gravityScale = 1f;
		dead = false;
		base.transform.GetChild(0).gameObject.SetActive(true);
		Block();
		anim.Play("witchy_respawn");
	}
}
