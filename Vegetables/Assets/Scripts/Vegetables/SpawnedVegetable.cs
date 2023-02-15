using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedVegetable : MonoBehaviour
{
    [SerializeField] List<Sprite> possibleVegetableSprites;

    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //Assign a random vegetable sprite to the spawned vegetable
        int spriteIndex = Random.Range(0, possibleVegetableSprites.Count);
        spriteRenderer.sprite= possibleVegetableSprites[spriteIndex];
    }
}
