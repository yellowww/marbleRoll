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
        float distance = main.levelSize / 2f + 2;
        Vector3 position = new Vector3(distance, distance, distance);
        float step = 3.33f * Time.deltaTime;
        this.transform.LookAt(blockContainer.transform);
        this.transform.position = Vector3.Lerp(this.transform.position, position, step);
    }
    
    void Update()
    {
        if(main.inBetweenLevel)
        {
            moveCameraFrame();
        }
    }
}
