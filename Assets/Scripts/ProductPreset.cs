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
    BoteChampu,
    PataJamon,
    Chuleta,
    Almeja,
    BoteCosmetica,
    BoteLimpieza,
    BotellaRefresco,
    BotellaRefrescoGrande,
    BrickLeche,
    BrickZumo,
    CepilloDientes,
    CongeladoCebolla,
    Escoba,
    Estropajo,
    FrascoCosmetica,
    Fregona,
    GarrafaAgua,
    Hamburguesa,
    LataRefresco,
    Manzana,
    Melon,
    MusloPollo,
    CongeladoMerluza,
    PapelHigienico,
    CongeladoNuggets,
    CongeladoPimientos,
    PastillaLavavajillas,
    PataPulpo,
    Pera,
    PezRape,
    PezEspada,
    Pizza,
    Platano,
    Puerro,
    RistraChorizos,
    Sardina,
    Tomate,
    Uvas
}
