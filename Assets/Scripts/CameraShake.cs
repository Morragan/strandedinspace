using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCam;

    private float shakeAmount = 0;

    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    public void EternalShake(float amount)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake2", 0, 0.01f);
        Invoke("StopShake2", 33F);
    }

    void BeginShake()
    {
        if (shakeAmount <= 0) return;
        float offsetX = Random.value * shakeAmount * 2 - shakeAmount,
            offsetY = Random.value * shakeAmount * 2 - shakeAmount;
        Vector3 camPos = mainCam.transform.position;

        camPos.x += offsetX;
        camPos.y += offsetY;

        mainCam.transform.position = camPos;
    }

    void BeginShake2()
    {
        if (shakeAmount <= 0) return;
        float offsetX = Random.value * shakeAmount * 2 - shakeAmount,
            offsetY = Random.value * shakeAmount * 2 - shakeAmount;
        Vector3 camPos = mainCam.transform.position;

        camPos.x += offsetX;
        camPos.y += offsetY;

        mainCam.transform.position = camPos;
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }

    void StopShake2()
    {
        CancelInvoke("BeginShake2");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
