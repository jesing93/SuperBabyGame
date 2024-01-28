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
    [SerializeField] private GameObject fadeBlack;
    [SerializeField] private GameObject grabPoint;
    [SerializeField] private GameObject tearParticles;
    [SerializeField] private GameObject poopParticles;
    [SerializeField] private GameObject angerParticles;
    [SerializeField] private List<ProductPreset> desiredObjectList;

    public static BabyManager Instance;
    private GameObject objectThrown;
    private StateMachine brain;
    private Animator animator;
    private float happiness;
    private float toiletNeed;
    private int normalHappinessDecreaseRatio;
    private bool canThrowItem = true;
    private int wantItemsTiming;
    private float timer;
    private bool hasRequestedItem;
    private ProductPreset desiredObject;
    private bool canTalk = true;
    private float angryTimer;
    public float Happiness { get => happiness; set => happiness = value; }
    public int NormalHappinessDecreaseRatio { get => normalHappinessDecreaseRatio; set => normalHappinessDecreaseRatio = value; }
    public bool CanTalk { get => canTalk; set => canTalk = value; }
    public float AngryTimer { get => angryTimer; set => angryTimer = value; }
    public ProductPreset DesiredObject { get => desiredObject; set => desiredObject = value; }

    private void Awake()
    {
        DesiredObject = null;
        Instance = this;
        Happiness = 50f;
        toiletNeed = Random.Range(0, 50);
        brain = GetComponent<StateMachine>();
        NormalHappinessDecreaseRatio = happinessDecreaseRatio;
        wantItemsTiming= Random.Range(45,120);
        animator= GetComponent<Animator>();
        brain.PushState(OnDefault, null, null);
    }

    private void Update()
    {
        if (happiness >0)
        {
            ReduceHappiness();
        }
        if (toiletNeed < 100)
        {
            IncreaseToiletNeed();
        }
        if (happiness < 20 || toiletNeed>=100)
        {
            AngryTimer += Time.deltaTime;
        }
        else
        {
            AngryTimer = 0;
        }
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
        Happiness -= Time.deltaTime / happinessDecreaseRatio;
    }

    private void SelectState()
    {
        if (Happiness >= 80)
        {
            brain.PushState(OnVeryHappy, OnVeryHappyEnter, OnVeryHappyExit);
        }else
        if (Happiness > 60 && Happiness < 80)
        {
            brain.PushState(OnHappy, OnHappyEnter, OnHappyExit);
        }else
        if (Happiness > 40 && Happiness < 60)
        {
            brain.PushState(OnDefault,null, null);
        }
        else
        if (Happiness > 20 && Happiness <= 40 )
        {
            brain.PushState(OnSad, OnSadEnter, OnSadExit);
        }
        else
        if (Happiness <= 20)
        {
            brain.PushState(OnAngry, OnAngryEnter, OnAngryExit);
        }
        if (toiletNeed >= 100)
        {
            brain.PushState(OnCry, OnCryEnter, OnCryExit);
        }
    }

    private void OnAngryExit()
    {
        animator.SetBool("IsAngry", false);
        angerParticles.SetActive(false);
    }

    private void OnAngryEnter()
    {
        animator.SetBool("IsAngry", true);
        angerParticles.SetActive(true);
        PopUpManager.instance.CreatePopUp("", Color.green, "El bebé esta cabreado.");
    }

    private void OnAngry()
    {
        if ((int)timer % 15 == 0)
        {
            if (Random.Range(0, 100) <= throwItemsChance && canThrowItem)
            {
                ThrowItem();
            }
        }
        //TODO sonido furioso
        if (Happiness > 20)
        {
            SelectState();
        }
    }

    private void OnDefault()
    {
        //TODO sonido default
        if (Happiness < 40 || Happiness > 60)
        {
            Debug.Log("default");
            SelectState();
        }
    }

    private void OnSadExit()
    {
        animator.SetBool("IsBored", false);
    }
    private void OnSad()
    {
        if ((int)timer % 15 == 0)
        {
            if (Random.Range(0, 100) <= throwItemsChance && canThrowItem)
            {
                ThrowItem();
            }
        }
        if (Happiness < 20 || Happiness > 40)
        {
            Debug.Log("sad");
            SelectState();
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
        tearParticles.SetActive(true);
        poopParticles.SetActive(true);
        PopUpManager.instance.CreatePopUp("",Color.green,"El bebé se ha hecho caca.");
    }
    private void OnCryExit()
    {
        tearParticles.SetActive(false);
        poopParticles.SetActive(false);
        animator.SetBool("IsCrying", false);
    }
    private void OnCry()
    {
        if (toiletNeed < 100)
            SelectState();
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
        if (Happiness < 80)
            SelectState();
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
        if (Happiness < 60 || Happiness > 80)
            SelectState();
    }

    public void Entertain()
    {
        animator.SetBool("IsBeeingEntertained", true);
        if (happiness + entertainHappinessIncrease >= 100)
        {
            happiness = 100;
        }
        else
        {
            happiness += entertainHappinessIncrease;
        }
        
        firstOption.GetComponent<Button>().onClick.RemoveAllListeners();
        secondOption.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdOption.GetComponent<Button>().onClick.RemoveAllListeners();
        firstOption.GetComponentInChildren<TMP_Text>().text = "";
        secondOption.GetComponentInChildren<TMP_Text>().text = "";
        thirdOption.GetComponentInChildren<TMP_Text>().text = "";
        firstOption.SetActive(false);
        secondOption.SetActive(false);
        thirdOption.SetActive(false);
    }

    public void CleanBaby()
    {
        firstOption.GetComponent<Button>().onClick.RemoveAllListeners();
        secondOption.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdOption.GetComponent<Button>().onClick.RemoveAllListeners();
        firstOption.GetComponentInChildren<TMP_Text>().text = "";
        secondOption.GetComponentInChildren<TMP_Text>().text = "";
        thirdOption.GetComponentInChildren<TMP_Text>().text = "";
        secondOption.SetActive(false);
        thirdOption.SetActive(false);
        //TODO sonido cambiar pañal 
        if (toiletNeed < 100)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "El bebe está limpio";
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        }
        else
        {
            StartCoroutine("FadeBlack");
            toiletNeed = 0;
            happinessDecreaseRatio = normalHappinessDecreaseRatio;
        }
        
    }
    private void ThrowItem()
    {
         objectThrown = Inventory.Instance.RemoveItemFromInventory();
        if (objectThrown != null)
        {
            animator.SetBool("IsThrowingObject", true);
            PopUpManager.instance.CreatePopUp("", Color.green, "El bebé ha tirado un objeto.");
        }
        else
        {
            PopUpManager.instance.CreatePopUp("", Color.green, "El bebé ha intentado tirar un objeto pero no ha podido.");
            happiness -= 20;
        }
        canThrowItem = false;
        Invoke("AvailThrowItem", 20f);
    }
    public void GrabItem()
    {
        objectThrown.transform.SetParent(grabPoint.transform);
        objectThrown.transform.localPosition = Vector3.zero;
    }
    public void ReleaseItem()
    {
        objectThrown.transform.parent = null;
        objectThrown.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 300 );
    }
    public void EndedThrowingObject()
    {
        animator.SetBool("IsThrowingObject", false);
    }
    void AvailThrowItem()
    {
        canThrowItem = true;
    }

    void WantItem()
    {
        DesiredObject = desiredObjectList[Random.Range(0, desiredObjectList.Count - 1)];
        PopUpManager.instance.CreatePopUp("", Color.green, "El bebé quiere: " + DesiredObject.name);
        Invoke("UncollectedBabyItem", 30f);
    }
    public void UncollectedBabyItem()
    {
        if (DesiredObject != null) {
            if (happiness - 20 <= 0)
            {
                happiness = 0;
            }
            else
            {
                happiness -= 20;
            }
            happinessDecreaseRatio = IncreasedHappinessDecreaseRatio;
            DesiredObject = null;
        }
    }
    public void CollectedBabyItem()
    {
        if (DesiredObject != null)
        {
            happiness += entertainHappinessIncrease;
            happinessDecreaseRatio = normalHappinessDecreaseRatio;
            DesiredObject = null;
        }
    }
    public void OpenBabyOptions()
    {
        
        CanTalk = false;
        firstOption.SetActive(true);
        secondOption.SetActive(true);
        thirdOption.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().IsOnDialog = true;
        dialogPanel.SetActive(true);
        npcPhrase.GetComponent<TMP_Text>().text = "¿Que quieres hacer?";
        firstOption.GetComponentInChildren<TMP_Text>().text = "1) Entretener.";
        secondOption.GetComponentInChildren<TMP_Text>().text = "2) Cambiar pañal.";
        thirdOption.GetComponentInChildren<TMP_Text>().text = "3) Marcharte.";
        firstOption.GetComponent<Button>().onClick.AddListener(Entertain);
        secondOption.GetComponent<Button>().onClick.AddListener(CleanBaby);
        thirdOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        animator.SetBool("IsBeeingEntertained", false);
    }
    void CloseDialog()
    {
        dialogPanel.SetActive(false);
        player.GetComponent<PlayerController>().IsOnDialog = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Invoke("ActivateCanTalk", 2f);
    }
    void ActivateCanTalk()
    {
        CanTalk = true;
    }
    private IEnumerator FadeBlack()
    {
        fadeBlack.SetActive(true);
        for (float i = 0; i <= 1; i += 0.05f)
        {
            fadeBlack.GetComponent<Image>().color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.05f);
        }
        fadeBlack.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(1.5f);
        for (float i = 1; i > 0; i -= 0.05f)
        {
            fadeBlack.GetComponent<Image>().color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.05f);
        }
        npcPhrase.GetComponent<TMP_Text>().text = "El bebe está limpio";
        firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
        firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        fadeBlack.SetActive(false);
    }
}
