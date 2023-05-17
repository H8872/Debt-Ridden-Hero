using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Rigidbody rb;
    SphereCollider sc;
    [SerializeField] public string wName, description; 
    [SerializeField] public int damage, roundedDamage;
    [SerializeField] public float speed, cooldown, lifetime;
    [SerializeField] public bool isDestroyOnhit, hasCollision, chargeSpeed, chargeDamage, chargeSize;
    PlayerController player;

    public enum AttackType
    {
        Arrow,
        Spell
    }
    [SerializeField] AttackType attackType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        roundedDamage = Mathf.RoundToInt(damage * player.chargeTime);
        if (hasCollision)
        {
            sc.isTrigger = false;
        } else {
            sc.isTrigger = true;
        }

        if (attackType == AttackType.Arrow)
        {
            if (chargeSpeed)
                rb.AddForce(transform.forward * speed * player.chargeTime);
            else
                rb.AddForce(transform.forward * speed);
        }

        if (chargeSize)
        {
            transform.localScale = transform.localScale * player.chargeTime;
        }
        player.rangedTime = cooldown;
        KillSelf(lifetime);
    }

    void KillSelf(float time)
    {
        player.chargeTime = player.minCharge;
        Destroy(gameObject, time);
    }

    void DamageBoss(BossController bossC)
    {
        if (chargeDamage)
        {
            bossC.GetHit(roundedDamage);
        } else {
            bossC.GetHit(damage);
        }
            
        if (isDestroyOnhit)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Boss")
        {
            BossController boss = other.GetComponent<BossController>();
            DamageBoss(boss);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Boss")
        {
            BossController boss = other.gameObject.GetComponent<BossController>();
            DamageBoss(boss);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
