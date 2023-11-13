using UnityEngine;

public class small_mushroom : MonoBehaviour
{
	private Animator anim;

	public bool have_audio;

	private void Start()
	{
		anim = GetComponent<Animator>();
		if (PlayerPrefs.HasKey("effects_volume") && have_audio)
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
	}

	public void Grow()
	{
		anim.SetTrigger("grow");
	}

	public void Expl()
	{
		anim.SetTrigger("expl");
		Collider2D[] array = Physics2D.OverlapBoxAll(new Vector3(base.transform.position.x, base.transform.position.y - 0.1f, base.transform.position.z), new Vector2(0.8f, 0.6f), 0f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "Player")
			{
				array[i].gameObject.SendMessage("DamageAnyway", 2);
			}
		}
	}
}
