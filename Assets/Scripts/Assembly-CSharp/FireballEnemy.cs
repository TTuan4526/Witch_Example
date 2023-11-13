using UnityEngine;

public class FireballEnemy : MonoBehaviour
{
	public string prefix;

	private float lifetime = 0.75f;

	private bool destroyed;

	private Rigidbody2D rbody;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
		rbody = base.gameObject.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (lifetime > 0f)
		{
			lifetime -= Time.deltaTime;
		}
		if (lifetime <= 0f)
		{
			Explode();
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("end"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (destroyed)
		{
			return;
		}
		if (col.gameObject.tag == "Player")
		{
			if (!col.gameObject.GetComponent<PlayerController>().iframe)
			{
				Explode();
				col.gameObject.SendMessage("Damage", 1);
			}
		}
		else if (col.gameObject.tag == "witchy_ground")
		{
			if (!col.gameObject.GetComponent<GroundCheck>().player.iframe)
			{
				Explode();
				col.gameObject.GetComponent<GroundCheck>().player.SendMessage("Damage", 1);
			}
		}
		else if (col.gameObject.tag != "Sense" && col.gameObject.tag != "Collectible" && col.gameObject.tag != "PlayerGround" && col.gameObject.tag != "Bullet" && col.gameObject.tag != "misc" && col.gameObject.tag != "dialog" && col.gameObject.tag != "enemy" && col.gameObject.tag != "chest" && col.gameObject.tag != "collect_area")
		{
			destroyed = true;
			Explode();
		}
	}

	private void Explode()
	{
		rbody.velocity = new Vector3(0f, 0f, 0f);
		anim.Play(prefix + "_explode");
		destroyed = true;
	}
}
