﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildLevel : MonoBehaviour
{
    // Start is called before the first frame update
    objectMovement objectMovement;
    main main;
    GameObject parent;

    public GameObject[] allPrefabs = new GameObject[] { };

    void Start()
    {
        objectMovement = FindObjectOfType<objectMovement>();
        main = FindObjectOfType<main>();
        parent = GameObject.Find("blockContainer");
        build(3);
    }


    public void build(int targetPeices)
    {
        int currentPeiceI = 0;
        bool firstLoop = true;
        GameObject lastBlock;
        GameObject thisBlock;

        while (currentPeiceI<=targetPeices)
        {
            currentPeiceI++;
            if (firstLoop)
            {
                Vector3 position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                thisBlock = Instantiate(allPrefabs[0], position, Quaternion.identity);
                thisBlock.name = "block" + currentPeiceI.ToString();
                thisBlock.transform.parent = parent.transform;
                main.allBlocks.Add(thisBlock);

                firstLoop = false;
            }
            else
            {
                lastBlock = GameObject.Find("block" + (currentPeiceI - 1).ToString());
                float[] lastBlockMeta = getMetaDataFrom(lastBlock, 0);
                Debug.Log(lastBlockMeta[1]);
                thisBlock = Instantiate(allPrefabs[0], new Vector3(0,0,0), Quaternion.identity);
                main.allBlocks.Add(thisBlock);
                float[] thisBlockMeta = getMetaDataFrom(thisBlock, 0);

                float thisBlockX = lastBlock.transform.position.x + lastBlockMeta[3] - thisBlockMeta[0];
                float thisBlockY = lastBlock.transform.position.y + lastBlockMeta[4] - thisBlockMeta[1];
                float thisBlockZ = lastBlock.transform.position.z + lastBlockMeta[5] - thisBlockMeta[2];
                thisBlock.transform.position = new Vector3(thisBlockX, thisBlockY, thisBlockZ);

                thisBlock.name = "block" + currentPeiceI.ToString();
                thisBlock.transform.parent = parent.transform;

                

            }
        }


        main.objectsOnScreen = currentPeiceI;
        main.init(getAllInitiatedObjects());
        main.loadingLevel = false;

    }

    GameObject[] getAllInitiatedObjects()
    {
        GameObject[] allObjects = new GameObject[main.objectsOnScreen];
        for(int i=1;i<=main.objectsOnScreen;i++)
        {
            allObjects[i-1] = GameObject.Find("block" + i);
        }
        return allObjects;
    }

    float[] getMetaDataFrom(GameObject piece, float positionModifier)
    {
        GameObject startObject = piece.transform.Find("startPos").gameObject;
        GameObject endObject = piece.transform.Find("endPos").gameObject;
        Vector3 startPos = piece.transform.TransformPoint(startObject.transform.position);
        Vector3 alteredStart = adjustMetaValues(startPos, positionModifier, piece);

        Vector3 endPos = piece.transform.TransformPoint(endObject.transform.position);
        Vector3 alteredEnd = adjustMetaValues(endPos, positionModifier, piece);

        float[] meta = new float[6] { (alteredStart.x - piece.transform.position.x), alteredStart.y - piece.transform.position.y, alteredStart.z - piece.transform.position.z, alteredEnd.x - piece.transform.position.x, alteredEnd.y - piece.transform.position.y, alteredEnd.z - piece.transform.position.z };

        return meta;
    }

    Vector3 adjustMetaValues(Vector3 position, float rotation, GameObject piece)
    {
        Vector3 newPosition = new Vector3(0, 0, 0);
        if (rotation == 0f)
        {
            newPosition.x = position.x - piece.transform.position.x;
            newPosition.y = position.y - piece.transform.position.y;
            newPosition.z = position.z - piece.transform.position.z;
        }
        if (rotation == -90f | rotation == 270f)
        {
            newPosition.x = position.z - piece.transform.position.z;
            newPosition.y = position.y - piece.transform.position.y;
            newPosition.z = (position.x - piece.transform.position.x) * -1;
        }
        if (rotation == 90f | rotation == -270f)
        {
            newPosition.x = (position.z - piece.transform.position.z) * -1;
            newPosition.y = position.y - piece.transform.position.y;
            newPosition.z = position.x - piece.transform.position.x;
        }
        if (rotation == 180f | rotation == -180f)
        {
            newPosition.x = (position.x - piece.transform.position.x) * -1;
            newPosition.y = position.y - piece.transform.position.y;
            newPosition.z = (position.z - piece.transform.position.z) * -1;
        }
        return newPosition;
    }
}
