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
        List<string> screenOptions = new List<string>();
        screenOptions.Add("FullScreen");
        screenOptions.Add("Borderless");
        screenOptions.Add("Windowed");
        screenDropdown.AddOptions(screenOptions);

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
        audioMixer.SetFloat("masterVolume", volume);
    }

    /// <summary>
    /// Change the screenmode to fullscreen or windowed
    /// </summary>
    /// <param name="fullScreen"></param>
    public void ScreenMode()
    {
        int fullScreen = screenDropdown.value;
        Debug.Log(fullScreen);
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
        }
    }

    public void SetQuality()
    {
        int quality = qualityDropdown.value;
        Debug.Log(quality);
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
        }
    }

    /// <summary>
    /// change the game resoluton
    /// </summary>
    /// <param name="index">index of the selected resolution in the menu</param>
    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
    /// <summary>
    /// show the controls panel
    /// </summary>
    public void OpenControlsMenu()
    {
        controlsPanel.SetActive(true);
    }
    /// <summary>
    /// show the setting panel
    /// </summary>
    public void OpenSettingsMenu()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        //pause or resume the game and open or close the ingame menu
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (!menu.activeSelf)
            {
                Time.timeScale = 0f;
                menu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                menu.SetActive(false);
            }

        }
    }
}
