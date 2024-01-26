using UnityEngine;

[CreateAssetMenu(fileName = "ProductPreset", menuName = "New Product", order = 0)]
public class ProductPreset : ScriptableObject
{
    public ProductType type;
    public bool isHeavy = false;
    public GameObject model;
}

public enum ProductType
{
    Jamon,
    Chuleta
}
