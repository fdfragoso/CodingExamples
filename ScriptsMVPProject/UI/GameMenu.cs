using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Toggle visualToggle, audioToggle, controlsToggle;
    [SerializeField] private GameObject visualOption, audioOption, controlsOption;

    public void SetOptionMenuActive()
    {
        visualOption.SetActive(visualToggle.isOn);
        audioOption.SetActive(audioToggle.isOn);
        controlsOption.SetActive(controlsToggle.isOn);
    }
}
