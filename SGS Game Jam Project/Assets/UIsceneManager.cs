using UnityEngine;
using UnityEngine.UI;

public class UIsceneManager : MonoBehaviour
{
    void Start()
    {
        GameObject buttonObj = GameObject.FindWithTag("StartButton");
        if (buttonObj != null)
        {
            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners(); // Optional: clear old bindings
                btn.onClick.AddListener(() => SceneManage.smInstance.LoadScene());
                Debug.Log("Bound LoadScene to Start button dynamically.");
            }
            else
            {
                Debug.LogWarning("No Button component found on StartButton.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'StartButton' found.");
        }
    }
}
