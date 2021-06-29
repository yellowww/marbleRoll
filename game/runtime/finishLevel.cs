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

            GameObject text = GameObject.Find("endLevelText");
            Text textComponent = text.GetComponent<Text>();
            textComponent.text = "Level " + main.currentLevel.ToString() + " Completed!";
            main.currentLevel++;
            main.levelDificulty += 5 / (main.currentLevel / 5 + 1);
            textComponent.enabled = true;

            Image nextButton = GameObject.Find("continueButton").GetComponent<Image>();
            nextButton.enabled = true;
        }
    }

    void hidePlayButton()
    {
        GameObject PlayButton = GameObject.Find("playButton");
        Color color = new Color32(255, 255, 255, 0);
        PlayButton.GetComponent<Image>().color = color;
    }

}
