using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildLevelEdit : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    main main;
    private void Start()
    {
        main = FindObjectOfType<main>();
    }
    void Update()
    {
        
    }

    public void centerLevel(GameObject[] array) {
        float[] allXPos = getPropertyArray(array, 0);
        float xMin = getMin(allXPos);
        float xMax = getMax(allXPos);
        float xMove = (xMax + xMin)/-2;

        float[] allYPos = getPropertyArray(array, 1);
        float yMin = getMin(allYPos);
        float yMax = getMax(allYPos);
        float yMove = (yMax + yMin)/-2;

        float[] allZPos = getPropertyArray(array, 2);
        float zMin = getMin(allZPos);
        float zMax = getMax(allZPos);
        float zMove = (zMax + zMin)/-2;

        moveAllBlocks(array, new Vector3(xMove, yMove, zMove));

    }

    void moveAllBlocks(GameObject[] blocks, Vector3 position)
    {
        for(int i=0;i<blocks.Length;i++)
        {
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


}
