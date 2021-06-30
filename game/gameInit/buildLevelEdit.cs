using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildLevelEdit : MonoBehaviour
{
    public Material initBlockColor;
    main main;
    
   
    void Start()
    {
        main = FindObjectOfType<main>();
        
    }



     public Vector3 centerLevel(GameObject[] array) {
        main = FindObjectOfType<main>();
        float[] allXPos = getPropertyArray(array, 0);
        float xMin = getMin(allXPos);
        float xMax = getMax(allXPos);
        float xMove = (xMax + xMin)/-2;
        float xWid = Mathf.Abs(xMax - xMin);

        float[] allYPos = getPropertyArray(array, 1);
        float yMin = getMin(allYPos);
        float yMax = getMax(allYPos);
        float yMove = (yMax + yMin)/-2;
        float yWid = Mathf.Abs(yMax - yMin);

        float[] allZPos = getPropertyArray(array, 2);
        float zMin = getMin(allZPos);
        float zMax = getMax(allZPos);
        float zMove = (zMax + zMin)/-2;
        float zWid = Mathf.Abs(zMax - zMin);
        if (zWid > yWid && zWid > xWid)
        {
            main.levelSize = zWid;
        } else if(yWid > zWid && yWid > xWid)
        {
            main.levelSize = yWid;
        } else
        {
            main.levelSize = xWid;
        }
        return new Vector3(xMove, yMove, zMove);
        

    }

    public void moveAllBlocks(GameObject[] blocks, Vector3 position)
    {

        for (int i = 0; i < blocks.Length; i++)
        {
            Debug.Log(blocks[i].name.ToString() + ' ' + blocks[i].transform.position.ToString());
            Debug.Log(blocks[i].name.ToString() + ' ' + position.ToString());
            blocks[i].transform.position += position;
        }
    }

    float getMin(float[] array)
    {
        float min = Mathf.Infinity;
        for(int i=0;i<array.Length;i++)
        {
            if(array[i]<min)
            {
                min = array[i];
            }
        }
        return min;
    }
    float getMax(float[] array)
    {
        float max = Mathf.NegativeInfinity;
        for(int i=0;i<array.Length;i++)
        {
            if(array[i] > max)
            {
                max = array[i];
            }
        }
        return max;
    }

    float[] getPropertyArray(GameObject[] array, int index)
    {
        float[] propertyArray = new float[array.Length];
        for(int i=0;i<array.Length;i++)
        {
            propertyArray[i] = array[i].transform.position[index];
        }
        return propertyArray;
    }


    public void shadeInitBlocks(GameObject[] allBlocks)
    {
        for(int i=0;i<allBlocks.Length;i++)
        {

            GameObject[] children = getAllChildren(allBlocks[i]);
            for(int i2 = 0;i2<children.Length;i2++)
            {
                GameObject obj = children[i2];
                if (obj.GetComponent<MeshRenderer>())
                {
                    obj.GetComponent<MeshRenderer>().material = initBlockColor;
                }
            }
        }
    }

    void getChildren(GameObject gameObject, List<GameObject> list)
    {
        foreach(Transform child in gameObject.transform)
        {
            list.Add(child.gameObject);
        }
    }

    GameObject[] getAllChildren(GameObject thisObject)
    {
        List<GameObject> childrenList = new List<GameObject>();
        childrenList.Add(thisObject);
        int i = 0;
        while(i<childrenList.Count)
        {
            getChildren(childrenList[i], childrenList);
            i++;
        }


        GameObject[] childrenArray = new GameObject[childrenList.Count];
        for(i=0;i<childrenList.Count;i++)
        {
            childrenArray[i] = childrenList[i];
        }
        return childrenArray;
    }
}
