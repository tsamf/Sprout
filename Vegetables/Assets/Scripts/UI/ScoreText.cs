using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    GameManager gameManager;
    
    private void Awake() {
        textMeshProUGUI =  GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();
    }
    
    private void OnEnable() {
        GameManager.OnPointUpdate += UpdatScoreText;
    }

    private void OnDisable(){
        GameManager.OnPointUpdate -= UpdatScoreText;
    }
   
    void UpdatScoreText()
    {
        textMeshProUGUI.text = "Score:" + gameManager.GetPlayerScore();
    }
}
