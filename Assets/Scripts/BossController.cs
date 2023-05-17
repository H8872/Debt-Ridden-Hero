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
    List<Transform> shootPointsList = new List<Transform>();
    PlayerController pControl;
    public float Hp;
    [SerializeField] float turningSpeed = 1, shootPointsAmount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        eye = transform.Find("Eye");

        RegenerateShootPoints();
    }

    void RegenerateShootPoints()
    {
        if(shootPointsList.Count > 0)
        {
            foreach (Transform point in shootPointsList)
            {
                shootPointsList.Remove(point);
                Destroy(point.gameObject);
            }
        }
        float pointAngle = 360f/shootPointsAmount;
        for (int i = 0; i < shootPointsAmount; i++)
        {
            eye.Rotate(0,pointAngle,0);
            Transform newShootPoint = Instantiate(eye.gameObject, eye.position + eye.forward * 2, eye.rotation).transform;
            newShootPoint.SetParent(transform);
            newShootPoint.position = new Vector3(newShootPoint.position.x, 1, newShootPoint.position.z);
            newShootPoint.name = "Shootpoint" + i;
            shootPointsList.Add(newShootPoint);
        }
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
        GameObject newProjectile;
        switch(type)
        {
            case 1:
                foreach(Transform point in shootPointsList)
                {
                    newProjectile = Instantiate(Projectile, point.position, point.rotation);
                    if(newProjectile.GetComponent<BossAttackBehaviour>() == null)
                    {
                        Debug.LogWarning("Add BossAttackBehaviour script, thanks");
                        Destroy(newProjectile);
                    }
                }
                break;
            default:
                newProjectile = Instantiate(Projectile, transform.position + transform.forward * 2, transform.rotation);
                if(newProjectile.GetComponent<BossAttackBehaviour>() == null)
                {
                    Debug.LogWarning("Add BossAttackBehaviour script, thanks");
                    Destroy(newProjectile);
                }
                break;
        }
        
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
