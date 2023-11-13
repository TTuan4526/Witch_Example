using System.Reflection;
using UnityEngine;

public class Data : MonoBehaviour
{
	private static Data instance;

	public bool fire_skill_1;

	public bool fire_skill_2;

	public bool lightning_skill_1;

	public bool lightning_skill_2;

	public bool frost_skill_1;

	public int story_progress;

	public int hp;

	public int mana;

	public int crystal;

	public int shards;

	public int lvl;

	public int lvl_spawn;

	public int lvl_last;

	public int hp_shard_count;

	public int mp_shard_count;

	public bool mp_1_sold;

	public bool hp_1_sold;

	public bool mp_2_sold;

	public bool hp_2_sold;

	public bool[] lvl_0_chests = new bool[3];

	public bool[] lvl_1_chests = new bool[3];

	public bool[] lvl_2_chests = new bool[4];

	public bool[] lvl_3_chests = new bool[3];

	public bool[] lvl_4_chests = new bool[3];

	public bool[] lvl_0_rooms = new bool[39];

	public bool[] lvl_1_rooms = new bool[39];

	public bool[] lvl_2_rooms = new bool[50];

	public bool[] lvl_3_rooms = new bool[47];

	public bool[] lvl_4_rooms = new bool[46];

	public bool[] lvl_5_rooms = new bool[30];

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

	public bool GetSkillValue(string name)
	{
		Data component = base.gameObject.GetComponent<Data>();
		FieldInfo field = component.GetType().GetField(name);
		return (bool)field.GetValue(component);
	}
}
