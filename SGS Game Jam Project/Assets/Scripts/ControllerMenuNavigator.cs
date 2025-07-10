using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class ControllerMenuNavigator : MonoBehaviour
{
    private EventSystem eventSystem;
    private Gamepad gamepad;
    private int currentSelectionIndex = 0;
    public GameObject[] menuButtons;

    private Color normalColor = Color.white;
    private Color highlightedColor = Color.red;
    private float inputCooldown = 0.2f;
    private float nextInputTime = 0f;
    
    public GameObject startPanel;
    public GameObject patchNotesPanel;

    private bool isPatchNotesActive = false;

    void Start()
    {
      
        eventSystem = EventSystem.current;
        if (Gamepad.all.Count > 0)
        {
            gamepad = Gamepad.current;
        }
        if (menuButtons.Length > 0)
        {
            SelectButton(currentSelectionIndex);
        }
    }

    void Update()
    {
        if (gamepad == null && Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }

        if (gamepad != null && Time.time >= nextInputTime)
        {
            Vector2 dpadInput = gamepad.dpad.ReadValue();

            if (dpadInput.y > 0.5f)
            {
                NavigateUp();
                nextInputTime = Time.time + inputCooldown;
            }
            else if (dpadInput.y < -0.5f)
            {
                NavigateDown();
                nextInputTime = Time.time + inputCooldown;
            }
        }
        
        bool escPressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        bool bPressed = gamepad != null && gamepad.buttonEast.wasPressedThisFrame;

        if ((escPressed || bPressed) && patchNotesPanel.activeSelf)
        {
            GoBackToStart();
        }
    }

    void NavigateUp()
    {
        currentSelectionIndex--;
        if (currentSelectionIndex < 0) currentSelectionIndex = menuButtons.Length - 1;
        SelectButton(currentSelectionIndex);
    }

    void NavigateDown()
    {
        currentSelectionIndex++;
        if (currentSelectionIndex >= menuButtons.Length) currentSelectionIndex = 0;
        SelectButton(currentSelectionIndex);
    }

    void SelectButton(int index)
    {
        Debug.Log("Selected Button Index: " + index);
    
        if (index >= 0 && index < menuButtons.Length)
        {
            DeselectAllButtons();

            GameObject selectedObj = menuButtons[index];
            eventSystem.SetSelectedGameObject(selectedObj);

            Button selectedButton = selectedObj.GetComponent<Button>();
            if (selectedButton != null && selectedButton.onClick != null)
            {
           
                if (selectedButton.transition != Selectable.Transition.ColorTint)
                    selectedButton.transition = Selectable.Transition.ColorTint;

                ExecuteEvents.Execute(selectedObj, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
            }
        }
    }


   void DeselectAllButtons()
    {
        foreach (GameObject buttonObj in menuButtons)
        {
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                ColorBlock colorBlock = button.colors;
                colorBlock.highlightedColor = normalColor;
                button.colors = colorBlock;  
            }
        }
    }
   
    public void GoBackToStart()
    {
        patchNotesPanel.SetActive(false);
        startPanel.SetActive(true);

        menuButtons = startPanel.GetComponentsInChildren<Button>(true)
            .Select(b => b.gameObject)
            .ToArray();
        currentSelectionIndex = 0;
        SelectButton(currentSelectionIndex);
    }
    public void OnPatchNotesClicked()
    {
        startPanel.SetActive(false);
        patchNotesPanel.SetActive(true);
        menuButtons = patchNotesPanel.GetComponentsInChildren<Button>(true)
            .Select(b => b.gameObject)
            .ToArray();
        currentSelectionIndex = 0;
        SelectButton(currentSelectionIndex);
    }
}