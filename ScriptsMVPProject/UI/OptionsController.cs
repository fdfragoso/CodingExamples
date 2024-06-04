using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown, aspectRatioDropdown;

    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    [SerializeField] private List<ResItem> aspects = new List<ResItem>();

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider,musicSlider, effectsSlider, lobbySlider;

    private void Start()
    {
        SetupResolutions();
        SetupAspectRatios();
        LoadVolume();
        SetupVolumeSlider();
    }

    private void SetupResolutions()
    {
        List<string> options = new List<string>();

        foreach (ResItem res in resolutions)
        {
            string option = res.horizontal + "x" + res.vertical;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

        for (int i = 0; i < resolutions.Count; i++) //Select Current Res
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                resolutionDropdown.value = i;
                break;
            }
        }
    }

    private void SetupAspectRatios()
    {
        List<string> options = new List<string>();

        foreach (ResItem res in aspects)
        {
            string option = res.horizontal + "x" + res.vertical;
            options.Add(option);
        }

        aspectRatioDropdown.AddOptions(options);
    }


    public void ApplyGraphicsSettings ()
    {
        Screen.fullScreen = fullscreenToggle.isOn;

        Screen.SetResolution(resolutions[resolutionDropdown.value].horizontal, resolutions[resolutionDropdown.value].vertical, fullscreenToggle.isOn);


        //Aspect Ratio
        float x = Screen.height * (float)(aspects[aspectRatioDropdown.value].horizontal / (float)aspects[aspectRatioDropdown.value].vertical);
        float y = Screen.height;
        //Screen.SetResolution(x , y, fullscreenToggle.isOn);
    }

    private List<string> GetVolumeTypes()
    {
        List<string> types = new List<string>
        { "Master", "Music", "Effects"};
        if (lobbySlider != null)
            types.Add("Lobby");

        return types;
    }

    private void SetupVolumeSlider()
    {
        foreach(string type in GetVolumeTypes())
        {
            SetVolumeByType(type);
        }
    }

    public void SetVolumeByType(string type)
    {
        float volume = 1;

        switch (type)
        {
            case "Master":
                volume = masterSlider.value;
                break;
            case "Music":
                volume = musicSlider.value;
                break;
            case "Effects":
                volume = effectsSlider.value;
                break;
            case "Lobby":
                volume = lobbySlider.value;
                break;
        }
        
        mixer.SetFloat(type, Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat(type + "Volume", volume);
    }

    private void LoadVolume()
    {
        foreach(string type in GetVolumeTypes())
        {
            switch (type)
            {
                case "Master":
                    masterSlider.value = PlayerPrefs.GetFloat(type + "Volume", 1);
                    break;
                case "Music":
                    musicSlider.value = PlayerPrefs.GetFloat(type + "Volume", 1);
                    break;
                case "Effects":
                    effectsSlider.value = PlayerPrefs.GetFloat(type + "Volume", 1);
                    break;
                case "Lobby":
                    lobbySlider.value = PlayerPrefs.GetFloat(type + "Volume", 1);
                    break;
            }
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
