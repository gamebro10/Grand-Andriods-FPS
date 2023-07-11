using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] protected float healthLerpSpeed;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Renderer[] renderers;
    [SerializeField] protected Color flashColor;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image lerpHealthBar;

    protected float angleToPlayer;

    protected bool isPlayerInRange;
    protected bool destinationChosen;
    protected bool isShooting;
    protected bool canChangeColor = true;

    protected Vector3 playerDir;
    protected Vector3 startingPosition;

    protected Transform targetTransform;

    protected GameObject currentTarget;
    protected GameObject Player;

    float maxHealth;
    float lerpTimer;
    float lerpValue;
    float lerpHealthAmount;
    float lerpGoal;

    bool addingUpLerpAmount;
    bool startLerping;

    protected virtual void Start()
    {
        GameManager.Instance.updateEnemy(1);
        startingPosition = transform.position;
        Player = GameManager.Instance.player;
        maxHealth = hp;
    }

    protected virtual void Update()
    {
        playerDir = Player.transform.position - transform.position;
        LerpHealthBar();
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
        if (hp <= 0)
        {
            GameManager.Instance.updateEnemy(-1);
            Destroy(gameObject);
        }
        healthBar.fillAmount = hp / maxHealth;
        StartCoroutine(ILerpHealthBar(amount));
        if (renderers.Length > 0 && canChangeColor)
        {
            StartCoroutine(IFlashMaterial(renderers));
        }
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
        angleToPlayer = GetAngleToPlayer();

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

    protected virtual IEnumerator IFlashMaterial(Renderer[] renderers)
    {
        canChangeColor = false;
        Color[] colors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            colors[i] = renderers[i].material.color;
        }
        ChangeRendererColor(flashColor, renderers);
        yield return new WaitForSeconds(.1f);
        ChangeRendererColor(Color.white, renderers, colors);
        yield return new WaitForSeconds(.1f);
        ChangeRendererColor(flashColor, renderers);
        yield return new WaitForSeconds(.1f);
        ChangeRendererColor(Color.white, renderers, colors);
        yield return new WaitForSeconds(.1f);
        ChangeRendererColor(flashColor, renderers);
        yield return new WaitForSeconds(.1f);
        ChangeRendererColor(Color.white, renderers, colors);

        canChangeColor = true;
    }

    void ChangeRendererColor(Color color, Renderer[] renderers, Color[] colors = null)
    {
        if (colors == null)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = color;
            }
        }
        else
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = colors[i];
            }
        }
        
        
    }

    IEnumerator ILerpHealthBar(float amount)
    {
        lerpTimer = .5f;
        if (!addingUpLerpAmount)
        {
            lerpValue = 0f;
            addingUpLerpAmount = true;
            yield return new WaitForSeconds(lerpTimer);
            lerpGoal = healthBar.fillAmount;
            lerpHealthAmount = lerpHealthBar.fillAmount;
            lerpTimer = 0f;
            addingUpLerpAmount = false;
            startLerping = true;
        }
        
    }

    void LerpHealthBar()
    {
        if (startLerping)
        {
            float lerpAmont = Mathf.Lerp(lerpHealthAmount, lerpGoal, lerpValue);
            lerpHealthBar.fillAmount = lerpAmont;
            lerpValue += Time.deltaTime * healthLerpSpeed; 
            if (lerpHealthBar.fillAmount <= lerpGoal)
            {
                startLerping = false;
            }
        }
    }

    protected float GetAngleToPlayer()
    {
        return Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
    }

}
