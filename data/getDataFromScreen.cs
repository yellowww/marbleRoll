using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getDataFromScreen : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    bool keyDown = false;
    dev developer;
    void Start()
    {
        main = FindObjectOfType<main>();
        developer = FindObjectOfType<dev>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && developer.inDev)
        {
            if(!keyDown)
            {
                keyDown = true;
                CopyToClipboard(getData());
            }
        }  else
        {
            keyDown = false;
        }
    }

    public static void CopyToClipboard(string s)
    {
        Debug.Log("copied!");
        GUIUtility.systemCopyBuffer = s;
    }

    string getFullVector(Vector3 position)
    {
        float x = position.x;
        float y = position.y;
        float z = position.z;

        return "(" + x + "," + y + "," + z + ")";
    }

    string getData()
    {
        string data = "";
        string subString;
        objectMovement script;
        for(int i=0;i<main.allBlocks.Count;i++)
        {
            script = main.allBlocks[i].GetComponent<objectMovement>();

            subString = "";
            subString += getFullVector(main.allBlocks[i].transform.position) + ';';
            subString += script.rotation.ToString() + ';';
            subString += script.blockType.ToString() + ';';
            subString += script.isRemoved.ToString();
            data += subString;
            data += "\n";

        }
        data += "cp";
        data += "\n";
        foreach (Transform child in GameObject.Find("checkPoints").transform)
        {
            subString = "";
            subString += getFullVector(child.gameObject.transform.position) + ';';
            subString += child.gameObject.transform.rotation.ToString();
            data += subString;
            data += "\n";
        }
        return data;
    }
}
