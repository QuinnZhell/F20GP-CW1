using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected string text;
    [SerializeField] protected bool interactive = false;
    [SerializeField] protected UserInterface display;
    [SerializeField] protected GameManager gameManager;

    private void Start() {
        display = FindObjectOfType<UserInterface>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public virtual void OnTriggerEnter(Collider other) {
        if(other.name == "Player"){
            display.ShowPrompt(text);
            interactive = true;
        }
    }
    public virtual void OnTriggerExit(Collider other) {
        if(other.name == "Player"){
            display.ShowPrompt("");
            interactive = false;
        }
    }

    protected virtual void Remove(){
        display.ShowPrompt("");
        Destroy(gameObject);
    }
}
