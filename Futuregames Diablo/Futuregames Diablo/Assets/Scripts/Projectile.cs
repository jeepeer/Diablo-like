using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{  
    public float projectileSpeed;

    public Transform parentTransform;

    public void ShootProjectile()
    {
        GameObject shot = Instantiate(gameObject, parentTransform.transform.position, Quaternion.identity);
        shot.GetComponent<Rigidbody>().AddForce(parentTransform.transform.forward * projectileSpeed);
    }

    private void OnTriggerEnter(Collider enemyCollider)
    {
        enemyCollider.gameObject.GetComponent<EnemyController>().DealDamageToEnemy();
        DestroyGameObject();
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
