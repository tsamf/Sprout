using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrop : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] Transform dropVegetablePrefab;
    [SerializeField] float dropSpawnPointOffest = 1f;
    [SerializeField][Range(0, 5)] float dropRangeX = 2f;
    [SerializeField][Range(0, 5)] float dropRangeY = 2f;
    [SerializeField] float droprate = .1f;
    [SerializeField] float loadEndScreenDelay = 2f;

    private GameManager gameManager;

    public delegate void OnAllVegetablesDropped(int vegetables);
    public static event OnAllVegetablesDropped onAllVegetablesDropped;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable() {
        Basket.onLevelComplete += StartVegetableDrop;
    }

    private void OnDisable() {
        Basket.onLevelComplete -= StartVegetableDrop;
    }

    private void StartVegetableDrop(int amount)
    {
        StartCoroutine(DropAllVegtables(amount));
    }

    IEnumerator DropAllVegtables(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            gameManager.RemoveVegetables(1);
            DropVegetables(1);
            yield return new WaitForSeconds(droprate);
        }

        yield return new WaitForSeconds(loadEndScreenDelay);
        onAllVegetablesDropped!.Invoke(amount);
    }

    private void DropVegetables(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 vegetableSpawnPoint = transform.position + Vector3.up * dropSpawnPointOffest;

            Transform droppedVegetable = Instantiate(dropVegetablePrefab, vegetableSpawnPoint, Quaternion.identity);

            //Generate a drop velocity based on a range
            float dropVelocityY = UnityEngine.Random.Range(0, dropRangeY);
            float dropVelocityX = UnityEngine.Random.Range(0, dropRangeX);
            Vector2 dropVelocity = new Vector2(dropVelocityX, dropVelocityY);

            //Apply the drop velocity
            droppedVegetable.GetComponent<Rigidbody2D>().velocity = dropVelocity;
        }
    }
}
