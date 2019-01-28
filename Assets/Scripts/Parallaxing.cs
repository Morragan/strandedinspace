using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;                 // backgrounds and foregrounds
    private float[] parallaxScales;
    public float smoothing = 1f;                    // must be greater than 0

    private Transform cam;                          // camera's transform
    private Vector3 prevCamPos;                     // position of camera in prev frame

    // great for setting up references
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        prevCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (prevCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            // position updated by parallaxing
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            // deltatime - frames to seconds
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        // update prevcampos
        prevCamPos = cam.position;
    }
}
