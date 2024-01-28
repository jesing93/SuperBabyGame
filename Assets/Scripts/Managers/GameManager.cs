using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject menuPause;
    public GameObject menuOptions;
    private bool isPaused;
    private bool isGameStarted;
    private bool isGameEnded;
    public Slider timeSlider;
    public float timeStandard = 10;
    public GameObject sliderPoint;
    bool isCatched;
    public GameObject paperInventory;
    public GameObject panelDerrota;
    public GameObject panelVictoria;
    public GameObject panelDialog;
    
    void Start()
    {
        isGameStarted = true;
        panelDerrota.SetActive(false);
        isGameEnded = false;
        Time.timeScale = 1.0f;
        
    }

    void Update()
    {
        timeSlider.value = timeStandard;
        timeStandard -= Time.deltaTime;
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
        if (timeStandard <= 0)
        {
            Timing();
        }
        Paper();
    }
    private void Pause()
    {
        isPaused = true;
        menuPause.SetActive(true);
        //CameraController.instance.TogglePause(isPaused);
        DOTween.Kill("CamRot", false);
        Time.timeScale = 0.0f;
        menuOptions.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.Instance.TogglePause();
        panelDialog.SetActive(false);
    }
    public void Unpause()
    {
        isPaused = false;
        //CameraController.instance.TogglePause(isPaused);
        Time.timeScale = 1.0f;
        menuPause.SetActive(false);
        menuOptions.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.Instance.TogglePause();
    }
    public void Timing()
    {
        Time.timeScale = 0.0f;
        sliderPoint.SetActive(false);
        isGameEnded = true;
        panelDerrota.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        panelDialog.SetActive(false);
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
    public void Restart()
    {
        isGameEnded = false;
        isGameStarted = true;
        Debug.Log("Cañita" + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
    public void Victory()
    {
        panelVictoria.SetActive(true);
    }
}
