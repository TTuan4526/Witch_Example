using System.Collections;
using UnityEngine;

public class fire_circle : MonoBehaviour
{
	public GameObject fireball;

	private Animator anim;

	public Transform fire_position_1;

	public Transform fire_position_2;

	public Transform fire_position_3;

	private void Start()
	{
		anim = GetComponent<Animator>();
		anim.Play("fire_circle_start");
		StartCoroutine(SpawnFireball(0.25f, fire_position_1.position));
		StartCoroutine(SpawnFireball(0.5f, fire_position_2.position));
		StartCoroutine(SpawnFireball(0.75f, fire_position_3.position));
		StartCoroutine("Fade");
	}

	private IEnumerator Fade()
	{
		yield return new WaitForSeconds(2f);
		anim.Play("fire_circle_fade");
	}

	private void DestroyCircle()
	{
		Object.Destroy(base.gameObject);
	}

	private IEnumerator SpawnFireball(float delay, Vector3 position)
	{
		yield return new WaitForSeconds(delay);
		Object.Instantiate(fireball, position, base.transform.rotation);
	}
}
