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

    public List<GameObject> allBlocks = null;
    void Start()
    {
        init(new GameObject[]{GameObject.Find("block1")});
    }

    private void init(GameObject[] startingBlocks)
    {
        allBlocks = new List<GameObject>(startingBlocks);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
