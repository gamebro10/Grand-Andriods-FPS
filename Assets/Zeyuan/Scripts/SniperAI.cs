using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperAI : EnemyBase
{
    [SerializeField] float shootRange;
    [SerializeField] float aimTime;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePos;
    [SerializeField] LineRenderer laser;

    bool showLaser;

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        base.Update();
        anime.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
        if (currentTarget == Player)
        {
            InCombat();
            if (showLaser)
            {
                ShowLaser();
            }
        }
        else
        {
            OutCombat();
        }
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
        showLaser = true;
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(aimTime);
        Instantiate(bullet, firePos.transform.position, Quaternion.LookRotation(Player.transform.position - firePos.position));
        showLaser = false;
        laser.gameObject.SetActive(false);
        yield return new WaitForSeconds(attackCD);
        isShooting = false;
    }

    void ShowLaser()
    {
        laser.SetPosition(0, firePos.transform.position);
        laser.SetPosition(1, Player.transform.position + new Vector3(0, .05f, 0));
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

    protected override void OnDeath()
    {
        base.OnDeath();
        laser.enabled = false;
    }
}
