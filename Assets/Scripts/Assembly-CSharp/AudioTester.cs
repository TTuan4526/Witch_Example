using UnityEngine;

public class AudioTester : MonoBehaviour
{
	private AudioSource source;

	public AudioClip chest_open;

	public AudioClip enemy_death;

	public AudioClip enemy_hit;

	public AudioClip fire_bomb;

	public AudioClip fire_cast;

	public AudioClip fire_dash;

	public AudioClip ice_block;

	public AudioClip ice_cast;

	public AudioClip item_pickup;

	public AudioClip jump;

	public AudioClip jump_2;

	public AudioClip jump_dash;

	public AudioClip jump_penta;

	public AudioClip magic_shrine_fire;

	public AudioClip magic_shrine_ice;

	public AudioClip magic_shrine_thunder;

	public AudioClip roll;

	public AudioClip saving;

	public AudioClip special_text;

	public AudioClip swing_1;

	public AudioClip swing_2;

	public AudioClip thunder_cast;

	private void Start()
	{
		source = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			source.PlayOneShot(chest_open, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			source.PlayOneShot(enemy_death, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			source.PlayOneShot(enemy_hit, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			source.PlayOneShot(fire_bomb, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			source.PlayOneShot(fire_cast, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			source.PlayOneShot(fire_dash, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			source.PlayOneShot(ice_block, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			source.PlayOneShot(ice_cast, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			source.PlayOneShot(item_pickup, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			source.PlayOneShot(jump, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad0))
		{
			source.PlayOneShot(jump_2, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad1))
		{
			source.PlayOneShot(jump_dash, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad2))
		{
			source.PlayOneShot(jump_penta, 0.5f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad3))
		{
			source.PlayOneShot(magic_shrine_fire, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad4))
		{
			source.PlayOneShot(magic_shrine_ice, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad5))
		{
			source.PlayOneShot(magic_shrine_thunder, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad6))
		{
			source.PlayOneShot(roll, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad7))
		{
			source.PlayOneShot(saving, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad8))
		{
			source.PlayOneShot(special_text, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad9))
		{
			source.PlayOneShot(swing_1, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			source.PlayOneShot(swing_2, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			source.PlayOneShot(thunder_cast, 0.25f);
		}
		else if (Input.GetKeyDown(KeyCode.O))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.25f;
		}
		else if (Input.GetKeyDown(KeyCode.P))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.5f;
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.75f;
		}
		else if (Input.GetKeyDown(KeyCode.K))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 1f;
		}
	}
}
