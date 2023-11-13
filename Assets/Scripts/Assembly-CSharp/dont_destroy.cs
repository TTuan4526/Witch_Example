using UnityEngine;

public class dont_destroy : MonoBehaviour
{
	private static dont_destroy instance;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
