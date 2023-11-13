using UnityEngine;

public class ice_surface : MonoBehaviour
{
	public float wide;

	private void Start()
	{
		wide = GetComponent<BoxCollider2D>().size.x;
	}
}
