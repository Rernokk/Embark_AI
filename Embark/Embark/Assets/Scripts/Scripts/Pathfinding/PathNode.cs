using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
	public PathNode previous = null;
	public bool visited = false;
	public bool activeNode = true;
	public bool isReady = false;
	public List<PathNode> neighbors = new List<PathNode>();

	public void ResetNode()
	{
		previous = null;
		visited = false;
	}

	void Start()
	{
		Vector3 newPos = transform.position;
		Texture2D reference = WorldManager.instance.WorldHeightMap;
		newPos.y = WorldManager.instance.HeightAtPosition(transform.position);
		transform.position = newPos;
		int worldSize = WorldManager.instance.WorldSize;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				int tarX = (int)transform.position.x + i;
				int tarY = (int)transform.position.z + j;

				if ((i != 0 || j != 0) && tarX >= 0 && tarY >= 0 && tarX < worldSize && tarY < worldSize)
				{
					neighbors.Add(PathManager.instance.WorldGrid[(int)transform.position.x + i, (int)transform.position.z + j]);
				}
			}
		}
		isReady = true;
	}
	
}
