using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject Slam, Projectile;
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void PlayerTargetedSlamGround(float delay)
    {
        SlamGround(player.transform.position, delay);
    }

    void SlamGround(Vector3 target, float delay = 0f)
    {
        if(target == Vector3.zero)
            target = transform.position+Vector3.forward*2;
        // make sure the gameobject has BossAttackBehaviour script
        GameObject newSlam = Instantiate(Slam, target, transform.rotation);
        BossAttackBehaviour behaviour = newSlam.GetComponent<BossAttackBehaviour>();
        if(behaviour == null)
        {
            Debug.LogWarning("Add BossAttackBehaviour script, thanks");
            Destroy(newSlam);
        }
        else
        {
            behaviour.hitDelay = delay;
        }
    }

    void ShootProjectile(int type)
    {
        GameObject newProjectile = Instantiate(Projectile, transform.position + transform.forward * 2, transform.rotation);
        if(newProjectile.GetComponent<BossAttackBehaviour>() == null)
        {
            Debug.LogWarning("Add BossAttackBehaviour script, thanks");
            Destroy(newProjectile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time);
    }
}
