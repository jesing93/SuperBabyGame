using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NpcBehaviour : MonoBehaviour
{
    [SerializeField] private NpcData npcData;
    [SerializeField] private GameObject shelves;
    [SerializeField] private GameObject tearParticles;
    [SerializeField] private GameObject stunParticles;
    [SerializeField] private List<GameObject> shelvesStopPoints;
    [SerializeField] private float maxWalkingDistance;
    [SerializeField] private float stopInShelveRate;
    [SerializeField] private float findPlayerRate;
    [SerializeField] private float stopInTime;
    [SerializeField] private float followTime;
    [SerializeField] private GameObject player;
    [SerializeField] private int maxAggressionTimes;

    private GameObject objectToGuide;
    private float npcSpeed;
    private bool returnToDefaultPosition;
    private bool guideToObject;
    private bool guardTalkedToPlayer;
   
    private bool playerIsAggressive;
    private Vector3 nextPoint;
    private NavMeshAgent agent;
    private Vector3 nearestPoint;
    private bool isStoppedInShelve=false;
    private GameObject busyShelve;
    private bool followPlayer;
    private DialogManager dialogManager;
    private bool stopCry;
    private bool npcStunned;
    private float lastReprimend;

    public NpcData NpcData { get => npcData; set => npcData = value; }
    public Vector3 NextPoint { get => nextPoint; set => nextPoint = value; }
    public bool NpcStunned { get => npcStunned; set => npcStunned = value; }
    public bool PlayerIsAggressive { get => playerIsAggressive; set => playerIsAggressive = value; }
    public int MaxAggressionTimes { get => maxAggressionTimes; set => maxAggressionTimes = value; }
    public bool GuardTalkedToPlayer { get => guardTalkedToPlayer; set => guardTalkedToPlayer = value; }
    public bool IsStoppedInShelve { get => isStoppedInShelve; set => isStoppedInShelve = value; }
    public bool GuideToObject { get => guideToObject; set => guideToObject = value; }
    public GameObject ObjectToGuide { get => objectToGuide; set => objectToGuide = value; }

    private void Awake()
    {
        dialogManager = GetComponent<DialogManager>();
        NextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
        agent = gameObject.GetComponent<NavMeshAgent>();
        foreach(GameObject stopPoint in GameObject.FindGameObjectsWithTag("StopPoint"))
        {
            shelvesStopPoints.Add(stopPoint);
        }
        agent.speed = NpcData.Speed;
        lastReprimend = 60;
        npcSpeed = agent.speed;
    }
    private void Update()
    {   
        goToNextPoint();
        lastReprimend += Time.deltaTime;
    }

    private void goToNextPoint()
    {
        if (!npcStunned)
        {
            //GUARD LOGIC
            if (NpcData.NpcType.Equals(NpcType.guard))
            {
                if (!dialogManager.GuardIsCrying)
                {
                   
                    if (PlayerController.Instance.AgrresionCount>= MaxAggressionTimes || BabyManager.Instance.AngryTimer >= 15f && lastReprimend >= 60f)
                    {
                        NextPoint = player.transform.position;
                    }
                    
                    if (!agent.pathPending && agent.remainingDistance < 3.5f)
                    {
                        if (BabyManager.Instance.AngryTimer >= 15f && lastReprimend>=60f)
                        {
                            dialogManager.OpenDialog();
                            lastReprimend = 0;
                        }
                        else
                        if (PlayerController.Instance.AgrresionCount >= MaxAggressionTimes && !GuardTalkedToPlayer)
                        {
                            dialogManager.OpenDialog();
                            GuardTalkedToPlayer = true;
                        }
                        else
                        {
                            NextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                        }
                    }
                }
                else
                {
                    if (!stopCry)
                    {
                        tearParticles.SetActive(true);
                        stopCry = true;
                        Invoke("StopCrying", 40f);
                    }

                }
            }

            //ELDER LOGIC
            else if (NpcData.NpcType.Equals(NpcType.elder))
            {

                if (followPlayer)
                {
                    NextPoint = player.transform.position;
                }


                if (followPlayer && agent.remainingDistance < 3.5f)
                {
                    dialogManager.OpenDialog();
                    followPlayer = false;

                }

                else if (!agent.pathPending && agent.remainingDistance < 0.2f)
                {
                    if (IsStoppedInShelve)
                    {
                        Invoke("LeaveShelve", stopInTime);
                    }
                    if (Random.Range(0, 100) < findPlayerRate)
                    {
                        followPlayer = true;
                        Invoke("StopFollowingPlayer", followTime);
                    }
                    else
                    {
                        if (Random.Range(0, 100) < stopInShelveRate)
                        {
                            NextPoint = GetEmptyShelve();
                            IsStoppedInShelve = true;
                        }
                        else
                        {
                            NextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                        }
                    }
                }

            }

            //CLIENTS LOGIC
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.2f)
                {
                    if (IsStoppedInShelve)
                    {
                        Invoke("LeaveShelve", stopInTime);
                    }
                    else
                    {
                        if (Random.Range(0, 100) < stopInShelveRate)
                        {
                            NextPoint = GetEmptyShelve();
                            IsStoppedInShelve = true;
                        }
                        else
                        {
                            NextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                        }
                    }
                }
            }
            //Shopkeeper logic
            if (NpcData.NpcType.Equals(NpcType.shopkeeper))
            {
                if (GuideToObject)
                {
                    agent.SetDestination(objectToGuide.transform.position);
                    if (!agent.pathPending && agent.remainingDistance <1f)
                    {
                        PopUpManager.instance.CreatePopUp("Shopkeeper", Color.white, "El objeto está en esta estanteria.");
                        Invoke("ReturnToDefault",5f);
                    }
                 }
                if (returnToDefaultPosition)
                {
                    agent.SetDestination(NextPoint);
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        returnToDefaultPosition = false;
                    }
                    }
                   
            }
            else
            {
                agent.SetDestination(NextPoint);
            }
            
        }
    }
    void ReturnToDefault()
    {
        GuideToObject = false;
        returnToDefaultPosition = true;
    }
    private void StopFollowingPlayer()
    {
        followPlayer = false;
    }
    private void LeaveShelve()
    {
        busyShelve.GetComponentInParent<ShelvesManager>().ChangeAvailavility(false);
        IsStoppedInShelve = false;
    }
    private Vector3 GetEmptyShelve()
    {
        nearestPoint = Vector3.one;
        int randomShelve;
        do {
            randomShelve=(int)Random.Range(0, shelvesStopPoints.Count-1);
        }while (shelvesStopPoints[randomShelve].GetComponentInParent<ShelvesManager>().IsBusyNpc || shelvesStopPoints[randomShelve].GetComponentInParent<ShelvesManager>().IsBusyPlayer) ;
        nearestPoint = shelvesStopPoints[randomShelve].transform.position;
        busyShelve = shelvesStopPoints[randomShelve];
        busyShelve.GetComponentInParent<ShelvesManager>().ChangeAvailavility(true);
        return nearestPoint;
    }

    public Vector3 GetRandomNavmeshLocation(float radius)
    { 
        Vector3 finalPosition = Vector3.zero;
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit; 
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
        } while (Vector3.Distance(finalPosition, transform.position)<1);
        return finalPosition;
    }

    void StopCrying()
    {
        tearParticles.SetActive(false);
        dialogManager.GuardIsCrying = false;
        stopCry = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Product")
        {
            
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 1) { 
                playerIsAggressive = true;
                PlayerController.Instance.AgrresionCount++;
                Invoke("EndPlayerIsAggressive", 20f);
                if (npcData.IsStuneable)
                {
                    Debug.Log("npcStunned");
                    
                    npcStunned = true;
                    agent.speed = 0;
                    stunParticles.SetActive(true);
                    if (IsStoppedInShelve)
                    {
                        busyShelve.GetComponentInParent<ShelvesManager>().ChangeAvailavility(false);
                        IsStoppedInShelve = false;
                    }
                    Invoke("EndStun", 6f);
                }
            }
        }
    }
    void EndStun()
    {
        stunParticles.SetActive(false);
        npcStunned = false;
        agent.speed = npcSpeed;
    }
    void EndPlayerIsAggressive()
    {
        playerIsAggressive = false;
    }
}
