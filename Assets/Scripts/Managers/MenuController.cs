using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [SerializeField]
    private GameObject[] settingsItems;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private TMP_Dropdown resolutionsDropdown;
    [SerializeField]
    private TMP_Dropdown qualityDropdown;
    [SerializeField]
    private TMP_Dropdown screenDropdown;
    private bool isLoadingSettings;
    Resolution[] resolutions;

    private void Awake()
    {
        instance = this;
        LoadPrefs();
    }
    private void Start()
    {
        Time.timeScale = 1.0f;
        ApplyPrefs();
        //Get all resolutions availables from the user system
        resolutions = Screen.resolutions;
        if (resolutionsDropdown != null)
        {
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
        
       
    }
    public void OnSettingsChanged()
    {
        if (!isLoadingSettings)
        {
            //Save settings
            PlayerPrefs.SetFloat("MasterVolume", settingsItems[0].GetComponent<Slider>().value);
            PlayerPrefs.SetFloat("MusicVolume", settingsItems[1].GetComponent<Slider>().value);
            PlayerPrefs.SetFloat("VFXVolume", settingsItems[2].GetComponent<Slider>().value);
            PlayerPrefs.SetFloat("VoiceVolume", settingsItems[3].GetComponent<Slider>().value);
            //Applying to the audio mixer
            ApplyPrefs();
        }
    }
    private void LoadPrefs()
    {
        isLoadingSettings = true;
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            //Update UI
            settingsItems[0].GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
            settingsItems[1].GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
            settingsItems[2].GetComponent<Slider>().value = PlayerPrefs.GetFloat("VFXVolume");
            settingsItems[3].GetComponent<Slider>().value = PlayerPrefs.GetFloat("VoiceVolume");
        }
        isLoadingSettings = false;
    }

    private void ApplyPrefs()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            //Update audio mixer
            audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("VFXVolume"));
            audioMixer.SetFloat("UIVolume", PlayerPrefs.GetFloat("VoiceVolume"));
        }
        /*if (PlayerController.instance != null)
        {
            PlayerController.instance.UpdateSensibility();
        }*/
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ScreenMode()
    {
        int fullScreen;
        
            fullScreen = screenDropdown.value;
            
        
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

       
            quality = qualityDropdown.value;
            
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

        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
    public void Initiating()
    {
        SceneManager.LoadScene(1);
    }
}
