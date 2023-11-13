using UnityEngine;

public class ExplosionBig : MonoBehaviour
{
	private void Awake()
	{
		Object.Destroy(base.gameObject, 1f);
	}
}
