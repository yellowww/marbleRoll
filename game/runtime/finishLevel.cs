using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finishLevel : MonoBehaviour
{
    main main;
    playMode playMode;
    private void Start()
    {
        main = FindObjectOfType<main>();
        playMode = FindObjectOfType<playMode>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "marble" && main.checkpoints == 3)
        {
            main.inBetweenLevel = true;
            playMode.shadeUI(0);
            hidePlayButton();
        }
    }

    void hidePlayButton()
    {
        GameObject PlayButton = GameObject.Find("playButton");
        Color color = new Color32(255, 255, 255, 0);
        PlayButton.GetComponent<Image>().color = color;
    }

}
