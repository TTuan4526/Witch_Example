using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldScriptHome : MonoBehaviour
{
	public int this_lvl;

	public bool game_paused;

	public bool home;

	public AudioClip PauseSound;

	public AudioClip FinishSound;

	public GameObject PauseGamePanel;

	public GameObject home_dialog;

	public Text ui_resume_text;

	public Text ui_restart_text;

	public Text ui_exit_text;

	public GameObject ui_book;

	public GameObject menu_arrow;

	public GameObject loading_panel;

	public Text loading_text;

	public Sprite ps_button_cross;

	public Sprite xbox_button_b;

	public Sprite keyboard_button_esc;

	public GameObject ui_close_book_button;

	public Text ui_close_book_text;

	private GameObject Player;

	private AudioSource source;

	private bool controller_x_axis_press;

	private bool controller_y_axis_press;

	private bool controller_6_axis_press;

	private bool controller_7_axis_press;

	private bool controller_8_axis_press;

	private string current_control_scheme = "PC";

	private int pause_book_state;

	private int pause_menu_state;

	private string AchievementID;

	private Vector3 menu_arrow_default_position;

	private void Start()
	{
		Cursor.visible = false;
		if (!PlayerPrefs.HasKey("control_scheme"))
		{
			PlayerPrefs.SetString("control_scheme", "PC");
			current_control_scheme = "PC";
		}
		else
		{
			current_control_scheme = PlayerPrefs.GetString("control_scheme");
		}
		menu_arrow_default_position = menu_arrow.transform.localPosition;
		Player = GameObject.FindGameObjectWithTag("Player");
		Player.GetComponent<Animator>().Play("witchy_sleep");
		Player.GetComponent<PlayerControllerHome>().Unblock();
		source = GetComponent<AudioSource>();
		GetComponent<LocalizationHome>().Translate();
		StartCoroutine("WakeUpEnum");
		SetVolume();
		SetControlButtons();
	}

	public void SetControlButtons()
	{
		switch (current_control_scheme)
		{
		case "XBOX":
			ui_close_book_button.GetComponent<Image>().sprite = xbox_button_b;
			break;
		case "PS":
			ui_close_book_button.GetComponent<Image>().sprite = ps_button_cross;
			break;
		case "PC":
			ui_close_book_button.GetComponent<Image>().sprite = keyboard_button_esc;
			break;
		}
	}

	private void SetVolume()
	{
		if (PlayerPrefs.HasKey("music_volume"))
		{
			GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.03f * (float)PlayerPrefs.GetInt("music_volume");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || (Input.GetButtonDown("ControllerSubmit") && current_control_scheme == "XBOX") || (Input.GetButtonDown("ControllerSubmitPS") && current_control_scheme == "PS") || (Input.GetButtonDown("ControllerSubmit") && current_control_scheme == "PC"))
		{
			PauseGame();
		}
		if (!game_paused)
		{
			return;
		}
		if (Input.GetAxis("ControllerAxis8") == 0f && controller_8_axis_press)
		{
			controller_8_axis_press = false;
		}
		if (Input.GetAxis("ControllerAxis6") == 0f && controller_6_axis_press)
		{
			controller_6_axis_press = false;
		}
		if (controller_x_axis_press && ((Input.GetAxis("Horizontal") > 0f && (double)Input.GetAxis("Horizontal") < 0.8) || (Input.GetAxis("Horizontal") < 0f && (double)Input.GetAxis("Horizontal") > -0.8)))
		{
			controller_x_axis_press = false;
		}
		if (Input.GetAxis("ControllerAxis7") == 0f && controller_7_axis_press)
		{
			controller_7_axis_press = false;
		}
		if (controller_y_axis_press && ((Input.GetAxis("Vertical") > 0f && (double)Input.GetAxis("Vertical") < 0.8) || (Input.GetAxis("Vertical") < 0f && (double)Input.GetAxis("Vertical") > -0.8)))
		{
			controller_y_axis_press = false;
		}
		if (pause_book_state != 1)
		{
			return;
		}
		if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)) && pause_menu_state != 2)
		{
			if (Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press)
			{
				controller_7_axis_press = true;
			}
			else if (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press)
			{
				controller_8_axis_press = true;
			}
			else if (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)
			{
				controller_y_axis_press = true;
			}
			source.Play();
			pause_menu_state++;
			menu_arrow.transform.localPosition = new Vector3(menu_arrow.transform.localPosition.x, menu_arrow.transform.localPosition.y - 64f, menu_arrow.transform.localPosition.z);
		}
		else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)) && pause_menu_state != 0)
		{
			if (Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press)
			{
				controller_7_axis_press = true;
			}
			else if (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press)
			{
				controller_8_axis_press = true;
			}
			else if (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)
			{
				controller_y_axis_press = true;
			}
			source.Play();
			pause_menu_state--;
			menu_arrow.transform.localPosition = new Vector3(menu_arrow.transform.localPosition.x, menu_arrow.transform.localPosition.y + 64f, menu_arrow.transform.localPosition.z);
		}
		else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
		{
			if (pause_menu_state == 0)
			{
				PauseGame();
			}
			else if (pause_menu_state == 1)
			{
				Player.GetComponent<PlayerControllerHome>().RestartAfterDeath(0.1f);
				PauseGame();
			}
			else if (pause_menu_state == 2)
			{
				Time.timeScale = 1f;
				Object.Destroy(GameObject.Find("BackgroundMusic"));
				Object.Destroy(GameObject.FindGameObjectWithTag("Data"));
				LoadSceneAsyncronical("main_menu");
			}
		}
	}

	public void LoadSceneAsyncronical(string scene_name)
	{
		loading_panel.SetActive(true);
		SceneManager.LoadScene(scene_name);
	}

	public void PauseGame()
	{
		game_paused = !game_paused;
		if (game_paused)
		{
			Player.GetComponent<PlayerControllerHome>().StopMoving();
			source.PlayOneShot(PauseSound, 1f);
			PauseGamePanel.SetActive(true);
			ui_book.GetComponent<Animator>().Play("ui_book_page_main");
			ui_resume_text.gameObject.SetActive(true);
			ui_restart_text.gameObject.SetActive(true);
			ui_exit_text.gameObject.SetActive(true);
			menu_arrow.gameObject.SetActive(true);
			ui_close_book_button.gameObject.SetActive(true);
			ui_close_book_text.gameObject.SetActive(true);
			pause_menu_state = 0;
			pause_book_state = 1;
		}
		else
		{
			Player.GetComponent<PlayerControllerHome>().paused = false;
			source.PlayOneShot(PauseSound, 1f);
			PauseGamePanel.SetActive(false);
			menu_arrow.transform.localPosition = menu_arrow_default_position;
		}
	}

	public void LoadFirstLvl()
	{
		loading_panel.SetActive(true);
		SceneManager.LoadScene("lvl_0");
	}

	public void WakeUp()
	{
		home_dialog.GetComponent<home_dialog>().StartDialog();
	}

	private IEnumerator WakeUpEnum()
	{
		yield return new WaitForSeconds(3f);
		Player.GetComponent<Animator>().Play("witchy_wakeup");
	}

	private void GetAchievment()
	{
	}
}
