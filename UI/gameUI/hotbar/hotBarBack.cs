using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotBarBack : MonoBehaviour
{
    // Start is called before the first frame update
    main main;
    void Start()
    {
        main = FindObjectOfType<main>();
    }



    public void backHover()
    {
        main.inventoryBackHover = true;
        main.totalInventoryHover = main.inventoryElementHover || main.inventoryBackHover;
    }
    public void backUnhover()
    {
        main.inventoryBackHover = false;
        main.totalInventoryHover = main.inventoryElementHover || main.inventoryBackHover;
    }
}
