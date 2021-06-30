using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prepNextLevel : MonoBehaviour
{
    main main;
    buildLevel buildLevel;
    void Start()
    {
        main = FindObjectOfType<main>();
        buildLevel = FindObjectOfType<buildLevel>();
    }

    public void nextLevel()
    {
        if (main.inBetweenLevel)
        {
            
            removeAllChildren(GameObject.Find("blockContainer"),true);
            removeAllChildren(GameObject.Find("checkPoints"),false);
            removeAllChildren(GameObject.Find("marbles"), false);
            main.checkpoints = 0;
            main.inBetweenLevel = false;
            main.inPlayMode = false;
            main.canCompleateLevel = false;
            buildLevel.initiateBuild();
        }
    }

    void removeAllChildren(GameObject parent, bool fullWipe)
    {
        foreach (Transform child in parent.transform)
        {
            if(fullWipe)
            {
                destroyBlock(child.gameObject);
            } else
            {
                Destroy(child.gameObject);
            }
            
        }
    }

    void destroyBlock(GameObject block)
    {
        clearData(block);
        Destroy(block);
        main.objectsOnScreen--;
        int blockIndex = int.Parse(block.name.Split('k')[1]);
        main.allBlocks.RemoveAt(blockIndex - 1);

        for (int i = blockIndex - 1; i < main.objectsOnScreen; i++)
        {
            main.allBlocks[i].name = "block" + (i + 1);
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
}
