using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI prompt;
    [SerializeField] private TextMeshProUGUI treasureText;

    private void Awake() {
        FindAnyObjectByType<GameManager>().SetActiveUI(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPrompt(string text) {
        prompt.text = text;
    }

    public void TreasureCollect(int count) {
        treasureText.text = "Treasure Collected: " + count + "/ 5";
    }
}
