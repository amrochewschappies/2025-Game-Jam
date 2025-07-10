using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureTilingLooper : MonoBehaviour
{
    public float speed = 1.0f;         // Speed of the loop
    public float loopDuration = 2.0f;  // Time in seconds for one full loop
    public float startTilingX = 1.07f; // Starting and ending X tiling value
    public float maxTilingX = 2.0f;    // Peak X tiling before reset

    private Renderer rend;
    private Material mat;
    private Vector2 baseTiling;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        baseTiling = mat.mainTextureScale;
    }

    void Update()
    {
        // Time loop in range [0, 1]
        float t = Mathf.Repeat(Time.time / loopDuration, 1.0f);

        // Linear interpolation from start to max and back to start
        float tilingX = Mathf.Lerp(startTilingX, maxTilingX, t);
        baseTiling.x = tilingX;

        mat.mainTextureScale = baseTiling;
    }
}
