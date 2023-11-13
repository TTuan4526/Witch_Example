using System.Collections;
using UnityEngine;

public class goblin_fireball : MonoBehaviour
{
	private Animator anim;

	private bool exploded;

	private void Start()
	{
		anim = GetComponent<Animator>();
		anim.Play("goblin_fireball_appear");
		Object.Destroy(base.gameObject, 4f);
		StartCoroutine("Shoot");
	}

	private IEnumerator Shoot()
	{
		yield return new WaitForSeconds(1.5f);
		GetComponent<CircleCollider2D>().enabled = true;
		anim.Play("goblin_fireball_fly");
		Transform player = GameObject.FindGameObjectWithTag("Player").transform;
		Vector3 targ = new Vector3(player.position.x, player.position.y, base.transform.position.z);
		Vector3 objectPos = base.transform.position;
		targ.x -= objectPos.x;
		targ.y -= objectPos.y;
		float angle = Mathf.Atan2(targ.y, targ.x) * 57.29578f;
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		GetComponent<Rigidbody2D>().velocity = base.transform.right * 4.5f;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !exploded && !col.gameObject.GetComponent<PlayerController>().fire_dash && !col.gameObject.GetComponent<PlayerController>().meteor_attack)
		{
			col.gameObject.SendMessage("Damage", 1);
			exploded = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			anim.Play("goblin_fireball_break");
		}
		else if (col.gameObject.tag == "witchy_dialog" && !exploded && !col.gameObject.GetComponent<GroundCheck>().player.fire_dash && !col.gameObject.GetComponent<GroundCheck>().player.meteor_attack)
		{
			col.gameObject.SendMessage("Damage", 1);
			exploded = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			anim.Play("goblin_fireball_break");
		}
	}

	private void DestroyFireball()
	{
		Object.Destroy(base.gameObject);
	}
}
