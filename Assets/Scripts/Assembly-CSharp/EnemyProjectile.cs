using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	private Animator anim;

	private bool exploded;

	public string prefix;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("end"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!exploded && col.gameObject.tag != "e_projectile" && col.gameObject.tag != "collect_area" && col.gameObject.tag != "misc" && col.gameObject.tag != "dialog" && col.gameObject.tag != "enemy" && !col.gameObject.name.Contains("Platform"))
		{
			if (col.gameObject.tag == "Player" && !col.gameObject.GetComponent<PlayerController>().fire_dash && !col.gameObject.GetComponent<PlayerController>().meteor_attack)
			{
				col.gameObject.SendMessage("Damage", 1);
			}
			exploded = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			GetComponent<Rigidbody2D>().gravityScale = 0f;
			anim.Play(prefix + "_end");
		}
	}
}
