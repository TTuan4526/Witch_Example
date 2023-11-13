using System.Collections;
using UnityEngine;

public class frost_circle : MonoBehaviour
{
	public GameObject frostbolt;

	private Animator anim;

	public Transform fire_position_1;

	public Transform fire_position_2;

	public Transform fire_position_3;

	private void Start()
	{
		anim = GetComponent<Animator>();
		anim.Play("frost_circle_start");
		StartCoroutine(SpawnFireball(0.25f, fire_position_1.position, Quaternion.AngleAxis(90f, Vector3.forward)));
		StartCoroutine(SpawnFireball(0.5f, fire_position_2.position, Quaternion.AngleAxis(180f, Vector3.forward)));
		StartCoroutine(SpawnFireball(0.75f, fire_position_3.position, Quaternion.AngleAxis(0f, Vector3.forward)));
		StartCoroutine("Fade");
	}

	private IEnumerator Fade()
	{
		yield return new WaitForSeconds(2f);
		anim.Play("frost_circle_end");
	}

	private void DestroyCircle()
	{
		Object.Destroy(base.gameObject);
	}

	private IEnumerator SpawnFireball(float delay, Vector3 position, Quaternion rotat)
	{
		yield return new WaitForSeconds(delay);
		Object.Instantiate(frostbolt, position, rotat);
	}
}
