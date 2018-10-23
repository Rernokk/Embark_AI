using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWarehouse : WarehouseBase {

	protected override void Start () {
		base.Start();
		if (storageItemQuantity == 0)
			storageItemQuantity = 60;
		
		if (storageItemSize == 0)
			storageItemSize = 10;
	}

	protected override void Update()
	{
		base.Update();
	}
}
