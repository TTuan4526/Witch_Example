using UnityEngine;

public class farm_ccrystal : MonoBehaviour
{
	private int hp = 9;

	private Animator anim;

	public bool collected;

	public bool picked;

	public GameObject shard_1;

	public GameObject shard_2;

	public GameObject shard_3;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void HitMeBabyOneMoreTime()
	{
		if (hp > 0)
		{
			hp--;
			if (hp == 0)
			{
				DropShards(5);
				collected = true;
				GetComponent<BoxCollider2D>().enabled = false;
			}
			else
			{
				DropShards(1);
			}
			anim.Play("farm_crystal_" + hp);
		}
	}

	private void DropShards(int number)
	{
		for (int i = 0; i < number; i++)
		{
			switch (Random.Range(1, 4))
			{
			case 1:
			{
				GameObject gameObject3 = Object.Instantiate(shard_1, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject3.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 2:
			{
				GameObject gameObject2 = Object.Instantiate(shard_2, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject2.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			case 3:
			{
				GameObject gameObject = Object.Instantiate(shard_3, new Vector3(base.transform.position.x + Random.Range(-1f, 1f) / 5f, base.transform.position.y, base.transform.position.z), base.transform.rotation);
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(1, 7), 0f), ForceMode2D.Impulse);
				break;
			}
			}
		}
	}

	public void SpawnItem()
	{
		if (!picked && !collected)
		{
			GetComponent<Animator>().Play("farm_crystal_idle");
			collected = false;
			hp = 9;
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			GetComponent<Animator>().Play("farm_crystal_empty");
		}
	}

	public void ResetItemState()
	{
		if (!picked && collected)
		{
			collected = false;
		}
	}

	public void SaveObjectState()
	{
		if (collected && !picked)
		{
			picked = true;
		}
	}
}
