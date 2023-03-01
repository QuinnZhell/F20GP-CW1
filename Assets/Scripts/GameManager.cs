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
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void applyDamage(float damage) {
        currentHealth = currentHealth - damage;
        UI.SetHealth((int)(currentHealth > 0 ? currentHealth : 0));

        if (currentHealth < 0) {
            Death();
        } 
    }

    public void Death() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
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
        vaultDoor.locked = false;
    }
}
