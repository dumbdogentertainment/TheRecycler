using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spawner : MonoBehaviour
{
    public GameObject[] TrashBagPrefabs;
    public GameObject GameBoard;

    private BoxCollider2D spawnerCollider;

    private float spawnCooldownRemaining;

    void Start()
    {
        this.spawnerCollider = GetComponent<BoxCollider2D>();
        this.spawnCooldownRemaining = Random.Range(0.15f, 0.75f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.spawnCooldownRemaining -= Time.deltaTime;

        if (this.spawnCooldownRemaining <= 0f)
        {
            SpawnTrashBag();
            this.spawnCooldownRemaining = Random.Range(0.15f, 0.75f);
        }
    }

    void SpawnTrashBag()
    {
        if (this.spawnerCollider.IsTouchingLayers())
        {
            return;
        }

        var randomIndex = Random.Range(0, this.TrashBagPrefabs.Length - 1);
        Instantiate(this.TrashBagPrefabs[randomIndex], this.transform.position, Quaternion.identity, this.GameBoard.transform);
    }
}
