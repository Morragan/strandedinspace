using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;
    public bool hasRightBuddy = false, hasLeftBuddy = false;
    public bool reverseScale = false;
    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    // Use this for initialization
    void Start () {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        spriteWidth = renderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (hasLeftBuddy == false || hasRightBuddy == false)
        {
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height; //half of camera view
            // calculate x position where camera can see the sprite's edge
            float edgeVisiblePositionRight = myTransform.position.x + spriteWidth / 2 - camHorizontalExtend;
            float edgeVisiblePositionLeft = myTransform.position.x - spriteWidth / 2 + camHorizontalExtend;
            // checking if the edge is visible
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasRightBuddy)
            {
                MakeNewBuddy(1);
                hasRightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && !hasLeftBuddy)
            {
                MakeNewBuddy(-1);
                hasLeftBuddy = true;
            }
        }
	}
    void MakeNewBuddy(int rightOrLeft)
    {
        // new buddy's position
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // reverse scale if not tilable
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if(rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
        }
    }
}
