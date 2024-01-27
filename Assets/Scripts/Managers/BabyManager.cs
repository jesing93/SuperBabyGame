using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BabyManager : MonoBehaviour
{
    [SerializeField] private int happinessDecreaseRatio;
    [SerializeField] private int toiletNeedIcreaseRatio;
    [SerializeField] private int IncreasedHappinessDecreaseRatio;
    [SerializeField] private int throwItemsChance;
    [SerializeField] private int entertainHappinessIncrease;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject npcPhrase;
    [SerializeField] private GameObject firstOption;
    [SerializeField] private GameObject secondOption;
    [SerializeField] private GameObject thirdOption;
    [SerializeField] private GameObject player;

    private StateMachine brain;
    private Animator animator;
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
        animator= GetComponent<Animator>();
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
            brain.PushState(OnVeryHappy, OnVeryHappyEnter, OnVeryHappyExit);
        }
        if (Happiness > 60 && Happiness < 80)
        {
            brain.PushState(OnHappy, OnHappyEnter, OnHappyExit);
        }
        if (Happiness > 40 && Happiness < 60)
        {
            brain.PushState(OnDefault,null, null);
        }
        if (Happiness > 20 && Happiness <= 40 )
        {
            brain.PushState(OnSad, OnSadEnter, OnSadExit);
        }
        if (Happiness <= 20)
        {
            brain.PushState(OnAngry, OnAngryEnter, OnAngryExit);
        }
        if (toiletNeed == 100)
        {
            brain.PushState(OnCry, OnCryEnter, OnCryExit);
        }
    }

    private void OnAngryExit()
    {
        animator.SetBool("IsAngry", false);
    }

    private void OnAngryEnter()
    {
        animator.SetBool("IsAngry", true);
        PopUpManager.instance.CreatePopUp("", Color.green, "El bebé esta cabreado.");
    }

    private void OnAngry()
    {
        //TODO sonido furioso
    }

    private void OnDefault()
    {
        //TODO sonido default
    }

    private void OnSadExit()
    {
        animator.SetBool("IsBored", false);
    }
    private void OnSad()
    {
        if (Random.Range(0, 100) <= throwItemsChance && canThrowItem)
        {
            ThrowItem();
        }
        //TODO sonido triste 
    }

    private void OnSadEnter()
    {
        animator.SetBool("IsBored", true);
        PopUpManager.instance.CreatePopUp("", Color.green, "El bebé esta aburrido.");
    }

    private void OnCryEnter()
    {
        animator.SetBool("IsCrying", true);
        PopUpManager.instance.CreatePopUp("",Color.green,"El bebé se ha hecho caca.");
    }
    private void OnCryExit()
    {
        animator.SetBool("IsCrying", false);
    }
    private void OnCry()
    {
        happinessDecreaseRatio = IncreasedHappinessDecreaseRatio;
        //TODO  sonido llorar
    }
    private void OnVeryHappyExit()
    {
        animator.SetBool("IsVeryHappy",false);
    }
    private void OnVeryHappyEnter()
    {
        animator.SetBool("IsVeryHappy", true);
    }

    private void OnVeryHappy()
    {
        //TODO sonido muy feliz
    }
    private void OnHappyExit()
    {
        animator.SetBool("IsHappy", false);
    }

    private void OnHappyEnter()
    {
        animator.SetBool("IsHappy", true);
    }

    private void OnHappy()
    {
       //TODO sonido feliz
    }

    public void Entertain()
    {
        //TODO Animacion de entretener al bebe
        happiness += entertainHappinessIncrease;
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
        PopUpManager.instance.CreatePopUp("", Color.green, "El bebé ha tirado un objeto.");
        Invoke("AvailThrowItem", 20f);
    }
    void AvailThrowItem()
    {
        canThrowItem = true;
    }

    void WantItem()
    {
        //TODO incluir item deseado en la lista,abrir cuadro de dialogo informando que el bebe quiere el item, Resetear ratio de felizidad cuando consigas el item
        PopUpManager.instance.CreatePopUp("", Color.green, "El bebé quiere: ");
        happinessDecreaseRatio = IncreasedHappinessDecreaseRatio;
    }
    public void CollectedBabyItem()
    {
        happiness += entertainHappinessIncrease;
        happinessDecreaseRatio = normalHappinessDecreaseRatio;
    }
    public void OpenBabyOptions()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().IsOnDialog = true;
        dialogPanel.SetActive(true);
        npcPhrase.GetComponent<TMP_Text>().text = "¿Que quieres hacer?";
        firstOption.GetComponentInChildren<TMP_Text>().text = "1) Jugar.";
        secondOption.GetComponentInChildren<TMP_Text>().text = "2) Cambiar pañal.";
        thirdOption.GetComponentInChildren<TMP_Text>().text = "3) Marcharte.";
        firstOption.GetComponent<Button>().onClick.AddListener(Entertain);
        secondOption.GetComponent<Button>().onClick.AddListener(CleanBaby);
        thirdOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
    }
    void CloseDialog()
    {
        dialogPanel.SetActive(false);
        player.GetComponent<PlayerController>().IsOnDialog = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
