using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState {
        Idle,
        Attacking,
        Tracking,
        Hurt,
        Dead
    }
    public BossState bossState;
    Animator anim;
    Transform eye;
    [SerializeField] GameObject Slam, Projectile;
    GameObject player;
    List<Transform> shootPointsList = new List<Transform>();
    [field: SerializeField] public BossAttackDefinition[] AttackSequence;
    public float Hp, actDelay;
    int currentAct = 0, attackInRow;
    float attackHitDelay = 0f;

    PlayerController pControl;
    [SerializeField] float turningSpeed = 1, shootPointsAmount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        eye = transform.Find("Eye");

        RegenerateShootPoints();
        ActNextOnSequence();
    }

    void RegenerateShootPoints()
    {
        if(shootPointsList.Count > 0)
        {
            for (int i = shootPointsList.Count-1; i >= 0; i--)
            {
                GameObject p = shootPointsList[i].gameObject;
                shootPointsList.RemoveAt(i);
                Destroy(p);
            }
        }
        eye.rotation = transform.rotation;
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

    /* Types:
    1: 
    default/0: Player centered Slam
    */
    void SlamGround(int type = 0)
    {
        Vector3 target;
        GameObject newSlam;
        BossAttackBehaviour behaviour;
        switch(type)
        {
            case 1:
                break;
            default:
                target = player.transform.position;
                // make sure the gameobject has BossAttackBehaviour script
                newSlam = Instantiate(Slam, target, transform.rotation);
                behaviour = newSlam.GetComponent<BossAttackBehaviour>();
                behaviour.hitDelay = attackHitDelay;
                break;
        }
        
    }

    /* Types:
    1: Simultaneous Projectile from each of the shoot points
    2: Shoot multiple in Projectiles in a row from different shoot points
    default/0: Single Projectile from first shoot point
    */
    void ShootProjectile(int type = 0)
    {
        GameObject newProjectile;
        switch(type)
        {
            case 1:
                foreach(Transform point in shootPointsList)
                {
                    newProjectile = Instantiate(Projectile, point.position, point.rotation);
                }
                break;
            case 2:
                attackInRow--;
                newProjectile = Instantiate(Projectile, shootPointsList[attackInRow].position, shootPointsList[attackInRow].rotation);
                break;
            default:
                newProjectile = Instantiate(Projectile, shootPointsList[0].position, shootPointsList[0].rotation);
                break;
        }
        
    }

    public void GetHit(int damage)
    {
        Hp -= damage;
        Debug.Log($"Ouch! My HP is {Hp}!");
        if(Hp <= 0)
            bossState = BossState.Dead;
    }

    void SetState(int state)
    {
        switch(state)
        {
            case 0: 
                bossState = BossState.Idle;
                break;
            case 1: 
                bossState = BossState.Attacking;
                break;
            case 2: 
                bossState = BossState.Tracking;
                break;
            case 3: 
                bossState = BossState.Hurt;
                break;
            case 4: 
                bossState = BossState.Dead;
                break;
        }
    }

    void ActNextOnSequence()
    {
        bossState = BossState.Idle;
        if(AttackSequence.Length == currentAct)
            currentAct = 0;
        BossAttackDefinition actDefinition = AttackSequence[currentAct];
        actDelay = actDefinition.delay;

        switch(AttackSequence[currentAct].attackName)
        {
            case BossAttackDefinition.AttackName.Wait:
                anim.Play("BossIdle");
                Invoke("ActNextOnSequence", actDelay);
                actDelay = 0;
                break;
            case BossAttackDefinition.AttackName.PlayerCenteredSlam:
                attackHitDelay = 1f;
                anim.Play("BossPlayerCenteredSlam");
                break;
            case BossAttackDefinition.AttackName.SimultaneousProjectile:
                shootPointsAmount = actDefinition.amount;
                RegenerateShootPoints();
                anim.Play("BossSimultaneousProjectile");
                break;
            case BossAttackDefinition.AttackName.SequenceProjectile:
                actDefinition.amount = 3;
                shootPointsAmount = actDefinition.amount;
                RegenerateShootPoints();
                attackInRow = (int)actDefinition.amount;
                anim.Play("BossSequenceProjectile");
                break;
            case BossAttackDefinition.AttackName.SimultanoeusSlamShower:
                break;
            case BossAttackDefinition.AttackName.SequenceSlamShower:
                break;
            default:
                break;
        }
        currentAct++;
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
            case BossState.Dead:
                CancelInvoke("ActNextOnSequence");
                break;
            default:
                break;
        }


        if(pControl.playerState == PlayerController.PlayerState.Dead)
        {
            if(bossState != BossState.Idle)
                anim.Play("BossIdle");
            bossState = BossState.Idle;
        }
        // Debug.Log(Time.time);
    }
}
