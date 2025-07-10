using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Queuer : MonoBehaviour
{
    [SerializeField] private Animator hoboAnimator;

    private NavMeshAgent agent;
    private static readonly int Spead = Animator.StringToHash("Spead");
    private static readonly int UaSleep = Animator.StringToHash("UASleep");


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (hoboAnimator == null)
        {
            hoboAnimator = GetComponent<Animator>();
        }
    }
    public void MoveToOpenQueueSpot(QueueManager queue)
    {
        queue.AddQueuer(this);
        agent.SetDestination(queue.GetOpenPoint());
        Debug.Log("Move to spot 2");
        Debug.Log(agent.SetDestination(queue.GetOpenPoint()));
    }

    public void MoveToDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private void Update()
    {
        hoboAnimator.SetFloat(Spead, agent.velocity.magnitude);    
    }
    private bool HasReachedDestination()
    {
        return agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}
