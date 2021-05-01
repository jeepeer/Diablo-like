using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{   
    //LayerMasks
    public LayerMask groundMask, enemyMask;
    //playerHP
    public int playerMaxHP = 10;
    public int currentPlayerHP;
    private float HPRegenTimer = 2f;

    //player mana
    public int playerMana;
    private int maxPlayerMana = 20;
    private float ManaRegenTimer;

    //speed buff
    public bool isFast;
    private float speedBuff = 8;
    public float speedBuffTimer = 3f;

    //navmesh and general range indicators
    NavMeshAgent agent;
    Vector3 clickPos = Vector3.one;
    public float range;
    public bool enemyInRange;

    Projectile projectile;
    GameController gameController;
    //auto attacking
    public float autoAttackCooldown = 2f;
    //auto attacking tagging 
    private string enemyTag = "enemyTag";
    private Transform target;
    //used to separate normal attacks and automatical attacks
    public bool playerAttacking;

    public bool isCasting;
    public float castingCooldown;
    public float channelHealTimer;
    public bool channeling;

    // Use this for initialization
    void Start ()
    {
        gameController = FindObjectOfType<GameController>();
        projectile = FindObjectOfType<Projectile>();
        agent = GetComponent<NavMeshAgent>();
        //sets current HP to max HP
        currentPlayerHP = playerMaxHP;
        playerMana = maxPlayerMana;
        //searches for the closest enemy every 1 second
        InvokeRepeating("ClosestEnemy", 0f, 1f);

        MovementSpeedBuff();
    }
 
	// Update is called once per frame
	void Update ()
    {
        RegenHP();
        HPRegenTimer -= Time.deltaTime;

        StopMovingAndAttacking();
        speedBuffTimer -= Time.deltaTime;
        MovementSpeedBuff();

        AutoAttacking();
        autoAttackCooldown -= Time.deltaTime;

        CheckAbilityTwoCast();
        castingCooldown -= Time.deltaTime;
        ChannelHeal();
        channelHealTimer -= Time.deltaTime;

        RegenMana();
        ManaRegenTimer -= Time.deltaTime;
        
        //sees if the enemyMask is in range
        enemyInRange = Physics.CheckSphere(transform.position, range, enemyMask);
    }

    //right click commands (walking, attacking)
    public void OnMouseClick()
    {
        //right mousebutton inputs
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            playerAttacking = true;
            //Moves character on groundMask
            if (Physics.Raycast(ray, out hit, 100f, groundMask))
            {
                clickPos = hit.point;
                agent.SetDestination(clickPos);
            }
            //Attacks enemies with an enemyMask (normal attack) 
            if(Physics.Raycast(ray, out hit, 100f, enemyMask))
            {
                //if enemy is in range, the player will stop and shoot when clicking on them
                if (enemyInRange)
                {
                    transform.LookAt(clickPos);
                    Attack();
                }
                //if the enemy is out of range the player will walk towards it and shoot when enemyInRange = true, using AutoAttacking()
                else
                {
                    agent.SetDestination(clickPos);
                    playerAttacking = false;
                }
                Debug.Log("Enemy");
            }
            Debug.Log(clickPos);
        }
    }
    //a commands (walking automatically attacking)
    private void OnAClick()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            playerAttacking = false;
            //Moves character on groundMask
            if (Physics.Raycast(ray, out hit, 100f, groundMask))
            {
                clickPos = hit.point;
                agent.SetDestination(clickPos);
            }
            //if you click an enemy with A it will work the same way as with mouse1 except it will automatically attack when the cooldown = 0
            if (Physics.Raycast(ray, out hit, 100f, enemyMask))
            {
                if (enemyInRange)
                {
                    agent.SetDestination(transform.position);
                    transform.LookAt(clickPos);
                    AutoAttacking();
                }
                // walks towards the enemy and uses an auto attack
                else { agent.SetDestination(clickPos); autoAttackCooldown = 0f; }
                Debug.Log("Enemy");
            }
            Debug.Log(clickPos);
        }
    }
    // shoots from where you are when holding shift + rightclick
    private void OnShiftClick()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
        {
            transform.LookAt(clickPos);
            Attack();
        }
    }
    //first ability Q
    private void OnAbilityOneClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            clickPos = hit.point;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gameController.GCDIsAvailable)
            {
                agent.SetDestination(transform.position);
                transform.LookAt(clickPos);
                FindObjectOfType<AbilityOne>().ShootAbilityOne();
            }
        }
    }
    // second ability W
    private void OnAbilityTwoClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            clickPos = hit.point;
        }
        // when you press W isCasting gets set to true and the cooldown timer is reset. Making it so you "cast" an ability
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (gameController.GCDIsAvailable)
            {
                isCasting = true;
                castingCooldown = 2f;
                if (isCasting)
                {
                    agent.SetDestination(transform.position);
                }
            }
        }
    }
    // looks for when the casting has been finished so that it can use the ability
    private void CheckAbilityTwoCast()
    {
        if (isCasting && castingCooldown <= 0)
        {
            isCasting = false;
            FindObjectOfType<AbilityTwo>().CastAbilityTwo();
        }
    }

    // with the "S" key movement and attacks will stop, but will be resumed when using an input again (PUT ALL CLICKS OR ABILITIES IN HERE)
    private void StopMovingAndAttacking()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            agent.SetDestination(transform.position);
            playerAttacking = true;
        }
        else
        {
            OnMouseClick();
            OnAClick();
            OnShiftClick();
            OnAbilityOneClick();
            OnAbilityTwoClick();
        }
    }

    //stops player and shoots projectile
    private void Attack()
    {
        agent.SetDestination(transform.position);
        if (autoAttackCooldown <= 0)
        {
            projectile.ShootProjectile();
            autoAttackCooldown = 2f;
        }
    }
    //auto attack when in range of an enemy
    private void AutoAttacking()
    {
        if (!playerAttacking)
        {
            if (autoAttackCooldown <= 0 && enemyInRange)
            {
                transform.LookAt(target);
                Attack();
                autoAttackCooldown = 2f;
            }
        }
    }

    //finds the enemy closest to the player
    public void ClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
    }


    public void MovementSpeedBuff()
    {
        if (isFast)
        {
            agent.speed = speedBuff;
            if (speedBuffTimer <= 0)
            {
                agent.speed = 4;
                isFast = false;
            }
        }
    }
    //starts the slow debuff
    public void StartMovementSpeedBuff()
    {
        isFast = true;
        speedBuffTimer = 3f;
    }

    // regens hp every 4 seconds
    private void RegenHP()
    {
        if(currentPlayerHP != playerMaxHP && HPRegenTimer <= 0)
        {
            currentPlayerHP += 1;
            HPRegenTimer = 4;
        }
    }

    // regens mana every 2 seconds
    private void RegenMana()
    {
        if(playerMana < maxPlayerMana && ManaRegenTimer <= 0)
        {
            playerMana += 1;
            ManaRegenTimer = 2;
        }
    }

    //ADD PLAYER DEATH
    
    // removes 3 hp for 3 mana
    public void SacrificeHPForMana()
    {
        if (playerMana == maxPlayerMana)
        {
            return;
        }
        else        
        currentPlayerHP -= 3;
        playerMana += 3;        
    }

    public void ChannelHeal()
    {
        if(currentPlayerHP < playerMaxHP && channelHealTimer <= 0 && channeling)
        {
            currentPlayerHP += 1;
            channelHealTimer = 1;
            Debug.Log("healing");
        }
    }


    //range of abilities and attacks
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
