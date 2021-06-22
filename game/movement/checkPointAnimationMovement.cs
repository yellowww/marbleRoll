using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointAnimationMovement : MonoBehaviour
{
    public GameObject camera;
    main main;
    int thisCheckpoint;
    void doAnimations()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(thisCheckpoint*(Screen.height/9f+20f),Screen.height - (Screen.height / 4.5f - 100f), Camera.main.nearClipPlane+1));
        Vector3 scale = new Vector3(0.04f, 0.04f, 0.04f);

        Vector3 rotation = camera.transform.localRotation.eulerAngles;

        

        float step = 10 * Time.deltaTime;

        this.transform.position = Vector3.Lerp(this.transform.position, position, step);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, scale, step);
        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, rotation, step);

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
