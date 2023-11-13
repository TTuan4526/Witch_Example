using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class main_menu : MonoBehaviour
{
	public GameObject loading_panel;

	public GameObject settings_left_pointer;

	public GameObject settings_right_pointer;

	public GameObject settings_back_panel;

	public GameObject ui_continue;

	public GameObject main_panel;

	public GameObject confirmation_panel;

	public GameObject settings_panel;

	public GameObject controls_panel;

	public GameObject console_descr;

	public GameObject pc_descr;

	public Text[] setting_texts;

	public Text[] setting_values;

	public Text[] menu_texts;

	public Sprite[] control_schemes;

	public GameObject[] menu_texts_back;

	public GameObject control_scheme;

	public GameObject settings_button_icon;

	public GameObject back_button_icon;

	public GameObject back_controls_button_icon;

	public GameObject select_button_icon;

	public GameObject controls_button_icon;

	public GameObject select_button_text;

	public GameObject conf_back_icon;

	public GameObject dif_back_icon;

	public Sprite xbox_button_a;

	public Sprite xbox_button_b;

	public Sprite xbox_button_rb;

	public Sprite xbox_button_lb;

	public Sprite ps_button_cross;

	public Sprite ps_button_box;

	public Sprite ps_button_r1;

	public Sprite ps_button_l1;

	public Sprite keyboard_button_enter;

	public Sprite keyboard_button_esc;

	public Sprite keyboard_button_tab;

	private AudioSource source;

	private int menu_state = 1;

	private int confirmation_state;

	private int settings_state;

	private int music_volume = 5;

	private int effects_volume = 5;

	private int current_resolution_cell = 10;

	private bool the_game_is_on;

	private Vector3 main_pointer_position;

	private Vector3 settings_back_panel_position;

	private Vector3 settings_left_pointer_position;

	private Vector3 settings_right_pointer_position;

	private Data data;

	private Color light_color;

	private Color dark_color;

	private string current_control_scheme = "XBOX";

	private string state = "main";

	public string on_phrase;

	public string off_phrase;

	public string crt_status;

	public string noize_status;

	public string volume_status;

	public string fullscreen_status;

	public string lang_phrase;

	public string lvl_phrase;

	public string settings_phrase;

	public string controls_phrase;

	private int[] width = new int[15]
	{
		1024, 1280, 1280, 1280, 1360, 1366, 1440, 1600, 1680, 1920,
		1920, 2560, 2560, 3440, 3840
	};

	private int[] height = new int[15]
	{
		768, 800, 1024, 720, 768, 768, 900, 900, 1050, 1080,
		1200, 1440, 1080, 1440, 2160
	};

	private bool controller_y_axis_press;

	private bool controller_8_axis_press;

	private bool controller_x_axis_press;

	private bool controller_6_axis_press;

	private void Start()
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("SteamManager initializee");
		}
		source = GetComponent<AudioSource>();
		Cursor.visible = false;
		light_color = new Color(0.8359f, 0.7187f, 0.4375f);
		dark_color = new Color(0.414f, 0.2187f, 0.25f);
		settings_left_pointer_position = settings_left_pointer.transform.localPosition;
		settings_right_pointer_position = settings_right_pointer.transform.localPosition;
		settings_back_panel_position = settings_back_panel.transform.localPosition;
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		if (!PlayerPrefs.HasKey("book_0_read"))
		{
			PlayerPrefs.SetInt("book_0_read", 0);
		}
		if (!PlayerPrefs.HasKey("book_1_read"))
		{
			PlayerPrefs.SetInt("book_1_read", 0);
		}
		if (!PlayerPrefs.HasKey("book_2_read"))
		{
			PlayerPrefs.SetInt("book_2_read", 0);
		}
		if (!PlayerPrefs.HasKey("book_3_read"))
		{
			PlayerPrefs.SetInt("book_3_read", 0);
		}
		if (PlayerPrefs.HasKey("resolution_width") && PlayerPrefs.HasKey("resolution_height"))
		{
			if (PlayerPrefs.HasKey("fullscreen"))
			{
				Screen.SetResolution(PlayerPrefs.GetInt("resolution_width"), PlayerPrefs.GetInt("resolution_height"), PlayerPrefs.GetInt("fullscreen") == 1);
			}
			else
			{
				Screen.SetResolution(PlayerPrefs.GetInt("resolution_width"), PlayerPrefs.GetInt("resolution_height"), Screen.fullScreen);
			}
			for (int i = 0; i < width.Length; i++)
			{
				if (Screen.width == width[i])
				{
					current_resolution_cell = i;
					break;
				}
			}
		}
		else
		{
			for (int j = 0; j < width.Length; j++)
			{
				if (width[j] == Screen.currentResolution.width && height[j] == Screen.currentResolution.height)
				{
					PlayerPrefs.SetInt("resolution_width", Screen.currentResolution.width);
					PlayerPrefs.SetInt("resolution_height", Screen.currentResolution.height);
					current_resolution_cell = j;
					break;
				}
			}
		}
		if (!Directory.Exists(Application.dataPath + "/Saves"))
		{
			Directory.CreateDirectory(Application.dataPath + "/Saves");
		}
		string path = Application.dataPath + "/Saves/save.pidg";
		if (File.Exists(path))
		{
			the_game_is_on = true;
			ui_continue.SetActive(true);
			menu_state = 0;
			menu_texts[0].GetComponent<Shadow>().enabled = true;
			menu_texts_back[0].SetActive(true);
		}
		else
		{
			menu_texts[1].GetComponent<Shadow>().enabled = true;
			menu_texts_back[1].SetActive(true);
		}
		if (!PlayerPrefs.HasKey("language"))
		{
			PlayerPrefs.SetString("language", "english");
		}
		if (!PlayerPrefs.HasKey("control_scheme"))
		{
			if (Input.GetJoystickNames().Length > 0)
			{
				for (int k = 0; k < Input.GetJoystickNames().Length; k++)
				{
					if (Input.GetJoystickNames()[k].ToLower().Contains("xbox"))
					{
						PlayerPrefs.SetString("control_scheme", "XBOX");
						current_control_scheme = "XBOX";
					}
					else if (Input.GetJoystickNames()[k].ToLower().Contains("ps"))
					{
						PlayerPrefs.SetString("control_scheme", "PS");
						current_control_scheme = "PS";
					}
					else
					{
						PlayerPrefs.SetString("control_scheme", "PC");
						current_control_scheme = "PC";
					}
				}
			}
			else
			{
				PlayerPrefs.SetString("control_scheme", "PC");
				current_control_scheme = "PC";
			}
		}
		else
		{
			current_control_scheme = PlayerPrefs.GetString("control_scheme");
		}
		if (!PlayerPrefs.HasKey("crt_status"))
		{
			PlayerPrefs.SetString("crt_status", "disabled");
		}
		if (!PlayerPrefs.HasKey("music_volume"))
		{
			PlayerPrefs.SetInt("music_volume", 5);
		}
		else
		{
			music_volume = PlayerPrefs.GetInt("music_volume");
		}
		if (!PlayerPrefs.HasKey("effects_volume"))
		{
			PlayerPrefs.SetInt("effects_volume", 10);
		}
		else
		{
			effects_volume = PlayerPrefs.GetInt("effects_volume");
		}
		SetNewMusicVolume();
		SetNewEffectsVolume();
		SetControlScheme();
		UpdateResolutionText();
		StartCoroutine("SetFullscreenDelay");
		GetComponent<LocalizationMainMenu>().Translate();
		UpdateCrtFilter();
		UpdateCrtFilterText();
		UpdateLanguageText();
		UpdateMusicVolumeText();
		UpdateEffectsVolumeText();
	}

	private void Update()
	{
		if (controller_8_axis_press && (double)Input.GetAxis("ControllerAxis8") > -0.1 && (double)Input.GetAxis("ControllerAxis8") < 0.1)
		{
			controller_8_axis_press = false;
		}
		else if (controller_y_axis_press && ((Input.GetAxis("Vertical") > 0f && (double)Input.GetAxis("Vertical") < 0.8) || (Input.GetAxis("Vertical") < 0f && (double)Input.GetAxis("Vertical") > -0.8)))
		{
			controller_y_axis_press = false;
		}
		if (controller_6_axis_press && (double)Input.GetAxis("ControllerAxis6") > -0.1 && (double)Input.GetAxis("ControllerAxis6") < 0.1)
		{
			controller_6_axis_press = false;
		}
		else if (controller_x_axis_press && ((Input.GetAxis("Horizontal") > 0f && (double)Input.GetAxis("Horizontal") < 0.8) || (Input.GetAxis("Horizontal") < 0f && (double)Input.GetAxis("Horizontal") > -0.8)))
		{
			controller_x_axis_press = false;
		}
		if (state == "main")
		{
			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)) && menu_state != 3)
			{
				if (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press)
				{
					controller_8_axis_press = true;
				}
				else if (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)
				{
					controller_y_axis_press = true;
				}
				source.Play();
				menu_texts_back[menu_state].SetActive(false);
				menu_texts[menu_state].GetComponent<Shadow>().enabled = false;
				menu_state++;
				menu_texts_back[menu_state].SetActive(true);
				menu_texts[menu_state].GetComponent<Shadow>().enabled = true;
			}
			else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)) && ((menu_state != 1 && !the_game_is_on) || (menu_state != 0 && the_game_is_on)))
			{
				if (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press)
				{
					controller_8_axis_press = true;
				}
				else if (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)
				{
					controller_y_axis_press = true;
				}
				source.Play();
				menu_texts_back[menu_state].SetActive(false);
				menu_texts[menu_state].GetComponent<Shadow>().enabled = false;
				menu_state--;
				menu_texts_back[menu_state].SetActive(true);
				menu_texts[menu_state].GetComponent<Shadow>().enabled = true;
			}
			else
			{
				if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetButtonDown("ControllerA"))
				{
					return;
				}
				source.Play();
				if (menu_state == 1)
				{
					if (the_game_is_on)
					{
						state = "confirmation";
						menu_texts_back[5].SetActive(true);
						menu_texts[5].GetComponent<Shadow>().enabled = true;
						select_button_icon.SetActive(false);
						select_button_text.SetActive(false);
						confirmation_panel.SetActive(true);
					}
					else
					{
						select_button_icon.SetActive(false);
						select_button_text.SetActive(false);
						StartNewGame();
					}
				}
				else if (menu_state == 0)
				{
					LoadGame();
				}
				else if (menu_state == 2)
				{
					state = "settings";
					settings_state = 0;
					setting_texts[0].color = dark_color;
					setting_values[0].color = dark_color;
					settings_left_pointer.transform.localPosition = settings_left_pointer_position;
					settings_right_pointer.transform.localPosition = settings_right_pointer_position;
					settings_back_panel.transform.localPosition = settings_back_panel_position;
					settings_panel.SetActive(true);
				}
				else if (menu_state == 3)
				{
					Application.Quit();
				}
			}
		}
		else if (state == "confirmation")
		{
			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)) && confirmation_state == 0)
			{
				source.Play();
				menu_texts_back[4].SetActive(true);
				menu_texts_back[5].SetActive(false);
				menu_texts[5].GetComponent<Shadow>().enabled = false;
				menu_texts[4].GetComponent<Shadow>().enabled = true;
				confirmation_state = 1;
			}
			else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)) && confirmation_state == 1)
			{
				source.Play();
				menu_texts_back[5].SetActive(true);
				menu_texts_back[4].SetActive(false);
				menu_texts[5].GetComponent<Shadow>().enabled = true;
				menu_texts[4].GetComponent<Shadow>().enabled = false;
				confirmation_state = 0;
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
			{
				if (confirmation_state == 0)
				{
					source.Play();
					state = "main";
					confirmation_state = 0;
					menu_texts_back[4].SetActive(false);
					menu_texts_back[5].SetActive(false);
					menu_texts[4].GetComponent<Shadow>().enabled = false;
					menu_texts[5].GetComponent<Shadow>().enabled = false;
					menu_state = 1;
					select_button_icon.SetActive(true);
					select_button_text.SetActive(true);
					confirmation_panel.SetActive(false);
				}
				else
				{
					source.Play();
					confirmation_panel.SetActive(false);
					menu_texts_back[4].SetActive(false);
					menu_texts_back[5].SetActive(false);
					menu_texts[4].GetComponent<Shadow>().enabled = false;
					menu_texts[5].GetComponent<Shadow>().enabled = false;
					StartNewGame();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("ControllerB"))
			{
				state = "main";
				confirmation_state = 0;
				menu_texts_back[4].SetActive(false);
				menu_texts_back[5].SetActive(false);
				menu_texts[4].GetComponent<Shadow>().enabled = false;
				menu_texts[5].GetComponent<Shadow>().enabled = false;
				select_button_icon.SetActive(true);
				select_button_text.SetActive(true);
				menu_state = 1;
				confirmation_panel.SetActive(false);
			}
		}
		else if (state == "settings")
		{
			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)) && settings_state < 6)
			{
				if (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press)
				{
					controller_8_axis_press = true;
				}
				else if (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)
				{
					controller_y_axis_press = true;
				}
				setting_texts[settings_state].color = light_color;
				setting_values[settings_state].color = light_color;
				settings_state++;
				setting_texts[settings_state].color = dark_color;
				setting_values[settings_state].color = dark_color;
				source.Play();
				settings_back_panel.transform.localPosition = new Vector3(settings_back_panel.transform.localPosition.x, settings_back_panel.transform.localPosition.y - 75f, settings_back_panel.transform.localPosition.z);
				settings_right_pointer.transform.localPosition = new Vector3(settings_right_pointer.transform.localPosition.x, settings_right_pointer.transform.localPosition.y - 75f, settings_right_pointer.transform.localPosition.z);
				settings_left_pointer.transform.localPosition = new Vector3(settings_left_pointer.transform.localPosition.x, settings_left_pointer.transform.localPosition.y - 75f, settings_left_pointer.transform.localPosition.z);
			}
			else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)) && settings_state > 0)
			{
				if (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press)
				{
					controller_8_axis_press = true;
				}
				else if (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)
				{
					controller_y_axis_press = true;
				}
				setting_texts[settings_state].color = light_color;
				setting_values[settings_state].color = light_color;
				settings_state--;
				setting_texts[settings_state].color = dark_color;
				setting_values[settings_state].color = dark_color;
				source.Play();
				settings_back_panel.transform.localPosition = new Vector3(settings_back_panel.transform.localPosition.x, settings_back_panel.transform.localPosition.y + 75f, settings_back_panel.transform.localPosition.z);
				settings_right_pointer.transform.localPosition = new Vector3(settings_right_pointer.transform.localPosition.x, settings_right_pointer.transform.localPosition.y + 75f, settings_right_pointer.transform.localPosition.z);
				settings_left_pointer.transform.localPosition = new Vector3(settings_left_pointer.transform.localPosition.x, settings_left_pointer.transform.localPosition.y + 75f, settings_left_pointer.transform.localPosition.z);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press) || (Input.GetAxis("ControllerAxis6") > 0.8f && !controller_6_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") > 0.8f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				source.Play();
				if (settings_state == 0)
				{
					ChangeResolutionRight();
				}
				else if (settings_state == 1)
				{
					source.Play();
					ChangeFullscreen();
				}
				else if (settings_state == 2)
				{
					if (music_volume >= 10)
					{
						music_volume = 0;
					}
					else
					{
						music_volume++;
					}
					SetNewMusicVolume();
					UpdateMusicVolumeText();
				}
				else if (settings_state == 3)
				{
					if (effects_volume >= 10)
					{
						effects_volume = 0;
					}
					else
					{
						effects_volume++;
					}
					SetNewEffectsVolume();
					UpdateEffectsVolumeText();
				}
				else if (settings_state == 4)
				{
					source.Play();
					if (PlayerPrefs.HasKey("crt_status"))
					{
						if (PlayerPrefs.GetString("crt_status") == "disabled")
						{
							PlayerPrefs.SetString("crt_status", "enabled");
						}
						else
						{
							PlayerPrefs.SetString("crt_status", "disabled");
						}
					}
					UpdateCrtFilter();
					UpdateCrtFilterText();
				}
				else if (settings_state == 5)
				{
					ChangeLanguageRight();
				}
				else if (settings_state == 6)
				{
					source.Play();
					if (current_control_scheme == "XBOX")
					{
						current_control_scheme = "PS";
					}
					else if (current_control_scheme == "PS")
					{
						current_control_scheme = "PC";
					}
					else if (current_control_scheme == "PC")
					{
						current_control_scheme = "XBOX";
					}
					SetControlScheme();
				}
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press) || (Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				source.Play();
				if (settings_state == 0)
				{
					ChangeResolutionLeft();
				}
				else if (settings_state == 1)
				{
					ChangeFullscreen();
				}
				else if (settings_state == 2)
				{
					if (music_volume >= 1)
					{
						music_volume--;
					}
					else
					{
						music_volume = 10;
					}
					SetNewMusicVolume();
					UpdateMusicVolumeText();
				}
				else if (settings_state == 3)
				{
					if (effects_volume >= 1)
					{
						effects_volume--;
					}
					else
					{
						effects_volume = 10;
					}
					SetNewEffectsVolume();
					UpdateEffectsVolumeText();
				}
				else if (settings_state == 4)
				{
					source.Play();
					if (PlayerPrefs.HasKey("crt_status"))
					{
						if (PlayerPrefs.GetString("crt_status") == "disabled")
						{
							PlayerPrefs.SetString("crt_status", "enabled");
						}
						else
						{
							PlayerPrefs.SetString("crt_status", "disabled");
						}
					}
					UpdateCrtFilter();
					UpdateCrtFilterText();
				}
				else if (settings_state == 5)
				{
					ChangeLanguageLeft();
				}
				else if (settings_state == 6)
				{
					source.Play();
					if (current_control_scheme == "XBOX")
					{
						current_control_scheme = "PC";
					}
					else if (current_control_scheme == "PS")
					{
						current_control_scheme = "XBOX";
					}
					else if (current_control_scheme == "PC")
					{
						current_control_scheme = "PS";
					}
					SetControlScheme();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
			{
				source.Play();
				if (settings_state == 0)
				{
					ChangeResolutionRight();
				}
				else if (settings_state == 1)
				{
					ChangeFullscreen();
				}
				else if (settings_state == 2)
				{
					if (music_volume >= 1)
					{
						music_volume--;
					}
					else
					{
						music_volume = 10;
					}
					SetNewMusicVolume();
					UpdateMusicVolumeText();
				}
				else if (settings_state == 3)
				{
					if (effects_volume >= 1)
					{
						effects_volume--;
					}
					else
					{
						effects_volume = 10;
					}
					SetNewEffectsVolume();
					UpdateEffectsVolumeText();
				}
				else if (settings_state == 4)
				{
					source.Play();
					if (PlayerPrefs.HasKey("tube_status"))
					{
						if (PlayerPrefs.GetString("tube_status") == "disabled")
						{
							PlayerPrefs.SetString("tube_status", "enabled");
						}
						else
						{
							PlayerPrefs.SetString("tube_status", "disabled");
						}
					}
					UpdateCrtFilter();
					UpdateCrtFilterText();
				}
				else if (settings_state == 5)
				{
					ChangeLanguageRight();
				}
				else if (settings_state == 6)
				{
					source.Play();
					settings_panel.SetActive(false);
					settings_state = 1;
					state = "main";
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("ControllerB"))
			{
				source.Play();
				for (int i = 0; i <= 6; i++)
				{
					setting_texts[i].color = light_color;
					setting_values[i].color = light_color;
				}
				settings_panel.SetActive(false);
				settings_state = 1;
				state = "main";
			}
			else if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("ControllerRightTrigger"))
			{
				source.Play();
				state = "controls";
				settings_state = 0;
				setting_texts[0].color = dark_color;
				setting_values[0].color = dark_color;
				for (int j = 0; j <= 6; j++)
				{
					setting_texts[j].color = light_color;
					setting_values[j].color = light_color;
				}
				settings_left_pointer.transform.localPosition = settings_left_pointer_position;
				settings_right_pointer.transform.localPosition = settings_right_pointer_position;
				settings_back_panel.transform.localPosition = settings_back_panel_position;
				settings_panel.SetActive(false);
				controls_panel.SetActive(true);
			}
		}
		else
		{
			if (!(state == "controls"))
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("ControllerLeftTrigger"))
			{
				source.Play();
				state = "settings";
				settings_state = 0;
				settings_panel.SetActive(true);
				controls_panel.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("ControllerB"))
			{
				source.Play();
				for (int k = 0; k <= 6; k++)
				{
					setting_texts[k].color = light_color;
					setting_values[k].color = light_color;
				}
				controls_panel.SetActive(false);
				settings_state = 1;
				state = "main";
			}
		}
	}

	private void ChangeLanguageRight()
	{
		if (PlayerPrefs.HasKey("language"))
		{
			if (PlayerPrefs.GetString("language") == "english")
			{
				PlayerPrefs.SetString("language", "russian");
			}
			else if (PlayerPrefs.GetString("language") == "russian")
			{
				PlayerPrefs.SetString("language", "english");
			}
		}
		else
		{
			PlayerPrefs.SetString("language", "english");
		}
		GetComponent<LocalizationMainMenu>().Translate();
		UpdateCrtFilterText();
		UpdateLanguageText();
		StartCoroutine("SetFullscreenDelay");
		UpdateMusicVolumeText();
		UpdateEffectsVolumeText();
	}

	private void ChangeLanguageLeft()
	{
		if (PlayerPrefs.HasKey("language"))
		{
			if (PlayerPrefs.GetString("language") == "english")
			{
				PlayerPrefs.SetString("language", "russian");
			}
			else if (PlayerPrefs.GetString("language") == "russian")
			{
				PlayerPrefs.SetString("language", "english");
			}
		}
		else
		{
			PlayerPrefs.SetString("language", "english");
		}
		GetComponent<LocalizationMainMenu>().Translate();
		UpdateCrtFilterText();
		UpdateLanguageText();
		StartCoroutine("SetFullscreenDelay");
		UpdateMusicVolumeText();
		UpdateEffectsVolumeText();
	}

	private void ChangeFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
		StartCoroutine("SetFullscreenDelay");
	}

	private void ChangeResolutionRight()
	{
		if (current_resolution_cell == 14)
		{
			current_resolution_cell = 0;
		}
		else
		{
			current_resolution_cell++;
		}
		PlayerPrefs.SetInt("resolution_width", width[current_resolution_cell]);
		PlayerPrefs.SetInt("resolution_height", height[current_resolution_cell]);
		Screen.SetResolution(width[current_resolution_cell], height[current_resolution_cell], Screen.fullScreen);
		UpdateResolutionText();
	}

	private void ChangeResolutionLeft()
	{
		if (current_resolution_cell == 0)
		{
			current_resolution_cell = 14;
		}
		else
		{
			current_resolution_cell--;
		}
		PlayerPrefs.SetInt("resolution_width", width[current_resolution_cell]);
		PlayerPrefs.SetInt("resolution_height", height[current_resolution_cell]);
		Screen.SetResolution(width[current_resolution_cell], height[current_resolution_cell], Screen.fullScreen);
		UpdateResolutionText();
	}

	private void UpdateCrtFilterText()
	{
		if (PlayerPrefs.HasKey("crt_status"))
		{
			if (PlayerPrefs.GetString("crt_status") == "disabled")
			{
				setting_values[4].text = off_phrase;
			}
			else
			{
				setting_values[4].text = on_phrase;
			}
		}
	}

	private void UpdateMusicVolumeText()
	{
		if (PlayerPrefs.HasKey("music_volume"))
		{
			setting_values[2].text = PlayerPrefs.GetInt("music_volume").ToString();
		}
	}

	private void UpdateEffectsVolumeText()
	{
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			setting_values[3].text = PlayerPrefs.GetInt("effects_volume").ToString();
		}
	}

	private void UpdateLanguageText()
	{
		if (PlayerPrefs.HasKey("language"))
		{
			setting_values[5].text = lang_phrase;
		}
	}

	private void UpdateResolutionText()
	{
		setting_values[0].text = width[current_resolution_cell] + "x" + height[current_resolution_cell];
	}

	private void UpdateCrtFilter()
	{
		if (PlayerPrefs.HasKey("crt_status"))
		{
			if (PlayerPrefs.GetString("crt_status") == "disabled")
			{
				Camera.main.GetComponent<OLDTVTube>().enabled = false;
			}
			else
			{
				Camera.main.GetComponent<OLDTVTube>().enabled = true;
			}
		}
	}

	private void SetNewMusicVolume()
	{
		PlayerPrefs.SetInt("music_volume", music_volume);
		GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.03f * (float)music_volume;
	}

	private void SetNewEffectsVolume()
	{
		PlayerPrefs.SetInt("effects_volume", effects_volume);
		GetComponent<AudioSource>().volume = 0.1f * (float)effects_volume;
	}

	private void StartNewGame()
	{
		SaveSystem.SaveData(data);
		loading_panel.SetActive(true);
		SceneManager.LoadScene("intro");
	}

	public void LoadLvl()
	{
		loading_panel.SetActive(true);
		SceneManager.LoadScene("lvl_" + data.lvl);
	}

	private void SetControlScheme()
	{
		switch (current_control_scheme)
		{
		case "XBOX":
			control_scheme.GetComponent<Image>().sprite = control_schemes[0];
			settings_button_icon.GetComponent<Image>().sprite = xbox_button_rb;
			back_button_icon.GetComponent<Image>().sprite = xbox_button_b;
			select_button_icon.GetComponent<Image>().sprite = xbox_button_a;
			back_controls_button_icon.GetComponent<Image>().sprite = xbox_button_b;
			controls_button_icon.GetComponent<Image>().sprite = xbox_button_lb;
			conf_back_icon.GetComponent<Image>().sprite = xbox_button_b;
			setting_values[6].text = "xbox";
			console_descr.SetActive(true);
			pc_descr.SetActive(false);
			PlayerPrefs.SetString("control_scheme", "XBOX");
			break;
		case "PS":
			control_scheme.GetComponent<Image>().sprite = control_schemes[1];
			settings_button_icon.GetComponent<Image>().sprite = ps_button_r1;
			back_button_icon.GetComponent<Image>().sprite = ps_button_cross;
			select_button_icon.GetComponent<Image>().sprite = ps_button_box;
			back_controls_button_icon.GetComponent<Image>().sprite = ps_button_cross;
			controls_button_icon.GetComponent<Image>().sprite = ps_button_l1;
			conf_back_icon.GetComponent<Image>().sprite = ps_button_cross;
			setting_values[6].text = "ps";
			console_descr.SetActive(true);
			pc_descr.SetActive(false);
			PlayerPrefs.SetString("control_scheme", "PS");
			break;
		case "PC":
			control_scheme.GetComponent<Image>().sprite = control_schemes[2];
			settings_button_icon.GetComponent<Image>().sprite = keyboard_button_tab;
			back_button_icon.GetComponent<Image>().sprite = keyboard_button_esc;
			select_button_icon.GetComponent<Image>().sprite = keyboard_button_enter;
			back_controls_button_icon.GetComponent<Image>().sprite = keyboard_button_esc;
			controls_button_icon.GetComponent<Image>().sprite = keyboard_button_tab;
			conf_back_icon.GetComponent<Image>().sprite = keyboard_button_esc;
			setting_values[6].text = "pc";
			console_descr.SetActive(false);
			pc_descr.SetActive(true);
			PlayerPrefs.SetString("control_scheme", "PC");
			break;
		}
	}

	private IEnumerator SetFullscreenDelay()
	{
		yield return new WaitForSeconds(0.1f);
		PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
		if (PlayerPrefs.HasKey("fullscreen"))
		{
			setting_values[1].text = ((PlayerPrefs.GetInt("fullscreen") != 1) ? off_phrase : on_phrase);
		}
		else
		{
			setting_values[1].text = ((!Screen.fullScreen) ? off_phrase : on_phrase);
		}
	}

	private void LoadGame()
	{
		SavedData savedData = SaveSystem.LoadData();
		data.fire_skill_1 = savedData.fire_skill_1;
		data.fire_skill_2 = savedData.fire_skill_2;
		data.lightning_skill_1 = savedData.lightning_skill_1;
		data.lightning_skill_2 = savedData.lightning_skill_2;
		data.frost_skill_1 = savedData.frost_skill_1;
		data.story_progress = savedData.story_progress;
		data.hp = savedData.hp;
		data.mana = savedData.mana;
		data.crystal = savedData.crystal;
		data.shards = savedData.shards;
		data.lvl = savedData.lvl;
		data.lvl_spawn = savedData.lvl_spawn;
		data.lvl_last = savedData.lvl_last;
		data.mp_1_sold = savedData.mp_1_sold;
		data.hp_1_sold = savedData.hp_1_sold;
		data.mp_2_sold = savedData.mp_2_sold;
		data.hp_2_sold = savedData.hp_2_sold;
		data.hp_shard_count = savedData.hp_shard_count;
		data.mp_shard_count = savedData.mp_shard_count;
		data.lvl_0_rooms = savedData.lvl_0_rooms;
		data.lvl_1_rooms = savedData.lvl_1_rooms;
		data.lvl_2_rooms = savedData.lvl_2_rooms;
		data.lvl_3_rooms = savedData.lvl_3_rooms;
		data.lvl_4_rooms = savedData.lvl_4_rooms;
		data.lvl_5_rooms = savedData.lvl_5_rooms;
		data.lvl_0_chests = savedData.lvl_0_chests;
		data.lvl_1_chests = savedData.lvl_1_chests;
		data.lvl_2_chests = savedData.lvl_2_chests;
		data.lvl_3_chests = savedData.lvl_3_chests;
		data.lvl_4_chests = savedData.lvl_4_chests;
		LoadLvl();
	}
}
