using UnityEngine;

public class mana_fountain : MonoBehaviour
{
	private Animator anim;

	private AudioSource source;

	private PlayerController player_cont;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		player_cont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && player_cont.mana < player_cont.mana_max)
		{
			anim.SetTrigger("bigger");
			source.PlayDelayed(0.1f);
			player_cont.RestoreMana(4);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			anim.SetTrigger("lower");
		}
	}
}
