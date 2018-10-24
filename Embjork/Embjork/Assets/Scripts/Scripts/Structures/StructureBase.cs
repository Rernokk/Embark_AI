using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
	public Vector2Int StructureSize;

	protected bool positionedCorrectly = false;

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

			for (int i = -StructureSize.x; i <= StructureSize.x; i++)
			{
				for (int j = -StructureSize.y; j <= StructureSize.y; j++)
				{
					PathManager.instance.BlockNode(transform.position + new Vector3(i, 0, j));
				}
			}
		}
	}

	protected virtual void Update()
	{
		if (!positionedCorrectly)
		{
			Reposition();
		}
	}
}
