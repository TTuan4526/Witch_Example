using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerHome : MonoBehaviour
{
	public bool grounded;

	public bool prepare_for_action;

	public bool air_jump = true;

	public GameObject dust;

	public GameObject pentagram;

	public bool paused;

	public bool blocked;

	private float max_speed = 1.5f;

	private float jump_force = 24f;

	private float move;

	private bool facing_right = true;

	private bool second_jump;

	private Animator anim;

	private Rigidbody2D rbody;

	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		rbody = base.gameObject.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (!blocked && (Input.GetButtonDown("Jump") || Input.GetButtonDown("ControllerA")) && !Input.GetKey(KeyCode.DownArrow)
			&& !Input.GetKey(KeyCode.S) && !((double)Input.GetAxis("Vertical") < -0.8) && !paused)
		{
			if (grounded)
			{
				MakeDusty();
				rbody.velocity = new Vector2(rbody.velocity.x, 0f);
				rbody.AddForce(new Vector2(0f, jump_force));
				second_jump = true;
				source.Play();
			}
			else if (second_jump || (rbody.velocity.y < 0f && air_jump))
			{
				MakePentagram();
				anim.Play("witchy_jump");
				rbody.velocity = new Vector2(rbody.velocity.x, 0f);
				rbody.AddForce(new Vector2(0f, jump_force));
				second_jump = false;
				air_jump = false;
				source.Play();
			}
		}
	}

	public void Unblock()
	{
		blocked = !blocked;
	}

	public void StopMoving()
	{
		rbody.velocity = new Vector2(0f, 0f);
		paused = true;
	}

	public void PrepareForActionOn()
	{
		prepare_for_action = !prepare_for_action;
	}

	public void PrepareForActionOff()
	{
		prepare_for_action = !prepare_for_action;
	}

	public void RestartAfterDeath(float delay)
	{
		rbody.velocity = new Vector2(0f, 0f);
		rbody.gravityScale = 0f;
		base.transform.GetChild(0).gameObject.SetActive(false);
		Unblock();
		StartCoroutine("ResetStage", delay);
	}

	private void FixedUpdate()
	{
		if (!paused)
		{
			if (!blocked)
			{
				move = Input.GetAxisRaw("Horizontal");
			}
			else
			{
				move = 0f;
			}
			if (move != 0f)
			{
				anim.SetBool("run", true);
			}
			else
			{
				anim.SetBool("run", false);
			}
			rbody.velocity = new Vector2(move * max_speed, rbody.velocity.y);
			if ((move > 0f && !facing_right) || (move < 0f && facing_right))
			{
				Flip();
			}
			if (!grounded)
			{
				anim.SetFloat("vspeed", rbody.velocity.y);
			}
		}
	}

	private void MakePentagram()
	{
		pentagram.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.425f, base.transform.position.z);
		pentagram.GetComponent<Animator>().Play("pentagram_jump");
	}

	private void WakeUp()
	{
		GameObject.FindGameObjectWithTag("common").GetComponent<WorldScriptHome>().WakeUp();
	}

	private void MakeDusty()
	{
		dust.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.09f, base.transform.position.z);
		dust.GetComponent<Animator>().Play("dust_jump");
	}

	private void Flip()
	{
		facing_right = !facing_right;
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}

	private IEnumerator ResetStage()
	{
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
