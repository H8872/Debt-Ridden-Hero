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

    Vector3 displacement, desiredVelocity;
    Rigidbody rb;
    [SerializeField] float moveSpeed = 500f, meleeRange = 1f, meleeLifetime, timescaleMulti = 0.5f,
                    projectileLifetime = 1f, dodgeCooldown = 1f, projectileCooldown = 1f, meleeCooldown = 0.5f, 
                    meleeTime, rangedTime, dodgeTime, dodgeDistance = 10f;
    [SerializeField] GameObject melee, projectile;
    bool playerMelee, playerRanged, playerDodge;
    public PlayerState playerState = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        playerState = PlayerState.Ranged;
        Time.timeScale = timescaleMulti;
        displacement = new Vector3(
                Input.GetAxisRaw("Horizontal"), transform.position.y, Input.GetAxisRaw("Vertical")
                );
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
        transform.forward = new Vector3(displacement.x, 0, displacement.z);
    }

    // Update is called once per frame
    void Update()
    {
        playerMelee = Input.GetButtonDown("Melee");
        playerRanged = Input.GetButton("Ranged");
        playerDodge = Input.GetButtonDown("Dodge");

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
        if (Input.GetButtonUp("Ranged") && rangedTime <= 0)
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
        if (playerState == PlayerState.Idle || playerState == PlayerState.Move)
        {
            displacement = new Vector3(
                Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")
                );
            displacement = Vector3.ClampMagnitude(displacement, 1);
            rb.velocity = displacement * Time.deltaTime * moveSpeed;

            if (rb.velocity.magnitude > 0)
            {
                FaceStickDirection();
                playerState = PlayerState.Move;
            } else {
                playerState = PlayerState.Idle;
            }
        }
    }
}
