using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingListManager : MonoBehaviour
{
    public static ShoppingListManager instance;

    [SerializeField] private GameObject productText;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddProduct(string texto)
    {
        GameObject newProduct = Instantiate(productText, this.transform);
        newProduct.GetComponent<TextMeshProUGUI>().text = texto;
    }
    public void UpdateProduct(int index, string texto)
    {
        transform.GetChild(index).GetComponent<TextMeshProUGUI>().text = texto;
    }
}
