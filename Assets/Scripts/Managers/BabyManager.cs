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
    private int normalHappinessDecreaseRatio;
    private bool canThrowItem = true;
    private int wantItemsTiming;
    private float timer;
    private bool hasRequestedItem;
    public float Happiness { get => happiness; set => happiness = value; }
    public int NormalHappinessDecreaseRatio { get => normalHappinessDecreaseRatio; set => normalHappinessDecreaseRatio = value; }

    private void Awake()
    {
        Happiness = 50f;
        toiletNeed = Random.Range(0, 50);
        brain = GetComponent<StateMachine>();
        NormalHappinessDecreaseRatio = happinessDecreaseRatio;
        wantItemsTiming= Random.Range(45,150);
    }

    private void Update()
    {
        ReduceHappiness();
        IncreaseToiletNeed();
        SelectState();
        timer += Time.deltaTime;
        if (wantItemsTiming <= timer && !hasRequestedItem)
        {
            hasRequestedItem = true;
            WantItem();
        }
    }

    private void IncreaseToiletNeed()
    {
        toiletNeed += Time.deltaTime/toiletNeedIcreaseRatio;
    }

    private void ReduceHappiness()
    {
        Happiness += Time.deltaTime / happinessDecreaseRatio;
    }

    private void SelectState()
    {
        if (Happiness >= 80)
        {
            brain.PushState(OnVeryHappy, OnVeryHappyEnter, null);
        }
        if (Happiness > 60 && Happiness < 80)
        {
            brain.PushState(OnHappy, OnHappyEnter, null);
        }
        if (Happiness > 40 && Happiness < 60)
        {
            brain.PushState(OnDefault, OnDefaultEnter, null);
        }
        if (Happiness > 20 && Happiness <= 40 )
        {
            brain.PushState(OnSad, OnSadEnter, null);
        }
        if (Happiness <= 20)
        {
            brain.PushState(OnCry, OnCryEnter, null);
        }
        if (toiletNeed == 100)
        {
            brain.PushState(OnShit, OnShitEnter, null);
        }
    }


    private void OnDefault()
    {
        //TODO Animacion y tirar cosas del carro
    }

    private void OnDefaultEnter()
    {
        //TODO Reset Animacion
    }
    private void OnSad()
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
    private void OnShit()
    {
        //TODO Animacion y sonido cagarse encima
    }

    private void OnCryEnter()
    {
        //TODO Reset Animacion

       
    }

    private void OnCry()
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

    private void OnVeryHappy()
    {
        //TODO Animacion y sonido muy feliz
    }

    private void OnHappyEnter()
    {
        //TODO Reset Animacion
    }

    private void OnHappy()
    {
       //TODO Animacion y sonido feliz
    }

    public void Entertain()
    {
        //TODO Animacion de entretener al bebe
        Happiness += entertainHappinessIncrease;
    }

    public void CleanBaby()
    {
        //TODO animacion limpiar bebe
        toiletNeed = 0;
        happinessDecreaseRatio = normalHappinessDecreaseRatio;
    }
    private void ThrowItem()
    {
        canThrowItem = false;
        Invoke("AvailThrowItem", 20f);
    }
    void AvailThrowItem()
    {
        canThrowItem = true;
    }

    void WantItem()
    {
        //TODO incluir item deseado en la lista,abrir cuadro de dialogo informando que el bebe quiere el item, Resetear ratio de felizidad cuando consigas el item
        Happiness += IncreasedHappinessDecreaseRatio;
    }
}
