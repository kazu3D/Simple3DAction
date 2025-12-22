using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;     //Y•ûŒü‚Ì‰e‹¿‚ðÁ‚·

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * h + camForward * v;

        if(move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed*Time.deltaTime
            );
        }
        controller.Move(move * moveSpeed * Time.deltaTime);

        if(controller.isGrounded)
        {
            groundTimer = groundGraceTime;
        }
        else
        {
            groundTimer -= Time.deltaTime;
            groundTimer = Mathf.Max(groundTimer, 0f);
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        if (Input.GetButtonDown("Jump") && groundTimer > 0f)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            groundTimer = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

    }
    void Attack()
    {
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