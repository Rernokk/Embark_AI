using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {
	
	public static PathManager instance;
	PathNode[,] worldGrid;

	[SerializeField]
	GameObject pathNodePrefab;

	bool worldReady = false;

	#region Properties
	public PathNode[,] WorldGrid {
		get {
			return worldGrid;
		}
	}
	public bool WorldIsReady {
		get{
			return worldReady;
		}
	}
	#endregion

	#region Public Methods

	public float GetPointHeight(Vector3 position){
		if (worldReady){
			return worldGrid[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z)].transform.position.y;
		} else {
			worldReady = true;
			foreach (PathNode n in WorldGrid){
				if (!n.isReady){
					worldReady = false;
					break;
				}
			}
			return -9999f;
		}
	}

	public List<Vector3> RunPathfinder(Vector3 startPos, Vector3 endPos)
	{
		PathNode startNode = worldGrid[(int)startPos.x, (int)startPos.z];
		PathNode endNode = worldGrid[(int)endPos.x, (int)endPos.z];

		List<Vector3> genPath = new List<Vector3>();
		List<PathNode> queued = new List<PathNode>();
		foreach (PathNode n in worldGrid)
		{
			n.ResetNode();
		}
		queued.Add(startNode);
		PathNode currentNode = null;
		while (queued.Count > 0)
		{
			queued.Sort((q1, q2) => Vector3.Distance(q1.transform.position, endNode.transform.position).CompareTo(Vector3.Distance(q2.transform.position, endNode.transform.position)));
			currentNode = queued[0];
			queued.RemoveAt(0);
			foreach (PathNode n in currentNode.neighbors)
			{
				if (n == endNode)
				{
					startNode.previous = null;
					n.previous = currentNode;
					currentNode = n;
					while (currentNode.previous != null)
					{
						genPath.Add(currentNode.transform.position);
						currentNode = currentNode.previous;
					}
					genPath.Reverse();
					return genPath;
				}
				else if (!n.visited)
				{
					n.visited = true;
					n.previous = currentNode;
					queued.Add(n);
				}
			}
		}
		return genPath;
	}
	#endregion

	#region Private Methods
	void Awake() {
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	void Start() {
		int worldSize = WorldManager.instance.WorldSize;
		worldGrid = new PathNode[worldSize, worldSize];
		Transform worldNodeHost = new GameObject("WorldNodeHost").transform;
		for (int i = 0; i < worldSize; i++)
		{
			for (int j = 0; j < worldSize; j++){
				worldGrid[i, j] = Instantiate(pathNodePrefab, new Vector3(i, -10, j), Quaternion.identity).GetComponent<PathNode>();
				worldGrid[i, j].transform.parent = worldNodeHost;
			}
		}
	}

	void Update(){
		if (worldReady == false){ 
		worldReady = true;
			foreach (PathNode n in WorldGrid)
			{
				if (!n.isReady)
				{
					worldReady = false;
					break;
				}
			}
		}
	}
	#endregion

}
