using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntryData : MonoBehaviour
{
	Item myItem = null;
	RecipeDesigner designer;

	public Item MyItem
	{
		get
		{
			return myItem;
		}

		set
		{
			myItem = value;
		}
	}

	public RecipeDesigner Designer {
		get {
			return designer;
		}

		set {
			designer = value;
		}
	}

	public void SelectItem (){
		designer.DisplaySelectedItem(MyItem);
	}

	public void AddItemToRecipe(){
		designer.AddItemToRecipe(MyItem);
	}
}
