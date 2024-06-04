using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform mainMenuOriginalTransform, mainMenuLeftTransform;
    [SerializeField] private GameObject logoMenu, blurredBG, mainMenu, options;
    [SerializeField] private Toggle visualToggle, audioToggle, controlsToggle;
    [SerializeField] private GameObject visualOption, audioOption, controlsOption;
    
    private bool anyKeyPressed = false;
    private Scene scene;
    [SerializeField] private int initialSceneIndex = 1;

    void Update()
    {
        if(!anyKeyPressed)
        {
            InputSystem.onAnyButtonPress
                .CallOnce(ctrl => OnAnyKeyPressed(ctrl));
        }
    }

    public void ToggleOptions()
    {
        if (!options.activeSelf)
        {
            options.SetActive(true);
            (mainMenu.transform as RectTransform).anchoredPosition = (mainMenuLeftTransform as RectTransform).anchoredPosition;
        }
        else
        {
            options.SetActive(false);
            (mainMenu.transform as RectTransform).anchoredPosition = (mainMenuOriginalTransform as RectTransform).anchoredPosition;
        }
    }

    public void SetOptionMenuActive()
    {
        visualOption.SetActive(visualToggle.isOn);
        audioOption.SetActive(audioToggle.isOn);
        controlsOption.SetActive(controlsToggle.isOn);
    }

    /// TODO: Now it is just a simple load scene, but a better system need to be put in place
    /// <summary>
    /// Load the first scene of the game
    /// </summary>
    public void StartNewGame()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        scene = SceneManager.LoadScene(initialSceneIndex, parameters);
    }

    private void OnAnyKeyPressed(InputControl ctrl)
    {
        Debug.Log($"{ctrl} pressed");
        logoMenu.SetActive(false);
        blurredBG.SetActive(true);
        mainMenu.SetActive(true);
        anyKeyPressed = true;
    }
}
