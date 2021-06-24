using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildLevel : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    marbleInit marbleInit;
    buildLevelEdit buildEditor;

    GameObject parent;
    GameObject buildContainer;
    GameObject checkpointContainer;
    public GameObject checkPointPrefab;
    public GameObject endPrefab;
    public GameObject[] allPrefabs = new GameObject[] { };
    public GameObject[] allAvaliblePrefabs;

    string[] metaIndex = new string[] { "ramp", "rightAngleCurve","leftAngleCurve", "end"};
    //        bx-0  by-1   bz-2   ex-3 ey-4   ez-5   br-6  er-7
    float[] allMeta = new float[]              {0, 90,-90,0};

    void Start()
    {
        
        main = FindObjectOfType<main>();
        marbleInit = FindObjectOfType<marbleInit>();
        buildEditor = FindObjectOfType<buildLevelEdit>();

        parent = GameObject.Find("blockContainer");
        buildContainer = GameObject.Find("buildLevelContainer");
        checkpointContainer = GameObject.Find("checkPoints");

        loadAvaliblePrefabs();
        initiateBuild();

        
    }

    void initiateBuild()
    {
        int maxPieces = Mathf.RoundToInt(main.levelDificulty/7) + 5;
        if(maxPieces>20)
        {
            maxPieces = 20;
        }
        int minPeices = Mathf.RoundToInt(main.levelDificulty / 20) + 4;
        if(minPeices>16)
        {
            minPeices = 16;
        }
        build(Random.Range(minPeices, maxPieces));
    }

    void loadAvaliblePrefabs()
    {
        int length = Mathf.RoundToInt(main.levelDificulty);
        if(length>allPrefabs.Length)
        {
            length = allPrefabs.Length;
        }
        allAvaliblePrefabs = new GameObject[length];
        for(int i=0;i<length;i++)
        {
            allAvaliblePrefabs[i] = allPrefabs[i];
        }
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
        objectMovement lastBlockScript;
        while (currentPeiceI<=targetPeices)
        {
            currentPeiceI++;
            if (firstLoop)
            {
                Vector3 position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                thisBlock = Instantiate(allAvaliblePrefabs[0], position, Quaternion.identity);
                thisBlock.name = "block" + currentPeiceI.ToString();
                thisBlock.transform.parent = parent.transform;
                main.allBlocks.Add(thisBlock);

                firstLoop = false;
            }
            else
            {
                lastBlock = GameObject.Find("block" + (currentPeiceI - 1).ToString());
                float[] lastBlockMeta = getMetaDataFrom(lastBlock);
                GameObject pieceObject;
                if (currentPeiceI == targetPeices+1)
                {
                    pieceObject = endPrefab;
                } else {
                    pieceObject = findBestPiece(4, lastBlock.transform.position.x + lastBlockMeta[3], lastBlock.transform.position.z + lastBlockMeta[5], rotationBuffer);
                }
                
                thisBlock = Instantiate(pieceObject, new Vector3(0,0,0), Quaternion.identity);
                main.allBlocks.Add(thisBlock);
                
                blockScript = thisBlock.GetComponent<objectMovement>();
                lastBlockScript = lastBlock.GetComponent<objectMovement>();

                lastBlockScript.lockedWith[1] = thisBlock;
                lastBlockScript.endLocks[1] = true;
                blockScript.lockedWith[0] = lastBlock;
                blockScript.endLocks[0] = true;

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
        GameObject[] allInitiatedBlocks = getAllInitiatedObjects();

        buildEditor.centerLevel(allInitiatedBlocks);
        placeCheckpoints(allInitiatedBlocks);
        

        main.init(allInitiatedBlocks);
        main.loadingLevel = false;
        marbleInit.initiateMarble();
    }

    GameObject findBestPiece(float boundery, float currentX, float currentZ,float rotation)
    {
        if(Mathf.Abs(currentX) > boundery || Mathf.Abs(currentZ) > boundery)
        {
             float xStabalizer = currentX / Mathf.Abs(currentX);
             float zStabalizer = currentZ / Mathf.Abs(currentZ);
             float[] xDists = new float[allAvaliblePrefabs.Length];
             float[] zDists = new float[allAvaliblePrefabs.Length];

             for (int i=0;i<allAvaliblePrefabs.Length;i++)
             {
                 GameObject thisBlock= Instantiate(allAvaliblePrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                 thisBlock.transform.Rotate(new Vector3(0, rotation % 360, 0), Space.Self);
                 thisBlock.transform.parent = buildContainer.transform;
                 float[] thisPieceMeta = getMetaDataFrom(thisBlock);
                 Destroy(thisBlock);
                 xDists[i] = thisPieceMeta[3] * xStabalizer;
                 zDists[i] = thisPieceMeta[5] * zStabalizer;

             }

             bool offX = Mathf.Abs(currentX) > boundery;
             bool offZ = Mathf.Abs(currentZ) > boundery;

             float[] totalOffValue = new float[allAvaliblePrefabs.Length];

             if(offX && offZ)
             {
                 for(int i = 0; i < allAvaliblePrefabs.Length;i++)
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
            GameObject returnObject = allAvaliblePrefabs[minValues[blockType]];
            return returnObject;
        } else
        {
            int blockType = Random.Range(0, allAvaliblePrefabs.Length);
            return allAvaliblePrefabs[blockType];
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


    void placeCheckpoints(GameObject[] allData)
    {
        float[] positionX = new float[allData.Length];
        float[] positionY = new float[allData.Length];
        float[] positionZ = new float[allData.Length];

        GameObject thisBlock;
        for (int i=0;i<allData.Length;i++)
        {
            float[] metaData = getMetaDataFrom(allData[i]);
            positionX[i] = allData[i].transform.position.x + metaData[3];
            positionY[i] = allData[i].transform.position.y + metaData[4]+1f;
            positionZ[i] = allData[i].transform.position.z + metaData[5];
        }
        int[] usedIndexes = new int[3];
        for(int i=0;i<usedIndexes.Length;i++)
        {
            usedIndexes[i] = -1;
        }
        for(int i=0; i<3;i++)
        {
            int currentPosition = getCheckPointPosition(usedIndexes, allData);
            //Debug.Log(currentPosition);
            Vector3 position = new Vector3(positionX[currentPosition], positionY[currentPosition], positionZ[currentPosition]);
            thisBlock = Instantiate(checkPointPrefab, position, Quaternion.identity);
            thisBlock.transform.parent = checkpointContainer.transform;
            thisBlock.name = "checkpoint";
            objectMovement objectScript = allData[currentPosition].GetComponent<objectMovement>();

            string blockType = objectScript.blockType;
            float rotation = objectScript.rotation;
            rotation += allMeta[System.Array.IndexOf(metaIndex, blockType)];
            thisBlock.transform.Rotate(new Vector3(0, rotation % 360, 0), Space.Self);

            usedIndexes[i] = currentPosition;

        }

    }

    int getCheckPointPosition(int[] usedIndexes, GameObject[] allData)
    {
        bool used = false;
        while(!used)
        {
            int currentPosition = Random.Range(0, allData.Length - 2);
            int usedPosition = System.Array.IndexOf(usedIndexes, currentPosition);
            used = usedPosition == -1;
            if (used)
            {
                return currentPosition;
            }
        }
        return 0;
    }



    float[] getMetaDataFrom(GameObject piece)
    {
        float positionModifier = piece.transform.localRotation.eulerAngles.y;
        Transform startTrasform = piece.transform.Find("startPos");
        GameObject startObject;
        Transform endTransform = piece.transform.Find("endPos");
        GameObject endObject;

        float startLocator;
        float endLocator;
        Vector3 alteredStart;

        if (startTrasform != null)
        {
            startObject = startTrasform.gameObject;
            Vector3 startPos = piece.transform.TransformPoint(startObject.transform.position);
            alteredStart = adjustMetaValues(startPos, positionModifier, piece);
            startLocator = 1f;
        }
        else
        {
            alteredStart = new Vector3(0, 0, 0);
            startLocator = 0f;
        }
        Vector3 alteredEnd;
        if (endTransform != null)
        {
            endObject = endTransform.gameObject;
            Vector3 endPos = piece.transform.TransformPoint(endObject.transform.position);
            alteredEnd = adjustMetaValues(endPos, positionModifier, piece);
            endLocator = 1f;
        }
        else
        {
            alteredEnd = new Vector3(0, 0, 0);
            endLocator = 0f;
        }


        float[] meta = new float[8] { (alteredStart.x - piece.transform.position.x), alteredStart.y - piece.transform.position.y, alteredStart.z - piece.transform.position.z, alteredEnd.x - piece.transform.position.x, alteredEnd.y - piece.transform.position.y, alteredEnd.z - piece.transform.position.z, startLocator, endLocator };

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
