using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] public string wName, description; 
    [SerializeField] public int damage;
    [SerializeField] public bool isMelee;
    [SerializeField] public float range, speed, cooldown, lifetime;
    PlayerController player;
    [SerializeField] GameObject melee, ranged;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void Attack()
    {
        if (isMelee)
        {
            var newMelee = Instantiate(
                melee, transform.position + transform.forward * range, transform.rotation
                );
            player.meleeTime = cooldown;
            Destroy(newMelee, lifetime);
        } else {
            var newRanged = Instantiate(
                ranged, transform.position + transform.forward, transform.rotation
                );
            player.rangedTime = cooldown;
            Destroy(newRanged, lifetime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
