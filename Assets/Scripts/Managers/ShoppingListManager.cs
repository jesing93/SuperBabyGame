using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingListManager : MonoBehaviour
{
    public static ShoppingListManager instance;
    public GameObject listUiObject;

    [SerializeField] private GameObject productText;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void AddProduct(string texto)
    {
        GameObject newProduct = Instantiate(productText, listUiObject.transform);
        newProduct.GetComponent<TextMeshProUGUI>().text = texto;
    }
    public void UpdateProduct(int index, string texto)
    {
        listUiObject.transform.GetChild(index).GetComponent<TextMeshProUGUI>().text = texto;
    }
}
