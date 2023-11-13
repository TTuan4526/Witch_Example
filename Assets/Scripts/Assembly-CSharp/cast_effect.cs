using UnityEngine;

public class cast_effect : MonoBehaviour
{
	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("cast_effect_end"))
		{
			Object.Destroy(base.gameObject);
		}
	}
}
