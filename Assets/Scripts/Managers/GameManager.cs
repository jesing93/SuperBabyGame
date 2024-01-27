using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float TimeToLive = 60f;
    public float TimeWhenDestroy;

    void Start()
    {
        TimeWhenDestroy = Time.time + TimeToLive;
    }

    void Update()
    {
        if (Time.time >= TimeWhenDestroy)
        {
            Destroy(gameObject);
        }
    }
}
