using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class SoldierAI : NormalEnemyBase
{
    [SerializeField] float shootRange;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePos;
    [SerializeField] AudioClip footStep1Sound;
    [SerializeField] AudioClip footStep2Sound;

    bool canFootStep = true;

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        base.Update();
        anime.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
        if (canFootStep && agent.velocity.magnitude > 0.1f)
        {
            StartCoroutine(IFootStep());
        }

        if (currentTarget == Player)
        {
            InCombat();
        }
        else
        {
            OutCombat();
        }
    }

    IEnumerator IFootStep()
    {
        canFootStep = false;
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            audioSource.PlayOneShot(footStep1Sound, 1f);
        }
        else
        {
            audioSource.PlayOneShot(footStep2Sound, 1f);
        }
        yield return new WaitForSeconds(.5f);
        canFootStep = true;
    }

    void Attack()
    {
        Debug.DrawRay(firePos.position, Player.transform.position - firePos.position);
        if (!isShooting)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePos.position, Player.transform.position - firePos.position, out hit, shootRange, layerMask)) 
            {
                if (hit.collider.CompareTag("Player"))
                {
                    StartCoroutine(IAttack());
                }
            }
        }
    }

    IEnumerator IAttack()
    {
        isShooting = true;
        int randomAttackCount = UnityEngine.Random.Range(2, 5);

        for (int i = 0; i < randomAttackCount; i++)
        {
            Instantiate(bullet, firePos.transform.position, Quaternion.LookRotation(Player.transform.position - firePos.position));
            audioSource.PlayOneShot(shootSound, 1.5f);
            yield return new WaitForSeconds(attackRate);
        }
        yield return new WaitForSeconds(attackCD);
        isShooting = false;
    }

    protected override IEnumerator HitAndRun()
    {
        if (!destinationChosen)
        {
            float tempSpeed = agent.speed;
            agent.speed = 2;
            agent.updateRotation = false;
            destinationChosen = true;
            agent.stoppingDistance = 0;

            float randomAmount = Random.Range(4f, 8f);
            Vector3 moveDirection = Random.Range(0, 2) == 0 ? -transform.right * randomAmount : transform.right * randomAmount;
            Vector3 randomPos = transform.position + moveDirection;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, randomAmount, 1);

            agent.SetDestination(hit.position);

            yield return new WaitUntil(() => { return agent.remainingDistance - agent.stoppingDistance <= 0.05; });
            agent.speed = tempSpeed;
            agent.updateRotation = true;
            destinationChosen = false;
        }
    }

    void InCombat()
    {
        FaceToPlayer();
        if (Vector3.Distance(transform.position, Player.transform.position) > shootRange)
        {
            MoveToPlayer();
        }
        else
        {
            if (!isShooting)
            {
                int shouldStayOrMove = Random.Range(0, 10);
                if (shouldStayOrMove < 7)//80% change to hit and run, 20% to stay and shoot
                {
                    StartCoroutine(HitAndRun());
                    Attack();
                }
                else
                {
                    Attack();
                }
            }
        }
    }

    void OutCombat()
    {
        if (isPlayerInRange && CanSeePlayer())
        {
            currentTarget = Player;
        }
        else
        {
            StartCoroutine(Iroam());
        }
    }
}
