using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;

    public GameObject popUpPrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) { CreatePopUp("vieja", Color.red, "ayyy que mono el niñooo");}
    }

    public void CreatePopUp(string personaje, Color color, string comentario)
    {
        GameObject newPopUp = Instantiate(popUpPrefab, this.transform);
        newPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = personaje;
        newPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
        newPopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = comentario;
    }
}
