using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState {
        Idle,
        Attacking,
        Tracking,
        Hurt
    }
    public BossState bossState;
    Animator anim;
    Transform eye;
    [SerializeField] GameObject Slam, Projectile;
    GameObject player;
    PlayerController pControl;
    public float Hp;
    [SerializeField] float turningSpeed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        eye = transform.GetChild(1);
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

    public void GetHit(int damage)
    {
        Hp -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        switch(bossState)
        {
            case BossState.Idle:
                if(pControl.playerState != PlayerController.PlayerState.Dead)
                {
                    bossState = BossState.Tracking;
                }
                break;
            case BossState.Tracking:
                eye.LookAt(player.transform);
                Vector3 lookTo = Vector3.Scale(eye.rotation.eulerAngles, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(lookTo), Time.deltaTime * turningSpeed);
                break;
            default:
                break;
        }
        if(pControl.playerState == PlayerController.PlayerState.Dead)
        {
            bossState = BossState.Idle;
        }
        Debug.Log(Time.time);
    }
}
