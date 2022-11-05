using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resDropdown;


    void Awake()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        foreach (Resolution res in resolutions)
        {
            options.Add(res.width + "x" + res.height);

            if(res.width == Screen.currentResolution.width &&
               res.height == Screen.currentResolution.height){
                currentResolutionIndex = options.IndexOf(res.width + "x" + res.height);
            }

        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex){
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        //TODO: implement
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}
