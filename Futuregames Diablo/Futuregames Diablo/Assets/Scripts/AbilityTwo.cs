using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTwo : MonoBehaviour
{
    [SerializeField] private bool isGrowing;
    [SerializeField] private float growTimer;
    SphereCollider sphere;

    public void CastAbilityTwo()
    {
        FindObjectOfType<PlayerController>().playerMana -= 5;
        if(FindObjectOfType<Skilltree>().skillArray2[0])
        {
            StartGrowing();
        }
        if(FindObjectOfType<Skilltree>().skillArray2[1])
        {
            FindObjectOfType<PlayerController>().SacrificeHPForMana();
        }
        if(FindObjectOfType<Skilltree>().skillArray2[2])
        {
            FindObjectOfType<PlayerController>().channeling = true;
        }
    }

    private void OnTriggerEnter(Collider enemyCollider)
    {
        enemyCollider.gameObject.GetComponent<EnemyController>().StartFear();
    }

    private void Start()
    {
        sphere = gameObject.GetComponent<SphereCollider>();
    }
    private void Update()
    {
        GrowBig();
        growTimer += Time.deltaTime;
    }
    // makes the radius of the sphere collider grow to 10
    private void GrowBig()
    {
        if (isGrowing)
        {
            sphere.radius = growTimer * 2;
        }
        if (isGrowing && sphere.radius >= 10)
        {
            isGrowing = false;
            sphere.radius = 1;
        }
    }
    // starts the growing process
    private void StartGrowing()
    {
        growTimer = 0;
        isGrowing = true;
    }

}
