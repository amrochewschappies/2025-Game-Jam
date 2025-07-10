using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject player1;
    public GameObject Podium;
    public Camera PodiumCamera;
    public PlayerController _player1;
    public Animator PlayerAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerAnimator.SetBool("IsRunning", false);
            _player1.enabled = false;
            PodiumCamera.enabled = true;
            player1.transform.position = new Vector3(Podium.transform.position.x, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            StartCoroutine(waitBeforeLoadingStart());

        }
    }

    IEnumerator waitBeforeLoadingStart()
    {
        yield return new WaitForSeconds(3f);
    
        if (SceneManage.smInstance != null)
        {
            SceneManage.smInstance.LoadStartScene();
            Debug.Log("Loaded Start Scene from Tutorial Scene");
        }
        else
        {
            Debug.LogError("SceneManage instance is not initialized!");
        }
    }
}
