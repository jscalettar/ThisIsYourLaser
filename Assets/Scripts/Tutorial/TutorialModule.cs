using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tutorialTrigger { placed, placing, moved, moving, removed, removing, cursorMovedTo };
public enum startState { placeBase, placeLaser, idle };

public class TutorialModule : MonoBehaviour {
    [Tooltip("For keeping track of the order of tutorial levels, used when cycling through levels")]
    [Header("Module Index")]
    public int moduleOrderIndex;
    [Header("Starting State")]
    public startState initialState;
    [Header("Starting Building Selection")]
    public Building initialSelection = Building.Blocking;
    [Tooltip("List of creatures to place into tutorial level")]
    [Header("Initial Creature Spawnlist")]
    public List<SpawnItem> spawnList;
    [Header("Initial Popup(s)")]
    public Texture2D initialPopup;                              // Popup that appears when you first start the tutorial
    public Texture2D initialPopup2;                             // Another popup that appears when you first start the tutorial (optional)
    public Texture2D initialPopup3;                             // Another popup that appears when you first start the tutorial (optional)
    [Header("Destruction Popups")]
    public Texture2D firstDestroyed;                            // Popup for first creature that is destroyed, leave it null if none
    public bool endOnFirstDestroyed;
    public Texture2D baseDestroyed;                             // Popup when base is destroyed
    public bool endOnBaseDestruction;
    public List<specificDestructionPopup> specificDestroyed; // Popups for when buildings at specific locations are destroyed
    [Header("Interaction Popups")]
    public Texture2D firstPlacing;      
    public Texture2D firstPlaced;       
    public bool endOnPlaced;
    public Texture2D firstMoving;       
    public Texture2D firstMovingPlacing;
    public Texture2D firstMoved; 
    public bool endOnMoved;
    public Texture2D firstRemoving;
    public Texture2D firstRemoved;
    public bool endOnRemoved;
    public List<specificInteractionPopup> specificInteraction;           // Popups for when creatures at specific locations are moved/removed/placed
    [Header("Cursor Movement Popups")]
    public Texture2D movedLeft;
    public Texture2D movedRight;
    public Texture2D movedUp;
    public Texture2D movedDown;
    public bool endOnAllDirectionsMoved;
    public Texture2D allDrectionsMovedPopup;

    [Serializable]
    public class SpawnItem
    {
        public int x;
        public int y;
        public Building building;
        public Player player;
        public Direction direction;
    }

    [Serializable]
    public class specificDestructionPopup
    {
        public XY pos;
        public Texture2D popup;
        public bool endTrigger;
    }

    [Serializable]
    public class specificInteractionPopup
    {
        public XY pos = new XY(-1, -1);
        public Building building = Building.Any;
        public tutorialTrigger type;
        public Texture2D popup;
        public bool endTrigger;
    }
}
