using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMode : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    marbleInit marbleInit;
    private void Start()
    {
        main = FindObjectOfType<main>();
        marbleInit = FindObjectOfType<marbleInit>();
    }
    public void enterPlayMode()
    {
        main.inPlayMode = true;
        marbleInit.initiateMarble();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
