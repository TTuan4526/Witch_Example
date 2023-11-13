using System.Collections;
using UnityEngine;

public class meteor : MonoBehaviour
{
	private void Start()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			GetComponent<AudioSource>().volume = 0.1f * (float)PlayerPrefs.GetInt("effects_volume");
		}
		Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, 1f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.tag == "enemy" || array[i].gameObject.tag == "turnip")
			{
				if (array[i].transform.position.x < base.transform.position.x)
				{
					array[i].gameObject.SendMessage("DamageFromRight", 3);
				}
				else if (array[i].transform.position.x > base.transform.position.x)
				{
					array[i].gameObject.SendMessage("DamageFromLeft", 3);
				}
				else
				{
					array[i].gameObject.SendMessage("DamageFromLeft", 3);
				}
			}
			else if (array[i].gameObject.tag == "chest")
			{
				array[i].gameObject.GetComponent<chest>().Open("none");
			}
		}
	}

	private void MakeSound()
	{
		GetComponent<AudioSource>().Play();
	}

	private void MeteorEnd()
	{
		StartCoroutine("DestrMeteor");
	}

	private IEnumerator DestrMeteor()
	{
		yield return new WaitForSeconds(0.75f);
		Object.Destroy(base.gameObject);
	}
}
