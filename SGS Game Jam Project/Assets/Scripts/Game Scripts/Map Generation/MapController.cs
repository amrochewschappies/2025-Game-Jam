using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour
{
    public GameObject[] Tiles;
    public GameObject SmokeVfx;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int i = 0; i < 45; i++)
        {
            int randomTile = Random.Range(0, Tiles.Length);
            int randomNumber = Random.Range(1, 4) * 15;
            Vector3 targetScale = new Vector3(100, 100, randomNumber);

            // Start moving tile
            StartCoroutine(MoveTile(Tiles[randomTile], targetScale));

            // Spawn smoke at the original position
            StartCoroutine(SpawnSmoke(new Vector3(Tiles[randomTile].transform.position.x, 0, Tiles[randomTile].transform.position.z)));
        }
    }

    IEnumerator MoveTile(GameObject tile, Vector3 targetScale)
    {
        float duration = 5f; // Time in seconds
        float elapsedTime = 0;

        Vector3 startScale = tile.transform.localScale;
        Vector3 startPosition = tile.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate scale
            tile.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            // Update position dynamically based on the interpolated scale
            float newY = tile.transform.localScale.z * 0.0397325f;
            tile.transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            yield return null;
        }

        // Ensure final values are set precisely
        tile.transform.localScale = targetScale;
        tile.transform.position = new Vector3(startPosition.x, targetScale.z * 0.0397325f, startPosition.z);
    }

    IEnumerator SpawnSmoke(Vector3 position)
    {
        yield return new WaitForSeconds(1f);
        Instantiate(SmokeVfx, position, Quaternion.identity);
    }
}
