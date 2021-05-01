using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    // damage dealt
    public int damageToPlayer = 2, damageToEnemy = 4;
    //tick
    public int tickDamage = 2;
    //slow
    public float slowAmount = 1;

    public float globalCDTime;
    public bool GCDIsAvailable;

    PlayerController player;
    EnemyController enemy;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemy = FindObjectOfType<EnemyController>();


    }

    private void Update()
    {
        

        GlobalCooldown();
        globalCDTime -= Time.deltaTime;
    }

    public void DealDamageToPlayer()
    {
        player.currentPlayerHP -= damageToPlayer;
    }

    public void GlobalCooldown()
    {
        if(globalCDTime <= 0 && player.playerMana >= 5)
        {
            GCDIsAvailable = true;
        }
        else { GCDIsAvailable = false; }
    }


}
