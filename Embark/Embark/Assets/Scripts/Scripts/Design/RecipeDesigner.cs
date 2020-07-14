using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDesigner : MonoBehaviour
{
	#region Variables
	[SerializeField]
	Transform ItemHost;

	[SerializeField]
	Dropdown dropdownMenu;

	[SerializeField]
	Image selectedImage;

	[SerializeField]
	Transform selectedItemDisplay;

	[Header("------------")]
	[SerializeField]
	GameObject RecipeEntry;
	[SerializeField]
	GameObject IngredientEntry;

	List<Item> itemList;
	Item currentItem;

	#region Current Item Components
	InputField currentItemNameField;
	Text currentItemIDField;
	Button currentItemResetButton, currentItemUpdateButton, currentItemDeleteButton;
	Button currentItemUsesButton, currentItemRecipeButton;
	Transform currentItemRecipeField;
	bool ViewingUses = false;
	#endregion

	const string itemFilePath = "ItemDatabase.csv";
	#endregion

	#region Properties
	#endregion

	#region Public Methods

	public void CreateNewResource(InputField name)
	{
		for (int i = 0; i < itemList.Count; i++){
			if (itemList[i].ItemName == name.text){
				print("Duplicate Item Name Found.");
				return;
			}
		}

		Item newItem = new Item(name.text);
		int targetID = 0;
		itemList.Sort((q1, q2) => q1.ItemID.CompareTo(q2.ItemID));
		for (int i = 0; i < itemList.Count; i++)
		{
			if (targetID == itemList[i].ItemID)
			{
				targetID = itemList[i].ItemID + 1;
			} else {
				break;
			}
		}
		newItem.ItemID = targetID;
		newItem.ItemSprite = selectedImage.sprite.name;
		itemList.Add(newItem);
		SaveItemList();
	}

	public void SaveItemList()
	{
		string[] itemCSVFormat = new string[itemList.Count];
		for (int i = 0; i < itemList.Count; i++)
		{
			itemCSVFormat[i] = itemList[i].ItemName + "," + itemList[i].ItemID + "," + itemList[i].ItemSprite + "|";
			foreach (int myItem in itemList[i].Recipe.Keys){
				itemCSVFormat[i] += myItem + "," + itemList[i].Recipe[myItem].ToString() + ".";
			}
		}
		File.WriteAllLines(itemFilePath, itemCSVFormat);
		LoadItemList();
	}

	public void LoadItemList()
	{

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
		UpdateListDisplay();
	}

	public void UpdateListDisplay(){
		for (int i = ItemHost.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(ItemHost.GetChild(i).gameObject);
		}

		foreach (Item item in itemList)
		{
			GameObject obj = Instantiate(RecipeEntry, ItemHost);
			obj.transform.Find("Item").GetComponent<Text>().text = item.ToString();
			obj.transform.name = item.ToString();
			ItemEntryData dat = obj.GetComponent<ItemEntryData>();
			dat.MyItem = item;
			dat.Designer = this;
		}
	}

	public void UpdateDisplayedPicture (){
		selectedImage.sprite = dropdownMenu.options[dropdownMenu.value].image;
	}

	public void FilterItems (InputField searchTerm){
		for (int i = 0; i < ItemHost.childCount; i++){
			if (ItemHost.GetChild(i).name.ToLower().IndexOf(searchTerm.text.ToLower()) == -1){
				ItemHost.GetChild(i).gameObject.SetActive(false);
			} else {
				ItemHost.GetChild(i).gameObject.SetActive(true);
			}
		}
	}

	public void DisplaySelectedItem (Item item){
		if (item != currentItem){
			ViewingUses = false;
		}

		//Name
		currentItemNameField.interactable = true;
		currentItemNameField.text = item.ItemName;
	
		//ID
		currentItemIDField.text = item.ItemID.ToString();

		//Clearing Old List
		for (int i = currentItemRecipeField.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(currentItemRecipeField.GetChild(i).gameObject);
		}

		if (!ViewingUses)
		{
			//Recipe Information
			foreach (int ingredient in item.Recipe.Keys)
			{
				Item retItem = FetchItemById(ingredient);
				if (retItem != null)
				{
					GameObject obj = Instantiate(IngredientEntry, currentItemRecipeField);
					obj.transform.Find("ItemName").GetComponent<Text>().text = retItem.ItemName;
					obj.transform.Find("ID").GetComponent<Text>().text = retItem.ItemID.ToString();
					obj.transform.Find("Quantity").GetComponent<InputField>().text = item.Recipe[ingredient].ToString();
					obj.transform.Find("Quantity").GetComponent<InputField>().interactable = true;
				}
			}
		} else {
			foreach (Item i in itemList){
				if (i.Recipe.ContainsKey(currentItem.ItemID)){
					GameObject obj = Instantiate(IngredientEntry, currentItemRecipeField);
					obj.transform.Find("ItemName").GetComponent<Text>().text = i.ItemName;
					obj.transform.Find("ID").GetComponent<Text>().text = i.ItemID.ToString();
					obj.transform.Find("Quantity").GetComponent<InputField>().text = i.Recipe[currentItem.ItemID].ToString();
					obj.transform.Find("Quantity").GetComponent<InputField>().interactable = false;
				}
			}
		}

		//Actions
		currentItemDeleteButton.interactable = true;
		currentItemResetButton.interactable = true;
		currentItemUpdateButton.interactable = true;
		currentItemUsesButton.interactable = !ViewingUses;
		currentItemRecipeButton.interactable = ViewingUses;
		
		currentItem = item;
	}

	public void UpdateSelectedItem (){
		
		//Get Item Slot
		int ind = itemList.IndexOf(currentItem);
		
		//Update current info
		currentItem.ItemName = currentItemNameField.text;
		currentItem.Recipe.Clear();
		for (int i = 0; i < currentItemRecipeField.childCount; i++) {
			currentItem.Recipe.Add(int.Parse(currentItemRecipeField.GetChild(i).Find("ID").GetComponent<Text>().text), int.Parse(currentItemRecipeField.GetChild(i).Find("Quantity").GetComponent<InputField>().text));
		}

		//Push to dictionary
		itemList[ind] = currentItem;

		//Save
		SaveItemList();
	}

	public void ResetSelectedItemChanges () {
		currentItemNameField.text = currentItem.ItemName;
		currentItemIDField.text = currentItem.ItemID.ToString();
	}

	public void DeleteSelectedItemFromList () {
		itemList.Remove(currentItem);

		//Name
		currentItemNameField.interactable = false;
		currentItemNameField.text = "";

		//ID
		currentItemIDField.text = "-";

		//Clear List
		for (int i = currentItemRecipeField.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(currentItemRecipeField.GetChild(i).gameObject);
		}

		foreach (Item i in itemList){
			if (i.Recipe.ContainsKey(currentItem.ItemID)){
				i.Recipe.Remove(currentItem.ItemID);
			}
		}


		//Actions
		currentItemDeleteButton.interactable = false;
		currentItemResetButton.interactable = false;
		currentItemUpdateButton.interactable = false;

		SaveItemList();
	}

	public void AddItemToRecipe(Item item){
		if (currentItem == null){
			return;
		}

		if (!currentItem.Recipe.ContainsKey(item.ItemID))
		{
			currentItem.Recipe.Add(item.ItemID, 1);
		}

		DisplaySelectedItem(currentItem);
	}

	public void ViewUses (){
		ViewingUses = true;
		DisplaySelectedItem(currentItem);
	}

	public void ViewRecipe (){
		ViewingUses = false;
		DisplaySelectedItem(currentItem);
	}
	#endregion

	#region Private Methods
	void Awake() {
		itemList = new List<Item>();
		Sprite[] sprArray = Resources.LoadAll<Sprite>("ItemIcons");
		dropdownMenu.options.Clear();
		for (int i = 0; i < sprArray.Length; i++) {
			dropdownMenu.options.Add(new Dropdown.OptionData(sprArray[i].name, sprArray[i]));
		}
		dropdownMenu.value = 0;
		UpdateDisplayedPicture();
	}

	void Start () {
		LoadItemList();
		currentItemNameField = selectedItemDisplay.Find("CurrentItem/InputField").GetComponent<InputField>();
		currentItemIDField = selectedItemDisplay.Find("ID_Row/Panel/IDNumber").GetComponent<Text>();
		currentItemResetButton = selectedItemDisplay.Find("Action_Row/Panel/Reset").GetComponent<Button>();
		currentItemUpdateButton = selectedItemDisplay.Find("Action_Row/Panel/Update").GetComponent<Button>();
		currentItemDeleteButton = selectedItemDisplay.Find("Action_Row/Panel/Delete").GetComponent<Button>();
		currentItemRecipeField = selectedItemDisplay.Find("Recipe_Data/Item_Field");
		currentItemRecipeButton = selectedItemDisplay.Find("Recipe_Data/Recipe_Row/Sources").GetComponent<Button>();
		currentItemUsesButton = selectedItemDisplay.Find("Recipe_Data/Recipe_Row/Uses").GetComponent<Button>();
	}

	Item FetchItemById(int id)
	{
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i].ItemID == id)
			{
				return itemList[i];
			}
		}
		return null;
	}
	#endregion
}
