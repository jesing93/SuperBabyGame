using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcBehaviour : MonoBehaviour
{
    [SerializeField] private NpcData npcData;
    [SerializeField] private GameObject shelves;
    [SerializeField] private List<GameObject> shelvesStopPoints;
    [SerializeField] private float maxWalkingDistance;
    [SerializeField] private float stopInShelveRate;
    [SerializeField] private float findPlayerRate;
    [SerializeField] private float stopInTime;
    [SerializeField] private float followTime;
    [SerializeField] private GameObject player;
    [SerializeField] private int maxAggressionTimes;

    private int agrresionCount;
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

    public NpcData NpcData { get => npcData; set => npcData = value; }
    public Vector3 NextPoint { get => nextPoint; set => nextPoint = value; }
    public bool NpcStunned { get => npcStunned; set => npcStunned = value; }
    public bool PlayerIsAggressive { get => playerIsAggressive; set => playerIsAggressive = value; }

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
         
    }
    private void Update()
    {   
        goToNextPoint();  
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
                    if (agrresionCount>= maxAggressionTimes)
                    {
                        NextPoint = player.transform.position;
                    }
                    if (!agent.pathPending && agent.remainingDistance < 1f)
                    {
                        if (agrresionCount >= maxAggressionTimes)
                        {
                            dialogManager.OpenDialog();
                            followPlayer = false;
                            agent.speed = 0;
                            agrresionCount = 0;
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
                if (agent.remainingDistance < 1.5f)
                {

                    if (followPlayer)
                    {
                        dialogManager.OpenDialog();
                        agent.speed = 0;
                        followPlayer = false;
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
                            isStoppedInShelve = true;
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
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    if (isStoppedInShelve)
                    {
                        Invoke("LeaveShelve", stopInTime);
                    }
                    else
                    {
                        if (Random.Range(0, 100) < stopInShelveRate)
                        {
                            NextPoint = GetEmptyShelve();
                            isStoppedInShelve = true;
                        }
                        else
                        {
                            NextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                        }
                    }
                }
            }
            agent.SetDestination(NextPoint);
        }
    }

    private void StopFollowingPlayer()
    {
        followPlayer = false;
    }
    private void LeaveShelve()
    {
        busyShelve.GetComponentInParent<ShelvesManager>().ChangeAvailavility(false);
        isStoppedInShelve = false;
    }
    private Vector3 GetEmptyShelve()
    {
        nearestPoint = Vector3.one;
        int randomShelve;
        do {
            randomShelve=(int)Random.Range(0, shelvesStopPoints.Count);
        }while (shelvesStopPoints[randomShelve].GetComponentInParent<ShelvesManager>().IsBusyNpc && shelvesStopPoints[randomShelve].GetComponentInParent<ShelvesManager>().IsBusyPlayer) ;
        Debug.Log(!shelvesStopPoints[randomShelve].GetComponentInParent<ShelvesManager>().IsBusyNpc && !shelvesStopPoints[randomShelve].GetComponentInParent<ShelvesManager>().IsBusyPlayer);
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
        } while (Vector3.Distance(finalPosition, transform.position)<8);
        return finalPosition;
    }

    void StopCrying()
    {
        dialogManager.GuardIsCrying = false;
        stopCry = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.GetComponent<Rigidbody>().velocity);
        if (collision.gameObject.tag == "Product")
        {
            playerIsAggressive = true;
            agrresionCount++;
            Invoke("EndPlayerIsAggressive", 20f);
            if (npcData.IsStuneable)
            {
                npcStunned = true;
                if (isStoppedInShelve)
                {
                    busyShelve.GetComponentInParent<ShelvesManager>().ChangeAvailavility(false);
                    isStoppedInShelve = false;
                }
                Invoke("EndStun", 6f);
            }
        }
    }
    void EndStun()
    {
        npcStunned = false;
    }
    void EndPlayerIsAggressive()
    {
        playerIsAggressive = false;
    }
}
