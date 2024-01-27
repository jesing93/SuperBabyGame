using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BabyManager : MonoBehaviour
{
    [SerializeField] private int happinessDecreaseRatio;
    [SerializeField] private int toiletNeedIcreaseRatio;
    [SerializeField] private int IncreasedHappinessDecreaseRatio;
    [SerializeField] private int throwItemsChance;
    [SerializeField] private int entertainHappinessIncrease;
    private StateMachine brain;
    private float happiness;
    private float toiletNeed;
    private int NormalHappinessDecreaseRatio;
    private bool canThrowItem = true;

    private void Awake()
    {
        happiness = 50f;
        toiletNeed = Random.Range(0, 50);
        brain = GetComponent<StateMachine>();
        NormalHappinessDecreaseRatio = happinessDecreaseRatio;
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
        if (happiness >= 80)
        {
            brain.PushState(VeryHappy, OnVeryHappyEnter, null);
        }
        if (happiness > 40 && happiness < 70)
        {
            brain.PushState(Happy, OnHappyEnter, null);
        }
        if (happiness > 20 && happiness <= 40 )
        {
            brain.PushState(Sad, OnSadEnter, null);
        }
        if (happiness <= 20)
        {
            brain.PushState(Cry, OnCryEnter, null);
        }
        if (toiletNeed == 100)
        {
            brain.PushState(Shit, OnShitEnter, null);
        }
    }


    private void Sad()
    {
        //TODO Animacion y tirar cosas del carro
    }

    private void OnSadEnter()
    {
        //TODO Reset Animacion
    }
    private void OnShitEnter()
    {
        //TODO Reset Animacion
        happinessDecreaseRatio = IncreasedHappinessDecreaseRatio;
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
        if(Random.Range(0,100) <= throwItemsChance && canThrowItem)
        {
            ThrowItem();
        }
    }

    private void OnVeryHappyEnter()
    {
        //TODO Reset Animacion
    }

    private void VeryHappy()
    {
        //TODO Animacion y sonido muy feliz
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
        happiness += entertainHappinessIncrease;
    }

    public void CleanBaby()
    {
        toiletNeed = 0;
        happinessDecreaseRatio = NormalHappinessDecreaseRatio;
    }
    private void ThrowItem()
    {
        canThrowItem = false;
        Invoke("AvailThrowItem", 20f);
        throw new System.NotImplementedException();
    }
    void AvailThrowItem()
    {
        canThrowItem = true;
    }
}
