using System;
using System.Collections;
using UnityEngine;

public class boss_eyes : MonoBehaviour
{
	public Transform[] eyes;

	private Vector3 default_position;

	public string state = "stay";

	public int substate;

	private void Start()
	{
		default_position = base.transform.localPosition;
	}

	private void Update()
	{
		if (state == "down" && Vector3.Distance(base.transform.localPosition, new Vector3(0f, -0.15f, base.transform.localPosition.z)) <= 0.05f)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			StartCoroutine("NextSubstate");
			state = "stay";
		}
	}

	private IEnumerator NextSubstate()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.GetComponent<Animator>().Play("enemy_eye_disappear");
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = (disposable = enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
		yield return new WaitForSeconds(0.25f);
		base.transform.localPosition = default_position;
		for (int i = 0; i < eyes.Length; i++)
		{
			eyes[i].gameObject.SetActive(true);
		}
		IEnumerator enumerator2 = base.transform.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform2 = (Transform)enumerator2.Current;
				transform2.GetComponent<Animator>().Play("enemy_eye_idle");
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable3 = (disposable = enumerator2 as IDisposable);
			if (disposable != null)
			{
				disposable3.Dispose();
			}
		}
		if (substate == 2)
		{
			GameObject.FindGameObjectWithTag("boss").GetComponent<skull_boss>().AfterEyes();
			yield break;
		}
		eyes[UnityEngine.Random.Range(0, 9)].gameObject.SetActive(false);
		GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -1.5f);
		substate++;
		state = "down";
	}

	public void RestoreAll()
	{
		for (int i = 0; i < eyes.Length; i++)
		{
			eyes[i].gameObject.SetActive(true);
		}
		foreach (Transform item in base.transform)
		{
			item.GetComponent<Animator>().Play("enemy_eye_idle");
		}
		substate = 0;
		base.transform.localPosition = default_position;
	}

	public void GetDown()
	{
		for (int i = 0; i < eyes.Length; i++)
		{
			eyes[i].gameObject.SetActive(true);
		}
		substate = 0;
		eyes[UnityEngine.Random.Range(0, 9)].gameObject.SetActive(false);
		GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -1.5f);
		state = "down";
	}
}
