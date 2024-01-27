using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyManager : MonoBehaviour
{
    [SerializeField] private int happinessDecreaseRatio;
    [SerializeField] private int toiletNeedIcreaseRatio;

    private StateMachine brain;
    private float happiness;
    private float toiletNeed;

    private void Awake()
    {
        happiness = 50f;
        toiletNeed = Random.Range(0, 50);
        brain = GetComponent<StateMachine>();
    }

    private void Update()
    {
        ReduceHappiness();
        IncreaseToiletNeed();
        SelectState();
    }

    private void IncreaseToiletNeed()
    {
        toiletNeed += Time.deltaTime/toiletNeedIcreaseRatio;
    }

    private void ReduceHappiness()
    {
        happiness += Time.deltaTime / happinessDecreaseRatio;
    }

    private void SelectState()
    {
        if (happiness >= 70)
        {
            brain.PushState(Happy, OnHappyEnter, null);
        }
        if (happiness > 40 && happiness < 70)
        {
            brain.PushState(Default, OnDefaultEnter, null);
        }
        if (happiness > 20 && happiness <= 40 )
        {
            brain.PushState(Cry, OnCryEnter, null);
        }
        if (happiness <= 20)
        {
            brain.PushState(Rage, OnRageEnter, null);
        }
        if (toiletNeed == 100)
        {
            brain.PushState(Shit, OnShitEnter, null);
        }
    }


    private void Rage()
    {
        //TODO Animacion y tirar cosas del carro
    }

    private void OnRageEnter()
    {
        //TODO Reset Animacion
    }
    private void OnShitEnter()
    {
        //TODO Reset Animacion
    }
    private void Shit()
    {
        //TODO Animacion y sonido cagarse encima
    }

    private void OnCryEnter()
    {
        //TODO Reset Animacion
    }

    private void Cry()
    {
        //TODO Animacion y sonido llorar
    }

    private void OnDefaultEnter()
    {
        //TODO Reset Animacion
    }

    private void Default()
    {
        //TODO Animacion y sonido default
    }

    private void OnHappyEnter()
    {
        //TODO Reset Animacion
    }

    private void Happy()
    {
       //TODO Animacion y sonido feliz
    }

    public void Entertain()
    {
        //TODO Animacion de entretener al bebe
        happiness += 50;
    }

    public void ClearBaby()
    {
        toiletNeed = 0;
    }
}
