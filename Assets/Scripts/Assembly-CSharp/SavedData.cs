using System;

[Serializable]
public class SavedData
{
	public bool lvl_0_dialog_jake_0;

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

	public bool easy_mode;

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

	public SavedData(Data data)
	{
		fire_skill_1 = data.fire_skill_1;
		fire_skill_2 = data.fire_skill_2;
		lightning_skill_1 = data.lightning_skill_1;
		lightning_skill_2 = data.lightning_skill_2;
		frost_skill_1 = data.frost_skill_1;
		story_progress = data.story_progress;
		hp = data.hp;
		mana = data.mana;
		crystal = data.crystal;
		shards = data.shards;
		lvl = data.lvl;
		lvl_last = data.lvl_last;
		lvl_spawn = data.lvl_spawn;
		mp_1_sold = data.mp_1_sold;
		hp_1_sold = data.hp_1_sold;
		mp_2_sold = data.mp_2_sold;
		hp_2_sold = data.hp_2_sold;
		hp_shard_count = data.hp_shard_count;
		mp_shard_count = data.mp_shard_count;
		lvl_0_chests = data.lvl_0_chests;
		lvl_1_chests = data.lvl_1_chests;
		lvl_2_chests = data.lvl_2_chests;
		lvl_3_chests = data.lvl_3_chests;
		lvl_4_chests = data.lvl_4_chests;
		lvl_0_rooms = data.lvl_0_rooms;
		lvl_1_rooms = data.lvl_1_rooms;
		lvl_2_rooms = data.lvl_2_rooms;
		lvl_3_rooms = data.lvl_3_rooms;
		lvl_4_rooms = data.lvl_4_rooms;
		lvl_5_rooms = data.lvl_5_rooms;
	}
}
