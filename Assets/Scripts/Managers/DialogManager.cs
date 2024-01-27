
using TMPro;
using TreeEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject npcPhrase;
    [SerializeField] private GameObject firstOption;
    [SerializeField] private GameObject secondOption;
    [SerializeField] private GameObject thirdOption;

    private NpcBehaviour npcBehaviour;

    private void Awake()
    {
        ElderDialog(0, 0);
    }
    void OpenDialog()
    {
        if (npcBehaviour.NpcData.NpcType == NpcType.guard)
        {
            GuardDialog(0,0);
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.shopkeeper)
        {
            ShopkeeperDialog(0, 0);
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.elder)
        {
            ElderDialog(0, 0);
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.woman)
        {
            WomanDialog(0, 0);
        }
        if (npcBehaviour.NpcData.NpcType == NpcType.man)
        {
           ManDialog(0, 0);
        }
    }

    private void WomanDialog(int step, int option)
    {
        firstOption.GetComponent<Button>().onClick.RemoveAllListeners();
        secondOption.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdOption.GetComponent<Button>().onClick.RemoveAllListeners();
        //if(player.isAggresive){
        //npcPhrase.GetComponent<TMP_Text>().text = "¡Pero tio!";
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
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Pero...";
                secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
                firstOption.GetComponent<Button>().onClick.AddListener(delegate { WomanDialog(1, 1); });
                secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
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
            }
    }

    private void ManDialog(int step, int option)
    {
        firstOption.GetComponent<Button>().onClick.RemoveAllListeners();
        secondOption.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdOption.GetComponent<Button>().onClick.RemoveAllListeners();
        //if(player.isAggresive){
        //npcPhrase.GetComponent<TMP_Text>().text = "¡Marisa!";
        //}else{
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
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Disculpe.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { ManDialog(1, 1); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
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
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Es que mi hijo...";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { ManDialog(2, 1); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
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
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Me gustaría que no nos faltase al respeto.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { ManDialog(3, 1); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        }
        if (step == 3)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "Marisa... ay Marisa.";
        }
    }

    private void ElderDialog(int step, int option)
    {
        firstOption.GetComponent<Button>().onClick.RemoveAllListeners();
        secondOption.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdOption.GetComponent<Button>().onClick.RemoveAllListeners();
        //if(player.isAggresive){
        //npcPhrase.GetComponent<TMP_Text>().text = "Que maleducado, voy a sacar la chancla, oiga.";
        //}else{
        if (step == 0)
        {
            npcPhrase.GetComponent<TMP_Text>().text = "Que niño más mono. A mi nieta le gusta eso.";
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Señora, necesito coger una cosa.";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { ElderDialog(1, 1); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        }
        if (step == 1)
        {
            if ((int)Random.Range(0, 2) == 0) { 
                npcPhrase.GetComponent<TMP_Text>().text = "El que se va a Sevilla pierde su sitio, así que voy a tomar esto que le gusta a mi nieta.";
            }
            else 
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Es que estoy antes, quiero coger esto que tanto me gusta.A mi nieta también le encanta, ay a ver si me ayuda a buscarlo que parece muy amable.";
            }
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Disculpe pero...";
            secondOption.GetComponentInChildren<TMP_Text>().text = "2) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(delegate { ElderDialog(2, 1); });
            secondOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        }
        if(step==2) {
            if ((int)Random.Range(0, 2) == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "Perdonado, ahora ayudeme que se le ve muy fuerte.";
            }
            else
            {
                npcPhrase.GetComponent<TMP_Text>().text = "¿Sabe que mi nieta está en sexto de primaria? Sí, es una niña muy buena, antes iba a la universidad con otros niños pero se ha dejado el pelo largo, se ha cambiado el nombre y ahora enseña a los niños.";
            }
            firstOption.GetComponentInChildren<TMP_Text>().text = "1) Marcharte.";
            firstOption.GetComponent<Button>().onClick.AddListener(CloseDialog);
        }
           

        //}
    }
    private void ShopkeeperDialog(int step, int option)
    {
       
    }

    public void GuardDialog(int step, int option)
    {
        firstOption.GetComponent<Button>().onClick.RemoveAllListeners();
        secondOption.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdOption.GetComponent<Button>().onClick.RemoveAllListeners();
        //if(player.isAggresive){
        if (step == 0)
            {
                npcPhrase.GetComponent<TMP_Text>().text = "¡Eh, deja de molestar a la clientela que te arresto! ¡Que soy agente!";
                firstOption.GetComponentInChildren<TMP_Text>().text = "1) Haber estudiado entonces";
                secondOption.GetComponentInChildren<TMP_Text>().text = "2) Señor y va listo.";
                thirdOption.GetComponentInChildren<TMP_Text>().text = "3) Perdón, es que soy padre.";
                firstOption.GetComponent<Button>().onClick.AddListener(delegate { GuardDialog(1, 1); });
                secondOption.GetComponent<Button>().onClick.AddListener(delegate { GuardDialog(1, 2); });
                thirdOption.GetComponent<Button>().onClick.AddListener(delegate { GuardDialog(1, 3); });
            }
            if(step == 1)
            {
                if( option==1 || option == 2)
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "No hace falta ser tan agresivo, jo.";
                    //TODO El guardia se va a una esquina y llora
                }
                else
                {
                    npcPhrase.GetComponent<TMP_Text>().text = "Yo también y no voy tirando atunes a quien me cae mal, caballero.";
                }
            }
        /*}else{
         *    if((int)Random.Range(0,2)==0)
         *      npcPhrase.GetComponent<TMP_Text>().text ="Digame, que estoy vigilando";
         *     else
         *      npcPhrase.GetComponent<TMP_Text>().text ="A ver si puedo irme ya, que cansancio de no hacer nada.";
         * 
         }*/
    }
    void CloseDialog()
    {
        dialogPanel.SetActive(false);
    }
}
