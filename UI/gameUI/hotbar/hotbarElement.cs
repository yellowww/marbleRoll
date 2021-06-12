using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hotbarElement : MonoBehaviour
{
    // Start is called before the first frame update

    main main;
    cameraMovement cameraMovement;


    public GameObject prefab = null;
    public GameObject parent = null;

    void Start()
    {
        main = FindObjectOfType<main>();
        cameraMovement = FindObjectOfType<cameraMovement>();
        parent = GameObject.Find("blockContainer");
    }

    // Update is called once per frame
    void Update()
    {
        if(main.invintoryClickRelease && !main.loadingLevel)
        {
            checkMouse();
        }
        
    }

    public void onmouseover()
    {
        main.inventoryElementHover = true;
        main.totalInventoryHover = main.inventoryElementHover || main.inventoryBackHover;
    }

    public void onmouseout()
    {
        main.inventoryElementHover = false;
        main.totalInventoryHover = main.inventoryElementHover || main.inventoryBackHover;
    }

    public void OnMouseDown()
    {
        if(!main.hotbarGUIHover)
        {
            main.hotbarGUIHover = true;
            GameObject newBlock = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            main.objectsOnScreen++;
            newBlock.name = "block" + main.objectsOnScreen;

            newBlock.transform.parent = parent.transform;
            main.allBlocks.Add(newBlock);

            cameraMovement.currentDragingObject = main.objectsOnScreen;
            main.selectedObject = newBlock;


            cameraMovement.firstFrame = true;
            cameraMovement.dragingObject = true;
            main.invintoryClickRelease = true;
        }

        
    }

    void checkMouse()
    {
        if(!Input.GetMouseButton(0))
        {
            cameraMovement.dragingObject = false;
            main.invintoryClickRelease = false;
            cameraMovement.deleteObject();
        }
    }
}
