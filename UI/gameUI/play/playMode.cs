using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playMode : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    marbleInit marbleInit;

    public Sprite playButton;
    public Sprite exitPlayButton;
    private void Start()
    {
        main = FindObjectOfType<main>();
        marbleInit = FindObjectOfType<marbleInit>();
        
    }
    public void enterPlayMode()
    {
        main.inPlayMode = true;
        marbleInit.initiateMarble();
        this.gameObject.GetComponent<Image>().sprite = exitPlayButton;
        shadeUI(50);

    }

    public void exitPlayMode()
    {
        if(!main.inBetweenLevel)
        {
            main.inPlayMode = false;
            marbleInit.removeMarbles();
            this.gameObject.GetComponent<Image>().sprite = playButton;
            shadeUI(255);
        }

    }

    public void shadeUI(byte a)
    {
        GameObject thisObject;
        Color thisColor = new Color32(255, 255, 255, a);
        thisObject = GameObject.Find("rotateLeft");
        thisObject.GetComponent<Image>().color = thisColor;

        thisObject = GameObject.Find("rotateRight");
        thisObject.GetComponent<Image>().color = thisColor;

        thisObject = GameObject.Find("hotbarCanvas");
        foreach (Transform child in thisObject.transform)
        {
            GameObject obj = child.gameObject;
            obj.GetComponent<Image>().color = thisColor;
        }
    }

   

    public void onClick()
    {
        if(main.inPlayMode)
        {
            exitPlayMode();
        } else
        {
            enterPlayMode();
        }
    }
}
