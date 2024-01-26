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

    private Vector3 nextPoint;
    private NavMeshAgent agent;
    private Vector3 nearestPoint;
    private bool isStoppedInShelve=false;
    private GameObject busyShelve;
    private bool followPlayer;

    public NpcData NpcData { get => npcData; set => npcData = value; }

    private void Awake()
    {
        nextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
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
        if (NpcData.NpcType.Equals(NpcType.guard))
        {
            if (followPlayer)
            {
                nextPoint = player.transform.position;
            }
            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                if (followPlayer)
                {
                    //TODO Stop player and open dialog
                    followPlayer = false;
                }
                else
                {
                    nextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                }
            }
        }
        else if (NpcData.NpcType.Equals(NpcType.elder))
        {
           
            if (followPlayer)
            {
                nextPoint = player.transform.position;
            }
            Debug.Log(agent.remainingDistance);
            if (agent.remainingDistance < 1.5f)
            {

                if (followPlayer)
                {
                    //TODO Stop player and open dialog
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
                        nextPoint = GetEmptyShelve();
                        isStoppedInShelve = true;
                    }
                    else
                    {
                        nextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                    }
                }
              }
         }
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
                            nextPoint = GetEmptyShelve();
                            isStoppedInShelve = true;
                        }
                        else
                        {
                           nextPoint = GetRandomNavmeshLocation(maxWalkingDistance);
                        }
                    }
                }
        }
       
         agent.SetDestination(nextPoint);
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
        } while (Vector3.Distance(finalPosition, transform.position)<10);
        return finalPosition;
    }
}
