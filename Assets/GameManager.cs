using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int treasureCollected;
    [SerializeField] UserInterface UI;
    [SerializeField] Door vaultDoor;

    private void Awake() {
        treasureCollected = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TreasureCollected() {
        treasureCollected++;
        UI.TreasureCollect(treasureCollected);

        if(treasureCollected == 5)
            vaultDoor.Unlock();
    }

    public void SetActiveUI(UserInterface ui) {
        UI = ui;
    }

    public void SetActiveDoor(Door door) {
        vaultDoor = door;
        vaultDoor.locked = true;
    }
}
