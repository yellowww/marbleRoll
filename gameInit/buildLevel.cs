using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildLevel : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    GameObject parent;
    GameObject buildContainer;

    public GameObject[] allPrefabs = new GameObject[] { };

    string[] metaIndex = new string[] { "ramp", "rightAngleCurve" };
    //        bx-0  by-1   bz-2   ex-3 ey-4   ez-5   br-6  er-7
    float[] allMeta = new float[]              {0, 90};

    void Start()
    {
        
        main = FindObjectOfType<main>();
        parent = GameObject.Find("blockContainer");
        buildContainer = GameObject.Find("buildLevelContainer");
        build(70);
    }


    public void build(int targetPeices)
    {
        int currentPeiceI = 0;
        bool firstLoop = true;
        GameObject lastBlock;
        GameObject thisBlock;
        float rotationBuffer = 0;
        float thisRotation;
        objectMovement blockScript;
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
                float[] lastBlockMeta = getMetaDataFrom(lastBlock);
                GameObject pieceObject = findBestPiece(13, lastBlock.transform.position.x + lastBlockMeta[3], lastBlock.transform.position.z + lastBlockMeta[5], rotationBuffer);
                thisBlock = Instantiate(pieceObject, new Vector3(0,0,0), Quaternion.identity);
                main.allBlocks.Add(thisBlock);

                blockScript = thisBlock.GetComponent<objectMovement>();

                blockScript.rotation = rotationBuffer % 360;
                thisBlock.transform.Rotate(new Vector3(0, rotationBuffer % 360, 0), Space.Self);
                float[] thisBlockMeta = getMetaDataFrom(thisBlock);

                thisRotation = allMeta[System.Array.IndexOf(metaIndex, blockScript.blockType)];
                rotationBuffer += thisRotation;



                


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

    GameObject findBestPiece(float boundery, float currentX, float currentZ,float rotation)
    {
        if(Mathf.Abs(currentX) > boundery || Mathf.Abs(currentZ) > boundery)
        {
             float xStabalizer = currentX / Mathf.Abs(currentX);
             float zStabalizer = currentZ / Mathf.Abs(currentZ);
             float[] xDists = new float[allPrefabs.Length];
             float[] zDists = new float[allPrefabs.Length];

             for (int i=0;i<allPrefabs.Length;i++)
             {
                 GameObject thisBlock= Instantiate(allPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                 thisBlock.transform.Rotate(new Vector3(0, rotation % 360, 0), Space.Self);
                 thisBlock.transform.parent = buildContainer.transform;
                 float[] thisPieceMeta = getMetaDataFrom(thisBlock);
                 Destroy(thisBlock);
                 xDists[i] = thisPieceMeta[3] * xStabalizer;
                 zDists[i] = thisPieceMeta[5] * zStabalizer;

             }

             bool offX = Mathf.Abs(currentX) > boundery;
             bool offZ = Mathf.Abs(currentZ) > boundery;

             float[] totalOffValue = new float[allPrefabs.Length];

             if(offX && offZ)
             {
                 for(int i = 0; i < allPrefabs.Length;i++)
                 {
                     totalOffValue[i] = xDists[i] * zDists[i];
                 }
             } else if(offX)
             {
                 totalOffValue = xDists;
             } else if(offZ)
             {
                 totalOffValue = zDists;
             }

            int[] minValues = getMin(totalOffValue);
            int blockType = Random.Range(0, minValues.Length);
            //Debug.Log(totalOffValue[0].ToString() + " strait");
            //Debug.Log(totalOffValue[1].ToString() + " curve");
            //Debug.Log(blockType);
            GameObject returnObject = allPrefabs[minValues[blockType]];
            return returnObject;
        } else
        {
            int blockType = Random.Range(0, allPrefabs.Length);
            return allPrefabs[blockType];
        }
    }

    int[] getMin(float[] input)
    {
        float min = Mathf.Infinity;
        List<int> currentMinI = new List<int>();
        for (int i=0;i<input.Length;i++)
        {
            if(input[i]<min)
            {
                min = input[i];
                currentMinI = new List<int>();
                currentMinI.Add(i);
            } else if(input[i] == min)
            {
                currentMinI.Add(i);
            }
        }
        int[] returnArray = new int[currentMinI.Count];
        for(int i=0;i<currentMinI.Count;i++)
        {
            returnArray[i] = currentMinI[i];
        }
        return returnArray;
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

    float[] getMetaDataFrom(GameObject piece)
    {
        float positionModifier = piece.transform.localRotation.eulerAngles.y;
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
