using UnityEngine;

public class run_dust : MonoBehaviour
{
	public Transform run_dust_position;

	public void PlaceRunDust()
	{
		base.transform.position = run_dust_position.position;
	}
}
