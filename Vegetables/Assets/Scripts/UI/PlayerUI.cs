using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI scoreText;
    [SerializeField]TextMeshProUGUI vegetableCountText;
    GameManager gameManager;
    
    private void OnEnable() {
        GameManager.OnPointUpdate += UpdatScoreText;
        GameManager.OnVegetableCountUpdate += UpdateVegetableCountText;
    }

    private void OnDisable(){
        GameManager.OnPointUpdate -= UpdatScoreText;
        GameManager.OnVegetableCountUpdate -= UpdateVegetableCountText;
    }

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }
   
    void UpdatScoreText()
    {
        scoreText.text = "Score:" + gameManager.GetPlayerScore();
    }

    void UpdateVegetableCountText()
    {
      vegetableCountText.text = "Vegetables:" +gameManager.GetVegetableAmount();   
    }
}
