using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addCheckpoint : MonoBehaviour
{
    // Start is called before the first frame update
    cameraMovement cameraMovement;
    dev developer;
    buildLevel buildScript;

    bool hasCheckpoint = false;
    bool keyDown = false;

    GameObject addedCheckpoint;
    public GameObject checkPointPrefab;
    public GameObject checkpointContainer;

    void Start()
    {
        cameraMovement = FindObjectOfType<cameraMovement>();
        developer = FindObjectOfType<dev>();
        buildScript = FindObjectOfType<buildLevel>();
        checkpointContainer = GameObject.Find("checkPoints");
    }


    void Update()
    {
        if(canAddCheckpoint())
        {
            if(hasCheckpoint)
            {
                removeCheckpoint();
            } else
            {
                placeCheckpoint();
            }
            keyDown = true;
            
        } else
        {
            keyDown = false;
        }
    }

    bool canAddCheckpoint()
    {
        bool isSelected = cameraMovement.currentDragingObject == int.Parse(this.name.Split('k')[1]);
        bool cKeyDown = Input.GetKeyDown(KeyCode.C);
        return developer.inDev && isSelected && cKeyDown && !keyDown;
    }

    void placeCheckpoint()
    {
        objectMovement movementScript = this.gameObject.GetComponent<objectMovement>();
        float rotation = movementScript.rotation;
        float[] capPosition = getMetaDataFrom(this.gameObject, rotation);
        Vector3 position = new Vector3(capPosition[3], capPosition[4]+1f, capPosition[5]);
        
        addedCheckpoint = Instantiate(checkPointPrefab, position, Quaternion.identity);
        addedCheckpoint.transform.parent = checkpointContainer.transform;
        rotation += buildScript.updateRotationForCurves(movementScript);
        addedCheckpoint.transform.eulerAngles = new Vector3(0, rotation, 0);

        hasCheckpoint = true;
    }

    void removeCheckpoint()
    {
        Destroy(addedCheckpoint);
        hasCheckpoint = false;
    }



    public float[] getMetaDataFrom(GameObject piece, float positionModifier)
    {
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


        float[] meta = new float[8] { (alteredStart.x), alteredStart.y, alteredStart.z, alteredEnd.x, alteredEnd.y, alteredEnd.z, startLocator, endLocator };

        return meta;
    }

    public Vector3 adjustMetaValues(Vector3 position, float rotation, GameObject piece)
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
