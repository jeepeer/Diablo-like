using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public LayerMask playerMask;
    GameController gameController;
    NavMeshAgent agent;
    Skilltree skilltree;
    //enemyHP   
    public int enemyMaxHP = 10;
    public int currentEnemyHP;
    
    public GameObject player;
    //ranges
    public bool playerInRange;
    public bool playerInAttackRange;
    public float followRange, attackRange;

    private float attackCooldown = 1f;

    // take tick damage
    public float tickTimer = 1f;
    public float tickDuration = 4f;
    public bool tickDamageOn;
    // slow debuff
    public float slowTimer = 3f;
    public bool isSlowed;
    private float slowedMovementspeed = 1;

    [SerializeField]private bool isFeared;
    private float fearedTimer = 2f;

    // navpoints for patroling
    public Transform navPointUP, navPointDOWN, navPointLEFT, navPointRIGHT;
    private int randomNumber;

    // Use this for initialization
    void Start ()
    {
        skilltree = FindObjectOfType<Skilltree>();
        gameController = FindObjectOfType<GameController>();
        agent = GetComponent<NavMeshAgent>();
        currentEnemyHP = enemyMaxHP;
        //invokes Patrol() every 2 seconds
        InvokeRepeating("Patrol", 2, 2);

        FindObjectOfType<EnemySpawner>().enemyCount++;  


    }
	
	// Update is called once per frame
	void Update ()
    {
        //checks if the players is in range
        playerInRange = Physics.CheckSphere(transform.position, followRange, playerMask); Debug.Log("found");
        // checks if the player is in range to attack
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);
        
        FollowPlayer();
        AttackPlayer();
        
        TakeTickDamage();
        SlowEnemy();
        RunAway();

        attackCooldown -= Time.deltaTime;
        tickTimer -= Time.deltaTime;
        tickDuration -= Time.deltaTime;
        slowTimer -= Time.deltaTime;
        fearedTimer -= Time.deltaTime;
        //random number for the cases in Patrol()
        randomNumber = Random.Range(0, 5);

        if(currentEnemyHP <= 0)
        {
            EnemyDead();
        }


    }

    // follows the players transform.position
    private void FollowPlayer()
    {
        if (playerInRange)
        {
            agent.SetDestination(player.transform.position);
            Debug.Log("I see player");
            transform.LookAt(player.transform.position);
        }
    }

    private void AttackPlayer()
    {
        if(playerInAttackRange && attackCooldown <= 0)
        {
            gameController.DealDamageToPlayer();
            attackCooldown = 1f;
        }
    }

    //patrols in 1 of 4 directions
    private void Patrol()
    {
        switch (randomNumber)
        {
            case 0:
                agent.SetDestination(navPointUP.position);
                Debug.Log("up");
                break;
            case 1:
                agent.SetDestination(navPointDOWN.position);
                Debug.Log("down");
                break;
            case 2:
                agent.SetDestination(navPointLEFT.position);
                Debug.Log("left");
                break;
            case 3:
                agent.SetDestination(navPointRIGHT.position);
                Debug.Log("right");
                break;
            case 4:
                agent.SetDestination(transform.position);
                Debug.Log("stop");
                break;
        }
    }

    // _______________________________________________________________

    //damage types and debuffs
    private void EnemyDead()
    {
        FindObjectOfType<EnemySpawner>().enemyCount--;
        Invoke("Destroy(gameObject)",3f);
    }
    public void DealDamageToEnemy()
    {
        currentEnemyHP -= gameController.damageToEnemy;
    }
    // checks what debuff is active            See if i can make this look better
    public void CheckDebuff()
    {
        if(skilltree.skillArray1[0] == true)
        {
            TickDamageStart();
        }
        if (skilltree.skillArray1[1] == true)
        {
            SlowEnemyStart();
        }
    }
    public void CheckDebuffTwo()
    {
        if (skilltree.skillArray2[0] == true)
        {
            StartFear();
        }
        if (skilltree.skillArray2[2] == true)
        {
            TickDamageStart();
        }
    }
    //deals the tick damage
    public void TakeTickDamage()
    {
        if (tickDamageOn)
        {
            if (tickTimer <= 0)
            {
                currentEnemyHP -= gameController.tickDamage;
                tickTimer = 1;
            }
            if (tickDuration <= 0)
            {
                tickDamageOn = false;
            }
        }
    }
    //resets the times so TickDamage() works as well as enables tickDamageOn
    public void TickDamageStart()
    {
        tickDamageOn = true;
        tickDuration = 4;
        tickTimer = 1;
    }
    //slows the enemy movementspeed
    public void SlowEnemy()
    {
        if (isSlowed)
        {
            agent.speed = slowedMovementspeed;
            if (slowTimer <= 0)
            {
                agent.speed = 2;
                isSlowed = false;
            }
        }
    }
    //starts the slow debuff
    public void SlowEnemyStart()
    {
        isSlowed = true;
        slowTimer = 3f;
    }

    private void RunAway()
    {
        if (isFeared)
        {
            agent.SetDestination(-player.transform.position);
            if(fearedTimer <= 0)
            {
                isFeared = false;
            }
        }
    }
    public void StartFear()
    {
        isFeared = true;
        fearedTimer = 2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
