using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// superclass which allows similar behaviour to pass to children
public class Interactable : MonoBehaviour
{
    [SerializeField] protected string text;
    [SerializeField] protected bool interactive = false;
    [SerializeField] protected UserInterface display;
    [SerializeField] protected GameManager gameManager;

    // assign important members
    private void Start() {
        display = FindObjectOfType<UserInterface>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // display prompt when player is close
    public virtual void OnTriggerEnter(Collider other) {
        if(other.name == "Player"){
            display.ShowPrompt(text);
            interactive = true;
        }
    }

    // remove prompt when player moves out of range
    public virtual void OnTriggerExit(Collider other) {
        if(other.name == "Player"){
            display.ShowPrompt("");
            interactive = false;
        }
    }

    // remove the interactable from the game world
    protected virtual void Remove(){
        display.ShowPrompt("");
        Destroy(gameObject);
    }
}
