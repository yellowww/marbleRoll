using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    GameObject camera;
    GameObject blockContainer;
    main main;
    Vector3 mousePos;
    float deltaMouseX;
    float deltaMouseY;
    float lastFrameX;
    float lastFrameY;
    public float lastXAngle = 0;
    public float lastYAngle = 0;

    public float cameraDistance = 15;

    public bool firstFrame = false;
    public bool dragingObject;
    public int currentDragingObject;

    

    // Start is called before the first frame update
    void Start()
    {
      camera = GameObject.Find("camera");
      blockContainer = GameObject.Find("blockContainer");
      main = FindObjectOfType<main>();

    }
    
    // Update is called once per frame
    void Update()
    {
        if(!main.loadingLevel)
        {
            updateMouseInputs();
            
        }
        setScrollMin();

    }

    bool canDrag()
    {
        return !main.GUIClick && !main.hotbarGUIHover && !main.inBetweenLevel;
    }

    void setScrollMin()
    {
        if(cameraDistance<1.5f)
        {
            cameraDistance = 1.5f;
        }
    }

    void updateMouseInputs()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 )
        {
            if (cameraDistance > 1.5f)
            {
                cameraDistance -= Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * 200;
            }
            else
            {
                cameraDistance = 1.5f;
            }
            moveCamera();
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && cameraDistance < 30)
        {

            cameraDistance -= Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * 200;
            moveCamera();
        }

        mousePos = Input.mousePosition;

        if(Input.GetMouseButton(0) && !firstFrame && main.GUIHover)
        {
            main.GUIClick = true;
            
        } else
        {
            main.GUIClick = false;
        }

        if (Input.GetMouseButton(0) && !firstFrame && canDrag()) {
            
            RaycastHit hit;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                
                GameObject hitObject = findParent(hit.collider.gameObject);
                if (hitObject != null)
                {
                    objectMovement objectScript = hitObject.GetComponent<objectMovement>();

                    if (objectScript.moveable && !main.inPlayMode)
                    {
                        firstFrame = true;
                        dragingObject = true;
                        string[] dragingIndex = hitObject.name.Split('k');
                        currentDragingObject = int.Parse(dragingIndex[1]);
                        main.selectedObject = hitObject;
                    }
                }
            }
        }
        if (Input.GetMouseButton(0) && canDrag())
        {
            
            if (!dragingObject) {
                if(!firstFrame)
                {
                    firstFrame = true;
                    lastFrameX = mousePos.x;
                    lastFrameY = mousePos.y;
                }
                
                deltaMouseX = mousePos.x - lastFrameX + lastXAngle;
                if((mousePos.y - lastFrameY + lastYAngle)/300f > Mathf.PI/2.1f)
                {
                    deltaMouseY = Mathf.PI / 2.1f * 300f;
                } else if((mousePos.y - lastFrameY + lastYAngle)/300f < Mathf.PI / -2.1f)
                {
                    deltaMouseY = Mathf.PI / -2.1f * 300f;
                } else
                {
                    deltaMouseY = mousePos.y - lastFrameY + lastYAngle;
                }

                main.selectedObject = null;
                moveCamera();
            }
            
        } else
        {
            if(!Input.GetMouseButton(0))
            {
                main.hotbarGUIHover = false;
            }
            


            if(firstFrame)
            {
                firstFrame = false;
                if (!dragingObject)
                {
                    lastXAngle = deltaMouseX;
                    lastYAngle = deltaMouseY;
                }
                if(dragingObject)
                {
                    deleteObject();
                }
                if(!main.invintoryClickRelease)
                {
                    dragingObject = false;
                }
                

                

            }



        }
    }

    public void deleteObject()
    {
        GameObject mouseUpObject = GameObject.Find("block" + currentDragingObject.ToString());
        Vector3 draggingScale = mouseUpObject.GetComponent<objectMovement>().toScaleValue;

        if (draggingScale.x == 0.5f)
        {

            destroyBlock(mouseUpObject);
        }
    }

    void destroyBlock(GameObject block)
    {
        main mainScript = FindObjectOfType<main>();
        clearData(block);
        Destroy(block);
        mainScript.objectsOnScreen--;
        int blockIndex = int.Parse(block.name.Split('k')[1]);
        mainScript.allBlocks.RemoveAt(blockIndex - 1);

        for (int i = blockIndex - 1; i < mainScript.objectsOnScreen; i++)
        {
            mainScript.allBlocks[i].name = "block" + (i + 1);
        }
    }

    void clearData(GameObject block)
    {
        objectMovement script = block.GetComponent<objectMovement>();
        script.endLocks[0] = false;
        script.endLocks[1] = false;
        if (script.lockedWith[1] != null)
        {
            GameObject attachedFrontObject = script.lockedWith[1];
            objectMovement attachedScript = attachedFrontObject.GetComponent<objectMovement>();
            attachedScript.endLocks[0] = false;
            attachedScript.lockedWith[0] = null;
            script.lockedWith[1] = null;
        }

        if (script.lockedWith[0] != null)
        {
            GameObject attachedBackObject = script.lockedWith[0];
            objectMovement attachedScript = attachedBackObject.GetComponent<objectMovement>();
            attachedScript.endLocks[1] = false;
            attachedScript.lockedWith[1] = null;

            script.lockedWith[0] = null;
        }
    }

    void moveCamera()
    {
        if (!main.inBetweenLevel)
        {
            //set camera position
            float xMovement = Mathf.Sin((deltaMouseX) / 300f) * cameraDistance;
            float yMovement = Mathf.Sin(((deltaMouseY) / 300f)) * -cameraDistance;
            float zMovement = (Mathf.Cos((deltaMouseX) / 300f) * (Mathf.Cos(((deltaMouseY) / 300f)))) * cameraDistance;

            camera.transform.position = new Vector3(xMovement, yMovement, zMovement);

            //rotate camera
            camera.transform.LookAt(Vector3.zero);
        }
    }

    public GameObject findParent(GameObject startObject)
    {
        GameObject currentParent = startObject;
        while(currentParent.transform.parent.gameObject.name != "blockContainer")
        {
            currentParent = currentParent.transform.parent.gameObject;
            if(currentParent.transform.parent == null)
            {
                return null;
            }
        }
        return currentParent;
    }
}
