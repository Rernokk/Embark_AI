using System.Collections;
using System.Collections.Generic;

public class Item {
	#region Variables
	private string itemName;
	private int itemID;
	private string itemSprite;
	Dictionary<int, int> recipe = new Dictionary<int, int>();
	#endregion

	#region Properties

	public string ItemName {
		get {
			return itemName;
		}

		set {
			itemName = value;
		}
	}

	public int ItemID {
		get {
			return itemID;
		}

		set {
			itemID = value;
		}
	}

	public string ItemSprite {
		get {
			return itemSprite;
		}

		set {
			itemSprite = value;
		}
	}
	#endregion

	#region Public Methods
	public Item() {
		itemName = "Unnamed";
		itemID = -999;
	}

	public Item (string name){
		itemName = name;
		itemID = -999;
	}

	public Item (string name, int ID){
		itemName = name;
		itemID = ID;
	}

	public Item (string name, int ID, string sprite)
	{
		itemName = name;
		itemID = ID;
		itemSprite = sprite;
	}

	public Dictionary<int, int> Recipe
	{
		get
		{
			return recipe;
		}
		set
		{
			recipe = value;
		}
	}

	public override string ToString()
	{
		return ItemName + ", " + ItemID;
	}
	#endregion

	#region Private Methods

	#endregion
}
