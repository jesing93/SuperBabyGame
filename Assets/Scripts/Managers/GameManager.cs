using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject menuPause;
    public GameObject menuOptions;
    private bool isPaused;
    private bool isGameStarted;
    private bool isGameEnded;
    public Slider timeSlider;
    public float timeStandard = 300;
    public float currentTime;
    public GameObject sliderPoint;
    bool isCatched;
    public GameObject paperInventory;
    void Start()
    {
        isGameStarted = true;
        
    }

    void Update()
    {

        if (isGameStarted && !isGameEnded && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
        Timing();
        Paper();
    }
    private void Pause()
    {
        isPaused = true;
        menuPause.SetActive(true);
        //CameraController.instance.TogglePause(isPaused);
        DOTween.Kill("CamRot", false);
        Time.timeScale = 0;
        menuOptions.SetActive(false);
    }
    public void Unpause()
    {
        isPaused = false;
        //CameraController.instance.TogglePause(isPaused);
        Time.timeScale = 1.0f;
        menuPause.SetActive(false);
        menuOptions.SetActive(false);
    }
    public void Timing()
    {
        timeSlider.value = currentTime;
        currentTime = timeStandard - Time.time;
        if (currentTime <= 0 && !isGameEnded)
        {
            sliderPoint.SetActive(false);
            isGameEnded = true;
        }
    }
    public void Paper()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(isCatched)
            {
                paperInventory.SetActive(false);
                isCatched = false;
            } else
            {
                paperInventory.SetActive(true);
                isCatched = true;
            }
        }
    }
}
