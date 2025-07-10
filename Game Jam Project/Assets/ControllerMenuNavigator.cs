using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerMenuNavigator : MonoBehaviour
{
    private EventSystem eventSystem;
    private Gamepad gamepad;
    private int currentSelectionIndex = 0;
    public GameObject[] menuButtons;

    private Color normalColor = Color.white;
    private Color highlightedColor = Color.red;

    void Start()
    {
        eventSystem = EventSystem.current;
        gamepad = Gamepad.current;

        if (menuButtons.Length > 0)
        {
            SelectButton(currentSelectionIndex);
        }
    }

    void Update()
    {
        if (gamepad != null)
        {
            Vector2 dpadInput = gamepad.dpad.ReadValue();

            if (dpadInput.y > 0)
            {
                NavigateUp();
            }
            else if (dpadInput.y < 0)
            {
                NavigateDown();
            }
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
            Button selectedButton = menuButtons[index].GetComponent<Button>();
            if (selectedButton != null)
            {
                ColorBlock colorBlock = selectedButton.colors;
                colorBlock.highlightedColor = highlightedColor;
                selectedButton.colors = colorBlock;

                selectedButton.Select();
                selectedButton.OnSelect(null);
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
            }
        }
    }

}