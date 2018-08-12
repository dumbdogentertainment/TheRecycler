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

    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    void Start()
    {
        this.spawnerCollider = GetComponent<BoxCollider2D>();
        this.spawnCooldownRemaining = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.spawnCooldownRemaining -= Time.deltaTime;

        this.minSpawnInterval = this.minSpawnInterval < 0.15f ? 0.15f : this.minSpawnInterval;
        this.maxSpawnInterval = this.maxSpawnInterval < 0.25f ? 0.25f : this.maxSpawnInterval;

        if (this.spawnCooldownRemaining <= 0f)
        {
            SpawnTrashBag();
            this.spawnCooldownRemaining = Random.Range(
                this.minSpawnInterval,
                this.maxSpawnInterval);
        }
    }

    void SpawnTrashBag()
    {
        if (this.spawnerCollider.IsTouchingLayers())
        {
            return;
        }

        var randomIndex = Random.Range(0, this.TrashBagPrefabs.Length);
        Instantiate(this.TrashBagPrefabs[randomIndex], this.transform.position, Quaternion.identity, this.GameBoard.transform);
    }
}
