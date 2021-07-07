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

                attachEndLocks(block, main.removedLockedWith[index]);

                main.removedBlocks[index] = null;


                updateShading();
            }

        }
    }


    void attachEndLocks(GameObject gameObject, GameObject[] lockedTo)
    {
        objectMovement thisMovementScript = gameObject.GetComponent<objectMovement>();

        GameObject lockedWith = lockedTo[0];
        objectMovement movementScript = lockedWith.GetComponent<objectMovement>();

        if(!movementScript.isRemoved)
        {
            thisMovementScript.lockedWith[0] = lockedWith;
            thisMovementScript.endLocks[0] = true;

            movementScript.lockedWith[1] = gameObject;
            movementScript.endLocks[1] = true;
        }

        lockedWith = lockedTo[1];
        movementScript = lockedWith.GetComponent<objectMovement>();

        if (!movementScript.isRemoved)
        {
            thisMovementScript.lockedWith[1] = lockedWith;
            thisMovementScript.endLocks[1] = true;

            movementScript.lockedWith[0] = gameObject;
            movementScript.endLocks[0] = true;
        }

    }

    int findBestRemoved()
    {
        bool works = false;
        List<GameObject> allAvalible = new List<GameObject>(main.removedBlocks);
        int actualIndex = -1;
        while (!works)
        {
            if(allAvalible.Count == 0)
            {
                return -1;
            }
            int currentIndex = getRandomDefined(allAvalible);
            if(checkIfOverlapes(allAvalible[currentIndex])) {
                allAvalible.Remove(allAvalible[currentIndex]);
            } else
            {
                actualIndex = getActualIndex(allAvalible[currentIndex]);
                works = true;
            }
        }
        return actualIndex;
    }

    int getActualIndex(GameObject gameObject)
    {
        for(int i=0;i<main.removedBlocks.Length;i++)
        {
            if(main.removedBlocks[i].name == gameObject.name)
            {
                return i;
            }
        }
        return -1;
    }

    bool checkIfOverlapes(GameObject gameObject)
    {
        for(int i=0;i<main.allBlocks.Count;i++)
        {
            if(main.allBlocks[i].name != gameObject.name)
            {
                if(roundTo(main.allBlocks[i].transform.position,0.01f) == roundTo(gameObject.transform.position,0.01f))
                {
                    return true;
                }
            }
        }
        return false;
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
        while(!isDefined)
        {
            random = Random.Range(0, list.Count-1);
            if(list[random] != null)
            {
                isDefined = true;
            }
        }
        return random;
    }
}
