using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownVegetable : MonoBehaviour
{

    [SerializeField] List<Sprite> possibleVegetableSprites;

    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        int spriteIndex = Random.Range(0, possibleVegetableSprites.Count);
        spriteRenderer.sprite= possibleVegetableSprites[spriteIndex];
    }
}
