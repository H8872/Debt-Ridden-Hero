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
        Hurt
    }

    Vector3 displacement, desiredVelocity;
    Rigidbody rb;
    [SerializeField] float moveSpeed = 500f, meleeRange = 1f;
    [SerializeField] GameObject melee;
    bool playerMelee;
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
        Destroy(newMelee, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        playerMelee = Input.GetButtonDown("Melee");

        if (playerMelee)
        {
            if (playerState == PlayerState.Idle || playerState == PlayerState.Move)
            {
                MeleeAttack();
            }
        }
    }

    void FixedUpdate()
    {
        if (playerState == PlayerState.Idle || playerState == PlayerState.Move)
        {
            displacement = new Vector3(
                Input.GetAxisRaw("Horizontal"), transform.position.y, Input.GetAxisRaw("Vertical")
                );
            displacement = Vector3.ClampMagnitude(displacement, 1);
            rb.velocity = displacement * Time.deltaTime * moveSpeed;

            if (rb.velocity.magnitude > 0)
            {
                transform.forward = new Vector3(displacement.x, 0, displacement.z);
                playerState = PlayerState.Move;
            } else {
                playerState = PlayerState.Idle;
            }



        }
    }
}
