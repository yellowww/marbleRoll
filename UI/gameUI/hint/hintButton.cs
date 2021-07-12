using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hintButton : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    buildLevelEdit buildEditor;
    void Start()
    {
        main = FindObjectOfType<main>();
        buildEditor = FindObjectOfType<buildLevelEdit>();
    }


    public void getHint()
    {
        addBlock();
    }

    void addBlock()
    {
        if (hasDefined(main.removedBlocks))
        {
            int index = findBestRemoved();
            if (index != -1)
            {
                GameObject block = main.removedBlocks[index];
                showAllChildren(block);

                attachEndLocks(block);

                main.removedBlocks[index] = null;


                updateShading();
            }

        }
    }

    void findEndLocks(GameObject gameObject)
    {
        
        
        float[] thisEndValues = getMetaDataFrom(gameObject, false);

        GameObject locked0 = checkIfEndLocatorsMatch(thisEndValues, gameObject, 0);
        GameObject locked1 = checkIfEndLocatorsMatch(thisEndValues, gameObject, 1);

        objectMovement script = gameObject.GetComponent<objectMovement>();
        script.endLocks[0] = locked0 != null;
        script.endLocks[1] = locked1 != null;
        if (locked0 != null) script.lockedWith[0] = locked0;
        if (locked1 != null) script.lockedWith[1] = locked1;

    }

    GameObject checkIfEndLocatorsMatch(float[] thisMeta, GameObject thisGameObject, int locatorPosition)
    {
        float[] selectedMeta;
        objectMovement selectedScript;
        for (int i = 0; i < main.allBlocks.Count; i++)
        {
            if (main.allBlocks[i].name != thisGameObject.name)
            {
                selectedMeta = getMetaDataFrom(main.allBlocks[i], false);
                selectedScript = main.allBlocks[i].GetComponent<objectMovement>();
                if(compareMetaValues(thisMeta,selectedMeta,locatorPosition, main.allBlocks[i].name))
                {
                    if(!selectedScript.isRemoved)
                    {
                        return main.allBlocks[i];
                    }
                    
                }
            }
        }
        return null;
    }

    bool compareMetaValues(float[] meta0, float[] meta1, int locater, string name)
    {
        // locator of 0 = front of this (meta0) to back of selected (meta1)
        bool canCheck = true;
        if (locater == 0)
        {
            if (meta0[6] == 0f || meta1[7] == 0f)
            {
                canCheck = false;
            }
        } else if (locater == 1)
        {
            if (meta0[7] == 0f || meta1[6] == 0f)
            {
                canCheck = false;
            }
        } else
        {
            Debug.LogError("ERROR: locator not in accepted range (0-1)");
        }
        locater *= 3;
        int locator1;
        if(locater == 0)
        {
            locator1 = 3;
        } else
        {
            locator1 = 0;
        }
        if(canCheck)
        {

            if (roundFTo(meta0[locater], 0.01f) == roundFTo(meta1[locator1], 0.01f)) 
            {
                if (roundFTo(meta0[locater + 1], 0.01f) == roundFTo(meta1[locator1 + 1], 0.01f))
                {
                    if (roundFTo(meta0[locater + 2], 0.01f) == roundFTo(meta1[locator1 + 2], 0.01f))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    float roundFTo(float value, float place)
    {
        return Mathf.Round(value / place) * place;
    } 


    void attachEndLocks(GameObject gameObject)
    {
        findEndLocks(gameObject);
        objectMovement thisMovementScript = gameObject.GetComponent<objectMovement>();

        GameObject lockedWith;
        objectMovement movementScript;
        if(thisMovementScript.endLocks[0])
        {
            lockedWith = thisMovementScript.lockedWith[0];
            movementScript = lockedWith.GetComponent<objectMovement>();
            if (movementScript.endLocks[1] == true)
            {
                GameObject ocupiedLock = movementScript.lockedWith[1];
                objectMovement ocupiedScript = ocupiedLock.GetComponent<objectMovement>();

                ocupiedScript.endLocks[0] = false;
                ocupiedScript.lockedWith[0] = null;
            }


            movementScript.lockedWith[1] = gameObject;
            movementScript.endLocks[1] = true;
        }

        
        if(thisMovementScript.endLocks[1])
        {
            lockedWith = thisMovementScript.lockedWith[1];
            movementScript = lockedWith.GetComponent<objectMovement>();

            if (movementScript.endLocks[0] == true)
            {
                GameObject ocupiedLock = movementScript.lockedWith[0];
                objectMovement ocupiedScript = ocupiedLock.GetComponent<objectMovement>();

                ocupiedScript.endLocks[1] = false;
                ocupiedScript.lockedWith[1] = null;
            }

            movementScript.lockedWith[0] = gameObject;
            movementScript.endLocks[0] = true;
        }


    }

    List<GameObject> removeShownObjects(List<GameObject> list)
    {
        List<GameObject> newList = new List<GameObject>(list);
        for(int i=0;i<newList.Count;)
        {
            //script = newList[i].GetComponent<objectMovement>();
            if(newList[i] == null)
            {
                newList.Remove(newList[i]);
            } else
            {
                i++;
            }
        }
        return newList;
    }

    int findBestRemoved()
    {
        bool works = false;
        List<GameObject> allAvalible = new List<GameObject>(main.removedBlocks);
        allAvalible = removeShownObjects(allAvalible);
        int actualIndex = -1;
        int currentIndex;
        while (!works)
        {
            if(allAvalible.Count == 0)
            {
                return -1;
            }
            currentIndex = getRandomDefined(allAvalible);
            if (currentIndex == -1) return -1;
            if (checkIfOverlapes(allAvalible[currentIndex])) {
                allAvalible.Remove(allAvalible[currentIndex]);
            } else
            {
                actualIndex = getActualIndex(allAvalible[currentIndex]);
                works = true;
                return actualIndex;
            }
        }
        return actualIndex;
    }

    int getActualIndex(GameObject gameObject)
    {
        for(int i=0;i<main.removedBlocks.Length;i++)
        {
            if(main.removedBlocks[i] != null)
            {
                if (main.removedBlocks[i].name == gameObject.name)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    bool checkIfOverlapes(GameObject gameObject)
    {
        objectMovement thisScript = gameObject.GetComponent<objectMovement>();
        objectMovement selectedScript;
        for(int i=0;i<main.allBlocks.Count;i++)
        {
            if(main.allBlocks[i].name != gameObject.name)
            {
                if(roundTo(main.allBlocks[i].transform.position,1f) == roundTo(gameObject.transform.position,1f))
                {
                    selectedScript = main.allBlocks[i].GetComponent<objectMovement>();
                    if (adjustRotation(selectedScript.rotation) == adjustRotation(thisScript.rotation))
                    {

                        if(selectedScript.blockType == thisScript.blockType)
                        {
                            return true;
                        }
                    }
                    
                }
            }
        }
        return false;
    }

    float adjustRotation(float rotation)
    {
        float newRotation = rotation;
        if(rotation<0)
        {
            newRotation = 360 + rotation;
        }
        return newRotation;
    }

    Vector3 roundTo(Vector3 value, float place)
    {
        Vector3 newPos = new Vector3(0, 0, 0);
        newPos.x = Mathf.Round(value.x / place) * place;
        newPos.y = Mathf.Round(value.y / place) * place;
        newPos.z = Mathf.Round(value.z / place) * place;
        return newPos;
    }

    void showAllChildren(GameObject gameObject)
    {
        GameObject[] allChildren = buildEditor.getAllChildren(gameObject);
        for (int i = 0; i < allChildren.Length; i++)
        {
            MeshRenderer renderer = allChildren[i].GetComponent<MeshRenderer>();
            BoxCollider colider = allChildren[i].GetComponent<BoxCollider>();
            if (renderer && colider)
            {
                renderer.enabled = true;
                colider.enabled = true;
            }

        }
    }
    public void updateShading()
    {
        Color color;
        if (findBestRemoved() == -1)
        {
            color = new Color32(255, 255, 255, 127);
        }
        else if (!hasDefined(main.removedBlocks))
        {
            color = new Color32(255, 255, 255, 127);
            
        } else
        {
            color = new Color32(255, 255, 255, 225);
        }
        this.gameObject.GetComponent<Image>().color = color;
    }

    bool hasDefined(GameObject[] array)
    {
        for(int i=0;i<array.Length;i++)
        {
            if(array[i] != null)
            {
                return true;
            }
        }
        return false;
    }

    int getRandomDefined(List<GameObject> list)
    {
        bool isDefined = false;
        int random = 0;
        List<GameObject> newList = cloneList(list);
        while (!isDefined && newList.Count > 0)
        {
            random = Random.Range(0, newList.Count-1);
            if(newList[random] != null)
            {
                isDefined = true;
            } else
            {
                newList.Remove(newList[random]);
            }
        }
        if(newList.Count < 1)
        {
            return -1;
        }
        return random;
    }
    List<GameObject> cloneList(List<GameObject> list)
    {
        List <GameObject> newList = new List<GameObject>();
        for(int i=0;i<list.Count;i++)
        {
            newList.Add(list[i]);
        }
        return newList;
    }



    float[] getMetaDataFrom(GameObject piece, bool getAltered)
    {
        float positionModifier = piece.transform.localRotation.eulerAngles.y;
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

        if(getAltered)
        {
            alteredStart -= piece.transform.position;
            alteredEnd -= piece.transform.position;
        }


        float[] meta = new float[8] { (alteredStart.x), alteredStart.y, alteredStart.z, alteredEnd.x, alteredEnd.y, alteredEnd.z, startLocator, endLocator };

        return meta;
    }

    Vector3 adjustMetaValues(Vector3 position, float rotation, GameObject piece)
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
