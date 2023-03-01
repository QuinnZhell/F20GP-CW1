using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Interactable
{

    public GameObject keyPrefab;
    private GameObject keyInstance;

    private void Start() {
        keyInstance = Instantiate(keyPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), keyPrefab.transform.rotation);
    }

    // Update is called once per frame
    private void Update() {
        if(interactive && Input.GetButtonDown("Interact"))
            Collect();

        keyInstance.transform.Rotate(0, 0.4f, 0.4f, Space.Self);
    }

    private void Collect() {
        gameManager.TreasureCollected();
        Remove();
        Destroy(keyInstance);
    }
}
