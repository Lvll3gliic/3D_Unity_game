using System.Collections;
using UnityEngine;
using TMPro; 

public class DeerSpawner : MonoBehaviour
{
    public GameObject deerPrefab;
    public Terrain terrain;
    public float spawnInterval = 3f;
    public int maxDeerCount = 10;

    private int currentDeerCount = 0;
    private Renderer referenceObjectRenderer;



    private void Start()
    {

        
        StartCoroutine(SpawnDeerCoroutine());
    }

    private IEnumerator SpawnDeerCoroutine()
    {
        while (currentDeerCount < maxDeerCount)
        {
            Vector3 spawnPosition = GetRandomSpawnPositionOnTerrain();
            GameObject deer = Instantiate(deerPrefab, spawnPosition, Quaternion.identity);
            currentDeerCount++;

            DeerMovement deerMovement = deer.GetComponent<DeerMovement>();
            if (deerMovement == null)
            {
                deerMovement = deer.AddComponent<DeerMovement>();
            }

            deerMovement.moveSpeed = Random.Range(2f, 4f);
            deerMovement.changeDirectionInterval = Random.Range(2f, 5f);
            deerMovement.minBounds = terrain.transform.position;
            deerMovement.maxBounds = terrain.transform.position + terrain.terrainData.size;
            deerMovement.groundOffset = 0.1f;
            deerMovement.groundLayer = LayerMask.GetMask("Ground");

            Animator deerAnimator = deer.GetComponent<Animator>();

            // Set the animator to the entry state
            deerAnimator.Play(deerAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, Random.value);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPositionOnTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;
        float randomX = Random.Range(0f, terrainWidth);
        float randomZ = Random.Range(0f, terrainLength);
        float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ));
        Vector3 spawnPosition = new Vector3(randomX, terrainHeight, randomZ);
        return spawnPosition;
    }
}
