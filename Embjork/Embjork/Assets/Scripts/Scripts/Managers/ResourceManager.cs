using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

	public static ResourceManager instance;
	Dictionary<RESOURCETYPE, List<BaseResourceNode>> resourceNodes = new Dictionary<RESOURCETYPE, List<BaseResourceNode>>();

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

	#region Public Methods
	public void AddResourceNode(BaseResourceNode node)
	{
		if (!resourceNodes.ContainsKey(node.myResourceType))
		{
			resourceNodes.Add(node.myResourceType, new List<BaseResourceNode>());
		} else {
			if (!resourceNodes[node.myResourceType].Contains(node))
				resourceNodes[node.myResourceType].Add(node);
		}
	}

	public List<BaseResourceNode> FetchNearestResource(RESOURCETYPE targetType, Vector3 position)
	{
		if (resourceNodes.ContainsKey(targetType))
		{
			List<BaseResourceNode> tempList = resourceNodes[targetType];
			tempList.Sort((q1, q2) => Vector3.Distance(q1.transform.position, position).CompareTo(Vector3.Distance(q2.transform.position, position)));
			return tempList;
		} else {
			return null;
		}
	}
	#endregion
}
