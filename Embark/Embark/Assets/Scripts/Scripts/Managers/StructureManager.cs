using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour {
	#region Variables
	public static StructureManager instance;
	List<WarehouseBase> civilizationWarehouses = new List<WarehouseBase>();
	#endregion

	#region Properties
	public List<WarehouseBase> Warehouses {
		get {
			return civilizationWarehouses;
		}
	}
	#endregion

	#region Public Methods
	public void AddWarehouseStructure(WarehouseBase warehouse){
		if (!civilizationWarehouses.Contains(warehouse))
		{
			civilizationWarehouses.Add(warehouse);
		}
	}

	public List<WarehouseBase> FetchNearestWarehouse(Vector3 position){
		List<WarehouseBase> tempList = civilizationWarehouses;
		tempList.Sort((q1, q2) => Vector3.Distance(q1.transform.position, position).CompareTo(Vector3.Distance(q2.transform.position, position)));
		return tempList;
	}
	#endregion

	#region Private Methods
	void Awake(){
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else {
			instance = this;
		}
	}
	#endregion
}
