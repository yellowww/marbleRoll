using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointMovement : MonoBehaviour
{
    checkPointAnimationInit CPAnimationInit;
    main main;
    private void Start()
    {
        CPAnimationInit = FindObjectOfType<checkPointAnimationInit>();
        main = FindObjectOfType<main>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "marble")
        {
            CPAnimationInit.createInstance(this.gameObject);
            main.checkpoints++;
        }
    }
}
