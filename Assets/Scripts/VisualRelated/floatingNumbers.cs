using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to display floating damage numbers over buildings, you can find configurable sliders on the script under the Board object.

public struct healthBuilding
{
    public float time;
    public float lastHP;
    public Building building;
    public healthBuilding(float hp = 0f, float rate = 1f, Building build = Building.Empty)
    {
        lastHP = hp;
        time = rate;
        building = build;
    }
}

public struct resourceBuilding
{
    public float time;
    public float lastResource;
    public resourceBuilding(float res, float rate = 1f)
    {
        lastResource = res;
        time = rate;
    }
}

public struct damageResourceGrid
{
    public Sprite[] laserite;
    Dictionary<XY, healthBuilding> grid;
    Dictionary<XY, resourceBuilding> gridR;
    private GameObject gameObject;
    // Damage
    private Color p1DamageColor;
    private Color p2DamageColor;
    private float emissionRate;
    private float randomnessRange;
    private float textSize;
    private float textSpeed;
    private float textLifetime;
    // Resource
    private Color p1ResourceColor;
    private Color p2ResourceColor;
    private float emissionRateR;
    private float randomnessRangeR;
    private float textSizeR;
    private float textSpeedR;
    private float textLifetimeR;
    private Font font;
    private float numP1;
    private float numP2;

    public damageResourceGrid(Sprite[] laser, GameObject container, Color p1, Color p2, Font customFont, Color p1R, Color p2R, float rate = 1f, float randomAngle = 0f, float size = 1f, float speed = 1f, float life = 1f,
                              float rateR = 1f, float randomAngleR = 0f, float sizeR = 1f, float speedR = 1f, float lifeR = 1f)
    {
        gameObject = container;
        grid = new Dictionary<XY, healthBuilding>();
        gridR = new Dictionary<XY, resourceBuilding>();
        emissionRate = 1f / rate;
        randomnessRange = randomAngle * 0.5f;
        textSize = size * 0.04f;
        textSpeed = speed * 0.5f;
        textLifetime = life * 1.5f;
        p1DamageColor = p1;
        p2DamageColor = p2;
        if (customFont == null) customFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        font = customFont;
        // Resource stuff
        emissionRateR = 1f / rateR;
        randomnessRangeR = randomAngleR * 0.5f;
        textSizeR = sizeR * 0.04f;
        textSpeedR = speedR * 0.5f;
        textLifetimeR = lifeR * 1.5f;
        p1ResourceColor = p1R;
        p2ResourceColor = p2R;
        laserite = laser;
        numP1 = 0;
        numP2 = 0;
    }

    public void checkDamage(XY pos, float currHP, float maxHP, Building building, Player buildingOwner)
    {
        healthBuilding value = new healthBuilding();
        if (!grid.TryGetValue(pos, out value)) grid.Add(pos, new healthBuilding(maxHP, emissionRate, building)); // Add to gridInfo if not already there
        else if (value.building != building || value.lastHP < currHP) grid[pos] = new healthBuilding(maxHP, emissionRate, building); // Reset if building has changed
        else if (value.time <= 0f) {
            // Emit damage number
            GameObject child = new GameObject();
            child.transform.parent = gameObject.transform;
            child.transform.localEulerAngles = new Vector3(90f, 0, 0);
            child.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x, pos.y + 0.5f, 1f);
            child.AddComponent<Rigidbody>();
            Rigidbody rigidBody = child.GetComponent<Rigidbody>();

            rigidBody.useGravity = false;
            rigidBody.velocity = Quaternion.AngleAxis(Random.Range(-randomnessRange, randomnessRange), Vector3.up) * Vector3.forward * textSpeed;


            child.AddComponent<TextMesh>();
            TextMesh textMesh = child.GetComponent<TextMesh>();

            textMesh.fontSize = 64;
            textMesh.characterSize = textSize;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = buildingOwner == Player.PlayerOne ? p1DamageColor : p2DamageColor;
            textMesh.fontStyle = FontStyle.Bold;
            textMesh.font = font;
            textMesh.text = "- " + ((value.lastHP - currHP) * gridManager.hpScale).ToString("F1");

            MonoBehaviour.Destroy(child, textLifetime);

            // Update damageGrid data
            value.time = emissionRate;
            value.lastHP = currHP;
            grid[pos] = value;
        } else {
            grid[pos] = new healthBuilding(value.lastHP, value.time - Time.deltaTime, value.building);
        }
    }
    public void checkResource(XY pos, float currResource, Player buildingOwner, State state)
    {
        if (state != State.placing && state != State.removing && state != State.moving)
        {
            resourceBuilding value = new resourceBuilding();
            if (!gridR.TryGetValue(pos, out value)) { gridR.Add(pos, new resourceBuilding(currResource, emissionRateR)); if (buildingOwner == Player.PlayerOne) numP1 += 1; else numP2 += 1; } // Add to gridInfo if not already ther
            else if (currResource < value.lastResource) gridR[pos] = new resourceBuilding(currResource, emissionRateR);
            else if (value.time <= 0f)
            {
                // Emit resource number
                GameObject child = new GameObject();
                child.transform.parent = gameObject.transform;
                child.transform.localEulerAngles = new Vector3(90f, 0, 0);
                child.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x + 0.25f, pos.y + 0.5f, 1f);
                child.AddComponent<Rigidbody>();
                Rigidbody rigidBody = child.GetComponent<Rigidbody>();

                rigidBody.useGravity = false;
                rigidBody.velocity = Quaternion.AngleAxis(Random.Range(-randomnessRangeR, randomnessRangeR), Vector3.up) * Vector3.forward * textSpeedR;


                child.AddComponent<TextMesh>();
                TextMesh textMesh = child.GetComponent<TextMesh>();

                textMesh.fontSize = 64;
                textMesh.characterSize = textSizeR;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.color = buildingOwner == Player.PlayerOne ? p1ResourceColor : p2ResourceColor;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.font = font;
				//Debug.Log (laserLogic.laserPowerMultiplier);
				textMesh.text = (laserLogic.resourceRate*laserLogic.laserPowerMultiplier).ToString("F1");


                GameObject child2 = new GameObject();
                child2.AddComponent<SpriteRenderer>();
                child2.GetComponent<SpriteRenderer>().sprite = buildingOwner == Player.PlayerOne ? laserite[0] : laserite[1];
                child2.transform.parent = gameObject.transform;
                child2.transform.localEulerAngles = new Vector3(90f, 0, 0);
                child2.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x - 0.25f, pos.y + 0.5f, 1f);
                child2.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
                child2.AddComponent<Rigidbody>();
                Rigidbody rigidBody2 = child2.GetComponent<Rigidbody>();

                rigidBody2.useGravity = false;
                rigidBody2.velocity = Quaternion.AngleAxis(Random.Range(-randomnessRangeR, randomnessRangeR), Vector3.up) * Vector3.forward * textSpeedR;

                MonoBehaviour.Destroy(child, textLifetimeR);
                MonoBehaviour.Destroy(child2, textLifetimeR);

                // Update damageGrid data
                value.time = emissionRateR;
                value.lastResource = currResource;
                gridR[pos] = value;
            }
            else
            {
                gridR[pos] = new resourceBuilding(value.lastResource, value.time - Time.deltaTime);
            }
        }
        else
        {
            // Emit damage number
            GameObject child = new GameObject();
            child.transform.parent = gameObject.transform;
            child.transform.localEulerAngles = new Vector3(90f, 0, 0);
            child.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x + 0.25f, pos.y + 0.5f, 1f);
            child.AddComponent<Rigidbody>();
            Rigidbody rigidBody = child.GetComponent<Rigidbody>();

            rigidBody.useGravity = false;
            rigidBody.velocity = state == State.removing ? Quaternion.AngleAxis(Random.Range(-randomnessRangeR, randomnessRangeR), Vector3.down) * Vector3.forward * textSpeedR : Quaternion.AngleAxis(Random.Range(-randomnessRangeR, randomnessRangeR), Vector3.down) * Vector3.back * textSpeedR;


            child.AddComponent<TextMesh>();
            TextMesh textMesh = child.GetComponent<TextMesh>();

            textMesh.fontSize = 64;
            textMesh.characterSize = textSize;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = buildingOwner == Player.PlayerOne ? Color.red : new Color(1,0.5f,0,1);
            textMesh.fontStyle = FontStyle.Bold;
            textMesh.font = font;
            textMesh.text = state == State.placing ? "- " + (currResource).ToString("F1") : (currResource).ToString("F1");


            GameObject child2 = new GameObject();
            child2.AddComponent<SpriteRenderer>();
            child2.GetComponent<SpriteRenderer>().sprite = buildingOwner == Player.PlayerOne ? laserite[0] : laserite[1];
            child2.transform.parent = gameObject.transform;
            child2.transform.localEulerAngles = new Vector3(90f, 0, 0);
            child2.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x - 0.25f, pos.y + 0.5f, 1f);
            child2.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
            child2.AddComponent<Rigidbody>();
            Rigidbody rigidBody2 = child2.GetComponent<Rigidbody>();

            rigidBody2.useGravity = false;
            rigidBody2.velocity = state == State.removing ? Quaternion.AngleAxis(Random.Range(-randomnessRangeR, randomnessRangeR), Vector3.down) * Vector3.forward * textSpeedR : Quaternion.AngleAxis(Random.Range(-randomnessRangeR, randomnessRangeR), Vector3.down) * Vector3.back * textSpeedR;
            MonoBehaviour.Destroy(child, textLifetimeR);
            MonoBehaviour.Destroy(child2, textLifetimeR);
        }
    }

}


public class floatingNumbers : MonoBehaviour {
    [Header("Damage Values")]
    // Damage
    [Range(0.5f, 10f)]
    public float emissionRate = 1f;
    [Range(0f, 360f)]
    public float randomnessRange = 0f;
    [Range(0.25f, 4f)]
    public float textSize = 1f;
    [Range(0.5f, 10f)]
    public float textSpeed = 1f;
    [Range(0.1f, 2f)]
    public float textLifetime = 1f;
    public Color p1DamageColor = Color.red;
    public Color p2DamageColor = Color.red;
    [Header("Resource Values")]
    // Resources
    [Range(0.5f, 10f)]
    public float emissionRateR = 1f;
    [Range(0f, 360f)]
    public float randomnessRangeR = 0f;
    [Range(0.25f, 4f)]
    public float textSizeR = 1f;
    [Range(0.5f, 10f)]
    public float textSpeedR = 1f;
    [Range(0.1f, 2f)]
    public float textLifetimeR = 1f;
    public Color p1ResourceColor = Color.red;
    public Color p2ResourceColor = Color.red;
    public Sprite[] laserite;
    public Font font;

    public static damageResourceGrid floatingNumbersStruct;

    void Awake()
    {
        floatingNumbersStruct = new damageResourceGrid(laserite, gameObject, p1DamageColor, p2DamageColor, font, p1ResourceColor, p2ResourceColor, emissionRate, randomnessRange, textSize, textSpeed, textLifetime,
                                                       emissionRateR, randomnessRangeR, textSizeR, textSpeedR, textLifetimeR);
    }

}
