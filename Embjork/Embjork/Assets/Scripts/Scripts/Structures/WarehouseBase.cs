using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseBase : MonoBehaviour {
	#region Variables
	protected Dictionary<RESOURCETYPE, int> StorageDictionary = new Dictionary<RESOURCETYPE, int>();

	[SerializeField]
	protected int storageItemSize;			//Total amount of unique items to store.

	[SerializeField]
	protected int storageItemQuantity;  //Total amount of units stored per item.

	[SerializeField]
	protected bool positionedCorrectly = false;
	#endregion

	#region Public Methods
	public bool StoreResource(RESOURCETYPE resource, int quantity) {
		//Storage is full of unique items, and this resource does not exist in this storage container. OR, Item exists, but storage item quantity is full.
		if ((StorageDictionary.Count == storageItemSize && !StorageDictionary.ContainsKey(resource)) ||
					(StorageDictionary.ContainsKey(resource) && storageItemQuantity <= StorageDictionary[resource])){
			return false;
		}

		if (!StorageDictionary.ContainsKey(resource)){
			StorageDictionary.Add(resource, 0);
		}

		StorageDictionary[resource] += quantity;
		return true;
	}

	public bool StorageRequest(RESOURCETYPE resource){
		return (StorageDictionary.ContainsKey(resource) && StorageDictionary[resource] < storageItemQuantity) || (!StorageDictionary.ContainsKey(resource) && storageItemSize > StorageDictionary.Count);
	}
	#endregion

	#region Private Methods
	protected virtual void Start()
	{
		StorageDictionary = new Dictionary<RESOURCETYPE, int>();
		StructureManager.instance.AddWarehouseStructure(this);
		Reposition();
	}

	protected virtual void Reposition(){
		if (PathManager.instance.WorldIsReady)
		{
			Vector3 repos = transform.position;
			repos.x = Mathf.FloorToInt(repos.x);
			repos.z = Mathf.FloorToInt(repos.z);
			//repos.y = PathManager.instance.GetPointHeight(repos);
			repos.y = WorldManager.instance.HeightAtPosition(transform.position);
			transform.position = repos;
			positionedCorrectly = true;
		}
	}

	protected virtual void Update()
	{
		if (!positionedCorrectly){
			Reposition();
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			PrintContents();
		}
	}
	protected void PrintContents()
	{
		print("Printing contents of warehouse:");
		foreach (RESOURCETYPE type in StorageDictionary.Keys)
		{
			print(transform.name + ", " + type + ": " + StorageDictionary[type]);
		}
	}
	#endregion
}
