using UnityEngine;

public class Grimoir : MonoBehaviour
{
	public GameObject Sugarball;

	public Transform gun_point;

	public Transform gun_fire;

	private float fire_tick = 0.2f;

	private float open_tick;

	private bool opened;

	public Animator animator;

	public Animator gunfire_animator;

	private Transform grimoire_position;

	private Transform Witchy;

	private Rigidbody2D rbody;

	private void Start()
	{
		Witchy = GameObject.Find("Witchy").transform;
		grimoire_position = GameObject.Find("GrimoirePosition").transform;
		animator = GetComponent<Animator>();
		gunfire_animator = gun_fire.GetComponent<Animator>();
		rbody = base.gameObject.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (fire_tick > 0f)
		{
			fire_tick -= Time.deltaTime;
		}
		if (open_tick > 0f)
		{
			open_tick -= Time.deltaTime;
		}
		if (Input.GetKeyDown(KeyCode.F) && fire_tick <= 0f)
		{
			if (!opened)
			{
				opened = true;
				open_tick = 1f;
			}
			open_tick = 1f;
			fire_tick = 0.2f;
			Attack();
		}
		if (open_tick <= 0f && opened)
		{
			opened = false;
			animator.Play("grimoir_close");
		}
		if (Witchy.localScale.x != base.transform.localScale.x)
		{
			base.transform.localScale = Witchy.localScale;
		}
		if ((double)Vector3.Distance(base.transform.position, grimoire_position.position) > 0.1)
		{
			Vector2 velocity = new Vector2((grimoire_position.position.x - base.transform.position.x) * 4f, (grimoire_position.position.y - base.transform.position.y) * 4f);
			rbody.velocity = velocity;
		}
		else
		{
			rbody.velocity = new Vector2(0f, 0f);
		}
	}

	private void Attack()
	{
	}
}
