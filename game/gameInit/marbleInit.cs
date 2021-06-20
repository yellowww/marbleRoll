using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marbleInit : MonoBehaviour
{
    
    public GameObject marblePrefab;
    public GameObject parent;
    public void initiateMarble()
    {
        GameObject startingBlock = GameObject.Find("block1");
        float xPos = startingBlock.transform.position.x;
        float yPos = startingBlock.transform.position.y + 0.5f;
        float zPos = startingBlock.transform.position.z;

        GameObject marble = Instantiate(marblePrefab,new Vector3(xPos,yPos,zPos),Quaternion.identity);
        marble.name = "marble";
        marble.transform.parent = parent.transform;
    }

}
