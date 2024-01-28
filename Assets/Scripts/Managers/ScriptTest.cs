using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScriptTest : MonoBehaviour
{
    public GameObject panelDerrota;
    public float timeStandard = 5;
    public float currentTime = 5;
    public Slider timeSlider;
    public GameObject sliderPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSlider.value = timeStandard;
        timeStandard -= Time.deltaTime;
        
    }
    public void Timing()
    {
        sliderPoint.SetActive(false);
        
        panelDerrota.SetActive(true);
    }
    public void Restart()
    {
        timeStandard = 300;
        currentTime = 300;
        Debug.Log("Cañita" + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);
    }
}
