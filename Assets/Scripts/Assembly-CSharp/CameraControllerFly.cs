using UnityEngine;

public class CameraControllerFly : MonoBehaviour
{
	public float left_x;

	public float right_x;

	private float new_x_pos;

	public Transform player;

	private void Start()
	{
		if (Screen.width == 1280 && Screen.height == 1024)
		{
			Camera.main.orthographicSize = 3.6f;
		}
		else if (Screen.width == 1024 && Screen.height == 768)
		{
			Camera.main.orthographicSize = 3.3f;
		}
		else
		{
			Camera.main.orthographicSize = 2.5f;
		}
	}

	public void Upd()
	{
		float num = 1.7777778f;
		float num2 = (float)Screen.width / (float)Screen.height;
		float num3 = num2 / num;
		Camera component = GetComponent<Camera>();
		if (num3 < 1f)
		{
			Rect rect = component.rect;
			rect.width = 1f;
			rect.height = num3;
			rect.x = 0f;
			rect.y = (1f - num3) / 2f;
			component.rect = rect;
		}
		else
		{
			float num4 = 1f / num3;
			Rect rect2 = component.rect;
			rect2.width = num4;
			rect2.height = 1f;
			rect2.x = (1f - num4) / 2f;
			rect2.y = 0f;
			component.rect = rect2;
		}
	}

	private void Update()
	{
		if (player.position.x <= left_x || player.position.x >= right_x)
		{
			new_x_pos = base.transform.position.x;
		}
		else
		{
			new_x_pos = player.position.x;
		}
		base.transform.position = new Vector3(new_x_pos, base.transform.position.y, base.transform.position.z);
	}
}
