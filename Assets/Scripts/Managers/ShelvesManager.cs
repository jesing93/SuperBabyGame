using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class ShelvesManager : MonoBehaviour
{
    [SerializeField] private bool isBusyPlayer = false;
    [SerializeField] private bool isBusyNpc = false;

    public bool IsBusyPlayer { get => isBusyPlayer; set => isBusyPlayer = value; }
    public bool IsBusyNpc { get => isBusyNpc; set => isBusyNpc = value; }

    public void ChangeAvailavility(bool availavility)
    {
        IsBusyNpc = availavility;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
            IsBusyPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsBusyPlayer = false;
    }
}
