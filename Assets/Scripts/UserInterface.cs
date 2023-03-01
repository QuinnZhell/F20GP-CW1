using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI prompt;
    [SerializeField] private TextMeshProUGUI treasureText;
    [SerializeField] private TextMeshProUGUI healthText;

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
        treasureText.text = count + "/5";
    }

    public void SetHealth(int health) {
        healthText.text = health.ToString();
    }
}
