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

public struct damageResourceGrid
{
    Dictionary<XY, healthBuilding> grid;
    private GameObject gameObject;
    private Color p1DamageColor;
    private Color p2DamageColor;
    private float emissionRate;
    private float randomnessRange;
    private float textSize;
    private float textSpeed;
    private float textLifetime;

    public damageResourceGrid(GameObject container, Color p1, Color p2, float rate = 1f, float randomAngle = 0f, float size = 1f, float speed = 1f, float life = 1f)
    {
        gameObject = container;
        grid = new Dictionary<XY, healthBuilding>();
        emissionRate = 1f / rate;
        randomnessRange = randomAngle * 0.5f;
        textSize = size * 0.04f;
        textSpeed = speed * 0.5f;
        textLifetime = life * 1.5f;
        p1DamageColor = p1;
        p2DamageColor = p2;
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
            child.transform.localPosition = gridManager.theGrid.coordsToWorld(pos.x, pos.y, 1f) + new Vector3(0,0,0.5f);
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

    public static damageResourceGrid floatingNumbersStruct;

    void Awake()
    {
        floatingNumbersStruct = new damageResourceGrid(gameObject, p1DamageColor, p2DamageColor, emissionRate, randomnessRange, textSize, textSpeed, textLifetime);
    }

}
