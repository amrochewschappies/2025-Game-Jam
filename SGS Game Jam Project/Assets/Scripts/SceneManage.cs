using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Random = System.Random;
using UnityEngine.InputSystem;


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
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private float timer = 0f;
    [SerializeField] private int currentTime = 0;
    public bool isLoaded = false;
    public bool isLoading = false;


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

            if (Keyboard.current.escapeKey.wasPressedThisFrame ||
               Gamepad.current?.startButton.wasPressedThisFrame == true ||
               Gamepad.current?.selectButton.wasPressedThisFrame == true)
            {
                Debug.Log("Video skipped via input.");
                SceneManager.LoadScene("TutorialScene");
            }

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
        Cursor.lockState = CursorLockMode.None;
        //Debug.Log("StartScene is loading");
    } 

    public void LoadTutorialScene()
    {
        isLoaded = false;
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void LoadScene()
    {
        Debug.Log("Button was clicked");

        if (isLoaded || isLoading) 
        {
            Debug.Log("One of the scene is already loading or has been loaded.");
            return;
        }

        isLoading = true; 
        randomiseScene();
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
        Cursor.lockState = CursorLockMode.None;
    }
    
    //using this for when start button is click to delay the start animation
    private IEnumerator WaitBeforeLoadingMain()
    {
        Debug.Log("Main scene 1 is loading...");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        isLoaded = true;
        isLoading = false; 
        Debug.Log("Game scene loaded.");
        Cursor.lockState = CursorLockMode.Locked;
    }
    private IEnumerator WaitBeforeLoadingJungle()
    {
        Debug.Log("JungleScene is loading...");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameSceneJungle", LoadSceneMode.Single);
        isLoaded = true;
        isLoading = false; 
        Debug.Log("Game scene loaded.");
        Cursor.lockState = CursorLockMode.Locked;
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

    private void randomiseScene()
    {
        int RandomNum = UnityEngine.Random.Range(1,2);
        if (RandomNum >=3)
        {
            StartCoroutine(WaitBeforeLoadingMain());
        }
        else
        {
            StartCoroutine(WaitBeforeLoadingJungle());
        }
    }
}
