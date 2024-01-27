using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [SerializeField]
    private GameObject[] settingsItems;
    [SerializeField]
    private AudioMixer audioMixer;
    private bool isLoadingSettings;
    private void Awake()
    {
        instance = this;
        LoadPrefs();
    }
    private void Start()
    {
        ApplyPrefs();
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
}
