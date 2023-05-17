using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the script is attached to the projectile/target of the attack
public class BossAttackBehaviour : MonoBehaviour
{
    enum AttackType 
    {
        GroundSlam,
        Projectile,
        BeamRay
    }
    [SerializeField] AttackType attackType;
    [SerializeField] float hitTime = 0.3f, speed = 1, damage = 1, knockBack = 1;
    public float hitDelay = 0;
    SphereCollider hitBox;
    ParticleSystem particles;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        hitTime += Time.time+hitDelay;
        hitDelay += Time.time;
        hitBox = GetComponentInChildren<SphereCollider>();
        particles = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        if(attackType == AttackType.Projectile)
        {
            rb.AddForce(transform.forward*speed);
        }
        hitBox.enabled = false;
        //Hitbox transform scale changed for visual debug reasons
        //Can be removed in case it breaks something
        hitBox.transform.localScale = Vector3.one/2;
        particles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(hitDelay<Time.time)
        {
            hitBox.enabled = true;
            hitBox.transform.localScale = Vector3.one;
            particles.gameObject.SetActive(true);
        }
        if(hitTime<Time.time)
        {
            hitBox.enabled = false;
            hitBox.gameObject.SetActive(false);
            if(attackType == AttackType.Projectile || attackType == AttackType.BeamRay)
                particles.Stop();
        }

        if(particles.isStopped && hitTime<Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.GetHit(transform.position, damage, knockBack);
        }
    }
}
