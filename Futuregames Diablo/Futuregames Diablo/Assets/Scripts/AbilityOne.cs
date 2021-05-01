using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityOne : MonoBehaviour
{
    //public bool playerMovementSpeed;

    private float abilitiyOneSpeed = 2000f;
    public Transform parentTransform;
    public Transform parentBuffTransform;

    public void ShootAbilityOne()
    {
        FindObjectOfType<PlayerController>().playerMana -= 5;
        if (FindObjectOfType<Skilltree>().skillArray1[0] == true 
            || FindObjectOfType<Skilltree>().skillArray1[1] == true)
        {
            GameObject abilityOne = Instantiate(gameObject, 
                parentTransform.transform.position, Quaternion.identity);

            abilityOne.GetComponent<Rigidbody>().AddForce(parentTransform.transform.forward 
                * abilitiyOneSpeed);
        }   
        if(FindObjectOfType<Skilltree>().skillArray1[2] == true)
        {
            FindObjectOfType<PlayerController>().StartMovementSpeedBuff();
        }
    }

    private void OnTriggerEnter(Collider enemyCollider)
    {
        enemyCollider.gameObject.GetComponent<EnemyController>().CheckDebuff(); 
        Destroy(gameObject);
    }
}
