using UnityEngine;

public class trampoline : MonoBehaviour
{
	private Animator anim;

	private AudioSource source;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			anim.Play("trampoline_fruit_jump");
			source.Play();
			col.gameObject.GetComponent<PlayerController>().second_jump = true;
			col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(col.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0f);
			col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0.65f), ForceMode2D.Impulse);
		}
	}
}
