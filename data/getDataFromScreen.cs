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
                Debug.Log("copied!");
                CopyToClipboard(getData());
            }
        }  else
        {
            keyDown = false;
        }
    }

    public static void CopyToClipboard(string s)
    {
        GUIUtility.systemCopyBuffer = s;
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
            subString += main.allBlocks[i].transform.position.ToString() + ',';
            subString += script.rotation.ToString() + ',';
            subString += script.blockType.ToString() + ',';
            subString += script.isRemoved.ToString();
            data += subString;
            if (i < main.allBlocks.Count-1)
            {
                data += "\n";
            }

        }
        return data;
    }
}
