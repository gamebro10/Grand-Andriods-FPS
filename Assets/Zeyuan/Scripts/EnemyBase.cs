using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IDamage
{
    [SerializeField] protected int faceToPlayerSpeed;
    [SerializeField] protected int roamTime;
    [SerializeField] protected int roamDistance;
    [SerializeField] protected int viewAngle;
    [SerializeField] protected int hp;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float attackCD;
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected GameObject Player;//will be deleted when a player instancee can be accessed globally. For now just assign player manually.
    [SerializeField] protected LayerMask layerMask;

    protected float angleToPlayer;

    protected bool isPlayerInRange;
    protected bool destinationChosen;
    protected bool isShooting;

    protected Vector3 playerDir;
    protected Vector3 startingPosition;

    protected Transform targetTransform;

    protected GameObject currentTarget;

    protected virtual void Start()
    {
        startingPosition = transform.position;
    }

    protected virtual void Update()
    {
        playerDir = Player.transform.position - transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public virtual void OnTakeDamage(int amount)
    {
        hp -= amount;
        currentTarget = Player;
    }

    protected void MoveToPlayer()
    {
        if (!destinationChosen)
        {
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(Player.transform.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                FaceToPlayer();
            }
        }
    }

    protected void FaceToPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceToPlayerSpeed);
    }

    protected bool CanSeePlayer()
    {
        agent.stoppingDistance = stoppingDistance;
        playerDir = Player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(transform.position, playerDir, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    protected IEnumerator Iroam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTime);

            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * roamDistance;

            randomPos += startingPosition;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);

            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    protected void MoveToRandomPosition(float amount)
    {
        destinationChosen = true;
        agent.stoppingDistance = 0;

        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * amount;

        randomPos += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, amount, 1);

        agent.SetDestination(hit.position);

    }

    protected virtual IEnumerator HitAndRun()
    {
        if (!destinationChosen)
        {
            agent.updateRotation = false;
            MoveToRandomPosition(Random.Range(1f, 2f));

            yield return new WaitUntil(() => { return agent.remainingDistance - agent.stoppingDistance <= 0.05; });
            agent.updateRotation = true;
            destinationChosen = false;
        }
    }

}
