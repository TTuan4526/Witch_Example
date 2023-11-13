using UnityEngine;

public class ShardGround : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "ground")
		{
			base.gameObject.GetComponentInParent<Rigidbody2D>().gravityScale = 0f;
			base.gameObject.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			Object.Destroy(base.gameObject);
		}
	}
}
