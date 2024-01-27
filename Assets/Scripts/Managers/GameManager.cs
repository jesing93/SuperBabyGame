using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    public float TimeToLive = 60f;
    public float TimeWhenDestroy;
    public GameObject menuPause;
    public GameObject menuOptions;
    private bool isPaused;
    private bool isGameStarted;
    private bool isGameEnded;
    void Start()
    {
        TimeWhenDestroy = Time.time + TimeToLive;
        isGameStarted = true;
    }

    void Update()
    {
        if (Time.time >= TimeWhenDestroy && !isGameEnded)
        {
            Debug.Log("TODO: Game over, baby");
            isGameEnded = true;
        }
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
}
