using System.Collections;
using UnityEngine;

public class CrystalShard : MonoBehaviour
{
	private AudioSource source;

	private WorldScript WorldScript;

	private Animator anim;

	private Rigidbody2D rbody;

	private GameObject player;

	public string idle_number = string.Empty;

	private bool collected;

	private bool fly_to_player;

	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		player = GameObject.FindGameObjectWithTag("Player");
		source = GetComponent<AudioSource>();
		WorldScript = GameObject.Find("WorldObject").GetComponent<WorldScript>();
		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		anim.Play("crystal_chank_idle_" + idle_number);
		StartCoroutine("StartFlying");
	}

	private void Update()
	{
		if (fly_to_player && (double)Vector3.Distance(base.transform.position, player.transform.position) > 0.01)
		{
			Vector2 velocity = new Vector2((player.transform.position.x - base.transform.position.x) * 6f, (player.transform.position.y - base.transform.position.y) * 6f);
			rbody.velocity = velocity;
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.gameObject.tag == "Player" || col.gameObject.tag == "ground_check" || col.gameObject.tag == "collect_area") && !collected)
		{
			collected = true;
			source.Play();
			GetComponent<CircleCollider2D>().enabled = false;
			WorldScript.GetCrystalShard();
			GetComponent<Animator>().Play("crystal_chank_collect");
			rbody.gravityScale = 0f;
			rbody.velocity = new Vector2(0f, 0f);
			fly_to_player = false;
		}
	}

	private IEnumerator StartFlying()
	{
		yield return new WaitForSeconds(Random.Range(0.15f, 0.75f));
		rbody.gravityScale = 0f;
		rbody.velocity = new Vector2(0f, 0f);
		fly_to_player = true;
	}

	private IEnumerator End()
	{
		yield return new WaitForSeconds(1f);
		Object.Destroy(base.gameObject);
	}
}
