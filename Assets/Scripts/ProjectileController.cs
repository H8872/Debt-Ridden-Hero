using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Rigidbody rb;
    SphereCollider sc;
    [SerializeField] public string wName, description; 
    [SerializeField] public int damage;
    [SerializeField] public float speed, cooldown, lifetime;
    [SerializeField] public bool isDestroyOnhit, hasCollision;
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
        if (hasCollision)
        {
            sc.isTrigger = false;
        } else {
            sc.isTrigger = true;
        }

        if (attackType == AttackType.Arrow)
        {
            rb.AddForce(transform.forward * speed);
        }
        player.rangedTime = cooldown;
        KillSelf(lifetime);
    }

    void KillSelf(float time)
    {
        Destroy(gameObject, time);
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Boss")
        {
            BossController boss = other.GetComponent<BossController>();
            boss.GetHit(damage);
            if (isDestroyOnhit)
            {
                Destroy(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
