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
    bool firstEndFrame = true;
    Vector3 scale;
    void doAnimations()
    {
        Vector3 position;
        if(main.doEndCheckPointAnimations)
        {
            position = getCheckpointEndPosition(thisCheckpoint);
        } else
        {
            position = Camera.main.ScreenToWorldPoint(new Vector3(thisCheckpoint * (Screen.height / 9f + 20f), Screen.height - (Screen.height / 4.5f - 100f), Camera.main.nearClipPlane + 1));
        }
        if(main.doEndCheckPointAnimations)
        {
            if(firstEndFrame)
            {
                scale = getCheckpointEndScale(thisCheckpoint, this.gameObject);
                firstEndFrame = false;
            }
            
        } else {
            scale = new Vector3(0.04f, 0.04f, 0.04f);
        }
        

        Vector3 rotation = camera.transform.localRotation.eulerAngles;


        float step;
        if(main.doEndCheckPointAnimations)
        {
            step = 4 * Time.deltaTime;
        } else
        {
            step = 10 * Time.deltaTime;
        }
        
        if (inPlace && !main.doEndCheckPointAnimations)
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
        } else
        {
            this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, rotation, step);
        }
        

        if (!inPlace && getSquareDist(this.transform.position,position,0.025f))
        {
            inPlace = true;
        }

        if(!rotationLock && getSquareDist(this.transform.eulerAngles,rotation,2))
        {
            rotationLock = true;
        }
    }

    Vector3 getCheckpointEndPosition(int checkPoint)
    {

        float height = Screen.height;
        if(height<600)
        {
            height = 600;
        }
        float x=0;
        float y=0;
        if(checkPoint == 1)
        {
            x = Screen.width / 2 - (250 * (height/800));
            y = Screen.height / 2;
        }
        if (checkPoint == 2)
        {
            x = Screen.width / 2;
            y = Screen.height / 2 + (150 * (height/800));
        }
        if(checkPoint == 3)
        {
            x = Screen.width / 2 + (250 * (height/800));
            y = Screen.height / 2;
        }
        return Camera.main.ScreenToWorldPoint(new Vector3(x, y, Camera.main.nearClipPlane + 1));
    }


    Vector3 getCheckpointEndScale(int checkPoint, GameObject checkpoint)
    {
        float scale;
        if (checkPoint == 1 || checkPoint == 3) {
            scale = 0.08f;

        } else
        {
            scale = 0.12f;
        }
        return new Vector3(scale,scale,scale);
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
