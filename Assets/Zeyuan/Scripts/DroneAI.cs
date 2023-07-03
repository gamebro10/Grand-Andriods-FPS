using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : EnemyBase
{
    [SerializeField] float floatingSpeed;
    [SerializeField] float shootRange;
    [SerializeField] Transform firePosLeft;
    [SerializeField] Transform firePosRight;
    [SerializeField] GameObject bullet;

    float floatParam = 0;
    float floatDivisor = 4;

    public GameObject Body;//used only for floating animation

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Float();
        if (currentTarget == Player)
        {
            InCombat();
        }
        else
        {
            OutCombat();
        }

    }

    void Float()
    {
        floatParam = floatParam > 180 ? 0 : floatParam;
        float y = Mathf.Sin(floatParam) * Mathf.PI / 180 / floatDivisor;
        floatParam += Time.deltaTime * floatingSpeed;
        Body.transform.position = new Vector3(Body.transform.position.x, Body.transform.position.y + y, Body.transform.position.z);
    }

    void Attack()
    {
        Debug.DrawRay(firePosLeft.position, Player.transform.position - firePosLeft.position);
        Debug.DrawRay(firePosRight.position, Player.transform.position - firePosRight.position);
        if (!isShooting)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePosLeft.position, Player.transform.position - firePosLeft.position, out hit, shootRange, layerMask))
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
        int randomAttackCount = UnityEngine.Random.Range(4, 8);

        for (int i = 0; i < randomAttackCount; i++)
        {
            if (i % 2 == 0)
            {
                Instantiate(bullet, firePosLeft.transform.position, Quaternion.LookRotation(Player.transform.position - firePosLeft.position));
            }
            else
            {
                Instantiate(bullet, firePosRight.transform.position, Quaternion.LookRotation(Player.transform.position - firePosRight.position));
            }
            yield return new WaitForSeconds(attackRate);
        }
        yield return new WaitForSeconds(attackCD);
        isShooting = false;
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
            StartCoroutine(HitAndRun());
            Attack();
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
