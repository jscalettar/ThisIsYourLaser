using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingHealthBars : MonoBehaviour {

    public Texture healthBarBG;
    [Range(-1.0f, 1.0f)]
    public float yOffset = 0.55f;
    [Range(0.2f, 5.0f)]
    public float yScale = 1.0f;
    [Range(0.1f, 2.0f)]
    public float xScale = 1.0f;
    // NOTE: COLORS SHOULD HAVE AN ALPHA VALUE OF 255 IN THE INSPECTOR
    public Color p1Background;
    public Color p1HealthColor;
    public Color p2Background;
    public Color p2HealthColor;

    void OnGUI()
    {
        Vector3 v1 = Camera.main.WorldToScreenPoint(gridManager.theGrid.coordsToWorld(0, 0));
        Vector3 v2 = Camera.main.WorldToScreenPoint(gridManager.theGrid.coordsToWorld(11, 7));
        float scale = (v2.x - v1.x) * 0.08f;

        foreach (KeyValuePair<XY, GameObject> pair in gridManager.theGrid.prefabDictionary) {
            if (pair.Value.GetComponent<buildingParameters>().buildingType == Building.Laser) continue;

            Vector3 center = Camera.main.WorldToScreenPoint(gridManager.theGrid.coordsToWorld(pair.Key.x, pair.Key.y));
            center.x -= scale * 0.5f * xScale;
            center.y -= scale * yOffset;

            GUI.color = pair.Value.GetComponent<buildingParameters>().owner == Player.PlayerOne ? p1Background : p2Background;
            GUI.DrawTexture(new Rect(center.x, Screen.height - center.y, scale * xScale, scale * 0.05f * yScale), healthBarBG, ScaleMode.StretchToFill);
            GUI.color = pair.Value.GetComponent<buildingParameters>().owner == Player.PlayerOne ? p1HealthColor : p2HealthColor;
            float hp = Mathf.Max(0f, pair.Value.GetComponent<buildingParameters>().currentHP / pair.Value.GetComponent<buildingParameters>().health * scale * xScale);
            GUI.DrawTexture(new Rect(center.x, Screen.height - center.y, hp, scale * 0.05f * yScale), healthBarBG, ScaleMode.StretchToFill);
        }
    }

}
