using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkkGod : MonoBehaviour
{
    public static DarkkGod Instance { get; private set; }

    [SerializeField] CameraShake cameraShake;
    private Animator animator;

    private void Start()
    {
        if (Instance == null) 
        {
            var temp = GameObject.FindGameObjectWithTag("DARKKGOD");
            Instance = temp.GetComponentInChildren<DarkkGod>();
        }
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        animator.SetBool("GodTime", true);
        cameraShake.EternalShake(0.0375F);
    }

    public void Hide()
    {
        animator.SetBool("GodTime", false);
    }
}
