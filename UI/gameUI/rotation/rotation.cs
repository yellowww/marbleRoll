﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rotation : MonoBehaviour
{
    main main;

    private void Start()
    {
        main = FindObjectOfType<main>();
    }

    public void rotateLeft()
    {
        if(main.selectedObject != null)
        {
            objectMovement movementScript = main.selectedObject.GetComponent<objectMovement>();

            movementScript.rotation = (movementScript.rotation - 90) % 360;
            main.selectedObject.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        }

    }
    public void rotateRight()
    {
        if(main.selectedObject != null)
        {
            objectMovement movementScript = main.selectedObject.GetComponent<objectMovement>();

            movementScript.rotation = (movementScript.rotation+90) % 360;
            main.selectedObject.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
        }

    }

    public void UIHover ()
    {
        main.GUIHover = true;
    }

    public void UIHoverLeave()
    {
        main.GUIHover = false;
    }
}
