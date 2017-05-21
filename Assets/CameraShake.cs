using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class CameraShake : MonoBehaviour
{
    // How long the object should shake for.
    public float shakeDuration = .5f;
    public static float shakeTimer = 2f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = .7f;
    public float decreaseFactor = 2.0f;

    Vector3 originalPos;

    private void Awake()
    {
        originalPos = Camera.main.transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer < shakeDuration)
        {
            Camera.main.transform.localPosition =  Vector3.MoveTowards(Camera.main.transform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount, .1f);
            //Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeTimer += Time.deltaTime;
        }
        else
        {
            Camera.main.transform.localPosition = originalPos;
        }
    }
    
}
