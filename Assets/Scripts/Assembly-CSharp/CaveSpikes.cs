using System.Collections;
using UnityEngine;

public class CaveSpikes : MonoBehaviour
{
	public GameObject[] spikes;

	public void DropLeftToRight()
	{
		StartCoroutine(DropLeftRight());
	}

	public void DropRightToLeft()
	{
		StartCoroutine(DropRightLeft());
	}

	private IEnumerator DropRightLeft()
	{
		for (int i = spikes.Length - 1; i >= 0; i--)
		{
			yield return new WaitForSeconds(0.2f);
			if (spikes[i].activeSelf)
			{
				spikes[i].SendMessage("Fall");
			}
		}
	}

	private IEnumerator DropLeftRight()
	{
		for (int i = 0; i < spikes.Length; i++)
		{
			yield return new WaitForSeconds(0.2f);
			if (spikes[i].activeSelf)
			{
				spikes[i].SendMessage("Fall");
			}
		}
	}

	public void SpawnItem()
	{
		StopAllCoroutines();
	}

	public void ResetItemState()
	{
	}

	public void SaveObjectState()
	{
	}
}
