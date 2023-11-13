using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldScript : MonoBehaviour
{
	public int this_lvl;

	public bool game_paused;

	public bool fly_open;

	public bool ready_to_fly;

	public bool map_opened;

	public bool fly_to_next;

	public GameObject loading_panel;

	public Text loading_text;

	public AudioClip pause_sound;

	public AudioClip respawn_sound;

	public GameObject pause_game_panel;

	public GameObject map_panel;

	public Transform menu_arrow;

	public GameObject ui_message_box;

	public Text ui_message_text;

	public Image ui_message_icon;

	public Text ui_resume_text;

	public Text ui_restart_text;

	public Text ui_exit_text;

	public GameObject ui_book;

	public GameObject ui_fire_skill_1;

	public GameObject ui_fire_skill_2;

	public GameObject ui_lightning_skill_1;

	public GameObject ui_lightning_skill_2;

	public GameObject ui_frost_skill_1;

	public Text ui_spell_description;

	public GameObject ui_skill_book_select_button;

	public Text ui_skill_book_button_select_text;

	public GameObject ui_skill_book_back_button;

	public Text ui_skill_book_button_back_text;

	public GameObject ui_close_book_button;

	public Text ui_close_book_text;

	public GameObject ui_spell_page_button;

	public Text ui_spell_page_text;

	public GameObject ui_spell_map_button;

	public Text ui_spell_map_text;

	public GameObject ui_spell_main_button;

	public Text ui_spell_main_text;

	public GameObject ui_map_spell_button;

	public Text ui_map_spell_text;

	public GameObject ui_crystal_icon;

	public Text ui_boss_name;

	public int crystal;

	public GameObject ui_travel_panel;

	public GameObject ui_travel_button;

	public GameObject ui_travel_exit_button;

	public Text ui_travel_text;

	public Text ui_travel_exit_text;

	public Text ui_location_text;

	public Text ui_location_name_text;

	public GameObject ui_travel_marker;

	public AudioClip UpgradeSound;

	public Transform[] locations;

	private Data data;

	private GameObject teleport_panel;

	private GameObject player;

	private GameObject this_skill;

	private AudioSource source;

	private AudioSource back_music;

	private Animator ui_icon_crystal;

	private Text crystal_text;

	private Text witch_soul_text;

	private Text forest_heart_text;

	private bool controller_x_axis_press;

	private bool controller_6_axis_press;

	private bool controller_y_axis_press;

	private bool controller_7_axis_press;

	private bool controller_8_axis_press;

	private bool dialog_started;

	private bool skill_book_opened;

	public string current_control_scheme = "XBOX";

	private int pause_menu_state;

	private int pause_book_state;

	private int shards;

	private int effects_volume = 5;

	private int music_volume = 5;

	private int current_location;

	private Vector3 menu_arrow_default_position;

	private void Start()
	{
		data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
		LoadSave();
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
		SetVolume();
		UpdateCrtFilter();
		GetComponent<Localization>().Translate();
		menu_arrow_default_position = menu_arrow.transform.localPosition;
		back_music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
		crystal_text = GameObject.Find("ui_crystal_value").GetComponent<Text>();
		ui_icon_crystal = GameObject.Find("ui_crystal_icon").GetComponent<Animator>();
		witch_soul_text = GameObject.Find("ui_witch_soul_value").GetComponent<Text>();
		forest_heart_text = GameObject.Find("ui_forest_heart_value").GetComponent<Text>();
		player = GameObject.FindGameObjectWithTag("Player");
		source = GetComponent<AudioSource>();
		SpawnHero();
		GetComponent<CheckControlScheme>().SetControlButtons(current_control_scheme);
	}

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || (game_paused && Input.GetButtonDown("ControllerB")) || (Input.GetButtonDown("ControllerSubmit") && current_control_scheme == "XBOX") || (Input.GetButtonDown("ControllerSubmitPS") && current_control_scheme == "PS") || (Input.GetButtonDown("ControllerSubmit") && current_control_scheme == "PC")) && !skill_book_opened && !dialog_started && !fly_open && !fly_to_next)
		{
			if (map_opened)
			{
				OpenMap();
			}
			PauseGame();
		}
		if (ready_to_fly && !fly_open)
		{
			if (Input.GetButtonDown("ControllerA") || Input.GetKeyDown(KeyCode.Space))
			{
				OpenFlyUI();
			}
		}
		else if (fly_open)
		{
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
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press)
				{
					controller_7_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				bool flag = false;
				while (!flag)
				{
					current_location++;
					if (current_location > data.lvl_last)
					{
						current_location = 0;
					}
					if (current_location == data.lvl)
					{
						current_location++;
					}
					if (current_location >= 0 && current_location <= data.lvl_last && current_location != data.lvl)
					{
						flag = true;
					}
				}
				ui_travel_marker.transform.position = new Vector3(locations[current_location].position.x, locations[current_location].position.y, ui_travel_marker.transform.position.z);
				ui_location_name_text.text = GetComponent<Localization>().GetLineDescription("location_name_" + current_location);
				ui_location_text.text = GetComponent<Localization>().GetLineDescription("location_" + current_location);
				source.PlayOneShot(pause_sound, 1f);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press)
				{
					controller_7_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				bool flag2 = false;
				while (!flag2)
				{
					current_location--;
					if (current_location < 0)
					{
						current_location = data.lvl_last;
					}
					if (current_location == data.lvl)
					{
						current_location--;
					}
					if (current_location >= 0 && current_location <= data.lvl_last && current_location != data.lvl)
					{
						flag2 = true;
					}
				}
				ui_travel_marker.transform.position = new Vector3(locations[current_location].position.x, locations[current_location].position.y, ui_travel_marker.transform.position.z);
				ui_location_name_text.text = GetComponent<Localization>().GetLineDescription("location_name_" + current_location);
				ui_location_text.text = GetComponent<Localization>().GetLineDescription("location_" + current_location);
				source.PlayOneShot(pause_sound, 1f);
			}
			else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("ControllerB"))
			{
				CloseFlyUI();
				source.PlayOneShot(pause_sound, 1f);
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
			{
				data.lvl_spawn = 9;
				data.lvl = current_location;
				SaveSystem.SaveData(GameObject.FindGameObjectWithTag("Data").GetComponent<Data>());
				LoadSceneAsyncronical("lvl_" + current_location);
				source.PlayOneShot(pause_sound, 1f);
			}
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
		if (pause_book_state == 1)
		{
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
				source.PlayOneShot(pause_sound);
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
				source.PlayOneShot(pause_sound);
				pause_menu_state--;
				menu_arrow.transform.localPosition = new Vector3(menu_arrow.transform.localPosition.x, menu_arrow.transform.localPosition.y + 64f, menu_arrow.transform.localPosition.z);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("ControllerRightTrigger") || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press)
				{
					controller_7_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				pause_book_state = 2;
				ui_book.GetComponent<Animator>().Play("ui_book_page_skills");
				menu_arrow.gameObject.SetActive(false);
				ui_resume_text.gameObject.SetActive(false);
				ui_restart_text.gameObject.SetActive(false);
				ui_exit_text.gameObject.SetActive(false);
				ui_skill_book_select_button.SetActive(true);
				ui_skill_book_button_select_text.gameObject.SetActive(true);
				ui_fire_skill_1.SetActive(true);
				ui_fire_skill_2.SetActive(true);
				ui_lightning_skill_1.SetActive(true);
				ui_lightning_skill_2.SetActive(true);
				ui_frost_skill_1.SetActive(true);
				ui_spell_page_button.SetActive(false);
				ui_spell_page_text.gameObject.SetActive(false);
				ui_spell_map_button.SetActive(true);
				ui_spell_map_text.gameObject.SetActive(true);
				ui_spell_main_button.SetActive(true);
				ui_spell_main_text.gameObject.SetActive(true);
				SetSkillsToDefault();
				source.PlayOneShot(pause_sound, 1f);
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
			{
				if (pause_menu_state == 0)
				{
					PauseGame();
				}
				else if (pause_menu_state == 1)
				{
					back_music.UnPause();
					player.GetComponent<PlayerController>().RestartAfterDeath(0.1f);
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
		else if (pause_book_state == 2)
		{
			if (skill_book_opened)
			{
				if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press))
				{
					if (Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press)
					{
						controller_7_axis_press = true;
					}
					else if (Input.GetAxis("ControllerAxis8") < -0.95f && !controller_8_axis_press)
					{
						controller_8_axis_press = true;
					}
					else if (Input.GetAxis("Vertical") < -0.95f && !controller_y_axis_press)
					{
						controller_y_axis_press = true;
					}
					if (this_skill.GetComponent<book_skill_navigation>().skill_on_down != null)
					{
						SelectSkill(this_skill.GetComponent<book_skill_navigation>().skill_on_down);
					}
				}
				else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press) || (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press))
				{
					if (Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press)
					{
						controller_7_axis_press = true;
					}
					else if (Input.GetAxis("ControllerAxis8") > 0.95f && !controller_8_axis_press)
					{
						controller_8_axis_press = true;
					}
					else if (Input.GetAxis("Vertical") > 0.95f && !controller_y_axis_press)
					{
						controller_y_axis_press = true;
					}
					if (this_skill.GetComponent<book_skill_navigation>().skill_on_top != null)
					{
						SelectSkill(this_skill.GetComponent<book_skill_navigation>().skill_on_top);
					}
				}
				else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("ControllerRightTrigger") || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press))
				{
					if (Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press)
					{
						controller_6_axis_press = true;
					}
					else if (Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press)
					{
						controller_7_axis_press = true;
					}
					else if (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press)
					{
						controller_x_axis_press = true;
					}
					if (this_skill.GetComponent<book_skill_navigation>().skill_on_right != null)
					{
						SelectSkill(this_skill.GetComponent<book_skill_navigation>().skill_on_right);
					}
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("ControllerLeftTrigger") || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press))
				{
					if (Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press)
					{
						controller_6_axis_press = true;
					}
					else if (Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press)
					{
						controller_7_axis_press = true;
					}
					else if (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press)
					{
						controller_x_axis_press = true;
					}
					if (this_skill.GetComponent<book_skill_navigation>().skill_on_left != null)
					{
						SelectSkill(this_skill.GetComponent<book_skill_navigation>().skill_on_left);
					}
				}
				else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("ControllerB"))
				{
					skill_book_opened = false;
					ui_spell_description.text = string.Empty;
					SetSkillsToDefault();
					source.PlayOneShot(pause_sound, 1f);
					ui_spell_description.gameObject.SetActive(false);
					ui_skill_book_select_button.SetActive(true);
					ui_skill_book_button_select_text.gameObject.SetActive(true);
					ui_skill_book_back_button.SetActive(false);
					ui_skill_book_button_back_text.gameObject.SetActive(false);
					ui_close_book_button.SetActive(true);
					ui_close_book_text.gameObject.SetActive(true);
					ui_spell_map_button.SetActive(true);
					ui_spell_map_text.gameObject.SetActive(true);
					ui_spell_main_button.SetActive(true);
					ui_spell_main_text.gameObject.SetActive(true);
				}
				else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
				{
					source.PlayOneShot(pause_sound, 1f);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("ControllerA"))
			{
				skill_book_opened = true;
				source.PlayOneShot(pause_sound, 1f);
				ui_skill_book_select_button.SetActive(false);
				ui_skill_book_button_select_text.gameObject.SetActive(false);
				ui_skill_book_back_button.SetActive(true);
				ui_skill_book_button_back_text.gameObject.SetActive(true);
				ui_spell_description.gameObject.SetActive(true);
				ui_close_book_button.SetActive(false);
				ui_close_book_text.gameObject.SetActive(false);
				ui_spell_map_button.SetActive(false);
				ui_spell_map_text.gameObject.SetActive(false);
				ui_spell_main_button.SetActive(false);
				ui_spell_main_text.gameObject.SetActive(false);
				OpenSkillPage();
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("ControllerLeftTrigger") || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press)
				{
					controller_7_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				pause_book_state = 1;
				ui_book.GetComponent<Animator>().Play("ui_book_page_main");
				menu_arrow.gameObject.SetActive(true);
				ui_resume_text.gameObject.SetActive(true);
				ui_restart_text.gameObject.SetActive(true);
				ui_exit_text.gameObject.SetActive(true);
				ui_skill_book_select_button.SetActive(false);
				ui_skill_book_button_select_text.gameObject.SetActive(false);
				ui_fire_skill_1.SetActive(false);
				ui_fire_skill_2.SetActive(false);
				ui_lightning_skill_1.SetActive(false);
				ui_lightning_skill_2.SetActive(false);
				ui_frost_skill_1.SetActive(false);
				ui_spell_description.gameObject.SetActive(false);
				ui_spell_page_button.SetActive(true);
				ui_spell_page_text.gameObject.SetActive(true);
				ui_spell_map_button.SetActive(false);
				ui_spell_map_text.gameObject.SetActive(false);
				ui_spell_main_button.SetActive(false);
				ui_spell_main_text.gameObject.SetActive(false);
				source.PlayOneShot(pause_sound, 1f);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("ControllerRightTrigger") || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press))
			{
				if (Input.GetAxis("ControllerAxis6") > 0.95f && !controller_6_axis_press)
				{
					controller_6_axis_press = true;
				}
				else if (Input.GetAxis("ControllerAxis7") > 0.95f && !controller_7_axis_press)
				{
					controller_7_axis_press = true;
				}
				else if (Input.GetAxis("Horizontal") > 0.95f && !controller_x_axis_press)
				{
					controller_x_axis_press = true;
				}
				pause_book_state = 3;
				ui_book.GetComponent<Animator>().Play("ui_book_page_map");
				OpenMap();
				ui_skill_book_select_button.SetActive(false);
				ui_skill_book_button_select_text.gameObject.SetActive(false);
				ui_fire_skill_1.SetActive(false);
				ui_fire_skill_2.SetActive(false);
				ui_lightning_skill_1.SetActive(false);
				ui_lightning_skill_2.SetActive(false);
				ui_frost_skill_1.SetActive(false);
				ui_spell_description.gameObject.SetActive(false);
				ui_spell_map_button.SetActive(false);
				ui_spell_map_text.gameObject.SetActive(false);
				ui_spell_main_button.SetActive(false);
				ui_spell_main_text.gameObject.SetActive(false);
				ui_map_spell_button.SetActive(true);
				ui_map_spell_text.gameObject.SetActive(true);
				source.PlayOneShot(pause_sound, 1f);
			}
		}
		else if (pause_book_state == 3 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("ControllerLeftTrigger") || (current_control_scheme == "XBOX" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PC" && Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press) || (current_control_scheme == "PS" && Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press) || (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press)))
		{
			if (Input.GetAxis("ControllerAxis6") < -0.95f && !controller_6_axis_press)
			{
				controller_6_axis_press = true;
			}
			else if (Input.GetAxis("ControllerAxis7") < -0.95f && !controller_7_axis_press)
			{
				controller_7_axis_press = true;
			}
			else if (Input.GetAxis("Horizontal") < -0.95f && !controller_x_axis_press)
			{
				controller_x_axis_press = true;
			}
			pause_book_state = 2;
			OpenMap();
			ui_book.GetComponent<Animator>().Play("ui_book_page_skills");
			ui_skill_book_select_button.SetActive(true);
			ui_skill_book_button_select_text.gameObject.SetActive(true);
			ui_fire_skill_1.SetActive(true);
			ui_fire_skill_2.SetActive(true);
			ui_lightning_skill_1.SetActive(true);
			ui_lightning_skill_2.SetActive(true);
			ui_frost_skill_1.SetActive(true);
			ui_spell_map_button.SetActive(true);
			ui_spell_map_text.gameObject.SetActive(true);
			ui_spell_main_button.SetActive(true);
			ui_spell_main_text.gameObject.SetActive(true);
			ui_map_spell_button.SetActive(false);
			ui_map_spell_text.gameObject.SetActive(false);
			SetSkillsToDefault();
			source.PlayOneShot(pause_sound, 1f);
		}
	}

	private void OpenMap()
	{
		map_opened = !map_opened;
		if (map_opened)
		{
			map_panel.SetActive(true);
			GameObject[] array = GameObject.FindGameObjectsWithTag("room");
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				switch (this_lvl)
				{
				case 0:
					if (data.lvl_0_rooms[gameObject.GetComponent<ui_room>().room_number])
					{
						gameObject.GetComponent<ui_room>().Open();
					}
					else
					{
						gameObject.GetComponent<ui_room>().Close();
					}
					break;
				case 1:
					if (data.lvl_1_rooms[gameObject.GetComponent<ui_room>().room_number])
					{
						gameObject.GetComponent<ui_room>().Open();
					}
					else
					{
						gameObject.GetComponent<ui_room>().Close();
					}
					break;
				case 2:
					if (data.lvl_2_rooms[gameObject.GetComponent<ui_room>().room_number])
					{
						gameObject.GetComponent<ui_room>().Open();
					}
					else
					{
						gameObject.GetComponent<ui_room>().Close();
					}
					break;
				case 3:
					if (data.lvl_3_rooms[gameObject.GetComponent<ui_room>().room_number])
					{
						gameObject.GetComponent<ui_room>().Open();
					}
					else
					{
						gameObject.GetComponent<ui_room>().Close();
					}
					break;
				case 4:
					if (data.lvl_4_rooms[gameObject.GetComponent<ui_room>().room_number])
					{
						gameObject.GetComponent<ui_room>().Open();
					}
					else
					{
						gameObject.GetComponent<ui_room>().Close();
					}
					break;
				case 5:
					if (data.lvl_5_rooms[gameObject.GetComponent<ui_room>().room_number])
					{
						gameObject.GetComponent<ui_room>().Open();
					}
					else
					{
						gameObject.GetComponent<ui_room>().Close();
					}
					break;
				}
			}
		}
		else
		{
			map_panel.SetActive(false);
		}
	}

	public void GetMPShard()
	{
		data.mp_shard_count++;
		if (data.mp_shard_count >= 4 && data.mana < 4)
		{
			data.mp_shard_count = 0;
			data.mana++;
			source.PlayOneShot(UpgradeSound);
			player.GetComponent<PlayerController>().SetMaxMana(data.mana);
			ShowMessage("mp_increased");
		}
		else
		{
			ShowMessage("mp_shard_get");
		}
		witch_soul_text.text = data.mp_shard_count.ToString();
	}

	public void GetHPShard()
	{
		data.hp_shard_count++;
		if (data.hp_shard_count >= 4 && data.hp < 7)
		{
			data.hp_shard_count = 0;
			data.hp++;
			source.PlayOneShot(UpgradeSound);
			player.GetComponent<PlayerController>().SetMaxHP(data.hp);
			ShowMessage("hp_increased");
		}
		else
		{
			ShowMessage("hp_shard_get");
		}
		forest_heart_text.text = data.hp_shard_count.ToString();
	}

	private void OpenSkillPage()
	{
		SetSkillsToDefault();
		this_skill = ui_fire_skill_1;
		SpellDescription(this_skill.name.Substring(3));
		if (data.fire_skill_1)
		{
			ui_fire_skill_1.GetComponent<Animator>().Play("ui_fire_skill_1_learned_active");
		}
		else
		{
			ui_fire_skill_1.GetComponent<Animator>().Play("ui_fire_skill_1_unlearned_active");
		}
	}

	private void SetSkillsToDefault()
	{
		if (data.fire_skill_1)
		{
			ui_fire_skill_1.GetComponent<Animator>().Play("ui_fire_skill_1_learned_inactive");
		}
		else
		{
			ui_fire_skill_1.GetComponent<Animator>().Play("ui_fire_skill_1_unlearned_inactive");
		}
		if (data.fire_skill_2)
		{
			ui_fire_skill_2.GetComponent<Animator>().Play("ui_fire_skill_2_learned_inactive");
		}
		else
		{
			ui_fire_skill_2.GetComponent<Animator>().Play("ui_fire_skill_2_unlearned_inactive");
		}
		if (data.lightning_skill_1)
		{
			ui_lightning_skill_1.GetComponent<Animator>().Play("ui_lightning_skill_1_learned_inactive");
		}
		else
		{
			ui_lightning_skill_1.GetComponent<Animator>().Play("ui_lightning_skill_1_unlearned_inactive");
		}
		if (data.lightning_skill_2)
		{
			ui_lightning_skill_2.GetComponent<Animator>().Play("ui_lightning_skill_2_learned_inactive");
		}
		else
		{
			ui_lightning_skill_2.GetComponent<Animator>().Play("ui_lightning_skill_2_unlearned_inactive");
		}
		if (data.frost_skill_1)
		{
			ui_frost_skill_1.GetComponent<Animator>().Play("ui_frost_skill_1_learned_inactive");
		}
		else
		{
			ui_frost_skill_1.GetComponent<Animator>().Play("ui_frost_skill_1_unlearned_inactive");
		}
	}

	private void SelectSkill(GameObject selected)
	{
		string text = this_skill.name.Substring(3);
		if (data.GetComponent<Data>().GetSkillValue(text))
		{
			this_skill.GetComponent<Animator>().Play("ui_" + text + "_learned_inactive");
		}
		else
		{
			this_skill.GetComponent<Animator>().Play("ui_" + text + "_unlearned_inactive");
		}
		string text2 = selected.name.Substring(3);
		if (data.GetComponent<Data>().GetSkillValue(text2))
		{
			selected.GetComponent<Animator>().Play("ui_" + text2 + "_learned_active");
		}
		else
		{
			selected.GetComponent<Animator>().Play("ui_" + text2 + "_unlearned_active");
		}
		this_skill = selected;
		SpellDescription(text2);
	}

	private void SpellDescription(string skill_name)
	{
		ui_spell_description.text = GetComponent<Localization>().GetLineDescription(skill_name);
	}

	public void PauseGame()
	{
		game_paused = !game_paused;
		if (game_paused)
		{
			Time.timeScale = 0f;
			player.GetComponent<PlayerController>().StopMoving();
			player.GetComponent<PlayerController>().Block();
			source.PlayOneShot(pause_sound);
			pause_game_panel.SetActive(true);
			pause_book_state = 3;
			OpenMap();
			ui_book.GetComponent<Animator>().Play("ui_book_page_map");
			ui_resume_text.gameObject.SetActive(false);
			ui_restart_text.gameObject.SetActive(false);
			ui_exit_text.gameObject.SetActive(false);
			menu_arrow.gameObject.SetActive(false);
			ui_lightning_skill_1.SetActive(false);
			ui_lightning_skill_2.SetActive(false);
			ui_frost_skill_1.SetActive(false);
			ui_fire_skill_1.SetActive(false);
			ui_fire_skill_2.SetActive(false);
			ui_skill_book_select_button.SetActive(false);
			ui_skill_book_button_select_text.gameObject.SetActive(false);
			ui_skill_book_back_button.SetActive(false);
			ui_skill_book_button_back_text.gameObject.SetActive(false);
			ui_close_book_button.SetActive(true);
			ui_close_book_text.gameObject.SetActive(true);
			ui_spell_page_button.SetActive(false);
			ui_spell_page_text.gameObject.SetActive(false);
			ui_spell_map_button.SetActive(false);
			ui_spell_map_text.gameObject.SetActive(false);
			ui_spell_main_button.SetActive(false);
			ui_spell_main_text.gameObject.SetActive(false);
			ui_map_spell_button.SetActive(true);
			ui_map_spell_text.gameObject.SetActive(true);
		}
		else
		{
			Time.timeScale = 1f;
			player.GetComponent<PlayerController>().paused = false;
			player.GetComponent<PlayerController>().Unblock();
			source.PlayOneShot(pause_sound, 1f);
			pause_game_panel.SetActive(false);
			menu_arrow.transform.localPosition = menu_arrow_default_position;
		}
	}

	public void ReSpawnHero()
	{
		SpawnHero();
	}

	public void SpawnHero()
	{
		teleport_panel = GameObject.Find("ui_teleport_panel");
		teleport_panel.GetComponent<CanvasGroup>().alpha = 0.75f;
		foreach (Transform item in GameObject.Find("Terrain").transform)
		{
			item.GetComponent<SpriteRenderer>().enabled = true;
			foreach (Transform item2 in item)
			{
				item2.gameObject.SetActive(true);
			}
		}
		Vector3 position = GameObject.Find("player_spawn_" + data.lvl_spawn).transform.position;
		Vector3 position2 = GameObject.Find("camera_spawn_" + data.lvl_spawn).transform.position;
		player.transform.position = new Vector3(position.x, position.y, -3f);
		Camera.main.transform.position = new Vector3(position2.x, position2.y, -10f);
		if (this_lvl == 0 && data.story_progress <= 8)
		{
			GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().StartChapter();
		}
		else if (this_lvl == 1 && data.story_progress <= 12)
		{
			GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().StartChapter();
		}
		else if (this_lvl == 2 && data.story_progress <= 16)
		{
			GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().StartChapter();
		}
		else if (this_lvl == 3 && data.story_progress <= 19)
		{
			GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().StartChapter();
		}
		else if (this_lvl == 4 && data.story_progress <= 22)
		{
			GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().StartChapter();
		}
		else if (this_lvl == 5 && data.story_progress <= 27)
		{
			GameObject.FindGameObjectWithTag("chapter").GetComponent<ChapterStart>().StartChapter();
		}
		player.GetComponent<PlayerController>().this_room = GameObject.Find("camera_spawn_" + data.lvl_spawn);
		player.GetComponent<PlayerController>().this_room_number = GameObject.Find("camera_spawn_" + data.lvl_spawn).GetComponent<this_room>().room_number;
		player.GetComponent<PlayerController>().SetMaxHP(data.hp);
		player.GetComponent<PlayerController>().RestoreParameters();
		player.GetComponent<PlayerController>().SetMaxMana(data.mana);
		player.GetComponent<PlayerController>().SetMaxElement(data.fire_skill_1, data.fire_skill_2, data.lightning_skill_1, data.lightning_skill_2, data.frost_skill_1);
		foreach (Transform item3 in GameObject.Find("Terrain").transform)
		{
			if (!(player.GetComponent<PlayerController>().this_room.transform.parent != item3))
			{
				continue;
			}
			item3.GetComponent<SpriteRenderer>().enabled = false;
			foreach (Transform item4 in item3)
			{
				if (item4.name != "RespawnItems" && item4.name != "Mobs")
				{
					item4.gameObject.SetActive(false);
				}
			}
		}
		witch_soul_text.text = data.mp_shard_count.ToString();
		forest_heart_text.text = data.hp_shard_count.ToString();
		GameObject.Find("camera_spawn_" + data.lvl_spawn).GetComponent<this_room>().SpawnThemAll();
		SetCrystalAmount(data.crystal, data.shards);
		source.PlayOneShot(respawn_sound);
		player.GetComponent<PlayerController>().Block();
		player.GetComponent<Animator>().Play("witchy_respawn");
		StartCoroutine("FadeOutPanel");
	}

	public void GetCrystal(int amount)
	{
		crystal += amount;
		data.crystal = crystal;
		if (crystal >= 500)
		{
		}
		UpdateCrystalUI();
	}

	public void SpendCrystal(int amount)
	{
		crystal -= amount;
		data.crystal = crystal;
		UpdateCrystalUI();
	}

	public void GetCrystalShard()
	{
		shards++;
		if (shards >= 5)
		{
			GetCrystal(1);
			shards = 0;
		}
		data.shards = shards;
	}

	public void DialogStarted()
	{
		dialog_started = !dialog_started;
	}

	public void OpenFlyUI()
	{
		player.GetComponent<PlayerController>().Block();
		fly_open = true;
		if (this_lvl != 0)
		{
			ui_location_name_text.text = GetComponent<Localization>().GetLineDescription("location_name_0");
			ui_location_text.text = GetComponent<Localization>().GetLineDescription("location_0");
			ui_travel_marker.transform.position = new Vector3(locations[0].position.x, locations[0].position.y, ui_travel_marker.transform.position.z);
		}
		else
		{
			current_location = 1;
			ui_location_name_text.text = GetComponent<Localization>().GetLineDescription("location_name_1");
			ui_location_text.text = GetComponent<Localization>().GetLineDescription("location_1");
			ui_travel_marker.transform.position = new Vector3(locations[1].position.x, locations[1].position.y, ui_travel_marker.transform.position.z);
		}
		ui_travel_panel.SetActive(true);
	}

	public void CloseFlyUI()
	{
		ui_travel_panel.SetActive(false);
		fly_open = false;
		player.GetComponent<PlayerController>().Unblock();
	}

	private void UpdateCrystalUI()
	{
		crystal_text.text = crystal.ToString();
		ui_icon_crystal.Play("icon_crystal_get");
	}

	private void SetCrystalAmount(int crystal_amount, int shards_amount)
	{
		crystal = crystal_amount;
		shards = shards_amount;
		UpdateCrystalUI();
	}

	public void LoadSave()
	{
		SavedData savedData = SaveSystem.LoadData();
		data.lvl_spawn = savedData.lvl_spawn;
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
	}

	private void SetVolume()
	{
		if (PlayerPrefs.HasKey("music_volume"))
		{
			music_volume = PlayerPrefs.GetInt("music_volume");
		}
		if (PlayerPrefs.HasKey("effects_volume"))
		{
			effects_volume = PlayerPrefs.GetInt("effects_volume");
		}
		GetComponent<AudioSource>().volume = 0.1f * (float)effects_volume;
		GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.03f * (float)music_volume;
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

	public void HideUI()
	{
		GameObject.Find("ui_crystal_icon").SetActive(false);
		GameObject.Find("ui_crystal_value").SetActive(false);
		GameObject.Find("ui_forest_heart_icon").SetActive(false);
		GameObject.Find("ui_witch_soul_icon").SetActive(false);
		GameObject.Find("ui_forest_heart_value").SetActive(false);
		GameObject.Find("ui_witch_soul_value").SetActive(false);
		GameObject.Find("ui_hp_frame").SetActive(false);
		GameObject.Find("ui_elements_frame").SetActive(false);
	}

	public void ShowMessage(string spell_type)
	{
		ui_message_box.GetComponent<Animator>().Play("ui_message_box_open");
		source.PlayOneShot(UpgradeSound);
		switch (spell_type)
		{
		case "fire_skill_1_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().fire_spell_1, spell_type));
			break;
		case "fire_skill_2_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().fire_spell_2, spell_type));
			break;
		case "lightning_skill_1_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().lightning_spell_1, spell_type));
			break;
		case "lightning_skill_2_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().lightning_spell_2, spell_type));
			break;
		case "frost_skill_1_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().frost_spell_1, spell_type));
			break;
		case "hp_increased":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().hp, spell_type));
			break;
		case "mp_increased":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().mp, spell_type));
			break;
		case "hp_shard_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().hp, spell_type));
			break;
		case "mp_shard_get":
			StartCoroutine(ShowMessageEnumerator(GetComponent<SpellsIcons>().mp, spell_type));
			break;
		}
	}

	public void LoadSceneAsyncronical(string scene_name)
	{
		loading_panel.SetActive(true);
		SceneManager.LoadScene(scene_name);
	}

	private IEnumerator ShowMessageEnumerator(Sprite icon, string spell_type)
	{
		yield return new WaitForSeconds(0.4f);
		ui_message_icon.GetComponent<Image>().enabled = true;
		ui_message_icon.GetComponent<Image>().sprite = icon;
		yield return new WaitForSeconds(0.4f);
		ui_message_text.text = GetComponent<Localization>().GetLineDescription(spell_type);
		StartCoroutine("EndMessage");
	}

	private IEnumerator EndMessage()
	{
		yield return new WaitForSeconds(2.5f);
		ui_message_box.GetComponent<Animator>().Play("ui_message_box_hide");
		yield return new WaitForSeconds(0.5f);
		ui_message_icon.GetComponent<Image>().enabled = false;
		ui_message_text.text = string.Empty;
	}

	private IEnumerator FadeOutPanel()
	{
		float counter = 0f;
		while (counter < 0.5f)
		{
			counter += Time.deltaTime;
			teleport_panel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / 0.5f);
			yield return null;
		}
	}
}
