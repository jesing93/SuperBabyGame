using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown screenDropdown;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject controlsPanel;
    Resolution[] resolutions;

    private void Start()
    {
        //Get all resolutions availables from the user system
        resolutions = Screen.resolutions;

        //Clear default options in dropdown
        resolutionsDropdown.ClearOptions();
        qualityDropdown.ClearOptions();
        screenDropdown.ClearOptions();

        //Add the screen options
        List<string> screenOptions = new List<string>();
        screenOptions.Add("FullScreen");
        screenOptions.Add("Borderless");
        screenOptions.Add("Windowed");
        screenDropdown.AddOptions(screenOptions);


        //Add the quality options
        List<string> qualityOptions = new List<string>();
        qualityOptions.Add("Low");
        qualityOptions.Add("Medium");
        qualityOptions.Add("High");
        qualityDropdown.AddOptions(qualityOptions);

        int currentResolution = 0;
        //create a list of strings with the resolutions from the array resolutions so unity can work with the propper formating
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add(resolutions[i].width + "x" + resolutions[i].height);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        //Add the values from the resolutions list to the dropdown
        resolutionsDropdown.AddOptions(resolutionOptions);
        resolutionsDropdown.value = currentResolution;
        resolutionsDropdown.RefreshShownValue();

        ScreenMode();
    }

    //close the game
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// change the master volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        if (PlayerPrefs.HasKey("volume"))
        {
           volume = PlayerPrefs.GetFloat("volume");
        }
        audioMixer.SetFloat("masterVolume", volume);
    }

    /// <summary>
    /// Change the screenmode to fullscreen or windowed
    /// </summary>
    /// <param name="fullScreen"></param>
    public void ScreenMode()
    {
        int fullScreen;
        if (PlayerPrefs.HasKey("screenMode"))
        {
            fullScreen = PlayerPrefs.GetInt("screenMode");
        }
        else
        {
            fullScreen = screenDropdown.value;
            PlayerPrefs.SetInt("screenMode", fullScreen);
        }
        switch (fullScreen)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                default:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
            }    
        
       
    }

    public void SetQuality()
    {
        int quality = qualityDropdown.value;

        if (PlayerPrefs.HasKey("quality"))
        {
            quality = PlayerPrefs.GetInt("quality");
        }
        else
        {
            quality = qualityDropdown.value;
            PlayerPrefs.SetInt("quality", quality);
        }
        switch (quality)
        {
            case 0:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(3, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(5, true);
                break;
            default:
                QualitySettings.SetQualityLevel(5, true);
                break;
        }
    }

    /// <summary>
    /// change the game resoluton
    /// </summary>
    /// <param name="index">index of the selected resolution in the menu</param>
    public void SetResolution(int index)
    {
        if (PlayerPrefs.HasKey("resolution"))
        {
            index = PlayerPrefs.GetInt("resolution");
        }
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
    
}
