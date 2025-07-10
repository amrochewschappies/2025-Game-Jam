using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class ControllerLoadingMenuNav : MonoBehaviour
{
    private PlayerInput playerInput;
    private Gamepad currentGamepad;
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        currentGamepad = Gamepad.current;
    }

    private void Update()
    {
        if (currentGamepad != null)
        {
            bool controllerPressed = currentGamepad != null && currentGamepad.startButton.wasReleasedThisFrame;
            bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame;

            if (controllerPressed || spacePressed)
            {
                Debug.Log("sdlnfnskdfns");
                StartCoroutine(skipLoadingMenu());
            }
        }
    }

    
    private IEnumerator skipLoadingMenu()
    {
        yield return new WaitForSeconds(0.5f);


        SceneManage.smInstance.isLoading = false;
        SceneManage.smInstance.isLoaded = false;


        SceneManage.smInstance.LoadTutorialScene();
        Debug.Log("kkkkkkkkkkkkkkkkkksdlnfnskdfns");
    }

     private void onGui()
     {
         
     }
}
