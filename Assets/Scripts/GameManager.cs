using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // player
    public float maxHealth = 100.0f;
    private float currentHealth;

    int treasureCollected;
    [SerializeField] UserInterface UI;
    [SerializeField] Door vaultDoor;

    private void Awake() {
        treasureCollected = 0;
        currentHealth = maxHealth;
    }

    public void applyDamage(float damage) {
        currentHealth = currentHealth - damage;
        UI.SetHealth((int)(currentHealth > 0 ? currentHealth : 0));

        if (currentHealth < 0) {
            WinOrLose(2);
        } 
    }

    public void WinOrLose(int i) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(i);
    }

    public void TreasureCollected() {
        treasureCollected++;
        UI.TreasureCollect(treasureCollected);

        if(treasureCollected == 5)
            vaultDoor.Unlock();

        if(treasureCollected > 5) {
            WinOrLose(3);
        }
    }

    public void SetActiveUI(UserInterface ui) {
        UI = ui;
    }

    public void SetActiveDoor(Door door) {
        vaultDoor = door;
    }
}
