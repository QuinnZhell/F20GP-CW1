using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI prompt;
    [SerializeField] private TextMeshProUGUI treasureText;

    // treasures collected
    int treasureCount = 0;

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

    public void TreasureCollect() {
        treasureCount++;
        treasureText.text = "Treasure Collected: " + treasureCount + "/ 5";
    }
}
