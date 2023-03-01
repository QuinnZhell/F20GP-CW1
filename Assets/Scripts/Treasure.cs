using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Interactable
{

    // Update is called once per frame
    private void Update() {
        if(interactive && Input.GetButtonDown("Interact"))
            Collect();
    }

    private void Collect() {
        gameManager.TreasureCollected();
        Remove();
    }
}
