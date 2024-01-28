
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject npcPhrase;
    [SerializeField] private GameObject firstOption;
    [SerializeField] private GameObject secondOption;
    [SerializeField] private GameObject thirdOption;
    [SerializeField] private GameObject cryGuardSpot;

    private bool guardIsCrying;
    private NpcBehaviour npcBehaviour;
    private bool hasAskedForHelp = false;
    private bool canTalk=true;
    public bool GuardIsCrying { get => guardIsCrying; set => guardIsCrying = value; }
    public bool CanTalk { get => canTalk; set => canTalk = value; }

    private void Awake()
    {
        npcBehaviour=GetComponent<NpcBehaviour>();
    }
    public void OpenDialog()
    {
        canTalk = false;
        GetComponent<NavMeshAgent>().speed = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().IsOnDialog = true;
        dialogPanel.SetActive(true);
        if (npcBehaviour.NpcData.NpcType == NpcType.guard)
        {
            StartCoroutine(GuardDialog(0,0));
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.shopkeeper)
        {
            StartCoroutine(ShopkeeperDialog(0, 0));
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.elder)
        {
            StartCoroutine(ElderDialog(0, 0));
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.woman)
        {
            StartCoroutine(WomanDialog(0, 0));
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.man)
        {
            StartCoroutine(ManDialog(0, 0));
        }
    }

    private IEnumerator WomanDialog(int step, int option)
    {
        ResetDialogOptions();
        if (!npcBehaviour.IsStoppedInShelve)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "¡Aparta, aparta, aparta!"; 
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
        else
        {
            if (npcBehaviour.PlayerIsAggressive)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "¡Pero tio!";
            }
            else
            {
                if (step == 0)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Ahora mismo estoy yo mirando.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "No me interesa.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Pero...";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(WomanDialog(1, 1)); });
                    ActiveOptions();
                }
                if (step == 1)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Venga, vayase.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "No haber tenido hijos.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                    firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                    ActiveOptions();
                }
            }
        }
    }

    private IEnumerator ManDialog(int step, int option)
    {
        ResetDialogOptions();
        if (!npcBehaviour.IsStoppedInShelve)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "Esta noche toca cenar pizza";
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }

        else
        {
            if (npcBehaviour.PlayerIsAggressive)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "¡Marisa!";
            }
            else
            {
                if (step == 0)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Otra noche solo, y Marisa no me deja ver a los niños.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Que bien es no tener hijos.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Disculpe.";
                    secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ManDialog(1, 1)); });
                    secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                    ActiveOptions();
                }
                if (step == 1)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "No pienso moverme, puedo tomarme todo el tiempo del mundo.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Mi mujer es mala persona, no me deja ver a los niños.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Es que mi hijo...";
                    secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ManDialog(2, 1)); });
                    secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                    ActiveOptions();
                }
                if (step == 2)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Que se los hubiera llevado su madre. Los míos los tiene mi mujer y estoy muy feliz.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Menudo planchabragas.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Me gustaría que no nos faltase al respeto.";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ManDialog(3, 1)); });
                    ActiveOptions();
                }
                if (step == 3)
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "Marisa... ay Marisa.";
                }
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
            }
        }
    }

    private IEnumerator ElderDialog(int step, int option)
    {
        ResetDialogOptions();
        if (npcBehaviour.IsStoppedInShelve)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "¿A quien no le va a gustar un imperio Romano?";
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }

        else
        {
            if (npcBehaviour.PlayerIsAggressive)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Que maleducado, voy a sacar la chancla, oiga.";
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
            }
            else
            {
                if (step == 0)
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "Que niño más mono. A mi nieta le gusta eso.";
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Señora, necesito coger una cosa.";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ElderDialog(1, 1)); });
                    ActiveOptions();
                }
                if (step == 1)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "El que se va a Sevilla pierde su sitio, así que voy a tomar esto que le gusta a mi nieta.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Es que estoy antes, quiero coger esto que tanto me gusta.A mi nieta también le encanta, ay a ver si me ayuda a buscarlo que parece muy amable.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Disculpe pero...";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ElderDialog(2, 1)); });
                    ActiveOptions();
                }
                if (step == 2)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Perdonado, ahora ayudeme que se le ve muy fuerte.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "¿Sabe que mi nieta está en sexto de primaria? Sí, es una niña muy buena, antes iba a la universidad con otros niños pero se ha dejado el pelo largo, se ha cambiado el nombre y ahora enseña a los niños.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                    firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                    ActiveOptions();
                }
            }
        }
    }
    private IEnumerator ShopkeeperDialog(int step, int option)
    {
        ResetDialogOptions();
        
        if (step == 0)
        {
            if ((int)Random.Range(0, 2) == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "¿Necesita ayuda?";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Se ha quedado buena tarde.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) No se donde está este producto.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) No, todo bien.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ShopkeeperDialog(1, 1)); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
        if(step == 1)
        {
                npcPhrase.GetComponent<TMP_Text>().text = "Está aquí. Sígame.";
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
                npcBehaviour.GuideToObject = true;
                foreach (var listItem in Inventory.Instance.ShoppingList)
                {
                    if (listItem.Value.Items.Count == 0)
                    {
                        npcBehaviour.ObjectToGuide =GameObject.Find(listItem.Key.name);
                        break;
                    }
                }
        }

    }

    public IEnumerator GuardDialog(int step, int option)
    {
        ResetDialogOptions();
        if (GuardIsCrying)
        {
            if ((int)Random.Range(0, 2) == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Tendría que haber sido bombero.";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Nadie me respeta.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            npcBehaviour.GuardTalkedToPlayer = false;
            ActiveOptions();
        }
        else if (BabyManager.Instance.AngryTimer>=15f)
        {
            if (PlayerController.Instance.AgrresionCount >= 3)
            {
                PlayerController.Instance.AgrresionCount = 0;
            }
            if (step == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Señor, su hijo parece no estar contento.";
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Perdón pero tengo prisa.";
                secondOption.GetComponentInChildren<TMP_Text>().text = "2) Si, perdón.";
                firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(GuardDialog(1, 1)); });
                secondOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(GuardDialog(1, 2)); });
                //npcBehaviour.GuardTalkedToPlayer = false;
                ActiveOptions();
            }
            if (step == 1)
            {
                if (option == 1) {
                    npcPhrase.GetComponent<TMP_Text>().text = "Me da igual, o lo cuidas o tendré que actuar.";
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Vale, no quiero problemas...";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(GuardDialog(2, 1)); });
                    //npcBehaviour.GuardTalkedToPlayer = false;
                    ActiveOptions();
                }
                else
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "Muy bien, pero que no vuelva a ocurrir";
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                    firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                    //npcBehaviour.GuardTalkedToPlayer = false;
                    ActiveOptions();
                }
            }
            if (step == 2)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Muy bien, que no vuelva a ocurrir";
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                //npcBehaviour.GuardTalkedToPlayer = false;
                ActiveOptions();
            }
            }
        else
        {
            if(PlayerController.Instance.AgrresionCount >= npcBehaviour.MaxAggressionTimes)
            {
                if (step == 0)
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "¡Eh, deja de molestar a la clientela que te arresto! ¡Que soy agente!";
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Haber estudiado entonces";
                    secondOption.GetComponentInChildren<TMP_Text>().text = "2) Señor y va listo.";
                    thirdOption.GetComponentInChildren<TMP_Text>().text = "3) Perdón, es que soy padre.";
                    firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(GuardDialog(1, 1)); });
                    secondOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(GuardDialog(1, 2)); });
                    thirdOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(GuardDialog(1, 3)); });
                    ActiveOptions();
                }
                if (step == 1)
                {
                    if (option == 1 || option == 2)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "No hace falta ser tan agresivo, jo.";
                        gameObject.GetComponent<NpcBehaviour>().NextPoint = cryGuardSpot.transform.position;
                        GuardIsCrying = true;
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Yo también y no voy tirando atunes a quien me cae mal, caballero.";
                    }
                    yield return new WaitForSeconds(2f);
                    firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                    firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                    ActiveOptions();
                    PlayerController.Instance.AgrresionCount = 0;
                    npcBehaviour.GuardTalkedToPlayer = false;
                }
            }else{
                if ((int)Random.Range(0, 2) == 0) {
                    npcPhrase.GetComponent<TMP_Text>().text = "Digame, que estoy vigilando";
                }
                else {
                    npcPhrase.GetComponent<TMP_Text>().text = "A ver si puedo irme ya, que cansancio de no hacer nada.";
                }
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
            }
        }
    }
    void CloseDialog()
    {
        dialogPanel.SetActive(false);
        player.GetComponent<PlayerController>().IsOnDialog=false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<NavMeshAgent>().speed = npcBehaviour.NpcData.Speed;
        Invoke("ActivateCanTalk", 2f);
    }

    void ActivateCanTalk()
    {
        canTalk = true;
    }
    void ResetDialogOptions()
    {
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

    void ActiveOptions()
    {
        if(firstOption.GetComponentInChildren<TMP_Text>().text != "")
        {
            firstOption.SetActive(true);
        }
        if (secondOption.GetComponentInChildren<TMP_Text>().text != "")
        {
            secondOption.SetActive(true);
        }
        if (thirdOption.GetComponentInChildren<TMP_Text>().text != "")
        {
            thirdOption.SetActive(true);
        }
    }
}
