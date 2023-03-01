using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string text;
    protected bool interactive = false;
    [SerializeField] protected UserInterface display;

    private void Awake() {
        display = FindObjectOfType<UserInterface>();
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
