using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<ProductPreset, ShoppingItem> ShoppingList;

    private void Awake()
    {
        ShoppingList = new();
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
    }

    /// <summary>
    /// Add item to the inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(ProductPreset item)
    {
        if (ShoppingList.ContainsKey(item))
        {
            ShoppingItem currentItem = ShoppingList[item];
            currentItem.Current ++;
            ShoppingList[item] = currentItem;
            Debug.Log("Ahora tienes " + currentItem.Current + " de " + currentItem.Needed + " " + item.productName);
        }
        else
        {
            ShoppingList.Add(item, new ShoppingItem(1, 0));
            Debug.Log("Ahora tienes 1 de 0" + item.productName);
        }
    }

    /// <summary>
    /// Remove an item from the inventory, if none specified the baby throws a random item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public ProductPreset RemoveItemFromInventory(ProductPreset item = null)
    {
        //If remove a random item
        if(item == null)
        {
            KeyValuePair<ProductPreset, ShoppingItem> randomItem = ShoppingList.ElementAt(Random.Range(0,ShoppingList.Count + 1));
            if(randomItem.Key.isHeavy)
            {
                //TODO: Bebé se enfada porque pesa
            }
            else {
                //TODO: Bebé lanza el objeto
                ShoppingItem tempItem = randomItem.Value;
                tempItem.Current--;
                ShoppingList[randomItem.Key] = tempItem;
            }
        }
        else //Remove a specific item
        {
            if (ShoppingList.ContainsKey(item))
            {
                ShoppingItem currentItem = ShoppingList[item];
                currentItem.Current--;
                ShoppingList[item] = currentItem;
                Debug.Log("Ahora tienes " + currentItem.Current + " de " + currentItem.Needed + " " + item.productName);
            }
            else
            {
                Debug.Log("Item don't exist");
            }
        }
        return item;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Product"))
        {
            AddItemToInventory(other.gameObject.GetComponent<Product>().preset);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Product"))
        {
            RemoveItemFromInventory(other.gameObject.GetComponent<Product>().preset);
        }
    }
}

public struct ShoppingItem
{
    private int current;
    private int needed;

    public ShoppingItem(int newCurrent, int newNeeded) : this()
    {
        current = newCurrent;
        needed = newNeeded;
    }

    public int Current { get => current; set => current = value; }
    public int Needed { get => needed; set => needed = value; }
}
