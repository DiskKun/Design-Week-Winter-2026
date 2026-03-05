using UnityEngine;
using UnityEngine.InputSystem;

public class Quit : MonoBehaviour
{
    public GameObject menuPanel;


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeInHierarchy);
        Cursor.lockState = menuPanel.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
