using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointAnimationInit : MonoBehaviour
{
    public GameObject checkPointAnimationPrefab;
    public GameObject parent;

    public void createInstance(GameObject original)
    {
        GameObject checkPoint = Instantiate(checkPointAnimationPrefab, new Vector3(0,0,0), Quaternion.identity);
        Vector3 position = original.transform.position;
        Vector3 rotation = original.transform.localRotation.eulerAngles;
        checkPoint.transform.position = new Vector3(position.x, position.y, position.z);
        checkPoint.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        checkPoint.transform.Rotate(0, rotation.y, 0, Space.Self);
        checkPoint.name = "checkPointAnimation";
        checkPoint.transform.parent = parent.transform;
        Destroy(original);
    }
}
