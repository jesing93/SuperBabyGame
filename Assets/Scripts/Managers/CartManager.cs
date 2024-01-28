using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    List<GameObject> items = new ();
    public void CatchContent()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, transform.rotation);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Product"))
            {
                items.Add(collider.gameObject);
                collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                collider.transform.SetParent(transform);
            }
        }
    }

    public void FreeContent()
    {
        foreach(GameObject item in items)
        {
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().isKinematic = false;
        }
        items.Clear();
    }
}
