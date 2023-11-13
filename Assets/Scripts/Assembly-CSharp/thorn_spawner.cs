using System.Collections;
using UnityEngine;

public class thorn_spawner : MonoBehaviour
{
	public bool facing_right;

	public GameObject thorn;

	public void Spawn()
	{
		StartCoroutine("SpawnEnum");
	}

	private IEnumerator SpawnEnum()
	{
		for (int i = 0; i <= 20; i++)
		{
			yield return new WaitForSeconds(0.1f);
			if (!facing_right)
			{
				Object.Instantiate(thorn, new Vector3(base.transform.position.x + (float)i * 0.3f, base.transform.position.y, base.transform.position.z), Quaternion.identity);
			}
			else
			{
				Object.Instantiate(thorn, new Vector3(base.transform.position.x - (float)i * 0.3f, base.transform.position.y, base.transform.position.z), Quaternion.identity);
			}
		}
		yield return new WaitForSeconds(0.25f);
		Object.Destroy(base.gameObject);
	}
}
