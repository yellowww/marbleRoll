using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.name == "marble")
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
