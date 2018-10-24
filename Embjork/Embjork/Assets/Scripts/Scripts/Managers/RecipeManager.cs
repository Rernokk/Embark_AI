using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecipeManager : MonoBehaviour {
	const string itemFilePath = "ItemDatabase.csv";

	public static RecipeManager instance;

	List<Item> itemList;
	
	[SerializeField]
	int itemCount;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	void Start () {
		itemList = new List<Item>();
		if (!File.Exists(itemFilePath))
		{
			File.Create(itemFilePath).Close();
		}
		else
		{
			itemList.Clear();
			string[] itemArray = File.ReadAllLines(itemFilePath);
			foreach (string str in itemArray)
			{
				string[] craftingSplit = str.Split('|');
				string[] splitEntry = craftingSplit[0].Split(',');
				Item temp = new Item(splitEntry[0], int.Parse(splitEntry[1]), splitEntry[2]);

				if (craftingSplit[1] != "")
				{
					string[] recipeSplit = craftingSplit[1].Split('.');
					foreach (string ingredient in recipeSplit)
					{
						if (ingredient != "")
						{
							if (!temp.Recipe.ContainsKey(int.Parse(ingredient.Split(',')[0])))
							{
								temp.Recipe.Add(int.Parse(ingredient.Split(',')[0]), int.Parse(ingredient.Split(',')[1]));
							}
						}
					}
				}

				itemList.Add(temp);
			}
		}

		itemCount = itemList.Count;
	}

	public Item FetchItemByID(int id){
		foreach (Item item in itemList){
			if (item.ItemID == id){
				return item;
			}
		}
		return null;
	}
}
	