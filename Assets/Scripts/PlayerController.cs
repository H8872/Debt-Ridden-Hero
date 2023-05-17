using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Dodge,
        Ranged,
        Hurt,
        Dead
    }

    Vector3 displacement, desiredVelocity, lookAt;
    Rigidbody rb;
    public float Hp = 10;
    [SerializeField] float moveSpeed = 500f, meleeRange = 1f, meleeLifetime = 0.5f, timescaleMulti = 0.5f,
                    projectileLifetime = 1f, dodgeCooldown = 1f, projectileCooldown = 1f, meleeCooldown = 0.5f, 
                    meleeTime, rangedTime, dodgeTime, dodgeDistance = 10f;
    [SerializeField] GameObject melee, projectile;
    GameObject bossGO;
    bool playerMelee, playerRanged, playerDodge, controlEnabled;
    public PlayerState playerState = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bossGO = GameObject.FindWithTag("Boss");
    }

    void MeleeAttack()
    {
        var newMelee = Instantiate(
            melee, transform.position + transform.forward * meleeRange, transform.rotation
            );
        meleeTime = meleeCooldown;
        Destroy(newMelee, meleeLifetime);
    }

    void RangedAttack()
    {
        if (rb.velocity != Vector3.zero)
            rb.velocity = Vector3.zero;
        if (lookAt == Vector3.zero)
            lookAt = new Vector3(0f, 0f, 0.00001f);
        playerState = PlayerState.Ranged;
        Time.timeScale = timescaleMulti;
        FaceStickDirection();
    }

    void Dodge()
    {
        playerState = PlayerState.Dodge;
        transform.position += transform.forward * dodgeDistance;
        dodgeTime = dodgeCooldown;
    }

    void ShootProjectile()
    {
        Debug.Log("Projectile shot");
        var newProjectile = Instantiate(
            projectile, transform.position + transform.forward, transform.rotation
            );
        Destroy(newProjectile, projectileLifetime);
    }

    void FaceStickDirection()
    {
        if (displacement != Vector3.zero)
        {
            lookAt = displacement;
        }
        transform.forward = new Vector3(lookAt.x, 0, lookAt.z);
    }

    public void GetHit(Vector3 direction, float damageTaken, float knockBackMultiplier)
    {
        Hp -= damageTaken;
        if(Hp <= 0)
        {
            playerState = PlayerState.Dead;
            return;
        }
        else
            playerState = PlayerState.Hurt;
        
        Vector3 kbDirection = (transform.position-direction).normalized;
        if(kbDirection == Vector3.zero)
            kbDirection = (transform.position-bossGO.transform.position).normalized;
        
        displacement = -kbDirection;
        FaceStickDirection();
        rb.AddForce(kbDirection*knockBackMultiplier);
        Invoke("CheckState", 0.5f);
    }

    void CheckState()
    {
        playerState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        controlEnabled = (playerState != PlayerState.Hurt && playerState != PlayerState.Dead);

        playerMelee = Input.GetButtonDown("Melee") && controlEnabled;
        playerRanged = Input.GetButton("Ranged") && controlEnabled;
        playerDodge = Input.GetButtonDown("Dodge") && controlEnabled;

        if (playerMelee && meleeTime <= 0)
        {
            if (playerState == PlayerState.Idle || playerState == PlayerState.Move)
            {
                MeleeAttack();
            }
        }

        if (playerRanged && rangedTime <= 0)
        {
            RangedAttack();
        }
        if (Input.GetButtonUp("Ranged") && controlEnabled && rangedTime <= 0)
        {
            Debug.Log("Button up");
            ShootProjectile();
            Time.timeScale = 1f;
            rangedTime = projectileCooldown;
            playerState = PlayerState.Idle;
        }

        if (playerDodge && dodgeTime <= 0)
        {
            Dodge();
            playerState = PlayerState.Idle;
        }

        meleeTime -= Time.deltaTime;
        rangedTime -= Time.deltaTime;
        dodgeTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        displacement = new Vector3(
                                Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")
                                );
        displacement = Vector3.ClampMagnitude(displacement, 1);

        if (playerState == PlayerState.Idle || playerState == PlayerState.Move)
        {

            rb.velocity = displacement * Time.deltaTime * moveSpeed;

            if (rb.velocity.sqrMagnitude > 0)
            {
                FaceStickDirection();
                playerState = PlayerState.Move;
            } else {
                playerState = PlayerState.Idle;
            }
        }
    }
}
