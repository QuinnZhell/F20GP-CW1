using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected string text;
    protected bool interactive = false;
    [SerializeField] protected UserInterface display;
    [SerializeField] protected GameManager gameManager;

    private void Start() {
        display = FindObjectOfType<UserInterface>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public virtual void OnTriggerEnter(Collider other) {
        display.ShowPrompt(text);
        interactive = true;
    }
    public virtual void OnTriggerExit(Collider other) {
        display.ShowPrompt("");
        interactive = false;
    }

    protected virtual void Remove(){
        display.ShowPrompt("");
        Destroy(gameObject);
    }
}
