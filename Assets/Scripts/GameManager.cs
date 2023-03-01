using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // player
    public float maxHealth = 100.0f;
    private float currentHealth;

    // win condition is collecting 5 treasures and defeating the boss
    int treasureCollected;
    [SerializeField] UserInterface UI;
    [SerializeField] Door vaultDoor;

    // set the win and loss conditions to starting state
    private void Awake() {
        treasureCollected = 0;
        currentHealth = maxHealth;
    }

    // player has taken damage, if they reach 0 health they will lose
    public void applyDamage(float damage) {
        // remove damage from health pool
        currentHealth = currentHealth - damage;

        // display change
        UI.SetHealth((int)(currentHealth > 0 ? currentHealth : 0));

        // check if dead
        if (currentHealth < 0) {
            WinOrLose(2);
        } 
    }

    // depending on parameter, display win or lose screen
    // 2 = lose, 3 = win
    public void WinOrLose(int i) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(i);
    }

    // a treasure has been collected
    public void TreasureCollected() {
        // increment counter and update display
        treasureCollected++;
        UI.TreasureCollect(treasureCollected);

        // if all treasure collected, unlock path to boss
        if(treasureCollected == 5)
            vaultDoor.Unlock();
    }

    public void SetActiveUI(UserInterface ui) {
        UI = ui;
    }

    public void SetActiveDoor(Door door) {
        vaultDoor = door;
    }
}
