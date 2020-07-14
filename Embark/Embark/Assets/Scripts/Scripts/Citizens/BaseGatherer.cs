using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GatheringState { Approach, Gather, Store };

public class BaseGatherer : BaseCitizen
{
	#region Variables
	[SerializeField]
	int resourceCount = 0, resourceLimit = 10;

	float resourceCyclesPerSecond = 1f;
	float resourceTimer = 0f;

	bool hasPickedTarget = false;

	[SerializeField]
	Item currentResourceType = null;

	[SerializeField]
	GatheringState currentState = GatheringState.Approach;

	BaseResourceNode resourceTarget;
	WarehouseBase myWarehouse;
	#endregion

	#region Private Methods
	protected override void Start()
	{
		currentState = GatheringState.Approach;
		GetResourceTarget();
		GetWarehouse();
	}

	protected override void Update()
	{
		//Navigate Towards Target until in range.
		if (currentState == GatheringState.Approach)
		{
			ApproachItem();
		}

		//Once in Range, gather unit
		else if (currentState == GatheringState.Gather)
		{
			GatherResource();
		}

		//Inventory Full, return to warehouse.
		else if (currentState == GatheringState.Store)
		{
			StoreItem();
		}
	}

	protected void GetResourceTarget()
	{
		GameObject[] objArray = GameObject.FindGameObjectsWithTag("ResourceNode");
		resourceTarget = objArray[Random.Range(0, objArray.Length)].GetComponent<BaseResourceNode>();
	}

	protected void GetWarehouse()
	{
		GameObject[] objArray = GameObject.FindGameObjectsWithTag("Warehouse");
		myWarehouse = objArray[Random.Range(0, objArray.Length)].GetComponent<WarehouseBase>();
	}

	protected void GatherResource(){
		if (resourceTimer >= 1.0f / resourceCyclesPerSecond)
		{
			if (currentResourceType == null)
			{
				currentResourceType = resourceTarget.ResourceItem;
			}
			resourceTimer -= 1.0f / resourceCyclesPerSecond;
			resourceCount++;
			if (resourceCount == resourceLimit)
			{
				currentState = GatheringState.Store;
			}
		}
		else
		{
			resourceTimer += Time.deltaTime;
		}
	}

	protected void ApproachItem(){
		if (!hasPickedTarget)
		{
			List<BaseResourceNode> resourceNodes = ResourceManager.instance.FetchNearestResource(RESOURCETYPE.IRON, transform.position);
			for (int i = 0; i < resourceNodes.Count; i++)
			{
				resourceTarget = resourceNodes[0];
				break;
			}
			hasPickedTarget = true;
		}

		if (currentPath.Count <= 1 && resourceTarget != null)
		{
			currentPath = PathManager.instance.RunPathfinder(transform.position, resourceTarget.transform.position);
		}
		else
		{
			transform.position += (currentPath[0] - transform.position).normalized * Time.deltaTime * unitSpeed;
			transform.forward = (currentPath[0] - transform.position).normalized;
			if (Vector3.Distance(transform.position, currentPath[0]) < .35f)
			{
				currentPath.RemoveAt(0);
			}
		}
		if (resourceTarget != null)
		{
			Vector3 targetPos = resourceTarget.transform.position;
			targetPos.y = 0;
			Vector3 myPos = transform.position;
			myPos.y = 0;
			if (Vector3.Distance(myPos, targetPos) < 1.5f)
			{
				currentState = GatheringState.Gather;
			}
		}
	}
	
	protected void StoreItem(){

		if (!hasPickedTarget)
		{
			List<WarehouseBase> warehouses = StructureManager.instance.FetchNearestWarehouse(transform.position);
			myWarehouse = null;
			for (int i = 0; i < warehouses.Count; i++)
			{
				if (warehouses[i].StorageRequest(currentResourceType))
				{
					myWarehouse = warehouses[i];
					hasPickedTarget = true;
					break;
				}
			}
			if (myWarehouse == null)
			{
				print("No Valid Warehouses.");
			}
		}

		if (myWarehouse != null)
		{
			if (currentPath.Count <= 1)
			{
				currentPath = PathManager.instance.RunPathfinder(transform.position, myWarehouse.transform.position);
			}
			else
			{
				transform.position += (currentPath[0] - transform.position).normalized * Time.deltaTime * unitSpeed;
				transform.forward = (currentPath[0] - transform.position).normalized;
				if (Vector3.Distance(transform.position, currentPath[0]) < .35f)
				{
					currentPath.RemoveAt(0);
				}
			}

			Vector3 targetPos = myWarehouse.transform.position;
			targetPos.y = 0;
			Vector3 myPos = transform.position;
			myPos.y = 0;
			if (Vector3.Distance(myPos, targetPos) <= 1.5f)
			{
				print("Attempting to store.");
				if (myWarehouse.StoreResource(currentResourceType, resourceCount))
				{
					resourceCount = 0;
					currentResourceType = null;
					currentState = GatheringState.Approach;
					hasPickedTarget = false;
				}
				else
				{
					hasPickedTarget = false;
				}
			}
		}
	}
	#endregion
}
