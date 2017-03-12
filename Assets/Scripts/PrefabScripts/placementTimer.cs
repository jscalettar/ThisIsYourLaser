using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used on prefab when placing building to display time until the structure is constructed

public class placementTimer : MonoBehaviour {

    [Range(0.1f, 2f)]
    public float textSize = 1f;
    public Color p1Color = Color.white;
    public Color p2Color = Color.white;
    public Font font;

    private float time = 0f;
    private Player owner = Player.World;

    public void init (float timer, Player player)
    {
        time = timer;
        owner = player;
    }

    // Use this for initialization
    void Start () {
        TextMesh textMesh = gameObject.AddComponent<TextMesh>();

        textMesh.fontSize = 64;
        textMesh.characterSize = textSize * 0.04f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.color = owner == Player.PlayerOne ? p1Color : p2Color;
        textMesh.fontStyle = FontStyle.Bold;
        if (font == null) font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textMesh.font = font;
    }
	
	// Update is called once per frame
	void Update () {
        print(time);
        TextMesh textMesh = GetComponent<TextMesh>();
        textMesh.text = time.ToString("F1");
        time -= Time.deltaTime;
        if (time <= 0f) Destroy(gameObject);
    }
}
