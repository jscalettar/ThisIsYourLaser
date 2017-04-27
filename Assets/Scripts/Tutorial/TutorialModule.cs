using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tutorialTrigger { placed, moved, removed };

public class TutorialModule : MonoBehaviour {
    [Tooltip("For keeping track of the order of tutorial levels, used when cycling through levels")]
    [Header("Module Index")]
    public int moduleOrderIndex;
    [Tooltip("List of creatures to place into tutorial level")]
    [Header("Initial Creature Spawnlist")]
    public List<SpawnItem> spawnList;
    [Header("Initial Popup(s)")]
    public Sprite initialPopup;                              // Popup that appears when you first start the tutorial
    public Sprite initialPopup2;                             // Another popup that appears when you first start the tutorial (optional)
    public Sprite initialPopup3;                             // Another popup that appears when you first start the tutorial (optional)
    [Header("Destruction Popups")]
    public Sprite firstDestroyed;                            // Popup for first creature that is destroyed, leave it null if none
    public bool endOnFirstDestroyed;
    public Sprite baseDestroyed;                             // Popup when base is destroyed
    public bool endOnBaseDestruction;
    public List<specificDestructionPopup> specificDestroyed; // Popups for when buildings at specific locations are destroyed
    public bool endOnspecificDestroyed;
    [Header("Interaction Popups")]
    public Sprite firstPlacing;      
    public Sprite firstPlaced;       
    public bool endOnPlaced;
    public Sprite firstMoving;       
    public Sprite firstMovingPlacing;
    public Sprite firstMoved; 
    public bool endOnMoved;
    public Sprite firstRemoving;
    public Sprite firstRemoved;
    public bool endOnRemoved;
    public List<tutorialTrigger> specificInteraction;           // Popups for when creatures at specific locations are moved/removed/placed

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
        public Sprite popup;
    }

    [Serializable]
    public class specificInteractionPopup
    {
        public XY pos;
        public tutorialTrigger type;
        public Sprite popup;
    }
}
