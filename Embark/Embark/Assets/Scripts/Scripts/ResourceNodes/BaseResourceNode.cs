using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESOURCETYPE { NOTHING, IRON };

public class BaseResourceNode : MonoBehaviour
{
	[SerializeField]
	int resourceID;

	[SerializeField]
	RESOURCETYPE myResourceType;

	[SerializeField]
	Vector2Int structureSize;

	Item resourceItem;
	protected bool positionedCorrectly = false;

	public Item ResourceItem
	{
		get
		{
			return resourceItem;
		}

		set
		{
			resourceItem = value;
		}
	}

	public RESOURCETYPE MyResourceType
	{
		get
		{
			return myResourceType;
		}

		set
		{
			myResourceType = value;
		}
	}


	protected virtual void Reposition()
	{
		if (PathManager.instance.WorldIsReady)
		{
			Vector3 repos = transform.position;
			repos.x = Mathf.FloorToInt(repos.x);
			repos.z = Mathf.FloorToInt(repos.z);
			//repos.y = PathManager.instance.GetPointHeight(repos);
			repos.y = WorldManager.instance.HeightAtPosition(transform.position);
			transform.position = repos;
			positionedCorrectly = true;

			for (int i = -structureSize.x; i <= structureSize.x; i++)
			{
				for (int j = -structureSize.y; j <= structureSize.y; j++)
				{
					PathManager.instance.BlockNode(transform.position + new Vector3(i, 0, j));
				}
			}
		}
	}

	protected virtual void Start()
	{
		ResourceManager.instance.AddResourceNode(this);
		resourceItem = RecipeManager.instance.FetchItemByID(resourceID);
		resourceID = resourceItem.ItemID;
		Reposition();
	}

	protected virtual void Update()
	{
		if (!positionedCorrectly)
		{
			Reposition();
		}
	}
}
