using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<ProductPreset, ShoppingItem> ShoppingList;
    public static Inventory Instance;
    private int itemsNeeded = 0;
    private int currentItems = 0;
    public List<ProductPreset> itemPresets;
    private int shoppingListLength = 10;

    private void Awake()
    {
        Instance = this;
        ShoppingList = new();
        List<ProductPreset> shuffledList = itemPresets.OrderBy(x => Random.value).ToList().ConvertAll(input => input as ProductPreset);
        for (int i = 0; i < shoppingListLength; i++)
        {
            AddShoppingItem(shuffledList[i]);
        }
    }

    /// <summary>
    /// Add item to the shopping list
    /// </summary>
    /// <param name="item"></param>
    /// <param name="quantity"></param>
    public void AddShoppingItem(ProductPreset item, int quantity = 1)
    {
        if(ShoppingList.ContainsKey(item))
        {
            ShoppingItem currentItem = ShoppingList[item];
            currentItem.Needed += quantity;
            ShoppingList[item] = currentItem;
        }
        else
        {
            ShoppingList.Add(item, new ShoppingItem(0, quantity));
        }
        itemsNeeded++;
    }

    /// <summary>
    /// Add item to the inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(GameObject itemObject)
    {
        ProductPreset item = itemObject.GetComponent<ProductPreset>();
        if (ShoppingList.ContainsKey(item))
        {
            ShoppingItem currentItem = ShoppingList[item];
            currentItem.Current ++;
            currentItem.Items.Add(itemObject);
            ShoppingList[item] = currentItem;
            Debug.Log("Ahora tienes " + currentItem.Current + " de " + currentItem.Needed + " " + item.productName);
            if(currentItem.Current <= currentItem.Needed && currentItem.Needed > 0)
            {
                currentItems++;
                if(currentItems == itemsNeeded)
                {
                    Debug.Log("Win");
                    //TODO: Call Gamemanager.Win
                }
            }
        }
        else
        {
            ShoppingItem newItem = new ShoppingItem(1, 0);
            newItem.Items.Add(itemObject);
            ShoppingList.Add(item, newItem);
            Debug.Log("Ahora tienes 1 de 0" + item.productName);
        }
    }

    /// <summary>
    /// Remove an item from the inventory, if none specified the baby throws a random item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public GameObject RemoveItemFromInventory(ProductPreset item = null, GameObject itemObject = null)
    {
        //If remove a random item
        if(item == null)
        {
            List<KeyValuePair<ProductPreset, ShoppingItem>> randomList = new();
            foreach(var listItem in ShoppingList)
            {
                if(listItem.Value.Items.Count > 0)
                {
                    randomList.Add(listItem);
                }
            }
            if(randomList.Count > 0)
            {
                KeyValuePair<ProductPreset, ShoppingItem> randomItem = randomList.ElementAt(Random.Range(0, randomList.Count + 1));
                if (randomItem.Key.isHeavy)
                {
                    //Bebé se enfada porque pesa
                    return null;
                }
                else
                {
                    //Bebé lanza el objeto
                    ShoppingItem tempItem = randomItem.Value;
                    tempItem.Current--;
                    if(tempItem.Current < tempItem.Needed)
                    {
                        currentItems--;
                    }
                    int outItemId = Random.Range(0, tempItem.Items.Count);
                    GameObject outItem = tempItem.Items[outItemId];
                    tempItem.Items.RemoveAt(outItemId);
                    ShoppingList[randomItem.Key] = tempItem;
                    return outItem;
                }
            }
            else { 
                //No items to return
                return null; 
            }
        }
        else //Remove a specific item
        {
            if (ShoppingList.ContainsKey(item))
            {
                ShoppingItem currentItem = ShoppingList[item];
                currentItem.Current--;
                if(currentItem.Current < currentItem.Needed)
                {
                    currentItems--;
                }
                int outItemId = Random.Range(0, currentItem.Items.Count);
                GameObject outItem = currentItem.Items[outItemId];
                currentItem.Items.RemoveAt(outItemId);
                ShoppingList[item] = currentItem;
                Debug.Log("Ahora tienes " + currentItem.Current + " de " + currentItem.Needed + " " + item.productName);
                return outItem;
            }
            else
            {
                Debug.Log("Item don't exist");
            }
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Product"))
        {
            AddItemToInventory(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Product"))
        {
            RemoveItemFromInventory(other.gameObject.GetComponent<Product>().preset, other.gameObject);
        }
    }
}

public struct ShoppingItem
{
    private int current;
    private int needed;
    private List<GameObject> items;

    public ShoppingItem(int newCurrent, int newNeeded) : this()
    {
        current = newCurrent;
        needed = newNeeded;
        items = new();
    }

    public int Current { get => current; set => current = value; }
    public int Needed { get => needed; set => needed = value; }
    public List<GameObject> Items { get => items; set => items = value; }
}
