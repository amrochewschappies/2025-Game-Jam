using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManage : MonoBehaviour
{
    public static SceneManage smInstance { get; private set; }

    private void Awake()
    {
        if (smInstance != null && smInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            smInstance = this;
        }
    }

    [SerializeField] private float timer = 0f;
    [SerializeField] private int currentTime = 0;
    [SerializeField] private bool isLoaded = false;
    private bool isLoading = false; 

    private void Start()
    {
        currentTime = 0;
        timer = 0;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScene" && !isLoaded)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                currentTime += 1;
                timer = 0f;
            }

            if (currentTime >= 42f)
            {
                LoadTutorialScene();
                isLoaded = true;
            }
        }
        else if (SceneManager.GetActiveScene().name != "LoadingScene")
        {
            timer = 0;
            currentTime = 0;
        }
    }

    public void LoadStartScene()
    {
        isLoaded = false;
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        Debug.Log("StartScene is loading");
    } 

    public void LoadTutorialScene()
    {
        isLoaded = false;
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
    }
    
   public void LoadMainScene()
    {
        if (isLoaded || isLoading) 
        {
            Debug.Log("Main Scene is already loading or has been loaded.");
            return;
        }

        isLoading = true; 
        StartCoroutine(WaitBeforeLoadingMain());
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    
    
    //using this for when player dies or touches the lava
    public IEnumerator WaitBeforeLoading()
    {
        yield return new WaitForSeconds(15f);
        LoadStartScene();
        GameManager.gmInstance.isSceneLoading = false;
    }
    
    //using this for when start button is click to delay the start animation
    private IEnumerator WaitBeforeLoadingMain()
    {
        Debug.Log("Main scene is loading...");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        isLoaded = true;
        isLoading = false; 
        Debug.Log("Game scene loaded.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        isLoaded = scene.name != "StartScene";
        Debug.Log($"Loaded {scene.name}.");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
