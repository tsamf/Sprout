using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VegetableCountText : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    GameManager gameManager;
    
    private void Awake() {
        textMeshProUGUI =  GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();
    }
    
    private void OnEnable() {
        GameManager.OnVegetableCountUpdate += UpdateVegetableCountText;
    }

    private void OnDisable(){
        GameManager.OnVegetableCountUpdate -= UpdateVegetableCountText;
    }

    // Update is called once per frame
    void UpdateVegetableCountText()
    {
      textMeshProUGUI.text = "Vegetables:" +gameManager.GetVegetableAmount();   
    }
}
