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

    Vector3 displacement, lookAt;
    Rigidbody rb;
    Animator anim;
    public float MaxHp = 20, Hp = 20, debt = 0;
    [SerializeField] public float moveSpeed = 500f, timescaleMulti = 0.5f, dodgeCooldown = 1f, 
                        dodgeDistance = 10f, chargeTime = 0.5f, maxCharge = 5f, minCharge = 0.75f;
    [SerializeField] GameObject projectile;
    GameObject bossGO;
    public float rangedTime, dodgeTime;
    public bool hasEaten, wasHealed;
    bool playerRanged, playerDodge, controlEnabled;
    UIPlayerChargeBar chargeBar;
    UIPlayerHealthBar healthBar;
    public PlayerState playerState = PlayerState.Idle;
    BossSceneManager bossScene;

    void Awake() 
    {
        chargeBar = FindObjectOfType<UIPlayerChargeBar>();
        healthBar = FindObjectOfType<UIPlayerHealthBar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        bossGO = GameObject.FindWithTag("Boss");
        bossScene = GameObject.Find("BossSceneManager").GetComponent<BossSceneManager>();
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
        
        Hp = GameManager.instance.playerHp;
        if(!GameManager.instance.playerEat)
        {
            minCharge = 0.1f;
            maxCharge = 0.75f;
        }

        chargeBar.SetChargeBarValues(maxCharge, minCharge);
        healthBar.SetPlayerMaxHealth(MaxHp);
        healthBar.SetPlayerCurrentHealth(Hp);
    }

    void RangedAttack()
    {
        anim.Play("ChargeBow");
        chargeBar.SetCurrentBarValue(chargeTime);
        if (rb.velocity != Vector3.zero)
            rb.velocity = Vector3.zero;
        if (lookAt == Vector3.zero)
            lookAt = new Vector3(0f, 0f, 0.00001f);
        chargeTime += Time.deltaTime;
        anim.SetFloat("ChargeTime", Mathf.Lerp(0, maxCharge, chargeTime)/maxCharge);
        playerState = PlayerState.Ranged;
        Time.timeScale = timescaleMulti;
        FaceStickDirection();
    }

    void Dodge()
    {
        anim.Play("PlayerDodge");
        playerState = PlayerState.Dodge;
        transform.position += transform.forward * dodgeDistance;
        dodgeTime = dodgeCooldown;
    }

    void ShootProjectile()
    {
        Debug.Log("Projectile shot");
        anim.Play("PlayerIdle");
        debt += GameManager.instance.arrowCost;
        if (chargeTime > maxCharge)
            chargeTime = maxCharge;
        var newProjectile = Instantiate(
            projectile, transform.position + transform.forward, transform.rotation
            );
        Time.timeScale = 1f;
        chargeBar.SetChargeBarInactive();
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
        if(playerState == PlayerState.Ranged)
        {
            chargeTime = minCharge;
            Time.timeScale = 1f;
            rangedTime = projectile.gameObject.GetComponent<ProjectileController>().cooldown;
            chargeBar.SetChargeBarInactive();
        }

        Hp -= damageTaken;
        healthBar.SetPlayerCurrentHealth(Hp);
        if(Hp <= 0)
        {
            anim.Play("PlayerDead");
            playerState = PlayerState.Dead;
            GameObject.FindAnyObjectByType<BossSceneManager>().BossEnd(false);
            return;
        }
        else
        {
            playerState = PlayerState.Hurt;
            anim.Play("PlayerHurt");
        }
        

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
        if(playerState != PlayerState.Dead)
            playerState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerState);
        controlEnabled = (playerState != PlayerState.Hurt && playerState != PlayerState.Dead && !bossScene.Paused);

        playerRanged = Input.GetButton("Ranged") && controlEnabled;
        playerDodge = Input.GetButtonDown("Dodge") && controlEnabled;

        if (playerRanged && rangedTime <= 0)
        {
            chargeBar.SetChargeBarActive();
            RangedAttack();
        }
        if (Input.GetButtonUp("Ranged") && controlEnabled && rangedTime <= 0)
        {
            Debug.Log("Button up");
            ShootProjectile();
            playerState = PlayerState.Idle;
        }

        if (playerDodge && dodgeTime <= 0)
        {
            Dodge();
            playerState = PlayerState.Idle;
        }

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
                anim.SetBool("Moving", true);
            } else {
                playerState = PlayerState.Idle;
                anim.SetBool("Moving", false);
            }
        }
    }
}
