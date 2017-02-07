using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instanceClass : instantiateBuildings
{
	/*
	public void placeBuilding (int x, int y, Building newBuilding, Player playerID, Direction facing = Direction.None) {
		GameObject building = Instantiate(buildingList[(int)newBuilding], new Vector3(x, 0f, y), Quaternion.identity) as GameObject;
		building.transform.SetParent(buildingContainer.transform);

	}
	*/
}


public class instantiateBuildings : MonoBehaviour {

	public List<GameObject> buildingList;
	[HideInInspector]
	public GameObject buildingContainer;

	void Awake () {
		buildingContainer = new GameObject("buildingContainer");
		buildingContainer.transform.SetParent(gameObject.transform);
	}

	public void placeBuilding (int x, int y, Building newBuilding, Player playerID, Direction facing = Direction.None) {
		GameObject building = Instantiate(buildingList[(int)newBuilding], new Vector3(x, 0f, y), Quaternion.identity) as GameObject;
		building.transform.SetParent(buildingContainer.transform);

	}


}
