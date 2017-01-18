using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour {
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const int TILE_DIMX = 14;
    private const int TILE_DIMY = 10;

    private int selectionX = -1;
    private int selectionY = -1;
    private bool selectionValid = false;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private void Update() {

        UpdateSelection();
        DrawChessboard();
    }

    private void UpdateSelection() {
        if (!Camera.main)
            return;

        RaycastHit hit;
        //Raycast returns bool of: 
        //(origin [camera array], info put into the out parameter to use later (if there is a collision), max distance, layer mask (only hit chess board not chess piece))
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) {
            //Debug.Log(hit.point);
            selectionX = Mathf.FloorToInt(hit.point.x);
            selectionY = Mathf.FloorToInt(hit.point.z);
            selectionValid = true;
        }
        else{
            selectionValid = false;
        }
    }

    private void DrawChessboard() {
        Vector3 widthLine = Vector3.right * TILE_DIMX; //Unity vector of length 1 pointing right * 8 (because there are 8 tiles in a chess game)
        Vector3 heightLine = Vector3.forward * TILE_DIMY; //same as above, but pointing forward

        for(int i=0; i<= TILE_DIMY; i++) {
            Vector3 start = (Vector3.forward * i) - new Vector3 (TILE_DIMX / 2, 0, TILE_DIMY / 2);
            Debug.DrawLine(start, start + widthLine);
            for(int j=0; j<= TILE_DIMX; j++) {
                start = (Vector3.right * j) - new Vector3(TILE_DIMX / 2, 0, TILE_DIMY / 2);
                Debug.DrawLine(start, start + heightLine);
            }
        }

        //Draw the selection
        if(selectionValid) {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX, 
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX +1));
            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
}
