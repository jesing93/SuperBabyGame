
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

    public bool GuardIsCrying { get => guardIsCrying; set => guardIsCrying = value; }

    private void Awake()
    {
    }
    public void OpenDialog()
    {
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

        //if(player.isAggresive){
        //npcPhrase.GetComponent<TMP_Text>().text = "�Pero tio!";
        //}else{
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
                secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(WomanDialog(1, 1)); });
                secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
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

    private IEnumerator ManDialog(int step, int option)
    {
        ResetDialogOptions();
        //if(player.isAggresive){
        //npcPhrase.GetComponent<TMP_Text>().text = "�Marisa!";
        //}else{
        if (step == 0)
        {
            if ((int)Random.Range(0, 2) == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Otra noche solo, y Marisa no me deja ver a los ni�os.";
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
                npcPhrase.GetComponent<TMP_Text>().text = "Mi mujer es mala persona, no me deja ver a los ni�os.";
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
                npcPhrase.GetComponent<TMP_Text>().text = "Que se los hubiera llevado su madre. Los m�os los tiene mi mujer y estoy muy feliz.";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Menudo planchabragas.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Me gustar�a que no nos faltase al respeto.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ManDialog(3, 1)); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
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

    private IEnumerator ElderDialog(int step, int option)
    {
        ResetDialogOptions();
        //if(player.isAggresive){
        //npcPhrase.GetComponent<TMP_Text>().text = "Que maleducado, voy a sacar la chancla, oiga.";
        //}else{
        if (step == 0)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "Que ni�o m�s mono. A mi nieta le gusta eso.";
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Se�ora, necesito coger una cosa.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ElderDialog(1, 1)); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
        if (step == 1)
        {
            if ((int)Random.Range(0, 2) == 0) { 
                npcPhrase.GetComponent<TMP_Text>().text = "El que se va a Sevilla pierde su sitio, as� que voy a tomar esto que le gusta a mi nieta.";
            }
            else 
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Es que estoy antes, quiero coger esto que tanto me gusta.A mi nieta tambi�n le encanta, ay a ver si me ayuda a buscarlo que parece muy amable.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Disculpe pero...";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ElderDialog(2, 1)); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
        if(step==2) {
            if ((int)Random.Range(0, 2) == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Perdonado, ahora ayudeme que se le ve muy fuerte.";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "�Sabe que mi nieta est� en sexto de primaria? S�, es una ni�a muy buena, antes iba a la universidad con otros ni�os pero se ha dejado el pelo largo, se ha cambiado el nombre y ahora ense�a a los ni�os.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
           

        //}
    }
    private IEnumerator ShopkeeperDialog(int step, int option)
    {
        ResetDialogOptions();
        //if(player.isAggresive){
        if (step == 0)
        {
            if ((int)Random.Range(0, 2) == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "�Necesita ayuda?";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Se ha quedado buena tarde.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) No encuentro a mi hijo.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) No se donde est� este producto.";
            thirdOption.GetComponentInChildren<TMP_Text>().text = "3) No, todo bien.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ShopkeeperDialog(1, 1)); });
            secondOption.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ShopkeeperDialog(1, 2)); });
            thirdOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
        if(step == 1)
        {
            if(option == 1)
            {
                if (hasAskedForHelp)
                {
                    if ((int)Random.Range(0, 2) == 0)
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "�Otra vez? Vamos a buscarle.";
                    }
                    else
                    {
                        npcPhrase.GetComponent<TMP_Text>().text = "Ains, mucho estr�s �Eh?";
                    }
                }
                else
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "�Vamos a buscarle!";
                }
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
                // TODO Guiar hasta el carro
            }
            else if(option == 2)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Est� aqu�. S�game.";
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
                // TODO Guiar hasta siguiente objeto de la lista
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
                npcPhrase.GetComponent<TMP_Text>().text = "Tendr�a que haber sido bombero.";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Nadie me respeta.";
            }
            yield return new WaitForSeconds(2f);
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
            ActiveOptions();
        }
        else
        {
            //if(player.isAggresive){
            if (step == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "�Eh, deja de molestar a la clientela que te arresto! �Que soy agente!";
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Haber estudiado entonces";
                secondOption.GetComponentInChildren<TMP_Text>().text = "2) Se�or y va listo.";
                thirdOption.GetComponentInChildren<TMP_Text>().text = "3) Perd�n, es que soy padre.";
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
                    npcPhrase.GetComponent<TMP_Text>().text = "Yo tambi�n y no voy tirando atunes a quien me cae mal, caballero.";
                }
                yield return new WaitForSeconds(2f);
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
                ActiveOptions();
            }
            /*}else{
             *    if((int)Random.Range(0,2)==0)
             *      npcPhrase.GetComponent<TMP_Text>().text ="Digame, que estoy vigilando";
             *     else
             *      npcPhrase.GetComponent<TMP_Text>().text ="A ver si puedo irme ya, que cansancio de no hacer nada.";
             * 
             }*/
        }
    }
    void CloseDialog()
    {
        dialogPanel.SetActive(false);
        GetComponent<NavMeshAgent>().speed = npcBehaviour.NpcData.Speed;
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
