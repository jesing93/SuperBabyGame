using UnityEngine;

[CreateAssetMenu(fileName = "ProductPreset", menuName = "New Product", order = 0)]
public class ProductPreset : ScriptableObject
{
    public ProductType type;
    public string productName;
    public bool isHeavy = false;
    //public GameObject model;
}

public enum ProductType
{
    Champu,
    Jamon,
    Chuleta
}
