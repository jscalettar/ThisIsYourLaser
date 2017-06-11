using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State { placeBase, placeLaser, placingLaser, placing, moving, placingMove, removing, idle };

public class inputController : MonoBehaviour {

    // These should be gameObjects that contain a sprite renderer
    public GameObject cursorObjP1;
    public GameObject cursorObjP2;

    public GameObject cursorSpriteP1;
    public GameObject cursorSpriteP2;
    public GameObject indicatorP1;
    public GameObject indicatorP2;
    public GameObject LaserArrowP1;
    public GameObject LaserArrowP2;

    public GameObject selectorP1;
    public GameObject selectorP2;

    // Sprites for cursor appearance
    public Sprite P1BaseSprite;
    public Sprite P1BlockSprite;
    public Sprite P1LaserSprite;
    public Sprite P1ReflectSprite;
    public Sprite P1RefractSprite;
    public Sprite P1RedirectSprite;
    public Sprite P1ResourceSprite;

    public Sprite P2BaseSprite;
    public Sprite P2BlockSprite;
    public Sprite P2LaserSprite;
    public Sprite P2ReflectSprite;
    public Sprite P2RefractSprite;
    public Sprite P2RedirectSprite;
    public Sprite P2ResourceSprite;

    public Sprite RedirectUp;
    public Sprite RedirectDown;
    public Sprite RedirectLeft;
    public Sprite RedirectRight;

    public Sprite RedirectUp2;
    public Sprite RedirectDown2;
    public Sprite RedirectLeft2;
    public Sprite RedirectRight2;

    // Cursor movement speed
    private const float cursorSpeed = 8f;
    private float delayFactor = 1f / cursorSpeed;
    private float diagSpeed = 0.707f;

    // Analog deadzone
    private float deadzone = 0.5f;

    //List of Sounds
    public Audios[] setSounds;
    public static Audios[] Sounds;
    public Audios[] setUISounds;
    public static Audios[] UISounds;
    public Audios[] setMusicSounds;
    public static Audios[] musicSounds;

    // Pause menu and win screen
    public GameObject PauseMenu;
    public GameObject Win;

    public class Cursor
    {
        public int x, y;
        public XY moveOrigin;
        public Building moveBuilding;
        public Building selection;
        public Direction direction;
        public State state;
        public bool moving;
        public bool colored;
        public float moveDelay;

        public Cursor(int X, int Y, Direction dir, Building selected, State current)
        {
            x = X;
            y = Y;
            direction = dir;
            selection = selected;
            state = current;
            moveOrigin = new XY(-1, -1);
            moveBuilding = Building.Empty;
            moving = false;
            colored = false;
            moveDelay = 0f;
        }

        public override int GetHashCode()
        {
            int value = x + (y * 8) + ((int)selection * 69) + ((int)direction * 1337);
            return value.GetHashCode();
        }
    }

    private int clamp(int value, int min, int max) {
        if (value > max) return max;
        else if (value < min) return min;
        return value;
    }

    private bool isValid(State state, int value, int min, int max) {
        if (state == State.placeBase)
            return value < max - 1 && value > min + 1;
        return value <= max && value >= min;
    }

    private void enqueueMovement(Player cursorOwner, Direction moveDir, bool analog = false)
    {
        // Only move if queue is empty to avoid extremely rapid movement
        if ((cursorOwner == Player.PlayerOne ? moveQueueP1.Count : moveQueueP2.Count) > 0 || (cursorOwner == Player.PlayerOne ? cursorP1.moveDelay : cursorP2.moveDelay) > 0) return;

        State currState = cursorOwner == Player.PlayerOne ? cursorP1.state : cursorP2.state;
        int x = cursorOwner == Player.PlayerOne ? cursorP1.x : cursorP2.x;
        int y = cursorOwner == Player.PlayerOne ? cursorP1.y : cursorP2.y;

        if (moveDir == Direction.Up) { y += 1; if (!isValid(currState, y, 0, yEnd)) return; }
        else if (moveDir == Direction.Down) { y -= 1; if (!isValid(currState, y, 0, yEnd)) return; }
        else if (moveDir == Direction.Right && currState != State.placeLaser) { x += 1; if (!isValid(currState, x, 0, xEnd)) return; }
        else if (moveDir == Direction.Left && currState != State.placeLaser) { x -= 1; if (!isValid(currState, x, 0, xEnd)) return; }
        else return;

        if (cursorOwner == Player.PlayerOne) {
            if (!cursorP1.moving) cursorP1.moveDelay = 0.2f;
            cursorP1.moving = true;
            cursorP1.x = x;
            cursorP1.y = y;
            moveQueueP1.Enqueue(new XY(x, y));
        } else {
            if (!cursorP2.moving) cursorP2.moveDelay = 0.2f;
            cursorP2.moving = true;
            cursorP2.x = x;
            cursorP2.y = y;
            moveQueueP2.Enqueue(new XY(x, y));
        }

        SoundManager.PlaySound(Sounds[0].audioclip, .2f, true, .95f, 1.05f);
    }

	public static Cursor cursorP1, cursorP2, cursorP1Last, cursorP2Last;
	private int xEnd, yEnd;
	private int cycleP1, cycleP2;
	public static bool p1HasPlacedBase = false, p2HasPlacedBase = false;
    private Queue<XY> moveQueueP1;
    private Queue<XY> moveQueueP2;

	public void initCursors()
	{
        moveQueueP1 = new Queue<XY>();
        moveQueueP2 = new Queue<XY>();
        p1HasPlacedBase = false;
		p2HasPlacedBase = false;
		cycleP1 = 0;
		cycleP2 = 0;
		xEnd = gridManager.theGrid.getDimX() - 1;
		yEnd = gridManager.theGrid.getDimY() - 1;
		cursorP1 = new Cursor(0, 2, Direction.Down, Building.Resource, State.placeBase);
		cursorP2 = new Cursor(xEnd, yEnd - 2, Direction.Down, Building.Resource, State.placeBase);
		if (TutorialFramework.tutorialActive) { gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().setupCursorState(); }
		cursorP1Last = new Cursor(cursorP1.x, cursorP1.y, cursorP1.direction, cursorP1.selection, cursorP1.state);
        cursorP2Last = new Cursor(cursorP2.x, cursorP2.y, cursorP2.direction, cursorP2.selection, cursorP2.state);
		//PauseMenu = GameObject.Find("Pause Menu");
		// Set initial cursor positions
		cursorObjP1.transform.position = new Vector3(cursorP1.x + (-gridManager.theGrid.getDimX() / 2f + 0.5f), 0.01f, cursorP1.y + (-gridManager.theGrid.getDimY() / 2f + 0.5f));
		cursorObjP2.transform.position = new Vector3(cursorP2.x + (-gridManager.theGrid.getDimX() / 2f + 0.5f), 0.01f, cursorP2.y + (-gridManager.theGrid.getDimY() / 2f + 0.5f));
	}

	void Start () {
		Sounds = setSounds;
		UISounds = setUISounds;
		musicSounds = setMusicSounds;
		initCursors();
        gridManager.theGrid.updateSquares();
        gridManager.theGrid.updateLaser();
        SoundManager.PlayMusic(musicSounds[0].audioclip, .2f, true, true, 5f, 1.5f);
        indicatorP1.transform.localScale = new Vector3(2f, 2f, 2f);
        indicatorP2.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    void Update()
	{
		bool notNow1 = false;
		bool notNow2 = false;
		if (Time.timeScale != 0) {
            // Check that the game isn't paused
            if ((PauseMenu != null && Win != null && PauseMenu.activeInHierarchy == false && Win.activeInHierarchy == false) && 
                    !(TutorialFramework.tutorialActive && TutorialFramework.skipFrame) && !(pauseMenu.skipFrame)) {
                
                // Cursor Selection P1
                int p1 = 0;
                switch (cursorP1.selection)
                {
                    case Building.Blocking: p1 = 0; break;
                    case Building.Reflecting: p1 = 1; break;
                    case Building.Refracting: p1 = 2; break;
                    case Building.Redirecting: p1 = 3; break;
                    case Building.Resource: p1 = 4; break;
                }
                // Cycle P1
                if (cursorP1.state != State.placing) {
                    //if (Input.GetKeyDown("1")) { cursorP1.selection = Building.Blocking; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("2")) { cursorP1.selection = Building.Reflecting; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("3")) { cursorP1.selection = Building.Refracting; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("4")) { cursorP1.selection = Building.Redirecting; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("5")) { cursorP1.selection = Building.Resource; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); }
                    if (Input.GetButtonDown("cycleR_1") || Input.GetKeyDown("3")) {
                        if (cursorP1.selection == Building.Resource) {
                            cursorP1.selection = Building.Blocking;
                        }
                        else {
                            cursorP1.selection += 1;
                        }
                        SoundManager.PlayUISound(UISounds[0].audioclip, .1f);
                    }
                    else if (Input.GetButtonDown("cycleL_1") || Input.GetKeyDown("2")) {
                        if (cursorP1.selection == Building.Blocking) {
                            cursorP1.selection = Building.Resource;
                        }
                        else {
                            cursorP1.selection -= 1;
                        }
                        SoundManager.PlayUISound(UISounds[0].audioclip, .1f);
                    }
                    selectorP1.transform.localPosition = new Vector3(-15f + p1 * 87.6f, -25f, 4f);
                }

                // Cursor Selection P2
                int p2 = 0;
                switch (cursorP2.selection)
                {
                    case Building.Blocking: p2 = 0; break;
                    case Building.Reflecting: p2 = 1; break;
                    case Building.Refracting: p2 = 2; break;
                    case Building.Redirecting: p2 = 3; break;
                    case Building.Resource: p2 = 4; break;
                }
                
                if (cursorP2.state != State.placing) {
                   // if (Input.GetKeyDown("7")) { cursorP2.selection = Building.Blocking; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("8")) { cursorP2.selection = Building.Reflecting; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("9")) { cursorP2.selection = Building.Refracting; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("0")) { cursorP2.selection = Building.Redirecting; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); } else if (Input.GetKeyDown("-")) { cursorP2.selection = Building.Resource; SoundManager.PlayUISound(UISounds[0].audioclip, .1f); }
                    if (Input.GetButtonDown("cycleR_2") || Input.GetKeyDown("9")) {
                        if (cursorP2.selection == Building.Resource) {
                            cursorP2.selection = Building.Blocking;
                        }
                        else {
                            cursorP2.selection += 1;
                        }
                        SoundManager.PlayUISound(UISounds[0].audioclip, .1f);

                    }
                    else if (Input.GetButtonDown("cycleL_2") || Input.GetKeyDown("8")) {
                        if (cursorP2.selection == Building.Blocking) {
                            cursorP2.selection = Building.Resource;
                        }
                        else {
                            cursorP2.selection -= 1;
                        }

                        SoundManager.PlayUISound(UISounds[0].audioclip, .1f);
                    }
                    selectorP2.transform.localPosition = new Vector3(14f + p2 * 87.6f, -25f, 4f);
                }

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                
                // Deadzone
                Vector2 p1LeftAnalog = new Vector2(Input.GetAxisRaw("xboxLeftHor"), Input.GetAxisRaw("xboxLeftVert"));
                if (Mathf.Abs(p1LeftAnalog.x) < deadzone) p1LeftAnalog.x = 0f;
                if (Mathf.Abs(p1LeftAnalog.y) < deadzone) p1LeftAnalog.y = 0f;

                Vector2 p1RightAnalog = new Vector2(Input.GetAxisRaw("xboxRightStickX1"), Input.GetAxisRaw("xboxRightStickY1"));
                if (Mathf.Abs(p1RightAnalog.x) < deadzone) p1RightAnalog.x = 0f;
                if (Mathf.Abs(p1RightAnalog.y) < deadzone) p1RightAnalog.y = 0f;

                // Calculate Joystick direction accuratly
                Vector2 dirVec = p1LeftAnalog.normalized;
                int dirIndx = ((Mathf.RoundToInt(Mathf.Atan2(dirVec.y, dirVec.x) / (2 * Mathf.PI / 4))) + 4) % 4;
                Direction directionL1 = Direction.Down;
                switch (dirIndx) {
                    case 0: directionL1 = Direction.Right; break;
                    case 1: directionL1 = Direction.Up; break;
                    case 2: directionL1 = Direction.Left; break;
                    case 3: directionL1 = Direction.Down; break;
                }
                // Now for Right Stick
                dirVec = p1RightAnalog.normalized;
                dirIndx = ((Mathf.RoundToInt(Mathf.Atan2(dirVec.y, dirVec.x) / (2 * Mathf.PI / 4))) + 4) % 4;
                Direction directionR1 = Direction.Down;
                switch (dirIndx) {
                    case 0: directionR1 = Direction.Right; break;
                    case 1: directionR1 = Direction.Up; break;
                    case 2: directionR1 = Direction.Left; break;
                    case 3: directionR1 = Direction.Down; break;
                }

                // Check if cursorP1 should be moving
                if (cursorP1.state != State.placing && cursorP1.state != State.placingLaser && cursorP1.state != State.placingMove && cursorP1.state != State.removing) {

                    // Analog movement
                    if (!(Input.GetButton("up_1") || Input.GetButton("down_1") || Input.GetButton("left_1") || Input.GetButton("right_1")) && p1LeftAnalog != Vector2.zero) {
                        // Enqueue movement
                        enqueueMovement(Player.PlayerOne, directionL1, true);
                    } else {
                        // Keyboard movement
                        if (Input.GetButton("up_1") || Input.GetAxis("xboxDpadY1") > 0) enqueueMovement(Player.PlayerOne, Direction.Up);
                        else if (Input.GetButton("down_1") || Input.GetAxis("xboxDpadY1") < 0) enqueueMovement(Player.PlayerOne, Direction.Down);
                        else if (Input.GetButton("right_1") || Input.GetAxis("xboxDpadX1") > 0) enqueueMovement(Player.PlayerOne, Direction.Right);
                        else if (Input.GetButton("left_1") || Input.GetAxis("xboxDpadX1") < 0) enqueueMovement(Player.PlayerOne, Direction.Left);
                        else cursorP1.moving = false;
                    }
                } else {
                    // Cursor Rotation P1
                    bool selectionMade = false;
					if ((cursorP1.selection == Building.Blocking || cursorP1.selection == Building.Refracting) && cursorP1.state != State.placingLaser) cursorP1.direction = Direction.Down;
                    else if (p1LeftAnalog != Vector2.zero) {
                        if (directionL1 == Direction.Right || directionL1 == Direction.Left) {
                            if (cursorP1.state != State.placingLaser) cursorP1.direction = directionL1;
                        } else cursorP1.direction = directionL1;
                    } else if (Input.GetButtonDown("up_1") || Input.GetAxis("xboxDpadY1") > 0) cursorP1.direction = Direction.Up;
                    else if (Input.GetButtonDown("down_1") || Input.GetAxis("xboxDpadY1") < 0) cursorP1.direction = Direction.Down;
                    else if (cursorP1.state != State.placingLaser && Input.GetButtonDown("right_1") || Input.GetAxis("xboxDpadX1") > 0) cursorP1.direction = Direction.Right;
                    else if (cursorP1.state != State.placingLaser && Input.GetButtonDown("left_1") || Input.GetAxis("xboxDpadX1") < 0) cursorP1.direction = Direction.Left;

                    if (Input.GetButtonDown("place_1")) { selectionMade = true; notNow1 = true; }
                    if (selectionMade) { // If placing or moving, finalize action
                        if (cursorP1.state == State.placingMove) move(Player.PlayerOne, cursorP1.state);
                        else place(Player.PlayerOne, cursorP1.state);
                    }
                }
                cursorP1.moveDelay = Mathf.Max(0f, cursorP1.moveDelay - Time.deltaTime); // Delay is used to make slight pause before continuous movement

                // Right stick rotation P1
                if (cursorP1.state == State.idle || cursorP1.state == State.moving || cursorP1.state == State.placing || cursorP1.state == State.placingMove || cursorP1.state == State.placingLaser || cursorP1.state == State.placeLaser) {
                    if (p1RightAnalog != Vector2.zero) {
                        if (directionR1 == Direction.Right || directionR1 == Direction.Left) {
                            if (cursorP1.state != State.placingLaser && cursorP1.state != State.placeLaser) cursorP1.direction = directionR1;
                        } else cursorP1.direction = directionR1;
                    }
                }

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                // Deadzone
                Vector2 p2LeftAnalog = new Vector2(Input.GetAxisRaw("xboxLeftHor2"), Input.GetAxisRaw("xboxLeftVert2"));
                if (Mathf.Abs(p2LeftAnalog.x) < deadzone) p2LeftAnalog.x = 0f;
                if (Mathf.Abs(p2LeftAnalog.y) < deadzone) p2LeftAnalog.y = 0f;

                Vector2 p2RightAnalog = new Vector2(Input.GetAxisRaw("xboxRightStickX2"), Input.GetAxisRaw("xboxRightStickY2"));
                if (Mathf.Abs(p2RightAnalog.x) < deadzone) p2RightAnalog.x = 0f;
                if (Mathf.Abs(p2RightAnalog.y) < deadzone) p2RightAnalog.y = 0f;

                // Calculate Joystick direction accuratly
                dirVec = p2LeftAnalog.normalized;
                dirIndx = ((Mathf.RoundToInt(Mathf.Atan2(dirVec.y, dirVec.x) / (2 * Mathf.PI / 4))) + 4) % 4;
                Direction directionL2 = Direction.Down;
                switch (dirIndx) {
                    case 0: directionL2 = Direction.Right; break;
                    case 1: directionL2 = Direction.Up; break;
                    case 2: directionL2 = Direction.Left; break;
                    case 3: directionL2 = Direction.Down; break;
                }
                // Now for Right Stick
                dirVec = p2RightAnalog.normalized;
                dirIndx = ((Mathf.RoundToInt(Mathf.Atan2(dirVec.y, dirVec.x) / (2 * Mathf.PI / 4))) + 4) % 4;
                Direction directionR2 = Direction.Down;
                switch (dirIndx) {
                    case 0: directionR2 = Direction.Right; break;
                    case 1: directionR2 = Direction.Up; break;
                    case 2: directionR2 = Direction.Left; break;
                    case 3: directionR2 = Direction.Down; break;
                }

                // Check if cursorP2 should be moving
                if (cursorP2.state != State.placing && cursorP2.state != State.placingLaser && cursorP2.state != State.placingMove && cursorP2.state != State.removing) {

                    // Analog movement
                    if (!(Input.GetButton("up_2") || Input.GetButton("down_2") || Input.GetButton("left_2") || Input.GetButton("right_2")) && p2LeftAnalog != Vector2.zero) {
                        // Enqueue movement
                        enqueueMovement(Player.PlayerTwo, directionL2, true);
                    } else {
                        // Keyboard movement
                        if (Input.GetButton("up_2") || Input.GetAxis("xboxDpadY2") > 0) enqueueMovement(Player.PlayerTwo, Direction.Up);
                        else if (Input.GetButton("down_2") || Input.GetAxis("xboxDpadY2") < 0) enqueueMovement(Player.PlayerTwo, Direction.Down);
                        else if (Input.GetButton("right_2") || Input.GetAxis("xboxDpadX2") > 0) enqueueMovement(Player.PlayerTwo, Direction.Right);
                        else if (Input.GetButton("left_2") || Input.GetAxis("xboxDpadX2") < 0) enqueueMovement(Player.PlayerTwo, Direction.Left);
                        else cursorP2.moving = false;
                    }
                } else {
                    // Cursor Rotation P2
                    bool selectionMade = false;
					if ((cursorP2.selection == Building.Blocking || cursorP2.selection == Building.Refracting) && cursorP2.state != State.placingLaser) cursorP2.direction = Direction.Down;
                    else if(p2LeftAnalog != Vector2.zero) {
                        if (directionL2 == Direction.Right || directionL2 == Direction.Left) {
                            if (cursorP2.state != State.placingLaser) cursorP2.direction = directionL2;
                        } else cursorP2.direction = directionL2;
                    } else if (Input.GetButtonDown("up_2") || Input.GetAxis("xboxDpadY2") > 0) cursorP2.direction = Direction.Up;
                    else if (Input.GetButtonDown("down_2") || Input.GetAxis("xboxDpadY2") < 0) cursorP2.direction = Direction.Down;
                    else if (cursorP2.state != State.placingLaser && Input.GetButtonDown("right_2") || Input.GetAxis("xboxDpadX2") > 0) cursorP2.direction = Direction.Right;
                    else if (cursorP2.state != State.placingLaser && Input.GetButtonDown("left_2") || Input.GetAxis("xboxDpadX2") < 0) cursorP2.direction = Direction.Left;

                    if (Input.GetButtonDown("place_2")) { selectionMade = true; notNow2 = true; }
                    if (selectionMade) { // If placing or moving, finalize action
                        if (cursorP2.state == State.placingMove) move(Player.PlayerTwo, cursorP2.state);
                        else place(Player.PlayerTwo, cursorP2.state);
                    }
                }
                cursorP2.moveDelay = Mathf.Max(0f, cursorP2.moveDelay - Time.deltaTime); // Delay is used to make slight pause before continuous movement

                // Right stick rotation P2
                if (cursorP2.state == State.idle || cursorP2.state == State.moving || cursorP2.state == State.placing || cursorP2.state == State.placingMove || cursorP2.state == State.placingLaser || cursorP2.state == State.placeLaser) {
                    if (p2RightAnalog != Vector2.zero) {
                        if (directionR2 == Direction.Right || directionR2 == Direction.Left) {
                            if (cursorP2.state != State.placingLaser && cursorP2.state != State.placeLaser) cursorP2.direction = directionR2;
                        } else cursorP2.direction = directionR2;
                    }
                }

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                // Cursor Functions P1
                if ((Input.GetButtonDown("place_1")) && !notNow1) place(Player.PlayerOne, cursorP1.state);
                else if (Input.GetButtonDown("move_1")) move(Player.PlayerOne, cursorP1.state);
                else if (Input.GetButtonDown("remove_1")) remove(Player.PlayerOne, cursorP1.state);
                else if (Input.GetButtonDown("cancel_1") && cursorP1.state != State.placeBase && cursorP1.state != State.placeLaser)
                {
                    cursorP1.state = cursorP1.state == State.placingLaser ? State.placeLaser : State.idle;
                }
				// Cursor Functions P2
				if (!TutorialFramework.tutorialActive) {
					if (Input.GetButtonDown("place_2") && !notNow2) place(Player.PlayerTwo, cursorP2.state);
					else if (Input.GetButtonDown("move_2")) move(Player.PlayerTwo, cursorP2.state);
					else if (Input.GetButtonDown("remove_2")) remove(Player.PlayerTwo, cursorP2.state);
					else if (Input.GetButtonDown("cancel_2") && cursorP2.state != State.placeBase && cursorP2.state != State.placeLaser)
                    {
                        cursorP2.state = cursorP2.state == State.placingLaser ? State.placeLaser : State.idle;
                    }
                }


                // // Shark direction P1
                if (cursorP1.state == State.placingLaser || cursorP1.state == State.placeLaser) {

                    // If shark 2 from edge, force proper direction
                    if (cursorP1.y > yEnd - 2) cursorP1.direction = Direction.Down;
                    else if (cursorP1.y < 2) cursorP1.direction = Direction.Up;
                }

                // Shark direction P2
                if (cursorP2.state == State.placingLaser || cursorP2.state == State.placeLaser) {

                    // If shark 2 from edge, force proper direction
                    if (cursorP2.y > yEnd - 2) cursorP2.direction = Direction.Down;
                    else if (cursorP2.y < 2) cursorP2.direction = Direction.Up;
                }

                // Cursor Visual Code -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                Building buildingToDisplayP1 = Building.Blocking;
                Building buildingToDisplayP2 = Building.Blocking;
                if (cursorP1.state == State.moving || cursorP1.state == State.placingMove) {
                    buildingToDisplayP1 = cursorP1.moveBuilding;
                } else {
                    buildingToDisplayP1 = cursorP1.selection;
                }
                if (cursorP2.state == State.moving || cursorP2.state == State.placingMove) {
                    buildingToDisplayP2 = cursorP2.moveBuilding;
                } else {
                    buildingToDisplayP2 = cursorP2.selection;
                }
                if (cursorP1.state == State.placeBase) buildingToDisplayP1 = Building.Base;
                if (cursorP2.state == State.placeBase) buildingToDisplayP2 = Building.Base;
                if (cursorP1.state == State.placeLaser || cursorP1.state == State.placingLaser) buildingToDisplayP1 = Building.Laser;
                if (cursorP2.state == State.placeLaser || cursorP2.state == State.placingLaser) buildingToDisplayP2 = Building.Laser;

                cursor.SpriteWithOffsets buildingSprite = new cursor.SpriteWithOffsets();
                float scale = 1f;

                switch (buildingToDisplayP1) {
                    case Building.Base: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[6][0]; scale = 1f / 3.4f; break;
                    case Building.Laser: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[5][cursorP1.direction == Direction.Up ? 1 : 0]; scale = 1f / 3.4f; break;
                    case Building.Blocking: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[0][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .15f; break;
                    case Building.Reflecting: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[1][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .375f; break;
                    case Building.Refracting: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[2][0]; scale = .2f; break;
                    case Building.Redirecting: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[3][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .25f; break;
                    case Building.Resource: buildingSprite = cursorObjP1.GetComponent<cursor>().Sprites[4][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .15f; break;
                }

                if (buildingSprite.sprite != null) {
                    cursorSpriteP1.GetComponent<SpriteRenderer>().sprite = buildingSprite.sprite;
					cursorSpriteP1.transform.localPosition = new Vector3(buildingSprite.offsetX, buildingSprite.offset, -0.05f);
                    cursorSpriteP1.transform.localScale = new Vector3(scale * 3.4f, scale * 3.4f, scale * 3.4f);
                }

                if (!TutorialFramework.tutorialActive) {
                    buildingSprite = new cursor.SpriteWithOffsets();
                    scale = 1f;

                    switch (buildingToDisplayP2) {
                        case Building.Base: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[6][0]; scale = 1f / 3.4f; break;
                        case Building.Laser: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[5][cursorP2.direction == Direction.Up ? 1 : 0]; scale = 1f / 3.4f; break;
                        case Building.Blocking: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[0][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .15f; break;
                        case Building.Reflecting: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[1][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .375f; break;
                        case Building.Refracting: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[2][0]; scale = .2f; break;
                        case Building.Redirecting: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[3][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .25f; break;
                        case Building.Resource: buildingSprite = cursorObjP2.GetComponent<cursor>().Sprites[4][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .15f; break;
                    }

                    if (buildingSprite.sprite != null) {
                        cursorSpriteP2.GetComponent<SpriteRenderer>().sprite = buildingSprite.sprite;
						cursorSpriteP2.transform.localPosition = new Vector3(buildingSprite.offsetX, buildingSprite.offset, -0.05f);
                        cursorSpriteP2.transform.localScale = new Vector3(scale * 3.4f, scale * 3.4f, scale * 3.4f);
                    }
                } else { cursorSpriteP2.GetComponent<SpriteRenderer>().enabled = false; }

                // Matt's Code --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                // Update Cursor Indicator---------------------------------
                if (cursorP1.state == State.placing) {
					indicatorP1.GetComponent<SpriteRenderer>().enabled = true;
					indicatorP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor>().UISprites[0];
				} else if (cursorP1.state == State.placingLaser) {
					indicatorP1.GetComponent<SpriteRenderer>().enabled = true;
				} else if (cursorP1.state == State.placingMove || cursorP1.state == State.moving) {
					indicatorP1.GetComponent<SpriteRenderer>().enabled = true;
					indicatorP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor>().UISprites[2];
				} else if (cursorP1.state == State.removing) {
					indicatorP1.GetComponent<SpriteRenderer>().enabled = true;
					indicatorP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor>().UISprites[1];
				} else {
					indicatorP1.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (cursorP2.state == State.placing) {
					indicatorP2.GetComponent<SpriteRenderer>().enabled = true;
					indicatorP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor>().UISprites[0];
				} else if (cursorP2.state == State.placingLaser) {
					indicatorP2.GetComponent<SpriteRenderer>().enabled = true;
				} else if (cursorP2.state == State.placingMove || cursorP2.state == State.moving) {
					indicatorP2.GetComponent<SpriteRenderer>().enabled = true;
					indicatorP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor>().UISprites[2];
				} else if (cursorP2.state == State.removing) {
					indicatorP2.GetComponent<SpriteRenderer>().enabled = true;
					indicatorP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor>().UISprites[1];
				} else {
					indicatorP2.GetComponent<SpriteRenderer>().enabled = false;
				}

                // End Matt's Code ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                float xOff = -gridManager.theGrid.getDimX() / 2f + 0.5f;
                float yOff = -gridManager.theGrid.getDimY() / 2f + 0.5f;

                // Update Cursor Position P1
                if (moveQueueP1.Count > 0) {
                    cursorObjP1.transform.position = Vector3.MoveTowards(cursorObjP1.transform.position, new Vector3(moveQueueP1.Peek().x + xOff, 0.01f, moveQueueP1.Peek().y + yOff), Time.deltaTime * cursorSpeed * (0.8f + Mathf.Pow(moveQueueP1.Count, 1.5f) * 0.2f));
                    if (Vector2.Distance(new Vector2(cursorObjP1.transform.position.x, cursorObjP1.transform.position.z), new Vector2(moveQueueP1.Peek().x + xOff, moveQueueP1.Peek().y + yOff)) == 0f) moveQueueP1.Dequeue();
                }
                // Update Cursor Position P2
                if (moveQueueP2.Count > 0) {
                    cursorObjP2.transform.position = Vector3.MoveTowards(cursorObjP2.transform.position, new Vector3(moveQueueP2.Peek().x + xOff, 0.01f, moveQueueP2.Peek().y + yOff), Time.deltaTime * cursorSpeed * (0.8f + Mathf.Pow(moveQueueP2.Count, 1.5f) * 0.2f));
                    if (Vector2.Distance(new Vector2(cursorObjP2.transform.position.x, cursorObjP2.transform.position.z), new Vector2(moveQueueP2.Peek().x + xOff, moveQueueP2.Peek().y + yOff)) == 0f) moveQueueP2.Dequeue();
                }

                // Check if ghost laser update needed
                if (!cursorP1.Equals(cursorP1Last)) { ghostLaser.ghostUpdateNeeded = true; if (TutorialFramework.tutorialActive && gridManager.theGrid.tutorialObject != null) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().moveEvent(cursorP1, cursorP1Last); }
				else if (!cursorP2.Equals(cursorP2Last)) { ghostLaser.ghostUpdateNeeded = true; }
                cursorP1Last = new Cursor(cursorP1.x, cursorP1.y, cursorP1.direction, cursorP1.selection, cursorP1.state);
                cursorP2Last = new Cursor(cursorP2.x, cursorP2.y, cursorP2.direction, cursorP2.selection, cursorP2.state);

                // Move building color
                //P1
                if (cursorP1.state == State.moving || cursorP1.state == State.placingMove) {
                    if (!cursorP1.colored && gridManager.theGrid.prefabDictionary.ContainsKey(cursorP1.moveOrigin)) {
                        cursorP1.colored = true;
                        gridManager.theGrid.prefabDictionary[cursorP1.moveOrigin].GetComponent<Renderer>().material.color = new Vector4(1f, 1f, 0.5f, 1f);
                    }
                } else {
                    if (cursorP1.colored && gridManager.theGrid.prefabDictionary.ContainsKey(cursorP1.moveOrigin)) {
                        gridManager.theGrid.prefabDictionary[cursorP1.moveOrigin].GetComponent<Renderer>().material.color = new Vector4(1f, 1f, 1f, 1f);
                    }
                    cursorP1.colored = false;
                }
                //P2
                if (cursorP2.state == State.moving || cursorP2.state == State.placingMove) {
                    if (!cursorP2.colored && gridManager.theGrid.prefabDictionary.ContainsKey(cursorP2.moveOrigin)) {
                        cursorP2.colored = true;
                        gridManager.theGrid.prefabDictionary[cursorP2.moveOrigin].GetComponent<Renderer>().material.color = new Vector4(1f, 1f, 0.5f, 1f);
                    }
                } else {
                    if (cursorP2.colored && gridManager.theGrid.prefabDictionary.ContainsKey(cursorP2.moveOrigin)) {
                        gridManager.theGrid.prefabDictionary[cursorP2.moveOrigin].GetComponent<Renderer>().material.color = new Vector4(1f, 1f, 1f, 1f);
                    }
                    cursorP2.colored = false;
                }
            }
		}
        if (TutorialFramework.skipFrame) TutorialFramework.skipFrame = false;
        if (pauseMenu.skipFrame) pauseMenu.skipFrame = false;
	}

	private Building cycleToBuilding(int index)
	{
		switch(index) {
		case 0: return Building.Blocking;
		case 1: return Building.Reflecting;
		case 2: return Building.Refracting;
		case 3: return Building.Redirecting;
		}
		return Building.Resource;
	}

	public static bool validPlacement(int x, int y, Direction direction, Building building, int originX = -1, int originY = -1)
	{
		if (gridManager.theGrid.getBuilding(x, y) != Building.Empty || !gridManager.theGrid.probeGrid(x, y, direction, building, originX, originY)) return false;
		return true;
	}

	private void place(Player player, State currentState)
	{
        // Instant placement for refracting and blocking blocks (bypass rotation state)
        if (currentState == State.placeBase) {
			if (player == Player.PlayerOne) {
				if (cursorP1.x > 0) print("Base must be placed on the edge of the board");
				else if (cursorP1.y == 0 || cursorP1.y == yEnd) print("Base cannot be placed in corners");
				else
				{
					cursorP1.state = State.placeLaser; p1HasPlacedBase = true;
                    gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Base, Player.PlayerOne);

                    if (TutorialFramework.tutorialActive) {
                        gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().placedEvent(new XY(cursorP1.x, cursorP1.y), Building.Base);
                    }
				}
			} else {
				if (cursorP2.x < xEnd) print("Base must be placed on the edge of the board");
				else if (cursorP2.y == 0 || cursorP2.y == yEnd) print("Base cannot be placed in corners");
				else {
					cursorP2.state = State.placeLaser; p2HasPlacedBase = true;
                    gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Base, Player.PlayerTwo);

				}
			}

		} else if (currentState == State.placeLaser && p1HasPlacedBase && p2HasPlacedBase) {
			if (player == Player.PlayerOne) {
				if (cursorP1.x > 0) print("Laser must be placed on the edge of the board");
				else {
					if (!validPlacement(cursorP1.x, cursorP1.y, Direction.Right, Building.Laser)){ print("Laser can not be placed that close to the base.");   }
					else{ cursorP1.state = State.placingLaser; if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().placingEvent(new XY(cursorP1.x, cursorP1.y), Building.Laser); }
				}
			} else {
				if (cursorP2.x < xEnd) print("Laser must be placed on the edge of the board");
				else {
					if (!validPlacement(cursorP2.x, cursorP2.y, Direction.Left, Building.Laser)){ print("Laser can not be placed that close to the base.");   }
					else{ cursorP2.state = State.placingLaser; }
				}
			}

		} else if (currentState == State.placingLaser) {
            if (player == Player.PlayerOne) {
				if (cursorP1.direction == Direction.Up && cursorP1.y != yEnd && cursorP1.y != yEnd - 1) { if (gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Laser, Player.PlayerOne, Direction.Up)) { laserLogic.laserHeadingP1 = Direction.NE; cursorP1.state = State.idle; cursorP1.direction = Direction.Down; } else { cursorP1.state = State.placeLaser; } }
				else if (cursorP1.direction == Direction.Down && cursorP1.y != 0 && cursorP1.y != 1) { if (gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Laser, Player.PlayerOne, Direction.Down)) { laserLogic.laserHeadingP1 = Direction.SE; cursorP1.state = State.idle; cursorP1.direction = Direction.Down; } else { cursorP1.state = State.placeLaser; } }
				else if (cursorP1.y != 0 && cursorP1.y != 1) { if (gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Laser, Player.PlayerOne, Direction.Down)) { laserLogic.laserHeadingP1 = Direction.SE; cursorP1.state = State.idle; cursorP1.direction = Direction.Down; } else { cursorP1.state = State.placeLaser; } }
				else if (cursorP1.y == 0 || cursorP1.y == 1) { if (gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Laser, Player.PlayerOne, Direction.Up)) { laserLogic.laserHeadingP1 = Direction.NE; cursorP1.state = State.idle; cursorP1.direction = Direction.Down; } else { cursorP1.state = State.placeLaser; } }
				else print("Press the up or down direction keys to place laser");
				if (cursorP1.state == State.idle) if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().placedEvent(new XY(cursorP1.x, cursorP1.y), Building.Laser);
			} else {
				if (cursorP2.direction == Direction.Up && cursorP2.y != yEnd && cursorP2.y != yEnd - 1) { if (gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Laser, Player.PlayerTwo, Direction.Up)) { laserLogic.laserHeadingP2 = Direction.NW; cursorP2.state = State.idle; cursorP2.direction = Direction.Down; } else { cursorP2.state = State.placeLaser; } }
				else if (cursorP2.direction == Direction.Down && cursorP2.y != 0 && cursorP2.y != 1) { if (gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Laser, Player.PlayerTwo, Direction.Down)) { laserLogic.laserHeadingP2 = Direction.SW; cursorP2.state = State.idle; cursorP2.direction = Direction.Down; } else { cursorP2.state = State.placeLaser; } }
				else if (cursorP2.y != 0 && cursorP2.y != 1) { if (gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Laser, Player.PlayerTwo, Direction.Down)) { laserLogic.laserHeadingP2 = Direction.SW; cursorP2.state = State.idle; cursorP2.direction = Direction.Down; } else { cursorP2.state = State.placeLaser; } }
				else if (cursorP2.y == 0 || cursorP2.y == 1) { if (gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Laser, Player.PlayerTwo, Direction.Up)) { laserLogic.laserHeadingP2 = Direction.NW; cursorP2.state = State.idle; cursorP2.direction = Direction.Down; } else { cursorP2.state = State.placeLaser; } }
				else print("Press the up or down direction keys to place laser");
			}
		} else if (currentState == State.placing) {

			if (player == Player.PlayerOne) {
				if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) != Building.Empty) { print("You can not place here, selection is no longer empty"); cursorP1.state = State.idle;   }
				else { if (!gridManager.theGrid.placeBuilding(cursorP1.x, cursorP1.y, cursorP1.selection, Player.PlayerOne, cursorP1.direction)) { print("Placing failed."); } else { if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().placedEvent(new XY(cursorP1.x, cursorP1.y), cursorP1.selection); }
				cursorP1.state = State.idle;   }
			} else {
				if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) != Building.Empty) { print("You can not place here, selection is no longer empty"); cursorP2.state = State.idle;   }
				else { if (!gridManager.theGrid.placeBuilding(cursorP2.x, cursorP2.y, cursorP2.selection, Player.PlayerTwo, cursorP2.direction)) print("Placing failed."); cursorP2.state = State.idle;   }
			}
		} else if (currentState == State.idle) {
			if (player == Player.PlayerOne) {
				if (!validPlacement(cursorP1.x, cursorP1.y, Direction.None, cursorP1.selection)){print("You can not place here, selection is not valid"); SoundManager.PlaySound(Sounds[4].audioclip, .4f, true, .95f, 1.05f);}
				else if (gridManager.theGrid.getCost(cursorP1.selection, cursorP1.x, Player.PlayerOne) <= gridManager.theGrid.getResourcesP1()){
					cursorP1.state = State.placing;
					if (cursorP1.selection == Building.Resource && laserLogic.laserData.grid[cursorP1.y, cursorP1.x].Count > 0) {
						Direction dir = ghostLaser.opposite(laserLogic.laserData.grid[cursorP1.y, cursorP1.x][0].getMarchDir());
                        if (!gridManager.theGrid.placeBuilding(cursorP1.x, cursorP1.y, Building.Resource, Player.PlayerOne, dir)) print("Placing failed.");
                        cursorP1.state = State.idle;
                    }
				}
				else {print("Not enough resources to place."); SoundManager.PlaySound(Sounds[4].audioclip, .4f, true, .95f, 1.05f);  }
			} else {
				if (!validPlacement(cursorP2.x, cursorP2.y, Direction.None, cursorP2.selection)){ print("You can not place here, selection is not valid"); SoundManager.PlaySound(Sounds[4].audioclip, .4f, true, .95f, 1.05f);}
				else if (gridManager.theGrid.getCost(cursorP2.selection, cursorP2.x, Player.PlayerTwo) <= gridManager.theGrid.getResourcesP2()){
					cursorP2.state = State.placing;
					if (cursorP2.selection == Building.Resource && laserLogic.laserData.grid[cursorP2.y, cursorP2.x].Count > 0) {
                        Direction dir = ghostLaser.opposite(laserLogic.laserData.grid[cursorP2.y, cursorP2.x][0].getMarchDir());
                        if (!gridManager.theGrid.placeBuilding(cursorP2.x, cursorP2.y, Building.Resource, Player.PlayerTwo, dir)) print("Placing failed.");
                        cursorP2.state = State.idle;
                    }
				}
				else{ print("Not enough resources to place.");   SoundManager.PlaySound(Sounds[4].audioclip, .4f, true, .95f, 1.05f);}
			}
		} else {
			print("Can not place, busy with some other action.");   
		}

	}

	private void move(Player player, State currentState)
	{
		if (currentState == State.moving) {

			if (player == Player.PlayerOne) {
				if (!validPlacement(cursorP1.x, cursorP1.y, Direction.None, cursorP1.moveBuilding, cursorP1.moveOrigin.x, cursorP1.moveOrigin.y) && !new XY(cursorP1.x, cursorP1.y).Equals(cursorP1.moveOrigin)){ print("You can not move to here, selection is not valid");  }
				else if (gridManager.theGrid.getCost(cursorP1.moveBuilding, cursorP1.x, Player.PlayerOne, true) < gridManager.theGrid.getResourcesP1()) {
                    cursorP1.state = State.placingMove;
                    if (cursorP1.moveBuilding == Building.Resource && laserLogic.laserData.grid[cursorP1.y, cursorP1.x].Count > 0) {
                        Direction dir = ghostLaser.opposite(laserLogic.laserData.grid[cursorP1.y, cursorP1.x][0].getMarchDir());
                        if (!gridManager.theGrid.moveBuilding(cursorP1.moveOrigin.x, cursorP1.moveOrigin.y, cursorP1.x, cursorP1.y, Player.PlayerOne, dir)) print("Moving failed.");
                        cursorP1.state = State.idle;
                    }
                    if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().movingPlacingEvent(new XY(cursorP1.x, cursorP1.y), cursorP1.moveBuilding);
                } else print("Not enough resources to move.");
			} else {
				if (!validPlacement(cursorP2.x, cursorP2.y, Direction.None, cursorP2.moveBuilding, cursorP2.moveOrigin.x, cursorP2.moveOrigin.y) && !new XY(cursorP2.x, cursorP2.y).Equals(cursorP2.moveOrigin)){ print("You can not move to here, selection is not valid");   }
				else if (gridManager.theGrid.getCost(cursorP2.moveBuilding, cursorP2.x, Player.PlayerTwo, true) < gridManager.theGrid.getResourcesP2()) {
                    cursorP2.state = State.placingMove;
                    if (cursorP2.moveBuilding == Building.Resource && laserLogic.laserData.grid[cursorP2.y, cursorP2.x].Count > 0) {
                        Direction dir = ghostLaser.opposite(laserLogic.laserData.grid[cursorP2.y, cursorP2.x][0].getMarchDir());
                        if (!gridManager.theGrid.moveBuilding(cursorP1.moveOrigin.x, cursorP1.moveOrigin.y, cursorP2.x, cursorP2.y, Player.PlayerTwo, dir)) print("Moving failed.");
                        cursorP2.state = State.idle;
                    }
                } else print("Not enough resources to move.");
			}
		} else if (currentState == State.placingMove) {
			if (player == Player.PlayerOne) {
				if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) != Building.Empty && !cursorP1.moveOrigin.Equals(new XY(cursorP1.x, cursorP1.y))) { print("You can not move here, selection is no longer empty"); cursorP1.state = State.idle;    }
				else {
					if (!gridManager.theGrid.moveBuilding(cursorP1.moveOrigin.x, cursorP1.moveOrigin.y, cursorP1.x, cursorP1.y, Player.PlayerOne, cursorP1.direction)) { print("Moving failed."); } else { if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().movedEvent(new XY(cursorP1.x, cursorP1.y), cursorP1.moveBuilding); }
					cursorP1.state = State.idle;   }
			} else {
				if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) != Building.Empty && !cursorP2.moveOrigin.Equals(new XY(cursorP2.x, cursorP2.y))) { print("You can not move here, selection is no longer empty"); cursorP2.state = State.idle;   }
				else { if (!gridManager.theGrid.moveBuilding(cursorP2.moveOrigin.x, cursorP2.moveOrigin.y, cursorP2.x, cursorP2.y, Player.PlayerTwo, cursorP2.direction)) print("Moving failed."); cursorP2.state = State.idle;   }
			}
		} else if (currentState == State.idle) {
			if (player == Player.PlayerOne) {
				if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Empty || gridManager.theGrid.getCellInfo(cursorP1.x, cursorP1.y).owner != Player.PlayerOne) {print("Invalid move target.");  }
				else if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Laser) {print("Cannot move this building.");  }
				else {
                    cursorP1.moveOrigin = new XY(cursorP1.x, cursorP1.y);
                    cursorP1.moveBuilding = gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y);
                    cursorP1.state = State.moving;
                    if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().movingEvent(new XY(cursorP1.x, cursorP1.y), cursorP1.moveBuilding); }
			} else {
				if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Empty || gridManager.theGrid.getCellInfo(cursorP2.x, cursorP2.y).owner != Player.PlayerTwo){ print("Invalid move target.");  }
				else if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Laser) {print("Cannot move this building.");  }
				else {
                    cursorP2.moveOrigin = new XY(cursorP2.x, cursorP2.y);
                    cursorP2.moveBuilding = gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y);
                    cursorP2.state = State.moving;
                }
			}
		} else {
			print("Can not move, busy with some other action.");  
		}
	}

	private void remove(Player player, State currentState)
	{
		if(currentState == State.removing)
		{
			if (player == Player.PlayerOne)
			{
				if (!gridManager.theGrid.removeBuilding(cursorP1.x, cursorP1.y, Player.PlayerOne)) { print("Removing failed.");     }
				else { if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().removedEvent(new XY(cursorP1.x, cursorP1.y), gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y));}
				cursorP1.state = State.idle;
			}
			else
			{
				if (!gridManager.theGrid.removeBuilding(cursorP2.x, cursorP2.y, Player.PlayerTwo)){ print("Removing failed.");      }
				cursorP2.state = State.idle;
			}
		}
		else if (currentState == State.idle) {	
			if (player == Player.PlayerOne) {
				if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Empty){ print("Nothing to remove here.");  }
				else if (gridManager.theGrid.getCellInfo(cursorP1.x, cursorP1.y).owner != Player.PlayerOne){ print("You can not remove a building that you do not own.");   }
				else if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Laser) print("Cannot remove this building.");
				else { cursorP1.state = State.removing; if (TutorialFramework.tutorialActive) gridManager.theGrid.tutorialObject.GetComponent<TutorialFramework>().removingEvent(new XY(cursorP1.x, cursorP1.y), gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y)); }
			} else {
				if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Empty){ print("Nothing to remove here.");   }
				else if (gridManager.theGrid.getCellInfo(cursorP2.x, cursorP2.y).owner != Player.PlayerTwo){ print("You can not remove a building that you do not own.");   }
				else if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Laser){ print("Cannot remove this building.");   }
				else { cursorP2.state = State.removing; }
			}

		} else {
			print("Can not remove, busy with some other action.");

		}
	}
}
