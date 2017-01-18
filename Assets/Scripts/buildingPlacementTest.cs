using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingPlacementTest : MonoBehaviour {

    // This is for Testing building placements with laser logic.
    // Assumes top down ortho camera looking at board that is centered at (0,0,0)
    // Use 1-8 to select buildings
    // Left mouse places Player1 buildings
    // Right mouse places Player2 buildings
    // MMB deletes buildings

    private Building selection;

    private Vector2 mouseToCoords(Vector3 input)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(input);
        int xCoord = Mathf.FloorToInt(worldPos.x) + gridManager.theGrid.getDimX() / 2;
        int yCoord = Mathf.FloorToInt(worldPos.z) + gridManager.theGrid.getDimY() / 2;
        return new Vector2(xCoord, yCoord);
    }

    void Start() { selection = Building.Base; }

    void Update()
    {
        if (Input.GetKeyDown("1")) { selection = Building.Base; print("Base Selected"); }
        if (Input.GetKeyDown("2")) { selection = Building.Laser; print("Laser Selected"); }
        if (Input.GetKeyDown("3")) { selection = Building.Blocking; print("Blocking Selected"); }
        if (Input.GetKeyDown("4")) { selection = Building.Reflecting; print("Reflecting Selected"); }
        if (Input.GetKeyDown("5")) { selection = Building.Refracting; print("Refracting Selected"); }
        if (Input.GetKeyDown("6")) { selection = Building.Redirecting; print("Redirecting Selected"); }
        if (Input.GetKeyDown("7")) { selection = Building.Portal; print("Portal Selected"); }
        if (Input.GetKeyDown("8")) { selection = Building.Resource; print("Resource Selected"); }


        if (Input.GetMouseButtonDown(0)) {
            Vector2 coords = mouseToCoords(Input.mousePosition);
            gridManager.theGrid.placeBuilding((int)coords.x, (int)coords.y, selection, Player.PlayerOne);
        } else if (Input.GetMouseButtonDown(1)) {
            Vector2 coords = mouseToCoords(Input.mousePosition);
            gridManager.theGrid.placeBuilding((int)coords.x, (int)coords.y, selection, Player.PlayerTwo);
        } else if (Input.GetMouseButtonDown(2)) {
            Vector2 coords = mouseToCoords(Input.mousePosition);
            gridManager.theGrid.destroyBuilding((int)coords.x, (int)coords.y, Player.World);
        }
    }
}
