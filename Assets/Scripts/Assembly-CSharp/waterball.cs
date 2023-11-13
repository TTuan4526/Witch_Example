using UnityEngine;

public class waterball : MonoBehaviour
{
	private float lifetime = 0.7f;

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
		if (col.gameObject.tag == "enemy" || col.gameObject.tag == "turnip")
		{
			destroyed = true;
			Explode();
			if (col.transform.position.x < base.transform.position.x)
			{
				col.gameObject.SendMessage("DamageFromRight", 1);
			}
			else if (col.transform.position.x > base.transform.position.x)
			{
				col.gameObject.SendMessage("DamageFromLeft", 1);
			}
			else
			{
				col.gameObject.SendMessage("DamageFromLeft", 1);
			}
		}
		else if (col.gameObject.tag == "boss")
		{
			destroyed = true;
			Explode();
			col.gameObject.SendMessage("DamageFromRight", 1);
		}
		else if (col.gameObject.tag == "brick")
		{
			col.gameObject.SendMessage("Explode", "frost");
			destroyed = true;
			Explode();
		}
		else if (col.gameObject.tag == "chest")
		{
			col.gameObject.SendMessage("Open", "frost");
			destroyed = true;
			Explode();
		}
		else if (col.gameObject.tag != "Sense" && col.gameObject.tag != "Collectible" && col.gameObject.tag != "PlayerGround" && col.gameObject.tag != "Bullet" && col.gameObject.tag != "Player" && col.gameObject.tag != "misc" && col.gameObject.tag != "trigger" && col.gameObject.tag != "dialog" && col.gameObject.tag != "end" && col.gameObject.tag != "fireball")
		{
			destroyed = true;
			Explode();
		}
	}

	private void Explode()
	{
		rbody.velocity = new Vector3(0f, 0f, 0f);
		anim.Play("waterball_explode");
		base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}
}
