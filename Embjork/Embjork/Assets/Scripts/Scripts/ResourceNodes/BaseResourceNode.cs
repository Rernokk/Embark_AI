using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESOURCETYPE { NOTHING, IRON };

public class BaseResourceNode : MonoBehaviour {
	public RESOURCETYPE myResourceType = RESOURCETYPE.IRON;

	void Start() {
		myResourceType = RESOURCETYPE.IRON;
		ResourceManager.instance.AddResourceNode(this);
	}
}
