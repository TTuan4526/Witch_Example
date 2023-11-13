using UnityEngine;

public class bone : MonoBehaviour
{
	public float xRotation = 90f;

	public float xRotation1;

	public float RotationSpeed = 100f;

	private bool destroyed;

	private void Update()
	{
		if (!destroyed)
		{
			base.transform.Rotate(Vector3.forward * (550f * Time.deltaTime));
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !destroyed)
		{
			if (!col.gameObject.GetComponent<PlayerController>().iframe)
			{
				Explode();
				col.gameObject.SendMessage("Damage", 1);
			}
		}
		else if (col.gameObject.tag != "Sense" && col.gameObject.tag != "e_projectile" && col.gameObject.tag != "PlayerGround" && col.gameObject.tag != "platform" && col.gameObject.tag != "dialog" && col.gameObject.tag != "enemy" && col.gameObject.tag != "boss" && col.gameObject.tag != "collect_area" && !col.gameObject.name.Contains("Platform") && !destroyed)
		{
			Explode();
		}
	}

	private void Explode()
	{
		destroyed = true;
		GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		GetComponent<Rigidbody2D>().gravityScale = 0f;
		base.transform.rotation = Quaternion.identity;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<Animator>().Play("bone_explode");
	}

	private void Destr()
	{
		Object.Destroy(base.gameObject);
	}
}
