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
    private Color p1DamageColor;
    private Color p2DamageColor;
    private float emissionRate;
    private float randomnessRange;
    private float textSize;
    private float textSpeed;
    private float textLifetime;
    private Font font;

    public damageResourceGrid(Sprite[] laser, GameObject container, Color p1, Color p2, Font customFont, float rate = 1f, float randomAngle = 0f, float size = 1f, float speed = 1f, float life = 1f)
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
        laserite = laser;
    }

    public void showCost(XY pos, State currState, float cost, Player buildingOwner)
    {
        GameObject child = new GameObject();
        child.transform.parent = gameObject.transform;
        child.transform.localEulerAngles = new Vector3(90f, 0, 0);
        child.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x+0.7f, pos.y - 0.5f, 1f);
        
        child.AddComponent<TextMesh>();
        TextMesh textMesh = child.GetComponent<TextMesh>();

        textMesh.fontSize = 64;
        textMesh.characterSize = textSize;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.color = buildingOwner == Player.PlayerOne ? p1DamageColor : p2DamageColor;
        textMesh.fontStyle = FontStyle.Bold;
        textMesh.font = font;
        textMesh.text = currState == State.placing ?(cost).ToString("F1"):(cost / 2).ToString("F1");

        MonoBehaviour.Destroy(child, .05f);
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
            textMesh.text = "-" + ((value.lastHP - currHP) * gridManager.hpScale).ToString("F1");

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
        if (state != State.placing)
        {
            resourceBuilding value = new resourceBuilding();
            if (!gridR.TryGetValue(pos, out value)) gridR.Add(pos, new resourceBuilding(currResource, emissionRate)); // Add to gridInfo if not already there
            else if (currResource < value.lastResource) gridR[pos] = new resourceBuilding(currResource, emissionRate); // Reset if building has changed
            else if (value.time <= 0f)
            {
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
                textMesh.color = buildingOwner == Player.PlayerOne ? Color.red : Color.green;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.font = font;
                textMesh.text = ((currResource - value.lastResource)).ToString("F1");


                GameObject child2 = new GameObject();
                child2.AddComponent<SpriteRenderer>();
                child2.GetComponent<SpriteRenderer>().sprite = buildingOwner == Player.PlayerOne ? laserite[0] : laserite[1];
                child2.transform.parent = gameObject.transform;
                child2.transform.localEulerAngles = new Vector3(90f, 0, 0);
                child2.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x - 0.5f, pos.y + 0.5f, 1f);
                child2.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                child2.AddComponent<Rigidbody>();
                Rigidbody rigidBody2 = child2.GetComponent<Rigidbody>();

                rigidBody2.useGravity = false;
                rigidBody2.velocity = Quaternion.AngleAxis(Random.Range(-randomnessRange, randomnessRange), Vector3.up) * Vector3.forward * textSpeed;

                MonoBehaviour.Destroy(child, textLifetime);
                MonoBehaviour.Destroy(child2, textLifetime);

                // Update damageGrid data
                value.time = emissionRate;
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
            child.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x, pos.y + 0.5f, 1f);
            child.AddComponent<Rigidbody>();
            Rigidbody rigidBody = child.GetComponent<Rigidbody>();

            rigidBody.useGravity = false;
            rigidBody.velocity = Quaternion.AngleAxis(Random.Range(-randomnessRange, randomnessRange), Vector3.down) * Vector3.back * textSpeed;


            child.AddComponent<TextMesh>();
            TextMesh textMesh = child.GetComponent<TextMesh>();

            textMesh.fontSize = 64;
            textMesh.characterSize = textSize;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = buildingOwner == Player.PlayerOne ? Color.red : Color.green;
            textMesh.fontStyle = FontStyle.Bold;
            textMesh.font = font;
            textMesh.text = "-" + (currResource).ToString("F1");


            GameObject child2 = new GameObject();
            child2.AddComponent<SpriteRenderer>();
            child2.GetComponent<SpriteRenderer>().sprite = buildingOwner == Player.PlayerOne ? laserite[0] : laserite[1];
            child2.transform.parent = gameObject.transform;
            child2.transform.localEulerAngles = new Vector3(90f, 0, 0);
            child2.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x - 0.5f, pos.y + 0.5f, 1f);
            child2.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            child2.AddComponent<Rigidbody>();
            Rigidbody rigidBody2 = child2.GetComponent<Rigidbody>();

            rigidBody2.useGravity = false;
            rigidBody2.velocity = Quaternion.AngleAxis(Random.Range(-randomnessRange, randomnessRange), Vector3.down) * Vector3.back * textSpeed;

            MonoBehaviour.Destroy(child, textLifetime);
            MonoBehaviour.Destroy(child2, textLifetime);
        }
    }

}


public class floatingNumbers : MonoBehaviour {

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
    public Sprite[] laserite;
    public Font font;

    public static damageResourceGrid floatingNumbersStruct;

    void Awake()
    {
        floatingNumbersStruct = new damageResourceGrid(laserite, gameObject, p1DamageColor, p2DamageColor, font, emissionRate, randomnessRange, textSize, textSpeed, textLifetime);
    }

}
