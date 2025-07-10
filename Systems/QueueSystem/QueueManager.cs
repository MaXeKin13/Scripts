using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class QueueManager : MonoBehaviour
{
    public int spotNum;
    public float distanceBetweenSpots = 2f;
    public Vector3[] spots;

    private int openSpot = 0;

    //test
    public List<Queuer> allQueuers = new List<Queuer>();
    //end test
    public List<Queuer> queuers = new List<Queuer>();

    //destinations? destination types?

    public Transform[] destinations;
    private int destinationIndex = 0; 


    private void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            foreach(Queuer queuer in allQueuers)
            {
                Debug.Log("Move to Spot");
                queuer.MoveToOpenQueueSpot(this);
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            LeaveSpot();
        }
    }
    private void Start()
    {
        InitializeWaitPoints();
    }

    private void InitializeWaitPoints()
    {
        spots = new Vector3[spotNum];

        for(int i = 0; i < spotNum; i++)
        {
            spots[i] = new Vector3(transform.position.x, transform.position.y, transform.position.z - i * distanceBetweenSpots);
        }
    }
    public void UpdateSpots()
    {
        for(int i = 0; i < queuers.Count; i++)
        {
            queuers[i].MoveToDestination(spots[i]);
        }
    }
    public Vector3 GetOpenPoint()
    {
        openSpot++;
        return spots[openSpot-1];
    }
    public void AddQueuer(Queuer queuer)
    {
        queuers.Add(queuer);
    }

    public void LeaveSpot()
    {        
        queuers[0].MoveToDestination(destinations[destinationIndex].position);
        destinationIndex++;
        queuers.Remove(queuers[0]);

        UpdateSpots();
    }

}
