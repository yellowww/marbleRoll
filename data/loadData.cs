using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class loadData : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset textAsset;
    public GameObject parent;
    public GameObject[] assetArray;
    main main;
    
    string[] assetLink = new string[5] { "end", "leftAngleCurve", "ramp", "rightAngleCurve", "start" };
    void Start()
    {
        main = FindObjectOfType<main>();
        buildLevel(3);
        
    }

    void buildLevel(int level)
    {
        main.allBlocks = new List<GameObject>();
        main.objectsOnScreen = 0;

        string[] allData = getLevelData(level);
        string[] blockData = getAllBlockData(allData);
        string[] cpData = getAllCheckpoints(allData);
        string[] thisSplit;
        for(int i=0;i<blockData.Length;i++)
        {

            thisSplit = blockData[i].Split(';');

            Vector3 pos = stringToVec(thisSplit[0]);

            float rot = float.Parse(thisSplit[1]);
            GameObject prefab = assetArray[System.Array.IndexOf(assetLink, thisSplit[2])];

            GameObject thisObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            thisObject.transform.position = pos;
            thisObject.name = "block" + (i+1);
            thisObject.transform.Rotate(0, rot, 0);
            thisObject.transform.parent = parent.transform;

            objectMovement movementScript = thisObject.GetComponent<objectMovement>();

            movementScript.rotation = rot;

            main.objectsOnScreen++;
            main.allBlocks.Add(thisObject);


        }
    }

    Vector3 stringToVec(string String)
    {
        float x = float.Parse(String.Split(',')[0].Split('(')[1]);     
        float y = float.Parse(String.Split(',')[1]);
        float z = float.Parse(String.Split(',')[2].Split(')')[0]);
        Vector3 vec = new Vector3(x, y, z);
        return vec;
    }

    string[] getAllBlockData(string[] allData)
    {
        List<string> blockList = new List<string>();
        for(int i=0;allData[i] != "cp";i++)
        {
            blockList.Add(allData[i]);
        }

        string[] blockArray = new string[blockList.Count];
        for(int i=0;i<blockArray.Length;i++)
        {
            blockArray[i] = blockList[i];
        }
        return blockArray;
    }

    string[] getAllCheckpoints(string[] allData)
    {
        List<string> cpList = new List<string>();
        bool canAdd = false;
        for (int i = 0; i<allData.Length; i++)
        {
            if(canAdd)
            {
                cpList.Add(allData[i]);
            } else if (allData[i] == "cp")
            {
                canAdd = true;
            }
            
        }

        string[] cpArray = new string[cpList.Count];
        for (int i = 0; i < cpArray.Length; i++)
        {
            cpArray[i] = cpList[i];
        }
        return cpArray;
    }



    string[] getLevelData(int level)
    {
        string[] splitLines = decodeWholeFile(textAsset);
        int currentLevel = 1;
        List<string> thisLevel = new List<string>();
        for(int i=0;currentLevel<=level && i<splitLines.Length;i++)
        {
            if (splitLines[i] == "/")
            {
                currentLevel++;
            }
            else if (currentLevel == level)
            {
                thisLevel.Add(splitLines[i]);
            }

        }
        string[] levelArray = new string[thisLevel.Count];
        for(int i=0;i<levelArray.Length;i++)
        {
            levelArray[i] = thisLevel[i];
        }
        return levelArray;
    }

    string[] decodeWholeFile(TextAsset file)
    {

        string text = file.text;
        string[] splitLines = text.Split('\n');
        return splitLines;
    }
}
