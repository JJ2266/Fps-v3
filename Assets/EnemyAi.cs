using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyBehaviour
{
    Wander,
    Attack,
    Pursue,
    Recover,
}

public class EnemyAi : MonoBehaviour
{
    float wanderRange = 8;
    Vector3 startingLocation;
    float sightRange = 6;
    float attackRange = 3;
    float currentStateElapsed;
    float recoveryTime = 8;
    [SerializeField] EnemyBehaviour currentState; 

    Rigidbody rb;
    NavMeshAgent agent;
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        startingLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, target.position)<sightRange)
        {
            currentState = EnemyBehaviour.Pursue;
        }
        if (Vector3.Distance(gameObject.transform.position, target.position) < attackRange)
        {
            currentState = EnemyBehaviour.Attack;
        }


        switch (currentState) 
        {
            case EnemyBehaviour.Wander:
                Enterwander();
                break;
            case EnemyBehaviour.Attack:
                agent.enabled = false;
                
                Enterattack();
                break;
            case EnemyBehaviour.Pursue:
                EnterPursue();
                break;
            case EnemyBehaviour.Recover :
                EnterRecover();
                break;

        }
    }
    Vector3 GetRandomPointInRange()
    {
        Vector3 offset = new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));
        
        NavMeshHit hit;

        bool gotpoint = NavMesh.SamplePosition(startingLocation + offset, out hit, 1, NavMesh.AllAreas);

        if (gotpoint)
            return hit.position;

        return Vector3.zero;
    }

    [ContextMenu("Wander")]
    void Enterwander()
    {
        agent.enabled = true;
        
        agent.SetDestination(GetRandomPointInRange());

    }
    void Enterattack()
    {
        rb.AddForce(target.position * 1);
        rb.isKinematic = false;
        StartCoroutine(EnterRecover());
    }
    void EnterPursue()
    {
        rb.isKinematic = true;
        agent.enabled = true;
        agent.SetDestination(target.position);
    }
    IEnumerator EnterRecover()
    {
        agent.enabled = false;
        rb.isKinematic = false;
        yield return new WaitForSeconds(recoveryTime);
        

    }
}
