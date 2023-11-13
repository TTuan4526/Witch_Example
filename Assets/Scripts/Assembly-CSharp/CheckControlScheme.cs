using UnityEngine;
using UnityEngine.UI;

public class CheckControlScheme : MonoBehaviour
{
	private WorldScript world_script;

	public Sprite xbox_button_a;

	public Sprite xbox_button_b;

	public Sprite xbox_button_x;

	public Sprite xbox_button_y;

	public Sprite xbox_button_rt;

	public Sprite xbox_button_lt;

	public Sprite ps_button_cross;

	public Sprite ps_button_round;

	public Sprite ps_button_box;

	public Sprite ps_button_triangle;

	public Sprite ps_button_r1;

	public Sprite ps_button_l1;

	public Sprite keyboard_button_f;

	public Sprite keyboard_button_esc;

	public Sprite keyboard_button_q;

	public Sprite keyboard_button_e;

	public Sprite keyboard_button_r;

	public Sprite keyboard_button_enter;

	public Sprite keyboard_button_space;

	public Sprite keyboard_button_shift;

	public Sprite keyboard_button_a;

	public Sprite keyboard_button_d;

	public void SetControlButtons(string control_scheme)
	{
		world_script = GetComponent<WorldScript>();
		switch (control_scheme)
		{
		case "XBOX":
			world_script.ui_skill_book_select_button.GetComponent<Image>().sprite = xbox_button_a;
			world_script.ui_skill_book_back_button.GetComponent<Image>().sprite = xbox_button_b;
			world_script.ui_travel_button.GetComponent<Image>().sprite = xbox_button_a;
			world_script.ui_travel_exit_button.GetComponent<Image>().sprite = xbox_button_b;
			world_script.ui_close_book_button.GetComponent<Image>().sprite = xbox_button_b;
			world_script.ui_spell_page_button.GetComponent<Image>().sprite = xbox_button_rt;
			world_script.ui_spell_map_button.GetComponent<Image>().sprite = xbox_button_rt;
			world_script.ui_spell_main_button.GetComponent<Image>().sprite = xbox_button_lt;
			world_script.ui_map_spell_button.GetComponent<Image>().sprite = xbox_button_lt;
			break;
		case "PS":
			world_script.ui_skill_book_select_button.GetComponent<Image>().sprite = ps_button_box;
			world_script.ui_skill_book_back_button.GetComponent<Image>().sprite = ps_button_cross;
			world_script.ui_travel_button.GetComponent<Image>().sprite = ps_button_box;
			world_script.ui_travel_exit_button.GetComponent<Image>().sprite = ps_button_cross;
			world_script.ui_close_book_button.GetComponent<Image>().sprite = ps_button_cross;
			world_script.ui_spell_page_button.GetComponent<Image>().sprite = ps_button_r1;
			world_script.ui_spell_map_button.GetComponent<Image>().sprite = ps_button_r1;
			world_script.ui_spell_main_button.GetComponent<Image>().sprite = ps_button_l1;
			world_script.ui_map_spell_button.GetComponent<Image>().sprite = ps_button_l1;
			break;
		case "PC":
			world_script.ui_skill_book_select_button.GetComponent<Image>().sprite = keyboard_button_enter;
			world_script.ui_skill_book_back_button.GetComponent<Image>().sprite = keyboard_button_esc;
			world_script.ui_travel_button.GetComponent<Image>().sprite = keyboard_button_enter;
			world_script.ui_travel_exit_button.GetComponent<Image>().sprite = keyboard_button_esc;
			world_script.ui_close_book_button.GetComponent<Image>().sprite = keyboard_button_esc;
			world_script.ui_spell_page_button.GetComponent<Image>().sprite = keyboard_button_d;
			world_script.ui_spell_map_button.GetComponent<Image>().sprite = keyboard_button_d;
			world_script.ui_spell_main_button.GetComponent<Image>().sprite = keyboard_button_a;
			world_script.ui_map_spell_button.GetComponent<Image>().sprite = keyboard_button_a;
			break;
		}
	}
}
