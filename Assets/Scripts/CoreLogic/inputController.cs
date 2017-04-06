using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State { placeBase, placeLaser, placingLaser, placing, moving, placingMove, idle };

public class inputController : MonoBehaviour {
	
    // These should be gameObjects that contain a sprite renderer
    public GameObject cursorObjP1;
    public GameObject cursorObjP2;

    public GameObject indicatorP1;
    public GameObject indicatorP2;
    public GameObject LaserArrowP1;
    public GameObject LaserArrowP2;

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

    // Cursor movement speed
    private const float cursorSpeed = 8f;
    private float delayFactor = 1f / cursorSpeed;
    private float diagSpeed = 0.707f;

    // Variables used with cursor movement
    private float vertDelayP1 = 0f;
    private float vertCounterP1 = 0f;
    private float horDelayP1 = 0f;
    private float horCounterP1 = 0f;
    private bool vertMovingP1 = false;
    private bool horMovingP1 = false;
    private float vertDelayP2 = 0f;
    private float vertCounterP2 = 0f;
    private float horDelayP2 = 0f;
    private float horCounterP2 = 0f;
    private bool vertMovingP2 = false;
    private bool horMovingP2 = false;
    private bool flagVP1 = false;
    private bool flagHP1 = false;
    private bool flagVP2 = false;
    private bool flagHP2 = false;

	//List of Sounds
	public Audios[] setSounds;
	public static Audios[] Sounds;
	public Audios[] setUISounds;
	public static Audios[] UISounds;

    //Vars for UI
    public static playerOneUI p1UI;
    public static playerTwoUI p2UI;

    // Pause menu
    public GameObject PauseMenu;

    public struct Cursor
    {
        public int x, y;
        public XY moveOrigin;
        public Building moveBuilding;
        public Building selection;
        public Direction direction;
        public State state;
        public Cursor(int X, int Y, Direction dir, Building selected, State current)
        {
            x = X;
            y = Y;
            direction = dir;
            selection = selected;
            state = current;
            moveOrigin = new XY(-1, -1);
            moveBuilding = Building.Empty;
        }
    }

    private int clamp(int value, int min, int max) {
        if (value > max) return max;
        else if (value < min) return min;
        return value;
    }

    private bool isValid(State state, int value, int min, int max) {
        if (state == State.placeBase)
            return value < max && value > min;
        return value <= max && value >= min;
    }

    public static Cursor cursorP1, cursorP2;
    private int xEnd, yEnd;
    private int cycleP1, cycleP2;
    private bool p1HasPlacedBase = false, p2HasPlacedBase = false;
    private Queue<XY> moveQueueP1 = new Queue<XY>();
    private Queue<XY> moveQueueP2 = new Queue<XY>();

    void Start () {
		Sounds = setSounds;
		UISounds = setUISounds;
        //default values for Player 1 UI  
        p1UI = gameObject.AddComponent<playerOneUI>();
        p1UI.State = GameObject.Find("p1State").GetComponent<Text>();
        //default values for Player 2 UI  
        p2UI = gameObject.AddComponent<playerTwoUI>();
        p2UI.State = GameObject.Find("p2State").GetComponent<Text>();

        cycleP1 = 0;
        cycleP2 = 0;
        xEnd = gridManager.theGrid.getDimX() - 1;
        yEnd = gridManager.theGrid.getDimY() - 1;
        cursorP1 = new Cursor(0, 1, Direction.Right, Building.Blocking, State.placeBase);
        cursorP2 = new Cursor(xEnd, yEnd-1, Direction.Left, Building.Blocking, State.placeBase);
        PauseMenu = GameObject.Find("Pause Menu");
        // Set initial cursor positions
        cursorObjP1.transform.position = new Vector3(cursorP1.x + (-gridManager.theGrid.getDimX() / 2f + 0.5f), 0.01f, cursorP1.y + (-gridManager.theGrid.getDimY() / 2f + 0.5f));
        cursorObjP2.transform.position = new Vector3(cursorP2.x + (-gridManager.theGrid.getDimX() / 2f + 0.5f), 0.01f, cursorP2.y + (-gridManager.theGrid.getDimY() / 2f + 0.5f));
    }

    void Update()
    {
        bool notNow1 = false;
        bool notNow2 = false;
        // Check that the game isn't paused
        if (PauseMenu.activeInHierarchy == false)
        {
			// Cursor Selection P1
			if (Input.GetKeyDown ("1")) {cursorP1.selection = Building.Blocking; SoundManager.PlaySound (UISounds [0].audioclip, SoundManager.globalUISoundsVolume / 25, true, .95f, 1.05f);} 
			else if (Input.GetKeyDown ("2")) {cursorP1.selection = Building.Reflecting; SoundManager.PlaySound (UISounds [0].audioclip, SoundManager.globalUISoundsVolume / 25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("3")){ cursorP1.selection = Building.Refracting; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("4")) {cursorP1.selection = Building.Redirecting; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("5")){ cursorP1.selection = Building.Resource; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			
            // Cycle P1
			if (Input.GetButtonDown("cycleR_1"))
			{
				if (cursorP1.selection == Building.Resource) cursorP1.selection = Building.Blocking;
				else cursorP1.selection += 1;
				
                SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);
			}
			else if (Input.GetButtonDown("cycleL_1"))
			{
				if (cursorP1.selection == Building.Blocking) cursorP1.selection = Building.Redirecting;
				else cursorP1.selection -= 1;
				
                SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);
			}

			// Cursor Selection P2
			if (Input.GetKeyDown("7")){ cursorP2.selection = Building.Blocking; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("8")){ cursorP2.selection = Building.Reflecting; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("9")){ cursorP2.selection = Building.Refracting; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("0")){ cursorP2.selection = Building.Redirecting; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			else if (Input.GetKeyDown("-")){ cursorP2.selection = Building.Resource; SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);}
			
            // Cycle P2
			if (Input.GetButtonDown("cycleR_2"))
			{
				if (cursorP2.selection == Building.Resource) cursorP2.selection = Building.Blocking;
				else cursorP2.selection += 1;
                
                SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);
			}
			else if (Input.GetButtonDown("cycleL_2"))
			{
				if (cursorP2.selection == Building.Blocking) cursorP2.selection = Building.Redirecting;
				else cursorP2.selection -= 1;
                
                SoundManager.PlaySound(UISounds[0].audioclip, SoundManager.globalUISoundsVolume/25, true, .95f, 1.05f);
			}
            if (cursorP1.state != State.placing && cursorP1.state != State.placingLaser && cursorP1.state != State.placingMove)
            {
                // Cursor Movement P1
                if (Input.GetAxis("xboxLeftVert") != 0) { vertDelayP1 = delayFactor; if (!flagVP1) { vertCounterP1 = 1f / cursorSpeed; flagVP1 = true; } } else { flagVP1 = false; }
                if (Input.GetAxis("xboxLeftHor") != 0) { horDelayP1 = delayFactor; if (!flagHP1) { horCounterP1 = 1f / cursorSpeed; flagHP1 = true; } } else { flagHP1 = false; }

                if (Input.GetButtonDown("up_1")) { if (isValid(cursorP1.state, cursorP1.y + 1, 0, yEnd)) { cursorP1.y += 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } }
                else if (Input.GetButton("up_1") || Input.GetAxis("xboxLeftVert") == 1) { vertMovingP1 = true; vertDelayP1 += Time.deltaTime; if (vertDelayP1 >= delayFactor) {vertCounterP1 += (!horMovingP1 || (cursorP1.x == xEnd || cursorP1.x == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (vertCounterP1 >= 1f / cursorSpeed) { if (isValid(cursorP1.state, cursorP1.y + 1, 0, yEnd)) { cursorP1.y += 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } vertCounterP1 = 0f; } } }
                else if (Input.GetButtonDown("down_1")) { if (isValid(cursorP1.state, cursorP1.y - 1, 0, yEnd)) { cursorP1.y -= 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } }
                else if (Input.GetButton("down_1") || Input.GetAxis("xboxLeftVert") == -1) { vertMovingP1 = true; vertDelayP1 += Time.deltaTime; if (vertDelayP1 >= delayFactor) { vertCounterP1 += (!horMovingP1 || (cursorP1.x == xEnd || cursorP1.x == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (vertCounterP1 >= 1f / cursorSpeed) { if (isValid(cursorP1.state, cursorP1.y - 1, 0, yEnd)) { cursorP1.y -= 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } vertCounterP1 = 0f; } } }
                else { vertCounterP1 = 0f; vertDelayP1 = 0f; vertMovingP1 = false; }

                if ((Input.GetButtonDown("right_1")) && (cursorP1.state != State.placeLaser && cursorP1.state != State.placeBase)) { if (isValid(cursorP1.state, cursorP1.x + 1, 0, xEnd)) { cursorP1.x += 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } }
                else if ((Input.GetButton("right_1") || Input.GetAxis("xboxLeftHor") == 1) && (cursorP1.state != State.placeLaser && cursorP1.state != State.placeBase)) { horMovingP1 = true; horDelayP1 += Time.deltaTime; if (horDelayP1 >= delayFactor) { horCounterP1 += (!vertMovingP1 || (cursorP1.y == yEnd || cursorP1.y == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (horCounterP1 >= 1f / cursorSpeed) { if (isValid(cursorP1.state, cursorP1.x + 1, 0, xEnd)) { cursorP1.x += 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } horCounterP1 = 0f; } } }
                else if (Input.GetButtonDown("left_1")) { if (isValid(cursorP1.state, cursorP1.x - 1, 0, xEnd)) { cursorP1.x -= 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } }
                else if (Input.GetButton("left_1") || Input.GetAxis("xboxLeftHor") == -1) { horDelayP1 += Time.deltaTime; if (horDelayP1 >= delayFactor) { horMovingP1 = true; horCounterP1 += (!vertMovingP1 || (cursorP1.y == yEnd || cursorP1.y == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (horCounterP1 >= 1f / cursorSpeed) { if (isValid(cursorP1.state, cursorP1.x - 1, 0, xEnd)) { cursorP1.x -= 1; moveQueueP1.Enqueue(new XY(cursorP1.x, cursorP1.y)); } horCounterP1 = 0f; } } }
                else { horCounterP1 = 0f; horDelayP1 = 0f; horMovingP1 = false; }
            }
            else
            {
                // Cursor Rotation P1
                bool selectionMade = false;
                if (Input.GetButtonDown("up_1") || Input.GetAxis("xboxLeftVert") == 1) { cursorP1.direction = Direction.Up; }
                else if (Input.GetButtonDown("down_1") || Input.GetAxis("xboxLeftVert") == -1) { cursorP1.direction = Direction.Down; }
                else if (Input.GetButtonDown("right_1") || Input.GetAxis("xboxLeftHor") == 1) { cursorP1.direction = Direction.Right; }
                else if ((Input.GetButtonDown("left_1") || Input.GetAxis("xboxLeftHor") == -1)) { cursorP1.direction = Direction.Left; }
                if (Input.GetButtonDown("place_1")) { selectionMade = true; notNow1 = true; }
                if (selectionMade)
                { // If placing or moving, finalize action
                    if (cursorP1.state == State.placingMove) move(Player.PlayerOne, cursorP1.state);
                    else place(Player.PlayerOne, cursorP1.state);
                }
            }

            if (cursorP2.state != State.placing && cursorP2.state != State.placingLaser && cursorP2.state != State.placingMove)
            {
                // Cursor Movement P2
                if (Input.GetAxis("xboxLeftVert2") != 0) { vertDelayP2 = delayFactor; if (!flagVP2) { vertCounterP2 = 1f / cursorSpeed; flagVP2 = true; } } else { flagVP2 = false; }
                if (Input.GetAxis("xboxLeftHor2") != 0) { horDelayP2 = delayFactor; if (!flagHP2) { horCounterP2 = 1f / cursorSpeed; flagHP2 = true; } } else { flagHP2 = false; }

                if (Input.GetButtonDown("up_2")) { if (isValid(cursorP2.state, cursorP2.y + 1, 0, yEnd)) { cursorP2.y += 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } }
                else if (Input.GetButton("up_2") || Input.GetAxis("xboxLeftVert2") == 1) { vertMovingP2 = true; vertDelayP2 += Time.deltaTime; if (vertDelayP2 >= delayFactor) { vertCounterP2 += (!horMovingP2 || (cursorP2.x == xEnd || cursorP2.x == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (vertCounterP2 >= 1f / cursorSpeed) { if (isValid(cursorP2.state, cursorP2.y + 1, 0, yEnd)) { cursorP2.y += 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } vertCounterP2 = 0f; } } }
                else if (Input.GetButtonDown("down_2")) { if (isValid(cursorP2.state, cursorP2.y - 1, 0, yEnd)) { cursorP2.y -= 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } }
                else if (Input.GetButton("down_2") || Input.GetAxis("xboxLeftVert2") == -1) { vertMovingP2 = true; vertDelayP2 += Time.deltaTime; if (vertDelayP2 >= delayFactor) { vertCounterP2 += (!horMovingP2 || (cursorP2.x == xEnd || cursorP2.x == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (vertCounterP2 >= 1f / cursorSpeed) { if (isValid(cursorP2.state, cursorP2.y - 1, 0, yEnd)) { cursorP2.y -= 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } vertCounterP2 = 0f; } } }
                else { vertCounterP2 = 0f; vertDelayP2 = 0f; vertMovingP2 = false; }

                if (Input.GetButtonDown("right_2")) { if (isValid(cursorP2.state, cursorP2.x + 1, 0, xEnd)) { cursorP2.x += 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } }
                else if (Input.GetButton("right_2") || Input.GetAxis("xboxLeftHor2") == 1) { horMovingP2 = true; horDelayP2 += Time.deltaTime; if (horDelayP2 >= delayFactor) { horCounterP2 += (!vertMovingP2 || (cursorP2.y == yEnd || cursorP2.y == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (horCounterP2 >= 1f / cursorSpeed) { if (isValid(cursorP2.state, cursorP2.x + 1, 0, xEnd)) { cursorP2.x += 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } horCounterP2 = 0f; } } }
                else if (Input.GetButtonDown("left_2") && (cursorP2.state != State.placeLaser && cursorP2.state != State.placeBase)) { if (isValid(cursorP2.state, cursorP2.x - 1, 0, xEnd)) { cursorP2.x -= 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } }
                else if ((Input.GetButton("left_2") || Input.GetAxis("xboxLeftHor2") == -1) && (cursorP2.state != State.placeLaser && cursorP2.state != State.placeBase)) { horDelayP2 += Time.deltaTime; if (horDelayP2 >= delayFactor) { horMovingP2 = true; horCounterP2 += (!vertMovingP2 || (cursorP2.y == yEnd || cursorP2.y == 0)) ? Time.deltaTime : Time.deltaTime * diagSpeed; if (horCounterP2 >= 1f / cursorSpeed) { if (isValid(cursorP2.state, cursorP2.x - 1, 0, xEnd)) { cursorP2.x -= 1; moveQueueP2.Enqueue(new XY(cursorP2.x, cursorP2.y)); } horCounterP2 = 0f; } } }
                else { horCounterP2 = 0f; horDelayP2 = 0f; horMovingP2 = false; }
            }
            else
            {
                // Cursor Rotation P2
                bool selectionMade = false;
                if (Input.GetButtonDown("up_2") || Input.GetAxis("xboxLeftVert2") == 1) { cursorP2.direction = Direction.Up; }
                else if (Input.GetButtonDown("down_2") || Input.GetAxis("xboxLeftVert2") == -1) { cursorP2.direction = Direction.Down; }
                else if (Input.GetButtonDown("right_2") || Input.GetAxis("xboxLeftHor2") == 1) { cursorP2.direction = Direction.Right; }
                else if (Input.GetButtonDown("left_2") || Input.GetAxis("xboxLeftHor2") == -1) { cursorP2.direction = Direction.Left; }
                if (Input.GetButtonDown("place_2")) { selectionMade = true; notNow2 = true; }
                if (selectionMade)
                { // If placing or moving, finalize action
                    if (cursorP2.state == State.placingMove) move(Player.PlayerTwo, cursorP2.state);
                    else place(Player.PlayerTwo, cursorP2.state);
                }
            }

            // Cursor Functions P1
            if ((Input.GetButtonDown("place_1")) && !notNow1) place(Player.PlayerOne, cursorP1.state);
            else if (Input.GetButtonDown("move_1")) move(Player.PlayerOne, cursorP1.state);
            else if (Input.GetButtonDown("remove_1")) remove(Player.PlayerOne, cursorP1.state);
            else if (Input.GetButtonDown("cancel_1") && cursorP1.state != State.placeBase && cursorP1.state != State.placeLaser) cursorP1.state = cursorP1.state == State.placingLaser ? State.placeLaser : State.idle;

            // Cursor Functions P2
            if (Input.GetButtonDown("place_2") && !notNow2) place(Player.PlayerTwo, cursorP2.state);
            else if (Input.GetButtonDown("move_2")) move(Player.PlayerTwo, cursorP2.state);
            else if (Input.GetButtonDown("remove_2")) remove(Player.PlayerTwo, cursorP2.state);
            else if (Input.GetButtonDown("cancel_2") && cursorP2.state != State.placeBase && cursorP2.state != State.placeLaser) cursorP2.state = cursorP2.state == State.placingLaser ? State.placeLaser : State.idle;

            // Update Cursor/UI Appearance P1
            if (cursorP1.state == State.placeBase) { cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1BaseSprite; p1UI.State.text = "Place base on the current column \nPress [e] to place base"; }
            else if (cursorP1.state == State.placeLaser)
            {
                cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1LaserSprite;
                p1UI.State.text = "Press [e] to place laser \nPress [w] or [s] for direction \n[e] to confirm";
            }
            else if(cursorP1.state == State.placingLaser) {
                switch (cursorP1.direction)
                {
                    case Direction.Up: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[5][1]; break;
                    case Direction.Down: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[5][0];  break;
                }
                
                p1UI.State.text = "Press [e] to place laser \nPress [w] or [s] for direction \n[e] to confirm";
            }
            else if (cursorP1.state == State.placing) // in here change the sprite while choosing direction
            {
                float scale = 1f;
                switch (cursorP1.selection)
                {
                    case Building.Blocking: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[0][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .15f; break;
                    case Building.Reflecting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[1][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .4f; break;
                    case Building.Refracting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[2][0]; scale = .2f; break;
                    case Building.Redirecting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[3][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .25f; break;
                    case Building.Resource: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[4][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .3f; break;
                }
                cursorObjP1.GetComponent<Renderer>().material.color = new Vector4(1f, 0.7f, 0.7f, .5f);
                cursorObjP1.transform.localScale = new Vector3(scale, scale, scale);
            }
            else if (cursorP1.state == State.placingMove) // in here change the sprite while choosing direction
            {
                float scale = 1f;
                switch (cursorP1.moveBuilding)
                {
                    case Building.Blocking: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[0][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .15f; break;
                    case Building.Reflecting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[1][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .4f; break;
                    case Building.Refracting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[2][0]; scale = .2f; break;
                    case Building.Redirecting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[3][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .25f; break;
                    case Building.Resource: cursorObjP1.GetComponent<SpriteRenderer>().sprite = cursorObjP1.GetComponent<cursor1>().Sprites[4][gridManager.theGrid.directionToIndex(cursorP1.direction)]; scale = .3f; break;
                }
                cursorObjP1.GetComponent<Renderer>().material.color = new Vector4(1f, 0.7f, 0.7f, .5f);
                cursorObjP1.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                float scale = 1f;
                switch (cursorP1.selection)
                {
                    case Building.Blocking: cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1BlockSprite; scale = .15f; break;
                    case Building.Reflecting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1ReflectSprite; scale = .4f; break;
                    case Building.Refracting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1RefractSprite; scale = .2f; break;
                    case Building.Redirecting: cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1RedirectSprite; scale = .25f;  break;
                    case Building.Resource: cursorObjP1.GetComponent<SpriteRenderer>().sprite = P1ResourceSprite; scale = .3f; break;
                }
                cursorObjP1.GetComponent<Renderer>().material.color = new Vector4(1f, 0.7f, 0.7f, .5f);
                cursorObjP1.transform.localScale = new Vector3(scale, scale, scale);
            }

            // Update Cursor/UI Appearance P2
            if (cursorP2.state == State.placeBase) { cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2BaseSprite; p2UI.State.text = "Place base on the current column \nPress [o] to place base"; }
            else if (cursorP2.state == State.placeLaser)
            {
                cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2LaserSprite;
                p2UI.State.text = "Press [e] to place laser \nPress [w] or [s] for direction \n[e] to confirm";
            }
            else if (cursorP2.state == State.placingLaser)
            {
                switch (cursorP2.direction)
                {
                    case Direction.Up: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[5][1]; break;
                    case Direction.Down: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[5][0]; break;
                }

                p2UI.State.text = "Press [e] to place laser \nPress [w] or [s] for direction \n[e] to confirm";
            }
            else if (cursorP2.state == State.placing) // in here change the sprite while choosing direction
            {
                float scale = 1f;
                switch (cursorP2.selection)
                {
                    case Building.Blocking: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[0][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .15f; break;
                    case Building.Reflecting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[1][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .4f; break;
                    case Building.Refracting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[2][0]; scale = .25f; break;
                    case Building.Redirecting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[3][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .25f; break;
                    case Building.Resource: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[4][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .3f; break;
                }
                cursorObjP2.GetComponent<Renderer>().material.color = new Vector4(0.7f, 1f, 0.7f, .5f);
                cursorObjP2.transform.localScale = new Vector3(scale, scale, scale);
            }
            else if (cursorP2.state == State.placingMove) // in here change the sprite while choosing direction
            {
                float scale = 1f;
                switch (cursorP2.moveBuilding)
                {
                    case Building.Blocking: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[0][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .15f; break;
                    case Building.Reflecting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[1][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .4f; break;
                    case Building.Refracting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[2][0]; scale = .2f; break;
                    case Building.Redirecting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[3][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .25f; break;
                    case Building.Resource: cursorObjP2.GetComponent<SpriteRenderer>().sprite = cursorObjP2.GetComponent<cursor1>().Sprites[4][gridManager.theGrid.directionToIndex(cursorP2.direction)]; scale = .3f; break;
                }
                cursorObjP2.GetComponent<Renderer>().material.color = new Vector4(1f, 0.7f, 0.7f, .5f);
                cursorObjP2.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                float scale = 1f;
                switch (cursorP2.selection)
                {
                    case Building.Blocking: cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2BlockSprite; scale = .15f; break;
                    case Building.Reflecting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2ReflectSprite; scale = .4f; break;
                    case Building.Refracting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2RefractSprite; scale = .2f; break;
                    case Building.Redirecting: cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2RedirectSprite; scale = .25f; break;
                    case Building.Resource: cursorObjP2.GetComponent<SpriteRenderer>().sprite = P2ResourceSprite; scale = .3f; break;
                }
                cursorObjP2.GetComponent<Renderer>().material.color = new Vector4(0.7f, 1, 0.7f, .5f);
                cursorObjP2.transform.localScale = new Vector3(scale, scale, scale);
            }

            float xOff = -gridManager.theGrid.getDimX() / 2f + 0.5f;
            float yOff = -gridManager.theGrid.getDimY() / 2f + 0.5f;

            // Update Cursor Position P1
            if (moveQueueP1.Count > 0)
            {
                cursorObjP1.transform.position = Vector3.MoveTowards(cursorObjP1.transform.position, new Vector3(moveQueueP1.Peek().x + xOff, 0.01f, moveQueueP1.Peek().y + yOff), Time.deltaTime * cursorSpeed * (0.8f + Mathf.Pow(moveQueueP1.Count, 1.5f) * 0.2f));
                if (Vector2.Distance(new Vector2(cursorObjP1.transform.position.x, cursorObjP1.transform.position.z), new Vector2(moveQueueP1.Peek().x + xOff, moveQueueP1.Peek().y + yOff)) == 0f) moveQueueP1.Dequeue();
            }
            // Update Cursor Position P2
            if (moveQueueP2.Count > 0)
            {
                cursorObjP2.transform.position = Vector3.MoveTowards(cursorObjP2.transform.position, new Vector3(moveQueueP2.Peek().x + xOff, 0.01f, moveQueueP2.Peek().y + yOff), Time.deltaTime * cursorSpeed * (0.8f + Mathf.Pow(moveQueueP2.Count, 1.5f) * 0.2f));
                if (Vector2.Distance(new Vector2(cursorObjP2.transform.position.x, cursorObjP2.transform.position.z), new Vector2(moveQueueP2.Peek().x + xOff, moveQueueP2.Peek().y + yOff)) == 0f) moveQueueP2.Dequeue();
            }

            // Update Cursor Indicator
            if (cursorP1.state == State.placing)
            {
                indicatorP1.GetComponent<SpriteRenderer>().enabled = true;
                LaserArrowP1.GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (cursorP1.state == State.placeLaser)
            {
                LaserArrowP1.GetComponent<SpriteRenderer>().enabled = true;
                indicatorP1.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                LaserArrowP1.GetComponent<SpriteRenderer>().enabled = false;
                indicatorP1.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (cursorP2.state == State.placing)
            {
                indicatorP2.GetComponent<SpriteRenderer>().enabled = true;
                LaserArrowP2.GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (cursorP2.state == State.placeLaser)
            {
                LaserArrowP2.GetComponent<SpriteRenderer>().enabled = true;
                indicatorP2.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                LaserArrowP2.GetComponent<SpriteRenderer>().enabled = false;
                indicatorP2.GetComponent<SpriteRenderer>().enabled = false;
            }

            // Cursor sound effect
            if ((Input.GetButtonDown("up_1") || Input.GetButtonDown("down_1") || Input.GetButtonDown("right_1") || Input.GetButtonDown("left_1") && moveQueueP1.Count > 0) ||
                (Input.GetButtonDown("up_2") || Input.GetButtonDown("down_2") || Input.GetButtonDown("right_2") || Input.GetButtonDown("left_2") && moveQueueP2.Count > 0)) {
				SoundManager.PlaySound(Sounds[0].audioclip, SoundManager.globalSoundsVolume/25, true, .95f, 1.05f);

            }
        }
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

    private bool validPlacement(int x, int y, Direction direction, Building building)
    {
        if (gridManager.theGrid.getBuilding(x, y) != Building.Empty || !gridManager.theGrid.probeGrid(x, y, direction, building)) return false;
        if (gridManager.theGrid.prefabDictionary.ContainsKey(new XY(x, y))) return false;
        return true;
    }

    private void place(Player player, State currentState)
    {
        //print(gridManager.theGrid.placementList.Count);
        // Instant placement for refracting and blocking blocks (bypass rotation state)
        if (currentState == State.idle && ((player == Player.PlayerOne ? cursorP1.selection == Building.Refracting : cursorP2.selection == Building.Refracting) || (player == Player.PlayerOne ? cursorP1.selection == Building.Blocking : cursorP2.selection == Building.Blocking))) {
            currentState = State.placing;
            if (player == Player.PlayerOne) cursorP1.direction = Direction.Down;
            else cursorP2.direction = Direction.Down;
        }
        if (currentState == State.placeBase) {
            if (player == Player.PlayerOne) {
                if (cursorP1.x > 0) print("Base must be placed on the edge of the board");
                else if (cursorP1.y == 0 || cursorP1.y == yEnd) print("Base cannot be placed in corners");
                else
                {
                    gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Base, Player.PlayerOne);
                    cursorP1.state = State.placeLaser; p1HasPlacedBase = true;
                }
            } else {
                if (cursorP2.x < xEnd) print("Base must be placed on the edge of the board");
                else if (cursorP2.y == 0 || cursorP2.y == yEnd) print("Base cannot be placed in corners");
                else {
                    gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Base, Player.PlayerTwo);
                    cursorP2.state = State.placeLaser; p2HasPlacedBase = true;
                }
            }
			SoundManager.PlaySound(Sounds[2].audioclip, SoundManager.globalSoundsVolume/25, true, .95f, 1.05f);
        } else if (currentState == State.placeLaser && p1HasPlacedBase && p2HasPlacedBase) {
            if (player == Player.PlayerOne) {
                if (cursorP1.x > 0) print("Laser must be placed on the edge of the board");
                else {
                    if (!validPlacement(cursorP1.x, cursorP1.y, Direction.Right, Building.Laser)) print("Laser can not be placed that close to the base.");
                    else cursorP1.state = State.placingLaser;
                }
            } else {
                if (cursorP2.x < xEnd) print("Laser must be placed on the edge of the board");
                else {
                    if (!validPlacement(cursorP2.x, cursorP2.y, Direction.Left, Building.Laser)) print("Laser can not be placed that close to the base.");
                    else cursorP2.state = State.placingLaser;
                }
            }
			SoundManager.PlaySound(Sounds[2].audioclip, SoundManager.globalSoundsVolume/25, true, .95f, 1.05f);
        } else if (currentState == State.placingLaser) {
            if (player == Player.PlayerOne) {
                p1UI.State.text = "Press [e] to place creatures \nPress WASD for direction \n[e] to confirm";
                if (cursorP1.direction == Direction.Up) { if (gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Laser, Player.PlayerOne, Direction.Up)) { laserLogic.laserHeadingP1 = Direction.NE; cursorP1.state = State.idle; } else { cursorP1.state = State.placeLaser; } }
                else if (cursorP1.direction == Direction.Down) { if (gridManager.theGrid.placeBuilding(0, cursorP1.y, Building.Laser, Player.PlayerOne, Direction.Down)) { laserLogic.laserHeadingP1 = Direction.SE; cursorP1.state = State.idle; } else { cursorP1.state = State.placeLaser; } }
                else print("Press the up or down direction keys to place laser");
            } else {
                p2UI.State.text = "Press [o] to place creatures \nPress IJKL for direction \n[o] to confirm";
                if (cursorP2.direction == Direction.Up) { if (gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Laser, Player.PlayerTwo, Direction.Up)) { laserLogic.laserHeadingP2 = Direction.NW; cursorP2.state = State.idle; } else { cursorP2.state = State.placeLaser; } }
                else if (cursorP2.direction == Direction.Down) { if (gridManager.theGrid.placeBuilding(xEnd, cursorP2.y, Building.Laser, Player.PlayerTwo, Direction.Down)) { laserLogic.laserHeadingP2 = Direction.SW; cursorP2.state = State.idle; } else { cursorP2.state = State.placeLaser; } }
                else print("Press the up or down direction keys to place laser");
            }
        } else if (currentState == State.placing) {
			SoundManager.PlaySound(Sounds[2].audioclip, SoundManager.globalSoundsVolume/25, true, .95f, 1.05f);
            if (player == Player.PlayerOne) {
                if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) != Building.Empty) { print("You can not place here, selection is no longer empty"); cursorP1.state = State.idle; }
                else { if (!gridManager.theGrid.placeBuilding(cursorP1.x, cursorP1.y, cursorP1.selection, Player.PlayerOne, cursorP1.direction)) print("Placing failed."); cursorP1.state = State.idle; }
            } else {
                if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) != Building.Empty) { print("You can not place here, selection is no longer empty"); cursorP2.state = State.idle; }
                else { if (!gridManager.theGrid.placeBuilding(cursorP2.x, cursorP2.y, cursorP2.selection, Player.PlayerTwo, cursorP2.direction)) print("Placing failed."); cursorP2.state = State.idle; }
            }
        } else if (currentState == State.idle) {
            if (player == Player.PlayerOne) {
                if (!validPlacement(cursorP1.x, cursorP1.y, Direction.None, cursorP1.selection)) print("You can not place here, selection is not valid");
                else if (gridManager.theGrid.getCost(cursorP1.selection, cursorP1.x, Player.PlayerOne) <= gridManager.theGrid.getResourcesP1()) cursorP1.state = State.placing;
                else print("Not enough resources to place.");
            } else {
                if (!validPlacement(cursorP2.x, cursorP2.y, Direction.None, cursorP2.selection)) print("You can not place here, selection is not valid");
                else if (gridManager.theGrid.getCost(cursorP2.selection, cursorP2.x, Player.PlayerTwo) <= gridManager.theGrid.getResourcesP2()) cursorP2.state = State.placing;
                else print("Not enough resources to place.");
            }
        } else {
            print("Can not place, busy with some other action.");
        }

    }

    private void move(Player player, State currentState)
    {
        if (currentState == State.moving) {
            if (player == Player.PlayerOne) {
                p1UI.State.text = "Press [u] to place creature \nPress WASD for direction \n[u] to confirm";
                if (!validPlacement(cursorP1.x, cursorP1.y, Direction.None, cursorP1.moveBuilding) && !new XY(cursorP1.x, cursorP1.y).Equals(cursorP1.moveOrigin)) print("You can not move to here, selection is not valid");
                else if (gridManager.theGrid.getCost(cursorP1.moveBuilding, cursorP1.x, Player.PlayerOne, true) < gridManager.theGrid.getResourcesP1()) cursorP1.state = State.placingMove;
                else print("Not enough resources to move.");
            } else {
                p2UI.State.text = "Press [u] to place creatures \nPress IJKL for direction \n[u] to confirm";
                if (!validPlacement(cursorP2.x, cursorP2.y, Direction.None, cursorP2.moveBuilding) && !new XY(cursorP2.x, cursorP2.y).Equals(cursorP2.moveOrigin)) print("You can not move to here, selection is not valid");
                else if (gridManager.theGrid.getCost(cursorP2.moveBuilding, cursorP2.x, Player.PlayerTwo, true) < gridManager.theGrid.getResourcesP2()) cursorP2.state = State.placingMove;
                else print("Not enough resources to move.");
            }
        } else if (currentState == State.placingMove) {
            if (player == Player.PlayerOne) {
                if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) != Building.Empty && !cursorP1.moveOrigin.Equals(new XY(cursorP1.x, cursorP1.y))) { print("You can not move here, selection is no longer empty"); cursorP1.state = State.idle; }
                else { if (!gridManager.theGrid.moveBuilding(cursorP1.moveOrigin.x, cursorP1.moveOrigin.y, cursorP1.x, cursorP1.y, Player.PlayerOne, cursorP1.direction)) print("Moving failed."); cursorP1.state = State.idle; }
            } else {
                if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) != Building.Empty && !cursorP2.moveOrigin.Equals(new XY(cursorP2.x, cursorP2.y))) { print("You can not move here, selection is no longer empty"); cursorP2.state = State.idle; }
                else { if (!gridManager.theGrid.moveBuilding(cursorP2.moveOrigin.x, cursorP2.moveOrigin.y, cursorP2.x, cursorP2.y, Player.PlayerTwo, cursorP2.direction)) print("Moving failed."); cursorP2.state = State.idle; }
            }
        } else if (currentState == State.idle) {
            if (player == Player.PlayerOne) {
                if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Empty || gridManager.theGrid.getCellInfo(cursorP1.x, cursorP1.y).owner != Player.PlayerOne) print("Invalid move target.");
                else if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Laser) print("Cannot move this building.");
                else { cursorP1.moveOrigin = new XY(cursorP1.x, cursorP1.y); cursorP1.moveBuilding = gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y); cursorP1.state = State.moving; }
            } else {
                if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Empty || gridManager.theGrid.getCellInfo(cursorP2.x, cursorP2.y).owner != Player.PlayerTwo) print("Invalid move target.");
                else if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Laser) print("Cannot move this building.");
                else { cursorP2.moveOrigin = new XY(cursorP2.x, cursorP2.y); cursorP2.moveBuilding = gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y); cursorP2.state = State.moving; }
            }
        } else {
            print("Can not move, busy with some other action.");
        }
    }

    private void remove(Player player, State currentState)
    {
        if (currentState == State.idle) {	
            if (player == Player.PlayerOne) {
                if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Empty) print("Nothing to remove here.");
                else if (gridManager.theGrid.getCellInfo(cursorP1.x, cursorP1.y).owner != Player.PlayerOne) print("You can not remove a building that you do not own.");
                else if (gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP1.x, cursorP1.y) == Building.Laser) print("Cannot remove this building.");
                else { if (!gridManager.theGrid.removeBuilding(cursorP1.x, cursorP1.y, Player.PlayerOne)) print("Removing failed."); }
            } else {
                if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Empty) print("Nothing to remove here.");
                else if (gridManager.theGrid.getCellInfo(cursorP2.x, cursorP2.y).owner != Player.PlayerTwo) print("You can not remove a building that you do not own.");
                else if (gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Base || gridManager.theGrid.getBuilding(cursorP2.x, cursorP2.y) == Building.Laser) print("Cannot remove this building.");
                else { if (!gridManager.theGrid.removeBuilding(cursorP2.x, cursorP2.y, Player.PlayerTwo)) print("Removing failed."); }
            }
			SoundManager.PlaySound(Sounds[3].audioclip, SoundManager.globalSoundsVolume/25, true, .95f, 1.05f);
        } else {
            print("Can not remove, busy with some other action.");
        }
    }
}
