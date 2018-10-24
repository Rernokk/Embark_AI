using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseBase : StructureBase
{
	#region Variables
	protected Dictionary<Item, int> StorageDictionary = new Dictionary<Item, int>();

	[SerializeField]
	protected int storageItemSize;      //Total amount of unique items to store.

	[SerializeField]
	protected int storageItemQuantity;  //Total amount of units stored per item.

	#endregion

	#region Public Methods
	public bool StoreResource(Item resourceID, int quantity)
	{
		//Storage is full of unique items, and this resource does not exist in this storage container. OR, Item exists, but storage item quantity is full.
		if ((StorageDictionary.Count == storageItemSize && !StorageDictionary.ContainsKey(resourceID)) ||
					(StorageDictionary.ContainsKey(resourceID) && storageItemQuantity <= StorageDictionary[resourceID]))
		{
			return false;
		}

		if (!StorageDictionary.ContainsKey(resourceID))
		{
			StorageDictionary.Add(resourceID, 0);
		}

		StorageDictionary[resourceID] += quantity;
		return true;
	}

	public bool StorageRequest(Item resourceID)
	{
		return (StorageDictionary.ContainsKey(resourceID) && StorageDictionary[resourceID] < storageItemQuantity) || (!StorageDictionary.ContainsKey(resourceID) && storageItemSize > StorageDictionary.Count);
	}
	#endregion

	#region Private Methods
	protected virtual void Start()
	{
		StorageDictionary = new Dictionary<Item, int>();
		StructureManager.instance.AddWarehouseStructure(this);
		Reposition();
	}


	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.F))
		{
			PrintContents();
		}
	}

	protected void PrintContents()
	{
		print("Printing contents of warehouse:");
		foreach (Item type in StorageDictionary.Keys)
		{
			print(transform.name + ", " + type + ": " + StorageDictionary[type]);
		}
	}
	#endregion
}
