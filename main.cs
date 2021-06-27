using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int objectsOnScreen = 2;
    public GameObject selectedObject = null;
    public bool GUIHover = false;
    public bool GUIClick = false;
    public bool invintoryClickRelease = false;
    public bool hotbarGUIHover = false;
    public bool inventoryBackHover = false;
    public bool inventoryElementHover = false;
    public bool totalInventoryHover = false;
    public bool canCompleateLevel = false;
    public float levelDificulty = 10;
    public bool loadingLevel = true;

    public bool inPlayMode = false;

    public int checkpoints = 0;

    public bool inBetweenLevel = true;

    public float levelSize;

    public List<GameObject> allBlocks = null;
    void Start()
    {
    }

    public void init(GameObject[] startingBlocks)
    {
        allBlocks = new List<GameObject>(startingBlocks);
    }
    public void initFromBuild()
    {
        GameObject[] startingBlocks = new GameObject[objectsOnScreen];
        for(int i=0;i<objectsOnScreen;i++)
        {
            startingBlocks[i] = GameObject.Find("block" + (i + 1));
        }
        allBlocks = new List<GameObject>(startingBlocks);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
