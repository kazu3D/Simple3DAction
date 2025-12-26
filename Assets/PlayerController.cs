using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioSource jumpSE;
    [SerializeField] AudioSource damageSE;
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.8f;
    public float rotateSpeed = 10f;
    public float maxHP = 100;
    public float currentHP;

    public HPBarController hpBar;

    public float invincibleTime = 1.0f;
    private bool isInvincible;

    private float groundTimer;
    public float groundGraceTime = 0.15f;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;
    public float attackRange = 1.2f;

    public float fallLimitY = -10f;



    private CharacterController controller;
    private Vector3 velocity;

    private Transform cameraTransform;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        currentHP = maxHP;
        hpBar.UpdateHP(currentHP, maxHP);
    }

    bool isGameOver = false;

    void Update()
    {
        // ‡@ GameOver’†‚Í‰½‚à‚µ‚È‚¢
        if (isGameOver)
            return;

        // ‡A “ü—Íæ“¾
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * h + camForward * v;

        // ‡B ‰ñ“]
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        // ‡C ‰¡ˆÚ“®
        controller.Move(move * moveSpeed * Time.deltaTime);

        // ‡D Ú’n—P—\
        if (controller.isGrounded)
        {
            groundTimer = groundGraceTime;
        }
        else
        {
            groundTimer -= Time.deltaTime;
            groundTimer = Mathf.Max(groundTimer, 0f);
        }

        // ‡E Ú’n‚Ì—‰ºƒŠƒZƒbƒg
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        // ‡F ƒWƒƒƒ“ƒv
        if (Input.GetButtonDown("Jump") && groundTimer > 0f)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            groundTimer = 0f;
            jumpSE.Play();
        }

        // ‡G d—Í
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ‡H UŒ‚
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        // ‡I š—‰º”»’è‚Í•K‚¸ÅŒãš
        if (transform.position.y < fallLimitY)
        {
            isGameOver = true;
            velocity = Vector3.zero;
            controller.enabled = false;
            currentHP = 0;
            HPBarController.instance.UpdateHP(currentHP,maxHP);
            GameOverUIController.instance.ShowGameOver();
        }
    }

    void Attack()
    {
        damageSE.Play();
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHPController>()?.TakeDamage(1);
        }

    }



    public void Damage(float damage)
    {
        if (isInvincible) return;
        {
            damageSE.Play();
            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);

            hpBar.UpdateHP(currentHP, maxHP);
        }
        HPBarController.instance.UpdateHP(currentHP, maxHP);

        StartCoroutine(InvincibleTime());

        if(currentHP <= 0)
        {
            currentHP = 0;
            GameOverUIController.instance.ShowGameOver();
        }
    }

    IEnumerator InvincibleTime()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1.0f);
        isInvincible = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Enemy"))
        {
            Damage(1);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}