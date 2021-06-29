using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraEndMovement : MonoBehaviour
{
    main main;
    GameObject blockContainer;

    
    void Start()
    {
        main = FindObjectOfType<main>();

        blockContainer = GameObject.Find("blockContainer");
    }
    void moveCameraFrame()
    {
        float distance = main.levelSize/1.5f + 2;
        Vector3 position = new Vector3(distance, distance, distance);
        float step = 3.33f * Time.deltaTime;
        this.transform.LookAt(blockContainer.transform);

        if (!main.doEndCheckPointAnimations)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, position, step);
        }

        if(!main.doEndCheckPointAnimations && getSquareDist(this.transform.position,position,0.05f))
        {
            main.doEndCheckPointAnimations = true;
            this.transform.position = position;
        }
    }

    bool getSquareDist(Vector3 pos0, Vector3 pos1, float dist)
    {
        float xPos = Mathf.Abs(pos0.x - pos1.x);
        float yPos = Mathf.Abs(pos0.y - pos1.y);
        float zPos = Mathf.Abs(pos0.z - pos1.z);
        return xPos < dist && yPos < dist && zPos < dist;
    }

    void Update()
    {
        if(main.inBetweenLevel)
        {
            moveCameraFrame();
        }
    }
}
