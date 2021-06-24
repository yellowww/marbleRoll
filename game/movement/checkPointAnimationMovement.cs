using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointAnimationMovement : MonoBehaviour
{
    public GameObject camera;
    main main;
    int thisCheckpoint;
    bool inPlace = false;
    bool rotationLock = false;
    void doAnimations()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(thisCheckpoint*(Screen.height/9f+20f),Screen.height - (Screen.height / 4.5f - 100f), Camera.main.nearClipPlane+1));
        Vector3 scale = new Vector3(0.04f, 0.04f, 0.04f);

        Vector3 rotation = camera.transform.localRotation.eulerAngles;



        float step = 10 * Time.deltaTime;
        if (inPlace)
        {
            this.transform.position = position;
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, position, step);
        }
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, scale, step);

        if(rotationLock)
        {
            this.transform.eulerAngles = rotation;
        }
        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, rotation, step);

        if (!inPlace && getSquareDist(this.transform.position,position,0.025f))
        {
            inPlace = true;
        }

        if(!rotationLock && getSquareDist(this.transform.eulerAngles,rotation,2))
        {
            rotationLock = true;
        }
    }
    bool getSquareDist(Vector3 pos0, Vector3 pos1, float dist)
    {
        float xPos = Mathf.Abs(pos0.x-pos1.x);
        float yPos = Mathf.Abs(pos0.y-pos1.y);
        float zPos = Mathf.Abs(pos0.z-pos1.z);
        return xPos < dist && yPos < dist && zPos < dist;
    }
    private void Start()
    {
        main = FindObjectOfType<main>();
        thisCheckpoint = main.checkpoints;
        
        camera = GameObject.Find("camera");
    }

    void Update()
    {
        doAnimations();
    }
}
