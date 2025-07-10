using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera PlayerOneCamera;
    public Camera PlayerTwoCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerOneCamera.rect = new Rect(0, 0.5f, 1, 0.5f);
        PlayerTwoCamera.rect = new Rect(0, 0, 1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
