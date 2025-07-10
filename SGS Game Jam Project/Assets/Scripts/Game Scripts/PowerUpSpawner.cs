using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] Tiles;               // Array to store child tiles
    public GameObject TilesParent;           // The parent GameObject containing the tiles
    public float interval = 0f;              // Time interval for spawning power-ups
    public GameObject[] PowerUpPrefabs;         // The prefab to be spawned
    public float spawnHeightOffset = 3f;     // The height offset above the tile's Y position to spawn the power-up

    // Start is called once before the first execution of Update
    void Start()
    {
        // Get all child tiles under TilesParent
        Tiles = GetChildren(TilesParent);
    }

    // Update is called once per frame
    void Update()
    {
        if (interval >= 12f)
        {
            SpawnPowerUp();
            interval = 0f; // Reset the interval after spawning
        }
        else
        {
            interval += Time.deltaTime; // Increment the interval
        }
    }

    // Method to get all child objects under the TilesParent
    GameObject[] GetChildren(GameObject parent)
    {
        // Get all child transforms and convert them to GameObject array
        Transform[] childTransforms = parent.GetComponentsInChildren<Transform>();
        GameObject[] children = new GameObject[childTransforms.Length - 1]; // Exclude the parent itself

        for (int i = 1; i < childTransforms.Length; i++) // Start from 1 to skip the parent itself
        {
            children[i - 1] = childTransforms[i].gameObject;
        }

        return children;
    }

    // Method to spawn the power-up
    void SpawnPowerUp()
    {
        // Get a random tile from the Tiles array
        int randomIndex = Random.Range(0, Tiles.Length);
        GameObject randomTile = Tiles[randomIndex];

        // Get the position of the tile (we will use the X and Z, but add an offset to the Y position)
        Vector3 spawnPosition = randomTile.transform.position;
        spawnPosition.y += spawnHeightOffset; // Add offset to Y position

        // Instantiate the PowerUpPrefab at the new spawn position
        Instantiate(PowerUpPrefabs[Random.Range(0, 3)], spawnPosition, Quaternion.identity);
    }
}
