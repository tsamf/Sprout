using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vegetable", fileName = "New Vegetable")]
public class VegetableSO : ScriptableObject
{

    [SerializeField] float amount = 5f;
    [SerializeField] Sprite vegetableSprite;

    public Sprite GetVegetableSprite()
    {
        return vegetableSprite;
    }

    public float GetAmount()
    {
        return amount;
    }
}
