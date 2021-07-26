using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class loadData : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset textAsset;
    void Start()
    {
        string[] data = getLevelData(2);
        for(int i=0;i<data.Length;i++)
        {
            Debug.Log(data[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
                i++;
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
