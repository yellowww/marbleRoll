using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectMovement : MonoBehaviour
{
    // Start is called before the first frame update
    cameraMovement cameraMovementScript;
    main mainScript;
    bool firstMovementFrame;
    public string blockType;
    float offX;
    float offY;
    float offZ;
    bool thisHotbarHover = false;
    bool doScaleAnimation = false;
    public Vector3 toScaleValue = new Vector3(0,0,0);

    public float rotation = 0;

    string[] metaIndex = new string[] { "ramp", "rightAngleCurve" };
    //        bx-0  by-1   bz-2   ex-3 ey-4   ez-5   br-6  er-7
    float[][] allMeta = new float[][] { new float[2] {0f, 180f},
                                        new float[2] {0f, -90f } };

    //                             front  back
    bool[] endLocks = new bool[] { false, false };

    GameObject[] lockedWith = new GameObject[] { null, null };

    public bool moveable = true;

    void Start()
    {
        cameraMovementScript = FindObjectOfType<cameraMovement>();
        mainScript = FindObjectOfType<main>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraMovementScript.dragingObject && cameraMovementScript.currentDragingObject == int.Parse(this.name.Split('k')[1]))
        {
            if (moveable)
            {
                moveObject();
                checkHotbarHover();
            }
            firstMovementFrame = false;
        } else
        {
            firstMovementFrame = true;
        }
        doAminations();
    }



    void moveObject()
    {

        float cameraZDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        //calculate the position
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // find the offsets
        if (firstMovementFrame)
        {
            offX = transform.position.x - worldPos.x;
            offY = transform.position.y - worldPos.y;
            offZ = transform.position.z - worldPos.z;
        }
        // apply the offsets
        Vector3 alteredWorldPos = new Vector3(worldPos.x + offX, worldPos.y + offY, worldPos.z + offZ);
        bool foundObject = findCloseObjects(alteredWorldPos);
        if(!foundObject)
        {
            transform.position = alteredWorldPos;
            endLocks[0] = false;
            endLocks[1] = false;
            if(lockedWith[1] != null)
            {
                GameObject attachedFrontObject = lockedWith[1];
                objectMovement attachedScript = attachedFrontObject.GetComponent<objectMovement>();
                attachedScript.endLocks[0] = false;
                Debug.Log("done");
                attachedScript.lockedWith[0] = null;
                lockedWith[1] = null;
            }

            if (lockedWith[0] != null) {
                GameObject attachedBackObject = lockedWith[0];
                objectMovement attachedScript = attachedBackObject.GetComponent<objectMovement>();
                attachedScript.endLocks[1] = false;
                attachedScript.lockedWith[1] = null;

                lockedWith[0] = null;
            }
            
        }
        

        
    }

    void checkHotbarHover()
    {
        if(mainScript.totalInventoryHover && !thisHotbarHover)
        {
            doScaleAnimation = true;
            toScaleValue = new Vector3(0.5f, 0.5f, 0.5f);

            thisHotbarHover = !thisHotbarHover;
        }
        if (!mainScript.totalInventoryHover && thisHotbarHover)
        {
            doScaleAnimation = true;
            Vector3 oldScale = this.transform.localScale;
            toScaleValue = new Vector3(1f, 1f, 1f);

            thisHotbarHover = !thisHotbarHover;
        }
    }

    void doAminations()
    {
        if(doScaleAnimation)
        {
            float step = 10 * Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, toScaleValue, step);
        }
    }

    bool findCloseObjects(Vector3 position)
    {
        // this rotationMetaData
        float[] thisRMetaData = allMeta[System.Array.IndexOf(metaIndex, blockType)];
        //this positionMetaData
        float[] thisPMetaData = getMetaDataFrom(this.gameObject, rotation);
        for (int i = 1; i <= mainScript.objectsOnScreen; i++)
        {
            if (this.name != "block" + i)
            {
                GameObject selectedObject = mainScript.allBlocks[i-1];
                objectMovement selectedMovementScript = selectedObject.GetComponent<objectMovement>();
                //selected rotationMetaData
                float[] selectedRMetaData = allMeta[System.Array.IndexOf(metaIndex, selectedMovementScript.blockType)];
                float anglesMatch = 0f;
                bool endLocksWork = false;
                bool lockedWithThis = false;
                //selected positionMetaData
                float[] selectedPMetaData = getMetaDataFrom(selectedObject, selectedMovementScript.rotation);
                //check back of this to front of selected

                anglesMatch = Mathf.Abs((thisRMetaData[0] + rotation) - (selectedRMetaData[1] + selectedMovementScript.rotation));
                endLocksWork = !(endLocks[0] || selectedMovementScript.endLocks[1]);
                if(lockedWith[0] != null)
                {
                    lockedWithThis = lockedWith[0].name == selectedObject.name && selectedMovementScript.lockedWith[1].name == this.gameObject.name;
                }
                //Debug.Log("thisBack " + endLocks[1].ToString() + ' ' + lockedWith[1]);
                //Debug.Log("otherFront " + selectedMovementScript.endLocks[0].ToString() + ' ' + selectedMovementScript.lockedWith[0]);
                //Debug.Log(endLocksWork);
                if ((anglesMatch==180f || anglesMatch == 540f) && (endLocksWork || lockedWithThis))
                {
                    
                    float distX = Mathf.Abs((position.x + thisPMetaData[0]) - (selectedObject.transform.position.x + selectedPMetaData[3]));
                    float distY = Mathf.Abs((position.y + thisPMetaData[1]) - (selectedObject.transform.position.y + selectedPMetaData[4]));
                    float distZ = Mathf.Abs((position.z + thisPMetaData[2]) - (selectedObject.transform.position.z + selectedPMetaData[5]));
                    if (distX<1.5f && distY<1.5f && distZ<1.5f)
                    {
                        float targetX = selectedObject.transform.position.x + selectedPMetaData[3] - thisPMetaData[0];
                        float targetY = selectedObject.transform.position.y + selectedPMetaData[4] - thisPMetaData[1];
                        float targetZ = selectedObject.transform.position.z + selectedPMetaData[5] - thisPMetaData[2];
                        transform.position = new Vector3(targetX, targetY, targetZ);
                        endLocks[0] = true;
                        selectedMovementScript.endLocks[1] = true;

                        lockedWith[0] = selectedObject;
                        selectedMovementScript.lockedWith[1] = this.gameObject;
                        return true;
                    }                    
                }

                // check front of this to back of selected
                anglesMatch = Mathf.Abs((thisRMetaData[1] + rotation) - (selectedRMetaData[0] + selectedMovementScript.rotation));
                endLocksWork = endLocksWork = !(endLocks[1] || selectedMovementScript.endLocks[0]);
                lockedWithThis = false;
                if(lockedWith[1] != null)
                {
                    lockedWithThis = lockedWith[1].name == selectedObject.name && selectedMovementScript.lockedWith[0].name == this.gameObject.name;
                }

                if ((anglesMatch == 180f || anglesMatch == 540f) && (endLocksWork || lockedWithThis))
                {
                    float distX = Mathf.Abs((position.x + thisPMetaData[3]) - (selectedObject.transform.position.x + selectedPMetaData[0]));
                    float distY = Mathf.Abs((position.y + thisPMetaData[4]) - (selectedObject.transform.position.y + selectedPMetaData[1]));
                    float distZ = Mathf.Abs((position.z + thisPMetaData[5]) - (selectedObject.transform.position.z + selectedPMetaData[2]));
 
                    if (distX < 1.5f && distY < 1.5f && distZ < 1.5f)
                    {
                        
                        float targetX = selectedObject.transform.position.x + selectedPMetaData[0] - thisPMetaData[3];
                        float targetY = selectedObject.transform.position.y + selectedPMetaData[1] - thisPMetaData[4];
                        float targetZ = selectedObject.transform.position.z + selectedPMetaData[2] - thisPMetaData[5];
                        transform.position = new Vector3(targetX, targetY, targetZ);
                        endLocks[1] = true;
                        selectedMovementScript.endLocks[0] = true;

                        lockedWith[1] = selectedObject;
                        selectedMovementScript.lockedWith[0] = this.gameObject;

                        return true;
                    }
                }
            }   
        }
        return false;
    }

    float[] getMetaDataFrom(GameObject piece, float positionModifier)
    {
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
        if(rotation == 0f)
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
            newPosition.x = (position.z - piece.transform.position.z)*-1;
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
