using UnityEngine;

public class CatNormal : MonoBehaviour
{
	public bool grounded;

	private AudioSource source;

	private Animator anim;

	private Rigidbody2D rbody;

	private bool blocked;

	private bool dead;

	private bool paused;

	private bool facing_right = true;

	private float move;

	private float max_speed = 2.75f;

	private float jump_force = 20f;

	private void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!blocked && !dead && (Input.GetButtonDown("Jump") || Input.GetButtonDown("ControllerA")) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S) && !((double)Input.GetAxis("Vertical") < -0.8) && !dead && !paused && grounded)
		{
			rbody.velocity = new Vector2(rbody.velocity.x, 0f);
			rbody.AddForce(new Vector2(0f, jump_force));
			source.Play();
		}
	}

	private void FixedUpdate()
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

	private void Flip()
	{
		facing_right = !facing_right;
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}
}
