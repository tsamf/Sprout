using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vegetable", fileName = "New Vegetable")]
public class VegetableSO : ScriptableObject
{

    [SerializeField] int amount = 5;
    [SerializeField] Sprite vegetableSprite;

    public Sprite GetVegetableSprite()
    {
        return vegetableSprite;
    }

    public int GetAmount()
    {
        return amount;
    }
}
